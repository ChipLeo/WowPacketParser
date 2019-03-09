using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ImportPriceQuality, HasIndexInData = false)]
    public class ImportPriceQualityEntry
    {
        public float Factor { get; set; }
    }
}
