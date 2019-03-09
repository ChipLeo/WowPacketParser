using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.Phase, HasIndexInData = false)]
    public class PhaseEntry
    {
        public ushort Flags { get; set; }
    }
}
