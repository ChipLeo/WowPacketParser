using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
//using WowPacketParserModule.V5_4_8_18414.Enums;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class ItemHandler
    {
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

        [Parser(Opcode.CMSG_AUTOEQUIP_ITEM)]
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

        [Parser(Opcode.CMSG_AUTOSTORE_BAG_ITEM)]
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
            var entry = (uint)packet.ReadInt32("Entry");
            packet.ReadTime("Hotfix date");
            var type = packet.ReadUInt32E<DB2Hash>("DB2 File");

            var size = packet.ReadInt32("Size");
            var data = packet.ReadBytes(size);
            var db2File = new Packet(data, packet.Opcode, packet.Time, packet.Direction, packet.Number, packet.Writer, packet.FileName);

            if ((int)entry < 0)
            {
                packet.WriteLine("Row {0} has been removed.", -(int)entry);
                return;
            }

            switch (type)
            {
                case DB2Hash.BroadcastText:
                    {
                        var broadcastText = new BroadcastText();

                        var Id = db2File.ReadEntry("Id");
                        broadcastText.Language = db2File.ReadInt32("Language");
                        if (db2File.ReadUInt16() > 0)
                            broadcastText.MaleText = db2File.ReadCString("Male Text");
                        if (db2File.ReadUInt16() > 0)
                            broadcastText.FemaleText = db2File.ReadCString("Female Text");

                        broadcastText.EmoteID = new uint[3];
                        broadcastText.EmoteDelay = new uint[3];
                        for (var i = 0; i < 3; ++i)
                            broadcastText.EmoteID[i] = (uint)db2File.ReadInt32("Emote ID", i);
                        for (var i = 0; i < 3; ++i)
                            broadcastText.EmoteDelay[i] = (uint)db2File.ReadInt32("Emote Delay", i);

                        broadcastText.SoundId = db2File.ReadUInt32("Sound Id");
                        broadcastText.UnkEmoteId = db2File.ReadUInt32("Unk MoP 1"); // unk emote
                        broadcastText.Type = db2File.ReadUInt32("Unk MoP 2"); // kind of type?

                        Storage.BroadcastTexts.Add((uint)Id.Key, broadcastText, packet.TimeSpan);
                        packet.AddSniffData(StoreNameType.BroadcastText, Id.Key, "BROADCAST_TEXT");
                        break;
                    }
                case DB2Hash.Creature: // New structure - 5.4.0
                    {
                        db2File.ReadUInt32("Creature Id");
                        db2File.ReadUInt32("Item Id 1");
                        db2File.ReadUInt32("Item Id 2");
                        db2File.ReadUInt32("Item Id 3");
                        db2File.ReadUInt32("Mount");
                        for (var i = 0; i < 4; ++i)
                            db2File.ReadInt32("Display Id", i);

                        for (var i = 0; i < 4; ++i)
                            db2File.ReadSingle("Display Id Probability", i);

                        if (db2File.ReadUInt16() > 0)
                            db2File.ReadCString("Name");

                        if (db2File.ReadUInt16() > 0)
                            db2File.ReadCString("SubName");

                        if (db2File.ReadUInt16() > 0)
                            db2File.ReadCString("Female SubName");

                        db2File.ReadUInt32("Rank");
                        db2File.ReadUInt32("Inhabit Type");
                        break;
                    }
                case DB2Hash.CreatureDifficulty:
                    {
                        var creatureDifficulty = new CreatureDifficulty();

                        var Id = db2File.ReadEntry("Id");
                        creatureDifficulty.CreatureID = db2File.ReadUInt32("Creature Id");
                        creatureDifficulty.FactionID = db2File.ReadUInt32("Faction Template Id");
                        creatureDifficulty.Expansion = db2File.ReadInt32("Expansion");
                        creatureDifficulty.MinLevel = db2File.ReadInt32("Min Level");
                        creatureDifficulty.MaxLevel = db2File.ReadInt32("Max Level");

                        creatureDifficulty.Flags = new uint[5];
                        for (var i = 0; i < 5; ++i)
                            creatureDifficulty.Flags[i] = db2File.ReadUInt32("Flags", i);

                        Storage.CreatureDifficultys.Add((uint)Id.Key, creatureDifficulty, packet.TimeSpan);
                        break;
                    }
                case DB2Hash.GameObjects:
                    {
                        var gameObjectTemplateDB2 = new GameObjectTemplateDB2();

                        var Id = db2File.ReadEntry("GameObject Id");

                        gameObjectTemplateDB2.MapID = db2File.ReadUInt32("Map");

                        gameObjectTemplateDB2.DisplayId = db2File.ReadUInt32("Display Id");

                        gameObjectTemplateDB2.PositionX = db2File.ReadSingle("Position X");
                        gameObjectTemplateDB2.PositionY = db2File.ReadSingle("Position Y");
                        gameObjectTemplateDB2.PositionZ = db2File.ReadSingle("Position Z");
                        gameObjectTemplateDB2.RotationX = db2File.ReadSingle("Rotation X");
                        gameObjectTemplateDB2.RotationY = db2File.ReadSingle("Rotation Y");
                        gameObjectTemplateDB2.RotationZ = db2File.ReadSingle("Rotation Z");
                        gameObjectTemplateDB2.RotationW = db2File.ReadSingle("Rotation W");

                        gameObjectTemplateDB2.Size = db2File.ReadSingle("Size");
                        gameObjectTemplateDB2.Type = db2File.ReadInt32E<GameObjectType>("Type");

                        gameObjectTemplateDB2.Data = new int[4];
                        for (var i = 0; i < gameObjectTemplateDB2.Data.Length; i++)
                            gameObjectTemplateDB2.Data[i] = db2File.ReadInt32("Data", i);

                        if (db2File.ReadUInt16() > 0)
                            gameObjectTemplateDB2.Name = db2File.ReadCString("Name");

                        Storage.GameObjectTemplateDB2s.Add((uint)Id.Key, gameObjectTemplateDB2, packet.TimeSpan);
                        break;
                    }
                case DB2Hash.Item:
                    {
                        var item = Storage.ItemTemplates.ContainsKey(entry)
                            ? Storage.ItemTemplates[entry].Item1
                            : new ItemTemplate();

                        db2File.ReadUInt32<ItemId>("Item Entry");
                        item.Class = db2File.ReadInt32E<ItemClass>("Class");
                        item.SubClass = db2File.ReadUInt32("Sub Class");
                        item.SoundOverrideSubclass = db2File.ReadInt32("Sound Override Subclass");
                        item.Material = db2File.ReadInt32E<Material>("Material");
                        item.DisplayId = db2File.ReadUInt32("Display ID");
                        item.InventoryType = db2File.ReadUInt32E<InventoryType>("Inventory Type");
                        item.SheathType = db2File.ReadInt32E<SheathType>("Sheath Type");

                        Storage.ItemTemplates.Add(entry, item, packet.TimeSpan);
                        packet.AddSniffData(StoreNameType.Item, (int)entry, "DB_REPLY");
                        break;
                    }
                case DB2Hash.ItemExtendedCost:
                    {
                        db2File.ReadUInt32("Item Extended Cost Entry");
                        db2File.ReadUInt32("Required Honor Points");
                        db2File.ReadUInt32("Required Arena Points");
                        db2File.ReadUInt32("Required Arena Slot");
                        for (var i = 0; i < 5; ++i)
                            db2File.ReadUInt32("Required Item", i);

                        for (var i = 0; i < 5; ++i)
                            db2File.ReadUInt32("Required Item Count", i);

                        db2File.ReadUInt32("Required Personal Arena Rating");
                        db2File.ReadUInt32("Item Purchase Group");
                        for (var i = 0; i < 5; ++i)
                            db2File.ReadUInt32("Required Currency", i);

                        for (var i = 0; i < 5; ++i)
                            db2File.ReadUInt32("Required Currency Count", i);

                        db2File.ReadUInt32("Required Faction Id");
                        db2File.ReadUInt32("Required Faction Standing");
                        db2File.ReadUInt32("Requirement Flags");
                        db2File.ReadUInt32("Required Guild Level");
                        db2File.ReadInt32<AchievementId>("Required Achievement");
                        break;
                    }
                case DB2Hash.ItemCurrencyCost:
                    {
                        db2File.ReadUInt32("Id");
                        db2File.ReadUInt32<ItemId>("Item Entry");
                        break;
                    }
                case DB2Hash.RulesetItemUpgrade:
                    {
                        db2File.ReadUInt32("Id");
                        db2File.ReadUInt32("Item Upgrade Level");
                        db2File.ReadUInt32("Item Upgrade Id");
                        db2File.ReadUInt32<ItemId>("Item Entry");
                        break;
                    }
                case DB2Hash.Item_sparse:
                    {
                        var item = Storage.ItemTemplates.ContainsKey(entry)
                            ? Storage.ItemTemplates[entry].Item1
                            : new ItemTemplate();

                        db2File.ReadUInt32<ItemId>("Item Sparse Entry");
                        item.Quality = db2File.ReadInt32E<ItemQuality>("Quality");
                        item.Flags1 = db2File.ReadUInt32E<ItemProtoFlags>("Flags 1");
                        item.Flags2 = db2File.ReadInt32E<ItemFlagExtra>("Flags 2");
                        item.Flags3 = db2File.ReadUInt32("Flags 3");
                        item.Unk430_1 = db2File.ReadSingle("Unk430_1");
                        item.Unk430_2 = db2File.ReadSingle("Unk430_2");
                        item.BuyCount = db2File.ReadUInt32("Buy count");
                        item.BuyPrice = db2File.ReadUInt32("Buy Price");
                        item.SellPrice = db2File.ReadUInt32("Sell Price");
                        item.InventoryType = db2File.ReadInt32E<InventoryType>("Inventory Type");
                        item.AllowedClasses = db2File.ReadInt32E<ClassMask>("Allowed Classes");
                        item.AllowedRaces = db2File.ReadInt32E<RaceMask>("Allowed Races");
                        item.ItemLevel = db2File.ReadUInt32("Item Level");
                        item.RequiredLevel = db2File.ReadUInt32("Required Level");
                        item.RequiredSkillId = db2File.ReadUInt32("Required Skill ID");
                        item.RequiredSkillLevel = db2File.ReadUInt32("Required Skill Level");
                        item.RequiredSpell = (uint)db2File.ReadInt32<SpellId>("Required Spell");
                        item.RequiredHonorRank = db2File.ReadUInt32("Required Honor Rank");
                        item.RequiredCityRank = db2File.ReadUInt32("Required City Rank");
                        item.RequiredRepFaction = db2File.ReadUInt32("Required Rep Faction");
                        item.RequiredRepValue = db2File.ReadUInt32("Required Rep Value");
                        item.MaxCount = db2File.ReadInt32("Max Count");
                        item.MaxStackSize = db2File.ReadInt32("Max Stack Size");
                        item.ContainerSlots = db2File.ReadUInt32("Container Slots");

                        item.StatTypes = new ItemModType[10];
                        for (var i = 0; i < 10; i++)
                        {
                            var statType = db2File.ReadInt32E<ItemModType>("Stat Type", i);
                            item.StatTypes[i] = statType == ItemModType.None ? ItemModType.Mana : statType; // TDB
                        }

                        item.StatValues = new int[10];
                        for (var i = 0; i < 10; i++)
                            item.StatValues[i] = db2File.ReadInt32("Stat Value", i);

                        item.ScalingValue = new int[10];
                        for (var i = 0; i < 10; i++)
                            item.ScalingValue[i] = db2File.ReadInt32("Scaling Value", i);

                        item.SocketCostRate = new int[10];
                        for (var i = 0; i < 10; i++)
                            item.SocketCostRate[i] = db2File.ReadInt32("Socket Cost Rate", i);

                        item.ScalingStatDistribution = db2File.ReadInt32("Scaling Stat Distribution");
                        item.DamageType = db2File.ReadInt32E<DamageType>("Damage Type");
                        item.Delay = db2File.ReadUInt32("Delay");
                        item.RangedMod = db2File.ReadSingle("Ranged Mod");

                        item.TriggeredSpellIds = new int[5];
                        for (var i = 0; i < 5; i++)
                            item.TriggeredSpellIds[i] = db2File.ReadInt32<SpellId>("Triggered Spell ID", i);

                        item.TriggeredSpellTypes = new ItemSpellTriggerType[5];
                        for (var i = 0; i < 5; i++)
                            item.TriggeredSpellTypes[i] = db2File.ReadInt32E<ItemSpellTriggerType>("Trigger Spell Type", i);

                        item.TriggeredSpellCharges = new int[5];
                        for (var i = 0; i < 5; i++)
                            item.TriggeredSpellCharges[i] = db2File.ReadInt32("Triggered Spell Charges", i);

                        item.TriggeredSpellCooldowns = new int[5];
                        for (var i = 0; i < 5; i++)
                            item.TriggeredSpellCooldowns[i] = db2File.ReadInt32("Triggered Spell Cooldown", i);

                        item.TriggeredSpellCategories = new uint[5];
                        for (var i = 0; i < 5; i++)
                            item.TriggeredSpellCategories[i] = db2File.ReadUInt32("Triggered Spell Category", i);

                        item.TriggeredSpellCategoryCooldowns = new int[5];
                        for (var i = 0; i < 5; i++)
                            item.TriggeredSpellCategoryCooldowns[i] = db2File.ReadInt32(
                                "Triggered Spell Category Cooldown", i);

                        item.Bonding = db2File.ReadInt32E<ItemBonding>("Bonding");

                        if (db2File.ReadUInt16() > 0)
                            item.Name = db2File.ReadCString("Name", 0);

                        for (var i = 1; i < 4; ++i)
                            if (db2File.ReadUInt16() > 0)
                                db2File.ReadCString("Name", i);

                        if (db2File.ReadUInt16() > 0)
                            item.Description = db2File.ReadCString("Description");

                        item.PageText = db2File.ReadUInt32("Page Text");
                        item.Language = db2File.ReadInt32E<Language>("Language");
                        item.PageMaterial = db2File.ReadInt32E<PageMaterial>("Page Material");
                        item.StartQuestId = (uint)db2File.ReadInt32<QuestId>("Start Quest");
                        item.LockId = db2File.ReadUInt32("Lock ID");
                        item.Material = db2File.ReadInt32E<Material>("Material");
                        item.SheathType = db2File.ReadInt32E<SheathType>("Sheath Type");
                        item.RandomPropery = db2File.ReadInt32("Random Property");
                        item.RandomSuffix = db2File.ReadUInt32("Random Suffix");
                        item.ItemSet = db2File.ReadUInt32("Item Set");
                        item.AreaId = db2File.ReadUInt32<AreaId>("Area");
                        item.MapId = db2File.ReadInt32<MapId>("Map ID");
                        item.BagFamily = db2File.ReadInt32E<BagFamilyMask>("Bag Family");
                        item.TotemCategory = db2File.ReadInt32E<TotemCategory>("Totem Category");

                        item.ItemSocketColors = new ItemSocketColor[3];
                        for (var i = 0; i < 3; i++)
                            item.ItemSocketColors[i] = db2File.ReadInt32E<ItemSocketColor>("Socket Color", i);

                        item.SocketContent = new uint[3];
                        for (var i = 0; i < 3; i++)
                            item.SocketContent[i] = db2File.ReadUInt32("Socket Item", i);

                        item.SocketBonus = db2File.ReadInt32("Socket Bonus");
                        item.GemProperties = db2File.ReadInt32("Gem Properties");
                        item.ArmorDamageModifier = db2File.ReadSingle("Armor Damage Modifier");
                        item.Duration = db2File.ReadUInt32("Duration");
                        item.ItemLimitCategory = db2File.ReadInt32("Limit Category");
                        item.HolidayId = db2File.ReadInt32E<Holiday>("Holiday");
                        item.StatScalingFactor = db2File.ReadSingle("Stat Scaling Factor");
                        item.CurrencySubstitutionId = db2File.ReadUInt32("Currency Substitution Id");
                        item.CurrencySubstitutionCount = db2File.ReadUInt32("Currency Substitution Count");

                        Storage.ObjectNames.Add(entry, new ObjectName { ObjectType = ObjectType.Item, Name = item.Name },
                            packet.TimeSpan);
                        packet.AddSniffData(StoreNameType.Item, (int)entry, "DB_REPLY");
                        break;
                    }
                case DB2Hash.KeyChain:
                    {
                        db2File.ReadUInt32("Key Chain Id");
                        db2File.ReadBytes("Key", 32);
                        break;
                    }
                case DB2Hash.SceneScript: // lua ftw!
                    {
                        db2File.ReadUInt32("Scene Script Id");
                        if (db2File.ReadUInt16() > 0)
                            db2File.ReadCString("Name");

                        if (db2File.ReadUInt16() > 0)
                            db2File.ReadCString("Script");
                        db2File.ReadUInt32("Previous Scene Script Part");
                        db2File.ReadUInt32("Next Scene Script Part");
                        break;
                    }
                case DB2Hash.Vignette:
                    {
                        db2File.ReadUInt32("Vignette Entry");
                        if (db2File.ReadUInt16() > 0)
                            db2File.ReadCString("Name");

                        db2File.ReadUInt32("Icon");
                        db2File.ReadUInt32("Flag"); // not 100% sure (8 & 32 as values only) - todo verify with more data
                        db2File.ReadSingle("Unk Float 1");
                        db2File.ReadSingle("Unk Float 2");
                        break;
                    }
                case DB2Hash.WbAccessControlList:
                    {
                        db2File.ReadUInt32("Id");

                        if (db2File.ReadUInt16() > 0)
                            db2File.ReadCString("Address");

                        db2File.ReadUInt32("Unk MoP 1");
                        db2File.ReadUInt32("Unk MoP 2");
                        db2File.ReadUInt32("Unk MoP 3");
                        db2File.ReadUInt32("Unk MoP 4"); // flags?
                        break;
                    }
                default:
                    {
                        db2File.AddValue("Unknown DB2 file type", string.Format("{0} (0x{0:x})", type));
                        for (var i = 0; ; ++i)
                        {
                            if (db2File.Length - 4 >= db2File.Position)
                            {
                                var blockVal = db2File.ReadUpdateField();
                                string key = "Block Value " + i;
                                string value = blockVal.UInt32Value + "/" + blockVal.SingleValue;
                                packet.AddValue(key, value);
                            }
                            else
                            {
                                var left = db2File.Length - db2File.Position;
                                for (var j = 0; j < left; ++j)
                                {
                                    string key = "Byte Value " + i;
                                    var value = db2File.ReadByte();
                                    packet.AddValue(key, value);
                                }
                                break;
                            }
                        }
                        break;
                    }
            }

            db2File.ClosePacket();
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

        [Parser(Opcode.SMSG_SEND_ITEM_UPGRADE)]
        public static void HandleSendItemUpgrade(Packet packet)
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
    }
}
