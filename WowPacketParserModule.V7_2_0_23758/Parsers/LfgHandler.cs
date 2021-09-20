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
            packet.ReadInt32("GroupFinderActivityId", idx);
            packet.ReadSingle("RequiredItemLevel", idx);
            packet.ReadInt32("RequiredHonorLevel", idx);

            packet.ResetBitReader();

            var lenName = packet.ReadBits(8);
            var lenComment = packet.ReadBits(11);
            var lenVoiceChat = packet.ReadBits(8);
            var hasQuest = false;
            packet.ReadBit("AutoAccept", idx);
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_1_5_23360))
            {
                packet.ReadBit("IsPrivate", idx);
                hasQuest = packet.ReadBit("HasQuest", idx);
            }

            packet.ReadWoWString("Name", lenName, idx);
            packet.ReadWoWString("Comment", lenComment, idx);
            packet.ReadWoWString("VoiceChat", lenVoiceChat, idx);

            if (hasQuest)
                packet.ReadInt32("QuestID", idx);
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

        [Parser(Opcode.CMSG_LFG_LIST_GET_STATUS)]
        [Parser(Opcode.CMSG_REQUEST_LFG_LIST_BLACKLIST)]
        public static void HandleLfgZero(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_LFG_LIST_ACTIVE_ENTRY)]
        public static void HandleLfgListActiveEntry(Packet packet)
        {
            ReadCliRideTicket(packet, "RideTicket");
            packet.ReadInt32("unk48");
            packet.ReadByte("unk52");
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

        public static void ReadLfgListSearchResultMemberInfo(Packet packet, params object[] idx)
        {
            packet.ReadByteE<Class>("Class", idx);
            packet.ReadByteE<LfgRole>("Role", idx);
        }

        public static void ReadLfgListSearchResult(Packet packet, params object[] idx)
        {//650E43 22996 651B56 23420
            ReadCliRideTicket(packet, idx, "LFGListRideTicket");

            packet.ReadUInt32("SequenceNum", idx);

            packet.ReadPackedGuid128("Leader", idx);
            packet.ReadPackedGuid128("LastTouchedAny", idx);
            packet.ReadPackedGuid128("LastTouchedName", idx);
            packet.ReadPackedGuid128("LastTouchedComment", idx);
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_3_0_24920))
                packet.ReadPackedGuid128("LastTouchedVoiceChat", idx);

            packet.ReadUInt32("VirtualRealmAddress", idx);

            var bnetFriendCount = packet.ReadUInt32();
            var characterFriendCount = packet.ReadUInt32();
            var guildMateCount = packet.ReadUInt32();
            var memberCount = packet.ReadUInt32();

            packet.ReadUInt32("CompletedEncountersMask", idx);
            packet.ReadTime("CreationTime", idx);
            packet.ReadByte("Unk4", idx);

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_3_5_25848))
                packet.ReadPackedGuid128("PartyGUID", idx);

            for (int i = 0; i < bnetFriendCount; i++)
                packet.ReadPackedGuid128("BNetFriends", idx, i);
            for (int i = 0; i < characterFriendCount; i++)
                packet.ReadPackedGuid128("CharacterFriends", idx, i);
            for (int i = 0; i < guildMateCount; i++)
                packet.ReadPackedGuid128("GuildMates", idx, i);

            for (int i = 0; i < memberCount; i++)
                ReadLfgListSearchResultMemberInfo(packet, "Members", idx, i);   //679F95 22996

            ReadLFGListJoinRequest(packet, idx, "LFGListJoinRequest");          //650D44 22996
        }

        [Parser(Opcode.SMSG_LFG_PROPOSAL_UPDATE)]
        public static void HandleLfgProposalUpdate(Packet packet)
        {
            ReadCliRideTicket(packet);

            packet.ReadUInt64("InstanceID");
            packet.ReadUInt32("ProposalID");
            packet.ReadUInt32("Slot");
            packet.ReadSByte("State");
            packet.ReadUInt32("CompletedMask");
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_3_0_24920))
                packet.ReadUInt32("EncounterMask");

            var playerCount = packet.ReadUInt32("PlayersCount");
            packet.ReadByte();
            packet.ReadBit("ValidCompletedMask");
            packet.ReadBit("ProposalSilent");
            packet.ReadBit("IsRequeue");

            for (var i = 0u; i < playerCount; i++)
            {
                packet.ReadUInt32("Roles", i);

                packet.ResetBitReader();

                packet.ReadBit("Me", i);
                packet.ReadBit("SameParty", i);
                packet.ReadBit("MyParty", i);
                packet.ReadBit("Responded", i);
                packet.ReadBit("Accepted", i);
            }
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
            uint commentLength = 0;
            if (ClientVersion.RemovedInVersion(ClientVersionBuild.V7_1_5_23360))
                commentLength = packet.ReadBits("UnkBits8", 8);
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_1_5_23360))
                packet.ReadBit("unk");
            packet.ResetBitReader();

            packet.ReadByte("PartyIndex");
            packet.ReadInt32E<LfgRoleFlag>("Roles");
            var slotsCount = packet.ReadInt32();

            if (ClientVersion.RemovedInVersion(ClientVersionBuild.V7_1_5_23360))
            for (var i = 0; i < 3; ++i) // Needs
                packet.ReadUInt32("Need", i);
            if (ClientVersion.RemovedInVersion(ClientVersionBuild.V7_1_5_23360))
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
            packet.ReadTime("RemainingTime");
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

        [Parser(Opcode.CMSG_SET_LFG_BONUS_FACTION_ID)]
        public static void HandleSetLFGBonusFactionID(Packet packet)
        {
            packet.ReadInt32("FactionID");
        }

        [Parser(Opcode.SMSG_LFG_LIST_APPLICANT)]
        public static void HandleLfgListApplicant(Packet packet)
        {
            ReadCliRideTicket(packet,"Ticket");
            var count = packet.ReadInt32();
            packet.ReadInt32("unk1");
            for (int i = 0; i < count; i++)
            {
                ReadCliRideTicket(packet, i, "Tickets");
                packet.ReadPackedGuid128("Player", i);
                var cnt = packet.ReadInt32("cnt2", i);
                for (int j = 0; j < cnt; j++)
                {
                    packet.ReadPackedGuid128("guid", i, j);
                    packet.ReadInt32("VirtualRealmAddress", i, j);
                    packet.ReadInt32("unk4", i, j);
                    packet.ReadInt32("Level", i, j);
                    packet.ReadInt32("unk6", i, j);
                    packet.ReadByte("unk7", i, j);
                    packet.ReadByte("unk8", i, j);
                    {
                        var cnt2 = packet.ReadInt32("cnt", i, j);
                        for (var k = 0; k < cnt2; k++)
                        {
                            packet.ReadInt32("unk", i, j, k);
                            packet.ReadInt32("unk", i, j, k);
                        }
                    }
                }
                packet.ResetBitReader();
                packet.ReadBits("unkb1", 4, i);
                packet.ReadBit("unkb2", i);
                packet.ReadWoWString("Name", packet.ReadBits(8), i);
            }
        }

        [Parser(Opcode.SMSG_LFG_LIST_SEARCH_STATUS)]
        public static void HandleLfgListSearchStatus(Packet packet)
        {
            ReadCliRideTicket(packet);
            packet.ReadByte("unk48");

            packet.ResetBitReader();
            packet.ReadBit("unk49");
        }

        [Parser(Opcode.SMSG_SOCIAL_QUEUE_UPDATE)]
        public static void HandleSocialQueueUpdate(Packet packet)
        {
            packet.ReadInt64("unk64");
            packet.ReadPackedGuid128("Player");
            {//64FE42:22996 650CB9:23420 65011E:23360
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
                        ReadLfgListSearchResult(packet);
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
            ReadLfgListSearchResult(packet);
            packet.ReadInt32("unk20");
            packet.ReadByte("unk84");
            packet.ReadByte("unk92");
            packet.ResetBitReader();
            packet.ReadBits("unk88", 4);
        }

        [Parser(Opcode.CMSG_QUICK_JOIN_AUTO_ACCEPT_REQUESTS)]
        public static void HandleLfgQuickJoinAutoAcceptRequests(Packet packet)
        {
            packet.ReadBit("AutoAccept");
        }

     /*   [Parser(Opcode.SMSG_LFG_LIST_UPDATE_STATUS)]
        public static void HandleLfgListUpdateStatus(Packet packet)
        {
            ReadCliRideTicket(packet, "RideTicket");
            packet.ReadTime("RemainingTime");
            packet.ReadByte("ResultId");
            ReadLFGListJoinRequest(packet, "LFGListJoinRequest");
            packet.ResetBitReader();
            packet.ReadBit("Listed");
        }*/

        [Parser(Opcode.CMSG_LFG_LIST_UPDATE_REQUEST)]
        public static void HandleLfgListUpdateRequest(Packet packet)
        {
            ReadCliRideTicket(packet, "RideTicket");
            ReadLFGListJoinRequest(packet, "LFGListJoinRequest");
        }

        [Parser(Opcode.SMSG_LFG_LIST_JOIN_RESULT)]
        public static void HandleLfgListJoinResult(Packet packet)
        {
            ReadCliRideTicket(packet, "RideTicket");
            packet.ReadByte("ResultId");
            packet.ReadByte("Unk"); // unused?
        }

        [Parser(Opcode.CMSG_LFG_LIST_DECLINE_APPLICANT)]
        public static void HandleLfgListDeclineApplicant(Packet packet)
        {
            ReadCliRideTicket(packet, "LFGListRideTicket");
            ReadCliRideTicket(packet, "ApplicationRideTicket");
        }

        [Parser(Opcode.SMSG_LFG_LIST_APPLY_TO_GROUP_RESULT)]
        public static void HandleLfgApplyForGroupResult(Packet packet)
        {
            ReadCliRideTicket(packet, "ApplicationRideTicket");
            ReadCliRideTicket(packet, "LFGListRideTicket");

            packet.ReadTime("RemainingTimeoutTime");
            packet.ReadByte("ResultId");
            packet.ReadByte("Unk1"); // always 0
            ReadLfgListSearchResult(packet, "LFGListEntry");
            packet.ReadBitsE<LfgListApplicationStatus>("Status", 4);
        }

        [Parser(Opcode.CMSG_LFG_LIST_APPLY_TO_GROUP)]
        public static void HandleLFGListApplyToGroup(Packet packet)
        {//063AC1E 22996
            //652C07 22996
            ReadCliRideTicket(packet, "RideTicket");
            packet.ReadInt32("GroupFinderActivityId");
            packet.ReadByteE<LfgRoleFlag>("Roles");
            packet.ResetBitReader();
            var len = packet.ReadBits(8);
            packet.ReadWoWString("Comment", len);
        }

        [Parser(Opcode.CMSG_LFG_LIST_INVITE_APPLICANT)]
        public static void HandleLfgListInviteApplicant(Packet packet)
        {
            ReadCliRideTicket(packet, "LFGListRideTicket");
            ReadCliRideTicket(packet, "ApplicationRideTicket");
            var memberNum = packet.ReadUInt32("PartyMemberNum");

            for (int i = 0; i < memberNum; i++)
            {
                packet.ReadPackedGuid128("PlayerGUID", i);
                packet.ReadByteE<LfgRoleFlag>("ChosenRoles", i);
            }
        }

        [Parser(Opcode.SMSG_LFG_LIST_APPLICATION_STATUS_UPDATE)]
        public static void HandleLfgListApplicationUpdate(Packet packet)
        {
            ReadCliRideTicket(packet, "ApplicationRideTicket");
            ReadCliRideTicket(packet, "LFGListRideTicket");

            packet.ReadInt32("Unk");
            packet.ReadByte("ResultId");
            packet.ReadByteE<LfgRoleFlag>("Role");
            packet.ReadBitsE<LfgListApplicationStatus>("Status", 4);
        }

        [Parser(Opcode.SMSG_LFG_LIST_SEARCH_RESULTS)]
        public static void HandleLfgListSearchResults(Packet packet)
        {
            packet.ReadUInt16("TotalResults");
            var resultCount = packet.ReadUInt32();

            for (int j = 0; j < resultCount; j++)
                ReadLfgListSearchResult(packet, "Entry", j);
        }

        [Parser(Opcode.CMSG_LFG_LIST_SEARCH)]
        public static void HandleLFGListSearch(Packet packet)
        {
            uint searchFilter = 0;
            packet.ResetBitReader();
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_2_5_24330))
                searchFilter = packet.ReadBits("SearchFilterNum", 5);
            else
                searchFilter = packet.ReadBits("SearchFilterNum", 6);

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_2_5_24330))
            {
                uint[] length = new uint[4];
                for (int i = 0; i < searchFilter; i++)
                {
                    packet.ResetBitReader();
                    for (int z = 0; z < 3; z++)
                        length[z] = packet.ReadBits("SearchFilterLength", 5, i, z);
                    for (int z = 0; z < 3; z++)
                        packet.ReadWoWString("SearchFilter", length[z], i, z);
                }
            }
            packet.ReadInt32("GroupFinderCategoryId");
            packet.ReadInt32("SubActivityGroupID");
            packet.ReadInt32E<LfgListFilter>("LFGListFilter");
            packet.ReadUInt32E<LocaleConstantFlags>("LanguageFilter"); // 0x6b3 is default in enUS client (= enUS, koKR, ptBR, none, zhCN, zhTW, esMX)

            var entryCount = packet.ReadInt32();
            var guidCount = packet.ReadInt32();

            if (ClientVersion.RemovedInVersion(ClientVersionBuild.V7_2_5_24330))
                packet.ReadWoWString("SearchFilter", searchFilter);

            for (int i = 0; i < entryCount; i++)
            {
                if (ClientVersion.RemovedInVersion(ClientVersionBuild.V7_2_5_24330))
                {
                    packet.ReadInt32("GroupFinderActivityId");
                    packet.ReadInt32E<LfgLockStatus>("LockStatus");
                }
                else
                {
                    packet.ReadInt32("Unk");
                }
            }

            for (int i = 0; i < guidCount; i++)
                packet.ReadPackedGuid128("UnkGUID", i); // PartyMember?
        }

        public static void ReadLfgListSearchResultPartialUpdate(Packet packet, params object[] idx)
        {
            ReadCliRideTicket(packet, idx, "Ticket");
            packet.ReadUInt32("SequenceNum", idx);
            var memberCount = packet.ReadUInt32();
            for (int j = 0; j < memberCount; j++)
                ReadLfgListSearchResultMemberInfo(packet, "Members", idx, j);

            packet.ResetBitReader();
            var hasLeader = packet.ReadBit("ChangeLeader", idx);
            var hasVirtualRealmAddress = packet.ReadBit("ChangeVirtualRealmAddress", idx);
            var hasCompletedEncountersMask = packet.ReadBit("ChangeCompletedEncountersMask", idx);
            packet.ReadBit("Delisted", idx);
            packet.ReadBit("ChangeTitle", idx);
            var hasAny = packet.ReadBit();
            var hasName = packet.ReadBit("ChangeName", idx);
            var hasComment = packet.ReadBit("ChangeComment", idx);
            var hasVoice = false;
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_3_0_24920))
                hasVoice = packet.ReadBit("ChangeVoice", idx);
            var hasItemLevel = packet.ReadBit("ChangeItemLevel", idx);
            packet.ReadBit("ChangeAutoAccept", idx);
            packet.ReadBit("ChangeHonorLevel", idx);
            packet.ReadBit("ChangePrivate", idx);

            ReadLFGListJoinRequest(packet, idx, "LFGListJoinRequest");

            if (hasLeader)
                packet.ReadPackedGuid128("Leader", idx);

            if (hasVirtualRealmAddress)
                packet.ReadUInt32("VirtualRealmAddress", idx);

            if (hasCompletedEncountersMask)
                packet.ReadUInt32("CompletedEncountersMask", idx);

            if (hasAny)
                packet.ReadPackedGuid128("LastTouchedAny");

            if (hasName)
                packet.ReadPackedGuid128("LastTouchedName", idx);

            if (hasComment)
                packet.ReadPackedGuid128("LastTouchedComment", idx);

            if (hasVoice)
                packet.ReadPackedGuid128("LastTouchedVoiceChat", idx);
        }

        [Parser(Opcode.SMSG_LFG_LIST_SEARCH_RESULTS_UPDATE)]
        public static void HandleLfgListSearchResultsUpdate(Packet packet)
        {
            var count = packet.ReadUInt32();

            for (int i = 0; i < count; i++)
                ReadLfgListSearchResultPartialUpdate(packet, "LFGListSearchResultPartialUpdate", i);
        }

        [Parser(Opcode.SMSG_LFG_LIST_APPLICANT_LIST_UPDATE)]
        public static void HandleLfgListApplicantListUpdate(Packet packet)
        {
            ReadCliRideTicket(packet, "Ticket");
            var applicantCount = packet.ReadUInt32();
            packet.ReadUInt32("Result");

            for (int i = 0; i < applicantCount; i++)
            {
                ReadCliRideTicket(packet, i, "Ticket");
                packet.ReadPackedGuid128("Joiner", i);
                var memberCount = packet.ReadUInt32();
                for (int j = 0; j < memberCount; j++)
                {
                    packet.ReadPackedGuid128("Guid", i, j);
                    packet.ReadUInt32("VirtualRealmAddress", i, j);
                    packet.ReadSingle("ItemLevel", i, j);
                    packet.ReadUInt32("Level", i, j);
                    packet.ReadInt32("HonorLevel", i, j);
                    packet.ReadByteE<LfgRoleFlag>("Queued role", i, j);
                    packet.ReadByteE<LfgRoleFlag>("Assigned role", i, j);

                    var provingGroundRankNum = packet.ReadUInt32();
                    for (int x = 0; x < provingGroundRankNum; x++)
                    {
                        packet.ReadUInt32("CriteriaID", i, j, x);
                        packet.ReadUInt32("Quantity", i, j, x);
                    }
                }

                packet.ResetBitReader();
                packet.ReadBitsE<LfgListApplicationStatus>("Status", 4, i);
                packet.ReadBit("Listed", i);
                var len = packet.ReadBits(8);

                packet.ReadWoWString("Comment", len);
            }
        }
    }
}