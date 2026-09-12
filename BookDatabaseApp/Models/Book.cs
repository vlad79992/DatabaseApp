namespace BookDatabaseApp.Models;

public record Book
(
    int Id,
    string Authors,
    string Title,
    string Publisher,
    int PublishYear,
    string Annotation
)
{
    public Book() : this(0, "", "", "", 0, "") { }
}