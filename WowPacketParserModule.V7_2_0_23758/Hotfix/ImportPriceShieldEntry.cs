using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ImportPriceShield, HasIndexInData = false)]
    public class ImportPriceShieldEntry
    {
        public float Factor { get; set; }
    }
}
