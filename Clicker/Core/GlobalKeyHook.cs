using System;
using System.Runtime.InteropServices;
using Clicker.Core.Native;

namespace Clicker.Core
{
    public class GlobalKeyHook : IDisposable
    {
        private const int WhKeyboardLl = 13;
        private const int WmKeydown = 0x100;

        public readonly int ObservableKeyCode;
        public event EventHandler KeyPressed;

        private IntPtr _user32Ptr;
        private IntPtr _hookPtr;
        private readonly HookProc _hookProcedureHandler;

        public GlobalKeyHook(int virtualKeyCode, EventHandler keyPressedHandler = null)
        {
            ObservableKeyCode = virtualKeyCode;
            if (keyPressedHandler != null) KeyPressed += keyPressedHandler;

            _user32Ptr = IntPtr.Zero;
            _hookPtr = IntPtr.Zero;
            _hookProcedureHandler = HookProcedureHandler;
        }

        public void SetHook()
        {
            if (_user32Ptr == IntPtr.Zero)
            {
                _user32Ptr = WindowsNative.LoadNativeLibrary("user32.dll");
                if (_user32Ptr == IntPtr.Zero) throw new Exception("user32.dll cannot be loaded!");
            }

            //Since this is a desktop app - ThreadId is 0
            _hookPtr = WindowsNative.SetWindowsHook(WhKeyboardLl, _hookProcedureHandler, _user32Ptr, 0);
        }

        public void ReleaseHook()
        {
            WindowsNative.UnhookWindowsHook(_hookPtr);
        }

        private IntPtr HookProcedureHandler(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0 && wParam == (IntPtr) WmKeydown)
            {
                var pressedKeyCode = Marshal.ReadInt32(lParam);
                if (pressedKeyCode == ObservableKeyCode)
                {
                    KeyPressed?.Invoke(this, EventArgs.Empty);
                }
            }

            return WindowsNative.CallNextHook(_hookPtr, code, (int) wParam, lParam);
        }

        private void ReleaseUnmanagedResources()
        {
            ReleaseHook();
            WindowsNative.FreeNativeLibrary(_user32Ptr);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~GlobalKeyHook()
        {
            ReleaseUnmanagedResources();
        }
    }
}