using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Collections;
using BookDatabaseApp.Models;
using BookDatabaseApp.Services;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookDatabaseApp.ViewModels.CRUD;

internal partial class ReadViewModel(IServiceProvider serviceProvider, IDbContextFactory<BookContext> dbFactory, INavigationService navigationService) : ViewModelBase, INamed, IRefreshable
{
    private readonly IServiceProvider serviceProvider = serviceProvider;
    private readonly IDbContextFactory<BookContext> bookContextFactory = dbFactory;
    private readonly INavigationService navigationService = navigationService;
    
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
        
        Books.Clear();
        Books.AddRange(books);
    }

    [RelayCommand]
    private void Edit(Book book)
    {
        var vm = ActivatorUtilities.CreateInstance<EditViewModel>(serviceProvider, book.Id);
        navigationService.NavigateTo(vm);
    }
    
    [RelayCommand]
    private void Delete(Book book)
    {
        var deleteViewModel = ActivatorUtilities.CreateInstance<DeleteViewModel>(serviceProvider, book.Id);
        navigationService.NavigateTo(deleteViewModel);
    }
}
