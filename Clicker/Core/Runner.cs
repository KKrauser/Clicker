using System;
using System.Threading;
using System.Threading.Tasks;
using Clicker.Core.Native;

namespace Clicker.Core
{
    public class Runner
    {
        private const int MouseLeftDownFlag = 0x02;
        private const int MouseLeftUpFlag = 0x04;

        private readonly Random _deviationRandomizer;

        private readonly int _clicksTotal;
        private readonly int _delay;
        private readonly int _delayDeviation;

        public Runner(int clicksTotal, int delay, int delayDeviation = 0)
        {
            _clicksTotal = clicksTotal < 0 ? throw new ArgumentOutOfRangeException(nameof(clicksTotal)) : clicksTotal;
            _delay = delay < 0 ? throw new ArgumentOutOfRangeException(nameof(delay)) : delay;
            _delayDeviation = delayDeviation < 0 ? throw new ArgumentOutOfRangeException(nameof(delayDeviation)) : delayDeviation;

            _deviationRandomizer = new Random((int) DateTime.Now.ToFileTimeUtc());
        }

        public async Task RunAsync(CancellationToken token, IProgress<double> progress = null)
        {
            var delayMin = _delay - _delayDeviation;
            if (delayMin < 0) delayMin = 0;
            var delayMax = _delay + _delayDeviation + 1;

            if (_clicksTotal > 0) await LimitedClicksRun(delayMin, delayMax, token, progress);
            else await EndlessClicksRun(delayMin, delayMax, token, progress);
        }

        private async Task LimitedClicksRun(int delayMin, int delayMax, CancellationToken token, IProgress<double> progress = null)
        {
            for (var i = 1; i <= _clicksTotal; i++)
            {
                Click();
                progress?.Report(i * 100d / _clicksTotal);
                var delay = _deviationRandomizer.Next(delayMin, delayMax);
                await Task.Delay(delay, token);
            }
        }

        private async Task EndlessClicksRun(int delayMin, int delayMax, CancellationToken token, IProgress<double> progress = null)
        {
            progress?.Report(0);
            
            while (true)
            {
                Click();
                var delay = _deviationRandomizer.Next(delayMin, delayMax);
                await Task.Delay(delay, token);
            }
        }

        private static void Click()
        {
            WindowsNative.GetCursorPosition(out var cursorPosition);
            WindowsNative.MouseEvent(MouseLeftDownFlag | MouseLeftUpFlag, (uint) cursorPosition.X,
                (uint) cursorPosition.Y, 0, 0);
        }
    }
}