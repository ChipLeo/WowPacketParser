using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class LootHandler
    {
        public static void ReadCurrenciesData(Packet packet, params object[] idx)
        {
            packet.ReadUInt32("CurrencyID", idx);
            packet.ReadUInt32("Quantity", idx);
            packet.ReadByte("LootListId", idx);

            packet.ResetBitReader();

            packet.ReadBits("UiType", 3, idx);
        }

        public static void ReadLootItem(Packet packet, params object[] indexes)
        {
            packet.ResetBitReader();

            packet.ReadBits("ItemType", 2, indexes);
            packet.ReadBits("ItemUiType", 3, indexes);
            packet.ReadBit("CanTradeToTapList", indexes);

            Parsers.ItemHandler.ReadItemInstance(packet, indexes, "ItemInstance");

            packet.ReadUInt32("Quantity", indexes);
            packet.ReadByte("LootItemType", indexes);
            packet.ReadByte("LootListID", indexes);
        }

        [Parser(Opcode.CMSG_LOOT_RELEASE)]
        public static void HandleLootRelease(Packet packet)
        {
            packet.ReadPackedGuid128("Object GUID");
        }

        [Parser(Opcode.SMSG_LOOT_RESPONSE)]
        public static void HandleLootResponse(Packet packet)
        {
            packet.ReadPackedGuid128("Owner");
            packet.ReadPackedGuid128("LootObj");
            packet.ReadByteE<LootError>("FailureReason");
            packet.ReadByteE<LootType>("AcquireReason");
            packet.ReadByteE<LootMethod>("LootMethod");
            packet.ReadByteE<ItemQuality>("Threshold");

            packet.ReadUInt32("Coins");

            var itemCount = packet.ReadUInt32("ItemCount");
            var currencyCount = packet.ReadUInt32("CurrencyCount");

            packet.ResetBitReader();

            packet.ReadBit("Acquired");
            packet.ReadBit("AELooting");
            packet.ReadBit("PersonalLooting");

            for (var i = 0; i < itemCount; ++i)
                ReadLootItem(packet, i, "LootItem");

            for (var i = 0; i < currencyCount; ++i)
                Parsers.LootHandler.ReadCurrenciesData(packet, i, "Currencies");
        }

        [Parser(Opcode.SMSG_START_LOOT_ROLL)]
        public static void HandleLootStartRoll(Packet packet)
        {
            packet.ReadPackedGuid128("LootObj");
            packet.ReadInt32<MapId>("MapID");
            packet.ReadUInt32("RollTime");
            packet.ReadByte("ValidRolls");
            packet.ReadByteE<LootMethod>("Method");
            ReadLootItem(packet, "LootItem");
        }

        [Parser(Opcode.SMSG_LOOT_REMOVED)]
        public static void HandleLootRemoved(Packet packet)
        {
            packet.ReadPackedGuid128("Loot Owner");
            packet.ReadPackedGuid128("LootObj");
            packet.ReadByte("LootListId");
        }

        [Parser(Opcode.SMSG_LOOT_ROLL)]
        public static void HandleLootRollServer(Packet packet)
        {
            packet.ReadPackedGuid128("LootObj");
            packet.ReadPackedGuid128("Player");
            packet.ReadInt32("Roll");
            packet.ReadByte("RollType");
            ReadLootItem(packet, "LootItem");
            packet.ReadBit("Autopassed");
        }

        [Parser(Opcode.SMSG_LOOT_ROLL_WON)]
        public static void HandleLootRollWon(Packet packet)
        {
            packet.ReadPackedGuid128("LootObj");
            packet.ReadPackedGuid128("Player");
            packet.ReadInt32("Roll");
            packet.ReadByte("RollType");
            ReadLootItem(packet, "LootItem");
            packet.ReadBit("MainSpec");
        }

        [Parser(Opcode.SMSG_LOOT_ALL_PASSED)]
        public static void HandleLootAllPassed(Packet packet)
        {
            packet.ReadPackedGuid128("LootObj");
            ReadLootItem(packet, "LootItem");
        }
    }
}
