using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.SpellPowerDifficulty)]
    public class SpellPowerDifficultyEntry
    {
        public byte DifficultyID { get; set; }
        public byte PowerIndex { get; set; }
        public uint ID { get; set; }
    }
}
