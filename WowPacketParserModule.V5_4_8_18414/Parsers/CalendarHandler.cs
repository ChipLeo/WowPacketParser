using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class CalendarHandler
    {
        [Parser(Opcode.CMSG_CALENDAR_ADD_EVENT)]
        public static void HandleAddCalendarEvent(Packet packet)
        {
            packet.ReadInt32("unk1200"); // 1200
            packet.ReadInt32("unk1180"); // 1180
            packet.ReadInt32("unk1172"); // 1172
            packet.ReadInt32("unk1176"); // 1176
            packet.ReadEnum<CalendarEventType>("Event Type", TypeCode.Byte); // 1170

            var count = packet.ReadBits(22); // 1184
            var descLen = packet.ReadBits(11); // 145

            var guid = new byte[count][];

            for (var i = 0; i < count; i++)
                guid[i] = packet.StartBitStream(7, 2, 6, 3, 5, 1, 0, 4);

            var titleLen = packet.ReadBits(8); // 16

            for (var i = 0; i < count; i++)
            {
                packet.ReadXORByte(guid[i], 4);
                packet.ReadXORByte(guid[i], 2);
                packet.ReadXORByte(guid[i], 3);
                packet.ReadXORByte(guid[i], 1);
                packet.ReadXORByte(guid[i], 0);
                packet.ReadXORByte(guid[i], 6);
                packet.ReadXORByte(guid[i], 7);
                packet.ReadEnum<CalendarEventStatus>("Status", TypeCode.Byte, i); // 8
                packet.ReadXORByte(guid[i], 5);
                packet.ReadEnum<CalendarModerationRank>("Moderation Rank", TypeCode.Byte, i); // 9

                packet.WriteGuid("Guid", guid[i], i);
            }

            packet.ReadWoWString("Title", titleLen);
            packet.ReadWoWString("Description", descLen);
        }
    }
}
