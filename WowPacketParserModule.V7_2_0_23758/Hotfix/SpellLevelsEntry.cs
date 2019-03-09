using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.SpellLevels, HasIndexInData = false)]
    public class SpellLevelsEntry
    {
        public uint SpellID { get; set; }
        public ushort BaseLevel { get; set; }
        public ushort MaxLevel { get; set; }
        public ushort SpellLevel { get; set; }
        public byte DifficultyID { get; set; }
        public byte MaxUsableLevel { get; set; }
    }
}
