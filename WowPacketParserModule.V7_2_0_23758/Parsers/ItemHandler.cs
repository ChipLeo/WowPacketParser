﻿using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class ItemHandler
    {
        //650932 22996
        public static void ReadBonuses(Packet packet, params object[] idx)
        {
            packet.ReadByte("Context", idx);
            ReadBonusList(packet, idx);
        }

        //650963 22996
        public static void ReadBonusList(Packet packet, params object[] idx)
        {
            var bonusCount = packet.ReadUInt32();
            for (var j = 0; j < bonusCount; ++j)
                packet.ReadUInt32("BonusListID", idx, j);
        }

        //6509B1 22996
        public static byte sub_6509B1(Packet packet, params object[] idx)
        {
            packet.ReadInt32("unk1", idx);
            packet.ReadInt32("unk2", idx);
            packet.ReadInt32("unk3", idx);
            return packet.ReadByte("unk4", idx);
        }

        //650B42 22996
        public static int ReadItemInstance(Packet packet, params object[] indexes)
        {
            var itemId = packet.ReadInt32<ItemId>("ItemID", indexes);
            packet.ReadUInt32("RandomPropertiesSeed", indexes);
            packet.ReadUInt32("RandomPropertiesID", indexes);

            packet.ResetBitReader();

            var hasBonuses = packet.ReadBit("HasItemBonus", indexes);
            var hasModifications = packet.ReadBit("HasModifications", indexes);
            if (hasBonuses)
                ReadBonuses(packet, indexes);

            if (hasModifications)
            {//0064E5B1 22996 not decoded
                var mask = packet.ReadUInt32();
                for (var j = 0; mask != 0; mask >>= 1, ++j)
                    if ((mask & 1) != 0)
                        packet.ReadInt32(((ItemModifier)j).ToString(), indexes);
            }

            packet.ResetBitReader();

            return itemId;
        }

        public static void ReadItemGemInstanceData(Packet packet, params object[] idx)
        {
            packet.ReadByte("Slot", idx);
            Parsers.ItemHandler.ReadItemInstance(packet, "Item", idx);
        }

        public static void ReadItemPurchaseContents(Packet packet, params object[] indexes)
        {
            packet.ReadInt64("Money");

            for (int i = 0; i < 5; i++)
                ReadItemPurchaseRefundItem(packet, indexes, i, "ItemPurchaseRefundItem");

            for (int i = 0; i < 5; i++)
                ReadItemPurchaseRefundItem(packet, indexes, i, "ItemPurchaseRefundCurrency");
        }

        public static void ReadItemPurchaseRefundItem(Packet packet, params object[] indexes)
        {
            packet.ReadInt32<ItemId>("ItemID", indexes);
            packet.ReadInt32("ItemCount", indexes);
        }

        [Parser(Opcode.CMSG_CHANGE_BAG_SLOT_FLAG)]
        public static void HandleChangeBagSlotFlag(Packet packet)
        {
            packet.ReadInt32("unk1");
            packet.ReadInt32("unk2");
            packet.ReadBit("unk3");
        }

        [Parser(Opcode.CMSG_AUTOBANK_ITEM)]
        [Parser(Opcode.CMSG_AUTOBANK_REAGENT)]
        [Parser(Opcode.CMSG_AUTO_EQUIP_ITEM)]
        [Parser(Opcode.CMSG_AUTOSTORE_BANK_ITEM)]
        [Parser(Opcode.CMSG_AUTOSTORE_BANK_REAGENT)]
        [Parser(Opcode.CMSG_SWAP_INV_ITEM)]
        public static void HandleAutoItem(Packet packet)
        {
            var bits2 = packet.ReadBits("InvItemCount", 2);
            for (int i = 0; i < bits2; i++)
            {
                packet.ReadByte("ContainerSlot", i);
                packet.ReadByte("Slot", i);
            }

            packet.ReadByte("Slot");
            packet.ReadByte("PackSlot");
        }

        [Parser(Opcode.CMSG_LOOT_ITEM)]
        public static void HandleAutoStoreLootItem(Packet packet)
        {
            var int16 = packet.ReadInt32("Count");

            for (var i = 0; i < int16; ++i)
            {
                packet.ReadPackedGuid128("LootObj", i);
                packet.ReadByte("Slot", i);
            }
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

            Parsers.ItemHandler.ReadItemInstance(packet, "ItemInstance");
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

        [Parser(Opcode.SMSG_SET_ITEM_PURCHASE_DATA)]
        public static void HandleSetItemPurchaseData(Packet packet)
        {
            packet.ReadPackedGuid128("ItemGUID");
            ReadItemPurchaseContents(packet, "ItemPurchaseContents");
            packet.ReadInt32("Flags");
            packet.ReadInt32("PurchaseTime");
        }

        [Parser(Opcode.SMSG_SOCKET_GEMS)]
        public static void HandleSocketGemsResult(Packet packet)
        {
            packet.ReadPackedGuid128("Item");
        }
    }
}
