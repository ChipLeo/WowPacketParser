using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class TicketHandler
    {
        [Parser(Opcode.CMSG_GM_TICKET_ACKNOWLEDGE_SURVEY)]
        public static void HandleGMTicketAcknowledgeSurvey(Packet packet)
        {
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.SMSG_GM_TICKET_GET_TICKET)]
        public static void HandleGMTicketGetTicket(Packet packet)
        {
            var hasData = packet.ReadBit("HasData");
            if (hasData)
            {
                var len1 = packet.ReadBits(11); // len6
                var len2 = packet.ReadBits(10); // len2042
                packet.ReadInt32("Ticket ID"); // 5
                packet.ReadByteE<GMTicketEscalationStatus>("Escalation Status"); // 2040
                packet.ReadByteE<GMTicketOpenedByGMStatus>("Opened"); // 2041
                packet.ReadByte("unk2025"); // 2025
                packet.ReadInt32("MapID"); // 509
                packet.ReadWoWString("Ticket", len1); // 6
                packet.ReadInt32("unk767"); // 767
                packet.ReadInt32("Ticket oldest"); // 507
                packet.ReadInt32("Ticket age"); // 508
                packet.ReadWoWString("str2042", len2); // 2042
            }
            packet.ReadInt32E<GMTicketSystemStatus>("System Status");
        }

        [Parser(Opcode.SMSG_GM_TICKET_RESOLVE_RESPONSE)]
        public static void HandleGMTicketResolveResponse(Packet packet)
        {
            packet.ReadBit("ShowSurvey");
        }

        [Parser(Opcode.SMSG_GM_TICKET_SYSTEM_STATUS)]
        public static void HandleGMTicketSystemStatus(Packet packet)
        {
            packet.ReadInt32("Status");
        }

        [Parser(Opcode.SMSG_GM_TICKET_RESPONSE_ERROR)]
        public static void HandleGMTicketNull(Packet packet)
        {
        }
    }
}
