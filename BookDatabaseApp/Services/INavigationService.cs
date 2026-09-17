using System;
using BookDatabaseApp.ViewModels;

namespace BookDatabaseApp.Services;

public interface INavigationService
{
    void NavigateTo(ViewModelBase viewModel);
    void GoBack();
    void GoForward();
    bool CanGoBack { get; }
    bool CanGoForward { get; }
}
