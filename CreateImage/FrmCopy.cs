using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateImage
{
    public partial class FrmCopy : Form
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104; // Added WM_SYSKEYDOWN
        private const int VK_A = 0x41;
        private const int VK_C = 0x43; // Added VK_C constant
        private const int VK_CONTROL = 0x11;

        private IntPtr hookId = IntPtr.Zero;
        private LowLevelKeyboardProc keyboardProc;

        public FrmCopy()
        {
            InitializeComponent();
            KeyPreview = true;

            // Set up the low-level keyboard hook
            keyboardProc = HookCallback;
            hookId = SetHook(keyboardProc);
        }

        private void FrmCopy_Load(object sender, EventArgs e)
        {
            this.Activate();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (ProcessModule curModule = Process.GetCurrentProcess().MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                int vkCode = Marshal.ReadInt32(lParam);

                // Check for Ctrl+A or Ctrl+C
                if ((Control.ModifierKeys & Keys.Control) != 0)
                {
                    if (vkCode == VK_A)
                    {
                        // Perform actions for Ctrl+A using SendInput
                        SendCtrlA();
                    }
                    else if (vkCode == VK_C)
                    {
                        // Perform actions for Ctrl+C using SendInput
                        SendCtrlC();
                    }
                }
            }

            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        private void SendCtrlA()
        {
            // Simulate Ctrl+A using SendInput
            INPUT[] inputs = new INPUT[4];
            inputs[0] = new INPUT { Type = INPUT_KEYBOARD, Ki = new KEYBDINPUT { WVk = VK_CONTROL, WScan = 0, Flags = 0 } };
            inputs[1] = new INPUT { Type = INPUT_KEYBOARD, Ki = new KEYBDINPUT { WVk = VK_A, WScan = 0, Flags = 0 } };
            inputs[2] = new INPUT { Type = INPUT_KEYBOARD, Ki = new KEYBDINPUT { WVk = VK_A, WScan = 0, Flags = KEYEVENTF_KEYUP } };
            inputs[3] = new INPUT { Type = INPUT_KEYBOARD, Ki = new KEYBDINPUT { WVk = VK_CONTROL, WScan = 0, Flags = KEYEVENTF_KEYUP } };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        private void SendCtrlC()
        {
            // Simulate Ctrl+C using SendInput
            INPUT[] inputs = new INPUT[4];
            inputs[0] = new INPUT { Type = INPUT_KEYBOARD, Ki = new KEYBDINPUT { WVk = VK_CONTROL, WScan = 0, Flags = 0 } };
            inputs[1] = new INPUT { Type = INPUT_KEYBOARD, Ki = new KEYBDINPUT { WVk = VK_C, WScan = 0, Flags = 0 } };
            inputs[2] = new INPUT { Type = INPUT_KEYBOARD, Ki = new KEYBDINPUT { WVk = VK_C, WScan = 0, Flags = KEYEVENTF_KEYUP } };
            inputs[3] = new INPUT { Type = INPUT_KEYBOARD, Ki = new KEYBDINPUT { WVk = VK_CONTROL, WScan = 0, Flags = KEYEVENTF_KEYUP } };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));

            System.Threading.Thread.Sleep(1000);

            // Retrieve the copied text from the clipboard
            if (Clipboard.ContainsText())
            {
                string copiedText = Clipboard.GetText();
                MessageBox.Show($"Copied Text: {copiedText}");
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private void FrmCopy_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Unhook the keyboard hook when the form is closing
            UnhookWindowsHookEx(hookId);
        }

        private void btnMinimize_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint Type;
            public KEYBDINPUT Ki;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public ushort WVk;
            public ushort WScan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }
        private const int INPUT_KEYBOARD = 1;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
    }
   
}
