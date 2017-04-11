using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ItemSpecOverride, HasIndexInData = false)]
    public class ItemSpecOverrideEntry
    {
        public uint ItemID { get; set; }
        public ushort SpecID { get; set; }
    }
}
