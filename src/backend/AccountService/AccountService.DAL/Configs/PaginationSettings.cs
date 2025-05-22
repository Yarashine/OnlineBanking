namespace AccountService.DAL.Configs;

public class PaginationSettings
{
    public const string SectionName = "Pagination";

    public int DefaultPageSize { get; set; } = 10;
}