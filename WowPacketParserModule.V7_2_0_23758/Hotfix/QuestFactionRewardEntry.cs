using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.QuestFactionReward, HasIndexInData = false)]
    public class QuestFactionRewardEntry
    {
        [HotfixArray(10)]
        public short[] QuestRewFactionValue { get; set; }
    }
}
