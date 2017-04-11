using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ScalingStatDistribution, HasIndexInData = false)]
    public class ScalingStatDistributionEntry
    {
        public ushort ItemLevelCurveID { get; set; }
        public uint MinLevel { get; set; }
        public uint MaxLevel { get; set; }
    }
}
