using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.Curve, HasIndexInData = false)]
    public class CurveEntry
    {
        public byte Type { get; set; }
        public byte Unused { get; set; }
    }
}
