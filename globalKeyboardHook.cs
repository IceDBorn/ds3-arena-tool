using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DS3_Arena_Tool {
    public class GlobalKeyboardHook {
        #region Constant, Structure and Delegate Definitions

        private delegate int KeyboardHookProc(int code, int wParam, ref KeyboardHookStruct lParam);

        private struct KeyboardHookStruct {
            public readonly int VkCode;
            private int _scanCode;
            private int _flags;
            private int _time;
            private int _dwExtraInfo;

            public KeyboardHookStruct(int vkCode, int scanCode, int flags, int time, int dwExtraInfo) : this() {
	            this.VkCode = vkCode;
	            _scanCode = scanCode;
	            _flags = flags;
	            _time = time;
	            _dwExtraInfo = dwExtraInfo;
            }
        }

        private const int WhKeyboardLl = 13;
        private const int WmKeydown = 0x100;
        private const int WmKeyup = 0x101;
        private const int WmSyskeydown = 0x104;
        private const int WmSyskeyup = 0x105;
		#endregion

		#region Instance Variables
		public readonly List<Keys> HookedKeys = new List<Keys>();
		IntPtr _hhook = IntPtr.Zero;
		#endregion

		#region Events
		public event KeyEventHandler KeyDown;
		#endregion

		#region Constructors and Destructors
		public GlobalKeyboardHook() {
			Hook();
		}
		
		~GlobalKeyboardHook() {
			Unhook();
		}
		#endregion

		#region Public Methods

		private void Hook() {
			var hInstance = LoadLibrary("User32");
			_hhook = SetWindowsHookEx(WhKeyboardLl, HookProc, hInstance, 0);
		}

		private void Unhook() {
			UnhookWindowsHookEx(_hhook);
		}

		private int HookProc(int code, int wParam, ref KeyboardHookStruct lParam) {
			if (code < 0) return CallNextHookEx(_hhook, code, wParam, ref lParam);
			var key = (Keys)lParam.VkCode;
			if (!HookedKeys.Contains(key)) return CallNextHookEx(_hhook, code, wParam, ref lParam);
			var kea = new KeyEventArgs(key);
			if ((wParam == WmKeydown || wParam == WmSyskeydown) && (KeyDown != null)) {
				KeyDown(this, kea);
			}
			return kea.Handled ? 1 : CallNextHookEx(_hhook, code, wParam, ref lParam);
		}
		#endregion

		#region DLL imports
		[DllImport("user32.dll")]
		private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, uint threadId);
		
		[DllImport("user32.dll")]
		private static extern bool UnhookWindowsHookEx(IntPtr hInstance);
		
		[DllImport("user32.dll")]
		private static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KeyboardHookStruct lParam);
		
		[DllImport("kernel32.dll")]
		private static extern IntPtr LoadLibrary(string lpFileName);
		#endregion
	}
}