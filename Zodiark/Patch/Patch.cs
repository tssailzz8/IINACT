using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiark.Memory;

namespace Zodiark.Patch
{
	public sealed class Patch
	{
		private ProcessMemory Memory;
		public readonly IntPtr PatchAddress;
		public byte?[] patch { get; private set; }
		public byte?[] origin { get; private set; }
		public int size { get; private set; }
		private bool[] mask { get; set; }

		public Patch(ZodiarkProcess Zodiark, IntPtr address, byte?[] content)
		{
			this.Memory = Zodiark.Memory;
			PatchAddress = address;
			patch = content;
			size = content.Length;
			GetMask(content);
			origin = new byte?[size];
			ReadOrigin();
		}

		public Patch(ZodiarkProcess Zodiark, IntPtr address, string mask)
		{
			this.Memory = Zodiark.Memory;
			PatchAddress = address;
			var content = SigToNeedle(mask);
			size = content.Length;
			origin = new byte?[size];
			GetMask(content);
			ReadOrigin();
		}

		//public Patch(Zodiark zodiark, IntPtr address, string[] asm) {
		//    PatchAddress = address;
		//    Process = zodiark;
		//    var content = AsmjitCSharp.Assemble(string.Join("\n",asm));
		//    size = content.Length;
		//    patch = new byte?[size];
		//    for (int i = 0; i < size; i++) {
		//        patch[i]=content[i];
		//    }
		//    origin = new byte?[size];
		//    ReadOrigin();
		//}


		public void Enable()
		{
			Write(patch);
		}

		public void Diable()
		{
			Write(origin);
		}

		private void Write(byte?[] content)
		{
			for (int i = 0; i < content.Length; i++)
			{
				if (mask[i]) Memory.WriteByte(PatchAddress + i, (byte)content[i]);
			}
		}

		private void ReadOrigin()
		{
			for (int i = 0; i < mask.Length; i++)
			{
				if (mask[i]) origin[i] = Memory.ReadByte(PatchAddress + i);
				else origin[i] = null;
			}
		}

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

		private void GetMask(byte?[] content)
		{
			mask = new bool[content.Length];
			for (int i = 0; i < content.Length; i++)
			{
				mask[i] = content[i] == null ? false : true;
			}
		}
	}
}
