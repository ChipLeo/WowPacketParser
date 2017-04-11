using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ArmorLocation, HasIndexInData = false)]
    public class ArmorLocationEntry
    {
        [HotfixArray(5)]
        public float[] Modifier { get; set; }
    }
}
