using System;
using System.Runtime.InteropServices;

namespace Clicker.Core.Native
{
    public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

    public static class WindowsNative
    {
        public static void MouseEvent(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo)
        {
            mouse_event(dwFlags, dx, dy, cButtons, dwExtraInfo);
        }

        public static bool GetCursorPosition(out Point lpPoint)
        {
            return GetCursorPos(out lpPoint);
        }
        
        public static IntPtr SetWindowsHook(int idHook, HookProc callback, IntPtr hInstance, uint threadId)
        {
            return SetWindowsHookEx(idHook, callback, hInstance, threadId);
        }

        public static bool UnhookWindowsHook(IntPtr hInstance)
        {
            return UnhookWindowsHookEx(hInstance);
        }

        public static IntPtr CallNextHook(IntPtr idHook, int nCode, int wParam, IntPtr lParam)
        {
            return CallNextHookEx(idHook, nCode, wParam, lParam);
        }

        public static IntPtr LoadNativeLibrary(string lpFileName)
        {
            return LoadLibrary(lpFileName);
        }

        public static bool FreeNativeLibrary(IntPtr hModule)
        {
            return FreeLibrary(hModule);
        }
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeLibrary(IntPtr hModule);
    }
}