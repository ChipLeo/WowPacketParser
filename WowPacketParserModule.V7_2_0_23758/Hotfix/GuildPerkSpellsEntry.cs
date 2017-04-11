using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.GuildPerkSpells, HasIndexInData = false)]
    public class GuildPerkSpellsEntry
    {
        public uint SpellID { get; set; }
    }
}
