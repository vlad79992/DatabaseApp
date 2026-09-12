using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using BookDatabaseApp.Services;

namespace BookDatabaseApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly NavigationService navigationService;
    public ViewModelBase CurrentPage => navigationService.CurrentPage!;
    public MainWindowViewModel(NavigationService navigationService)
    {
        this.navigationService = navigationService;

        navigationService.PropertyChanged += (s, e) =>
        {
            switch (e.PropertyName)
            {
                // Когда меняется CurrentPage в сервисе
                case nameof(NavigationService.CurrentPage):
                    this.CurrentPageName = CurrentPage is INamed namedPage
                        ? namedPage.PageName
                        : CurrentPage.GetType().FullName;

                    OnPropertyChanged(nameof(CurrentPage));
                    OnPropertyChanged(nameof(CanGoHome));
                    GoHomeCommand.NotifyCanExecuteChanged();
                    break;
                // Когда меняется CanGoBack в сервисе
                case nameof(NavigationService.CanGoBack):
                    OnPropertyChanged(nameof(CanGoBack));
                    GoBackCommand.NotifyCanExecuteChanged(); // Уведомляем команду
                    break;
                // Когда меняется CanGoForward в сервисе
                case nameof(NavigationService.CanGoForward):
                    OnPropertyChanged(nameof(CanGoForward));
                    GoForwardCommand.NotifyCanExecuteChanged(); // Уведомляем команду
                    break;
            }
        };

        this.CurrentPageName = CurrentPage is INamed namedPage
            ? namedPage.PageName
            : CurrentPage.GetType().FullName;
    }

    /// <summary>
    /// Домашняя страница может быть изменена при разных сценариях, 
    /// например, при авторизации домашняя страница -- это выбор вида входа,
    /// а после -- список чатов
    /// </summary>
    public Type HomePage { get; set; } = typeof(ActionSelectionViewModel);

    [ObservableProperty]
    public partial string? CurrentPageName { get; set; }
    public bool CanGoBack => navigationService.CanGoBack;
    public bool CanGoForward => navigationService.CanGoForward;

    [RelayCommand(CanExecute = nameof(CanGoBack))]
    public void GoBack() => navigationService.GoBack();

    [RelayCommand(CanExecute = nameof(CanGoForward))]
    public void GoForward() => navigationService.GoForward();

    public bool CanGoHome => CurrentPage.GetType() != HomePage;

    [RelayCommand(CanExecute = nameof(CanGoHome))]
    public void GoHome()
    {
        navigationService.NavigateTo(HomePage);
    }
}