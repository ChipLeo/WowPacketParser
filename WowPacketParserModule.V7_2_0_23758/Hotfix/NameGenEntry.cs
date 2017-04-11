using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.NameGen, HasIndexInData = false)]
    public class NameGenEntry
    {
        public string Name { get; set; }
        public byte Race { get; set; }
        public byte Sex { get; set; }
    }
}
