using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_0_3_22248.Hotfix
{
    [HotfixStructure(DB2Hash.DungeonEncounter, HasIndexInData = false)]
    public class DungeonEncounterEntry
    {
        public string Name { get; set; }
        public uint CreatureDisplayID { get; set; }
        public ushort MapID { get; set; }
        public ushort SpellIconID { get; set; }
        public byte DifficultyID { get; set; }
        public byte Bit { get; set; }
        public byte Flags { get; set; }
        public uint OrderIndex { get; set; }
    }
}
