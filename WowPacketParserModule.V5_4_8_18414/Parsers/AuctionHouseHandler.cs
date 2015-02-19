using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class AuctionHouseHandler
    {
        [Parser(Opcode.CMSG_AUCTION_LIST_BIDDER_ITEMS)]
        public static void HandleAuctionListBidderItems(Packet packet)
        {
            packet.ReadUInt32("List From"); // 40
            var guid = new byte[8];
            guid[3] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var count = packet.ReadBits("Outbidded Count", 7);
            guid[7] = packet.ReadBit();
            guid[0] = packet.ReadBit();

            packet.ParseBitStream(guid, 3, 4, 1, 0, 2, 5);

            for (var i = 0; i < count; i++)
                packet.ReadUInt32("Auction Id", i);

            packet.ParseBitStream(guid, 7, 6);

            packet.WriteGuid("Auctioneer GUID", guid);
        }

        [Parser(Opcode.CMSG_AUCTION_LIST_ITEMS)]
        public static void HandleAuctionListItems(Packet packet)
        {
            packet.ReadInt32("unk40"); // 40
            packet.ReadInt32("unk28"); // 28
            packet.ReadInt32("unk32"); // 32
            packet.ReadByte("unk24"); // 24
            packet.ReadByte("unk25"); // 25
            packet.ReadByte("unk36"); // 36
            packet.ReadInt32("unk48"); // 48
            packet.ReadInt32("unk44"); // 44
            var unk328 = packet.ReadInt32("unk328"); // 328
            packet.ReadBytes("unk", unk328);
            var guid = new byte[8];
            guid[3] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            packet.ReadBit("unk308"); // 308
            packet.ReadBit("unk309"); // 309
            guid[7] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var unk52 = packet.ReadBits("unk52", 8); // 52
            guid[1] = packet.ReadBit();
            guid[6] = packet.ReadBit();

            packet.ParseBitStream(guid, 6, 3, 4, 0, 7, 2);
            packet.ReadWoWString("str2", unk52);
            packet.ParseBitStream(guid, 1, 5);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_AUCTION_LIST_OWNER_ITEMS)]
        public static void HandleAuctionListOwnerItems(Packet packet)
        {
            packet.ReadUInt32("List From");
            var guid = packet.StartBitStream(4, 5, 2, 1, 7, 0, 3, 6);
            packet.ParseBitStream(guid, 5, 7, 3, 6, 4, 2, 0, 1);

            packet.WriteGuid("Auctioneer GUID", guid);
        }

        [Parser(Opcode.CMSG_AUCTION_PLACE_BID)]
        public static void HandleAuctionPlaceBid(Packet packet)
        {
            packet.ReadUInt32("Auction Id");
            packet.ReadUInt64("Price");

            var guid = packet.StartBitStream(1, 6, 3, 7, 2, 4, 0, 5);
            packet.ParseBitStream(guid, 3, 2, 1, 4, 6, 5, 7, 0);
            packet.WriteGuid("Auctioneer GUID", guid);
        }

        [Parser(Opcode.CMSG_AUCTION_REMOVE_ITEM)]
        public static void HandleAuctionRemoveItem(Packet packet)
        {
            packet.ReadUInt32("Auction Id");
            var guid = packet.StartBitStream(6, 4, 2, 3, 7, 5, 1, 0);
            packet.ParseBitStream(guid, 3, 2, 6, 5, 4, 7, 0, 1);
            packet.WriteGuid("Auctioneer GUID", guid);
        }

        [Parser(Opcode.CMSG_AUCTION_SELL_ITEM)]
        public static void HandleAuctionSellItem(Packet packet)
        {
            packet.ReadUInt64("Bid"); // 16
            packet.ReadUInt64("Buyout"); // 24
            packet.ReadUInt32("Expire Time"); // 32

            var guid = new byte[8];

            guid[3] = packet.ReadBit();
            var count = packet.ReadBits("count", 5);
            guid[0] = packet.ReadBit();
            var guids = new byte[count][];
            for (var i = 0; i < count; i++)
                guids[i] = packet.StartBitStream(4, 6, 2, 3, 5, 7, 1, 0);
            guid[6] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();

            for (var i = 0; i < count; i++)
            {
                packet.ParseBitStream(guids[i], 3, 1);
                packet.ReadInt32("Count");
                packet.ParseBitStream(guids[i], 6, 4, 5, 0, 2, 7);
                packet.WriteGuid("Item", guids[i]);
            }

            packet.ParseBitStream(guid, 3, 7, 2, 5, 6, 1, 0, 4);

            packet.WriteGuid("Auctioneer GUID", guid);
        }

        [Parser(Opcode.SMSG_AUCTION_LIST_BIDDER_ITEMS_RESULT)]
        public static void HandleAuctionListBidderItemsResult(Packet packet)
        {
            var count = packet.ReadUInt32("Count");
            for (var i = 0; i < count; ++i)
            {
                packet.ReadUInt32("Auction Id", i);
                packet.ReadEntry<UInt32>(StoreNameType.Item, "Item Entry", i);

                int enchantmentCount = 8;
                for (var j = 0; j < enchantmentCount; ++j)
                {
                    packet.ReadUInt32("Item Enchantment ID", i, j);
                    packet.ReadUInt32("Item Enchantment Duration", i, j);
                    packet.ReadUInt32("Item Enchantment Charges", i, j);
                }

                packet.ReadUInt32("Unk UInt32 1", i);
                packet.ReadInt32("Item Random Property ID", i);
                packet.ReadUInt32("Item Suffix", i);
                packet.ReadUInt32("Item Count", i);
                packet.ReadInt32("Item Spell Charges", i);
                packet.ReadUInt32E<ItemProtoFlags>("Item Flags", i);
                packet.ReadGuid("Owner", i);
                packet.ReadValue("Start Bid", TypeCode.UInt64, i);
                packet.ReadValue("Out Bid", TypeCode.UInt64, i);
                packet.ReadValue("Buyout ", TypeCode.UInt64, i);
                packet.ReadUInt32("Time Left", i);
                packet.ReadGuid("Bidder", i);
                packet.ReadValue("Bid", TypeCode.UInt64, i);
            }

            packet.ReadUInt32("Total item count");
            packet.ReadUInt32("Desired delay time");
        }

        [Parser(Opcode.SMSG_AUCTION_BIDDER_NOTIFICATION)]
        public static void HandleAuctionBidderNotification(Packet packet)
        {
            var guid = packet.StartBitStream(5, 4, 7, 6, 0, 1, 2, 3);
            packet.ParseBitStream(guid, 7, 3);
            packet.ReadEntry<UInt32>(StoreNameType.Item, "Item Entry"); // 32
            packet.ParseBitStream(guid, 1, 2);
            packet.ReadInt32("Location"); // 24
            packet.ParseBitStream(guid, 0);
            packet.ReadInt32("unk36"); // 36
            packet.ParseBitStream(guid, 5, 4, 6);
            packet.ReadInt32("unk40"); // 40
            packet.ReadInt32("Auction ID"); // 28

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_AUCTION_COMMAND_RESULT)]
        public static void HandleAuctionCommandResult(Packet packet)
        {
            var unk32 = !packet.ReadBit("!unk32"); // 32
            var unk16 = !packet.ReadBit("!unk16"); // 16
            var unk24 = !packet.ReadBit("!unk24"); // 24
            var guid = packet.StartBitStream(1, 4, 0, 6, 3, 5, 2, 7);
            packet.ParseBitStream(guid, 3, 0, 7, 1, 4, 6, 5, 2);
            if (unk16)
                packet.ReadInt64("unk16"); // 16
            var error = packet.ReadUInt32E<AuctionHouseError>("Error"); // 44
            var action = packet.ReadUInt32E<AuctionHouseAction>("Action"); // 48

            packet.ReadUInt32("Auction ID"); // 52
            if (unk32)
                packet.ReadInt64("unk32"); // 32
            packet.ReadInt32("unk40"); // 40  =40 if bag

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_AUCTION_LIST_RESULT)]
        public static void HandleAuctionListResult(Packet packet)
        {
            var count = packet.ReadUInt32("Count");
            for (var i = 0; i < count; ++i)
            {
                packet.ReadUInt32("Auction Id", i);
                packet.ReadEntry<UInt32>(StoreNameType.Item, "Item Entry", i);

                int enchantmentCount = 8;
                for (var j = 0; j < enchantmentCount; ++j)
                {
                    packet.ReadUInt32("Item Enchantment ID", i, j);
                    packet.ReadUInt32("Item Enchantment Duration", i, j);
                    packet.ReadUInt32("Item Enchantment Charges", i, j);
                }

                packet.ReadUInt32("Unk UInt32 1", i);
                packet.ReadInt32("Item Random Property ID", i);
                packet.ReadUInt32("Item Suffix", i);
                packet.ReadUInt32("Item Count", i);
                packet.ReadInt32("Item Spell Charges", i);
                packet.ReadUInt32E<ItemProtoFlags>("Item Flags", i);
                packet.ReadGuid("Owner", i);
                packet.ReadValue("Start Bid", TypeCode.UInt64, i);
                packet.ReadValue("Out Bid", TypeCode.UInt64, i);
                packet.ReadValue("Buyout ", TypeCode.UInt64, i);
                packet.ReadUInt32("Time Left", i);
                packet.ReadGuid("Bidder", i);
                packet.ReadValue("Bid", TypeCode.UInt64, i);
            }

            packet.ReadUInt32("Total item count");
            packet.ReadUInt32("Desired delay time");
        }

        [Parser(Opcode.SMSG_AUCTION_OUTBID_NOTIFICATION)]
        public static void HandleAuctionOutbidNotification(Packet packet)
        {
            packet.ReadInt32("Auction ID"); // 44
            packet.ReadInt32("unk40"); // 40
            packet.ReadInt32("unk56"); // 56
            packet.ReadInt64("unk16"); // 16
            packet.ReadInt32("Item ID"); // 48
            packet.ReadInt32("unk52"); // 52
            packet.ReadInt64("unk24"); // 24

            var guid = packet.StartBitStream(2, 5, 0, 1, 4, 6, 3, 7);
            packet.ParseBitStream(guid, 4, 7, 3, 0, 1, 6, 2, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_AUCTION_OWNER_NOTIFICATION)]
        public static void HandleAuctionOwnerNotification(Packet packet)
        {
            packet.ReadInt32("unk40"); // 40
            packet.ReadInt32("unk32"); // 32
            packet.ReadInt32("unk36"); // 36
            packet.ReadInt32("unk44"); // 44
            packet.ReadSingle("Unk float 14h"); // 14h
            packet.ReadInt64("unk24"); // 24
            packet.ReadBit("unk16"); // 16
        }
    }
}
