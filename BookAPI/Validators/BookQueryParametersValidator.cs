using BookAPI.DTOs.RequestDTOs;
using FluentValidation;

namespace BookAPI.Validators;

public class BookQueryParametersValidator : AbstractValidator<GetAllQueryParameters>
{
    private HashSet<string> _sortColumns = new HashSet<string>() { "title", "description", "pages", "publicationdate" };
    public BookQueryParametersValidator()
    {
        RuleFor(q => q.PageIndex)
            .GreaterThanOrEqualTo(0)
            .WithMessage("PageIndex must be 0 or greater than 0.");

        RuleFor(q => q.PageSize)
            .LessThanOrEqualTo(100)
            .WithMessage("PageSize must be 100 or lower than 100.");

        RuleFor(q => q.SortDirection)
            .Matches("ASC|DESC", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            .WithMessage("SortColumn must be ASC or DESC")
            .When(q => !string.IsNullOrEmpty(q.SortDirection));

        RuleFor(q => q.SortColumn)
            .Must(sortColumn => ValidatorMethods.IsValidColumn(_sortColumns, sortColumn))
            .WithMessage($"You must provide a valid column. Valid options: {string.Join(", ", _sortColumns)}.")
            .When(q => !string.IsNullOrEmpty(q.SortColumn));
    }

   
}
