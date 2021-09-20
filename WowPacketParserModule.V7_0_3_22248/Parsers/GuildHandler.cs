using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using RaceMask7 = WowPacketParserModule.V7_0_3_22248.Enums.RaceMask;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class GuildHandler
    {

        [Parser(Opcode.CMSG_GUILD_GET_ACHIEVEMENT_MEMBERS)]
        public static void HandleGuildGetAchievementMembers(Packet packet)
        {
            packet.ReadPackedGuid128("Player");
            packet.ReadPackedGuid128("Guild");
            packet.ReadInt32("Achievement");
        }

        [Parser(Opcode.SMSG_GUILD_ACHIEVEMENT_MEMBERS)]
        public static void HandleGuildAchievementMembersResponse(Packet packet)
        {
            packet.ReadPackedGuid128("Guild");
            packet.ReadInt32("Achievement");
            var cnt = packet.ReadInt32("Count");
            for (var i = 0; i < cnt; i++)
                packet.ReadPackedGuid128("Player", i);
        }

        [Parser(Opcode.SMSG_GUILD_BANK_LOG_QUERY_RESULTS)]
        public static void HandleGuildBankLogQueryResult(Packet packet)
        {
            packet.ReadInt32("Tab");
            var int32 = packet.ReadInt32("GuildBankLogEntryCount");
            packet.ResetBitReader();
            var bit24 = packet.ReadBit("HasWeeklyBonusMoney");

            for (int i = 0; i < int32; i++)
            {
                packet.ReadPackedGuid128("PlayerGUID", i);
                packet.ReadInt32("TimeOffset", i);
                packet.ReadSByte("EntryType", i);

                packet.ResetBitReader();

                var bit33 = packet.ReadBit("HasMoney", i);
                var bit44 = packet.ReadBit("HasItemID", i);
                var bit52 = packet.ReadBit("HasCount", i);
                var bit57 = packet.ReadBit("HasOtherTab", i);

                if (bit33)
                    packet.ReadInt64("Money", i);

                if (bit44)
                    packet.ReadInt32<ItemId>("ItemID", i);

                if (bit52)
                    packet.ReadInt32("Count", i);

                if (bit57)
                    packet.ReadSByte("OtherTab", i);

            }

            if (bit24)
                packet.ReadInt64("WeeklyBonusMoney");
        }

        [Parser(Opcode.SMSG_GUILD_CRITERIA_UPDATE)]
        public static void HandleGuildCriteriaUpdate(Packet packet)
        {
            var int16 = packet.ReadUInt32("ProgressCount");
            for (int i = 0; i < int16; i++)
            {
                packet.ReadInt32("CriteriaID", i);
                packet.ReadTime("DateCreated", i);
                packet.ReadTime("DateStarted", i);
                packet.ReadTime("DateUpdated", i);
                packet.ReadInt64("Quantity", i);
                packet.ReadPackedGuid128("PlayerGUID", i);

                packet.ReadInt32("Flags", i);
            }
        }
        [Parser(Opcode.SMSG_QUERY_GUILD_INFO_RESPONSE)]
        public static void HandleGuildQueryResponse(Packet packet)
        {
            packet.ReadPackedGuid128("Guild Guid");

            var hasData = packet.ReadBit();
            if (hasData)
            {
                packet.ReadPackedGuid128("GuildGUID");
                packet.ReadInt32("VirtualRealmAddress");
                var rankCount = packet.ReadInt32("RankCount");
                packet.ReadInt32("EmblemColor");
                packet.ReadInt32("EmblemStyle");
                packet.ReadInt32("BorderColor");
                packet.ReadInt32("BorderStyle");
                packet.ReadInt32("BackgroundColor");

                packet.ResetBitReader();
                var nameLen = packet.ReadBits(7);

                for (var i = 0; i < rankCount; i++)
                {
                    packet.ReadInt32("RankID", i);
                    packet.ReadInt32("RankOrder", i);

                    packet.ResetBitReader();
                    var rankNameLen = packet.ReadBits(7);
                    packet.ReadWoWString("Rank Name", rankNameLen, i);
                }

                packet.ReadWoWString("Guild Name", nameLen);
            }
        }

        [Parser(Opcode.SMSG_GUILD_ROSTER)]
        public static void HandleGuildRoster(Packet packet)
        {
            packet.ReadUInt32("NumAccounts");
            packet.ReadPackedTime("CreateDate");
            packet.ReadUInt32("GuildFlags");
            var int20 = packet.ReadUInt32("MemberDataCount");

            packet.ResetBitReader();
            var bits2037 = packet.ReadBits(10);
            var bits9 = packet.ReadBits(11);

            for (var i = 0; i < int20; ++i)
            {
                packet.ReadPackedGuid128("Guid", i);

                packet.ReadUInt32("RankID", i);
                packet.ReadUInt32<AreaId>("AreaID", i);
                packet.ReadUInt32("PersonalAchievementPoints", i);
                packet.ReadUInt32("GuildReputation", i);

                packet.ReadSingle("LastSave", i);

                for (var j = 0; j < 2; ++j)
                {
                    packet.ReadUInt32("DbID", i, j);
                    packet.ReadUInt32("Rank", i, j);
                    packet.ReadUInt32("Step", i, j);
                }

                packet.ReadUInt32("VirtualRealmAddress", i);

                packet.ReadByteE<GuildMemberFlag>("Status", i);
                packet.ReadByte("Level", i);
                packet.ReadByteE<Class>("ClassID", i);
                packet.ReadByteE<Gender>("Gender", i);

                packet.ResetBitReader();

                var bits36 = packet.ReadBits(6);
                var bits92 = packet.ReadBits(8);
                var bits221 = packet.ReadBits(8);

                packet.ReadBit("Authenticated", i);
                packet.ReadBit("SorEligible", i);

                packet.ReadWoWString("Name", bits36, i);
                packet.ReadWoWString("Note", bits92, i);
                packet.ReadWoWString("OfficerNote", bits221, i);
            }

            packet.ReadWoWString("WelcomeText", bits2037);
            packet.ReadWoWString("InfoText", bits9);
        }

        [Parser(Opcode.SMSG_GUILD_BANK_QUERY_RESULTS)]
        public static void HandleGuildBankQueryResults(Packet packet)
        {
            packet.ReadUInt64("Money");
            packet.ReadInt32("Tab");
            packet.ReadInt32("WithdrawalsRemaining");

            var tabInfoCount = packet.ReadInt32("TabInfoCount");
            var itemInfoCount = packet.ReadInt32("ItemInfoCount");
            packet.ResetBitReader();
            packet.ReadBit("FullUpdate");
            packet.ResetBitReader();
            for (int i = 0; i < tabInfoCount; i++)
            {
                packet.ReadInt32("TabIndex", i);

                packet.ResetBitReader();

                var bits1 = packet.ReadBits(7);
                var bits69 = packet.ReadBits(9);

                packet.ReadWoWString("Name", bits1, i);
                packet.ReadWoWString("Icon", bits69, i);
            }

            for (int i = 0; i < itemInfoCount; i++)
            {
                packet.ReadInt32("Slot", i);

                ItemHandler.ReadItemInstance(packet, i, "ItemInstance");

                packet.ReadInt32("Count", i);
                packet.ReadInt32("EnchantmentID", i);
                packet.ReadInt32("Charges", i);
                packet.ReadInt32("OnUseEnchantmentID", i);
                packet.ReadInt32("Flags", i);

                Substructures.ItemHandler.ReadItemInstance(packet, i, "ItemInstance");
            }
        }

        [Parser(Opcode.SMSG_GUILD_ITEM_LOOTED_NOTIFY)]
        public static void HandleGuildItemLooted(Packet packet)
        {
            packet.ReadPackedGuid128("Player");
            packet.ReadInt32("VirtualRealmAddress");
            ItemHandler.ReadItemInstance(packet, "Item");
            packet.ReadWoWString("Name", packet.ReadBits(6));
        }

        [Parser(Opcode.SMSG_GUILD_REWARD_LIST)]
        public static void HandleGuildRewardsList(Packet packet)
        {
            packet.ReadTime("Version");

            var size = packet.ReadUInt32("RewardItemsCount");
            for (int i = 0; i < size; i++)
            {
                packet.ReadUInt32<ItemId>("ItemID", i);
                packet.ReadUInt32("Unk4", i);
                var achievementReqCount = packet.ReadInt32("AchievementsRequiredCount", i);
                if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_3_5_25848))
                    packet.ReadUInt64E<RaceMask7>("RaceMask", i);
                else
                    packet.ReadUInt32E<RaceMask>("RaceMask", i);
                packet.ReadInt32("MinGuildLevel", i);
                packet.ReadInt32("MinGuildRep", i);
                packet.ReadInt64("Cost", i);

                for (int j = 0; j < achievementReqCount; j++)
                    packet.ReadInt32("AchievementsRequired", i, j);
            }
        }
    }
}
