using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ItemLimitCategory, HasIndexInData = false)]
    public class ItemLimitCategoryEntry
    {
        public string Name { get; set; }
        public byte Quantity { get; set; }
        public byte Flags { get; set; }
    }
}
