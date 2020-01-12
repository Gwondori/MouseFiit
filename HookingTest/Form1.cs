using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hook;

// 원본과 변경된 점:
// 1. Mouse Wheel Event(Down/Up) 추가.
// 2. 기능 2개 추가, Double Click - Copy, Wheel Button Down - Paste.
namespace HookingTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            KeyboardHook.KeyDown += KeyboardHook_KeyDown;
            KeyboardHook.KeyUp += KeyboardHook_KeyUp;
            MouseHook.MouseDown += MouseHook_MouseDown;
            MouseHook.MouseUp += MouseHook_MouseUp;
            // MouseHook.MouseMove += MouseHook_MouseMove;
            // MouseHook.MouseScroll += MouseHook_MouseScroll;

            KeyboardHook.HookStart();
            if (!MouseHook.HookStart())
            {
                MessageBox.Show("Mouse hook failed");
            }

            FormClosing += Form1_FormClosing;
        }

        private bool KeyboardHook_KeyDown(int vkCode)
        {
            AppendText($"KEYDOWN : {(Keys)vkCode}");
            return true;
        }

        private bool KeyboardHook_KeyUp(int vkCode)
        {
            AppendText($"KEYUP : {(Keys)vkCode}");
            return true;
        }

        private bool MouseHook_MouseDown(MouseEventType type, int x, int y)
        {
            AppendText($"MOUSEDOWN: {type} at ({x}, {y})");
            return true;
        }

        private bool MouseHook_MouseUp(MouseEventType type, int x, int y)
        {
            AppendText($"MOUSEUP: {type} at ({x}, {y})");
            if (type == MouseEventType.LEFT)
            {
                // Start, Get Window Handle.
                IntPtr hWnd = SpyWindow.Spy.WindowFromPointEx(x, y);
                // Get Selected Text and Copy to clipboard
                SpyWindow.Spy.TextCopyFromWindow(hWnd);
            }
            if (type == MouseEventType.WHEEL)
            {
                // Start, Get Window Handle.
                IntPtr hWnd = SpyWindow.Spy.WindowFromPointEx(x, y);
                // Get Selected Text and Paste from clipboard
                SpyWindow.Spy.TextPasteFromWindow(hWnd);
            }
            return true;
        }

        private bool MouseHook_MouseMove(MouseEventType type, int x, int y)
        {
            AppendText($"MOUSEMOVE: {type} at ({x}, {y})");
            return true;
        }

        private bool MouseHook_MouseScroll(MouseScrollType type)
        {
            AppendText($"MOUSESCROLL: {type}");
            return true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            KeyboardHook.HookEnd();
            MouseHook.HookEnd();
        }

        private void AppendText(string text)
        {
            this.textBox1.AppendText(text + "\r\n");
        }
    }
}
