using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using PostNamazu.Attributes;
using PostNamazu.Common;
using PostNamazu.Models;
using System.Runtime.InteropServices;

namespace PostNamazu.Actions
{
    internal class Mark : NamazuModule
    {
        private IntPtr MarkingFunc;
        private IntPtr LocalMarkingFunc;
        private IntPtr MarkingController;
        private unsafe delegate byte MarkingFuncDelegate(IntPtr a,MarkType b,uint c);
        private MarkingFuncDelegate markingFuncDelegate;
        private unsafe delegate byte LocalMarkingFuncDelegate(IntPtr a, MarkType b, uint c,int d);
        private LocalMarkingFuncDelegate localMarkingFuncDelegate;

        public override void GetOffsets() {
            base.GetOffsets();

            //char __fastcall sub_1407A6A60(__int64 g_MarkingController, __int64 MarkType, __int64 ActorID)
            MarkingFunc = base.SigScanner.ScanText("E8 ?? ?? ?? ?? 84 C0 74 ?? 48 8B 06 48 8B CE FF 50 ?? 48 8B C8 BA ?? ?? ?? ?? E8 ?? ?? ?? ?? 33 C0 E9");
            LocalMarkingFunc = base.SigScanner.ScanText("E8 ?? ?? ?? ?? 4C 8B C5 8B D7 48 8B CB E8");
            MarkingController = SigScanner.GetStaticAddressFromSig("48 8D 0D ?? ?? ?? ?? 4C 8B 85", 3);//正确
            markingFuncDelegate = Marshal.GetDelegateForFunctionPointer<MarkingFuncDelegate>(MarkingFunc);
            localMarkingFuncDelegate= Marshal.GetDelegateForFunctionPointer<LocalMarkingFuncDelegate>(LocalMarkingFunc);
        }
        //反正没人用,不如重构
        [Command("mark")]
        public void DoMarking(string command)
        {
            if (!isReady)
                throw new Exception("没有对应的游戏进程");

            if (command == "")
                throw new Exception("指令为空");

            var mark=JsonConvert.DeserializeObject<Marking>(command);
            if (mark?.MarkType == null) {
                throw new Exception("标记错误");
            }
            uint actorID = 0xE000000;
            actorID = mark.ActorID ?? GetActorIDByName(mark.Name);
            DoMarkingByActorID(actorID,mark.MarkType.Value,mark.LocalOnly);
        }
        private uint GetActorIDByName(string Name)
        {
            var combatant = FFXIV_ACT_Plugin.DataRepository.GetCombatantList().FirstOrDefault(i => i.Name != null && i.ID != 0xE0000000 && i.Name.Equals(Name));
            if (combatant == null) {
                throw new Exception($"未能找到{Name}");
            }
            return combatant.ID;
            //PluginUI.Log($"BNpcID={combatant.BNpcNameID},ActorID={combatant.ID:X},markingType={markingType}");
        }
        private void DoMarkingByActorID(uint ActorID, MarkType markingType, bool localOnly = false)
        {
            var combatant = FFXIV_ACT_Plugin.DataRepository.GetCombatantList().FirstOrDefault(i => i.ID == ActorID);
            if (ActorID != 0xE000000 &&  combatant == null) {
                throw new Exception($"未能找到{ActorID}");
            }
            var flag = false;
            try {
                if (!localOnly)
                    markingFuncDelegate( MarkingController, markingType, ActorID);
                else //本地标点的markingType从0开始，因此需要-1
                    localMarkingFuncDelegate( MarkingController, markingType - 1, ActorID, 0);
            }
            finally {
            }
        }
    }
}
