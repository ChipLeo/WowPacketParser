using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.PvpDifficulty, HasIndexInData = false)]
    public class PvPDifficultyEntry
    {
        public ushort MapID { get; set; }
        public byte BracketID { get; set; }
        public byte MinLevel { get; set; }
        public byte MaxLevel { get; set; }
    }
}
