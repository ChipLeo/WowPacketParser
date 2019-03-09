using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.PowerDisplay, HasIndexInData = false)]
    public class PowerDisplayEntry
    {
        public string GlobalStringBaseTag { get; set; }
        public byte PowerType { get; set; }
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
    }
}
