using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ItemBagFamily, HasIndexInData = false)]
    public class ItemBagFamilyEntry
    {
        public string Name { get; set; }
    }
}
