using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Zodiark.Memory;

namespace Zodiark.Scanner
{
	public sealed class SignatureScanner
	{
		//private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		/// <param name="module">The ProcessModule to be used for scanning.</param>
		public SignatureScanner(ZodiarkProcess zodiark, ProcessModule module)
		{
			this.Module = module;
			this.Is32BitProcess = !Environment.Is64BitProcess;
			this.ProcessMemory = zodiark.Memory;
			// Limit the search space to .text section.
			this.SetupSearchSpace(module);

			//logger.Debug($"Module base: {this.TextSectionBase}");
			//logger.Debug($"Module size: {this.TextSectionSize}");
		}
		private readonly ProcessMemory ProcessMemory;
		public bool Is32BitProcess { get; }
		public IntPtr SearchBase => this.Module.BaseAddress;
		public IntPtr TextSectionBase => new IntPtr(this.SearchBase.ToInt64() + this.TextSectionOffset);
		public long TextSectionOffset { get; private set; }
		public int TextSectionSize { get; private set; }
		public IntPtr DataSectionBase => new IntPtr(this.SearchBase.ToInt64() + this.DataSectionOffset);
		public long DataSectionOffset { get; private set; }
		public int DataSectionSize { get; private set; }
		public ProcessModule Module { get; private set; }

		private IntPtr TextSectionTop => this.TextSectionBase + this.TextSectionSize;

		/// <summary>
		/// Scan for a byte signature in the .text section.
		/// </summary>
		/// <param name="signature">The signature.</param>
		/// <returns>The real offset of the found signature.</returns>
		public IntPtr ScanText(string signature)
		{
			IntPtr mBase = this.TextSectionBase;
			IntPtr scanRet = this.Scan(mBase, this.TextSectionSize, signature);

			if (ProcessMemory.ReadByte(scanRet) == 0xE8)
				return this.ReadCallSig(scanRet);

			return scanRet;
		}

		/// <summary>
		/// Scan for a .data address using a .text function.
		/// This is intended to be used with IDA sigs.
		/// Place your cursor on the line calling a static address, and create and IDA sig.
		/// </summary>
		/// <param name="signature">The signature of the function using the data.</param>
		/// <param name="offset">The offset from function start of the instruction using the data.</param>
		/// <returns>An IntPtr to the static memory location.</returns>
		public IntPtr GetStaticAddressFromSig(string signature, int offset = 0)
		{
			IntPtr instrAddr = this.ScanText(signature);
			instrAddr = IntPtr.Add(instrAddr, offset);
			long bAddr = (long)this.Module.BaseAddress;
			long num;
			do
			{
				instrAddr = IntPtr.Add(instrAddr, 1);
				num = ProcessMemory.ReadInt32(instrAddr) + (long)instrAddr + 4 - bAddr;
			}
			while (!(num >= this.DataSectionOffset && num <= this.DataSectionOffset + this.DataSectionSize));
			return IntPtr.Add(instrAddr, ProcessMemory.ReadInt32(instrAddr) + 4);
		}

		/// <summary>
		/// Scan for a byte signature in the .data section.
		/// </summary>
		/// <param name="signature">The signature.</param>
		/// <returns>The real offset of the found signature.</returns>
		public IntPtr ScanData(string signature)
		{
			IntPtr scanRet = this.Scan(this.DataSectionBase, this.DataSectionSize, signature);
			return scanRet;
		}

		/// <summary>
		/// Scan for a byte signature in the whole module search area.
		/// </summary>
		/// <param name="signature">The signature.</param>
		/// <returns>The real offset of the found signature.</returns>
		public IntPtr ScanModule(string signature)
		{
			IntPtr scanRet = this.Scan(this.SearchBase, this.Module.ModuleMemorySize, signature);
			return scanRet;
		}

