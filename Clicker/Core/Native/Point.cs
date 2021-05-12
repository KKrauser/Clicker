using System.Runtime.InteropServices;

namespace Clicker.Core.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator System.Drawing.Point(Point p) => new(p.X, p.Y);

        public static implicit operator Point(System.Drawing.Point p) => new(p.X, p.Y);
    }
}