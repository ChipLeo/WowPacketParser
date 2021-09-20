using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class TokenHandler
    {
        [Parser(Opcode.SMSG_UPDATE_WOW_TOKEN_COUNT_RESPONSE)]
        public static void HandleTokenUpdateTokenCountResponse(Packet packet)
        {
            packet.ReadUInt32("UnkInt"); // send CMSG_UPDATE_WOW_TOKEN_COUNT

            packet.ReadUInt32("UnkInt2");

            var count1 = packet.ReadInt32("DistributionCount1");
            var count2 = packet.ReadInt32("DistributionCount2");

            for (int i = 0; i < count1; i++)
                packet.ReadInt64("DistributionID", i);

            for (int i = 0; i < count2; i++)
                packet.ReadInt64("DistributionID", i);
        }

        [Parser(Opcode.CMSG_GET_REMAINING_GAME_TIME)]
        public static void HandleGetRemainingGameTime(Packet packet)
        {
            packet.ReadUInt32("UnkInt32");
        }

        [Parser(Opcode.SMSG_TOKEN_UNK1)]
        public static void HandleTokenUnk1(Packet packet)
        {
            var count1 = packet.ReadInt32("DistributionCount1");
            var count2 = packet.ReadInt32("DistributionCount2");

            for (int i = 0; i < count1; i++)
                packet.ReadInt64("DistributionID", i);

            for (int i = 0; i < count2; i++)
                packet.ReadInt64("DistributionID", i);
        }

        [Parser(Opcode.SMSG_WOW_TOKEN_VETERAN_ELIGIBILITY_RESPONSE)]
        public static void HandleTokenVeteranEligibilityResponse(Packet packet)
        {
            packet.ReadInt64("UnkInt64");
            packet.ReadInt32("UnkInt32");
            packet.ReadInt32("UnkInt32 2");
        }
    }
}
