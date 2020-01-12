using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SpyWindow
{
    public static class Spy
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        private struct Point
        {
            public Int32 x;
            public Int32 y;
        }

        internal const int WM_COPY = 0x0301;
        internal const int WM_PASTE = 0x0302;

        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point lpPoint);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, IntPtr lparam, IntPtr wparam);

        public static void HelloDll()
        {
            Debug.WriteLine("Hello, DLL!");
        }

        public static IntPtr WindowFromPointEx(int x, int y)
        {
            Point temp_pt = new Point();
            temp_pt.x = x;
            temp_pt.y = y;

            return WindowFromPoint(temp_pt);
        }

        public static void TextCopyFromWindow(IntPtr hWnd)
        {
            SendMessage(hWnd, WM_COPY, IntPtr.Zero, IntPtr.Zero);
        }

        public static void TextPasteFromWindow(IntPtr hWnd)
        {
            SendMessage(hWnd, WM_PASTE, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
