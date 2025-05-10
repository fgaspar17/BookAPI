using BookAPI.DTOs.RequestDTOs;
using FluentValidation;

namespace BookAPI.Validators;

public class BookPutRequestDtoValidator : AbstractValidator<BookPutRequestDto>
{
    public BookPutRequestDtoValidator()
    {
        RuleFor(b => b.BookId)
            .NotEmpty()
            .WithMessage("You must provide a BookId.");

        RuleFor(b => b.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("The Title cannot be empty.")
            .MaximumLength(100)
            .WithMessage("The Title length cannot be higher than 100 characters."); ;

        RuleFor(b => b.Pages)
            .GreaterThan(0)
            .WithMessage("Pages must have a value greater than 0.");

        RuleFor(b => b.PublicationDate)
            .NotNull()
            .LessThan(DateTime.Now)
            .WithMessage("The PublicationDate must be lower than today.");
    }
}
