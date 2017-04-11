using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.DurabilityQuality, HasIndexInData = false)]
    public class DurabilityQualityEntry
    {
        public float QualityMod { get; set; }
    }
}
