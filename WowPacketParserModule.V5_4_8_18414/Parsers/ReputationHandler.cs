﻿using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class ReputationHandler
    {
        [Parser(Opcode.CMSG_REQUEST_FORCED_REACTIONS)]
        public static void HandleReputationZero(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_RESET_FACTION_CHEAT)]
        public static void HandleResetFactionCheat(Packet packet)
        {
            packet.ReadUInt32("Faction Id");
            packet.ReadUInt32("Unk Int");
        }

        [Parser(Opcode.SMSG_INITIALIZE_FACTIONS)]
        public static void HandleInitializeFactions(Packet packet)
        {
            var count = 256;
            for (var i = 0; i < count; i++)
            {
                packet.ReadByteE<FactionFlag>("Faction Flags", i);
                packet.ReadUInt32E<ReputationRank>("Faction Standing", i);
            }
            for (var i = 0; i < count; i++)
            {
                packet.ReadBit();
            }
        }

        [Parser(Opcode.SMSG_SET_FORCED_REACTIONS)]
        public static void HandleForcedReactions(Packet packet)
        {
            var counter = packet.ReadBits("Factions", 6);
            for (var i = 0; i < counter; i++)
            {
                packet.ReadUInt32("Faction Id", i);
                packet.ReadUInt32("Reputation Rank", i);
            }
        }
    }
}
