using System;
using WowPacketParser.Enums;
using WowPacketParser.Hotfix;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using WowPacketParserModule.V5_4_8_18414.Enums;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class ItemHandler
    {
        [Parser(Opcode.SMSG_ADD_ITEM_PASSIVE)]
        public static void HandleAddItemPassive(Packet packet)
        {
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.CMSG_AUTOBANK_ITEM)]
        public static void HandleAutoBankItem(Packet packet)
        {
            var unk1 = new byte[4];
            var unk2 = new byte[4];
            packet.ReadByte("Slot");
            packet.ReadSByte("Bag");
            var cnt = packet.ReadBits("Count", 2);
            for (var i = 0; i < cnt; i++)
            {
                unk1[i] = packet.ReadBit("unk1", i);
                unk2[i] = packet.ReadBit("unk2", i);
            }
            for (var j = 0; j < cnt; j++)
            {
                if (unk1[j]>0)
                    packet.ReadByte("Byte1", j);
                if (unk2[j]>0)
                    packet.ReadByte("Byte2", j);
            }
        }

        [Parser(Opcode.CMSG_AUTO_EQUIP_ITEM)]
        public static void HandleAutoEquipItem(Packet packet)
        {
            packet.ReadByte("Slot");
            packet.ReadSByte("Bag");

            var bits14 = (int)packet.ReadBits(2);

            var hasSlot = new bool[bits14];
            var hasBag = new bool[bits14];

            for (var i = 0; i < bits14; i++)
            {
                hasBag[i] = !packet.ReadBit();
                hasSlot[i] = !packet.ReadBit();
            }

            for (var i = 0; i < bits14; i++)
            {
                if (hasSlot[i])
                    packet.ReadByte("Slot", i);
                if (hasBag[i])
                    packet.ReadSByte("Bag", i);
            }
        }

        [Parser(Opcode.CMSG_AUTO_STORE_BAG_ITEM)]
        public static void HandleAutoStoreBagItem(Packet packet)
        {
            packet.ReadByte("Slot"); // 16
            packet.ReadByte("unk36"); // 36
            packet.ReadSByte("Bag"); // 17

            var bits14 = (int)packet.ReadBits(2);

            var hasSlot = new bool[bits14];
            var hasBag = new bool[bits14];

            for (var i = 0; i < bits14; i++)
            {
                hasBag[i] = !packet.ReadBit(); // 96
                hasSlot[i] = !packet.ReadBit(); // 97
            }

            for (var i = 0; i < bits14; i++)
            {
                if (hasSlot[i])
                    packet.ReadByte("Slot", i);
                if (hasBag[i])
                    packet.ReadSByte("Bag", i);
            }
        }

        [Parser(Opcode.CMSG_AUTOSTORE_BANK_ITEM)]
        public static void HandleAutostoreBankItem(Packet packet)
        {
            var hasByte1 = new bool[4];
            var hasByte2 = new bool[4];
            packet.ReadByte("Slot");
            packet.ReadSByte("Bag");
            var cnt = packet.ReadBits("Count", 2);
            for (var i = 0; i < cnt; i++)
            {
                hasByte1[i] = packet.ReadBit("has byte1", i);
                hasByte2[i] = packet.ReadBit("has byte2", i);
            }
            for (var i = 0; i < cnt; i++)
            {
                if (hasByte1[i])
                    packet.ReadByte("Byte1", i);
                if (hasByte2[i])
                    packet.ReadByte("Byte2", i);
            }
        }

        [Parser(Opcode.CMSG_AUTOSTORE_LOOT_ITEM)]
        public static void HandleAutoStoreLootItem(Packet packet)
        {
            var counter = packet.ReadBits("Count", 23);

            var guid = new byte[counter][];

            for (var i = 0; i < counter; i++)
                guid[i] = packet.StartBitStream(2, 7, 0, 6, 5, 3, 1, 4);

            packet.ResetBitReader();

            for (var i = 0; i < counter; i++)
            {
                packet.ParseBitStream(guid[i], 0, 4, 1, 7, 6, 5, 3, 2);
                packet.ReadByte("Slot", i);

                packet.WriteGuid("Lootee GUID", guid[i], i);
            }
        }

        [Parser(Opcode.CMSG_BUY_ITEM)]
        public static void HandleBuyItem(Packet packet)
        {
            packet.ReadUInt32("Bag Slot"); // 44
            packet.ReadUInt32("Item Count"); // 40
            packet.ReadUInt32<ItemId>("Entry"); // 36
            packet.ReadUInt32("Vendor Slot"); // 32

            var guid1 = new byte[8];
            var guid2 = new byte[8];

            guid2[6] = packet.ReadBit();
            guid1[6] = packet.ReadBit();
            guid1[4] = packet.ReadBit();
            guid2[4] = packet.ReadBit();

            packet.ReadBits("Item Type", 2);

            guid2[0] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            guid1[3] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid2[5] = packet.ReadBit();
            guid1[2] = packet.ReadBit();
            guid2[1] = packet.ReadBit();
            guid1[7] = packet.ReadBit();
            guid2[2] = packet.ReadBit();
            guid1[1] = packet.ReadBit();
            guid1[0] = packet.ReadBit();
            guid1[5] = packet.ReadBit();

            packet.ReadXORByte(guid2, 5);
            packet.ReadXORByte(guid2, 0);
            packet.ReadXORByte(guid1, 3);
            packet.ReadXORByte(guid1, 1);
            packet.ReadXORByte(guid1, 6);
            packet.ReadXORByte(guid2, 2);
            packet.ReadXORByte(guid2, 7);
            packet.ReadXORByte(guid2, 6);
            packet.ReadXORByte(guid1, 0);
            packet.ReadXORByte(guid1, 5);
            packet.ReadXORByte(guid2, 4);
            packet.ReadXORByte(guid1, 2);
            packet.ReadXORByte(guid2, 3);
            packet.ReadXORByte(guid1, 7);
            packet.ReadXORByte(guid2, 1);
            packet.ReadXORByte(guid1, 4);

            packet.WriteGuid("Bag Guid", guid1);
            packet.WriteGuid("Vendor Guid", guid2);
        }

        [Parser(Opcode.CMSG_BUY_BACK_ITEM)]
        public static void HandleBuyBackItem(Packet packet)
        {
            packet.ReadUInt32("Slot");
            var guid = packet.StartBitStream(2, 3, 0, 4, 1, 7, 5, 6);
            packet.ParseBitStream(guid, 0, 6, 1, 7, 5, 2, 3, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_DESTROY_ITEM)]
        public static void HandleDestroyItem(Packet packet)
        {
            packet.ReadUInt32("Count"); // 16
            packet.ReadSByte("Bag"); // 21
            packet.ReadByte("Slot"); // 20
        }

        [Parser(Opcode.CMSG_GET_ITEM_PURCHASE_DATA)]
        public static void HandleGetItemPurchaseData(Packet packet)
        {
            var guid = packet.StartBitStream(1, 0, 3, 2, 7, 4, 5, 6);
            packet.ResetBitReader();
            packet.ParseBitStream(guid, 3, 7, 5, 1, 0, 6, 4, 2);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_OPEN_ITEM)]
        public static void HandleOpenItem(Packet packet)
        {
            packet.ReadByte("Bag");
            packet.ReadByte("Slot");
        }

        [Parser(Opcode.CMSG_READ_ITEM)]
        public static void HandleReadItem(Packet packet)
        {
            packet.ReadByte("Bag");
            packet.ReadByte("Slot");
        }

        [Parser(Opcode.CMSG_REFORGE_ITEM)]
        public static void HandleReforgeItem(Packet packet)
        {
            packet.ReadInt32("ReforgeEntry");
            packet.ReadInt32("Bag");
            packet.ReadInt32("Slot");

            var guid = packet.StartBitStream(1, 0, 5, 3, 4, 2, 7, 6);
            packet.ParseBitStream(guid, 4, 6, 3, 1, 2, 7, 0, 5);

            packet.WriteGuid("NPC Guid", guid);
        }

        [Parser(Opcode.SMSG_REMOVE_ITEM_PASSIVE)]
        public static void HandleRemoveItemPassive(Packet packet)
        {
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.CMSG_REPAIR_ITEM)]
        public static void HandleRepairItem(Packet packet)
        {
            var guid24 = new byte[8];
            var guid32 = new byte[8];
            guid24[2] = packet.ReadBit();
            guid24[5] = packet.ReadBit();
            guid32[3] = packet.ReadBit();
            packet.ReadBit("unk16"); // 16
            guid32[7] = packet.ReadBit();
            guid24[4] = packet.ReadBit();
            guid32[2] = packet.ReadBit();
            guid24[0] = packet.ReadBit();
            guid24[3] = packet.ReadBit();
            guid32[6] = packet.ReadBit();
            guid32[1] = packet.ReadBit();
            guid32[4] = packet.ReadBit();
            guid24[6] = packet.ReadBit();
            guid32[5] = packet.ReadBit();
            guid32[0] = packet.ReadBit();
            guid24[7] = packet.ReadBit();
            guid24[1] = packet.ReadBit();

            packet.ParseBitStream(guid24, 2);
            packet.ParseBitStream(guid32, 1);
            packet.ParseBitStream(guid24, 1);
            packet.ParseBitStream(guid32, 4, 7, 3, 2);
            packet.ParseBitStream(guid24, 7);
            packet.ParseBitStream(guid32, 5, 0);
            packet.ParseBitStream(guid24, 5, 3, 4, 6);
            packet.ParseBitStream(guid32, 6);
            packet.ParseBitStream(guid24, 0);

            packet.WriteGuid("Guid24", guid24);
            packet.WriteGuid("Guid32", guid32);
        }

        [Parser(Opcode.CMSG_REQUEST_HOTFIX)]
        public static void HandleItemRequestHotfix(Packet packet)
        {
            packet.ReadUInt32E<WowPacketParser.Enums.DB2Hash>("Type");

            var count = packet.ReadBits("Count", 21);

            var guid = new byte[count][];

            for (var i = 0; i < count; i++)
                guid[i] = packet.StartBitStream(6, 3, 0, 1, 4, 5, 7, 2);

            packet.ResetBitReader();

            for (var i = 0; i < count; i++)
            {
                packet.ParseBitStream(guid[i], 1);
                packet.ReadUInt32<ItemId>("Entry", i);
                packet.ParseBitStream(guid[i], 0, 5, 6, 4, 7, 2, 3);
                packet.WriteGuid("GUID", guid[i], i);
            }
        }

        [Parser(Opcode.CMSG_SELL_ITEM)]
        public static void HandleSellItem(Packet packet)
        {
            packet.ReadInt32("Count"); // 32
            var guid16 = new byte[8];
            var guid24 = new byte[8];
            guid16[4] = packet.ReadBit();
            guid16[3] = packet.ReadBit();
            guid16[7] = packet.ReadBit();
            guid24[6] = packet.ReadBit();
            guid24[5] = packet.ReadBit();
            guid24[1] = packet.ReadBit();
            guid16[5] = packet.ReadBit();
            guid16[2] = packet.ReadBit();
            guid16[1] = packet.ReadBit();
            guid24[2] = packet.ReadBit();
            guid16[6] = packet.ReadBit();
            guid24[4] = packet.ReadBit();
            guid24[0] = packet.ReadBit();
            guid24[7] = packet.ReadBit();
            guid24[3] = packet.ReadBit();
            guid16[0] = packet.ReadBit();

            packet.ParseBitStream(guid24, 6, 3, 1);
            packet.ParseBitStream(guid16, 1);
            packet.ParseBitStream(guid24, 2);
            packet.ParseBitStream(guid16, 7, 5);
            packet.ParseBitStream(guid24, 7);
            packet.ParseBitStream(guid16, 2);
            packet.ParseBitStream(guid24, 0, 5);
            packet.ParseBitStream(guid16, 3, 6);
            packet.ParseBitStream(guid24, 4);
            packet.ParseBitStream(guid16, 4, 0);

            packet.WriteGuid("Guid16", guid16);
            packet.WriteGuid("Guid24", guid24);
        }

        [Parser(Opcode.CMSG_SOCKET_GEMS)]
        public static void HandleSocketGems(Packet packet)
        {
            var guid2 = new byte[3][];
            var guid = new byte[8];

            for (var i = 0; i < 3; i++)
            {
                guid2[i] = new byte[8];
                guid2[i][4] = packet.ReadBit();
            }

            for (var i = 0; i < 3; i++)
                guid2[i][0] = packet.ReadBit();

            for (var i = 0; i < 3; i++)
                guid2[i][6] = packet.ReadBit();

            for (var i = 0; i < 3; i++)
                guid2[i][2] = packet.ReadBit();

            for (var i = 0; i < 3; i++)
                guid2[i][1] = packet.ReadBit();

            for (var i = 0; i < 3; i++)
                guid2[i][7] = packet.ReadBit();

            for (var i = 0; i < 3; i++)
                guid2[i][3] = packet.ReadBit();

            for (var i = 0; i < 3; i++)
                guid2[i][5] = packet.ReadBit();

            guid = packet.StartBitStream(5, 0, 6, 2, 3, 4, 7, 1);
            packet.ParseBitStream(guid, 7, 2, 6);

            for (var i = 0; i < 3; i++)
                packet.ReadXORByte(guid2[i], 6);

            for (var i = 0; i < 3; i++)
                packet.ReadXORByte(guid2[i], 4);

            for (var i = 0; i < 3; i++)
                packet.ReadXORByte(guid2[i], 3);

            for (var i = 0; i < 3; i++)
                packet.ReadXORByte(guid2[i], 2);

            for (var i = 0; i < 3; i++)
                packet.ReadXORByte(guid2[i], 0);

            for (var i = 0; i < 3; i++)
                packet.ReadXORByte(guid2[i], 1);

            for (var i = 0; i < 3; i++)
                packet.ReadXORByte(guid2[i], 7);

            for (var i = 0; i < 3; i++)
                packet.ReadXORByte(guid2[i], 5);

            packet.ParseBitStream(guid, 4, 3, 1, 5, 0);

            packet.WriteGuid("Guid", guid);
            for (var i = 0; i < 3; i++)
                packet.WriteGuid("Gem guid", guid2[i], i);
        }

        [Parser(Opcode.CMSG_SPLIT_ITEM)]
        public static void HandleSplitItem(Packet packet)
        {
            packet.ReadByte("unk21"); // 21
            packet.ReadInt32("unk16"); // 16
            packet.ReadByte("unk23"); // 23
            packet.ReadByte("unk22"); // 22
            packet.ReadByte("unk20"); // 20

            var unk24 = packet.ReadBits("unk24", 2); // 24
            var unk28 = new bool[unk24, 2];
            for (var i = 0; i < unk24; i++)
            {
                unk28[i, 0] = !packet.ReadBit("!unk28", i);
                unk28[i, 1] = !packet.ReadBit("!unk29", i);
            }
            for (var i = 0; i < unk24; i++)
            {
                if (unk28[i, 1])
                    packet.ReadByte("unk29", i);
                if (unk28[i, 0])
                    packet.ReadByte("unk28", i);
            }
        }

        [Parser(Opcode.CMSG_SWAP_INV_ITEM)]
        public static void HandleSwapInventoryItem(Packet packet)
        {
            packet.ReadByte("Slot 2"); // 17
            packet.ReadByte("Slot 1"); // 16

            var count = packet.ReadBits("Count", 2); // 20
            var hasBag = new bool[count];
            var hasSlot = new bool[count];
            for (var i = 0; i < count; i++)
            {
                hasSlot[i] = !packet.ReadBit("!hasSlot", i); // 25
                hasBag[i] = !packet.ReadBit("!hasBag", i); // 24
            }
            for (var i = 0; i < count; i++)
            {
                if (hasSlot[i])
                    packet.ReadByte("Slot", i);
                if (hasBag[i])
                    packet.ReadByte("Bag", i);
            }
        }

        [Parser(Opcode.CMSG_SWAP_ITEM)]
        public static void HandleSwapItem(Packet packet)
        {
            packet.ReadByte("Slot 1"); // 34
            packet.ReadByte("Bag 2"); // 33
            packet.ReadByte("Slot 2"); // 35
            packet.ReadByte("Bag 1"); // 32

            var count = packet.ReadBits("Count", 2); // 16
            var hasBag = new bool[count];
            var hasSlot = new bool[count];
            for (var i = 0; i < count; i++)
            {
                hasSlot[i] = !packet.ReadBit("!hasSlot", i); // 21
                hasBag[i] = !packet.ReadBit("!hasBag", i); // 20
            }
            for (var i = 0; i < count; i++)
            {
                if (hasBag[i])
                    packet.ReadByte("Bag", i);
                if (hasSlot[i])
                    packet.ReadByte("Slot", i);
            }
        }

        [Parser(Opcode.CMSG_TRANSMOGRIFY_ITEMS)]
        public static void HandleTransmogrifyItems(Packet packet)
        {
            var guid = new byte[8];
            guid[5] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            var count = packet.ReadBits("count", 21);
            var hasGuid16 = new bool[count];
            var hasGuid00 = new bool[count];
            var guid16 = new byte[count][];
            var guid00 = new byte[count][];
            for (var i = 0; i < count; i++)
            {
                hasGuid00[i] = packet.ReadBit("unk120", i); // 120
                hasGuid16[i] = packet.ReadBit("unk136", i); // 136
            }
            guid[0] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            for (var i = 0; i < count; i++)
            {
                if (hasGuid16[i])
                    guid16[i] = packet.StartBitStream(5, 6, 1, 3, 0, 4, 7, 2);
                if (hasGuid00[i])
                    guid00[i] = packet.StartBitStream(4, 1, 0, 6, 5, 2, 7, 3);
            }
            packet.ResetBitReader();
            for (var i = 0; i < count; i++)
            {
                packet.ReadInt32("unk148", i); // 148
                packet.ReadInt32("unk144", i); // 144
            }
            packet.ParseBitStream(guid, 5, 0, 1, 2, 3, 4, 6, 7);

            for (var i = 0; i < count; i++)
            {
                if (hasGuid16[i])
                {
                    packet.ParseBitStream(guid16[i], 2, 5, 4, 3, 6, 0, 7, 1);
                    packet.WriteGuid("Guid16", guid16[i], i);
                }
                if (hasGuid00[i])
                {
                    packet.ParseBitStream(guid00[i], 7, 1, 6, 5, 4, 3, 0, 2);
                    packet.WriteGuid("Guid00", guid00[i], i);
                }
            }
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_USE_ITEM)] // sub_696A02
        public static void HandleUseItem2(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_WRAP_ITEM)]
        public static void HandleWrapItem(Packet packet)
        {
            var count = packet.ReadBits("count", 2);
            var hasBag = new bool[count];
            var hasSlot = new bool[count];
            for (var i = 0; i < count; i++)
            {
                hasBag[i] = !packet.ReadBit("!hasBag", i); // 80
                hasSlot[i] = !packet.ReadBit("!hasSlot", i); // 81
            }
            packet.ResetBitReader();
            for (var i = 0; i < count; i++)
            {
                if (hasSlot[i])
                    packet.ReadByte("Slot", i);
                if (hasBag[i])
                    packet.ReadSByte("Bag", i);
            }
        }

        [Parser(Opcode.SMSG_BUY_FAILED)]
        public static void HandleBuyFailed(Packet packet)
        {
            var guid = packet.StartBitStream(6, 3, 1, 2, 4, 5, 0, 7);
            packet.ReadByteE<BuyResult>("Result");
            packet.ParseBitStream(guid, 2, 7);
            packet.ReadUInt32<ItemId>("Entry");
            packet.ParseBitStream(guid, 4, 5, 1, 3, 6, 0);
            packet.WriteGuid("Vendor", guid);
        }

        [Parser(Opcode.SMSG_BUY_SUCCEEDED)]
        public static void HandleBuyItemResponse(Packet packet)
        {
            var guid = packet.StartBitStream(3, 4, 7, 6, 0, 2, 1, 5);
            packet.ParseBitStream(guid, 6, 7);
            packet.ReadInt32("Count"); // 24
            packet.ParseBitStream(guid, 1, 3, 5, 2);
            packet.ReadInt32("MaxCount"); // 32
            packet.ParseBitStream(guid, 0, 4);
            packet.ReadInt32("VendorSlot"); // 28
            packet.WriteGuid("GUID", guid);
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_DB_REPLY)]
        public static void HandleDBReply(Packet packet)
        {
            var entry = packet.ReadInt32("Entry");
            packet.ReadTime("Hotfix date");
            var type = packet.ReadUInt32E<DB2Hash>("DB2 File");

            var size = packet.ReadInt32("Size");
            var data = packet.ReadBytes(size);
            var db2File = new Packet(data, packet.Opcode, packet.Time, packet.Direction, packet.Number, packet.Writer, packet.FileName);

            if (entry < 0)
            {
                packet.WriteLine("Row {0} has been removed.", -entry);
                return;
            }
            else
            {
                packet.AddSniffData(StoreNameType.None, entry, type.ToString());
                HotfixStoreMgr.AddRecord(type, entry, db2File);
                db2File.ClosePacket(false);
            }
        }

        [Parser(Opcode.SMSG_ENCHANTMENT_LOG)]
        public static void HandleEnchantementLog(Packet packet)
        {
            packet.ReadInt32("unk40"); // 40
            packet.ReadInt32("unk48"); // 48
            packet.ReadInt32("unk44"); // 44

            var guid16 = new byte[8];
            var guid24 = new byte[8];
            var guid32 = new byte[8];

            guid24[6] = packet.ReadBit();
            guid24[7] = packet.ReadBit();
            guid16[6] = packet.ReadBit();
            guid16[4] = packet.ReadBit();
            guid24[5] = packet.ReadBit();
            guid32[7] = packet.ReadBit();
            guid32[2] = packet.ReadBit();
            guid32[3] = packet.ReadBit();
            guid24[4] = packet.ReadBit();
            guid24[3] = packet.ReadBit();
            guid32[6] = packet.ReadBit();
            guid16[1] = packet.ReadBit();
            guid24[2] = packet.ReadBit();
            guid16[5] = packet.ReadBit();
            guid32[4] = packet.ReadBit();
            guid16[0] = packet.ReadBit();
            guid32[1] = packet.ReadBit();
            guid24[0] = packet.ReadBit();
            guid16[3] = packet.ReadBit();
            guid16[7] = packet.ReadBit();
            guid32[5] = packet.ReadBit();
            guid32[0] = packet.ReadBit();
            guid16[2] = packet.ReadBit();
            guid24[1] = packet.ReadBit();

            packet.ReadXORByte(guid24, 0);
            packet.ReadXORByte(guid16, 2);
            packet.ReadXORByte(guid32, 7);
            packet.ReadXORByte(guid24, 1);
            packet.ReadXORByte(guid16, 4);
            packet.ReadXORByte(guid32, 5);
            packet.ReadXORByte(guid24, 4);
            packet.ReadXORByte(guid32, 2);
            packet.ReadXORByte(guid16, 6);
            packet.ReadXORByte(guid16, 0);
            packet.ReadXORByte(guid32, 0);
            packet.ReadXORByte(guid32, 4);
            packet.ReadXORByte(guid24, 3);
            packet.ReadXORByte(guid16, 5);
            packet.ReadXORByte(guid32, 1);
            packet.ReadXORByte(guid16, 3);
            packet.ReadXORByte(guid16, 7);
            packet.ReadXORByte(guid24, 7);
            packet.ReadXORByte(guid32, 3);
            packet.ReadXORByte(guid24, 6);
            packet.ReadXORByte(guid24, 2);
            packet.ReadXORByte(guid24, 5);
            packet.ReadXORByte(guid32, 6);
            packet.ReadXORByte(guid16, 1);

            packet.WriteGuid("Guid16", guid16);
            packet.WriteGuid("Guid24", guid24);
            packet.WriteGuid("Guid32", guid32);
        }

        [Parser(Opcode.SMSG_INVENTORY_CHANGE_FAILURE)]
        public static void HandleInventoryChangeFailure(Packet packet)
        {
            var guid48 = new byte[8];
            var guid56 = new byte[8];

            guid56[4] = packet.ReadBit();
            guid48[3] = packet.ReadBit();
            guid56[6] = packet.ReadBit();
            guid56[2] = packet.ReadBit();
            guid48[4] = packet.ReadBit();
            guid56[5] = packet.ReadBit();
            guid48[1] = packet.ReadBit();
            guid48[6] = packet.ReadBit();
            guid56[0] = packet.ReadBit();
            guid56[3] = packet.ReadBit();
            guid56[1] = packet.ReadBit();
            guid48[2] = packet.ReadBit();
            guid48[0] = packet.ReadBit();
            guid48[5] = packet.ReadBit();
            guid48[7] = packet.ReadBit();
            guid56[7] = packet.ReadBit();

            packet.ParseBitStream(guid56, 0);
            packet.ReadByte("Bag"); // 16
            packet.ParseBitStream(guid56, 6);
            packet.ParseBitStream(guid48, 4, 0, 7, 3);
            packet.ParseBitStream(guid56, 1, 5);
            packet.ParseBitStream(guid48, 5);
            packet.ParseBitStream(guid56, 7, 2);
            packet.ParseBitStream(guid48, 1, 6, 2);
            packet.ParseBitStream(guid56, 3, 4);

            var result = packet.ReadByteE<InventoryResult>("Result"); // 17

            if (result == InventoryResult.ItemMaxLimitCategoryCountExceeded ||
                result == InventoryResult.ItemMaxLimitCategorySocketedExceeded ||
                result == InventoryResult.ItemMaxLimitCategoryEquippedExceeded)
                packet.ReadUInt32("Limit Category"); // 20

            if (result == InventoryResult.EventAutoEquipBindConfirm)
                packet.ReadInt32("unk40"); // 40

            if (result == InventoryResult.PurchaseLevelTooLow ||
                result == InventoryResult.CantEquipLevel)
                packet.ReadInt32("unk44"); // 44

            if (result == InventoryResult.EventAutoEquipBindConfirm)
            {
                var guid24 = new byte[8];
                var guid32 = new byte[8];

                guid24[5] = packet.ReadBit();
                guid32[2] = packet.ReadBit();
                guid32[0] = packet.ReadBit();
                guid32[3] = packet.ReadBit();
                guid24[3] = packet.ReadBit();
                guid32[5] = packet.ReadBit();
                guid24[0] = packet.ReadBit();
                guid32[6] = packet.ReadBit();
                guid32[7] = packet.ReadBit();
                guid24[1] = packet.ReadBit();
                guid32[1] = packet.ReadBit();
                guid24[4] = packet.ReadBit();
                guid24[2] = packet.ReadBit();
                guid24[7] = packet.ReadBit();
                guid32[4] = packet.ReadBit();
                guid24[6] = packet.ReadBit();

                packet.ParseBitStream(guid32, 7);
                packet.ParseBitStream(guid24, 3, 1, 4);
                packet.ParseBitStream(guid32, 1);
                packet.ParseBitStream(guid24, 7);
                packet.ParseBitStream(guid32, 6, 2);
                packet.ParseBitStream(guid24, 6);
                packet.ParseBitStream(guid32, 0);
                packet.ParseBitStream(guid24, 2);
                packet.ParseBitStream(guid32, 4);
                packet.ParseBitStream(guid24, 0, 5);
                packet.ParseBitStream(guid32, 5, 3);

                packet.WriteGuid("Guid24", guid24);
                packet.WriteGuid("Guid32", guid32);
            }

            packet.WriteGuid("Guid48", guid48);
            packet.WriteGuid("Guid56", guid56);
        }

        [Parser(Opcode.SMSG_ITEM_ENCHANT_TIME_UPDATE)]
        public static void HandleItemEnchantTimeUpdate(Packet packet)
        {
            var itemGUID = new byte[8];
            var playerGUID = new byte[8];

            itemGUID[4] = packet.ReadBit();
            itemGUID[0] = packet.ReadBit();
            playerGUID[3] = packet.ReadBit();
            itemGUID[3] = packet.ReadBit();
            playerGUID[2] = packet.ReadBit();
            playerGUID[6] = packet.ReadBit();
            playerGUID[7] = packet.ReadBit();
            itemGUID[1] = packet.ReadBit();
            playerGUID[4] = packet.ReadBit();
            itemGUID[6] = packet.ReadBit();
            itemGUID[5] = packet.ReadBit();
            playerGUID[0] = packet.ReadBit();
            itemGUID[2] = packet.ReadBit();
            playerGUID[5] = packet.ReadBit();
            playerGUID[1] = packet.ReadBit();
            itemGUID[7] = packet.ReadBit();
            packet.ReadInt32("Slot");
            packet.ReadXORByte(playerGUID, 4);
            packet.ReadXORByte(playerGUID, 2);
            packet.ReadXORByte(itemGUID, 5);
            packet.ReadXORByte(itemGUID, 4);
            packet.ReadXORByte(playerGUID, 6);
            packet.ReadXORByte(itemGUID, 1);
            packet.ReadXORByte(playerGUID, 0);
            packet.ReadXORByte(playerGUID, 1);
            packet.ReadXORByte(itemGUID, 6);
            packet.ReadXORByte(itemGUID, 2);
            packet.ReadXORByte(playerGUID, 7);
            packet.ReadXORByte(itemGUID, 0);
            packet.ReadXORByte(itemGUID, 3);
            packet.ReadXORByte(itemGUID, 7);
            packet.ReadXORByte(playerGUID, 3);
            packet.ReadXORByte(playerGUID, 5);
            packet.ReadInt32("Duration");

            packet.WriteGuid("Player GUID", playerGUID);
            packet.WriteGuid("Item GUID", itemGUID);
        }

        [Parser(Opcode.SMSG_ITEM_PUSH_RESULT)]
        public static void HandleItemPushResult(Packet packet)
        {
            var playerGuid = new byte[8];
            var itemGuid = new byte[8];

            itemGuid[2] = packet.ReadBit(); // 50
            playerGuid[4] = packet.ReadBit(); // 84
            itemGuid[5] = packet.ReadBit(); // 53
            packet.ReadBit("Display"); // 56
            playerGuid[1] = packet.ReadBit(); // 81
            packet.ReadBit("rcvFrom"); // 28
            itemGuid[4] = packet.ReadBit(); // 52
            playerGuid[6] = packet.ReadBit(); // 86
            playerGuid[5] = packet.ReadBit(); // 85
            playerGuid[7] = packet.ReadBit(); // 87
            playerGuid[0] = packet.ReadBit(); // 80
            itemGuid[0] = packet.ReadBit(); // 48
            itemGuid[7] = packet.ReadBit(); // 55
            playerGuid[2] = packet.ReadBit(); // 82
            itemGuid[6] = packet.ReadBit(); // 54
            packet.ReadBit("Bonus"); // 57
            playerGuid[3] = packet.ReadBit(); // 83
            itemGuid[1] = packet.ReadBit(); // 49
            packet.ReadBit("Created"); // 58
            itemGuid[3] = packet.ReadBit(); // 51

            packet.ReadXORByte(playerGuid, 1); // 81
            packet.ReadXORByte(itemGuid, 1); // 49
            packet.ReadInt32("unk64"); // 64
            packet.ReadXORByte(itemGuid, 0); // 48
            packet.ReadXORByte(playerGuid, 5); // 85
            packet.ReadXORByte(playerGuid, 2); // 82
            packet.ReadInt32("Suffix Factor"); // 36
            packet.ReadXORByte(itemGuid, 7); // 55
            packet.ReadInt32("unk60"); // 60
            packet.ReadUInt32<ItemId>("Entry"); // 72
            packet.ReadInt32("Random Property ID"); // 68
            packet.ReadXORByte(itemGuid, 6); // 54
            packet.ReadInt32("unk24"); // 24
            packet.ReadInt32("Count in inventory"); // 40
            packet.ReadXORByte(itemGuid, 2); // 50
            packet.ReadXORByte(playerGuid, 0); // 80
            packet.ReadUInt32("Count"); // 16
            packet.ReadXORByte(playerGuid, 7); // 87
            packet.ReadXORByte(itemGuid, 5); // 53
            packet.ReadXORByte(playerGuid, 4); // 84
            packet.ReadInt32("Item Slot"); // 32
            packet.ReadByte("Slot"); // 29
            packet.ReadXORByte(playerGuid, 3); // 83
            packet.ReadXORByte(playerGuid, 6); // 86
            packet.ReadInt32("unk20"); // 20
            packet.ReadXORByte(itemGuid, 3); // 51
            packet.ReadXORByte(itemGuid, 4); // 52

            packet.WriteGuid("playerGuid", playerGuid);
            packet.WriteGuid("itemGuid", itemGuid);
        }

        [Parser(Opcode.SMSG_ITEM_TIME_UPDATE)]
        public static void HandleItemTimeUpdate(Packet packet)
        {
            var guid = packet.StartBitStream(5, 3, 4, 1, 2, 6, 0, 7);
            packet.ParseBitStream(guid, 2, 6, 7, 4, 0, 3, 5, 1);
            packet.ReadInt32("Duration");
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_READ_ITEM_RESULT_OK)]
        public static void HandleReadItemResultOK(Packet packet)
        {
            packet.ReadGuid("Item GUID");
        }

        [Parser(Opcode.SMSG_REFORGE_RESULT)]
        public static void HandleReforgeResult(Packet packet)
        {
            packet.ReadBit("Successful");
        }

        [Parser(Opcode.CMSG_ITEM_UPGRADE)]
        public static void HandleItemSendUpgrade(Packet packet)
        {
            packet.ReadInt32("unk36"); // 36
            packet.ReadInt32("unk40"); // 40
            packet.ReadInt32("unk32"); // 32 itemUpgrade.db2  field 3 (0-5)

            var guid = new byte[8];
            var guid2 = new byte[8];

            guid2[5] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid2[6] = packet.ReadBit();
            guid2[0] = packet.ReadBit();
            guid2[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid2[2] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid2[4] = packet.ReadBit();
            guid[3] = packet.ReadBit();

            packet.ReadXORByte(guid2, 7);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid2, 6);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid2, 5);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid2, 1);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid2, 2);
            packet.ReadXORByte(guid2, 0);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid2, 3);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid2, 4);

            packet.WriteGuid("Guid", guid);
            packet.WriteGuid("Guid2", guid2);
        }

        [Parser(Opcode.SMSG_ITEM_UPGRADE_RESULT)]
        public static void HandleItemUpgradeResult(Packet packet)
        {
            packet.ReadBit("Successful");
        }

        [Parser(Opcode.SMSG_ITEM_EXPIRE_PURCHASE_REFUND)]
        public static void HandleItemExpirePurchaseRefund(Packet packet)
        {
            var guid = packet.StartBitStream(7, 4, 2, 6, 5, 3, 1, 0);
            packet.ParseBitStream(guid, 4, 0, 6, 7, 1, 2, 3, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_SELL_ITEM)]
        public static void HandleSellItemResponse(Packet packet)
        {
            var guid16 = new byte[8];
            var guid24 = new byte[8];

            guid24[2] = packet.ReadBit();
            guid16[4] = packet.ReadBit();
            guid24[5] = packet.ReadBit();
            guid24[4] = packet.ReadBit();
            guid16[3] = packet.ReadBit();
            guid16[5] = packet.ReadBit();
            guid24[3] = packet.ReadBit();
            guid16[6] = packet.ReadBit();
            guid16[0] = packet.ReadBit();
            guid16[2] = packet.ReadBit();
            guid24[1] = packet.ReadBit();
            guid24[7] = packet.ReadBit();
            guid16[1] = packet.ReadBit();
            guid24[0] = packet.ReadBit();
            guid24[6] = packet.ReadBit();
            guid16[7] = packet.ReadBit();

            packet.ParseBitStream(guid24, 4, 1);

            packet.ReadByteE<SellResult>("Sell Result");

            packet.ParseBitStream(guid24, 2);
            packet.ParseBitStream(guid16, 4, 0, 5, 2);
            packet.ParseBitStream(guid24, 0);
            packet.ParseBitStream(guid16, 3);
            packet.ParseBitStream(guid24, 5, 6, 7);
            packet.ParseBitStream(guid16, 6, 1);
            packet.ParseBitStream(guid24, 3);
            packet.ParseBitStream(guid16, 7);

            packet.WriteGuid("Guid16", guid16);
            packet.WriteGuid("Guid24", guid24);
        }

        [Parser(Opcode.SMSG_SET_PROFICIENCY)]
        public static void HandleSetProficiency(Packet packet)
        {
            packet.ReadUInt32E<UnknownFlags>("Mask");
            packet.ReadByteE<ItemClass>("Class");
        }

        [Parser(Opcode.CMSG_USE_SOULSTONE)]
        [Parser(Opcode.SMSG_READ_ITEM_RESULT_FAILED)]
        public static void HandleItemNull(Packet packet)
        {
        }
    }
}
