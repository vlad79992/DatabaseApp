using BookDatabaseApp.Services;
using CommunityToolkit.Mvvm.Input;

namespace BookDatabaseApp.ViewModels;

public partial class ActionSelectionViewModel : ViewModelBase, INamed
{
    private readonly INavigationService? navigationService;
    public string PageName => this.GetType().FullName!;

    [RelayCommand]
    public void Create()
    {
        navigationService?.NavigateTo<CRUD.CreateViewModel>();
    }

    [RelayCommand]
    public void Read()
    {
        navigationService?.NavigateTo<CRUD.ReadViewModel>();
    }

    [RelayCommand]
    public void Update()
    {
        
    }

    [RelayCommand]
    public void Delete()
    {
        
    }
    
    public ActionSelectionViewModel(INavigationService navigation)
    {
        navigationService = navigation;
    }
}