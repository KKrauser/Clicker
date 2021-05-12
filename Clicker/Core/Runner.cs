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

            for (var i = 0; i < _clicksTotal;)
            {
                WindowsNative.GetCursorPos(out var cursorPosition);
                await Task.Delay(_deviationRandomizer.Next(delayMin, delayMax), token);
                WindowsNative.mouse_event(MouseLeftDownFlag | MouseLeftUpFlag, (uint) cursorPosition.X,
                    (uint) cursorPosition.Y, 0, 0);
                i++;

                progress?.Report(i * 100d / _clicksTotal);
                token.ThrowIfCancellationRequested();
            }
        }
    }
}