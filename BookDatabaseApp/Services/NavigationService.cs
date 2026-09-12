using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using BookDatabaseApp.ViewModels;

namespace BookDatabaseApp.Services;

public class NavigationService(IServiceProvider serviceProvider) : ViewModelBase, INavigationService
{
    private readonly LinkedList<Type> pages = new();
    private LinkedListNode<Type>? currentPageNode;

    public ViewModelBase? CurrentPage
    {
        get;
        private set => SetProperty(ref field, value);
    }

    public void Initialize(Type startPage)
    {
        pages.AddLast(startPage);
        currentPageNode = pages.Last;
        UpdateCurrentPage();
    }

    public void NavigateTo(Type viewModelType)
    {
        while (currentPageNode?.Next is not null) pages.RemoveLast();

        currentPageNode = pages.AddLast(viewModelType);
        UpdateCurrentPage();
        OnPropertyChanged(nameof(CanGoBack));
        OnPropertyChanged(nameof(CanGoForward));
    }

    public void GoBack()
    {
        if (currentPageNode?.Previous is not null)
        {
            currentPageNode = currentPageNode.Previous;
            UpdateCurrentPage();
            OnPropertyChanged(nameof(CanGoBack));
            OnPropertyChanged(nameof(CanGoForward));
        }
    }

    public void GoForward()
    {
        if (currentPageNode?.Next is not null)
        {
            currentPageNode = currentPageNode.Next;
            UpdateCurrentPage();
            OnPropertyChanged(nameof(CanGoBack));
            OnPropertyChanged(nameof(CanGoForward));
        }
    }

    public bool CanGoBack => currentPageNode?.Previous is not null;
    public bool CanGoForward => currentPageNode?.Next is not null;

    private void UpdateCurrentPage()
    {
        if (currentPageNode?.Value != null)
        {
            CurrentPage = (ViewModelBase)serviceProvider.GetRequiredService(currentPageNode.Value);
        }
    }

    public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
    {
        NavigateTo(typeof(TViewModel));
    }
}
