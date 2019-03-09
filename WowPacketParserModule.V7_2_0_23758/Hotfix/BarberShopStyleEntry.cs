using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.BarberShopStyle)]
    public class BarberShopStyleEntry
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public float CostModifier { get; set; }
        public byte Type { get; set; }
        public byte Race { get; set; }
        public byte Sex { get; set; }
        public byte Data { get; set; }
        public uint ID { get; set; }
    }
}
