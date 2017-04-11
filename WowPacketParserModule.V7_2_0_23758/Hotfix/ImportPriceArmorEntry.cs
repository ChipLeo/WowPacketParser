using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ImportPriceArmor, HasIndexInData = false)]
    public class ImportPriceArmorEntry
    {
        public float ClothFactor { get; set; }
        public float LeatherFactor { get; set; }
        public float MailFactor { get; set; }
        public float PlateFactor { get; set; }
    }
}
