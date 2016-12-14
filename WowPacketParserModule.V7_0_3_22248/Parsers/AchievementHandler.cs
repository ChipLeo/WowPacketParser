using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class AchievementHandler
    {
        [Parser(Opcode.CMSG_SET_ACHIEVEMENTS_HIDDEN)]
        public static void HandleSetacievementsHidden(Packet packet)
        {
            packet.ResetBitReader();
            packet.ReadBit("Hidden");
        }
    }
}
