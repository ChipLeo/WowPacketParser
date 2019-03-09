using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.RulesetItemUpgrade, HasIndexInData = false)]
    public class RulesetItemUpgradeEntry
    {
        public uint ItemID { get; set; }
        public ushort ItemUpgradeID { get; set; }
    }
}
