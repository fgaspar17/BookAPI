using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookAPI.DTOs.RequestDTOs;
using BookAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IConfiguration _configuration;

        private readonly UserManager<User> _userManager;

        public AccountController(
            AppDbContext context,
            IConfiguration configuration,
            UserManager<User> userManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newUser = new User();
                    newUser.UserName = input.UserName;
                    newUser.Email = input.Email;
                    var result = await _userManager.CreateAsync(
                    newUser, input.Password);
                    if (result.Succeeded)
                    {
                        return StatusCode(201,
                        $"User '{newUser.UserName}' has been created.");
                    }
                    else
                        throw new Exception(
                        string.Format("Error: {0}", string.Join(" ",
                        result.Errors.Select(e => e.Description))));
                }
                else
                {
                    var details = new ValidationProblemDetails(ModelState);
                    details.Type =
                    "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    details.Status = StatusCodes.Status400BadRequest;
                    return new BadRequestObjectResult(details);
                }
            }
            catch (Exception e)
            {
                var exceptionDetails = new ProblemDetails();
                exceptionDetails.Detail = e.Message;
                exceptionDetails.Status =
                StatusCodes.Status500InternalServerError;
                exceptionDetails.Type =
                "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                return StatusCode(
                StatusCodes.Status500InternalServerError,
                exceptionDetails);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(input.UserName);
                    if (user == null || !await _userManager.CheckPasswordAsync(user, input.Password))
                        throw new Exception("Invalid login attempt.");

                    var roles = await _userManager.GetRolesAsync(user);
                    var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));

                    var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(
                        _configuration["JWT:SigningKey"])), SecurityAlgorithms.HmacSha256);

                    List<Claim> claims = [ new Claim(ClaimTypes.Name, user.UserName)];
                    claims.AddRange(roleClaims);

                    var jwtToken = new JwtSecurityToken(
                            issuer: _configuration["JWT:Issuer"],
                            audience: _configuration["JWT:Audience"],
                            claims: claims,
                            expires: DateTime.Now.AddSeconds(300),
                            signingCredentials: signingCredentials);

                    var jwtString = new JwtSecurityTokenHandler()
                        .WriteToken(jwtToken);

                    return Ok(new { Token = jwtString });
                }

                else
                {
                    var details = new ValidationProblemDetails(ModelState);
                    details.Type =
                    "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    details.Status = StatusCodes.Status400BadRequest;
                    return new BadRequestObjectResult(details);
                }
            }
            catch (Exception e)
            {
                var exceptionDetails = new ProblemDetails();
                exceptionDetails.Detail = e.Message;
                exceptionDetails.Status =
                StatusCodes.Status401Unauthorized;
                exceptionDetails.Type =
                "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                return StatusCode(
                StatusCodes.Status401Unauthorized,
                exceptionDetails);
            }
        }
    }
}
