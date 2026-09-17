using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;
using System;
using BookDatabaseApp.ViewModels;
using BookDatabaseApp.ViewModels.CRUD;
using BookDatabaseApp.Views;
using BookDatabaseApp.Views.CRUD;
using CreateViewModel = BookDatabaseApp.ViewModels.CRUD.CreateViewModel;
using EditViewModel = BookDatabaseApp.ViewModels.CRUD.EditViewModel;
using DeleteViewModel = BookDatabaseApp.ViewModels.CRUD.DeleteViewModel;

namespace BookDatabaseApp
{
    public class ViewLocator : IDataTemplate
    {
        private readonly IServiceProvider services;

        public ViewLocator(IServiceProvider services)
        {
            this.services = services;
        }

        public Control Build(object? data)
        {
            return data switch
            {
                ActionSelectionViewModel => new ActionSelectionView(),
                CreateViewModel => new CreateView(),
                ReadViewModel => new ReadView(),
                EditViewModel => new EditView(),
                DeleteViewModel => new DeleteView(),
                _ => new TextBlock { Text = $"No view for {data?.GetType().Name}" }
            };
        }

        public bool Match(object? data) => data is ViewModelBase;
    }
}
