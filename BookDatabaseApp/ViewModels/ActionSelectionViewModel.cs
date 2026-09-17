using System;
using BookDatabaseApp.Services;
using BookDatabaseApp.ViewModels.CRUD;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace BookDatabaseApp.ViewModels;

public partial class ActionSelectionViewModel(INavigationService navigation, IServiceProvider serviceProvider) : ViewModelBase, INamed
{
    private readonly INavigationService? navigationService = navigation;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    public string PageName => this.GetType().FullName!;

    [RelayCommand]
    public void Create()
    {
        var createViewModel = ActivatorUtilities.CreateInstance<CreateViewModel>(_serviceProvider);
        navigationService?.NavigateTo(createViewModel);
    }

    [RelayCommand]
    public void Read()
    {
        var readViewModel = ActivatorUtilities.CreateInstance<ReadViewModel>(_serviceProvider);
        navigationService?.NavigateTo(readViewModel);
    }
}