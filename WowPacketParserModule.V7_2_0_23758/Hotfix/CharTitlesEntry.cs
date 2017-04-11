using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.CharTitles, HasIndexInData = false)]
    public class CharTitlesEntry
    {
        public string NameMale { get; set; }
        public string NameFemale { get; set; }
        public ushort MaskID { get; set; }
        public byte Flags { get; set; }
    }
}
