using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ItemCurrencyCost, HasIndexInData = false)]
    public class ItemCurrencyCostEntry
    {
        public uint ItemId { get; set; }
    }
}
