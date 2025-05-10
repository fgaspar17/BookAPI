using BookAPI.DTOs.RequestDTOs;
using FluentValidation;

namespace BookAPI.Validators;

public class AuthorBookPostRequestDtoValidator : AbstractValidator<AuthorBookPostRequestDto>
{
    public AuthorBookPostRequestDtoValidator()
    {
        RuleFor(ab => ab.AuthorId)
            .NotEmpty()
            .WithMessage("You must provide an AuthorId.");

        RuleFor(ab => ab.BookId)
            .NotEmpty()
            .WithMessage("You must provide a BookId.");
    }
}
