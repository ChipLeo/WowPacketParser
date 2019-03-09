using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ArtifactPowerLink, HasIndexInData = false)]
    public class ArtifactPowerLinkEntry
    {
        public ushort FromArtifactPowerID { get; set; }
        public ushort ToArtifactPowerID { get; set; }
    }
}
