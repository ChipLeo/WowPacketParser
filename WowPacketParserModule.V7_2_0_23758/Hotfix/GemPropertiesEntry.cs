using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.GemProperties, HasIndexInData = false)]
    public class GemPropertiesEntry
    {
        public uint Type { get; set; }
        public ushort EnchantID { get; set; }
        public ushort MinItemLevel { get; set; }
    }
}
