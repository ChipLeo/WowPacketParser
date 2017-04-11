using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.NamesReserved, HasIndexInData = false)]
    public class NamesReservedEntry
    {
        public string Name { get; set; }
    }
}
