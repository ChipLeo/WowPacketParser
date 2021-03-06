﻿using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class GuildHandler
    {
        [Parser(Opcode.CMSG_GUILD_ADD_RANK)]
        public static void HandleGuildAddRank(Packet packet)
        {
            packet.ReadInt32("Rank");
            packet.ReadWoWString("Name", packet.ReadBits(7));
        }

        [Parser(Opcode.CMSG_GUILD_ASSIGN_MEMBER_RANK)]
        public static void HandleGuildAssignMemberRank(Packet packet)
        {
            packet.ReadUInt32("Rank");

            var guid = packet.StartBitStream(2, 3, 1, 6, 0, 4, 7, 5);
            packet.ParseBitStream(guid, 7, 3, 2, 5, 6, 0, 4, 1);

            packet.WriteGuid("GUID", guid);
        }

        [Parser(Opcode.CMSG_GUILD_BANK_ACTIVATE)]
        public static void HandleGuildBankerActivate(Packet packet)
        {
            var guid = new byte[8];

            guid[3] = packet.ReadBit();
            packet.ReadBit("Full Slot List");
            guid[0] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[4] = packet.ReadBit();

            packet.ParseBitStream(guid, 7, 1, 0, 6, 4, 2, 5, 3);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_GUILD_BANK_BUY_TAB)]
        public static void HandleGuildBankBuyTab(Packet packet)
        {
            packet.ReadByte("TabID");

            var guid = packet.StartBitStream(0, 1, 3, 7, 2, 6, 5, 4);
            packet.ParseBitStream(guid, 1, 4, 6, 7, 3, 5, 2, 0);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_GUILD_BANK_DEPOSIT_MONEY)]
        public static void HandleGuildBankDepositMoney(Packet packet)
        {
            packet.ReadUInt64("Money");

            var guid = packet.StartBitStream(2, 7, 6, 4, 0, 1, 5, 3);
            packet.ParseBitStream(guid, 1, 4, 5, 0, 2, 7, 6, 3);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_GUILD_BANK_LOG_QUERY)]
        public static void HandleGuildBankLogQuery(Packet packet)
        {
            packet.ReadUInt32("Tab Id");
        }

        [Parser(Opcode.CMSG_GUILD_BANK_UPDATE_TAB)]
        public static void HandleGuildBankUpdateTab(Packet packet)
        {
            var guid = new byte[8];
            packet.ReadByte("BankTab");

            guid[5] = packet.ReadBit();
            var iconLen = packet.ReadBits(9);
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var nameLen = packet.ReadBits(7);

            packet.ResetBitReader();

            packet.ParseBitStream(guid, 7, 4);
            packet.ReadWoWString("Icon", iconLen);
            packet.ParseBitStream(guid, 5, 1, 0);
            packet.ReadWoWString("Name", nameLen);
            packet.ParseBitStream(guid, 2, 3, 6);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_GUILD_CHANGE_NAME_RESULT)]
        public static void HandleGuildChangeNameResult(Packet packet)
        {
            packet.ReadBool("Result");
        }

        [Parser(Opcode.CMSG_GUILD_DECLINE_INVITATION)]
        public static void HandleGuildDecline(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_GUILD_DELETE_RANK)]
        public static void HandleGuildDelRank(Packet packet)
        {
            packet.ReadUInt32("Rank Id");
        }

        [Parser(Opcode.CMSG_GUILD_DEMOTE_MEMBER)]
        public static void HandleGuildDemote(Packet packet)
        {
            var guid = packet.StartBitStream(3, 6, 0, 2, 7, 5, 4, 1);
            packet.ParseBitStream(guid, 7, 4, 2, 5, 1, 3, 0, 6);
            packet.WriteGuid("GUID", guid);
        }

        [Parser(Opcode.SMSG_GUILD_EVENT_PRESENCE_CHANGE)]
        public static void HandleGuildEventPresenceChange(Packet packet)
        {
            var guid = new byte[8];
            guid[0] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            packet.ReadBit("Mobile"); // 17
            guid[2] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var bits18 = packet.ReadBits(6);
            guid[1] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            packet.ReadBit("LoggedOn"); // 16
            packet.ParseBitStream(guid, 3, 2, 0);
            packet.ReadInt32("VirtualRealmAddress");
            packet.ParseBitStream(guid, 6);
            packet.ReadWoWString("Name", bits18);
            packet.ParseBitStream(guid, 4, 5, 7, 1);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_GUILD_EVENT_TAB_TEXT_CHANGED)]
        public static void HandleGuildEventTabTextChanged(Packet packet)
        {
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.SMSG_GUILD_EVENT_RANK_CHANGED)]
        public static void HandleGuildEventRankChanged(Packet packet)
        {
            packet.ReadInt32("RankID");
        }

        [Parser(Opcode.SMSG_GUILD_FLAGGED_FOR_RENAME)]
        public static void HandleGuildFlaggedForRename(Packet packet)
        {
            packet.ReadBool("Result");
        }

        [Parser(Opcode.CMSG_GUILD_GET_RANKS)]
        public static void HandleGuildRanks(Packet packet)
        {
            var guid = packet.StartBitStream(0, 2, 5, 4, 3, 7, 6, 1);
            packet.ParseBitStream(guid, 6, 0, 1, 7, 3, 2, 5, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_GUILD_GET_ROSTER)]
        public static void HandleGuildRosterRequest(Packet packet)
        {
            var guid1 = new byte[8];
            var guid2 = new byte[8];

            guid1[4] = packet.ReadBit();
            guid1[3] = packet.ReadBit();
            guid1[2] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid1[0] = packet.ReadBit();
            guid1[6] = packet.ReadBit();
            guid1[5] = packet.ReadBit();
            guid2[0] = packet.ReadBit();
            guid2[2] = packet.ReadBit();
            guid2[6] = packet.ReadBit();
            guid1[7] = packet.ReadBit();
            guid2[1] = packet.ReadBit();
            guid1[1] = packet.ReadBit();
            guid2[4] = packet.ReadBit();
            guid2[5] = packet.ReadBit();
            guid2[3] = packet.ReadBit();

            packet.ReadXORByte(guid1, 7);
            packet.ReadXORByte(guid2, 3);
            packet.ReadXORByte(guid2, 0);
            packet.ReadXORByte(guid2, 1);
            packet.ReadXORByte(guid1, 4);
            packet.ReadXORByte(guid1, 3);
            packet.ReadXORByte(guid1, 0);
            packet.ReadXORByte(guid1, 1);
            packet.ReadXORByte(guid2, 6);
            packet.ReadXORByte(guid2, 7);
            packet.ReadXORByte(guid1, 5);
            packet.ReadXORByte(guid2, 5);
            packet.ReadXORByte(guid1, 6);
            packet.ReadXORByte(guid1, 2);
            packet.ReadXORByte(guid2, 4);
            packet.ReadXORByte(guid2, 2);

            packet.WriteGuid("Guid1", guid1);
            packet.WriteGuid("Guid2", guid2);
        }

        [Parser(Opcode.CMSG_GUILD_INFO_TEXT)]
        public static void HandleGuildInfoText(Packet packet)
        {
            packet.ReadWoWString("Text", packet.ReadBits(11));
        }

        [Parser(Opcode.CMSG_GUILD_INVITE)]
        public static void HandleGuildInvite(Packet packet)
        {
            packet.ReadWoWString("Name", packet.ReadBits(9));
        }

        [Parser(Opcode.SMSG_GUILD_INVITE)]
        public static void HandleServerGuildInvite(Packet packet)
        {
            var guid16 = new byte[8];
            var guid136 = new byte[8];

            guid16[4] = packet.ReadBit();
            var bits152 = packet.ReadBits(7);
            guid136[4] = packet.ReadBit();
            guid16[6] = packet.ReadBit();
            guid136[2] = packet.ReadBit();
            guid136[1] = packet.ReadBit();
            guid136[5] = packet.ReadBit();
            guid136[7] = packet.ReadBit();
            guid16[0] = packet.ReadBit();
            guid136[3] = packet.ReadBit();
            guid16[5] = packet.ReadBit();
            guid136[6] = packet.ReadBit();
            var bits264 = packet.ReadBits(6);
            guid16[1] = packet.ReadBit();
            guid16[3] = packet.ReadBit();
            guid136[0] = packet.ReadBit();
            guid16[2] = packet.ReadBit();
            var bits32 = packet.ReadBits(7);
            guid16[7] = packet.ReadBit();

            packet.ReadXORByte(guid16, 1);
            packet.ReadInt32("BackgroundColor"); //256
            packet.ReadXORByte(guid16, 4);
            packet.ReadWoWString("Inviter", bits264);
            packet.ReadInt32("BorderStyle"); //316
            packet.ReadXORByte(guid136, 7);
            packet.ReadXORByte(guid16, 0);
            packet.ReadXORByte(guid16, 2);
            packet.ReadInt32("EmblemColor"); //28
            packet.ReadXORByte(guid136, 2);
            packet.ReadXORByte(guid136, 5);
            packet.ReadInt32("Level"); //252
            packet.ReadInt32("OldGuildVirtualRealmAddress"); //24
            packet.ReadXORByte(guid16, 7);
            packet.ReadXORByte(guid16, 3);
            packet.ReadXORByte(guid136, 4);
            packet.ReadInt32("BorderColor"); //144
            packet.ReadWoWString("GuildName", bits152);
            packet.ReadInt32("InviterVirtualRealmAddress"); //260
            packet.ReadInt32("EmblemStyle"); //148
            packet.ReadXORByte(guid136, 0);
            packet.ReadWoWString("oldGuildName", bits32);
            packet.ReadXORByte(guid16, 5);
            packet.ReadInt32("GuildVirtualRealmAddress"); //320
            packet.ReadXORByte(guid136, 1);
            packet.ReadXORByte(guid16, 6);
            packet.ReadXORByte(guid136, 3);
            packet.ReadXORByte(guid136, 6);

            packet.WriteGuid("Guild", guid16);
            packet.WriteGuid("oldGuild", guid136);
        }

        [Parser(Opcode.CMSG_GUILD_MOTD)]
        public static void HandleGuildMotd(Packet packet)
        {
            packet.ReadWoWString("Motd", packet.ReadBits(10));
        }

        [Parser(Opcode.SMSG_GUILD_NEWS_DELETED)]
        public static void HandleGuildNewsDeleted(Packet packet)
        {
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.CMSG_GUILD_NEWS_UPDATE_STICKY)]
        public static void HandleGuildNewsUpdateSticky(Packet packet)
        {
            var guid = new byte[8];

            packet.ReadInt32("NewsID");

            guid[6] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            packet.ReadBit("Sticky");
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[1] = packet.ReadBit();

            packet.ParseBitStream(guid, 5, 4, 0, 1, 6, 2, 3, 7);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_GUILD_OFFICER_REMOVE_MEMBER)]
        public static void HandleGuildRemove(Packet packet)
        {
            var guid = packet.StartBitStream(7, 3, 4, 2, 5, 6, 1, 0);
            packet.ParseBitStream(guid, 0, 2, 5, 6, 7, 1, 4, 3);
            packet.WriteGuid("GUID", guid);
        }

        [Parser(Opcode.CMSG_GUILD_PROMOTE_MEMBER)]
        public static void HandleGuildPromote(Packet packet)
        {
            var guid = packet.StartBitStream(6, 0, 4, 3, 1, 7, 2, 5);
            packet.ParseBitStream(guid, 1, 7, 2, 5, 3, 4, 0, 6);
            packet.WriteGuid("GUID", guid);
        }

        [Parser(Opcode.CMSG_QUERY_GUILD_INFO)]
        public static void HandleGuildQuery(Packet packet)
        {
            var playerGUID = new byte[8];
            var guildGUID = new byte[8];

            playerGUID[7] = packet.ReadBit();
            playerGUID[3] = packet.ReadBit();
            playerGUID[4] = packet.ReadBit();
            guildGUID[3] = packet.ReadBit();
            guildGUID[4] = packet.ReadBit();
            playerGUID[2] = packet.ReadBit();
            playerGUID[6] = packet.ReadBit();
            guildGUID[2] = packet.ReadBit();
            guildGUID[5] = packet.ReadBit();
            playerGUID[1] = packet.ReadBit();
            playerGUID[5] = packet.ReadBit();
            guildGUID[7] = packet.ReadBit();
            playerGUID[0] = packet.ReadBit();
            guildGUID[1] = packet.ReadBit();
            guildGUID[6] = packet.ReadBit();
            guildGUID[0] = packet.ReadBit();

            packet.ReadXORByte(playerGUID, 7);
            packet.ReadXORByte(guildGUID, 2);
            packet.ReadXORByte(guildGUID, 4);
            packet.ReadXORByte(guildGUID, 7);
            packet.ReadXORByte(playerGUID, 6);
            packet.ReadXORByte(playerGUID, 0);
            packet.ReadXORByte(guildGUID, 6);
            packet.ReadXORByte(guildGUID, 0);
            packet.ReadXORByte(guildGUID, 3);
            packet.ReadXORByte(playerGUID, 2);
            packet.ReadXORByte(guildGUID, 5);
            packet.ReadXORByte(playerGUID, 3);
            packet.ReadXORByte(guildGUID, 1);
            packet.ReadXORByte(playerGUID, 4);
            packet.ReadXORByte(playerGUID, 1);
            packet.ReadXORByte(playerGUID, 5);

            packet.WriteGuid("Player GUID", playerGUID);
            packet.WriteGuid("Guild GUID", guildGUID);
        }

        [Parser(Opcode.CMSG_GUILD_QUERY_NEWS)]
        public static void HandleGuildQueryNews434(Packet packet)
        {
            var guid = packet.StartBitStream(7, 4, 2, 6, 0, 5, 3, 1);
            packet.ParseBitStream(guid, 3, 2, 7, 0, 5, 4, 1, 6);
            packet.WriteGuid("GUID", guid);
        }

        [Parser(Opcode.CMSG_GUILD_SET_ACHIEVEMENT_TRACKING)]
        public static void HandleGuildSetAchievementTracking(Packet packet)
        {
            var count = packet.ReadBits("Count", 22);
            for (var i = 0; i < count; ++i)
                packet.ReadUInt32("Criteria Id", i); // 24
        }

        [Parser(Opcode.CMSG_GUILD_SET_GUILD_MASTER)]
        public static void HandleGuildSetGuildMaster(Packet packet)
        {
            packet.ReadWoWString("New GuildMaster name", packet.ReadBits(9));
        }

        [Parser(Opcode.CMSG_GUILD_SET_NOTE)]
        public static void HandleGuildPlayerSetNote(Packet packet)
        {
            var playerGUID = new byte[8];

            playerGUID[1] = packet.ReadBit();
            var noteLength = packet.ReadBits("note length", 8);
            playerGUID[4] = packet.ReadBit();
            playerGUID[2] = packet.ReadBit();
            var ispublic = packet.ReadBit("IsPublic");
            playerGUID[3] = packet.ReadBit();
            playerGUID[5] = packet.ReadBit();
            playerGUID[0] = packet.ReadBit();
            playerGUID[6] = packet.ReadBit();
            playerGUID[7] = packet.ReadBit();

            packet.ReadXORByte(playerGUID, 5);
            packet.ReadXORByte(playerGUID, 1);
            packet.ReadXORByte(playerGUID, 6);
            packet.ReadWoWString("note", noteLength);
            packet.ReadXORByte(playerGUID, 0);
            packet.ReadXORByte(playerGUID, 7);
            packet.ReadXORByte(playerGUID, 4);
            packet.ReadXORByte(playerGUID, 3);
            packet.ReadXORByte(playerGUID, 2);

            packet.WriteGuid("Player GUID", playerGUID);
        }

        [Parser(Opcode.CMSG_GUILD_SET_RANK_PERMISSIONS)]
        public static void HandleGuildSetRankPerm(Packet packet)
        {
            packet.ReadInt32("OldRankID"); // 92
            for (var tabId = 0; tabId < 8; ++tabId)
            {
                packet.ReadInt32("BankRights", tabId); // 128
                packet.ReadInt32("Slots", tabId); // 136
            }
            packet.ReadInt32("MoneyPerDay"); // 96
            packet.ReadInt32("NewRights"); // 20
            packet.ReadInt32("NewRankId"); // 100
            packet.ReadInt32("OldRights"); // 16

            var len = packet.ReadBits("Len", 7);
            packet.ReadWoWString("Name", len);
        }

        [Parser(Opcode.CMSG_LF_GUILD_ADD_APPLICATION)]
        public static void HandleLFGuildAddApplication(Packet packet)
        {
            packet.ReadInt32("unk20"); // 20
            packet.ReadInt32("unk16"); // 16
            packet.ReadInt32("unk1056"); // 1056
            var guid1048 = new byte[8];
            guid1048[7] = packet.ReadBit();
            guid1048[5] = packet.ReadBit();
            guid1048[2] = packet.ReadBit();
            guid1048[6] = packet.ReadBit();
            guid1048[1] = packet.ReadBit();
            guid1048[0] = packet.ReadBit();
            var len24 = packet.ReadBits("len24", 10); // 24
            guid1048[3] = packet.ReadBit();
            guid1048[4] = packet.ReadBit();
            packet.ParseBitStream(guid1048, 4, 0, 2);
            packet.ReadWoWString("str", len24);
            packet.ParseBitStream(guid1048, 6, 1, 5, 7, 3);
            packet.WriteGuid("Guid", guid1048);
        }

        [Parser(Opcode.CMSG_LF_GUILD_BROWSE)]
        public static void HandleLFGuildBrowse(Packet packet)
        {
            packet.ReadUInt32E<GuildFinderOptionsRoles>("Class Roles"); // 16
            packet.ReadUInt32E<GuildFinderOptionsAvailability>("Availability"); // 24
            packet.ReadUInt32E<GuildFinderOptionsInterest>("Guild Interests"); // 28
            packet.ReadUInt32("Player Level"); // 20
        }

        [Parser(Opcode.CMSG_LF_GUILD_GET_APPLICATIONS)]
        public static void HandlerLFGuildGetApplications(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_LF_GUILD_REMOVE_APPLICATION)]
        public static void HandlerLFGuildRemoveApplication(Packet packet)
        {
            var guid = packet.StartBitStream(7, 5, 4, 1, 6, 3, 2, 0);
            packet.ParseBitStream(guid, 6, 3, 7, 1, 2, 0, 5, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_OFFER_PETITION)]
        public static void HandlePetitionOffer(Packet packet)
        {
            packet.ReadUInt32("Unk UInt32");
            var guid = new byte[8];
            var targetGuid = new byte[8];
            targetGuid[4] = packet.ReadBit();
            targetGuid[1] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            targetGuid[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            targetGuid[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            targetGuid[3] = packet.ReadBit();
            targetGuid[7] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            targetGuid[5] = packet.ReadBit();
            targetGuid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();

            packet.ReadXORByte(targetGuid, 7);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(targetGuid, 6);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(targetGuid, 0);
            packet.ReadXORByte(targetGuid, 2);
            packet.ReadXORByte(targetGuid, 5);
            packet.ReadXORByte(targetGuid, 3);
            packet.ReadXORByte(targetGuid, 4);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(targetGuid, 1);
            packet.ReadXORByte(guid, 6);
            packet.WriteGuid("Guid", guid);
            packet.WriteGuid("targetGuid", targetGuid);
        }

        [Parser(Opcode.CMSG_PETITION_BUY)]
        public static void HandlePetitionBuy(Packet packet)
        {
            var guid144 = new byte[8];
            guid144[5] = packet.ReadBit();
            guid144[2] = packet.ReadBit();
            guid144[3] = packet.ReadBit();
            var len16 = packet.ReadBits(7);
            guid144[4] = packet.ReadBit();
            guid144[1] = packet.ReadBit();
            guid144[7] = packet.ReadBit();
            guid144[0] = packet.ReadBit();
            guid144[6] = packet.ReadBit();

            packet.ReadWoWString("str", len16);
            packet.ParseBitStream(guid144, 1, 7, 4, 6, 0, 5, 2, 3);
            packet.WriteGuid("Guid", guid144);
        }

        [Parser(Opcode.CMSG_PETITION_DECLINE)]
        public static void HandlePetitionDecline(Packet packet)
        {
            var guid = packet.StartBitStream(5, 6, 4, 3, 1, 7, 0, 2);
            packet.ParseBitStream(guid, 6, 2, 1, 5, 0, 7, 4, 3);
            packet.WriteGuid("GUID", guid);
        }

        [Parser(Opcode.CMSG_PETITION_QUERY)]
        public static void HandlePetitionQuery(Packet packet)
        {
            packet.ReadInt32("unk24"); // 24
            var guid = packet.StartBitStream(2, 3, 1, 0, 4, 7, 6, 5);
            packet.ParseBitStream(guid, 0, 4, 7, 5, 1, 6, 3, 2);
            packet.WriteGuid("GUID", guid);
        }

        [Parser(Opcode.CMSG_PETITION_RENAME)]
        public static void HandlePetitionRename(Packet packet)
        {
            var len24 = packet.ReadBits(7);
            var guid = packet.StartBitStream(7, 4, 6, 2, 0, 5, 3, 1);
            packet.ParseBitStream(guid, 4, 1, 7);
            packet.ReadWoWString("str", len24);
            packet.ParseBitStream(guid, 0, 3, 2, 6, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_PETITION_SHOW_SIGNATURES)]
        public static void HandlePetitionShowSignatures(Packet packet)
        {
            var guid = packet.StartBitStream(3, 7, 2, 4, 5, 6, 0, 1);
            packet.ParseBitStream(guid, 2, 4, 5, 7, 1, 0, 3, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_PETITION_SHOW_LIST)]
        public static void HandlePetitionShowlist(Packet packet)
        {
            var guid = packet.StartBitStream(1, 7, 2, 5, 4, 0, 3, 6);
            packet.ParseBitStream(guid, 6, 3, 2, 4, 1, 7, 5, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_PETITION_SIGN)]
        public static void HandlePetitionSign(Packet packet)
        {
            packet.ReadByte("unk24"); // 24
            var guid = packet.StartBitStream(4, 2, 0, 1, 5, 3, 6, 7);
            packet.ParseBitStream(guid, 6, 1, 7, 2, 5, 3, 0, 4);
            packet.WriteGuid("GUID", guid);
        }

        [Parser(Opcode.CMSG_REQUEST_GUILD_PARTY_STATE)]
        public static void HandleRequestGuildPartyState(Packet packet)
        {
            var guid = packet.StartBitStream(1, 5, 7, 2, 6, 3, 0, 4);
            packet.ParseBitStream(guid, 2, 5, 4, 6, 1, 0, 7, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_REQUEST_GUILD_XP)]
        public static void HandleRequestGuildXp(Packet packet)
        {
            var guid = packet.StartBitStream(5, 6, 0, 1, 3, 7, 4, 2);
            packet.ParseBitStream(guid, 4, 6, 3, 0, 7, 5, 2, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_TURN_IN_PETITION)]
        public static void HandlePetitionTurnIn(Packet packet)
        {
            var guid = packet.StartBitStream(1, 2, 3, 0, 5, 7, 4, 6);
            packet.ParseBitStream(guid, 2, 1, 4, 6, 0, 7, 5, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_GUILD_ACHIEVEMENT_DATA)]
        public static void HandleGuildAchievementData(Packet packet)
        {
            var count = packet.ReadBits("count", 20);
            var guids = new byte[count][];
            for (var i = 0; i < count; i++)
                guids[i] = packet.StartBitStream(0, 6, 2, 1, 7, 4, 5, 3);
            for (var i = 0; i < count; i++)
            {
                packet.ReadInt32("Achievement ID", i); // 20
                packet.ParseBitStream(guids[i], 5, 0);
                packet.ReadPackedTime("Time", i);
                packet.ParseBitStream(guids[i], 4, 3, 1, 6, 2);
                packet.ReadInt32("unk57", i); // 57
                packet.ReadInt32("unk53", i); // 53
                packet.ParseBitStream(guids[i], 7);
                packet.WriteGuid("guids", guids[i], i);
            }
        }

        [Parser(Opcode.SMSG_GUILD_MEMBER_DAILY_RESET)]
        public static void HandleServerGuildMemberDailyReset(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_GUILD_PARTY_STATE)]
        public static void HandleGuildPartyStateResponse(Packet packet)
        {
            packet.ReadInt32("Unk20"); // 20
            packet.ReadSingle("unk24"); // 24
            packet.ReadInt32("unk28"); // 28
            packet.ReadBit("unk16"); // 16
        }

        [Parser(Opcode.SMSG_GUILD_MOTD)]
        [Parser(Opcode.SMSG_GUILD_NEWS_TEXT)]
        public static void HandleNewText(Packet packet)
        {
            packet.ReadWoWString("Text", (int)packet.ReadBits(10));
        }

        [Parser(Opcode.SMSG_QUERY_GUILD_INFO_RESPONSE)]
        public static void HandleGuildQueryResponse(Packet packet)
        {
            var guid = new byte[8];
            var guid2 = new byte[8];

            uint[] rankName = null;
            var count = 0u;
            uint nameLen = 0;


            guid[5] = packet.ReadBit();
            var hasData = packet.ReadBit("hasData");
            if (hasData)
            {
                count = packet.ReadBits("Rank Count", 21);
                guid2[5] = packet.ReadBit();
                guid2[1] = packet.ReadBit();
                guid2[4] = packet.ReadBit();
                guid2[7] = packet.ReadBit();
                rankName = new uint[count];
                for (var i = 0; i < count; i++)
                {
                    rankName[i] = packet.ReadBits(7);
                }
                guid2[3] = packet.ReadBit();
                guid2[2] = packet.ReadBit();
                guid2[0] = packet.ReadBit();
                guid2[6] = packet.ReadBit();

                nameLen = packet.ReadBits(7);
            }
            guid[3] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            if (hasData)
            {
                packet.ReadInt32("Emblem Border Style");
                packet.ReadInt32("Emblem Style");
                packet.ParseBitStream(guid2, 2, 7);
                packet.ReadInt32("Emblem Color");
                packet.ReadInt32("RealmID"); // 32
                for (var i = 0; i < count; i++)
                {
                    packet.ReadInt32("Rights Order", i);
                    packet.ReadInt32("Creation Order", i);
                    packet.ReadWoWString("Rank Name", rankName[i], i);
                }
                var name = packet.ReadWoWString("Guild Name", nameLen);
                packet.ReadInt32("Emblem Background Color");
                packet.ParseBitStream(guid2, 5, 4);
                packet.ReadInt32("Emblem Border Color");
                packet.ParseBitStream(guid2, 1, 6, 0, 3);
                packet.WriteGuid("Guid2", guid2);
                var Guid = new WowGuid64(BitConverter.ToUInt64(guid2, 0));
                StoreGetters.AddOrUpdateName(Guid, name);
            }
            packet.ParseBitStream(guid, 2, 6, 4, 0, 7, 3, 5, 1);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_GUILD_RANKS)]
        public static void HandleGuildRankServer(Packet packet)
        {
            const int guildBankMaxTabs = 8;
            var count = packet.ReadBits("Count", 17);
            var length = new uint[count];

            for (var i = 0; i < count; ++i)
                length[i] = packet.ReadBits(7);

            for (var i = 0; i < count; ++i)
            {
                packet.ReadInt32("Creation Order", i);
                packet.ReadInt32("Gold Per Day", i);

                for (var j = 0; j < guildBankMaxTabs; ++j)
                {
                    packet.ReadInt32("Tab Slots", i, j);
                    packet.ReadInt32E<GuildBankRightsFlag>("Tab Rights", i, j);
                }

                packet.ReadWoWString("Name", length[i], i);

                packet.ReadInt32("Rights Order", i);
                packet.ReadInt32("Unk 1", i);
            }
        }

        [Parser(Opcode.SMSG_GUILD_RENAMED)]
        public static void HandleGuildRenamed(Packet packet)
        {
            var guid120 = new byte[8];
            guid120[0] = packet.ReadBit();
            guid120[7] = packet.ReadBit();
            guid120[3] = packet.ReadBit();
            guid120[1] = packet.ReadBit();
            guid120[5] = packet.ReadBit();
            guid120[6] = packet.ReadBit();
            var count16 = packet.ReadBits("count16", 7);
            guid120[2] = packet.ReadBit();
            guid120[4] = packet.ReadBit();
            packet.ReadWoWString("Name", count16);
            packet.ParseBitStream(guid120, 3, 2, 6, 7, 5, 0, 1, 4);
            packet.WriteGuid("Guid", guid120);
        }

        [Parser(Opcode.SMSG_GUILD_REPUTATION_WEEKLY_CAP)]
        public static void HandleGuildReputationWeeklyCap(Packet packet)
        {
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.SMSG_GUILD_ROSTER)] // sub_6A8B6B
        public static void HandleGuildRoster(Packet packet)
        {
            var count16 = packet.ReadBits("count16", 17);
            var count2041 = packet.ReadBits("count2041", 10);
            var guid = new byte[count16][];
            var unk53 = new uint[count16];
            var unk238 = new uint[count16];
            var unk109 = new uint[count16];
            for (var i = 0; i < count16; i++)
            {
                guid[i] = new byte[8];
                unk238[i] = packet.ReadBits("unk238*4", 8, i); // 238
                guid[i][5] = packet.ReadBit();
                packet.ReadBit("unk372*4", i); // 372
                unk109[i] = packet.ReadBits("unk109*4", 8, i); // 109
                guid[i][7] = packet.ReadBit();
                guid[i][0] = packet.ReadBit();
                guid[i][6] = packet.ReadBit();
                unk53[i] = packet.ReadBits("unk53*4", 6, i); // 53
                packet.ReadBit("unk371*4", i); // 371
                guid[i][3] = packet.ReadBit();
                guid[i][4] = packet.ReadBit();
                guid[i][1] = packet.ReadBit();
                guid[i][2] = packet.ReadBit();
            }
            var count40 = packet.ReadBits("count40", 11);
            for (var i = 0; i < count16; i++)
            {
                packet.ReadByte("unk384", i); // 384
                packet.ReadInt32("unk41*4", i); // 41
                packet.ReadWoWString("str", unk53[i], i);
                packet.ParseBitStream(guid[i], 0);
                for (var j = 0; j < 2; j++)
                {
                    packet.ReadInt32("unk381*4", i, j); // 381
                    packet.ReadInt32("unk373*4", i, j); // 373
                    packet.ReadInt32("unk377*4", i, j); // 377
                }
                packet.ReadByte("unk383", i); // 383
                packet.ReadByte("unk382", i); // 382
                packet.ReadInt32("unk33*4", i); // 33
                packet.ReadInt32("unk45*4", i); // 45
                packet.ParseBitStream(guid[i], 3);
                packet.ReadInt64("unk36", i); // 36
                packet.ReadWoWString("str2", unk238[i], i);
                packet.ReadSingle("unk2ch", i); // 2ch
                packet.ReadByte("unk385", i); // 385
                packet.ReadInt32("unk29", i); // 29
                packet.ReadInt32("unk105", i); // 105
                packet.ParseBitStream(guid[i], 5, 7);
                packet.ReadWoWString("str3", unk109[i], i);
                packet.ParseBitStream(guid[i], 4);
                packet.ReadInt64("unk52", i); // 52
                packet.ReadInt32("unk37*4", i); // 37
                packet.ParseBitStream(guid[i], 6, 1, 2);
                packet.WriteGuid("Guid", guid[i], i);
            }
            packet.ReadInt32("unk36"); // 36
            packet.ReadPackedTime("Time");
            packet.ReadWoWString("str4", count40);
            packet.ReadInt32("unk647*4"); // 647
            packet.ReadWoWString("str5", count2041);
            packet.ReadInt32("unk32"); // 32
        }

        [Parser(Opcode.SMSG_GUILD_SET_NOTE)]
        public static void HandleGuildClientSetNote(Packet packet)
        {
            var playerGUID = new byte[8];

            playerGUID[1] = packet.ReadBit();
            var noteLength = packet.ReadBits("note length", 8);
            var ispublic = packet.ReadBit("IsPublic");
            playerGUID[4] = packet.ReadBit();
            playerGUID[2] = packet.ReadBit();
            playerGUID[3] = packet.ReadBit();
            playerGUID[5] = packet.ReadBit();
            playerGUID[0] = packet.ReadBit();
            playerGUID[6] = packet.ReadBit();
            playerGUID[7] = packet.ReadBit();

            packet.ReadXORByte(playerGUID, 5);
            packet.ReadXORByte(playerGUID, 1);
            packet.ReadXORByte(playerGUID, 6);
            packet.ReadXORByte(playerGUID, 0);
            packet.ReadWoWString("note", noteLength);
            packet.ReadXORByte(playerGUID, 7);
            packet.ReadXORByte(playerGUID, 4);
            packet.ReadXORByte(playerGUID, 3);
            packet.ReadXORByte(playerGUID, 2);
            packet.ResetBitReader();

            packet.WriteGuid("Player GUID", playerGUID);
        }

        [Parser(Opcode.SMSG_GUILD_XP)]
        public static void HandleGuildXP(Packet packet)
        {
            packet.ReadUInt64("unk32"); // 32
            packet.ReadUInt64("unk40"); // 40
            packet.ReadUInt64("unk16"); // 16
            packet.ReadUInt64("unk24"); // 24
        }

        [Parser(Opcode.SMSG_GUILD_XP_GAIN)]
        public static void HandleGuildXPResponse(Packet packet)
        {
            packet.ReadInt64("XP");
        }

        [Parser(Opcode.SMSG_LF_GUILD_BROWSE_UPDATED)]
        public static void HandlerLFGuildBrowseUpdated(Packet packet)
        {
            var count = packet.ReadBits("count", 18);
            var guids = new byte[count][];
            var lens20 = new uint[count];
            var lens169 = new uint[count];
            for (var i = 0; i < count; i++)
            {
                guids[i] = new byte[8];
                guids[i][6] = packet.ReadBit();
                guids[i][5] = packet.ReadBit();
                guids[i][4] = packet.ReadBit();
                guids[i][0] = packet.ReadBit();
                guids[i][1] = packet.ReadBit();
                lens169[i] = packet.ReadBits("unk169", 10, i); // 169*4
                guids[i][3] = packet.ReadBit();
                lens20[i] = packet.ReadBits("unk20", 7, i); // 20
                guids[i][7] = packet.ReadBit();
                guids[i][2] = packet.ReadBit();
            }
            for (var i = 0; i < count; i++)
            {
                packet.ParseBitStream(guids[i], 3);
                packet.ReadInt32("unk149", i); // 149*4
                packet.ReadByte("unk1194", i); // 1194
                packet.ParseBitStream(guids[i], 0);
                packet.ReadInt32("unk129", i); // 129
                packet.ParseBitStream(guids[i], 2);
                packet.ReadInt32("unk133", i); // 133
                packet.ReadInt32("unk165", i); // 165
                packet.ReadInt32("unk121", i); // 121
                packet.ReadInt32("unk137", i); // 137
                packet.ReadInt32("unk141", i); // 141
                packet.ParseBitStream(guids[i], 5);
                packet.ReadInt32("unk117", i); // 117
                packet.ReadWoWString("str", lens20[i], i);
                packet.ReadInt32("unk157", i); // 157
                packet.ReadByte("unk1208", i); // 1208
                packet.ParseBitStream(guids[i], 7);
                packet.ReadInt32("unk153", i); // 153
                packet.ParseBitStream(guids[i], 6);
                packet.ReadInt32("unk145", i); // 145
                packet.ReadWoWString("str2", lens169[i]);
                packet.ReadInt32("unk161", i); // 161
                packet.ReadInt32("unk125", i); // 125
                packet.ParseBitStream(guids[i], 1, 4);
                packet.WriteGuid("Guid", guids[i], i);
            }
        }

        [Parser(Opcode.SMSG_TABARD_VENDOR_ACTIVATE)]
        public static void HandleTabardVendorActivate(Packet packet)
        {
            var guid = packet.StartBitStream(1, 5, 0, 7, 4, 6, 3, 2);
            packet.ParseBitStream(guid, 5, 4, 2, 3, 6, 0, 1, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_TURN_IN_PETITION_RESULT)]
        public static void HandlePetitionTurnInResults(Packet packet)
        {
            packet.ReadBitsE<PetitionResultType>("Result", 4);
        }

        [Parser(Opcode.SMSG_PLAYER_SAVE_GUILD_EMBLEM)]
        public static void HandlePlayerSaveGuildEmblem(Packet packet)
        {
            packet.ReadInt32E<GuildEmblemError>("Error");
        }

        [Parser(Opcode.SMSG_PETITION_SHOW_LIST)]
        public static void HandlePetitionShowList(Packet packet)
        {
            var guid = packet.StartBitStream(3, 5, 7, 6, 1, 0, 2, 4);
            packet.ParseBitStream(guid, 6, 0, 1);
            packet.ReadUInt32("Price");
            packet.ParseBitStream(guid, 4, 3, 5, 2, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_PETITION_QUERY_RESPONSE)]
        public static void HandlePetitionQueryResponse(Packet packet)
        {
            packet.ReadUInt32("low Guild/Team GUID");
            var hasData = packet.ReadBit("HasData");

            var NameLen = new uint[10];

            var guid = new byte[8];

            uint strlen42 = 0;
            uint strlen10 = 0;

            if (hasData)
            {
                for (var i = 0; i < 10; ++i)
                    NameLen[i] = packet.ReadBits(6);
                guid[2] = packet.ReadBit();
                guid[4] = packet.ReadBit();
                strlen42 = packet.ReadBits(12);
                guid[0] = packet.ReadBit();
                guid[7] = packet.ReadBit();
                guid[3] = packet.ReadBit();
                guid[6] = packet.ReadBit();
                guid[5] = packet.ReadBit();
                strlen10 = packet.ReadBits(7);
                guid[1] = packet.ReadBit();
            }

            if (hasData)
            {
                packet.ParseBitStream(guid, 5);
                packet.ReadInt32("unk1076"); // 1076
                packet.ReadWoWString("str10", strlen10); // 10
                packet.ReadInt32("unk1068"); // 1068
                packet.ReadWoWString("str42", strlen42); // 42
                packet.ParseBitStream(guid, 4);
                packet.ReadInt32("Type"); // 1067
                packet.ParseBitStream(guid, 6);
                packet.ReadInt32("unk1070"); // 1070
                packet.ReadInt32("Signs"); // 1066
                for (var i = 0; i < 10; ++i)
                    packet.ReadWoWString("str", NameLen[i], i);
                packet.ParseBitStream(guid, 1);
                packet.ParseBitStream(guid, 7);
                packet.ParseBitStream(guid, 0);
                packet.ReadInt32("unk1074"); // 1074
                packet.ReadInt32("unk1075"); // 1075
                packet.ParseBitStream(guid, 2);
                packet.ReadTime("Time"); // 6
                packet.ReadInt16("unk2146"); // 2146
                packet.ReadInt32("unk1077"); // 1077
                packet.ParseBitStream(guid, 3);
                packet.ReadInt32("unk1071"); // 1071
                packet.ReadInt32("unk1072"); // 1072
                packet.ReadInt32("unk1069"); // 1069
                packet.ReadInt32("unk1078"); // 1078

                packet.WriteGuid("Guid", guid);
            }
        }

        [Parser(Opcode.SMSG_PETITION_SHOW_SIGNATURES)]
        public static void HandlePetitionShowSignaturesServer(Packet packet)
        {
            var guid40 = new byte[8];
            var guid48 = new byte[8];

            packet.StartBitStream(guid40, 1);
            packet.StartBitStream(guid48, 3);
            packet.StartBitStream(guid40, 3);
            packet.StartBitStream(guid48, 4, 0);
            packet.StartBitStream(guid40, 7, 5);
            packet.StartBitStream(guid48, 1, 5, 7);
            packet.StartBitStream(guid40, 0, 6);
            packet.StartBitStream(guid48, 6);
            packet.StartBitStream(guid40, 2, 4);

            var count = packet.ReadBits("count", 21);
            var guids = new byte[count][];
            for (var i = 0; i < count; i++)
                guids[i] = packet.StartBitStream(2, 0, 4, 7, 5, 1, 6, 3);

            packet.StartBitStream(guid48, 2);

            for (var i = 0; i < count; i++)
            {
                packet.ParseBitStream(guids[i], 6, 0, 1, 3, 2, 5, 7, 4);
                packet.ReadInt32("Choise", i);
                packet.WriteGuid("Guid", guids[i], i);
            }

            packet.ParseBitStream(guid48, 6, 5, 4);
            packet.ParseBitStream(guid40, 4);
            packet.ParseBitStream(guid48, 1);

            packet.ReadInt32("PetitionID");

            packet.ParseBitStream(guid48, 2, 3, 7);
            packet.ParseBitStream(guid40, 5, 6, 3, 7, 1, 0);
            packet.ParseBitStream(guid48, 0);
            packet.ParseBitStream(guid40, 2);

            packet.WriteGuid("Owner", guid40);
            packet.WriteGuid("Item", guid48);
        }

        [Parser(Opcode.SMSG_GUILD_BANK_LOG_QUERY_RESULTS)]
        [Parser(Opcode.SMSG_GUILD_BANK_QUERY_RESULTS)]
        [Parser(Opcode.SMSG_GUILD_CHALLENGE_UPDATE)]
        [Parser(Opcode.SMSG_GUILD_COMMAND_RESULT)]
        [Parser(Opcode.SMSG_GUILD_EVENT_LOG_QUERY_RESULTS)]
        [Parser(Opcode.SMSG_GUILD_NEWS_UPDATE)]
        [Parser(Opcode.SMSG_GUILD_SEND_RANK_CHANGE)]
        [Parser(Opcode.SMSG_GUILD_REWARD_LIST)]
        [Parser(Opcode.SMSG_PETITION_ALREADY_SIGNED)]
        [Parser(Opcode.SMSG_PETITION_RENAME_RESPONSE)]
        [Parser(Opcode.SMSG_PETITION_SIGN_RESULTS)]
        public static void HandleGuild(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_GUILD_DISBAND)]
        [Parser(Opcode.CMSG_GUILD_EVENT_LOG_QUERY)]
        [Parser(Opcode.CMSG_GUILD_LEAVE)]
        [Parser(Opcode.CMSG_GUILD_REQUEST_CHALLENGE_UPDATE)]
        [Parser(Opcode.SMSG_GUILD_EVENT_BANK_CONTENTS_CHANGED)]
        [Parser(Opcode.SMSG_GUILD_EVENT_DISBANDED)]
        [Parser(Opcode.SMSG_GUILD_EVENT_TAB_ADDED)]
        [Parser(Opcode.SMSG_GUILD_EVENT_TAB_DELETED)]
        [Parser(Opcode.SMSG_GUILD_EVENT_RANKS_UPDATED)]
        public static void HandleGuildNull(Packet packet)
        {
        }
    }
}
