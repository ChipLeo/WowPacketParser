using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ItemBonusListLevelDelta)]
    public class ItemBonusListLevelDeltaEntry
    {
        public short Delta { get; set; }
        public uint ID { get; set; }
    }
}
