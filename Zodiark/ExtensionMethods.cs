using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiark
{
	public static class ExtensionMethods
	{
		public static string ToHex(this IntPtr p) => string.Format("0x{0:X}", (ulong)p);
	}
}
