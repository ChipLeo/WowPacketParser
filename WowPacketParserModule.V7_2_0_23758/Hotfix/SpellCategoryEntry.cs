using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.SpellCategory, HasIndexInData = false)]
    public class SpellCategoryEntry
    {
        public string Name { get; set; }
        public int ChargeRecoveryTime { get; set; }
        public byte Flags { get; set; }
        public byte UsesPerWeek { get; set; }
        public byte MaxCharges { get; set; }
        public uint ChargeCategoryType { get; set; }
    }
}
