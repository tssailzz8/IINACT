﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using PostNamazu.Attributes;
using PostNamazu.Common;

namespace PostNamazu.Actions
{
    internal class Command : NamazuModule
    {
        private IntPtr ProcessChatBoxPtr;
        private IntPtr UiModulePtr;
        private IntPtr UiModule => SigScanner.ReadIntPtr(SigScanner.ReadIntPtr(UiModulePtr));
        private IntPtr ModuleOffsetPtr;
        private int ModuleOffset;
        private IntPtr RaptureModule => UiModule + ModuleOffset;

        public override void GetOffsets()
        {
            base.GetOffsets();

            //Compatible with some plugins of Dalamud
            //ProcessChatBoxPtr = _scanner.ScanText("40 53 56 57 48 83 EC 70 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 44 24 ?? 48 8B 02");
            ProcessChatBoxPtr = SigScanner.ScanText("E8 ? ? ? ? 48 8D 4C 24 ? E8 ? ? ? ? C7 07 ? ? ? ? 48 8B C7 48 8B 8C 24");

            UiModulePtr = SigScanner.GetStaticAddressFromSig("48 8B 05 ?? ?? ?? ?? 48 8B D9 8B 40 14 85 C0");
            ModuleOffsetPtr = SigScanner.ScanText("48 8D 8F ?? ?? ?? ?? 4C 8B C7 48 8D 54 24 ??") + 3;
            ModuleOffset = SigScanner.ReadInt32(ModuleOffsetPtr);
        }
		[StructLayout(LayoutKind.Explicit, Size = 0x68)]
		struct ChatBoxString
		{
			[FieldOffset(0x0)] public IntPtr StringPtr;
			[FieldOffset(0x8)] public long BufSize;
			[FieldOffset(0x10)] public long StringLength;
			[FieldOffset(0x18)] public long Ukn;
		}

		/// <summary>
		///     执行给出的文本指令
		/// </summary>
		/// <param name="command">文本指令</param>
		[Command("command")] [Command("DoTextCommand")]
        public void DoTextCommand(string command)
        {
            if (!isReady) 
                throw new Exception("没有对应的游戏进程");
            
            if (command == "")
                throw new Exception("指令为空");
			string[] array = command.Split(new char[]
{
					' '
});
            switch (array[0])
            {
                case "/p":
				case "/say":
				case "/s":
				case "/yell":
                case "/y":
				case "/shout":
				case "/sh":
				case "/tell":
				case "/t":
				case "/party":
				case "/alliance":
				case "/a":
				case "/freecompany":
				case "/fc":
				case "/l":
				case "/beginner":

				case "/pvpteam":
				case "/linkshell":
				case "/cwlinkshellcmd":
                    throw new Exception("不支持对话命令");
				default:
                    break;
            }
            if (array[0].Contains("linkshell")|| array[0].Contains("cwlinkshell"))
            {
				throw new Exception("不支持对话命令");
			}
            var flag = false;
            try {
				var cmd = command;
				ChatBoxString cmdString;
				var cmdBytes = Encoding.UTF8.GetBytes(cmd);
				var stringMemPtr = Memory.Memory.Allocate(cmdBytes.Length + 30);
				Memory.Memory.WriteBytes(stringMemPtr, cmdBytes);
				cmdString.StringPtr = stringMemPtr;
				cmdString.BufSize = 0x40;
				cmdString.StringLength = cmdBytes.Length + 1;
				cmdString.Ukn = 0;
				var stringMem = Memory.Memory.Allocate(400);
				Memory.Memory.Write<ChatBoxString>(stringMem, cmdString);
				Memory.Execute(0, ProcessChatBoxPtr, RaptureModule, stringMem, IntPtr.Zero, 0);


				//Monitor.Enter(assemblyLock, ref flag);
    //            var array = Encoding.UTF8.GetBytes(command);
    //            using AllocatedMemory allocatedMemory = Memory.CreateAllocatedMemory(400), allocatedMemory2 = Memory.CreateAllocatedMemory(array.Length + 30);
    //            allocatedMemory2.AllocateOfChunk("cmd", array.Length);
    //            allocatedMemory2.WriteBytes("cmd", array);
    //            allocatedMemory.AllocateOfChunk<IntPtr>("cmdAddress");
    //            allocatedMemory.AllocateOfChunk<long>("t1");
    //            allocatedMemory.AllocateOfChunk<long>("tLength");
    //            allocatedMemory.AllocateOfChunk<long>("t3");
    //            allocatedMemory.Write("cmdAddress", allocatedMemory2.Address);
    //            allocatedMemory.Write("t1", 0x40);
    //            allocatedMemory.Write("tLength", array.Length + 1);
    //            allocatedMemory.Write("t3", 0x00);
    //            _ = Memory.CallInjected64<int>(ProcessChatBoxPtr, RaptureModule, allocatedMemory.Address, UiModule);
            }
            finally {
               
            }
        }
    }
}