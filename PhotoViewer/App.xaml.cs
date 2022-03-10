using PhotoViewer.Services;
using PhotoViewer.ViewModels;
using PhotoViewer.Views;
using System.Windows;

namespace PhotoViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string? fileName = null;
            if (e.Args.Length > 0)
            {
                fileName = e.Args[0];
            }

            var mainWindow = new MainWindow();
            var photoService = new LocalPhotoService();
            mainWindow.DataContext = new MainWindowViewModel(photoService, fileName);
            mainWindow.Show();
        }
    }
}
