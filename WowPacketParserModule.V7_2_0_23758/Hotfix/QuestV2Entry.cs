using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.QuestV2, HasIndexInData = false)]
    public class QuestV2Entry
    {
        public ushort UniqueBitFlag { get; set; }
    }
}
