using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using BookDatabaseApp.ViewModels;
using BookDatabaseApp.ViewModels.CRUD;

namespace BookDatabaseApp.Views.CRUD;

public partial class ReadView : UserControl
{
    public ReadView()
    {
        InitializeComponent();

        DataContextChanged += async (_, _) => await Load();
        Loaded += async (_, _) => await Load();
        return;

        async Task Load()
        {
            if (DataContext is ReadViewModel vm)
            {
                await vm.LoadAsync();
            }
        }
    }
}