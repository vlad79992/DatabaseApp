using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using BookDatabaseApp.Models;
using BookDatabaseApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookDatabaseApp.ViewModels.CRUD;

internal partial class CreateViewModel(IServiceProvider serviceProvider, IDbContextFactory<BookContext> dbFactory, INavigationService navigationService) : ViewModelBase, INamed
{
    private readonly IServiceProvider serviceProvider = serviceProvider;
    private readonly IDbContextFactory<BookContext> bookContextFactory = dbFactory;
    private readonly INavigationService navigationService = navigationService;
    
    public string PageName => this.GetType().FullName!;

    public Book Book { get; private set; } = new();
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(AddToDbCommand))]
    [Required(ErrorMessage = "Введите авторов")]
    public partial string? Authors { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(AddToDbCommand))]
    [Required(ErrorMessage = "Введите название")]
    public partial string? Title { get; set; }
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(AddToDbCommand))]
    [Required(ErrorMessage = "Введите издательство")]
    public partial string? Publisher { get; set; }
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(AddToDbCommand))]
    [Required(ErrorMessage = "Введите год выпуска")]
    [RegularExpression(@"^-?\d{1,4}$", ErrorMessage = "Введите корректный год")]
    public partial string? PublishYear { get; set; }
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(AddToDbCommand))]
    [Required(ErrorMessage = "Введите аннотацию")]
    public partial string? Annotation { get; set; }

    [ObservableProperty]
    public partial bool IsAdded { get; set; }
    
    [ObservableProperty]
    public partial string? AddedBookId { get; private set; }

    private bool CanAddToDb() => !IsAdded && !string.IsNullOrWhiteSpace(Authors) && 
                                 !string.IsNullOrWhiteSpace(Title) && 
                                 !string.IsNullOrWhiteSpace(Publisher) && 
                                 !string.IsNullOrWhiteSpace(PublishYear) &&
                                 !string.IsNullOrWhiteSpace(Annotation);
    
    [RelayCommand(CanExecute = nameof(CanAddToDb))]
    private async Task AddToDbAsync(CancellationToken cancellationToken = default)
    {
        var book = new Book
        {
            Authors = Authors!,
            Title = Title!,
            Publisher = Publisher!,
            PublishYear = int.Parse(PublishYear!),
            Annotation = Annotation!
        };
        
        await using var db = await bookContextFactory.CreateDbContextAsync(cancellationToken);
        
        var added = await db.Set<Book>().AddAsync(book, cancellationToken);
        
        await db.SaveChangesAsync(cancellationToken);
        
        IsAdded = added.Entity.Id != 0;

        if (!IsAdded) return;
        
        AddedBookId = added.Entity.Id.ToString();
    }

    [RelayCommand]
    private void Edit()
    {
        if (int.TryParse(AddedBookId, out var bookId))
        {
            var editViewModel = ActivatorUtilities.CreateInstance<EditViewModel>(serviceProvider, bookId);
            navigationService.NavigateTo(editViewModel);
        }
    }

    [RelayCommand]
    private void Delete()
    {
        if (int.TryParse(AddedBookId, out var bookId))
        {
            var deleteViewModel = ActivatorUtilities.CreateInstance<DeleteViewModel>(serviceProvider, bookId);
            navigationService.NavigateTo(deleteViewModel);
        }
    }

    [RelayCommand]
    private void AddAnother()
    {
        Authors = null;
        Title = null;
        Publisher = null;
        PublishYear = null;
        Annotation = null;
        IsAdded = false;
        AddedBookId = null;
    }
}
