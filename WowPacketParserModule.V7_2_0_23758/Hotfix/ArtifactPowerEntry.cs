using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ArtifactPower)]
    public class ArtifactPowerEntry
    {
        [HotfixArray(2)]
        public float[] Pos { get; set; }
        public byte ArtifactID { get; set; }
        public byte Flags { get; set; }
        public byte MaxRank { get; set; }
        public uint ID { get; set; }
        public int RelicType { get; set; }
    }
}
