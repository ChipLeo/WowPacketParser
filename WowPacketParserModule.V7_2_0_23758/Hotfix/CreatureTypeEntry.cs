using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.CreatureType, HasIndexInData = false)]
    public class CreatureTypeEntry
    {
        public string Name { get; set; }
        public byte Flags { get; set; }
    }
}
