using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.BankBagSlotPrices, HasIndexInData = false)]
    public class BankBagSlotPricesEntry
    {
        public uint Cost { get; set; }
    }
}
