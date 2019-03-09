using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ItemSet, HasIndexInData = false)]
    public class ItemSetEntry
    {
        public string Name { get; set; }
        [HotfixArray(17)]
        public uint[] ItemID { get; set; }
        public ushort RequiredSkillRank { get; set; }
        public uint RequiredSkill { get; set; }
        public uint Flags { get; set; }
    }
}
