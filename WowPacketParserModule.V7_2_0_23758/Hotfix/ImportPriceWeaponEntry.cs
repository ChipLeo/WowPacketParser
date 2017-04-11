using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ImportPriceWeapon, HasIndexInData = false)]
    public class ImportPriceWeaponEntry
    {
        public float Factor { get; set; }
    }
}
