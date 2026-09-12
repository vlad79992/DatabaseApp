using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Collections;
using BookDatabaseApp.Models;
using BookDatabaseApp.Services;
using Microsoft.EntityFrameworkCore;

namespace BookDatabaseApp.ViewModels.CRUD;

internal partial class ReadViewModel(IDbContextFactory<BookContext> dbFactory) : ViewModelBase, INamed, IRefreshable
{
    private readonly IDbContextFactory<BookContext> bookContextFactory = dbFactory;
    
    public string PageName => this.GetType().FullName!;

    public AvaloniaList<Book> Books { get; } = new();
    
    public async void OnRefresh()
    {
        await LoadAsync();
    }

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await bookContextFactory.CreateDbContextAsync(cancellationToken);
        
        var books = await db.Set<Book>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        books.AddRange( Enumerable.Range(1, 150).Select(x =>
            new Book(
                Id: x,
                Authors: "Рэй Брэдбери",
                Title: "451° по Фаренгейту",
                Publisher: "Test",
                PublishYear: 2026,
                Annotation: """
                            Рэй Брэдбери — лауреат Пулитцеровской премии и классик научной фантастики. Его произведение «451° по Фаренгейту» стало ответом на политику маккартизма, включавшую в себя сожжение прокоммунистической литературы. Это роман-предупреждение. СМИ однажды могут внушить всем, что пожарные всегда разжигали пожары, а не тушили их.
                              
                            451° по Фаренгейту — температура, при которой воспламеняется и горит бумага. Главный герой уже 10 лет сжигает книги. Иногда вместе с их хозяевами. Его жена безвылазно сидит в гостиной, стены которой заменяют огромные экраны телевизоров.  
                            Счастливы ли они? Вопрос чудаковатой соседки, любящей цветы и ночные прогулки, озадачивает пожарного. В нем что-то щелкает. Пожарный выхватывает книгу из огня. А дальше… механический пес, погоня в прямом эфире и люди-книги.
                            """
            )));
        
        Books.Clear();
        Books.AddRange(books);
    }
}