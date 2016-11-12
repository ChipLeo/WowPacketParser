using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class ChallengeModeHandler
    {
        [Parser(Opcode.CMSG_REQUEST_CHALLENGE_MODE_REWARDS)]
        [Parser(Opcode.CMSG_RESET_CHALLENGE_MODE)]
        public static void HandleChallengeModeZero(Packet packet)
        {
        }
    }
}
