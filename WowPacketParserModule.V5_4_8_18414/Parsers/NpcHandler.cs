using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Proto;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    [SuppressMessage("ReSharper", "UseObjectOrCollectionInitializer")]
    public static class NpcHandler
    {
        public static uint LastGossipPOIEntry = 0;

        [Parser(Opcode.CMSG_AUCTION_HELLO_REQUEST)]
        public static void HandleAuctionHello(Packet packet)
        {
            var GUID = packet.StartBitStream(1, 5, 2, 0, 3, 6, 4, 7);
            packet.ParseBitStream(GUID, 2, 7, 1, 3, 5, 0, 4, 6);
            packet.WriteGuid("GUID", GUID);
        }

        [Parser(Opcode.CMSG_BANKER_ACTIVATE)]
        public static void HandleBankerActivate(Packet packet)
        {
            var GUID = packet.StartBitStream(4, 5, 0, 6, 1, 2, 7, 3);
            packet.ParseBitStream(GUID, 1, 7, 2, 5, 6, 3, 0, 4);
            packet.WriteGuid("GUID", GUID);
        }

        [Parser(Opcode.CMSG_BINDER_ACTIVATE)]
        public static void HandleBinderActivate(Packet packet)
        {
            var GUID = packet.StartBitStream(0, 5, 4, 7, 6, 2, 1, 3);
            packet.ParseBitStream(GUID, 0, 4, 2, 3, 7, 1, 5, 6);
            packet.WriteGuid("GUID", GUID);
        }

        [Parser(Opcode.CMSG_GOSSIP_HELLO)]
        public static void HandleNpcHello(Packet packet)
        {
            var guid = packet.StartBitStream(2, 4, 0, 3, 6, 7, 5, 1);
            packet.ParseBitStream(guid, 4, 7, 1, 0, 5, 3, 6, 2);

            CoreParsers.NpcHandler.LastGossipOption.Guid = packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_GOSSIP_SELECT_OPTION)]
        public static void HandleNpcGossipSelectOption(Packet packet)
        {
            var guid = new byte[8];

            var menuEntry = packet.ReadUInt32("Menu Id"); // 272
            var gossipId = packet.ReadUInt32("Gossip Id"); // 288

            guid[3] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[6] = packet.ReadBit();

            var boxTextLength = packet.ReadBits("size", 8);

            guid[2] = packet.ReadBit();

            //flushbits

            packet.ParseBitStream(guid, 7, 3, 4, 6, 0, 5);

            packet.ReadWoWString("Box Text", boxTextLength);

            packet.ParseBitStream(guid, 2, 1);

            packet.WriteGuid("NPC Guid", guid);

            Storage.GossipSelects.Add(Tuple.Create(menuEntry, gossipId), null, packet.TimeSpan);
        }

        [Parser(Opcode.CMSG_LIST_INVENTORY)]
        public static void HandleNpcListInventory(Packet packet)
        {
            var guid = packet.StartBitStream(6, 7, 3, 1, 2, 0, 4, 5);
            packet.ParseBitStream(guid, 0, 7, 1, 6, 4, 3, 5, 2);
            CoreParsers.NpcHandler.LastGossipOption.Guid = packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_QUERY_NPC_TEXT)]
        public static void HandleNpcTextQuery(Packet packet)
        {
            packet.ReadUInt32("Text Id");
            var guid = packet.StartBitStream(4, 5, 1, 7, 0, 2, 6, 3);
            packet.ParseBitStream(guid, 4, 0, 2, 5, 1, 7, 3, 6);
            packet.WriteGuid("NPC Guid", guid);
        }

        [Parser(Opcode.CMSG_SPIRIT_HEALER_ACTIVATE)]
        public static void HandleSpiritHealerActivate(Packet packet)
        {
            var guid = packet.StartBitStream(2, 7, 6, 0, 5, 4, 1, 3);
            packet.ParseBitStream(guid, 1, 5, 6, 3, 2, 0, 7, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_TRAINER_BUY_SPELL)]
        public static void HandleTrainerBuySpell(Packet packet)
        {
            packet.ReadInt32("unk24"); // 24
            packet.ReadInt32("unk28"); // 28

            var guid = packet.StartBitStream(1, 4, 0, 6, 3, 2, 5, 7);
            packet.ParseBitStream(guid, 3, 1, 4, 7, 0, 5, 6, 2);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_TRAINER_LIST)]
        public static void HandleTrainerList(Packet packet)
        {
            var guid = new byte[8];

            packet.StartBitStream(guid, 0, 2, 7, 6, 1, 4, 5, 3);
            packet.ParseBitStream(guid, 3, 6, 7, 5, 1, 0, 2, 4);

            CoreParsers.NpcHandler.LastGossipOption.Guid = packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_AUCTION_HELLO_RESPONSE)]
        public static void HandleAuctionHelloResponse(Packet packet)
        {
            var GUID = new byte[8];
            GUID[6] = packet.ReadBit();
            GUID[7] = packet.ReadBit();
            GUID[3] = packet.ReadBit();
            var inUse = packet.ReadBit("inUse");
            GUID[4] = packet.ReadBit();
            GUID[2] = packet.ReadBit();
            GUID[5] = packet.ReadBit();
            GUID[0] = packet.ReadBit();
            GUID[1] = packet.ReadBit();

            packet.ReadXORByte(GUID, 3);
            var AHID = packet.ReadUInt32("Entry");
            packet.ParseBitStream(GUID, 4, 2, 7, 1, 0, 6, 5);
            packet.WriteGuid("GUID", GUID);
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_GOSSIP_MESSAGE)]
        public static void HandleNpcGossip(Packet packet)
        {
            PacketGossipMessage packetGossip = packet.Holder.GossipMessage = new();
            var guid = new byte[8];

            uint questgossips = packet.ReadBits(19);

            var titleLen = new uint[questgossips];
            for (int i = 0; i < questgossips; ++i)
            {
                packet.ReadBit("Change Icon", i);
                titleLen[i] = packet.ReadBits(9);
            }

            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[0] = packet.ReadBit();

            uint amountOfOptions = packet.ReadBits(20);

            guid[6] = packet.ReadBit();
            guid[2] = packet.ReadBit();

            var boxTextLen = new uint[amountOfOptions];
            var optionTextLen = new uint[amountOfOptions];
            for (int i = 0; i < amountOfOptions; ++i)
            {
                boxTextLen[i] = packet.ReadBits(12);
                optionTextLen[i] = packet.ReadBits(12);
            }

            guid[3] = packet.ReadBit();
            guid[1] = packet.ReadBit();

            for (int i = 0; i < questgossips; ++i)
            {
                var title = packet.ReadWoWString("Title", titleLen[i], i);
                packet.ReadUInt32E<QuestFlags>("Flags", i);//528
                packet.ReadInt32("Level", i);//8
                packet.ReadUInt32("Icon", i);//4
                var quest = packet.ReadUInt32<QuestId>("Quest ID", i); //528
                packet.ReadUInt32E<QuestFlagsEx>("FlagsEx", i);//532

                packetGossip.Quests.Add(new GossipQuestOption()
                {
                    Title = title,
                    QuestId = quest
                });
            }

            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 0);

            var gossipOptions = new List<GossipMenuOption>((int)amountOfOptions);
            for (int i = 0; i < amountOfOptions; ++i)
            {
                GossipMenuOption gossipMenuOption = new GossipMenuOption();
                gossipMenuOption.BoxMoney = packet.ReadUInt32("Required money", i);
                var boxText = packet.ReadWoWString("Box Text", boxTextLen[i], i);
                gossipMenuOption.OptionID = packet.ReadUInt32("OptionID", i);
                gossipMenuOption.BoxCoded = packet.ReadBool("Box", i);
                gossipMenuOption.OptionText = packet.ReadWoWString("Text", optionTextLen[i], i);
                gossipMenuOption.OptionNpc = packet.ReadByteE<GossipOptionNpc>("OptionNPC", i);

                if (!string.IsNullOrEmpty(boxText))
                    gossipMenuOption.BoxText = boxText;

                gossipOptions.Add(gossipMenuOption);

                packetGossip.Options.Add(new GossipMessageOption()
                {
                    OptionIndex = gossipMenuOption.OptionID.Value,
                    OptionNpc = (int)gossipMenuOption.OptionNpc,
                    BoxCoded = gossipMenuOption.BoxCoded.Value,
                    BoxCost = (uint)gossipMenuOption.BoxMoney.Value,
                    Text = gossipMenuOption.OptionText,
                    BoxText = gossipMenuOption.BoxText
                });
            }

            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 3);
            uint menuId = packetGossip.MenuId = packet.ReadUInt32("Menu Id");
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 4);
            uint friendshipFactionID = packet.ReadUInt32("Friendship Faction");
            packet.ReadXORByte(guid, 7);
            uint textId = packetGossip.TextId = packet.ReadUInt32("Text Id");

            GossipMenu gossip = new GossipMenu
            {
                MenuID = menuId,
                TextID = textId
            };

            WowGuid guidg = packet.WriteGuid("Guid", guid);

            packetGossip.GossipSource = guidg;

            gossip.ObjectType = guidg.GetObjectType();
            gossip.ObjectEntry = guidg.GetEntry();

            if (guidg.GetObjectType() == ObjectType.Unit)
            {
                if (!Storage.CreatureDefaultGossips.ContainsKey(guidg.GetEntry()))
                    Storage.CreatureDefaultGossips.Add(guidg.GetEntry(), menuId);
            }

            gossipOptions.ForEach(g =>
            {
                g.MenuID = menuId;
                g.FillBroadcastTextIDs();
                g.FillOptionType(guidg);
                Storage.GossipMenuOptions.Add((g.MenuID, g.OptionID), g, packet.TimeSpan);
            });

            Storage.Gossips.Add(gossip, packet.TimeSpan);

            CoreParsers.NpcHandler.AddGossipAddon(packetGossip.MenuId, (int)friendshipFactionID, 0, guidg, packet.TimeSpan);

            CoreParsers.NpcHandler.UpdateLastGossipOptionActionMessage(packet.TimeSpan, gossip.MenuID);

            packet.AddSniffData(StoreNameType.Gossip, (int)menuId, guidg.GetEntry().ToString(CultureInfo.InvariantCulture));
        }

        [Parser(Opcode.SMSG_GOSSIP_POI)]
        public static void HandleGossipPoi(Packet packet)
        {
            var protoPoi = packet.Holder.GossipPoi = new();
            ++LastGossipPOIEntry;
            PointsOfInterest gossipPOI = new PointsOfInterest();
            gossipPOI.ID = "@ID+" + LastGossipPOIEntry.ToString();

            gossipPOI.Flags = protoPoi.Flags = (uint)packet.ReadInt32E<UnknownFlags>("Flags");

            Vector2 pos = packet.ReadVector2("Coordinates");
            protoPoi.Coordinates = pos;
            gossipPOI.PositionX = pos.X;
            gossipPOI.PositionY = pos.Y;

            gossipPOI.Icon = packet.ReadUInt32E<GossipPOIIcon>("Icon");
            gossipPOI.Importance = protoPoi.Importance = packet.ReadUInt32("Importance");
            gossipPOI.Name = protoPoi.Name = packet.ReadCString("Icon Name");
            protoPoi.Icon = (uint)gossipPOI.Icon;

            Storage.GossipPOIs.Add(gossipPOI, packet.TimeSpan);
            CoreParsers.NpcHandler.UpdateTempGossipOptionActionPOI(packet.TimeSpan, gossipPOI.ID);
        }

        [Parser(Opcode.SMSG_HIGHEST_THREAT_UPDATE)]
        public static void HandleHighestThreatlistUpdate(Packet packet)
        {
            var guid = new byte[8];
            var newHighestGUID = new byte[8];

            guid[3] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            newHighestGUID[3] = packet.ReadBit();
            newHighestGUID[6] = packet.ReadBit();
            newHighestGUID[1] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            newHighestGUID[2] = packet.ReadBit();
            newHighestGUID[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            newHighestGUID[4] = packet.ReadBit();

            var count32 = packet.ReadBits("cnt32", 21);
            var guids = new byte[count32][];

            for (var i = 0; i < count32; i++)
                guids[i] = packet.StartBitStream(6, 1, 0, 2, 7, 4, 3, 5);

            newHighestGUID[7] = packet.ReadBit();
            newHighestGUID[0] = packet.ReadBit();
            guid[2] = packet.ReadBit();

            packet.ReadXORByte(newHighestGUID, 4);

            for (var i = 0; i < count32; i++)
            {
                packet.ParseBitStream(guids[i], 6);
                packet.ReadInt32("Threat", i); // 8
                packet.ParseBitStream(guids[i], 4, 0, 3, 5, 2, 1, 7);

                packet.WriteGuid("Hostile", guids[i], i);
            }
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(newHighestGUID, 5);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(newHighestGUID, 1);
            packet.ReadXORByte(newHighestGUID, 0);
            packet.ReadXORByte(newHighestGUID, 2);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(newHighestGUID, 7);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(newHighestGUID, 3);
            packet.ReadXORByte(newHighestGUID, 6);
            packet.ReadXORByte(guid, 5);

            packet.WriteGuid("Guid", guid);
            packet.WriteGuid("New Highest", newHighestGUID);
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_VENDOR_INVENTORY)]
        public static void HandleVendorInventoryList(Packet packet)
        {
            var guid = new byte[8];

            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[6] = packet.ReadBit();

            var itemCount = packet.ReadBits(18);

            var unkBit = new uint[itemCount];
            var hasExtendedCost = new bool[itemCount];
            var hasCondition = new bool[itemCount];

            for (var i = 0; i < itemCount; ++i)
            {
                unkBit[i] = packet.ReadBit();
                hasExtendedCost[i] = !packet.ReadBit(); // +44
                hasCondition[i] = !packet.ReadBit(); // +36
            }

            guid[4] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[2] = packet.ReadBit();

            packet.ReadByte("Byte10");

            var tempList = new List<NpcVendor>();
            for (var i = 0; i < itemCount; ++i)
            {
                NpcVendor vendor = new NpcVendor();

                packet.AddValue("unkBit", unkBit[i], i);

                packet.ReadInt32("Max Durability", i);   // +16
                packet.ReadInt32("Price", i);   // +20
                vendor.Type = packet.ReadUInt32("Type", i);    // +4
                var maxCount = packet.ReadInt32("Max Count", i);     // +24
                vendor.MaxCount = maxCount == -1 ? 0 : (uint)maxCount; // TDB
                packet.ReadInt32("Display ID", i);    // +12
                var buyCount = packet.ReadUInt32("Buy Count", i);    // +28

                if (vendor.Type == 2)
                    vendor.MaxCount = buyCount;

                vendor.Item = (int)packet.ReadInt32<ItemId>("Item ID", i);   // +8

                if (hasExtendedCost[i])
                    vendor.ExtendedCost = packet.ReadUInt32("Extended Cost", i);    // +36

                packet.ReadInt32("Item Upgrade ID", i);   // +32

                if (hasCondition[i])
                    packet.ReadInt32("Condition ID", i);    // +40

                vendor.Slot = (int)packet.ReadUInt32("Item Position", i);    // +0

                tempList.Add(vendor);
            }

            packet.ParseBitStream(guid, 3, 7, 0, 6, 2, 1, 4, 5);

            uint entry = packet.WriteGuid("Guid", guid).GetEntry();
            tempList.ForEach(v =>
            {
                v.Entry = entry;
                Storage.NpcVendors.Add(v, packet.TimeSpan);
            });
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_QUERY_NPC_TEXT_RESPONSE)]
        public static void HandleNpcTextUpdate(Packet packet)
        {
            var npcText = new NpcTextMop();

            var entry = packet.ReadEntry("Entry");
            if (entry.Value) // Can be masked
                return;

            var size = packet.ReadInt32("Size");
            var data = packet.ReadBytes(size);

            var pkt = new Packet(data, packet.Opcode, packet.Time, packet.Direction, packet.Number, packet.Writer, packet.FileName);
            npcText.Probabilities = new float[8];
            npcText.BroadcastTextId = new uint[8];
            for (var i = 0; i < 8; ++i)
                npcText.Probabilities[i] = pkt.ReadSingle("Probability", i);
            for (var i = 0; i < 8; ++i)
                npcText.BroadcastTextId[i] = pkt.ReadUInt32("Broadcast Text Id", i);

            pkt.ClosePacket(false);

            var hasData = packet.ReadBit();
            if (!hasData)
                return; // nothing to do

            packet.AddSniffData(StoreNameType.NpcText, entry.Key, "QUERY_RESPONSE");
            npcText.ID = (uint)entry.Key;

            Storage.NpcTextsMop.Add(npcText, packet.TimeSpan);
        }

        [Parser(Opcode.SMSG_SHOW_BANK)]
        public static void HandleShowBank(Packet packet)
        {
            var GUID = packet.StartBitStream(2, 4, 3, 6, 5, 1, 7, 0);
            packet.ParseBitStream(GUID, 7, 0, 5, 3, 6, 1, 4, 2);
            packet.WriteGuid("GUID", GUID);
        }

        [Parser(Opcode.SMSG_THREAT_CLEAR)]
        public static void HandleClearThreatlist(Packet packet)
        {
            var guid = packet.StartBitStream(6, 7, 4, 5, 2, 1, 0, 3);
            packet.ParseBitStream(guid, 7, 0, 4, 3, 2, 1, 6, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_THREAT_REMOVE)]
        public static void HandleRemoveThreatlist(Packet packet)
        {
            var hostileGUID = new byte[8];
            var victimGUID = new byte[8];

            victimGUID[0] = packet.ReadBit();
            victimGUID[1] = packet.ReadBit();
            victimGUID[5] = packet.ReadBit();
            hostileGUID[4] = packet.ReadBit();
            hostileGUID[0] = packet.ReadBit();
            victimGUID[4] = packet.ReadBit();
            victimGUID[6] = packet.ReadBit();
            hostileGUID[7] = packet.ReadBit();
            hostileGUID[6] = packet.ReadBit();
            hostileGUID[3] = packet.ReadBit();
            victimGUID[2] = packet.ReadBit();
            hostileGUID[1] = packet.ReadBit();
            victimGUID[3] = packet.ReadBit();
            victimGUID[7] = packet.ReadBit();
            hostileGUID[5] = packet.ReadBit();
            hostileGUID[2] = packet.ReadBit();

            packet.ReadXORByte(hostileGUID, 3);
            packet.ReadXORByte(hostileGUID, 0);
            packet.ReadXORByte(hostileGUID, 2);
            packet.ReadXORByte(victimGUID, 5);
            packet.ReadXORByte(victimGUID, 4);
            packet.ReadXORByte(victimGUID, 7);
            packet.ReadXORByte(victimGUID, 3);
            packet.ReadXORByte(victimGUID, 0);
            packet.ReadXORByte(hostileGUID, 4);
            packet.ReadXORByte(victimGUID, 1);
            packet.ReadXORByte(hostileGUID, 1);
            packet.ReadXORByte(victimGUID, 6);
            packet.ReadXORByte(hostileGUID, 7);
            packet.ReadXORByte(hostileGUID, 6);
            packet.ReadXORByte(victimGUID, 2);
            packet.ReadXORByte(hostileGUID, 5);

            packet.WriteGuid("Hostile GUID", hostileGUID);
            packet.WriteGuid("GUID", victimGUID);
        }

        [Parser(Opcode.SMSG_THREAT_UPDATE)]
        public static void HandleThreatlistUpdate(Packet packet)
        {
            var guid = new byte[8];

            packet.StartBitStream(guid, 5, 6, 1, 3, 7, 0, 4);

            var count = packet.ReadBits("Size", 21);

            var hostileGUID = new byte[count][];

            for (var i = 0; i < count; i++)
                hostileGUID[i] = packet.StartBitStream(2, 3, 6, 5, 1, 4, 0, 7);

            guid[2] = packet.ReadBit();

            for (var i = 0; i < count; i++)
            {
                packet.ParseBitStream(hostileGUID[i], 6, 7, 0, 1, 2, 5, 3, 4);
                packet.ReadUInt32("Threat", i);
                packet.WriteGuid("Hostile", hostileGUID[i], i);
            }

            packet.ParseBitStream(guid, 1, 4, 2, 3, 5, 6, 0, 7);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_TRAINER_BUY_FAILED)]
        public static void HandleTrainerBuyFailed(Packet packet)
        {
            /*
             ServerToClient: SMSG_UNK_042E (0x042E) Length: 15
             unk24: 0 (0x0000)
             Spell ID: 41536 (0xA240)
             Guid: Full: 0xF13044CD00000FA8 Type: Unit Entry: 17613 Low: 4008
             */
            var guid = packet.StartBitStream(3, 0, 4, 7, 6, 1, 5, 2);
            packet.ParseBitStream(guid, 1, 2, 0, 3, 4);
            packet.ReadInt32("Reason"); // 24
            packet.ParseBitStream(guid, 5, 6, 7);
            packet.ReadInt32<SpellId>("Spell ID"); // 28
            packet.WriteGuid("Guid", guid);
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_TRAINER_LIST)]
        public static void HandleServerTrainerList(Packet packet)
        {
            var guid = new byte[8];

            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var count = (int)packet.ReadBits(19);
            var titleLen = packet.ReadBits(11);
            guid[6] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[0] = packet.ReadBit();

            packet.ReadXORByte(guid, 4);

            var tempList = new List<TrainerSpell>();
            for (var i = 0; i < count; ++i)
            {
                TrainerSpell trainerSpell = new TrainerSpell
                {
                    ReqLevel = packet.ReadByte("ReqLevel", i),
                    MoneyCost = packet.ReadUInt32("MoneyCost", i),
                    SpellId = packet.ReadUInt32<SpellId>("SpellID", i),
                    ReqAbility = new uint[3]
                };

                for (var j = 0; j < 3; ++j)
                    trainerSpell.ReqAbility[j] = packet.ReadUInt32("ReqAbility", i, j);

                trainerSpell.ReqSkillLine = packet.ReadUInt32("ReqSkillLine", i);
                trainerSpell.ReqSkillRank = packet.ReadUInt32("ReqSkillRank", i);
                packet.ReadByteE<TrainerSpellState>("Usable", i);

                tempList.Add(trainerSpell);
            }

            Trainer trainer = new Trainer();
            trainer.Greeting = packet.ReadWoWString("Greeting", titleLen);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 3);
            trainer.Id = (uint)packet.ReadInt32("TrainerID");
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 2);
            trainer.Type = packet.ReadInt32E<TrainerType>("TrainerType");

            packet.WriteGuid("TrainerGUID", guid);
            Storage.Trainers.Add(trainer, packet.TimeSpan);
            tempList.ForEach(trainerSpell =>
            {
                trainerSpell.TrainerId = trainer.Id;
                Storage.TrainerSpells.Add(trainerSpell, packet.TimeSpan);
            });

            var lastGossipOption = CoreParsers.NpcHandler.LastGossipOption;
            if (lastGossipOption.HasSelection)
                Storage.CreatureTrainers.Add(new CreatureTrainer { CreatureID = lastGossipOption.Guid.GetEntry(), MenuID = lastGossipOption.MenuId, OptionID = lastGossipOption.OptionIndex, TrainerID = trainer.Id }, packet.TimeSpan);
            else
                Storage.CreatureTrainers.Add(new CreatureTrainer { CreatureID = lastGossipOption.Guid.GetEntry(), MenuID = 0, OptionID = 0, TrainerID = trainer.Id }, packet.TimeSpan);
        }
    }
}
