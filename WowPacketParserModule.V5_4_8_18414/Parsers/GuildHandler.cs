using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class GuildHandler
    {
        [Parser(Opcode.CMSG_AUTO_DECLINE_GUILD_INVITES)]
        public static void HandleAutodeclineGuildInvites(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_ADD_RANK)]
        public static void HandleGuildAddRank(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_ASSIGN_MEMBER_RANK)]
        public static void HandleGuildAssignMemberRank(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_BANK_BUY_TAB)]
        public static void HandleGuildBankBuyTab(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_BANK_DEPOSIT_MONEY)]
        public static void HandleGuildBankDepositMoney(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_BANK_LOG_QUERY)]
        public static void HandleGuildBankLogQuery(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_BANK_UPDATE_TAB)]
        public static void HandleGuildBankUpdateTab(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_BANKER_ACTIVATE)]
        public static void HandleGuildBankerActivate(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_DECLINE)]
        public static void HandleGuildDecline(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_GUILD_DEL_RANK)]
        public static void HandleGuildDelRank(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_DEMOTE)]
        public static void HandleGuildDemote(Packet packet)
        {
            var guid = packet.StartBitStream(3, 6, 0, 2, 7, 5, 4, 1);
            packet.ParseBitStream(guid, 7, 4, 2, 5, 1, 3, 0, 6);
            packet.WriteGuid("GUID", guid);
        }

        [Parser(Opcode.CMSG_GUILD_DISBAND)]
        public static void HandleGuildDisband(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_EVENT_LOG_QUERY)]
        public static void HandleGuildEventLogQuery(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_INFO_TEXT)]
        public static void HandleGuildInfoText(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_INVITE)]
        public static void HandleGuildInvite(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_LEAVE)]
        public static void HandleGuildLeave(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_MOTD)]
        public static void HandleGuildMotd(Packet packet)
        {
            var len = packet.ReadBits("Len", 10);
            packet.ReadWoWString("Motd", len);
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

        [Parser(Opcode.CMSG_GUILD_SET_ACHIEVEMENT_TRACKING)]
        public static void HandleGuildSetAchievementTracking(Packet packet)
        {
            var count = packet.ReadBits("Count", 22);
            for (var i = 0; i < count; ++i)
                packet.ReadUInt32("Criteria Id", i); // 24
        }

        [Parser(Opcode.CMSG_GUILD_NEWS_UPDATE_STICKY)]
        [Parser(Opcode.CMSG_GUILD_PROMOTE)]
        [Parser(Opcode.CMSG_GUILD_QUERY_NEWS)]
        [Parser(Opcode.CMSG_GUILD_QUERY_RANKS)]
        [Parser(Opcode.CMSG_GUILD_REMOVE)]
        [Parser(Opcode.CMSG_GUILD_REQUEST_CHALLENGE_UPDATE)]
        [Parser(Opcode.CMSG_GUILD_ROSTER)]
        [Parser(Opcode.CMSG_GUILD_SET_GUILD_MASTER)]
        [Parser(Opcode.SMSG_GUILD_COMMAND_RESULT)]
        [Parser(Opcode.SMSG_GUILD_NEWS_UPDATE)]
        [Parser(Opcode.SMSG_GUILD_RANKS_UPDATE)]
        [Parser(Opcode.SMSG_GUILD_REPUTATION_WEEKLY_CAP)]
        [Parser(Opcode.SMSG_GUILD_REWARDS_LIST)]
        [Parser(Opcode.SMSG_GUILD_XP)]
        [Parser(Opcode.SMSG_GUILD_XP_GAIN)]
        [Parser(Opcode.SMSG_PETITION_ALREADY_SIGNED)]
        [Parser(Opcode.SMSG_PETITION_QUERY_RESPONSE)]
        [Parser(Opcode.SMSG_PETITION_RENAME_RESULT)]
        [Parser(Opcode.SMSG_PETITION_SHOWLIST)]
        [Parser(Opcode.SMSG_PETITION_SHOW_SIGNATURES)]
        [Parser(Opcode.SMSG_PETITION_SIGN_RESULTS)]
        [Parser(Opcode.SMSG_TURN_IN_PETITION_RESULTS)]
        public static void HandleGuild(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_QUERY)]
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
            packet.ReadToEnd();
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
            var guid = packet.StartBitStream(2, 3, 1, 0, 4, 7, 6, 5);
            packet.ParseBitStream(guid, 0, 4, 7, 5, 1, 6, 3, 2);
            packet.WriteGuid("GUID", guid);
        }

        [Parser(Opcode.CMSG_PETITION_RENAME)]
        public static void HandlePetitionRename(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_PETITION_SHOW_SIGNATURES)]
        public static void HandlePetitionShowSignatures(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_PETITION_SHOWLIST)]
        public static void HandlePetitionShowlist(Packet packet)
        {
            var guid = packet.StartBitStream(1, 7, 2, 5, 4, 0, 3, 6);
            packet.ParseBitStream(guid, 6, 3, 2, 4, 1, 7, 5, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_PETITION_SIGN)]
        public static void HandlePetitionSign(Packet packet)
        {
            var guid = packet.StartBitStream(4, 2, 0, 1, 5, 3, 6, 7);
            packet.ParseBitStream(guid, 6, 1, 7, 2, 5, 3, 0, 4);
            packet.WriteGuid("GUID", guid);
        }

        [Parser(Opcode.CMSG_QUERY_GUILD_REWARDS)]
        public static void HandleQueryGuildRewards(Packet packet)
        {
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.CMSG_QUERY_GUILD_XP)]
        public static void HandleQueryGuildXp(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_TURN_IN_PETITION)]
        public static void HandlePetitionTurnIn(Packet packet)
        {
            packet.ReadToEnd();
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

        [Parser(Opcode.SMSG_GUILD_BANK_LIST)]
        public static void HandleServerGuildBankList(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_GUILD_BANK_LOG_QUERY_RESULT)]
        public static void HandleServerGuildBankLogQueryResult(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_GUILD_CHALLENGE_UPDATED)]
        public static void HandleServerGuildChallengeUpdated(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_GUILD_EVENT_LOG_QUERY_RESULT)]
        public static void HandleServerGuildEventLogQueryResult(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_GUILD_INVITE)]
        public static void HandleServerGuildInvite(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_GUILD_MEMBER_DAILY_RESET)]
        public static void HandleServerGuildMemberDailyReset(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_GUILD_PARTY_STATE_RESPONSE)]
        public static void HandleGuildPartyStateResponse(Packet packet)
        {
            packet.ReadInt32("Unk20"); // 20
            packet.ReadSingle("unk24"); // 24
            packet.ReadInt32("unk28"); // 28
            packet.ReadBit("unk16"); // 16
        }

        [Parser(Opcode.SMSG_GUILD_NEWS_TEXT)]
        public static void HandleNewText(Packet packet)
        {
            packet.ReadWoWString("Text", (int)packet.ReadBits(10));
        }

        [Parser(Opcode.SMSG_GUILD_QUERY_RESPONSE)]
        public static void HandleGuildQueryResponse(Packet packet)
        {
            var guid = new byte[8];
            var guid2 = new byte[8];

            var len = new uint[255];
            var count = 0u;
            var len2 = 0u;


            guid[5] = packet.ReadBit();
            var unk16 = packet.ReadBit("unk16");
            if (unk16)
            {
                count = packet.ReadBits("cnt", 21);
                guid2[5] = packet.ReadBit();
                guid2[1] = packet.ReadBit();
                guid2[4] = packet.ReadBit();
                guid2[7] = packet.ReadBit();
                for (var i = 0; i < count; i++)
                {
                    len[i] = packet.ReadBits(7);
                }
                guid2[3] = packet.ReadBit();
                guid2[2] = packet.ReadBit();
                guid2[0] = packet.ReadBit();
                guid2[6] = packet.ReadBit();

                len2 = packet.ReadBits("unk36", 7);
            }
            guid[3] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            if (unk16)
            {
                packet.ReadInt32("unk160");
                packet.ReadInt32("unk152");
                packet.ParseBitStream(guid2, 2, 7);
                packet.ReadInt32("unk156");
                packet.ReadInt32("RealmID"); // 32
                for (var i = 0; i < count; i++)
                {
                    packet.ReadInt32("unk144", i);
                    packet.ReadInt32("unk140", i);
                    packet.ReadWoWString("str", len[i], i);
                }
                packet.ReadWoWString("str", len2);
                packet.ReadInt32("unk168");
                packet.ParseBitStream(guid2, 5, 4);
                packet.ReadInt32("unk164");
                packet.ParseBitStream(guid2, 1, 6, 0, 3);
                packet.WriteGuid("Guid2", guid2);
            }
            packet.ParseBitStream(guid, 2, 6, 4, 0, 7, 3, 5, 1);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_GUILD_RANK)]
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
                    packet.ReadEnum<GuildBankRightsFlag>("Tab Rights", TypeCode.Int32, i, j);
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
            packet.ReadEnum<GuildFinderOptionsRoles>("Class Roles", TypeCode.UInt32); // 16
            packet.ReadEnum<GuildFinderOptionsAvailability>("Availability", TypeCode.UInt32); // 24
            packet.ReadEnum<GuildFinderOptionsInterest>("Guild Interests", TypeCode.UInt32); // 28
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

        [Parser(Opcode.SMSG_TABARDVENDOR_ACTIVATE)]
        public static void HandleTabardVendorActivate(Packet packet)
        {
            var guid = packet.StartBitStream(1, 5, 0, 7, 4, 6, 3, 2);
            packet.ParseBitStream(guid, 5, 4, 2, 3, 6, 0, 1, 7);
            packet.WriteGuid("Guid", guid);
        }
    }
}
