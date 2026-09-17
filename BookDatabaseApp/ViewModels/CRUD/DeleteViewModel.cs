using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookDatabaseApp.Models;
using BookDatabaseApp.Services;
using BookDatabaseApp.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookDatabaseApp.ViewModels.CRUD;

internal partial class DeleteViewModel : ViewModelBase, INamed
{
    private readonly IDbContextFactory<BookContext> bookContextFactory;
    private readonly INavigationService navigationService;
    private readonly IServiceProvider serviceProvider;
    private readonly int bookId;
    
    public string PageName => this.GetType().FullName!;

    public DeleteViewModel(int bookId, IDbContextFactory<BookContext> dbFactory, INavigationService navigationService, IServiceProvider serviceProvider)
    {
        this.bookId = bookId;
        bookContextFactory = dbFactory;
        this.navigationService = navigationService;
        this.serviceProvider = serviceProvider;
        
        using var db = bookContextFactory.CreateDbContext();
        var book = db.Set<Book>().FirstOrDefault(b => b.Id == bookId);
        
        if (book != null)
        {
            BookInfo = $"{book.Authors} — «{book.Title}» ({book.PublishYear})";
        }
        else
        {
            BookInfo = "Книга не найдена";
        }
    }
    
    [ObservableProperty]
    public partial string? BookInfo { get; set; }

    [ObservableProperty]
    public partial bool IsDeleted { get; set; }

    [RelayCommand]
    private async Task ConfirmDeleteAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await bookContextFactory.CreateDbContextAsync(cancellationToken);
        
        var book = await db.Set<Book>().FirstOrDefaultAsync(b => b.Id == bookId, cancellationToken);
        
        if (book != null)
        {
            db.Set<Book>().Remove(book);
            await db.SaveChangesAsync(cancellationToken);
        }
        
        IsDeleted = true;
    }

    [RelayCommand]
    private void Cancel()
    {
        var createViewModel = ActivatorUtilities.CreateInstance<CreateViewModel>(serviceProvider);
        navigationService.NavigateTo(createViewModel);
    }

    [RelayCommand]
    private void ReadAll()
    {
        var readViewModel = ActivatorUtilities.CreateInstance<ReadViewModel>(serviceProvider);
        navigationService.NavigateTo(readViewModel);
    }
}
