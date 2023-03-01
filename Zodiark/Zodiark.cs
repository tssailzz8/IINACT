using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Zodiark.Memory;
using Zodiark.Scanner;
using Zodiark.Patch;
using Zodiark.Injection;

namespace Zodiark
{
	public class ZodiarkProcess
	{
		public IntPtr Handle { get; private set; }
		public IntPtr BaseAddress { get; private set; }
		public SignatureScanner Scanner { get; private set; }
		public Process Process { get; private set; }
		public bool IsProcessAlive { get; private set; }
		public ProcessMemory Memory { get; private set; }
		//private readonly Dictionary<string, IntPtr> modules = new Dictionary<string, IntPtr>();


		public ZodiarkProcess(Process process)
		{
			OpenProcess(process);
			Memory = new ProcessMemory(this);
			Scanner = new SignatureScanner(this, Process.MainModule);
		}

		#region OpenProcess
		/// <summary>
		/// Open the PC game process with all security and access rights.
		/// </summary>
		public void OpenProcess(Process process)
		{
			Process = process;
			BaseAddress = Process.MainModule.BaseAddress;
			//if (!Process.Responding)
			//	throw new Exception("Target process id not responding");

			if (process.MainModule == null)
				throw new Exception("Process has no main module");

			Process.EnterDebugMode();
			int debugPrivilegeCheck = CheckSeDebugPrivilege(out bool isDebugEnabled);
			if (debugPrivilegeCheck != 0)
			{
				throw new Exception($"ERROR: CheckSeDebugPrivilege failed with error: {debugPrivilegeCheck}");
			}
			else if (!isDebugEnabled)
			{
				throw new Exception("ERROR: SeDebugPrivilege not enabled. Please report this!");
			}

			Handle = Kernel32.OpenProcess(0x001F0FFF, true, process.Id);
			if (Handle == IntPtr.Zero)
			{
				int eCode = Marshal.GetLastWin32Error();
			}

			// Set all modules
			//this.modules.Clear();
			//foreach (ProcessModule module in Process.Modules) {
			//    if (module == null)
			//        continue;

			//    if (string.IsNullOrEmpty(module.ModuleName))
			//        continue;

			//    if (this.modules.ContainsKey(module.ModuleName))
			//        continue;

			//    this.modules.Add(module.ModuleName, module.BaseAddress);
			//}
		}

		private int CheckSeDebugPrivilege(out bool isDebugEnabled)
		{
			isDebugEnabled = false;

			if (!Kernel32.OpenProcessToken(Kernel32.GetCurrentProcess(), 0x8 /*TOKEN_QUERY*/, out IntPtr tokenHandle))
				return Marshal.GetLastWin32Error();

			Kernel32.LUID luidDebugPrivilege = default;
			if (!Kernel32.LookupPrivilegeValue(null, "SeDebugPrivilege", ref luidDebugPrivilege))
				return Marshal.GetLastWin32Error();

			Kernel32.PRIVILEGE_SET requiredPrivileges = new Kernel32.PRIVILEGE_SET
			{
				PrivilegeCount = 1,
				Control = 1 /* PRIVILEGE_SET_ALL_NECESSARY */,
				Privilege = new Kernel32.LUID_AND_ATTRIBUTES[1],
			};

			requiredPrivileges.Privilege[0].Luid = luidDebugPrivilege;
			requiredPrivileges.Privilege[0].Attributes = 2 /* SE_PRIVILEGE_ENABLED */;

			if (!Kernel32.PrivilegeCheck(tokenHandle, ref requiredPrivileges, out bool bResult))
				return Marshal.GetLastWin32Error();

			// bResult == true => SeDebugPrivilege is on; otherwise it's off
			isDebugEnabled = bResult;

			Kernel32.CloseHandle(tokenHandle);

			return 0;
		}

		public bool GetIsProcessAlive()
		{
			if (Process == null || Process.HasExited)
				return false;

			if (!Process.Responding)
				return false;

			return true;
		}

		#endregion

		#region Scanner
		public SignatureScanner SetSignatureScanner(ProcessModule module)
		{
			var sigScaner = new SignatureScanner(this, module);
			return sigScaner;
		}

		public SignatureScanner SetSignatureScanner(string moduleName)
		{
			var module = Process.Modules.Cast<ProcessModule>().First(m => m.ModuleName == moduleName);
			var sigScaner = new SignatureScanner(this, module);
			return sigScaner;
		}
		#endregion

		#region Patch
		public Zodiark.Patch.Patch SetPatch(IntPtr address, byte?[] content)
		{
			var patch = new Zodiark.Patch.Patch(this, address, content);
			return patch;
		}

		public Zodiark.Patch.Patch SetPatch(IntPtr address, string mask)
		{
			var patch = new Zodiark.Patch.Patch(this, address, mask);
			return patch;
		}

		#endregion
		public IntPtr Execute( IntPtr address, params dynamic[] args) => FastCall.Execute(this,  address, args);

	}
}
