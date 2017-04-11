using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ItemModifiedAppearance)]
    public class ItemModifiedAppearanceEntry
    {
        public uint ItemID { get; set; }
        public ushort AppearanceID { get; set; }
        public byte AppearanceModID { get; set; }
        public byte Index { get; set; }
        public byte SourceType { get; set; }
        public uint ID { get; set; }
    }
}