		public IntPtr Scan(IntPtr baseAddress, int size, string signature)
		{

			var needle = this.SigToNeedle(signature);
			// Fast
			byte[] bigBuffer = new byte[size];
			ProcessMemory.ReadBytes(baseAddress, bigBuffer, size);

			unsafe
			{
				for (long offset = 0; offset < size - needle.Length; offset++)
				{
					if (this.IsMatch(needle, bigBuffer, offset))
					{
						UIntPtr ptr = new UIntPtr(Convert.ToUInt64(baseAddress.ToInt64() + offset));
						return (IntPtr)ptr.ToPointer();
					}
				}
			}

			throw new KeyNotFoundException($"Can't find a signature of {signature}");
		}

		private IntPtr ResolveRelativeAddress(IntPtr nextInstAddr, int relOffset)
		{
			if (this.Is32BitProcess)
				throw new NotSupportedException("32 bit is not supported.");

			return nextInstAddr + relOffset;
		}
		private void SetupSearchSpace(ProcessModule module)
		{
			IntPtr baseAddress = module.BaseAddress;

			// We don't want to read all of IMAGE_DOS_HEADER or IMAGE_NT_HEADER stuff so we cheat here.
			int ntNewOffset = ProcessMemory.ReadInt32(baseAddress, 0x3C);
			IntPtr ntHeader = baseAddress + ntNewOffset;

			// IMAGE_NT_HEADER
			IntPtr fileHeader = ntHeader + 4;
			short numSections = ProcessMemory.ReadInt16(ntHeader, 6);

			// IMAGE_OPTIONAL_HEADER
			IntPtr optionalHeader = fileHeader + 20;

			IntPtr sectionHeader;
			if (this.Is32BitProcess) // IMAGE_OPTIONAL_HEADER32
				sectionHeader = optionalHeader + 224;
			else // IMAGE_OPTIONAL_HEADER64
				sectionHeader = optionalHeader + 240;

			// IMAGE_SECTION_HEADER
			IntPtr sectionCursor = sectionHeader;
			for (int i = 0; i < numSections; i++)
			{
				long sectionName = ProcessMemory.ReadInt64(sectionCursor);

				// .text
				switch (sectionName)
				{
					case 0x747865742E: // .text
						this.TextSectionOffset = ProcessMemory.ReadInt32(sectionCursor, 12);
						this.TextSectionSize = ProcessMemory.ReadInt32(sectionCursor, 8);
						break;
					case 0x617461642E: // .data
						this.DataSectionOffset = ProcessMemory.ReadInt32(sectionCursor, 12);
						this.DataSectionSize = ProcessMemory.ReadInt32(sectionCursor, 8);
						break;
				}

				sectionCursor += 40;
			}
		}

		/// <summary>
		/// Helper for ScanText to get the correct address for
		/// IDA sigs that mark the first CALL location.
		/// </summary>
		/// <param name="sigLocation">The address the CALL sig resolved to.</param>
		/// <returns>The real offset of the signature.</returns>
		private IntPtr ReadCallSig(IntPtr sigLocation)
		{
			int jumpOffset = ProcessMemory.ReadInt32(IntPtr.Add(sigLocation, 1));
			return IntPtr.Add(sigLocation, 5 + jumpOffset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe bool IsMatch(byte?[] needle, byte[] buffer, long offset)
		{
			for (int i = 0; i < needle.Length; i++)
			{
				byte? expected = needle[i];
				if (expected == null)
					continue;

				byte actual = buffer[offset + i];
				if (expected != actual)
					return false;
			}

			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private byte?[] SigToNeedle(string signature)
		{
			// Strip all whitespaces
			signature = signature.Replace(" ", string.Empty);

			if (signature.Length % 2 != 0)
				throw new ArgumentException("Signature without whitespaces must be divisible by two.", nameof(signature));

			int needleLength = signature.Length / 2;
			var needle = new byte?[needleLength];

			for (int i = 0; i < needleLength; i++)
			{
				string hexString = signature.Substring(i * 2, 2);
				if (hexString == "??" || hexString == "**")
				{
					needle[i] = null;
					continue;
				}
				needle[i] = byte.Parse(hexString, NumberStyles.AllowHexSpecifier);
			}
			return needle;
		}
	}
}
