using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.DurabilityCosts, HasIndexInData = false)]
    public class DurabilityCostsEntry
    {
        [HotfixArray(21)]
        public ushort[] WeaponSubClassCost { get; set; }
        [HotfixArray(8)]
        public ushort[] ArmorSubClassCost { get; set; }
    }
}
