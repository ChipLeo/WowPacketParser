using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.NamesReservedLocale, HasIndexInData = false)]
    public class NamesReservedLocaleEntry
    {
        public string Name { get; set; }
        public byte LocaleMask { get; set; }
    }
}
