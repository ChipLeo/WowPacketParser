using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.QuestXp, HasIndexInData = false)]
    public class QuestXPEntry
    {
        [HotfixArray(10)]
        public ushort[] Exp { get; set; }
    }
}
