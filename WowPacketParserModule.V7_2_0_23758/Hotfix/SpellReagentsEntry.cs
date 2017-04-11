using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.SpellReagents, HasIndexInData = false)]
    public class SpellReagentsEntry
    {
        public uint SpellID { get; set; }
        [HotfixArray(8)]
        public int[] Reagent { get; set; }
        [HotfixArray(8)]
        public ushort[] ReagentCount { get; set; }
    }
}
