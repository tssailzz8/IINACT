using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zodiark.Memory
{
	public class ProcessMemory
	{
		public static IntPtr Handle { get; private set; }
		public static IntPtr BaseAddress { get; private set; }
		public Process Process { get; private set; }

		public ProcessMemory(ZodiarkProcess zodiark)
		{
			Handle = zodiark.Handle;
			BaseAddress = zodiark.BaseAddress;
			Process = zodiark.Process;
		}
		#region Read
		public byte ReadByte(IntPtr baseAddress, int offset = 0)
		{
			byte[] buffer = new byte[1];
			ReadBytes(baseAddress + offset, buffer, 1);
			return buffer[0];
		}

		public short ReadInt16(IntPtr baseAddress, int offset = 0)
		{
			byte[] buffer = new byte[2];
			ReadBytes(baseAddress + offset, buffer, 2);
			return BitConverter.ToInt16(buffer, 0);
		}

		public int ReadInt32(IntPtr baseAddress, int offset = 0)
		{
			byte[] buffer = new byte[4];
			ReadBytes(baseAddress + offset, buffer, 4);
			return BitConverter.ToInt32(buffer, 0);
		}

		public long ReadInt64(IntPtr baseAddress, int offset = 0)
		{
			byte[] buffer = new byte[8];
			ReadBytes(baseAddress + offset, buffer, 8);
			return BitConverter.ToInt64(buffer, 0);
		}

		public IntPtr ReadIntPtr(IntPtr baseAddress, int offset = 0)
		{
			byte[] buffer = new byte[8];
			ReadBytes(baseAddress + offset, buffer, 8);
			UInt64 ptr = BitConverter.ToUInt64(buffer, 0);
			return (IntPtr)ptr;
		}

		public T Read<T>(IntPtr address)
			where T : struct
		{
			if (address == IntPtr.Zero)
				throw new Exception("Invalid address");

			int size = Marshal.SizeOf(typeof(T));
			IntPtr mem = Marshal.AllocHGlobal(size);
			Kernel32.ReadProcessMemory(Handle, address, mem, size, out _);
			T? val = Marshal.PtrToStructure<T>(mem);
			Marshal.FreeHGlobal(mem);

			if (val != null)
				return (T)val;


			throw new Exception($"Failed to read memory {typeof(T)} from address {address}");
		}

		public bool ReadBytes(IntPtr address, byte[] buffer, int size = -1)
		{
			if (size <= 0)
				size = buffer.Length;
			return Kernel32.ReadProcessMemory(Handle, address, buffer, size, out _);
		}
		#endregion

		#region Write

		public void WriteByte(IntPtr baseAddress, byte data)
		{
			WriteBytes(baseAddress, new byte[] { (byte)data });
		}

		public void WriteInt16(IntPtr baseAddress, Int16 data)
		{
			WriteBytes(baseAddress, BitConverter.GetBytes(data));
		}

		public void WriteInt32(IntPtr baseAddress, Int32 data)
		{
			WriteBytes(baseAddress, BitConverter.GetBytes(data));
		}

		public void WriteInt64(IntPtr baseAddress, Int64 data)
		{
			WriteBytes(baseAddress, BitConverter.GetBytes(data));
		}
		/// <summary>
		/// Write a value to the specified offset, determined by type.
		/// </summary>
		/// <param name="offset">Offset to write to.</param>
		/// <param name="data">Value to write.</param>
		/// <exception cref="ArgumentException">Gets thrown, when the type to write is unsupported.</exception>
		public void Write(IntPtr offset, object data)
		{
			var @writeMethods = new Dictionary<Type, Action>
			{
				{typeof(byte[]), () => WriteBytes(offset, (byte[]) data)},
				{typeof(byte), () => WriteBytes(offset, new byte[] {(byte) data})},

				{typeof(char), () => WriteBytes(offset, new byte[] {(byte) data})},
				{typeof(short), () => WriteBytes(offset, BitConverter.GetBytes((short) data))},
				{typeof(ushort), () => WriteBytes(offset, BitConverter.GetBytes((ushort) data))},
				{typeof(int), () => WriteBytes(offset, BitConverter.GetBytes((int) data))},
				{typeof(uint), () => WriteBytes(offset, BitConverter.GetBytes((uint) data))},
				{typeof(long), () => WriteBytes(offset, BitConverter.GetBytes((long) data))},
				{typeof(ulong), () => WriteBytes(offset, BitConverter.GetBytes((ulong) data))},
				{typeof(float), () => WriteBytes(offset, BitConverter.GetBytes((float) data))},
				{typeof(double), () => WriteBytes(offset, BitConverter.GetBytes((double) data))},
			};

			if (@writeMethods.ContainsKey(data.GetType()))
				@writeMethods[data.GetType()]();
			else
				throw new ArgumentException("Unsupported type.");
		}

		public void Write<T>(IntPtr address, T value)
			where T : struct
		{
			if (address == IntPtr.Zero)
				return;

			int size = Marshal.SizeOf(typeof(T));
			IntPtr buffer = Marshal.AllocHGlobal(size);
			Marshal.StructureToPtr<T>(value, buffer, false);
			Kernel32.WriteProcessMemory(Handle, address, buffer, size, out _);
			Marshal.FreeHGlobal(buffer);
		}

		public bool WriteBytes(IntPtr address, byte[] buffer)
		{
			return Kernel32.WriteProcessMemory(Handle, address, buffer, buffer.Length, out _);
		}
		#endregion

		#region Alloc
		public IntPtr Allocate(int length)
		{
			IntPtr returnAddress = Kernel32.VirtualAllocEx
			(
				Handle,
				IntPtr.Zero,
				(UIntPtr)length,
				Kernel32.MEM_ALLOCATION_TYPE.MEM_COMMIT | Kernel32.MEM_ALLOCATION_TYPE.MEM_RESERVE,
				Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE
			);

			if (returnAddress == IntPtr.Zero)
				throw new Exception($"Failed to allocate memory in current process: {length} bytes, {Marshal.GetLastWin32Error()} last error.");

			return returnAddress;
		}

		public bool Free(IntPtr address)
		{
			Kernel32.VirtualFreeEx(Handle, address, (UIntPtr)0, Kernel32.MEM_ALLOCATION_TYPE.MEM_RELEASE);
			return true;
		}
		#endregion
	}
}
