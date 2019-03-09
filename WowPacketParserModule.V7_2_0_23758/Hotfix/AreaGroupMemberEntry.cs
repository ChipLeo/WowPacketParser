using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.AreaGroupMember, HasIndexInData = false)]
    public class AreaGroupMemberEntry
    {
        public ushort AreaGroupID { get; set; }
        public ushort AreaID { get; set; }
    }
}
