using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ItemDisenchantLoot, HasIndexInData = false)]
    public class ItemDisenchantLootEntry
    {
        public ushort MinItemLevel { get; set; }
        public ushort MaxItemLevel { get; set; }
        public ushort RequiredDisenchantSkill { get; set; }
        public byte ItemClass { get; set; }
        public sbyte ItemSubClass { get; set; }
        public byte ItemQuality { get; set; }
    }
}
