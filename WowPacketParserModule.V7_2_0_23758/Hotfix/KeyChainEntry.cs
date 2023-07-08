using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.Keychain, HasIndexInData = false)]
    public class KeyChainEntry
    {
        [HotfixArray(32)]
        public byte[] Key { get; set; }
    }
}
