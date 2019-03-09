﻿using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class MiscellaneousHandler
    {
        [Parser(Opcode.CMSG_ACCEPT_LEVEL_GRANT)]
        public static void HandleAcceptLevelGrant(Packet packet)
        {
            var guid = packet.StartBitStream(2, 7, 5, 4, 3, 0, 1, 6);
            packet.ParseBitStream(guid, 5, 3, 2, 7, 4, 1, 0, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_AREA_TRIGGER)]
        public static void HandleClientAreaTrigger(Packet packet)
        {
            var entry = packet.ReadEntry("Area Trigger Id");
            packet.ReadBit("Unk bit1");
            packet.ReadBit("Unk bit2");

            packet.AddSniffData(StoreNameType.AreaTrigger, entry.Key, "AREATRIGGER");
        }

        [Parser(Opcode.CMSG_BUY_BANK_SLOT)]
        public static void HandleBuyBankSlot(Packet packet)
        {
            var guid = packet.StartBitStream(7, 6, 1, 3, 2, 0, 4, 5);
            packet.ParseBitStream(guid, 3, 5, 1, 6, 7, 2, 0, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_CHALLENGE_MODE_REQUEST_LEADERS)]
        public static void HandleChallengeModeRequestLeaders(Packet packet)
        {
            packet.ReadInt32("unk16"); // 16
            packet.ReadInt32("unk20"); // 20
            packet.ReadInt32("unk24"); // 24
        }

        [Parser(Opcode.CMSG_QUERY_CORPSE_TRANSPORT)]
        public static void HandleQuesryCorpseTransport(Packet packet)
        {
            var guid = packet.StartBitStream(7, 6, 3, 0, 4, 1, 5, 2);
            packet.ParseBitStream(guid, 1, 6, 0, 5, 3, 2, 4, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_DEL_FRIEND)]
        public static void HandleDelFriend(Packet packet)
        {
            packet.ReadGuid("GUID");
        }

        [Parser(Opcode.CMSG_DEL_IGNORE)]
        public static void HandleDelIgnore(Packet packet)
        {
            packet.ReadGuid("GUID");
        }

        [Parser(Opcode.CMSG_GET_MIRROR_IMAGE_DATA)]
        public static void HandleGetMirrorImageData(Packet packet)
        {
            packet.ReadInt32("Display ID"); // 96
            var guid = packet.StartBitStream(0, 2, 1, 6, 5, 4, 7, 3);
            packet.ParseBitStream(guid, 6, 0, 3, 5, 4, 2, 1, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_INSPECT)]
        public static void HandleInspect(Packet packet)
        {
            var guid = packet.StartBitStream(0, 3, 7, 2, 5, 1, 4, 6);
            packet.ParseBitStream(guid, 3, 5, 2, 4, 1, 6, 0, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_INSPECT_HONOR_STATS)]
        public static void HandleInspectHonorStats(Packet packet)
        {
            var guid = packet.StartBitStream(4, 3, 6, 1, 0, 2, 5, 7);
            packet.ParseBitStream(guid, 0, 5, 1, 4, 2, 6, 7, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_LOADING_SCREEN_NOTIFY)]
        public static void HandleClientEnterWorld(Packet packet)
        {
            var mapId = packet.ReadUInt32<MapId>("MapID");
            packet.ReadBit("Loading");
            CoreParsers.MovementHandler.CurrentMapId = (uint)mapId;
            packet.AddSniffData(StoreNameType.Map, (int)mapId, "LOAD_SCREEN");
        }

        [Parser(Opcode.CMSG_LOG_DISCONNECT)]
        public static void HandleLogDisconnect(Packet packet)
        {
            packet.ReadUInt32("Disconnect Reason");
            // 4 is inability for client to decrypt RSA
            // 3 is not receiving "WORLD OF WARCRAFT CONNECTION - SERVER TO CLIENT"
            // 11 is sent on receiving opcode 0x140 with some specific data
        }

        [Parser(Opcode.CMSG_MINIMAP_PING)]
        public static void HandleMinimapPing(Packet packet)
        {
            packet.ReadSingle("Y");
            packet.ReadSingle("X");
            packet.ReadByte("byte24");
        }

        [Parser(Opcode.CMSG_NEUTRAL_PLAYER_SELECT_FACTION)]
        public static void HandleFactionSelect(Packet packet)
        {
            packet.ReadUInt32("Option");
        }

        [Parser(Opcode.CMSG_OPENING_CINEMATIC)]
        public static void HandleOpeningCinematic(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_PING)]
        public static void HandleClientPing(Packet packet)
        {
            packet.ReadUInt32("Latency");
            packet.ReadUInt32("Ping");
        }

        [Parser(Opcode.CMSG_DO_READY_CHECK)]
        public static void HandleDoReadyCheck(Packet packet)
        {
            packet.ReadByte("unk16"); // 16
        }

        [Parser(Opcode.CMSG_QUERY_COUNTDOWN_TIMER)]
        public static void HandleQueryCountdownTimer(Packet packet)
        {
            packet.ReadInt32("TimerType");
        }

        [Parser(Opcode.CMSG_READY_CHECK_RESPONSE)]
        public static void HandleReadyCheckResponse(Packet packet)
        {
            packet.ReadByte("unk16"); // 16
            var guid = new byte[8];
            guid[2] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            packet.ReadBit("unk17"); // 17
            guid[7] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();

            packet.ParseBitStream(guid, 1, 0, 3, 2, 4, 5, 7, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_RANDOM_ROLL)]
        public static void HandleRandomRoll(Packet packet)
        {
            packet.ReadInt32("unk20"); // 20
            packet.ReadInt32("unk16"); // 16
            packet.ReadByte("unk24"); // 24
        }

        [Parser(Opcode.CMSG_REALM_SPLIT)]
        public static void HandleClientRealmSplit(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_RECLAIM_CORPSE)]
        public static void HandleClientReclaimCorpse(Packet packet)
        {
            var guid = packet.StartBitStream(1, 5, 7, 2, 6, 3, 0, 4);
            packet.ParseBitStream(guid, 2, 5, 4, 6, 1, 0, 7, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_RESUME_TOKEN_ACK)]
        public static void HandleResumeTokenAck(Packet packet)
        {
            packet.ReadInt32("unk20"); // 20
        }

        [Parser(Opcode.CMSG_RESURRECT_RESPONSE)]
        public static void HandleResurrectResponse(Packet packet)
        {
            packet.ReadInt32("Response");
            var guid = packet.StartBitStream(3, 0, 6, 4, 5, 2, 1, 7);
            packet.ParseBitStream(guid, 7, 0, 1, 3, 4, 6, 2, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_SCRIPT_CAST)]
        public static void HandleScriptcast(Packet packet)
        {
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.CMSG_SET_PVP)]
        public static void HandleSetPVP(Packet packet)
        {
            packet.ReadBool("PvP");
        }

        [Parser(Opcode.CMSG_SET_LOOT_SPECIALIZATION)]
        public static void HandleSetLootSpec(Packet packet)
        {
            packet.ReadInt32("SpecializationID"); 
            // 0 Default by spec
        }

        [Parser(Opcode.CMSG_SET_SELECTION)]
        public static void HandleSetSelection(Packet packet)
        {
            var guid = packet.StartBitStream(7, 6, 5, 4, 3, 2, 1, 0);
            packet.ParseBitStream(guid, 0, 7, 3, 5, 1, 4, 6, 2);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_SET_TITLE)]
        public static void HandleSetTitle(Packet packet)
        {
            packet.ReadInt32("Title");
        }

        [Parser(Opcode.CMSG_SPELL_CLICK)]
        public static void HandleSpellClick(Packet packet)
        {
            var guidBytes = new byte[8];
            packet.StartBitStream(guidBytes, 7, 4, 0, 3, 6, 5);
            packet.ReadBit("unk");
            packet.StartBitStream(guidBytes, 1, 2);
            packet.ParseBitStream(guidBytes, 6, 1, 5, 4, 7, 2, 3, 0);

            packet.WriteGuid("Guid", guidBytes);

            var guid = new WowGuid64(BitConverter.ToUInt64(guidBytes, 0));
            if (guid.GetObjectType() == ObjectType.Unit)
                Storage.NpcSpellClicks.Add(guid, packet.TimeSpan);
        }

        [Parser(Opcode.CMSG_SUGGESTION_SUBMIT)]
        public static void HandleSuggestionSubmit(Packet packet)
        {
            packet.ReadSingle("unk28"); // 28
            packet.ReadSingle("unk32"); // 32
            packet.ReadSingle("unk24"); // 24
            packet.ReadInt32("unk16"); // 16
            packet.ReadSingle("unk20"); // 20
            packet.ReadWoWString("str", packet.ReadBits(10));
        }

        [Parser(Opcode.CMSG_SUPPORT_TICKET_SUBMIT_BUG)]
        public static void HandleSubmitBug(Packet packet)
        {
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            pos.O = packet.ReadSingle();
            packet.ReadInt32("MapID");
            var length = packet.ReadBits(10);
            packet.ReadWoWString("Text", length);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_TIME_SYNC_RESPONSE)]
        public static void HandleTimeSyncResponse(Packet packet)
        {
            packet.ReadUInt32("MCounter");
            packet.ReadUInt32("Client Ticks");
        }

        [Parser(Opcode.CMSG_WHO)]
        public static void HandleWhoRequest(Packet packet)
        {
            packet.ReadInt32("ClassMask");
            packet.ReadInt32("RaceMask");
            packet.ReadInt32("Max Level");
            packet.ReadInt32("Min Level");


            packet.ReadBit("bit2C4");
            packet.ReadBit("bit2C6");
            var bit2D4 = packet.ReadBit();
            var bits1AB = packet.ReadBits(9);
            packet.ReadBit("bit2C5");
            var PlayerNameLen = packet.ReadBits(6);
            var zones = packet.ReadBits(4);
            var bits73 = packet.ReadBits(9);
            var guildNameLen = packet.ReadBits(7);
            var patterns = packet.ReadBits(3);

            var bits2B8 = new uint[patterns];
            for (var i = 0; i < patterns; ++i)
                bits2B8[i] = packet.ReadBits(7);

            for (var i = 0; i < patterns; ++i)
                packet.ReadWoWString("Pattern", bits2B8[i], i);

            packet.ReadWoWString("string1AB", bits1AB);

            for (var i = 0; i < zones; ++i)
                packet.ReadInt32<ZoneId>("Zone Id");

            packet.ReadWoWString("Player Name", PlayerNameLen);

            packet.ReadWoWString("string73", bits73);

            packet.ReadWoWString("Guild Name", guildNameLen);

            if (bit2D4)
            {
                packet.ReadInt32("Int2C8");
                packet.ReadInt32("Int2D0");
                packet.ReadInt32("Int2CC");
            }
        }

        [Parser(Opcode.CMSG_KEYBOUND_OVERRIDE)]
        public static void HandleKeyBoundOverride(Packet packet)
        {
            packet.ReadInt16("unk16");
        }

        [Parser(Opcode.CMSG_SET_PET_SLOT)]
        public static void HandleSetPetSlot(Packet packet)
        {
            packet.ReadInt32("unk24");
            packet.ReadByte("unk28");
            var guid = packet.StartBitStream(5, 7, 3, 2, 6, 1, 0, 4);
            packet.ParseBitStream(guid, 0, 3, 2, 6, 5, 7, 4, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_UNLEARN_SPECIALIZATION)]
        public static void HandleUnlearnSpecialization(Packet packet)
        {
            packet.ReadByte("unk");
        }

        [Parser(Opcode.SMSG_AREA_TRIGGER_DENIED)]
        public static void HandleAreaTriggerDenied(Packet packet)
        {
            packet.ReadInt32("AreaTrigger");
            packet.ReadBit("unk16");
        }

        [Parser(Opcode.SMSG_CAMERA_SHAKE)]
        public static void HandleCameraShake(Packet packet)
        {
            packet.ReadInt32("Int20"); // 20
            packet.ReadInt32("Int16"); // 16
        }

        [Parser(Opcode.SMSG_CANCEL_SCENE)]
        public static void HandleCancelScene(Packet packet)
        {
            var hasData = !packet.ReadBit("!hasData");
            if (hasData)
                packet.ReadInt32("Data");
        }

        [Parser(Opcode.SMSG_CHALLENGE_MODE_ALL_MAP_STATS)]
        public static void HandleChallengeModeallMapStats(Packet packet)
        {
            var count = packet.ReadBits("Count", 19); // 16
            var unk68 = new UInt32[count];
            for (var i = 0; i < count; i++)
                unk68[i] = packet.ReadBits(23);
            for (var i = 0; i < count; i++)
            {
                packet.ReadPackedTime("Time", i);
                packet.ReadInt32("unk32", i); // 32
                packet.ReadInt32("unk20", i); // 20
                packet.ReadInt32("unk28", i); // 28
                for (var j = 0; j < unk68[i]; j++)
                    packet.ReadInt16("unk72", i, j);
                packet.ReadInt32("unk24", i); // 24
            }
        }

        [Parser(Opcode.SMSG_CHALLENGE_MODE_REQUEST_LEADERS_RESULT)]
        public static void HandleChallengeModeRequestLeadersResult(Packet packet)
        {
            packet.ReadInt32("unk36"); // 36
            packet.ReadInt32("unk32"); // 32
            packet.ReadInt32("unk40"); // 40
            var count44 = packet.ReadBits("count44", 19); // 44
            var count240 = new uint[count44];
            var guids44 = new byte[count44][][];
            for (var i = 0; i < count44; i++)
            {
                count240[i] = packet.ReadBits("unk240", 20, i); // 240
                guids44[i] = new byte[count240[i]][];
                for (var j = 0; j < count240[i]; j++)
                    guids44[i][j] = packet.StartBitStream(5, 2, 3, 0, 7, 1, 4, 6);
            }
            var count16 = packet.ReadBits("count16", 19); // 16
            var count212 = new uint[count16];
            var guids16 = new byte[count16][][];
            for (var i = 0; i < count16; i++)
            {
                count212[i] = packet.ReadBits("count212", 20, i); // 212
                guids16[i] = new byte[count212[i]][];
                for (var j = 0; j < count212[i]; j++)
                    guids16[i][j] = packet.StartBitStream(5, 3, 0, 1, 2, 4, 6, 7);
            }
            for (var i = 0; i < count16; i++)
            {
                for (var j = 0; j < count212[i]; j++)
                {
                    packet.ParseBitStream(guids16[i][j], 4, 3, 6, 7, 2, 1);
                    packet.ReadInt32("RealmID", i, j); // 276
                    packet.ReadInt32("unk292", i, j); // 292
                    packet.ReadInt32("RealmID", i, j); // 260
                    packet.ParseBitStream(guids16[i][j], 0, 5);
                    packet.WriteGuid("Guids16", guids16[i][j], i, j);
                }
                packet.ReadPackedTime("Time", i);
                packet.ReadInt32("unk20", i); // 20
                packet.ReadInt32("unk36", i); // 36
                packet.ReadInt32("unk52", i); // 52
                packet.ReadInt32("unk196", i); // 196
            }
            for (var i = 0; i < count44; i++)
            {
                for (var j = 0; j < count240[i]; j++)
                {
                    packet.ParseBitStream(guids44[i][j], 3);
                    packet.ReadInt32("unk320", i, j); // 320
                    packet.ReadInt32("unk304", i, j); // 304
                    packet.ParseBitStream(guids44[i][j], 1, 5, 0);
                    packet.ReadInt32("unk288", i, j); // 288
                    packet.ParseBitStream(guids44[i][j], 4, 7, 6, 2);
                    packet.WriteGuid("Guids44", guids44[i][j], i, j);
                }
                packet.ReadInt32("unk64", i); // 64
                packet.ReadInt32("unk224", i); // 224
                packet.ReadInt32("unk48", i); // 48
                packet.ReadInt32("unk80", i); // 80
                packet.ReadPackedTime("Time", i);
            }
        }

        [Parser(Opcode.SMSG_CHALLENGE_MODE_REWARDS)]
        public static void HandleChallengeModeRewards(Packet packet)
        {
            var count16 = packet.ReadBits("count16", 21);
            var count24 = new uint[count16];
            var unk68 = new uint[count16][];
            var unk28 = new uint[count16][];
            for (var i = 0; i < count16; i++)
            {
                count24[i] = packet.ReadBits("count24", 20, i);
                unk68[i] = new uint[count24[i]];
                unk28[i] = new uint[count24[i]];
                for (var j = 0; j < count24[i]; j++)
                {
                    unk68[i][j] = packet.ReadBits("unk68", 21, i, j); // 68
                    unk28[i][j] = packet.ReadBits("unk28", 20, i, j); // 28
                }
            }
            var unk32 = packet.ReadBits("unk32", 20); // 32
            for (var i = 0; i < count16; i++)
            {
                for (var j = 0; j < count24[i]; j++)
                {
                    for (var k = 0; k < unk28[i][j]; k++)
                    {
                        packet.ReadInt32("unk72", i, j, k); // 72
                        packet.ReadInt32("unk76", i, j, k); // 76
                        packet.ReadInt32("unk68", i, j, k); // 68
                    }
                    packet.ReadInt32("unk84", i, j); // 84
                    for (var k = 0; k < unk68[i][j]; k++)
                    {
                        packet.ReadInt32("unk132", i, j, k); // 132
                        packet.ReadInt32("unk136", i, j, k); // 136
                    }
                }
                packet.ReadInt32("unk20", i); // 20
            }
            for (var i = 0; i < unk32; i++)
            {
                packet.ReadInt32("unk40", i); // 40
                packet.ReadInt32("unk36", i); // 36
                packet.ReadInt32("unk44", i); // 44
            }
        }

        [Parser(Opcode.SMSG_CHEAT_IGNORE_DIMISHING_RETURNS)]
        public static void HandleCheatIgnoreDimishingReturns(Packet packet)
        {
            packet.ReadBool("Result");
        }

        [Parser(Opcode.SMSG_CLIENTCACHE_VERSION)]
        public static void HandleClientCacheVersion(Packet packet)
        {
            packet.ReadUInt32("Version");
        }

        [Parser(Opcode.SMSG_CROSSED_INEBRIATION_THRESHOLD)]
        public static void HandleCrossedInerbriationThreshold(Packet packet)
        {
            var guid = packet.StartBitStream(0, 4, 2, 6, 5, 1, 3, 7);
            packet.ParseBitStream(guid, 3);
            packet.ReadUInt32<ItemId>("Entry"); // 24
            packet.ReadUInt32E<DrunkenState>("Drunken State"); // 28
            packet.ParseBitStream(guid, 4, 6, 7, 0, 2, 5, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_DANCE_STUDIO_CREATE_RESULT)]
        public static void HandleDanceStudioCreateResult(Packet packet)
        {
            packet.ReadUInt32("Dword8"); // 8
            packet.ReadUInt32("Dword5"); // 5
            packet.ReadUInt32("Dword6"); // 6
            packet.ReadUInt32("Dword7"); // 7
            packet.ReadBit("Bit in Byte16");
        }

        [Parser(Opcode.SMSG_FEATURE_SYSTEM_STATUS)]
        public static void HandleFeatureSystemStatus(Packet packet)
        {
            packet.ReadInt32("Scroll of Resurrections Per Day");
            packet.ReadInt32("Scroll of Resurrections Remaining");

            packet.ReadInt32("Realm Id?");
            packet.ReadByte("Complain System Status");
            packet.ReadInt32("Unused Int32");

            packet.ReadBit("bit26");
            packet.ReadBit("Shop Enabled");
            packet.ReadBit("unk52");
            packet.ReadBit("unk17");
            packet.ReadBit("unk16");
            packet.ReadBit("unk44");
            packet.ReadBit("unk76");
            var sessionTimeAlert = packet.ReadBit("Session Time Alert");
            packet.ReadBit("unk46");
            var quickTicket = packet.ReadBit("EuropaTicketSystemEnabled");
            if (sessionTimeAlert)
            {
                packet.ReadInt32("unk60");
                packet.ReadInt32("unk64");
                packet.ReadInt32("unk68");
            }
            if (quickTicket)
            {
                packet.ReadInt32("unk36");
                packet.ReadInt32("unk32");
                packet.ReadInt32("unk24");
                packet.ReadInt32("unk28");
            }
        }

        [Parser(Opcode.SMSG_CUSTOM_LOAD_SCREEN)]
        public static void HandleCustomLoadScreen(Packet packet)
        {
            // unk = spell ID 130321 (The Mission: Teleport Player)
            packet.ReadInt32("Spell");
        }

        [Parser(Opcode.SMSG_DISPLAY_PROMOTION)]
        public static void HandleDisplayPromotion(Packet packet)
        {
            packet.ReadInt32("Dword4");
        }

        [Parser(Opcode.SMSG_FACTION_BONUS_INFO)]
        public static void HandleFactionBonusInfo(Packet packet)
        {
            for (var i = 0; i < 256; i++)
                packet.ReadBitVisible("Byte16", i);
        }

        [Parser(Opcode.SMSG_FEATURE_SYSTEM_STATUS_GLUE_SCREEN)]
        public static void HandleUnk121E(Packet packet)
        {
            packet.ReadBit("Bit in Byte16"); // 16
            packet.ReadBit("Bit in Byte18"); // 18
            packet.ReadBit("Bit in Byte17"); // 17
        }

        [Parser(Opcode.SMSG_FORCE_SET_VEHICLE_REC_ID)]
        public static void HandleForceSetVehicleRecId(Packet packet)
        {
            var guid = packet.StartBitStream(5, 7, 2, 1, 4, 0, 3, 6);
            packet.ParseBitStream(guid, 5, 7, 4, 6, 2, 1, 3, 0);
            packet.ReadInt32("unk24"); // 24
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_GMRESPONSE_RECEIVED)]
        public static void HandleSGMResponseReceived(Packet packet)
        {
            var count = packet.ReadBits("count", 20);
            var guid = new byte[count][];
            var unk2085 = new bool[count];
            var unk1045 = new bool[count];
            var unk1048 = new bool[count];
            var unk1061 = new uint[count];
            var unk17 = new uint[count];
            for (var i = 0; i < count; i++)
            {
                guid[i] = new byte[8];
                unk17[i] = packet.ReadBits("unk17*4", 11, i);
                unk2085[i] = !packet.ReadBit("!unk2085*4", i);
                unk1061[i] = packet.ReadBits("unk1061*4", 10, i);
                unk1045[i] = !packet.ReadBit("!unk1045*4", i);
                unk1048[i] = !packet.ReadBit("!unk1048*4", i);
                guid[i] = packet.StartBitStream(5, 0, 1, 2, 4, 7, 6, 3);
            }
            for (var i = 0; i < count; i++)
            {
                packet.ParseBitStream(guid[i], 7, 0, 5, 4, 3, 6, 2, 1);
                packet.ReadInt32("unk36", i);
                packet.ReadWoWString("Ticket", unk17[i], i);
                packet.ReadInt32("unk52", i);
                if (unk2085[i])
                    packet.ReadInt32("unk2085*4", i);
                packet.ReadInt32("unk20", i);
                if (unk1045[i])
                    packet.ReadInt32("unk1045*4", i);
                packet.ReadWoWString("Response", unk1061[i], i);
                packet.WriteGuid("Guid", guid[i], i);
            }
            packet.ReadInt32("unk32");
            packet.ReadInt32("unk36");
        }

        [Parser(Opcode.SMSG_GOD_MODE)]
        public static void HandlePetGodMode(Packet packet)
        {
            packet.ReadBool("GodMode");
        }

        [Parser(Opcode.SMSG_INITIAL_SETUP)]
        public static void HandleInitialsetup(Packet packet)
        {
            var hasData = !packet.ReadBit("!hasData");
            var count = packet.ReadBits("count", 24);
            packet.ReadInt32("unk20");
            packet.ReadByte("unk40");
            if (hasData)
            {
                packet.ReadInt32Visible("unk16");
            }
            for (var i = 0; i < count; i++)
            {
                packet.ReadByteVisible("unkb28", i);
            }
            packet.ReadByte("unk41");
        }

        [Parser(Opcode.SMSG_INSPECT_HONOR_STATS)]
        public static void HandleInspectHonorStatsResponse(Packet packet)
        {
            packet.ReadInt32("Life Time Kills"); // 24
            packet.ReadInt16("Yesterday Honorable Kills"); // 28
            packet.ReadInt16("Today Honorable Kills"); // 30
            packet.ReadByte("Lifetime Max Rank"); // 32

            var guid = packet.StartBitStream(2, 1, 6, 4, 5, 3, 7, 0);
            packet.ParseBitStream(guid, 1, 3, 6, 7, 2, 4, 5, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_INVALIDATE_PLAYER)]
        public static void HandleInvalidatePlayer(Packet packet)
        {
            var guid = packet.StartBitStream(6, 3, 1, 2, 7, 5, 0, 4);
            packet.ParseBitStream(guid, 7, 1, 2, 3, 6, 0, 4, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_LEARNED_DANCE_MOVES)]
        public static void HandleLearnedDanceMoves(Packet packet)
        {
            packet.ReadInt32("Danc Move");
            packet.ReadBits("unk", 19);
        }

        [Parser(Opcode.SMSG_MINIMAP_PING)]
        public static void HandleSMimimapPing(Packet packet)
        {
            packet.ReadSingle("Y");
            packet.ReadSingle("X");
            var guid = packet.StartBitStream(0, 5, 2, 7, 1, 3, 6, 4);
            packet.ParseBitStream(guid, 6, 5, 7, 2, 0, 3, 1, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MIRROR_IMAGE_CREATURE_DATA)]
        public static void HandleSMirrorimageCreatureData(Packet packet)
        {
            var guid = packet.StartBitStream(0, 1, 3, 5, 7, 6, 4, 2);
            packet.ParseBitStream(guid, 0, 3, 6, 5, 7);
            packet.ReadInt32("DisplayID");
            packet.ParseBitStream(guid, 4, 2, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MIRROR_IMAGE_COMPONENTED_DATA)]
        public static void HandleMirrorImageData(Packet packet)
        {
            var guid24 = new byte[8];
            var guid64 = new byte[8];

            guid24[4] = packet.ReadBit();
            guid64[3] = packet.ReadBit();
            guid64[6] = packet.ReadBit();
            guid24[0] = packet.ReadBit();
            guid64[7] = packet.ReadBit();
            guid24[1] = packet.ReadBit();
            guid24[5] = packet.ReadBit();
            guid64[2] = packet.ReadBit();
            guid64[1] = packet.ReadBit();
            guid24[7] = packet.ReadBit();
            guid64[4] = packet.ReadBit();
            guid64[0] = packet.ReadBit();
            guid24[2] = packet.ReadBit();
            guid64[5] = packet.ReadBit();
            guid24[3] = packet.ReadBit();
            var count44 = packet.ReadBits("count44", 22);
            guid24[6] = packet.ReadBit();

            packet.ReadByte("unk60"); // 60
            packet.ReadInt32("Display ID"); // 144
            packet.ReadByte("unk32"); // 32

            packet.ParseBitStream(guid64, 6, 4);
            packet.ParseBitStream(guid24, 7);
            packet.ParseBitStream(guid64, 1);
            packet.ParseBitStream(guid24, 3);

            packet.ReadByte("unk16"); // 16

            packet.ParseBitStream(guid24, 2, 0);

            packet.ReadByte("unk61"); // 61
            packet.ReadByte("unk63"); // 63

            packet.ParseBitStream(guid64, 7);

            for (var i = 0; i < count44; i++)
                packet.ReadUInt32("Item Entry", i); // 192

            packet.ParseBitStream(guid24, 4);

            packet.ReadByte("unk62"); // 62
            packet.ReadByte("unk72"); // 72
            packet.ReadByte("unk40"); // 40

            packet.ParseBitStream(guid24, 5);
            packet.ParseBitStream(guid64, 3, 2);
            packet.ParseBitStream(guid24, 1);
            packet.ParseBitStream(guid64, 0, 5);
            packet.ParseBitStream(guid24, 6);

            packet.WriteGuid("Unit Guid", guid24);
            packet.WriteGuid("Guild Guid", guid64);
        }

        [Parser(Opcode.SMSG_PLAY_MUSIC)]
        public static void HandleSoundMessages(Packet packet)
        {
            var music = packet.ReadUInt32("Music ID");
            Storage.Sounds.Add(music, packet.TimeSpan);
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_PLAY_OBJECT_SOUND)]
        public static void HandlePlayObjectSound(Packet packet)
        {
            var guid1 = new byte[8];
            var guid2 = new byte[8];

            guid2[5] = packet.ReadBit();
            guid1[7] = packet.ReadBit();
            guid1[0] = packet.ReadBit();
            guid1[3] = packet.ReadBit();
            guid2[1] = packet.ReadBit();
            guid1[4] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid2[2] = packet.ReadBit();
            guid2[4] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            guid1[5] = packet.ReadBit();
            guid1[1] = packet.ReadBit();
            guid1[6] = packet.ReadBit();
            guid1[2] = packet.ReadBit();
            guid2[6] = packet.ReadBit();
            guid2[0] = packet.ReadBit();

            packet.ReadXORBytes(guid1, 6, 2);
            packet.ReadXORBytes(guid2, 2, 5);
            packet.ReadXORBytes(guid1, 7, 5, 3, 1);
            packet.ReadXORBytes(guid2, 3, 1);

            var sound = packet.ReadUInt32("Sound Id");

            packet.ReadXORByte(guid1, 4);
            packet.ReadXORBytes(guid2, 4, 7, 0, 6);
            packet.ReadXORByte(guid1, 0);

            packet.WriteGuid("Guid 1", guid1);
            packet.WriteGuid("Guid 2", guid2);

            Storage.Sounds.Add(sound, packet.TimeSpan);
        }

        [Parser(Opcode.SMSG_PLAY_ONE_SHOT_ANIM_KIT)]
        public static void HandlePlayOneShotAnimKit(Packet packet)
        {
            /*
             ServerToClient: SMSG_UNK_043E (0x043E) Length: 9
             unk24: 1575 (0x0627)
             Guid: Full: 0xF130D54400000251 Type: Unit Entry: 54596 Low: 593
             */
            var guid = packet.StartBitStream(3, 1, 7, 6, 0, 4, 5, 2);
            packet.ParseBitStream(guid, 3, 6, 1, 4);
            packet.ReadInt16("AnimKit.dbc Id"); // 24
            packet.ParseBitStream(guid, 2, 7, 5, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_PLAY_SCENE)]
        public static void HandlePlayScene(Packet packet)
        {
            // target for spell 131057
            packet.ReadSingle("unk2ch"); // 2ch
            packet.ReadSingle("unk30h"); // 30h
            packet.ReadSingle("unk28h"); // 28h

            var unk7 = !packet.ReadBit("!unk7"); // 7
            var unk8 = !packet.ReadBit("!unk8"); // 8

            var unk34h = !packet.ReadBit("!unk34h"); // 34h

            var unk6 = !packet.ReadBit("!unk6"); // 6
            var unk9 = !packet.ReadBit("!unk9"); // 9
            packet.ReadBit("!unk4"); // 4

            var guid = packet.StartBitStream(3, 2, 6, 4, 7, 1, 5, 0);
            packet.ParseBitStream(guid, 6, 3, 5, 4, 1, 2, 0, 7);

            if (unk7)
                packet.ReadInt32("unk7");

            if (unk34h)
                packet.ReadSingle("unk34h");

            if (unk6)
                packet.ReadInt32("unk6");

            if (unk8)
                packet.ReadInt32("unk8");

            if (unk9)
                packet.ReadInt32("unk9");

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_PLAY_SOUND)]
        public static void HandlePlaySound(Packet packet)
        {
            var guid = packet.StartBitStream(2, 3, 7, 6, 0, 5, 4, 1);
            var sound = packet.ReadUInt32("Sound Id");
            packet.ParseBitStream(guid, 3, 2, 4, 7, 5, 0, 6, 1);
            packet.WriteGuid("Guid", guid);

            Storage.Sounds.Add(sound, packet.TimeSpan);
        }

        [Parser(Opcode.SMSG_PLAYED_TIME)]
        public static void HandlePlayedTime(Packet packet)
        {
            packet.ReadInt32("unk16");
            packet.ReadInt32("unk20");
            packet.ReadBit("unk24");
        }

        [Parser(Opcode.SMSG_PLAYER_BOUND)]
        public static void HandlePlayerBound(Packet packet)
        {
            var guid = packet.StartBitStream(2, 4, 0, 3, 6, 7, 5, 1);
            packet.ParseBitStream(guid, 6, 1, 2, 3, 4, 5, 7, 0);
            packet.ReadUInt32<AreaId>("Area ID");
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_PLAYER_VEHICLE_ENTER)]
        public static void HandlePlayerVehicleEnter(Packet packet)
        {
            var guid = packet.StartBitStream(5, 7, 3, 0, 2, 4, 6, 1);
            packet.ParseBitStream(guid, 5, 3, 1, 2, 7, 0, 6, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_PONG)]
        public static void HandleServerPong(Packet packet)
        {
            packet.ReadUInt32("Ping");
        }

        [Parser(Opcode.SMSG_PRE_RESSURECT)]
        public static void HandlePreRessurect(Packet packet)
        {
            var guid = packet.StartBitStream(1, 7, 5, 2, 6, 0, 3, 4);
            packet.ParseBitStream(guid, 5, 1, 7, 0, 6, 4, 2, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_PROPOSE_LEVEL_GRANT)]
        public static void HandleProposeLevelGrant(Packet packet)
        {
            var guid = packet.StartBitStream(6, 7, 2, 5, 3, 0, 1, 4);
            packet.ParseBitStream(guid, 2, 5, 6, 7, 1, 4, 3, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_READY_CHECK_STARTED)]
        public static void HandleSReadyCheckStarted(Packet packet)
        {
            var guid16 = new byte[8];
            var guid32 = new byte[8];
            guid16[4] = packet.ReadBit();
            guid16[2] = packet.ReadBit();
            guid32[4] = packet.ReadBit();
            guid16[3] = packet.ReadBit();
            guid16[7] = packet.ReadBit();
            guid16[1] = packet.ReadBit();
            guid16[0] = packet.ReadBit();
            guid32[6] = packet.ReadBit();
            guid32[5] = packet.ReadBit();
            guid16[6] = packet.ReadBit();
            guid16[5] = packet.ReadBit();
            guid32[0] = packet.ReadBit();
            guid32[1] = packet.ReadBit();
            guid32[2] = packet.ReadBit();
            guid32[7] = packet.ReadBit();
            guid32[3] = packet.ReadBit();

            packet.ReadInt32("unk24"); // 24

            packet.ParseBitStream(guid16, 2, 7, 3);
            packet.ParseBitStream(guid32, 4);
            packet.ParseBitStream(guid16, 1, 0);
            packet.ParseBitStream(guid32, 1, 2, 6, 5);
            packet.ParseBitStream(guid16, 6);
            packet.ParseBitStream(guid32, 0);
            packet.ReadByte("unk3"); // 3
            packet.ParseBitStream(guid32, 7);
            packet.ParseBitStream(guid16, 4);
            packet.ParseBitStream(guid32, 3);
            packet.ParseBitStream(guid16, 5);
            packet.WriteGuid("Guid16", guid16);
            packet.WriteGuid("Guid32", guid32);

        }

        [Parser(Opcode.SMSG_READY_CHECK_COMPLETED)]
        public static void Handlereadycheck(Packet packet)
        {
            var guid = packet.StartBitStream(4, 2, 5, 7, 1, 0, 3, 6);
            packet.ParseBitStream(guid, 6, 0, 3, 1, 5);
            packet.ReadByte("Party Index"); // 3
            packet.ParseBitStream(guid, 7, 2, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_READY_CHECK_RESPONSE)]
        public static void HandleSReadyCheckResponse(Packet packet)
        {
            var guid16 = new byte[8];
            var guid24 = new byte[8];

            guid16[4] = packet.ReadBit();
            guid24[5] = packet.ReadBit();
            guid24[3] = packet.ReadBit();
            packet.ReadBit("unk32"); // 32
            guid16[2] = packet.ReadBit();
            guid24[6] = packet.ReadBit();
            guid16[3] = packet.ReadBit();
            guid24[0] = packet.ReadBit();
            guid24[1] = packet.ReadBit();
            guid16[1] = packet.ReadBit();
            guid16[5] = packet.ReadBit();
            guid24[7] = packet.ReadBit();
            guid24[4] = packet.ReadBit();
            guid16[6] = packet.ReadBit();
            guid24[2] = packet.ReadBit();
            guid16[0] = packet.ReadBit();
            guid16[7] = packet.ReadBit();

            packet.ParseBitStream(guid24, 4, 2, 1);
            packet.ParseBitStream(guid16, 4, 2);
            packet.ParseBitStream(guid24, 0);
            packet.ParseBitStream(guid16, 5, 3);
            packet.ParseBitStream(guid24, 7);
            packet.ParseBitStream(guid16, 6, 1);
            packet.ParseBitStream(guid24, 6, 3, 5);
            packet.ParseBitStream(guid16, 0, 7);

            packet.WriteGuid("Guid16", guid16);
            packet.WriteGuid("Guid24", guid24);
        }

        [Parser(Opcode.SMSG_RAID_TARGET_UPDATE)]
        public static void HandleSRaidTargetUpdate(Packet packet)
        {
            packet.ReadByte("unk16"); // 16
            packet.ReadInt32("unk5"); // 5
            var count = packet.ReadBits("count", 3); // 24
            var guid7 = new byte[count][];
            for (var i = 0; i < count; i++)
                guid7[i] = packet.StartBitStream(6, 2, 7, 1, 4, 3, 5, 0);

            for (var i = 0; i < count; i++)
            {
                packet.ParseBitStream(guid7[i], 6);
                packet.ReadSingle("unk10h", i); // 10h
                packet.ParseBitStream(guid7[i], 2);
                packet.ReadSingle("unk0Ch", i); // 0Ch
                packet.ParseBitStream(guid7[i], 7, 5, 0, 4);
                packet.ReadSingle("unk14h", i); // 14h
                packet.ParseBitStream(guid7[i], 3, 1);
                packet.ReadInt32("unk8"); // 8
                packet.WriteGuid("Guid", guid7[i]);
            }
        }

        [Parser(Opcode.SMSG_RANDOM_ROLL)]
        public static void HandleSRandomRoll(Packet packet)
        {
            packet.ReadInt32("unk6"); // 6
            packet.ReadInt32("unk7"); // 7
            packet.ReadInt32("unk8"); // 8
            var guid = packet.StartBitStream(0, 6, 7, 1, 4, 5, 2, 3);
            packet.ParseBitStream(guid, 5, 4, 2, 0, 3, 1, 6, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_REQUEST_CEMETERY_LIST_RESPONSE)]
        public static void HandleRequestCemeteryListResponse(Packet packet)
        {
            var count = packet.ReadBits("Count", 22);
            packet.ReadBit("Is MicroDungeon"); // Named in WorldMapFrame.lua
            for (int i = 0; i < count; i++)
                packet.ReadInt32("Cemetery Id", i); // 6
        }

        [Parser(Opcode.SMSG_RESUME_TOKEN)]
        public static void HandleResumeToken(Packet packet)
        {
            packet.ReadInt32("unk20"); // 20
            packet.ReadBits("unk16", 2); // 16
        }

        [Parser(Opcode.SMSG_RESURRECT_REQUEST)]
        public static void HandleResurrectRequest(Packet packet)
        {
            packet.ReadUInt32("ResurrectOffererVirtualRealmAddress"); // 17
            packet.ReadInt32("PetNumber"); // 18
            packet.ReadUInt32("SpellID"); // 20

            var guid = new byte[8];

            guid[3] = packet.ReadBit();
            packet.ReadBit("Sickness"); // 76
            packet.ReadBit("UseTimer"); // 77
            guid[1] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[7] = packet.ReadBit();

            var len = packet.ReadBits(6);
            packet.ParseBitStream(guid, 7, 3, 5);
            packet.ReadWoWString("Name", len);
            packet.ParseBitStream(guid, 2, 4, 1, 6, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_SET_AI_ANIM_KIT)]
        public static void HandleSetAIAnimKit(Packet packet)
        {
            var guid = packet.StartBitStream(5, 4, 1, 3, 0, 2, 6, 7);
            packet.ParseBitStream(guid, 0, 1, 3, 7, 2, 4, 5);
            packet.ReadInt16("AnimKit.dbc Id"); // 48
            packet.ParseBitStream(guid, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_SET_ANIM_TIER)]
        public static void HandleSetAnimTier(Packet packet)
        {
            packet.ReadBits("unk24", 3);
            var guid = packet.StartBitStream(7, 4, 2, 1, 3, 5, 6, 0);
            packet.ParseBitStream(guid, 7, 5, 1, 4, 6, 2, 0, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_SET_DF_FAST_LAUNCH_RESULT)]
        public static void HandleSetDFFastLaunchresult(Packet packet)
        {
            packet.ReadBool("Result");
        }

        [Parser(Opcode.SMSG_SET_FACTION_AT_WAR)]
        public static void HandleSetFactionAtWar(Packet packet)
        {
            packet.ReadByte("unk16"); // 16
            packet.ReadBits("unk17", 10); // 17
        }

        [Parser(Opcode.SMSG_SET_FACTION_STANDING)]
        public static void HandleSetFactionStanding(Packet packet)
        {
            packet.ReadBit("unk16"); // 16
            var count = packet.ReadBits("Count", 21);
            for (var i = 0; i < count; i++)
            {
                packet.ReadInt32("unk28", i); // 28
                packet.ReadInt32("unk24", i); // 24
            }
            packet.ReadSingle("unk28h"); // 28h
            packet.ReadSingle("unk24h"); // 24h
        }

        [Parser(Opcode.SMSG_SET_PCT_SPELL_MODIFIER)]
        public static void HandleSetPctSpellModifier(Packet packet)
        {
            var count = packet.ReadBits("count", 22);
            var cnt = new uint[count];
            for (var i = 0; i < count; i++)
                cnt[i] = packet.ReadBits("cnt", 21, i);
            for (var i = 0; i < count; i++)
            {
                for (var j = 0; j < cnt[i]; j++)
                {
                    packet.ReadSingle("Amount", i, j);
                    packet.ReadByte("Spell Mask bitpos", i, j);
                }
                packet.ReadByteE<SpellModOp>("Spell Mod", i);
            }
        }

        [Parser(Opcode.SMSG_SET_TIME_ZONE_INFORMATION)]
        public static void HandleServerTimezone(Packet packet)
        {
            var Location2Lenght = packet.ReadBits(7);
            var Location1Lenght = packet.ReadBits(7);

            packet.ReadWoWString("Timezone Location1", Location1Lenght);
            packet.ReadWoWString("Timezone Location2", Location2Lenght);
        }

        [Parser(Opcode.SMSG_SET_VIGNETTE)]
        public static void HandleSetVignette(Packet packet)
        {
            var count1 = packet.ReadBits("count1", 24);
            packet.ReadBit("unk16");
            var count2 = packet.ReadBits("count36", 24);
            var guid2 = new byte[count2][];
            for (var i = 0; i < count2; i++)
            {
                guid2[i] = new byte[8];
                guid2[i] = packet.StartBitStream(4, 7, 3, 2, 6, 0, 5, 1);
            }
            var guid3 = new byte[count1][];
            for (var i = 0; i < count1; i++)
            {
                guid3[i] = new byte[8];
                guid3[i] = packet.StartBitStream(5, 4, 1, 7, 0, 6, 2, 3);
            }
            var count3 = packet.ReadBits("count3", 24);
            var guid4 = new byte[count3][];
            for (var i = 0; i < count3; i++)
            {
                guid4[i] = new byte[8];
                guid4[i] = packet.StartBitStream(1, 4, 3, 6, 2, 0, 7, 5);
            }
            var count4 = packet.ReadBits("count4", 20);
            var guid5 = new byte[count4][];
            for (var i = 0; i < count4; i++)
            {
                guid5[i] = new byte[8];
                guid5[i] = packet.StartBitStream(5, 3, 7, 4, 2, 0, 6, 1);
            }
            var count5 = packet.ReadBits("count5", 20);
            var guid6 = new byte[count5][];
            for (var i = 0; i < count5; i++)
            {
                guid6[i] = new byte[8];
                guid6[i] = packet.StartBitStream(3, 5, 2, 6, 4, 0, 1, 7);
            }
            for (var i = 0; i < count5; i++)
            {
                packet.ParseBitStream(guid6[i], 5, 2);
                packet.ReadSingle("unk72", i);
                packet.ReadSingle("unk64", i);
                packet.ParseBitStream(guid6[i], 1, 4, 6, 0);
                packet.ReadInt32("unk136", i);
                packet.ReadSingle("unk68", i);
                packet.ParseBitStream(guid6[i], 3, 7);
                packet.WriteGuid("Guid6", guid6[i], i);
            }
            for (var i = 0; i < count1; i++)
            {
                packet.ParseBitStream(guid3[i], 4, 0, 6, 7, 5, 3, 1, 2);
                packet.WriteGuid("Guid3", guid3[i], i);
            }
            for (var i = 0; i < count4; i++)
            {
                packet.ParseBitStream(guid5[i], 2, 5);
                packet.ReadSingle("unk104", i);
                packet.ReadInt32("unk108", i);
                packet.ReadSingle("unk100", i);
                packet.ParseBitStream(guid5[i], 1);
                packet.ReadSingle("unk96", i);
                packet.ParseBitStream(guid5[i], 6, 7, 4, 3, 0);
                packet.WriteGuid("Guid5", guid5[i], i);
            }
            for (var i = 0; i < count3; i++)
            {
                packet.ParseBitStream(guid4[i], 7, 1, 0, 6, 2, 3, 4, 5);
                packet.WriteGuid("Guid4", guid4[i], i);
            }
            for (var i = 0; i < count2; i++)
            {
                packet.ParseBitStream(guid2[i], 6, 2, 5, 0, 3, 4, 1, 7);
                packet.WriteGuid("Guid2", guid2[i], i);
            }
        }

        [Parser(Opcode.SMSG_START_ELAPSED_TIMERS)]
        public static void HandleStartElapsedTimers(Packet packet)
        {
            var count = packet.ReadBits("count", 21);
            for (var i = 0; i < count; i++)
            {
                packet.ReadInt32("unk20", i); // 20
                packet.ReadInt32("unk24", i); // 24
            }
        }

        [Parser(Opcode.SMSG_START_MIRROR_TIMER)]
        public static void HandleStartMirrorTimer(Packet packet)
        {
            packet.ReadUInt32("Max Value"); // 8
            packet.ReadUInt32("Spell Id"); // 4
            packet.ReadUInt32E<MirrorTimerType>("Timer Type"); // 5
            packet.ReadInt32("Regen"); // 6
            packet.ReadUInt32("Current Value"); // 9
            packet.ReadBool("Paused");
        }

        [Parser(Opcode.SMSG_STOP_MIRROR_TIMER)]
        public static void HandleStopMirrorTimer(Packet packet)
        {
            packet.ReadUInt32E<MirrorTimerType>("Timer Type");
        }

        [Parser(Opcode.SMSG_STREAMING_MOVIES)]
        public static void HandleStreamingMovie(Packet packet)
        {
            var count16 = packet.ReadBits("count16", 23);
            for (var i = 0; i < count16; i++)
                packet.ReadInt16("File Data ID"); // 20
        }

        [Parser(Opcode.SMSG_TIME_SYNC_REQUEST)]
        public static void HandleTimeSyncReq(Packet packet)
        {
            packet.ReadUInt32("MCounter");
        }

        [Parser(Opcode.SMSG_WEATHER)]
        public static void Handleweather(Packet packet)
        {
            var state = packet.ReadInt32E<WeatherState>("State");
            var grade = packet.ReadSingle("Grade");
            var unk = packet.ReadBit("Unk Bit"); // Type

            Storage.WeatherUpdates.Add(new WeatherUpdate
            {
                MapId = CoreParsers.MovementHandler.CurrentMapId,
                ZoneId = 0, // fixme
                State = state,
                Grade = grade,
                Unk = unk
            }, packet.TimeSpan);
        }

        [Parser(Opcode.SMSG_WHO)]
        public static void HandleWho(Packet packet)
        {
            var counter = (int)packet.ReadBits("List count", 6);

            var accountId = new byte[counter][];
            var playerGUID = new byte[counter][];
            var guildGUID = new byte[counter][];

            var guildNameLength = new uint[counter];
            var playerNameLength = new uint[counter];
            var bits14 = new uint[counter][];
            var bitED = new bool[counter];
            var bit214 = new bool[counter];

            for (var i = 0; i < counter; ++i)
            {
                accountId[i] = new byte[8];
                playerGUID[i] = new byte[8];
                guildGUID[i] = new byte[8];

                accountId[i][2] = packet.ReadBit();
                playerGUID[i][2] = packet.ReadBit();
                accountId[i][7] = packet.ReadBit();
                guildGUID[i][5] = packet.ReadBit();
                guildNameLength[i] = packet.ReadBits(7);
                accountId[i][1] = packet.ReadBit();
                accountId[i][5] = packet.ReadBit();
                guildGUID[i][7] = packet.ReadBit();
                playerGUID[i][5] = packet.ReadBit();
                bitED[i] = packet.ReadBit();
                guildGUID[i][1] = packet.ReadBit();
                playerGUID[i][6] = packet.ReadBit();
                guildGUID[i][2] = packet.ReadBit();
                playerGUID[i][4] = packet.ReadBit();
                guildGUID[i][0] = packet.ReadBit();
                guildGUID[i][3] = packet.ReadBit();
                accountId[i][6] = packet.ReadBit();
                bit214[i] = packet.ReadBit();
                playerGUID[i][1] = packet.ReadBit();
                guildGUID[i][4] = packet.ReadBit();
                accountId[i][0] = packet.ReadBit();

                bits14[i] = new uint[5];
                for (var j = 0; j < 5; ++j)
                    bits14[i][j] = packet.ReadBits(7);

                playerGUID[i][3] = packet.ReadBit();
                guildGUID[i][6] = packet.ReadBit();
                playerGUID[i][0] = packet.ReadBit();
                accountId[i][4] = packet.ReadBit();
                accountId[i][3] = packet.ReadBit();
                playerGUID[i][7] = packet.ReadBit();
                playerNameLength[i] = packet.ReadBits(6);
            }

            for (var i = 0; i < counter; ++i)
            {
                packet.ReadXORByte(playerGUID[i], 1);
                packet.ReadInt32("RealmID", i);
                packet.ReadXORByte(playerGUID[i], 7);
                packet.ReadInt32("RealmID", i);
                packet.ReadXORByte(playerGUID[i], 4);
                packet.ReadWoWString("Player Name", playerNameLength[i], i);
                packet.ReadXORByte(guildGUID[i], 1);
                packet.ReadXORByte(playerGUID[i], 0);
                packet.ReadXORByte(guildGUID[i], 2);
                packet.ReadXORByte(guildGUID[i], 0);
                packet.ReadXORByte(guildGUID[i], 4);
                packet.ReadXORByte(playerGUID[i], 3);
                packet.ReadXORByte(guildGUID[i], 6);
                packet.ReadInt32("Unk1", i);
                packet.ReadWoWString("Guild Name", guildNameLength[i], i);
                packet.ReadXORByte(guildGUID[i], 3);
                packet.ReadXORByte(accountId[i], 4);
                packet.ReadByteE<Class>("Class", i);
                packet.ReadXORByte(accountId[i], 7);
                packet.ReadXORByte(playerGUID[i], 6);
                packet.ReadXORByte(playerGUID[i], 2);

                for (var j = 0; j < 5; ++j)
                    packet.ReadWoWString("String14", bits14[i][j]);

                packet.ReadXORByte(accountId[i], 2);
                packet.ReadXORByte(accountId[i], 3);
                packet.ReadByteE<Race>("Race", i);
                packet.ReadXORByte(guildGUID[i], 7);
                packet.ReadXORByte(accountId[i], 1);
                packet.ReadXORByte(accountId[i], 5);
                packet.ReadXORByte(accountId[i], 6);
                packet.ReadXORByte(playerGUID[i], 5);
                packet.ReadXORByte(accountId[i], 0);
                packet.ReadByteE<Gender>("Gender", i);
                packet.ReadXORByte(guildGUID[i], 5);
                packet.ReadByte("Level", i);
                packet.ReadInt32<ZoneId>("Zone Id", i);

                packet.WriteGuid("PlayerGUID", playerGUID[i], i);
                packet.WriteGuid("GuildGUID", guildGUID[i], i);
                packet.AddValue("Account", BitConverter.ToUInt64(accountId[i], 0), i);
            }
        }

        [Parser(Opcode.SMSG_WORLD_SERVER_INFO)]
        public static void HandleWorldServerInfo(Packet packet)
        {
            var unk52 = packet.ReadBit("unk52");
            var unk40 = packet.ReadBit("unk40");
            var unk20 = packet.ReadBit("unk20");
            var unk28 = packet.ReadBit("unk28");

            if (unk40)
                packet.ReadInt32("unk36");

            packet.ReadByte("unk32");
            packet.ReadTime("Time");
            packet.ReadInt32("unk224");

            if (unk20)
                packet.ReadInt32("unk64");

            if (unk52)
                packet.ReadInt32("unk192");

            if (unk28)
                packet.ReadInt32("unk96");
        }

        [Parser(Opcode.SMSG_UNK_0C44)]
        public static void HandleUnk0C44(Packet packet)
        {
            packet.ReadInt32("unk1");
            packet.ReadInt32("unk2");
        }

        [Parser(Opcode.SMSG_SET_FLAT_SPELL_MODIFIER)]
        public static void HandleUnk10F2(Packet packet)
        {
            var count = packet.ReadBits("count", 22);
            var cnt = new uint[count];
            for (var i = 0; i < count; i++)
                cnt[i] = packet.ReadBits("cnt", 21, i);
            for (var i = 0; i < count; i++)
            {
                packet.ReadByteE<SpellModOp>("Spell Mod", i);
                for (var j = 0; j < cnt[i]; j++)
                {
                    packet.ReadSingle("Amount", i, j);
                    packet.ReadByte("Spell Mask bitpos", i, j);
                }
            }
        }

        [Parser(Opcode.SMSG_UNK_188F)]
        public static void HandleUnk188F(Packet packet)
        {
            packet.ReadBits("unk16", 2);
            packet.ReadInt32("unk20");
        }

        [Parser(Opcode.CMSG_UNK_0656)]
        [Parser(Opcode.CMSG_UNK_1446)]
        public static void HandleUnk1446(Packet packet)
        {
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.SMSG_GM_SUMMON)]
        public static void HandleGMSummon6(Packet packet)
        {
            packet.ReadBool("unk");
        }

        [Parser(Opcode.CMSG_ATTACK_STOP)]
        [Parser(Opcode.CMSG_COMMENTATOR_EXIT_INSTANCE)]
        [Parser(Opcode.CMSG_GAME_TIME_GET_DAY_PROGRESSION)]
        [Parser(Opcode.CMSG_GET_TIMEZONE_INFORMATION)]
        [Parser(Opcode.CMSG_MOVE_JUMP_OR_ASCEND_START)]
        [Parser(Opcode.CMSG_REQUEST_VEHICLE_NEXT_SEAT)]
        [Parser(Opcode.CMSG_REQUEST_VEHICLE_PREV_SEAT)]
        [Parser(Opcode.CMSG_UI_TIME_REQUEST)]
        [Parser(Opcode.MSG_MOVE_WORLDPORT_ACK)]  //0
        [Parser(Opcode.SMSG_BONUS_ROLL_EMPTY)]
        [Parser(Opcode.SMSG_CLEAR_LOSS_OF_CONTROL)]
        [Parser(Opcode.SMSG_FORCED_DEATH_UPDATE)]
        [Parser(Opcode.SMSG_INVALID_PROMOTION_CODE)]
        [Parser(Opcode.SMSG_NUKE_ALL_OBJECTS_DUE_TO_SEAMLESS_PORT)]
        [Parser(Opcode.SMSG_REFRESH_COMPONENT)]
        [Parser(Opcode.SMSG_SHOW_NEUTRAL_PLAYER_FACTION_SELECT_UI)]
        [Parser(Opcode.SMSG_SOR_START_EXPERIENCE_INCOMPLETE)]
        public static void HandleCNullMisc(Packet packet)
        {
        }
    }
}
