using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ItemChildEquipment, HasIndexInData = false)]
    public class ItemChildEquipmentEntry
    {
        public uint ItemID { get; set; }
        public uint AltItemID { get; set; }
        public byte AltEquipmentSlot { get; set; }
    }
}
