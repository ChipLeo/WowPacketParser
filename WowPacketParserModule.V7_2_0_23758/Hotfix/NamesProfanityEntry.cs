using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.NamesProfanity, HasIndexInData = false)]
    public class NamesProfanityEntry
    {
        public string Name { get; set; }
        public sbyte Language { get; set; }
    }
}
