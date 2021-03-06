﻿using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class CurrencyHandler
    {
        [Parser(Opcode.SMSG_SET_CURRENCY)]
        public static void HandleSetCurrency(Packet packet)
        {
            packet.ReadInt32("Type");
            packet.ReadInt32("Quantity");
            packet.ReadInt32("Flags");

            var bit28 = packet.ReadBit("HasTrackedQuantity");
            var bit36 = packet.ReadBit("HasWeeklyQuantity");
            var bit44 = packet.ReadBit("hasUnk");
            packet.ReadBit("SuppressChatLog");

            if (bit28)
                packet.ReadInt32("TrackedQuantity");
            if (bit36)
                packet.ReadInt32("WeeklyQuantity");
            if (bit44)
                packet.ReadInt32("Unk");
        }

        [Parser(Opcode.SMSG_SETUP_CURRENCY)]
        public static void HandleSetupCurrency(Packet packet)
        {
            var count = packet.ReadInt32("SetupCurrencyRecord");

            // ClientSetupCurrencyRecord
            for (var i = 0; i < count; ++i)
            {
                packet.ReadUInt32("Type", i);
                packet.ReadUInt32("Quantity", i);

                packet.ResetBitReader();

                var hasWeeklyQuantity = packet.ReadBit();
                var hasMaxWeeklyQuantity = packet.ReadBit();
                var hasTrackedQuantity = packet.ReadBit();
                var hasMaxQuantity = packet.ReadBit();
                packet.ReadBits("Flags", 5, i);

                if (hasWeeklyQuantity)
                    packet.ReadUInt32("WeeklyQuantity", i);

                if (hasMaxWeeklyQuantity)
                    packet.ReadUInt32("MaxWeeklyQuantity", i);

                if (hasTrackedQuantity)
                    packet.ReadUInt32("TrackedQuantity", i);

                if (hasMaxQuantity)
                    packet.ReadUInt32("MaxQuantity", i);
            }
        }

        [Parser(Opcode.SMSG_CONQUEST_FORMULA_CONSTANTS)]
        public static void HandleConquestFormulaConstants(Packet packet)
        {
            // Order guessed
            packet.ReadInt32("PvpMinCPPerWeek");
            packet.ReadInt32("PvpMaxCPPerWeek");
            packet.ReadSingle("PvpCPBaseCoefficient");
            packet.ReadSingle("PvpCPExpCoefficient");
            packet.ReadSingle("PvpCPNumerator");
        }
    }
}
