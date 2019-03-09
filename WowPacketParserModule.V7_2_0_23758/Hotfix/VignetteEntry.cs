using System;
using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.Vignette)]
    public class VignetteEntry
    {
        public string Name { get; set; }
        public Single PosX { get; set; }
        public Single PosY { get; set; }
        public ushort Icon { get; set; }
        public byte Flags { get; set; }
        public uint PlayerConditionID { get; set; }
        public uint Unk1 { get; set; }
    }
}
