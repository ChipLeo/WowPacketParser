using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class TicketHandler
    {
        [Parser(Opcode.CMSG_GMTICKET_GETTICKET)]
        public static void HandleGetTicket(Packet packet)
        {
            packet.ReadInt32("Ticket ID");
        }
    }
}
