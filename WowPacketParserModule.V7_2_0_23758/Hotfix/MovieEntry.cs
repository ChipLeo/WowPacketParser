using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.Movie, HasIndexInData = false)]
    public class MovieEntry
    {
        public uint AudioFileDataID { get; set; }
        public uint SubtitleFileDataID { get; set; }
        public byte Volume { get; set; }
        public byte KeyID { get; set; }
    }
}
