using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.CinematicSequences, HasIndexInData = false)]
    public class CinematicSequencesEntry
    {
        public uint SoundID { get; set; }
        [HotfixArray(8)]
        public ushort[] Camera { get; set; }
    }
}
