using BookAPI.DTOs.RequestDTOs;
using FluentValidation;

namespace BookAPI.Validators;

public sealed class AuthorPostRequestDtoValidator : AbstractValidator<AuthorPostRequestDto>
{
    public AuthorPostRequestDtoValidator()
    {
        RuleFor(a => a.FirstName)
            .NotNull()
            .NotEmpty()
            .WithMessage("The FirstName cannot be empty.")
            .MaximumLength(100)
            .WithMessage("The FirstName length cannot be higher than 100 characters.");

        RuleFor(a => a.LastName)
            .MaximumLength(100)
            .WithMessage("The LastName length cannot be higher than 100 characters.");

        RuleFor(a => a.Birthday)
            .NotNull()
            .LessThan(DateTime.Now)
            .WithMessage("The Birthday must be lower than today.");
    }
}
