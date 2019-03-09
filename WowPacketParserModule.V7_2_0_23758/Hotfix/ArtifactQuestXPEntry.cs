using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ArtifactQuestXP, HasIndexInData = false)]
    public class ArtifactQuestXPEntry
    {
        [HotfixArray(10)]
        public uint[] Exp { get; set; }
    }
}
