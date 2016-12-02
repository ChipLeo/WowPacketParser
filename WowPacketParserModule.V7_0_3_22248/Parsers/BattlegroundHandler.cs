using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class BattlegroundHandler
    {
        [Parser(Opcode.CMSG_BATTLEMASTER_JOIN_ARENA)]
        public static void HandleBattlemasterJoinArena(Packet packet)
        {
            packet.ReadByte("TeamSizeIndex");
            packet.ReadByteE<LfgRoleFlag>("Roles");
        }

        public static void ReadPlayerData(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("PlayerGUID", idx);
            packet.ReadUInt32("Kills", idx);
            packet.ReadUInt32("DamageDone", idx);
            packet.ReadUInt32("HealingDone", idx);
            var statsCount = packet.ReadUInt32("StatsCount", idx);
            packet.ReadInt32("PrimaryTalentTree", idx);
            packet.ReadInt32("PrimaryTalentTreeNameIndex", idx);
            packet.ReadInt32E<Race>("Race", idx);
            packet.ReadUInt32("Prestige", idx);

            for (int i = 0; i < statsCount; i++)
                packet.ReadUInt32("Stats", i, idx);

            packet.ResetBitReader();

            packet.ReadBit("Faction", idx);
            packet.ReadBit("IsInWorld", idx);

            var hasHonor = packet.ReadBit("HasHonor", idx);
            var hasPreMatchRating = packet.ReadBit("HasPreMatchRating", idx);
            var hasRatingChange = packet.ReadBit("HasRatingChange", idx);
            var hasPreMatchMMR = packet.ReadBit("HasPreMatchMMR", idx);
            var hasMmrChange = packet.ReadBit("HasMmrChange", idx);

            packet.ResetBitReader();

            if (hasHonor)
                V6_0_2_19033.Parsers.BattlegroundHandler.ReadHonorData(packet, "Honor");

            if (hasPreMatchRating)
                packet.ReadUInt32("PreMatchRating", idx);

            if (hasRatingChange)
                packet.ReadUInt32("RatingChange", idx);

            if (hasPreMatchMMR)
                packet.ReadUInt32("PreMatchMMR", idx);

            if (hasMmrChange)
                packet.ReadUInt32("MmrChange", idx);
        }

        [Parser(Opcode.SMSG_PVP_LOG_DATA)]
        public static void HandlePvPLogData(Packet packet)
        {
            var hasRatings = packet.ReadBit("HasRatings");
            var hasWinner = packet.ReadBit("HasWinner");

            var playersCount = packet.ReadUInt32("PlayersCount");

            for (int i = 0; i < 2; i++)
                packet.ReadByte("PlayerCount", i);

            if (hasRatings)
                V6_0_2_19033.Parsers.BattlegroundHandler.ReadRatingData(packet, "Ratings");

            if (hasWinner)
                packet.ReadByte("Winner");

            for (int i = 0; i < playersCount; i++)
                ReadPlayerData(packet, "Players", i);
        }

        [Parser(Opcode.SMSG_RATED_BATTLEFIELD_INFO)]
        public static void HandleRatedBattlefieldInfo(Packet packet)
        {
            for (int i = 0; i < 6; i++)
            {
                packet.ReadInt32("unk1", i);
                packet.ReadInt32("unk2", i);
                packet.ReadInt32("unk3", i);
                packet.ReadInt32("unk4", i);
                packet.ReadInt32("unk5", i);
                packet.ReadInt32("unk6", i);
                packet.ReadInt32("unk7", i);
                packet.ReadInt32("unk8", i);
                packet.ReadInt32("unk9", i);
                packet.ReadInt32("unk10", i);
                packet.ReadInt32("unk11", i);
            }
        }

        [Parser(Opcode.SMSG_BATTLE_PAY_VAS_PURCHASE_LIST)]
        public static void HandleBattlePayWasPurchaseList(Packet packet)
        {
            var cnt = packet.ReadBits("Count", 6);
            for (int i = 0; i < cnt; i++)
            {
                packet.ReadPackedGuid128("Guid", i);
                packet.ReadInt32("unk1", i);
                packet.ReadInt32("unk2", i);
                packet.ResetBitReader();
                var cnt2 = packet.ReadBits("cnt2", i);
                for (int j = 0; j < cnt2; ++j)
                    packet.ReadInt32("unk3", j, i);
            }
        }
    }
}
