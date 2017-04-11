using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class LfgHandler
    {
        //64FF95 22996
        public static void ReadCliRideTicket(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("RequesterGuid", idx);
            packet.ReadInt32("Id", idx);
            packet.ReadInt32("Type", idx);
            packet.ReadTime("Time", idx);
        }

        public static void ReadLFGBlackList(Packet packet, params object[] idx)
        {
            packet.ResetBitReader();
            var bit16 = packet.ReadBit("HasPlayerGuid", idx);
            var int24 = packet.ReadInt32("LFGBlackListCount", idx);

            if (bit16)
                packet.ReadPackedGuid128("PlayerGuid", idx);

            for (var i = 0; i < int24; ++i)
            {
                packet.ReadUInt32("Slot", idx, i);
                packet.ReadUInt32("Reason", idx, i);
                packet.ReadInt32("SubReason1", idx, i);
                packet.ReadInt32("SubReason2", idx, i);
            }
        }

        public static void ReadLFGListBlacklistEntry(Packet packet, params object[] indexes)
        {//653291 22996
            packet.ReadInt32("ActivityID", indexes);
            packet.ReadInt32("Reason", indexes);
        }

        public static void ReadLfgBootInfo(Packet packet, params object[] idx)
        {
            packet.ReadBit("VoteInProgress", idx);
            packet.ReadBit("VotePassed", idx);
            packet.ReadBit("MyVoteCompleted", idx);
            packet.ReadBit("MyVote", idx);
            var len = packet.ReadBits(8);
            packet.ReadPackedGuid128("Target", idx);
            packet.ReadUInt32("TotalVotes", idx);
            packet.ReadUInt32("BootVotes", idx);
            packet.ReadInt32("TimeLeft", idx);
            packet.ReadUInt32("VotesNeeded", idx);
            packet.ReadWoWString("Reason", len, idx);
        }

        public static void ReadLFGListJoinRequest(Packet packet, params object[] idx)
        {//650D44 22996
            packet.ReadInt32("ActivityID", idx);
            packet.ReadSingle("RequiredItemLevel", idx);
            packet.ReadInt32("unk3", idx);

            packet.ResetBitReader();

            var lenName = packet.ReadBits(8);
            var lenComment = packet.ReadBits(11);
            var lenVoiceChat = packet.ReadBits(8);
            packet.ReadBit("unkb", idx);

            packet.ReadWoWString("Name", lenName, idx);
            packet.ReadWoWString("Comment", lenComment, idx);
            packet.ReadWoWString("VoiceChat", lenVoiceChat, idx);
        }

        public static void ReadShortageReward(Packet packet, params object[] idx)
        {
            packet.ReadInt32("Mask", idx);
            packet.ReadInt32("RewardMoney", idx);
            packet.ReadInt32("RewardXP", idx);

            var int200 = packet.ReadInt32("ItemCount", idx);
            var int360 = packet.ReadInt32("CurrencyCount", idx);
            var int520 = packet.ReadInt32("QuantityCount", idx);

            // Item
            for (var k = 0; k < int200; ++k)
            {
                packet.ReadInt32("ItemID", idx, k);
                packet.ReadInt32("Quantity", idx, k);
            }

            // Currency
            for (var k = 0; k < int360; ++k)
            {
                packet.ReadInt32("CurrencyID", idx, k);
                packet.ReadInt32("Quantity", idx, k);
            }

            // BonusCurrency
            for (var k = 0; k < int520; ++k)
            {
                packet.ReadInt32("CurrencyID", idx, k);
                packet.ReadInt32("Quantity", idx, k);
            }

            packet.ResetBitReader();

            var bit16 = packet.ReadBit("HasBit16", idx);
            var bit24 = packet.ReadBit("HasBit24", idx);
            var bit32 = packet.ReadBit("HasBit32", idx);
            var bit40 = packet.ReadBit("HasBit40", idx);
            if (bit16)
                packet.ReadInt32("Unk16", idx);
            if (bit24)
                packet.ReadInt32("Unk24", idx);
            if (bit32)
                packet.ReadInt32("Unk32", idx);
            if (bit40)
                packet.ReadInt32("Unk40", idx);
        }

        public static void sub_650E43(Packet packet, params object[] idx)
        {//650E43 22996 651B56 23420
            ReadCliRideTicket(packet, idx, "RideTicket");
            packet.ReadInt32("unk", idx);
            packet.ReadPackedGuid128("guid1336", idx);
            packet.ReadPackedGuid128("guid1352", idx);
            packet.ReadPackedGuid128("guid1368", idx);
            packet.ReadPackedGuid128("guid1384", idx);
            packet.ReadInt32("VirtualRealmAddress", idx);//1400
            var cnt1404 = packet.ReadInt32("cnt1404", idx);
            var cnt1420 = packet.ReadInt32("cnt1420", idx);
            var cnt1436 = packet.ReadInt32("cnt1436", idx);
            var cnt1452 = packet.ReadInt32("cnt1452", idx);
            packet.ReadInt32("unk1468", idx);
            packet.ReadInt32("unk1472", idx);
            packet.ReadByte("unk1476", idx);
            for (var j = 0; j < cnt1404; ++j)
                packet.ReadPackedGuid128("guid1408", idx, j);
            for (var j = 0; j < cnt1420; ++j)
                packet.ReadPackedGuid128("guid1424", idx, j);
            for (var j = 0; j < cnt1436; ++j)
                packet.ReadPackedGuid128("guid1440", idx, j);
            for (var j = 0; j < cnt1452; ++j)
            {//679F95 22996
                packet.ReadByte("unk", idx, j);
                packet.ReadByte("unk1", idx, j);
            }
            {//650D44 22996
                packet.ReadInt32("unk", idx);
                packet.ReadInt32("unk", idx);
                packet.ReadInt32("unk", idx);
                packet.ResetBitReader();
                var len12 = packet.ReadBits(8);
                var len141 = packet.ReadBits(11);
                var len1166 = packet.ReadBits(8);
                packet.ReadBit("unkbit", idx);
                Bit hasUnk518 = 0;
                if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_1_5_23420))
                {
                    packet.ReadBit("unkbit2", idx);
                    hasUnk518 = packet.ReadBit("unkbit3", idx);
                }
                packet.ReadWoWString("str", len12, idx);
                packet.ReadWoWString("str2", len141, idx);
                packet.ReadWoWString("str3", len1166, idx);
                if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_1_5_23420))
                    if (hasUnk518)
                        packet.ReadInt32("unk", idx);
            }
        }

        [Parser(Opcode.CMSG_LFG_LIST_GET_STATUS)]
        [Parser(Opcode.CMSG_REQUEST_LFG_LIST_BLACKLIST)]
        public static void HandleLfgZero(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_LFG_LIST_APPLY_TO_GROUP)]
        public static void HandleLfgListApplyToGroup(Packet packet)
        {//063AC1E 22996
            //652C07 22996
            ReadCliRideTicket(packet, "RideTicket2");

            packet.ReadInt32("unk48");
            packet.ReadByte("unk52");
            packet.ResetBitReader();
            int cnt = (int)packet.ReadBits("unkb", 8);
            packet.ReadBytes(cnt);
        }

        [Parser(Opcode.SMSG_LFG_PLAYER_INFO)]
        public static void HandleLfgPlayerLockInfoResponse(Packet packet)
        {
            var int16 = packet.ReadInt32("DungeonCount");

            ReadLFGBlackList(packet, "LFGBlackList");

            // LfgPlayerDungeonInfo 65134D 22996
            for (var i = 0; i < int16; ++i)
            {
                packet.ReadInt32("Slot", i);
                packet.ReadInt32("CompletionQuantity", i);
                packet.ReadInt32("CompletionLimit", i);
                packet.ReadInt32("CompletionCurrencyID", i);
                packet.ReadInt32("SpecificQuantity", i);
                packet.ReadInt32("SpecificLimit", i);
                packet.ReadInt32("OverallQuantity", i);
                packet.ReadInt32("OverallLimit", i);
                packet.ReadInt32("PurseWeeklyQuantity", i);
                packet.ReadInt32("PurseWeeklyLimit", i);
                packet.ReadInt32("PurseQuantity", i);
                packet.ReadInt32("PurseLimit", i);
                packet.ReadInt32("Quantity", i);
                packet.ReadInt32("CompletedMask", i);

                packet.ReadInt32("unk", i);

                var int64 = packet.ReadInt32("ShortageRewardCount", i);

                packet.ResetBitReader();

                packet.ReadBit("FirstReward", i);
                packet.ReadBit("ShortageEligible", i);

                ReadShortageReward(packet, i, "ShortageReward");

                // ShortageReward
                for (var j = 0; j < int64; ++j)
                    ReadShortageReward(packet, i, j, "ShortageReward");

            }
        }

        [Parser(Opcode.SMSG_LFG_JOIN_RESULT)]
        public static void HandleLfgJoinResult(Packet packet)
        {
            ReadCliRideTicket(packet);

            packet.ReadByte("Result");
            packet.ReadByte("ResultDetail");

            var int16 = packet.ReadInt32("BlackListCount");
            for (int i = 0; i < int16; i++)
            {
                packet.ReadPackedGuid128("Guid", i);

                var int160 = packet.ReadInt32("SlotsCount", i);

                for (int j = 0; j < int160; j++)
                {
                    packet.ReadInt32("Slot", i, j);
                    packet.ReadInt32("Reason", i, j);
                    packet.ReadInt32("SubReason1", i, j);
                    packet.ReadInt32("SubReason2", i, j);
                }
            }
        }

        [Parser(Opcode.SMSG_LFG_UPDATE_STATUS)]
        public static void HandleLfgQueueStatusUpdate(Packet packet)
        {
            ReadCliRideTicket(packet);

            packet.ReadByte("SubType");
            packet.ReadByte("Reason");

            var int56 = packet.ReadInt32("SlotsCount");
            packet.ReadInt32("RequestedRoles");
            var int76 = packet.ReadInt32("SuspendedPlayersCount");

            for (int i = 0; i < int56; i++)
                packet.ReadInt32("Slots", i);

            for (int i = 0; i < int76; i++)
                packet.ReadPackedGuid128("SuspendedPlayers", i);

            packet.ResetBitReader();

            packet.ReadBit("IsParty");
            packet.ReadBit("NotifyUI");
            packet.ReadBit("Joined");
            packet.ReadBit("LfgJoined");
            packet.ReadBit("Queued");
        }

        [Parser(Opcode.CMSG_DF_GET_SYSTEM_INFO)]
        public static void HandleLFGLockInfoRequest(Packet packet)
        {
            packet.ReadBit("Player");
            packet.ReadByte("PartyIndex");
        }

        [Parser(Opcode.SMSG_LFG_QUEUE_STATUS)]
        public static void HandleLfgQueueStatusUpdate434(Packet packet)
        {
            ReadCliRideTicket(packet);

            packet.ReadInt32("Slot");
            packet.ReadInt32("AvgWaitTime");
            packet.ReadInt32("QueuedTime");

            for (int i = 0; i < 3; i++)
            {
                packet.ReadInt32("AvgWaitTimeByRole", i);
                packet.ReadByte("LastNeeded", i);
            }

            packet.ReadInt32("AvgWaitTimeMe");
        }

        [Parser(Opcode.SMSG_LFG_PROPOSAL_UPDATE)]
        public static void HandleLfgProposalUpdate(Packet packet)
        {
            ReadCliRideTicket(packet);

            packet.ReadInt64("InstanceID");

            packet.ReadInt32("ProposalID");
            packet.ReadInt32("Slot");

            packet.ReadByte("State");

            packet.ReadInt32("CompletedMask");
            var int68 = packet.ReadInt32("PlayersCount");
            for (int i = 0; i < int68; i++)
            {
                packet.ReadInt32("Roles", i);

                packet.ResetBitReader();

                packet.ReadBit("Me", i);
                packet.ReadBit("SameParty", i);
                packet.ReadBit("MyParty", i);
                packet.ReadBit("Responded", i);
                packet.ReadBit("Accepted", i);
            }

            packet.ResetBitReader();

            packet.ReadBit("ValidCompletedMask");
            packet.ReadBit("ProposalSilent");
        }

        public static void ReadLFGPlayerRewards(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("RewardItem", indexes);
            packet.ReadUInt32("RewardItemQuantity", indexes);
            packet.ReadInt32("BonusCurrency", indexes);
            packet.ReadBit("IsCurrency", indexes);
        }

        [Parser(Opcode.SMSG_LFG_PLAYER_REWARD)]
        public static void HandleLfgPlayerReward(Packet packet)
        {
            packet.ReadUInt32("ActualSlot"); // unconfirmed order
            packet.ReadUInt32("QueuedSlot"); // unconfirmed order
            packet.ReadInt32("RewardMoney");
            packet.ReadInt32("AddedXP");

            var count = packet.ReadInt32("RewardsCount");
            for (var i = 0; i < count; ++i)
                ReadLFGPlayerRewards(packet, i);
        }

        [Parser(Opcode.CMSG_DF_JOIN)]
        public static void HandleDFJoin(Packet packet)
        {
            packet.ReadBit("QueueAsGroup");
            var commentLength = packet.ReadBits("UnkBits8", 8);

            packet.ResetBitReader();

            packet.ReadByte("PartyIndex");
            packet.ReadInt32E<LfgRoleFlag>("Roles");
            var slotsCount = packet.ReadInt32();

            for (var i = 0; i < 3; ++i) // Needs
                packet.ReadUInt32("Need", i);

            packet.ReadWoWString("Comment", commentLength);

            for (var i = 0; i < slotsCount; ++i) // Slots
                packet.ReadUInt32("Slot", i);
        }

        public static void ReadLFGRoleCheckUpdateMember(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("Guid", idx);
            packet.ReadUInt32E<LfgRoleFlag>("RolesDesired", idx);
            packet.ReadByte("Level", idx);
            packet.ReadBit("RoleCheckComplete", idx);

            packet.ResetBitReader();
        }

        [Parser(Opcode.SMSG_LFG_LIST_SEARCH_RESULTS)]
        public static void HandleLfgListSearchResults(Packet packet)
        {
            packet.ReadInt16("unk16");
            var count = packet.ReadInt32("Count");
            for (var i = 0; i < count; ++i)
                sub_650E43(packet, i);
        }

        [Parser(Opcode.SMSG_LFG_ROLE_CHECK_UPDATE)]
        public static void HandleLfgRoleCheck(Packet packet)
        {
            packet.ReadByte("PartyIndex");
            packet.ReadByteE<LfgRoleCheckStatus>("RoleCheckStatus");
            var joinSlotsCount = packet.ReadInt32("JoinSlotsCount");
            packet.ReadUInt64("BgQueueID");
            packet.ReadInt32("ActivityID"); // NC
            var membersCount = packet.ReadInt32("MembersCount");

            for (var i = 0; i < joinSlotsCount; ++i) // JoinSlots
                packet.ReadUInt32("JoinSlot", i);

            for (var i = 0; i < membersCount; ++i) // Members
                ReadLFGRoleCheckUpdateMember(packet, i);

            packet.ReadBit("IsBeginning");
            packet.ReadBit("ShowRoleCheck"); // NC
        }

        [Parser(Opcode.SMSG_ROLE_CHOSEN)]
        public static void HandleRoleChosen(Packet packet)
        {
            packet.ReadPackedGuid128("Player");
            packet.ReadUInt32E<LfgRoleFlag>("RoleMask");
            packet.ReadBit("Accepted");
        }

        [Parser(Opcode.SMSG_LFG_PARTY_INFO)]
        public static void HandleLfgPartyInfo(Packet packet)
        {
            var blackListCount = packet.ReadInt32("BlackListCount");
            for (var i = 0; i < blackListCount; i++)
                ReadLFGBlackList(packet, i);
        }

        [Parser(Opcode.SMSG_LFG_BOOT_PLAYER)]
        public static void HandleLfgBootPlayer(Packet packet)
        {
            ReadLfgBootInfo(packet);
        }

        [Parser(Opcode.CMSG_DF_BOOT_PLAYER_VOTE)]
        public static void HandleDFBootPlayerVote(Packet packet)
        {
            packet.ReadBit("Vote");
        }

        [Parser(Opcode.CMSG_DF_PROPOSAL_RESPONSE)]
        public static void HandleDFProposalResponse(Packet packet)
        {
            ReadCliRideTicket(packet);
            packet.ReadInt64("InstanceID");
            packet.ReadInt32("ProposalID");
            packet.ReadBit("Accepted");
        }

        [Parser(Opcode.SMSG_LFG_LIST_UPDATE_BLACKLIST)]
        public static void HandleLFGListUpdateBlacklist(Packet packet)
        {
            var count = packet.ReadInt32("BlacklistEntryCount");
            for (int i = 0; i < count; i++)
                ReadLFGListBlacklistEntry(packet, i, "ListBlacklistEntry");
        }

        [Parser(Opcode.SMSG_LFG_LIST_UPDATE_STATUS)]
        public static void HandleLFGListUpdateStatus(Packet packet)
        {
            ReadCliRideTicket(packet, "RideTicket");
            ReadLFGListJoinRequest(packet, "LFGListJoinRequest");
            packet.ReadInt32("Unk");
            packet.ReadByte("Reason");

            packet.ResetBitReader();

            packet.ReadBit("Listed");
        }

        [Parser(Opcode.SMSG_LFG_TELEPORT_DENIED)]
        public static void HandleLFGTeleportDenied(Packet packet)
        {
            packet.ReadBits("Reason", 4);
        }

        [Parser(Opcode.CMSG_LFG_LIST_INVITE_RESPONSE)]
        public static void HandleLFGListInviteResponse(Packet packet)
        {
            ReadCliRideTicket(packet, "RideTicket");

            packet.ResetBitReader();
            packet.ReadBit("Accept");
        }

        [Parser(Opcode.CMSG_DF_TELEPORT)]
        public static void HandleDFTeleport(Packet packet)
        {
            packet.ReadBit("TeleportOut");
        }

        [Parser(Opcode.CMSG_DF_SET_ROLES)]
        public static void HandleDFSetRoles(Packet packet)
        {
            packet.ReadUInt32("RolesDesired");
            packet.ReadByte("PartyIndex");
        }

        [Parser(Opcode.CMSG_DF_LEAVE)]
        public static void HandleDFLeave(Packet packet)
        {
            ReadCliRideTicket(packet, "RideTicket");
        }

        [Parser(Opcode.CMSG_LFG_LIST_JOIN)]
        public static void HandleLFGListJoin(Packet packet)
        {
            ReadLFGListJoinRequest(packet, "LFGListJoinRequest");
        }

        [Parser(Opcode.CMSG_LFG_LIST_LEAVE)]
        public static void HandleLFGListLeave(Packet packet)
        {
            ReadCliRideTicket(packet, "RideTicket");
        }

        [Parser(Opcode.CMSG_LFG_LIST_SEARCH)] // To-Do: Rename Unks
        public static void HandleLFGListSearch(Packet packet)
        {
            var len = packet.ReadBits(6);

            packet.ReadInt32("Int64");
            packet.ReadInt32("Int68");
            packet.ReadInt32("Int72");
            packet.ReadInt32("Int76");
            var int80 = packet.ReadInt32("BlacklistEntryCount");
            var int96 = packet.ReadInt32("int96");

            packet.ReadWoWString("String", len);

            for (int i = 0; i < int80; ++i)
                ReadLFGListBlacklistEntry(packet, i, "ListBlacklistEntry");

            for (int i = 0; i < int96; ++i)
                packet.ReadPackedGuid128("SmartGuid96", i); // PartyMember?
        }

        [Parser(Opcode.CMSG_SET_LFG_BONUS_FACTION_ID)]
        public static void HandleSetLFGBonusFactionID(Packet packet)
        {
            packet.ReadInt32("FactionID");
        }

        [Parser(Opcode.SMSG_LFG_LIST_SEARCH_STATUS)]
        public static void HandleLfgListSearchStatus(Packet packet)
        {
            ReadCliRideTicket(packet);
            packet.ReadByte("unk48");
            packet.ReadBit("unk49");
        }

        [Parser(Opcode.SMSG_SOCIAL_QUEUE_UPDATE)]
        public static void HandleSocialQueueUpdate(Packet packet)
        {
            packet.ReadInt64("unk64");
            packet.ReadPackedGuid128("Player");
            {//64FE42 22996 650CB9 23420
                packet.ReadPackedGuid128("Party");
                var cnt = packet.ReadInt32("Cnt");
                packet.ResetBitReader();
                packet.ReadBit("unkb");

                for (var i = 0; i < cnt; ++i)
                {//64FEB8 22996 650D2F 23420
                    packet.ResetBitReader();
                    var v6 = packet.ReadBits("unkbc", 2, i);
                    if (v6 == 2)
                    {//64FD9E 22996
                        packet.ReadInt32("unk");
                        packet.ReadInt32("unk");
                        packet.ReadInt32("unk");
                        packet.ReadByte("unk");
                        packet.ReadInt32("unk");
                        packet.ResetBitReader();
                        packet.ReadBit("unk");
                        packet.ReadBit("unk");
                    }
                    if (v6 == 1)
                        sub_650E43(packet);
                    if (v6 == 0)
                    {//64F765 22996
                        packet.ReadInt32("LFG Slot", i);
                        var cnt2 = packet.ReadInt32("cnt2", i);
                        for (var j = 0; j < cnt2; ++j)
                            packet.ReadByte("unkb", i, j);
                        packet.ResetBitReader();
                        packet.ReadBit("unkb1", i);
                        packet.ReadBit("unkb2", i);
                    }
                }
            }
        }

        [Parser(Opcode.SMSG_LFG_UNK_2A2C)]
        public static void HandleLFGUnk2A2C(Packet packet)
        {
            var cnt = packet.ReadInt32("count");
            for (var i = 0; i < cnt; ++i)
            {//651043 22996
                ReadCliRideTicket(packet, i);
                packet.ReadInt32("unk32", i);
                var cnt1 = packet.ReadInt32("cnt1", i);
                for (var j = 0; j < cnt1; ++j)
                {//679F95 22996
                    packet.ReadByte("unkb1", i, j);
                    packet.ReadByte("unkb2", i, j);
                }

                packet.ResetBitReader();
                var hasUnk56 = packet.ReadBit("unk56", i);
                var hasUnk68 = packet.ReadBit("unk68", i);
                var hasUnk76 = packet.ReadBit("unk76", i);
                packet.ReadBit("unk96", i);
                var hasGuid120 = packet.ReadBit("unk120", i);
                var hasGuid144 = packet.ReadBit("unk144", i);
                var hasGuid168 = packet.ReadBit("unk168", i);
                packet.ReadBit("unk1472", i);
                packet.ReadBit("unk1473", i);
                packet.ReadBit("unk1474", i);

                //650D44
                {
                    packet.ReadInt32("unk1", i);
                    packet.ReadInt32("unk2", i);
                    packet.ReadInt32("unk3", i);
                    packet.ResetBitReader();
                    var len1 = packet.ReadBits(8);
                    var len2 = packet.ReadBits(11);
                    var len3 = packet.ReadBits(8);
                    packet.ReadBit("unk4", i);
                    packet.ReadWoWString("str1", len1, i);
                    packet.ReadWoWString("str2", len2, i);
                    packet.ReadWoWString("str3", len3, i);
                }

                if (hasUnk56)
                    packet.ReadPackedGuid128("guid", i);
                if (hasUnk68)
                    packet.ReadInt32("unk64", i);
                if (hasUnk76)
                    packet.ReadInt32("unk72", i);
                if (hasGuid120)
                    packet.ReadPackedGuid128("guid104", i);
                if (hasGuid144)
                    packet.ReadPackedGuid128("guid128", i);
                if (hasGuid168)
                    packet.ReadPackedGuid128("guid152", i);
            }
        }

        [Parser(Opcode.SMSG_LFG_UNK_2A28)]
        public static void HandleLFGUnk2A28(Packet packet)
        {
            ReadCliRideTicket(packet);
            ReadCliRideTicket(packet);
            packet.ReadInt32("unk20");
            packet.ReadByte("unk84");
            packet.ReadByte("unk92");
            packet.ResetBitReader();
            packet.ReadBits("unk88", 4);
        }

        [Parser(Opcode.SMSG_LFG_UNK_2A29)]
        public static void HandleLFGUnk2A29(Packet packet)
        {
            ReadCliRideTicket(packet);
            ReadCliRideTicket(packet);
            sub_650E43(packet);
            packet.ReadInt32("unk20");
            packet.ReadByte("unk84");
            packet.ReadByte("unk92");
            packet.ResetBitReader();
            packet.ReadBits("unk88", 4);
        }
    }
}
