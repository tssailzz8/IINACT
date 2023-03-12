using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiark.Injection
{
	class StaticRemoteCall
	{
		private readonly ZodiarkProcess Zodiark;
		public readonly IntPtr FunctionAddress;
		public readonly IntPtr InjectAddress;
		public readonly List<string> instructions = new List<string>();
		public readonly byte[] asmBytes;
		public readonly Type[] parameters;
		private readonly int parametersSize;
		//public RemoteCall(params Type[] parameters) {
	}
}
