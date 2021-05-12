using System;
using System.Windows;
using Jot;
using Jot.Storage;

namespace Clicker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Tracker _tracker = new Tracker(new JsonFileStore(Environment.CurrentDirectory));
        private MainWindowViewModel _viewModel;

        private void OnStartup(object sender, StartupEventArgs e)
        {
            _tracker.Configure<MainWindow>()
                .Id(w => w.Name, new Size(SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight))
                .Property(w => w.Top)
                .Property(w => w.Left)
                .Property(w => w.Count.Text, "Count")
                .Property(w => w.Delay.Text, "Delay")
                .Property(w => w.DelayDeviation.Text, "DelayDeviation")
                .Property(w => w.StartDelay.Text, "StartDelay")
                .PersistOn(nameof(Window.Closing))
                .StopTrackingOn(nameof(Window.Closing));

            _viewModel = new MainWindowViewModel();
            var window = new MainWindow {DataContext = _viewModel};
            _tracker.Track(window);
            window.Show();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            _viewModel.Dispose();
        }
    }
}