using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ItemRandomProperties, HasIndexInData = false)]
    public class ItemRandomPropertiesEntry
    {
        public string Name { get; set; }
        [HotfixArray(5)]
        public ushort[] Enchantment { get; set; }
    }
}
