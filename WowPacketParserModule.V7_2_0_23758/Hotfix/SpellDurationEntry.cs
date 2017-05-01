using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.SpellDuration, HasIndexInData = false)]
    public class SpellDurationEntry
    {
        public int Duration { get; set; }
        public int MaxDuration { get; set; }
        public uint DurationPerLevel { get; set; }
    }
}
