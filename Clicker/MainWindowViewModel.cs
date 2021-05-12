using System;
using System.Threading;
using System.Threading.Tasks;
using Clicker.Core;
using Prism.Commands;
using Prism.Mvvm;

namespace Clicker
{
    public class MainWindowViewModel : BindableBase, IDisposable
    {
        public int ClicksTotal { get; set; }
        public int Delay { get; set; }
        public int DelayDeviation { get; set; }
        public int StartDelay { get; set; }

        private double _percentsDone;
        public double PercentsDone
        {
            get => _percentsDone;
            set => SetProperty(ref _percentsDone, value);
        }
        
        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public DelegateCommand RunCommand { get; set; }
        
        private CancellationTokenSource _tokenSource;
        private readonly GlobalKeyHook _escapeKeyHook;

        public MainWindowViewModel()
        {
            RunCommand = new DelegateCommand(async () => await RunCommandHandler(), () => !IsRunning);
            _escapeKeyHook = new GlobalKeyHook(0x1B, GlobalEscapeKeyDownHandler);
        }

        private void GlobalEscapeKeyDownHandler(object sender, EventArgs e)
        {
            _tokenSource?.Cancel();
        }

        private async Task RunCommandHandler()
        {
            IsRunning = true;

            if (ClicksTotal > 0)
            {
                var progress = new Progress<double>(ProgressHandler);
                _tokenSource = new CancellationTokenSource();
                _escapeKeyHook.SetHook();

                var runner = new Runner(ClicksTotal, Delay, DelayDeviation);
                Task.Delay(TimeSpan.FromSeconds(StartDelay)).Wait();

                try
                {
                    await runner.RunAsync(_tokenSource.Token, progress);
                }
                catch (OperationCanceledException)
                {
                    _tokenSource.Dispose();
                    _tokenSource = null;
                }
                finally
                {
                    _escapeKeyHook.ReleaseHook();
                }
            }

            IsRunning = false;
        }

        private void ProgressHandler(double percentsDone)
        {
            if ((int) (percentsDone * 10) != (int) (PercentsDone * 10))
                PercentsDone = percentsDone;
        }

        private void ReleaseUnmanagedResources()
        {
            _escapeKeyHook.Dispose();
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing) _tokenSource?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MainWindowViewModel()
        {
            Dispose(false);
        }
    }
}