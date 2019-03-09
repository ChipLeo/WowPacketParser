using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.QuestSort, HasIndexInData = false)]
    public class QuestSortEntry
    {
        public string SortName { get; set; }
        public byte SortOrder { get; set; }
    }
}
