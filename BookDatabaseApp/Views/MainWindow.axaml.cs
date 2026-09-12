using Classic.Avalonia.Theme;
using Classic.CommonControls.Dialogs;

namespace BookDatabaseApp.Views
{
    public partial class MainWindow : ClassicWindow // какой-то баг, при развернутом на полный экран окне, оно не сворачивается при двойном клике
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void Stop(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var result = await MessageBox.ShowDialog(
                this,
                "Вы действительно хотите выйти из приложения?",
                "Подтверждение выхода",
                MessageBoxButtons.OkCancel,
                MessageBoxIcon.Question
            );

            if (result == MessageBoxResult.Ok)
            {
                this.Close();
            }
        }
        private void Refresh(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
        }
        private void Home(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
        }

    }
}