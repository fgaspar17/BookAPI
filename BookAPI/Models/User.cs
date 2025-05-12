using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BookAPI.Models;

public class User : IdentityUser
{
    public User() { }
}