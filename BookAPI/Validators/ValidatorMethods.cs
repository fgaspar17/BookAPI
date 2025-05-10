namespace BookAPI.Validators;

public static class ValidatorMethods
{
    public static bool IsValidColumn(HashSet<string> validColumns, string columnName)
    {
        return string.IsNullOrWhiteSpace(columnName) || validColumns.Contains(columnName.ToLower());
    }
}
