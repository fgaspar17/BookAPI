namespace BookAPI.DTOs.RequestDTOs;

public class GetAllQueryParameters
{
    public string? Filter { get; set; } = string.Empty;

    // Pagination
    public int PageIndex { get; set; } = 0;  // zero-based indexing
    public int PageSize { get; set; } = 10;

    // Sorting
    public string? SortColumn { get; set; }
    public string SortDirection { get; set; } = "ASC"; // ASC or DESC
}