using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookDatabaseApp.Models;
using BookDatabaseApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookDatabaseApp.ViewModels.CRUD;

internal partial class EditViewModel : ViewModelBase, INamed
{
    private readonly IDbContextFactory<BookContext> bookContextFactory;
    private readonly INavigationService navigationService;
    private readonly IServiceProvider serviceProvider;
    private int bookId;
    
    public string PageName => this.GetType().FullName!;

    public EditViewModel(int bookId, IDbContextFactory<BookContext> dbFactory, INavigationService navigationService, IServiceProvider serviceProvider)
    {
        bookContextFactory = dbFactory;
        this.navigationService = navigationService;
        this.bookId = bookId;
        this.serviceProvider = serviceProvider;
        
        using var db = bookContextFactory.CreateDbContext();
        
        var book = db.Set<Book>().FirstOrDefault(b => b.Id == bookId);
        
        if (book != null)
        {
            Authors = book.Authors;
            Title = book.Title;
            Publisher = book.Publisher;
            PublishYear = book.PublishYear.ToString();
            Annotation = book.Annotation;
        }
    }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    [Required(ErrorMessage = "Введите авторов")]
    public partial string? Authors { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    [Required(ErrorMessage = "Введите название")]
    public partial string? Title { get; set; }
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    [Required(ErrorMessage = "Введите издательство")]
    public partial string? Publisher { get; set; }
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    [Required(ErrorMessage = "Введите год выпуска")]
    [RegularExpression(@"^-?\d{1,4}$", ErrorMessage = "Введите корректный год")]
    public partial string? PublishYear { get; set; }
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    [Required(ErrorMessage = "Введите аннотацию")]
    public partial string? Annotation { get; set; }

    [ObservableProperty]
    public partial bool isSaved { get; set; }

    private bool CanSave() => !isSaved && !string.IsNullOrWhiteSpace(Authors) && 
                              !string.IsNullOrWhiteSpace(Title) && 
                              !string.IsNullOrWhiteSpace(Publisher) && 
                              !string.IsNullOrWhiteSpace(PublishYear) &&
                              !string.IsNullOrWhiteSpace(Annotation);

    [RelayCommand(CanExecute = nameof(CanSave))]
    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await bookContextFactory.CreateDbContextAsync(cancellationToken);
        
        var book = await db.Set<Book>().FirstOrDefaultAsync(b => b.Id == bookId, cancellationToken);
        
        if (book != null)
        {
            book.Authors = Authors!;
            book.Title = Title!;
            book.Publisher = Publisher!;
            book.PublishYear = int.Parse(PublishYear!);
            book.Annotation = Annotation!;
            
            await db.SaveChangesAsync(cancellationToken);
            
            isSaved = true;
        }
    }

    [RelayCommand]
    private void Edit()
    {
        var editViewModel = ActivatorUtilities.CreateInstance<EditViewModel>(serviceProvider, bookId);
        navigationService.NavigateTo(editViewModel);
    }

    [RelayCommand]
    private void ReadAll()
    {
        var readViewModel = ActivatorUtilities.CreateInstance<ReadViewModel>(serviceProvider);
        navigationService.NavigateTo(readViewModel);
    }
    
    [RelayCommand]
    private void Delete()
    {
        var deleteViewModel = ActivatorUtilities.CreateInstance<DeleteViewModel>(serviceProvider, bookId);
        navigationService.NavigateTo(deleteViewModel);
    }
}
