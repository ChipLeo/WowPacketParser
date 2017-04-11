using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ItemPriceBase, HasIndexInData = false)]
    public class ItemPriceBaseEntry
    {
        public float ArmorFactor { get; set; }
        public float WeaponFactor { get; set; }
        public ushort ItemLevel { get; set; }
    }
}
