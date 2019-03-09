using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.TactKey, HasIndexInData = false)]
    public class TactKeyEntry
    {
        [HotfixArray(16)]
        public byte[] Key { get; set; }
    }
}
