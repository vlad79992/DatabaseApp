using System;
using BookDatabaseApp.ViewModels;

namespace BookDatabaseApp.Services;

public interface INavigationService
{
    void NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
    void NavigateTo(Type viewModelType);
    void GoBack();
    void GoForward();
    bool CanGoBack { get; }
    bool CanGoForward { get; }
}
