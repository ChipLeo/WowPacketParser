﻿using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class BattlegroundHandler
    {
        [Parser(Opcode.CMSG_ARENA_TEAM_CREATE)]
        public static void HandleArenaTeamCreate(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_ARENA_TEAM_DISBAND)]
        [Parser(Opcode.CMSG_ARENA_TEAM_LEAVE)]
        public static void HandleArenaTeamIdMisc(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_ARENA_TEAM_INVITE)]
        [Parser(Opcode.CMSG_ARENA_TEAM_REMOVE)]
        [Parser(Opcode.CMSG_ARENA_TEAM_LEADER)]
        public static void HandleArenaTeamInvite(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_ARENA_TEAM_ROSTER)]
        [Parser(Opcode.CMSG_ARENA_TEAM_QUERY)]
        public static void HandleArenaTeamQuery(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_BATTLEMASTER_HELLO)]
        [Parser(Opcode.CMSG_REPORT_PVP_PLAYER_AFK)]
        public static void HandleBattlemasterHello(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_AREA_SPIRIT_HEALER_QUERY)]
        public static void HandleAreaSpiritHealerQuery(Packet packet)
        {
            var guid = packet.StartBitStream(5, 6, 0, 4, 1, 2, 7, 3);
            packet.ParseBitStream(guid, 0, 2, 6, 7, 1, 5, 3, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_AREA_SPIRIT_HEALER_QUEUE)]
        public static void HandleAreaSpiritHealerQueue(Packet packet)
        {
            var guid = packet.StartBitStream(5, 4, 0, 2, 7, 1, 6, 3);
            packet.ParseBitStream(guid, 1, 7, 6, 2, 4, 3, 0, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_BATTLEFIELD_JOIN)]
        public static void HandleBattlefieldJoin(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_BATTLEFIELD_LIST)]
        public static void HandleBattlefieldListClient(Packet packet)
        {
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.CMSG_BF_MGR_ENTRY_INVITE_RESPONSE)]
        public static void HandleBattlefieldMgrEntryInviteResponse(Packet packet)
        {
            var guid = new byte[8];

            guid[0] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBool("Accepted");
            guid[4] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            packet.ParseBitStream(guid, 1, 6, 2, 5, 3, 4, 7, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_BF_MGR_QUEUE_EXIT_REQUEST)]
        public static void HandleBattlefieldMgrExitRequest(Packet packet)
        {
            var guid = packet.StartBitStream(3, 2, 4, 1, 7, 0, 5, 6);
            packet.ParseBitStream(guid, 5, 6, 2, 3, 0, 4, 7, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_BF_MGR_QUEUE_INVITE_RESPONSE)]
        public static void HandleBattlefieldMgrQueueInviteResponse(Packet packet)
        {
            var guid = new byte[8];

            guid[3] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            packet.ReadBool("Accepted");
            guid[1] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            packet.ParseBitStream(guid, 4, 1, 2, 6, 0, 7, 5, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_BF_MGR_QUEUE_REQUEST)]
        public static void HandleBattelfieldMgrQueueRequest(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_BATTLEFIELD_PORT)]
        public static void HandleBattlefieldPort(Packet packet)
        {
            packet.ReadBool("Action");
            packet.ReadInt32("Slot");
            packet.ReadInt32("Id");
            packet.ReadTime("Time");
            var guid = packet.StartBitStream(6, 4, 2, 5, 0, 1, 7, 3);
            packet.ParseBitStream(guid, 2, 5, 3, 0, 7, 4, 6, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_BATTLEGROUND_PORT_AND_LEAVE)]
        public static void HandleBattlegroundPort(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_BATTLEMASTER_JOIN)]
        public static void HandleBattlemasterJoin(Packet packet)
        {
            var guid = new byte[8];

            for (var i = 0; i < 2; ++i)
                packet.ReadInt32("Blacklisted MapId", i);

            guid[1] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            packet.ReadBit("As Group");
            guid[4] = packet.ReadBit();
            var hasRoleMask = !packet.ReadBit("!(HasRoleMask==8)");
            guid[6] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            packet.ParseBitStream(guid, 7, 2, 4, 5, 0, 6, 3, 1);

            if (hasRoleMask)
                packet.ReadByte("RoleMask");

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_BATTLEMASTER_JOIN_ARENA)]
        public static void HandleBattlemasterJoinArena(Packet packet)
        {
            packet.ReadByte("Slot");
        }

        [Parser(Opcode.CMSG_LEAVE_BATTLEFIELD)]
        public static void HandleBattlefieldLeave(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_REQUEST_BATTLEFIELD_STATUS)]
        [Parser(Opcode.CMSG_REQUEST_CONQUEST_FORMULA_CONSTANTS)]
        [Parser(Opcode.CMSG_REQUEST_RATED_BATTLEFIELD_INFO)]
        public static void HandleBattlefieldZero(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_REQUEST_INSPECT_RATED_BG_STATS)]
        public static void HandleRequestInspectRBGStats(Packet packet)
        {
            packet.ReadInt32("RealmID");
            var guid = packet.StartBitStream(0, 2, 5, 1, 6, 4, 3, 7);
            packet.ParseBitStream(guid, 7, 3, 1, 5, 4, 0, 2, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_REQUEST_RATED_BG_INFO)]
        public static void HandleRequestRatedBGInfo(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_AREA_SPIRIT_HEALER_TIME)]
        public static void HandleAreaSpiritHealerTime(Packet packet)
        {
            var guid = packet.StartBitStream(5, 2, 7, 6, 1, 0, 3, 4);
            packet.ParseBitStream(guid, 2, 3, 5, 4, 6);
            packet.ReadInt32("Time");
            packet.ParseBitStream(guid, 7, 0, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_ARENA_OPPONENT_UPDATE)]
        public static void HandleArenaOpponentUpdate(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_ARENA_TEAM_COMMAND_RESULT)]
        public static void HandleArenaTeamCommandResult(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_ARENA_TEAM_EVENT)]
        public static void HandleArenaTeamEvent(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_ARENA_TEAM_INVITE)]
        public static void HandleArenaTeamInviteServer(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_ARENA_TEAM_QUERY_RESPONSE)]
        public static void HandleArenaTeamQueryResponse(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_ARENA_TEAM_ROSTER)]
        public static void HandleArenaTeamRoster(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_ARENA_TEAM_STATS)]
        public static void HandleArenaTeamStats(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_LIST)]
        public static void HandleBattlefieldListServer(Packet packet)
        {
            var guid = new byte[8];

            packet.ReadInt32("winnerConquest"); // 28
            packet.ReadInt32("loserHonor"); // 36
            packet.ReadByte("Min Level"); // 46
            packet.ReadInt32("winnerConquest"); // 24
            packet.ReadInt32("winnerHonor"); // 72
            packet.ReadInt32<BgId>("BGType"); // 32
            packet.ReadInt32("winnerHonor"); // 40
            packet.ReadByte("Max Level"); // 47
            packet.ReadInt32("loserHonor"); // 48

            guid[0] = packet.ReadBit();
            var unk45 = packet.ReadBit("unk45"); // 45
            guid[4] = packet.ReadBit();
            var unk68 = packet.ReadBit("unk68"); // 68
            guid[2] = packet.ReadBit();
            var unk76 = packet.ReadBit("unk76"); // 76
            guid[7] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var unk44 = packet.ReadBit("unk44"); // 44
            guid[3] = packet.ReadBit();

            var count52 = packet.ReadBits("count52", 22); // 52
            packet.ParseBitStream(guid, 7, 3, 4, 0, 5, 6, 1, 2);
            for (var i = 0; i < count52; i++)
                packet.ReadInt32("unk64", i);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_MGR_EJECT_PENDING)]
        public static void HandleBattlefieldMgrEjectPending(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_MGR_EJECTED)]
        public static void HandleBattlefieldMgrEjected(Packet packet)
        {
            var guid = new byte[8];
            guid[5] = packet.ReadBit();
            packet.ReadBit("Relocated"); // 17
            guid[4] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[3] = packet.ReadBit();

            packet.ParseBitStream(guid, 0, 2, 3, 4, 6, 5);
            packet.ReadByte("unk1"); // 32
            packet.ReadByte("unk2"); // 16
            packet.ParseBitStream(guid, 7, 1);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_MGR_ENTERING)]
        public static void HandleBattlefieldMgrEntered(Packet packet)
        {
            var guid = new byte[8];
            guid[5] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            packet.ReadBit("unk2"); // 16
            packet.ReadBit("unk"); // 17
            guid[7] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            packet.ReadBit("unk1"); // 32
            guid[1] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[6] = packet.ReadBit();

            packet.ParseBitStream(guid, 2, 5, 0, 6, 7, 3, 4, 1);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_MGR_ENTRY_INVITE)]
        public static void HandleBattlefieldMgrInviteSend(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_MGR_QUEUE_REQUEST_RESPONSE)]
        public static void HandleBattlefieldMgrQueueRequestResponse(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_MGR_QUEUE_INVITE)]
        public static void HandleBattlefieldMgrQueueInvite(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_MGR_STATE_CHANGED)]
        public static void HandleBattlefieldMgrStateChanged(Packet packet)
        {
            var guid = packet.StartBitStream(6, 1, 3, 7, 4, 2, 5, 0);
            packet.ParseBitStream(guid, 5, 2, 1, 6, 0);
            packet.ReadUInt32E<BattlegroundStatus>("Status");
            packet.ParseBitStream(guid, 3, 4, 7);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_PLAYER_POSITIONS)]
        public static void HandleBattlefieldPlayerPositions(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_RATED_INFO)]
        public static void HandleBattlefieldRatedInfo(Packet packet)
        {
            for (var i = 0; i < 4; i++)
            {
                packet.ReadInt32("unk20", i);
                packet.ReadInt32("unk36", i);
                packet.ReadInt32("unk24", i);
                packet.ReadInt32("unk16", i);
                packet.ReadInt32("unk28", i);
                packet.ReadInt32("unk40", i);
                packet.ReadInt32("unk32", i);
                packet.ReadInt32("unk44", i);
            }
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_STATUS)]
        public static void HandleBattlefieldStatusServer(Packet packet)
        {
            packet.ReadTime("unk32");
            packet.ReadInt32("unk24");
            packet.ReadInt32("Slot");
            var guid = packet.StartBitStream(2, 0, 4, 5, 3, 7, 1, 6);
            packet.ParseBitStream(guid, 3, 5, 6, 4, 2, 0, 1, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_STATUS_ACTIVE)]
        public static void HandleBattlefieldStatusActive(Packet packet)
        {
            var guid = new byte[8];
            var guid2 = new byte[8];

            guid[0] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid2[2] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid2[5] = packet.ReadBit();
            guid2[1] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var unk29 = packet.ReadBit("unk29"); // 29
            guid[6] = packet.ReadBit();
            guid2[0] = packet.ReadBit();
            var unk28 = packet.ReadBit("unk28"); // 28
            guid2[6] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid2[4] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var unk72 = packet.ReadBit("unk72"); // 72

            packet.ParseBitStream(guid, 3);
            packet.ReadTime("unk192"); // 192
            packet.ReadInt32("unk80"); // 80
            packet.ParseBitStream(guid2, 7, 5);
            packet.ParseBitStream(guid, 1);
            packet.ParseBitStream(guid2, 6);
            packet.ReadInt32("unk96"); // 96
            packet.ReadByte("Max Lvl"); // 65
            packet.ParseBitStream(guid2, 1, 2);
            packet.ReadInt32("unk176"); // 176
            packet.ParseBitStream(guid, 4);
            packet.ReadByte("unk64");
            packet.ParseBitStream(guid, 6);
            packet.ReadInt32<MapId>("MapID"); // 16
            packet.ParseBitStream(guid, 0, 5, 7);
            packet.ParseBitStream(guid2, 4);
            packet.ReadInt32("unk272"); // 272
            packet.ParseBitStream(guid, 2);
            packet.ReadByte("unk66"); // 66
            packet.ReadInt32("unk160"); // 160
            packet.ParseBitStream(guid2, 3, 0);

            packet.WriteGuid("Guid", guid);
            packet.WriteGuid("Guid2", guid2);
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_STATUS_FAILED)]
        public static void HandleBattlefieldStatusFailed(Packet packet)
        {
            var guid = new byte[8];
            var guid2 = new byte[8];
            var guid3 = new byte[8];

            packet.ReadInt32("unk40"); // 40
            packet.ReadInt32("unk32"); // 32
            packet.ReadInt32("unk36"); // 36
            packet.ReadInt32("unk48"); // 48

            guid3[7] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid3[5] = packet.ReadBit();
            guid2[2] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid3[4] = packet.ReadBit();
            guid2[1] = packet.ReadBit();
            guid3[0] = packet.ReadBit();
            guid2[0] = packet.ReadBit();
            guid3[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid2[4] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid3[3] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid3[1] = packet.ReadBit();
            guid2[6] = packet.ReadBit();
            guid2[5] = packet.ReadBit();
            guid3[6] = packet.ReadBit();

            packet.ParseBitStream(guid3, 1, 2, 7);
            packet.ParseBitStream(guid, 6, 0);
            packet.ParseBitStream(guid2, 5, 0);
            packet.ParseBitStream(guid, 1, 7);
            packet.ParseBitStream(guid2, 6);
            packet.ParseBitStream(guid3, 0);
            packet.ParseBitStream(guid, 5);
            packet.ParseBitStream(guid3, 6);
            packet.ParseBitStream(guid2, 1);
            packet.ParseBitStream(guid, 2);
            packet.ParseBitStream(guid2, 7, 2, 3);
            packet.ParseBitStream(guid3, 5);
            packet.ParseBitStream(guid2, 4);
            packet.ParseBitStream(guid3, 3);
            packet.ParseBitStream(guid, 3);
            packet.ParseBitStream(guid3, 4);
            packet.ParseBitStream(guid, 4);

            packet.WriteGuid("Guid", guid);
            packet.WriteGuid("Guid2", guid2);
            packet.WriteGuid("Guid3", guid3);
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_STATUS_NEED_CONFIRMATION)]
        public static void HandleBattlefieldStatusNeedConfirmation(Packet packet)
        {
            var guid = new byte[8];
            var guid2 = new byte[8];

            guid[7] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            guid2[2] = packet.ReadBit();
            var unk16 = 3 - packet.ReadBit("3 - unk16"); // 16
            guid2[0] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid2[4] = packet.ReadBit();
            guid2[1] = packet.ReadBit();
            var unk64 = packet.ReadBit("unk64"); // 64
            guid[2] = packet.ReadBit();
            guid2[6] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid2[5] = packet.ReadBit();

            packet.ParseBitStream(guid, 1);
            packet.ParseBitStream(guid2, 1);
            packet.ParseBitStream(guid, 2);
            packet.ReadByte("unk58"); // 58
            packet.ReadInt32("unk144"); // 144
            if (unk16 != 2)
                packet.ReadByte("unk16"); // 16
            packet.ReadInt32("unk240"); // 240
            packet.ParseBitStream(guid2, 6, 7);
            packet.ReadTime("unk160"); // 160
            packet.ParseBitStream(guid, 7);
            packet.ReadByte("Max Lvl"); // 57
            packet.ParseBitStream(guid, 4);
            packet.ParseBitStream(guid2, 2, 4);
            packet.ReadInt32("unk304"); // 304
            packet.ReadByte("unk56"); // 56
            packet.ParseBitStream(guid, 3);
            packet.ParseBitStream(guid2, 0);
            packet.ParseBitStream(guid, 5, 6);
            packet.ReadInt32("unk128"); // 128
            packet.ParseBitStream(guid2, 3);
            packet.ParseBitStream(guid, 0);
            packet.ParseBitStream(guid2, 5);
            packet.ReadInt32<MapId>("MapID"); // 288

            packet.WriteGuid("Guid", guid);
            packet.WriteGuid("Guid2", guid2);
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_STATUS_WAIT_FOR_GROUPS)]
        public static void HandleBattlefieldStatusWaitForGroups(Packet packet)
        {
            var guid1 = new byte[8];
            var guid2 = new byte[8];

            guid2[0] = packet.ReadBit();//40
            guid1[6] = packet.ReadBit();//22
            packet.ReadBit("IsRated");//56
            guid2[4] = packet.ReadBit();//44
            guid2[5] = packet.ReadBit();//45
            guid2[6] = packet.ReadBit();//46
            guid1[2] = packet.ReadBit();//18
            guid2[3] = packet.ReadBit();//43
            guid2[1] = packet.ReadBit();//41
            guid1[3] = packet.ReadBit();//19
            guid1[4] = packet.ReadBit();//20
            guid1[5] = packet.ReadBit();//21
            guid1[1] = packet.ReadBit();//17
            guid2[7] = packet.ReadBit();//47
            guid2[2] = packet.ReadBit();//42
            guid1[0] = packet.ReadBit();//16
            guid1[7] = packet.ReadBit();//23
            packet.ReadByte("Unk74"); //74

            packet.ReadXORByte(guid2, 1);
            packet.ReadXORByte(guid1, 5);
            packet.ReadXORByte(guid1, 7);
            packet.ReadUInt32("Unk64"); //64
            packet.ReadUInt32("Unk32"); //32
            packet.ReadXORByte(guid1, 1);
            packet.ReadXORByte(guid2, 3);
            packet.ReadXORByte(guid2, 2);
            packet.ReadByte("Unk75"); // 75
            packet.ReadXORByte(guid1, 4);
            packet.ReadInt32("Unk52"); //52
            packet.ReadXORByte(guid1, 6);
            packet.ReadUInt32("Unk68"); //68
            packet.ReadUInt32("Unk28"); //28
            packet.ReadXORByte(guid2, 0);
            packet.ReadXORByte(guid2, 5);
            packet.ReadUInt32("unk24"); //24
            packet.ReadXORByte(guid2, 6);
            packet.ReadByte("Unk49"); // 49
            packet.ReadByte("Unk48"); //48
            packet.ReadXORByte(guid2, 7);
            packet.ReadByte("Unk72"); //72
            packet.ReadXORByte(guid2, 4);
            packet.ReadXORByte(guid1, 2);
            packet.ReadByte("Unk50"); //50
            packet.ReadXORByte(guid1, 0);
            packet.ReadByte("Unk73"); //73 result
            packet.ReadXORByte(guid1, 3);

            packet.WriteGuid("Player Guid", guid1);
            packet.WriteGuid("BG Guid", guid2);
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_STATUS_QUEUED)]
        public static void HandleRGroupJoinedBattleground(Packet packet)
        {
            var guid = new byte[8];
            var guid2 = new byte[8];

            guid2[1] = packet.ReadBit();
            guid2[5] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            var unk72 = packet.ReadBit("unk72"); // 72
            var unk80 = packet.ReadBit("unk80"); // 80
            var unk17 = packet.ReadBit("unk17"); // 17
            guid[6] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var unk16 = packet.ReadBit("unk16"); // 16
            guid2[3] = packet.ReadBit();
            guid2[0] = packet.ReadBit();
            guid2[2] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid2[6] = packet.ReadBit();
            guid2[4] = packet.ReadBit();

            packet.ParseBitStream(guid2, 4);
            packet.ParseBitStream(guid, 6);
            packet.ReadInt32("unk160"); // 160
            packet.ParseBitStream(guid2, 5);
            packet.ReadInt32("unk80"); // 80
            packet.ParseBitStream(guid2, 3);
            packet.ParseBitStream(guid, 3, 4, 0);
            packet.ReadByte("unk64"); // 64
            packet.ParseBitStream(guid2, 0);
            packet.ReadTime("unk192"); // 192
            packet.ReadByte("unk66"); // 66
            packet.ParseBitStream(guid2, 1);
            packet.ReadInt32("unk96"); // 96
            packet.ParseBitStream(guid2, 7);
            packet.ReadInt32("Slot"); // 176
            packet.ParseBitStream(guid, 2);
            packet.ParseBitStream(guid2, 6);
            packet.ReadByte("Max Lvl"); // 65
            packet.ParseBitStream(guid2, 2);
            packet.ParseBitStream(guid, 5, 1);
            packet.ReadInt32("unk252"); // 252
            packet.ParseBitStream(guid, 7);

            packet.WriteGuid("Guid", guid);
            packet.WriteGuid("Guid2", guid2);
        }

        [Parser(Opcode.SMSG_BATTLEGROUND_EXIT_QUEUE)]
        public static void HandleBattlegroundExitQueue(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_BATTLEGROUND_IN_PROGRESS)]
        public static void HandleBattlegroundInProgress(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_BATTLEGROUND_PLAYER_JOINED)]
        public static void HandleBattlegroundPlayerJoined(Packet packet)
        {
            var guid = packet.StartBitStream(7, 1, 0, 4, 2, 5, 6, 3);
            packet.ParseBitStream(guid, 0, 3, 7, 5, 2, 6, 4, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_BATTLEGROUND_PLAYER_LEFT)]
        public static void HandleBattleGroundPlayerLeft(Packet packet)
        {
            var guid = packet.StartBitStream(3, 5, 6, 0, 1, 2, 7, 4);
            packet.ParseBitStream(guid, 0, 6, 5, 7, 2, 1, 3, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_BATTLEGROUND_PLAYER_POSITIONS)]
        public static void HandleBattlegroundPlayerPositions(Packet packet)
        {
            var count16 = packet.ReadBits("count16", 20);
            var guid = new byte[count16][];
            for (var i = 0; i < count16; i++)
                guid[i] = packet.StartBitStream(1, 5, 0, 4, 6, 3, 7, 2);
            for (var i = 0; i < count16; i++)
            {
                packet.ParseBitStream(guid[i], 0, 3, 7);
                packet.ReadByte("unk36", i);
                packet.ReadSingle("unk32", i);
                packet.ParseBitStream(guid[i], 4, 2, 1, 5);
                packet.ReadSingle("unk28", i);
                packet.ReadByte("unk37", i);
                packet.ParseBitStream(guid[i], 6);
                packet.WriteGuid("Guid", guid[i], i);
            }
        }

        [Parser(Opcode.SMSG_BATTLEGROUND_WAIT_JOIN)]
        public static void HandleBattlegroundWaitJoin(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_BATTLEGROUND_WAIT_LEAVE)]
        public static void HandleBattlegroundWaitLeave(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_JOINED_BATTLEGROUND_QUEUE)]
        public static void HandleJoinedBattlegroundQueue(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_INSPECT_RATED_BG_STATS)]
        public static void HandleInspectRatedBGStats(Packet packet)
        {
            var guid = packet.StartBitStream(4, 2, 3, 6, 0, 5, 7, 1);
            var count = packet.ReadBits("count", 3);
            for (var i = 0; i < count; i++)
            {
                packet.ReadInt32("unk48", i); // 48
                packet.ReadInt32("unk28", i); // 28
                packet.ReadInt32("unk44", i); // 44
                packet.ReadInt32("unk32", i); // 32
                packet.ReadInt32("unk36", i); // 36
                packet.ReadInt32("unk40", i); // 40
                packet.ReadInt32("unk52", i); // 52
                packet.ReadByte("unk28", i); // 28
            }
            packet.ParseBitStream(guid, 1, 7, 3, 2, 0, 5, 6, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_PVP_LOG_DATA)]
        public static void HandleSPvPLogData(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_PVP_OPTIONS_ENABLED)]
        public static void HandlePVPOptionsEnabled(Packet packet)
        {
            packet.ReadBit("unk16"); // 16
            packet.ReadBit("unk18"); // 18
            packet.ReadBit("unk17"); // 17
            packet.ReadBit("unk20"); // 20
            packet.ReadBit("unk19"); // 19
        }

        [Parser(Opcode.SMSG_RATED_BG_RATING)]
        public static void HandleRatedBGStats(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_REPORT_PVP_AFK_RESULT)]
        public static void HandleReportPvPAFKResult(Packet packet)
        {
            var unk26 = !packet.ReadBit("!unk26"); // 26
            var unk25 = !packet.ReadBit("!unk25"); // 25
            var unk24 = 5 - packet.ReadBit("unk24"); // 24

            var unk64 = !packet.ReadBit("!unk64"); // 64

            var guid = packet.StartBitStream(1, 5, 7, 2, 6, 4, 3, 0);
            packet.ParseBitStream(guid, 0, 6, 2, 4, 5, 1, 3, 7);
            if (unk25)
                packet.ReadByte("unk25"); // 25
            if (unk26)
                packet.ReadByte("unk26"); // 26
            if (unk24 != 4)
                packet.ReadByte("unk24"); // 24
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_REQUEST_PVP_REWARDS_RESPONSE)]
        public static void HandlePVPRewardsResponse(Packet packet)
        {
            packet.ReadInt32("unk28"); // 28
            packet.ReadInt32("unk24"); // 24
            packet.ReadInt32("unk52"); // 52
            packet.ReadInt32("unk20"); // 20
            packet.ReadInt32("unk16"); // 16
            packet.ReadInt32("unk40"); // 40
            packet.ReadInt32("unk48"); // 48
            packet.ReadInt32("unk32"); // 32
            packet.ReadInt32("unk44"); // 44
            packet.ReadInt32("unk36"); // 36
        }

        [Parser(Opcode.SMSG_WARGAME_REQUEST_SENT)]
        public static void HandleWarGameRequestSent(Packet packet)
        {
            var guid = packet.StartBitStream(4, 3, 0, 2, 1, 6, 5, 7);
            packet.ParseBitStream(guid, 1, 4, 5, 6, 2, 0, 3, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_GET_PVP_OPTIONS_ENABLED)]
        [Parser(Opcode.CMSG_BATTLEGROUND_PLAYER_POSITIONS)]
        [Parser(Opcode.SMSG_BATTLEGROUND_INFO_THROTTLED)]
        [Parser(Opcode.SMSG_BATTLEFIELD_PORT_DENIED)]
        [Parser(Opcode.CMSG_ARENA_TEAM_ACCEPT)]
        [Parser(Opcode.CMSG_ARENA_TEAM_DECLINE)]
        [Parser(Opcode.CMSG_BATTLEFIELD_LEAVE)]
        [Parser(Opcode.CMSG_BATTLEFIELD_REQUEST_SCORE_DATA)]
        [Parser(Opcode.CMSG_BATTLEFIELD_STATUS)]
        [Parser(Opcode.CMSG_PVP_LOG_DATA)]
        [Parser(Opcode.CMSG_QUERY_BATTLEFIELD_STATE)]
        [Parser(Opcode.CMSG_REQUEST_PVP_REWARDS)]
        [Parser(Opcode.CMSG_REQUEST_RATED_BG_STATS)]
        public static void HandleBattlegroundNull(Packet packet)
        {
        }
    }
}
