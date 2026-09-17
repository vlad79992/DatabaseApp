using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using BookDatabaseApp.Models;
using BookDatabaseApp.Services;
using BookDatabaseApp.ViewModels;
using BookDatabaseApp.Views;
using Microsoft.EntityFrameworkCore;

namespace BookDatabaseApp
{
    public partial class App : Application
    {
        private IServiceProvider serviceProvider = null!;
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var services = new ServiceCollection();

            services.AddSingleton<NavigationService>();
            services.AddSingleton<INavigationService>(sp => sp.GetRequiredService<NavigationService>());
            
            services.AddDbContextFactory<BookContext>(opt => opt.UseSqlite("Data Source=books.db"));

            // ViewModels
            services.AddTransient<ActionSelectionViewModel>();
            services.AddTransient<ViewModels.CRUD.CreateViewModel>();
            services.AddTransient<ViewModels.CRUD.ReadViewModel>();
            services.AddTransient<ViewModels.CRUD.EditViewModel>();
            services.AddTransient<ViewModels.CRUD.DeleteViewModel>();

            services.AddSingleton<MainWindowViewModel>();

            serviceProvider = services.BuildServiceProvider();

            var context = serviceProvider.GetRequiredService<BookContext>();
            
            context.Database.Migrate();
            
            _ = context.Set<Book>().AsNoTracking().Take(1).ToListAsync().Result;
            
            var navService = serviceProvider.GetRequiredService<NavigationService>();
            navService.Initialize(typeof(ActionSelectionViewModel));

            DataTemplates.Add(new ViewLocator(serviceProvider));

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>(),
                };
            }
            
            base.OnFrameworkInitializationCompleted();
        }
    }
}