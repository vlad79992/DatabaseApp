namespace BookDatabaseApp.Models;

public class Book
{
    public int Id { get; set; } = 0;
    public string Authors { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public int PublishYear { get; set; } = 0;
    public string Annotation { get; set; } = string.Empty;
}