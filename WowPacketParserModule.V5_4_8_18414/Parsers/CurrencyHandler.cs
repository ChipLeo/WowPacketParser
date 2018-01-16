using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class CurrencyHandler
    {
        [Parser(Opcode.SMSG_RESET_WEEKLY_CURRENCY)]
        public static void HandleCurrencyZero(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_CONQUEST_FORMULA_CONSTANTS)]
        public static void HandleConquestFormulaConstants(Packet packet)
        {
            // Order guessed
            packet.ReadInt32("PvpMinCPPerWeek"); // 24
            packet.ReadSingle("PvpCPBaseCoefficient"); // 32
            packet.ReadSingle("PvpCPExpCoefficient"); // 28
            packet.ReadInt32("PvpMaxCPPerWeek"); // 16
            packet.ReadSingle("PvpCPNumerator");
        }
    }
}
