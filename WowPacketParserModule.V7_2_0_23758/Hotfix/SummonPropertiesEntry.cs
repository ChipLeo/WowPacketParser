using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.SummonProperties, HasIndexInData = false)]
    public class SummonPropertiesEntry
    {
        public uint Category { get; set; }
        public uint Faction { get; set; }
        public uint Type { get; set; }
        public int Slot { get; set; }
        public uint Flags { get; set; }
    }
}
