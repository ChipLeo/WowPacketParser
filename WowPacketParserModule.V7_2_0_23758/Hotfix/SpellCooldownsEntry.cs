using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.SpellCooldowns, HasIndexInData = false)]
    public class SpellCooldownsEntry
    {
        public uint SpellID { get; set; }
        public uint CategoryRecoveryTime { get; set; }
        public uint RecoveryTime { get; set; }
        public uint StartRecoveryTime { get; set; }
        public byte DifficultyID { get; set; }
    }
}
