using Iced.Intel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using static Iced.Intel.AssemblerRegisters;
using System.Text;
using System.Threading.Tasks;

namespace Zodiark.Injection
{
	class AsmCache
	{
		private dynamic[] args;
		private IntPtr funcAddress;
		private IntPtr returnAddress;
		public List<string> instructions = new List<string>();
		private Assembler Assembler;

		public byte[] asmBytes;

		public AsmCache(IntPtr funcAddress, IntPtr returnAddress, params dynamic[] args)
		{
			this.funcAddress = funcAddress;
			this.returnAddress = returnAddress;
			this.args = args;
			Assembler = new Assembler(64);
			FormatFunctionCall();
			Assembly();
		}
		public void Assembly()
		{
			var stream = new MemoryStream();
			Assembler.Assemble(new StreamCodeWriter(stream), 0);
			asmBytes = stream.ToArray();
		}
		public void FormatFunctionCall()
		{
			switch (args.Length)
			{
				default:
					Assembler.mov(r9, (ulong)args[3]);
					goto case 3;
				case 3:
					Assembler.mov(r8, (ulong)args[2]);
					goto case 2;
				case 2:
					Assembler.mov(rdx, (ulong)args[1]);
					goto case 1;
				case 1:
					Assembler.mov(rcx, (ulong)args[0]);
					break;
				case 0:
					break;
			}
			foreach (var arg in args.Skip(4).Reverse())
			{
				Assembler.push((uint)arg);
			}
			Assembler.sub(rsp, 40);
			Assembler.mov(rax, (ulong)funcAddress);
			Assembler.call(rax);
			Assembler.mov(rcx, (ulong)returnAddress);
			Assembler.mov(__[rcx], rax);
			Assembler.add(rsp, 40);
			Assembler.ret();
		}
	}
}
