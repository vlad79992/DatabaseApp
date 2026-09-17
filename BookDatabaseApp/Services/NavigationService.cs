using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using BookDatabaseApp.ViewModels;

namespace BookDatabaseApp.Services;

public class NavigationService(IServiceProvider serviceProvider) : ViewModelBase, INavigationService
{
    private readonly LinkedList<ViewModelBase> pages = new();
    private LinkedListNode<ViewModelBase>? currentPageNode;

    public ViewModelBase? CurrentPage
    {
        get;
        private set => SetProperty(ref field, value);
    }

    public void Initialize(Type startPage)
    {
        var startVm = (ViewModelBase)serviceProvider.GetRequiredService(startPage);
        pages.AddLast(startVm);
        currentPageNode = pages.Last;
        CurrentPage = startVm;
    }

    public void NavigateTo(ViewModelBase viewModel)
    {
        while (currentPageNode?.Next is not null) pages.RemoveLast();

        currentPageNode = pages.AddLast(viewModel);
        CurrentPage = viewModel;
        
        OnPropertyChanged(nameof(CanGoBack));
        OnPropertyChanged(nameof(CanGoForward));
    }

    public void GoBack()
    {
        if (currentPageNode?.Previous is not null)
        {
            currentPageNode = currentPageNode.Previous;
            CurrentPage = currentPageNode.Value;
            
            OnPropertyChanged(nameof(CanGoBack));
            OnPropertyChanged(nameof(CanGoForward));
        }
    }

    public void GoForward()
    {
        if (currentPageNode?.Next is not null)
        {
            currentPageNode = currentPageNode.Next;
            CurrentPage = currentPageNode.Value;
            
            OnPropertyChanged(nameof(CanGoBack));
            OnPropertyChanged(nameof(CanGoForward));
        }
    }

    public bool CanGoBack => currentPageNode?.Previous is not null;
    public bool CanGoForward => currentPageNode?.Next is not null;
}
