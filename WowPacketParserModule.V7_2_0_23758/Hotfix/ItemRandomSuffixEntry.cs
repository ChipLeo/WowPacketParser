using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ItemRandomSuffix, HasIndexInData = false)]
    public class ItemRandomSuffixEntry
    {
        public string Name { get; set; }
        [HotfixArray(5)]
        public ushort[] Enchantment { get; set; }
        [HotfixArray(5)]
        public ushort[] AllocationPct { get; set; }
    }
}
