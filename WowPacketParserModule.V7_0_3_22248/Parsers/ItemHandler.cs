using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class ItemHandler
    {
        public static void ReadItemGemInstanceData(Packet packet, params object[] idx)
        {
            packet.ReadByte("Slot", idx);
            V6_0_2_19033.Parsers.ItemHandler.ReadItemInstance(packet, "Item", idx);
        }

        [Parser(Opcode.CMSG_CHANGE_BAG_SLOT_FLAG)]
        public static void HandleChangeBagSlotFlag(Packet packet)
        {
            packet.ReadInt32("unk1");
            packet.ReadInt32("unk2");
            packet.ReadBit("unk3");
        }

        [Parser(Opcode.SMSG_ITEM_PUSH_RESULT)]
        public static void HandleItemPushResult(Packet packet)
        {
            packet.ReadPackedGuid128("PlayerGUID");

            packet.ReadByte("Slot");

            packet.ReadInt32("SlotInBag");

            packet.ReadUInt32("QuestLogItemID");
            packet.ReadUInt32("Quantity");
            packet.ReadUInt32("QuantityInInventory");
            packet.ReadInt32("DungeonEncounterID");

            packet.ReadUInt32("BattlePetBreedID");
            packet.ReadUInt32("BattlePetBreedQuality");
            packet.ReadUInt32("BattlePetSpeciesID");
            packet.ReadUInt32("BattlePetLevel");

            packet.ReadPackedGuid128("ItemGUID");

            packet.ResetBitReader();

            packet.ReadBit("Pushed");
            packet.ReadBit("Created");
            packet.ReadBits("DisplayText", 3);
            packet.ReadBit("IsBonusRoll");
            packet.ReadBit("IsEncounterLoot");

            V6_0_2_19033.Parsers.ItemHandler.ReadItemInstance(packet, "ItemInstance");
        }

        [Parser(Opcode.CMSG_USE_ITEM)]
        public static void HandleUseItem(Packet packet)
        {
            packet.ReadByte("PackSlot");
            packet.ReadByte("Slot");
            packet.ReadPackedGuid128("CastItem");

            SpellHandler.ReadSpellCastRequest(packet, "Cast");
        }

        [Parser(Opcode.CMSG_USE_TOY)]
        public static void HandleUseToy(Packet packet)
        {
            packet.ReadInt32<ItemId>("ItemID");
            SpellHandler.ReadSpellCastRequest(packet, "Cast");
        }

        [Parser(Opcode.SMSG_CHARACTER_ITEM_FIXUP)]
        public static void HandleCharacterItemFixup(Packet packet)
        {
            packet.ReadBit("unk1");
            packet.ReadBit("unk2");
        }

        [Parser(Opcode.SMSG_SET_ITEM_PURCHASE_DATA)]
        public static void HandleSetItemPurchaseData(Packet packet)
        {
            packet.ReadPackedGuid128("ItemGUID");
            packet.ReadInt32("Flags");
            packet.ReadInt32("PurchaseTime");
        }
    }
}
