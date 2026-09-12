using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Avalonia.Logging;
using BookDatabaseApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookDatabaseApp.ViewModels.CRUD;

internal partial class CreateViewModel : ViewModelBase, INamed
{
    public string PageName => this.GetType().FullName!;

    public Book Book { get; private set; } = new();
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Введите авторов")]
    public partial string? Authors { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Введите название")]
    public partial string? Title { get; set; }
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Введите издательство")]
    public partial string? Publisher { get; set; }
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Введите год выпуска")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Год должен состоять только из цифр")]
    public partial string? PublishYear { get; set; }
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Введите аннотацию")]
    public partial string? Annotation { get; set; }


    private bool CanAddToDb() => false;
    
    [RelayCommand(CanExecute = nameof(CanAddToDb))]
    public async Task AddToDbAsync()
    {
        
    }
}
