using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.SpellInterrupts, HasIndexInData = false)]
    public class SpellInterruptsEntry
    {
        public uint SpellID { get; set; }
        [HotfixArray(2)]
        public uint[] AuraInterruptFlags { get; set; }
        [HotfixArray(2)]
        public uint[] ChannelInterruptFlags { get; set; }
        public ushort InterruptFlags { get; set; }
        public byte DifficultyID { get; set; }
    }
}
