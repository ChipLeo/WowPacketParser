using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.SpellFocusObject, HasIndexInData = false)]
    public class SpellFocusObjectEntry
    {
        public string Name { get; set; }
    }
}
