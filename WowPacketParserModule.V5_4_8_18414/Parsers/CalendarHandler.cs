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
            packet.ReadInt32("MaxInvites"); // 1200
            packet.ReadInt32E<CalendarFlag>("Flags"); // 1180
            packet.ReadInt32<LFGDungeonId>("Dungeon ID"); // 1172
            packet.ReadPackedTime("Event Time"); // 1176
            packet.ReadByteE<CalendarEventType>("Event Type"); // 1170

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
                packet.ReadByteE<CalendarEventStatus>("Status", i); // 8
                packet.ReadXORByte(guid[i], 5);
                packet.ReadByteE<CalendarModerationRank>("Moderation Rank", i); // 9

                packet.WriteGuid("Guid", guid[i], i);
            }

            packet.ReadWoWString("Title", titleLen);
            packet.ReadWoWString("Description", descLen);
        }

        [Parser(Opcode.CMSG_CALENDAR_UPDATE_EVENT)]
        public static void HandleUpdateCalendarEvent(Packet packet)
        {
            packet.ReadInt32("Max Invites");
            packet.ReadInt32<LFGDungeonId>("Dungeon ID");
            packet.ReadPackedTime("Event Time");
            packet.ReadInt32E<CalendarFlag>("Event Flags");
            packet.ReadByteE<CalendarEventType>("Event Type");

            var eventId = new byte[8];
            var inviteId = new byte[8];

            eventId[4] = packet.ReadBit();
            eventId[5] = packet.ReadBit();
            eventId[2] = packet.ReadBit();
            inviteId[4] = packet.ReadBit();
            eventId[7] = packet.ReadBit();
            eventId[0] = packet.ReadBit();
            inviteId[5] = packet.ReadBit();
            inviteId[3] = packet.ReadBit();
            eventId[6] = packet.ReadBit();
            eventId[1] = packet.ReadBit();
            inviteId[6] = packet.ReadBit();
            inviteId[2] = packet.ReadBit();
            inviteId[7] = packet.ReadBit();
            inviteId[1] = packet.ReadBit();
            inviteId[0] = packet.ReadBit();
            var descriptionLength = packet.ReadBits(11);
            var titleLength = packet.ReadBits(8);
            eventId[3] = packet.ReadBit();

            packet.ReadXORByte(inviteId, 6);
            packet.ReadXORByte(eventId, 0);
            packet.ReadXORByte(inviteId, 7);
            packet.ReadXORByte(inviteId, 3);
            packet.ReadXORByte(eventId, 6);
            packet.ReadXORByte(inviteId, 1);
            packet.ReadXORByte(eventId, 2);
            packet.ReadWoWString("Title", titleLength);
            packet.ReadXORByte(inviteId, 5);
            packet.ReadXORByte(inviteId, 4);
            packet.ReadXORByte(eventId, 5);
            packet.ReadXORByte(eventId, 3);
            packet.ReadXORByte(inviteId, 0);
            packet.ReadXORByte(eventId, 4);
            packet.ReadWoWString("Description", descriptionLength);
            packet.ReadXORByte(eventId, 1);
            packet.ReadXORByte(inviteId, 2);
            packet.ReadXORByte(eventId, 7);

            packet.WriteGuid("Invite", inviteId);
            packet.WriteGuid("EventId", eventId);
        }

        [Parser(Opcode.SMSG_CALENDAR_RAID_LOCKOUT_ADDED)]
        public static void HandleRaidLockoutAdded(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_CALENDAR_SEND_CALENDAR)]
        public static void HandleSendCalendar(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_CALENDAR_EVENT_INVITE)]
        public static void HandleSendCalendarEventInvite(Packet packet)
        {
            packet.ReadBool("Guild Event");
            packet.ReadByteE<CalendarEventStatus>("Status");
            packet.ReadInt64("Invite ID");
            packet.ReadByte("Player Level");
            packet.ReadInt64("Event ID");

            var guid = packet.StartBitStream(6, 4, 1, 3, 7, 0, 2, 5);
            var hasTime = !packet.ReadBit("!hasTime");
            packet.ReadBit("unk");

            packet.ParseBitStream(guid, 7, 0, 5);

            if (hasTime)
                packet.ReadPackedTime("Confirm Time");

            packet.ParseBitStream(guid, 2, 3, 4, 1, 6);

            packet.WriteGuid("Invitee GUID", guid);
        }

        [Parser(Opcode.SMSG_CALENDAR_EVENT_INVITE_ALERT)]
        public static void HandleCalendarEventInviteAlert(Packet packet)
        {
            packet.ReadInt64("Event ID");
            packet.ReadInt32<LFGDungeonId>("Dungeon ID");
            packet.ReadByteE<CalendarEventType>("Type");
            packet.ReadInt64("Invite ID");
            packet.ReadInt32E<CalendarFlag>("Event Flags");
            packet.ReadByteE<CalendarEventStatus>("Status");
            packet.ReadPackedTime("Time");
            packet.ReadByteE<CalendarModerationRank>("Moderation Rank");

            var guid = new byte[8];
            var guid2 = new byte[8];
            var GuildGuid = new byte[8];

            GuildGuid[7] = packet.ReadBit();
            guid2[6] = packet.ReadBit();
            GuildGuid[4] = packet.ReadBit();
            GuildGuid[0] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            GuildGuid[5] = packet.ReadBit();
            guid2[2] = packet.ReadBit();
            guid2[0] = packet.ReadBit();
            GuildGuid[1] = packet.ReadBit();
            guid2[5] = packet.ReadBit();
            GuildGuid[6] = packet.ReadBit();
            GuildGuid[3] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid2[4] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid2[1] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            GuildGuid[2] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var len = packet.ReadBits(8);
            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();

            packet.ReadXORByte(guid2, 5);
            packet.ReadXORByte(GuildGuid, 6);
            packet.ReadXORByte(GuildGuid, 0);
            packet.ReadXORByte(guid2, 6);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 4);

            packet.ReadWoWString("Title", len);

            packet.ReadXORByte(guid2, 0);
            packet.ReadXORByte(guid2, 1);
            packet.ReadXORByte(GuildGuid, 2);
            packet.ReadXORByte(GuildGuid, 7);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(GuildGuid, 1);
            packet.ReadXORByte(guid2, 2);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid2, 3);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid2, 7);
            packet.ReadXORByte(GuildGuid, 5);
            packet.ReadXORByte(GuildGuid, 4);
            packet.ReadXORByte(GuildGuid, 3);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid2, 4);

            packet.WriteGuid("Guild GUID", GuildGuid);
            packet.WriteGuid("Creator GUID", guid);
            packet.WriteGuid("Sender GUID", guid2);
        }

        [Parser(Opcode.SMSG_CALENDAR_SEND_EVENT)]
        public static void HandleSendCalendarEvent(Packet packet)
        {
            var invCount = packet.ReadBits("Invite Count", 20);

            var inviteGuid = new byte[invCount][];
            var inviteLen = new uint[invCount];

            for (var i = 0; i < invCount; i++)
            {
                inviteGuid[i] = new byte[8];

                inviteGuid[i][1] = packet.ReadBit();
                inviteGuid[i][2] = packet.ReadBit();
                inviteGuid[i][0] = packet.ReadBit();
                inviteGuid[i][7] = packet.ReadBit();
                inviteGuid[i][3] = packet.ReadBit();
                inviteGuid[i][5] = packet.ReadBit();
                inviteGuid[i][6] = packet.ReadBit();
                inviteLen[i] = packet.ReadBits(8);
                inviteGuid[i][4] = packet.ReadBit();
            }

            var titleLen = packet.ReadBits(8);
            var creatorGuid = new byte[8];
            var guildGuid = new byte[8];

            creatorGuid[2] = packet.ReadBit();
            creatorGuid[0] = packet.ReadBit();
            guildGuid[4] = packet.ReadBit();
            guildGuid[5] = packet.ReadBit();
            creatorGuid[1] = packet.ReadBit();
            creatorGuid[5] = packet.ReadBit();
            creatorGuid[3] = packet.ReadBit();
            guildGuid[6] = packet.ReadBit();

            var deskrLen = packet.ReadBits(11);

            guildGuid[1] = packet.ReadBit();
            guildGuid[7] = packet.ReadBit();
            creatorGuid[6] = packet.ReadBit();
            guildGuid[2] = packet.ReadBit();
            guildGuid[0] = packet.ReadBit();
            creatorGuid[4] = packet.ReadBit();
            guildGuid[3] = packet.ReadBit();
            creatorGuid[7] = packet.ReadBit();

            for (var i = 0; i < invCount; i++)
            {
                packet.ReadPackedTime("Status Time", i);
                packet.ParseBitStream(inviteGuid[i], 5);
                packet.ReadByte("Guild Member", i);
                packet.ParseBitStream(inviteGuid[i], 1);
                packet.ParseBitStream(inviteGuid[i], 2);
                packet.ParseBitStream(inviteGuid[i], 6);
                packet.ReadWoWString("Invite Text", inviteLen[i], i);
                packet.ReadByte("Player Level", i);
                packet.ParseBitStream(inviteGuid[i], 7);
                packet.ReadByteE<CalendarModerationRank>("Moderation Rank", i);
                packet.ReadInt64("Invite ID", i);
                packet.ParseBitStream(inviteGuid[i], 0);
                packet.ParseBitStream(inviteGuid[i], 3);
                packet.ParseBitStream(inviteGuid[i], 4);
                packet.ReadByteE<CalendarEventStatus>("Status", i);
                packet.WriteGuid("Invitee GUID", inviteGuid[i], i);
            }

            packet.ParseBitStream(creatorGuid, 0);
            packet.ParseBitStream(creatorGuid, 5);
            packet.ReadInt32E<CalendarFlag>("Event Flags");
            packet.ParseBitStream(guildGuid, 1);
            packet.ParseBitStream(creatorGuid, 7);
            packet.ParseBitStream(creatorGuid, 3);
            packet.ReadPackedTime("Event Time");
            packet.ParseBitStream(guildGuid, 6);
            packet.ParseBitStream(creatorGuid, 4);
            packet.ReadInt32<LFGDungeonId>("Dungeon ID");
            packet.ReadInt32("unk");
            packet.ParseBitStream(guildGuid, 4);
            packet.ReadByteE<CalendarEventType>("Event Type");
            packet.ReadWoWString("Title", titleLen);
            packet.ParseBitStream(creatorGuid, 6);
            packet.ParseBitStream(guildGuid, 3);
            packet.ParseBitStream(creatorGuid, 2);
            packet.ParseBitStream(guildGuid, 7);
            packet.ParseBitStream(creatorGuid, 1);
            packet.ReadWoWString("Description", deskrLen);
            packet.ParseBitStream(guildGuid, 2);
            packet.ParseBitStream(guildGuid, 5);
            packet.ParseBitStream(guildGuid, 0);
            packet.ReadInt64("Event ID");
            packet.ReadByteE<CalendarSendEventType>("Send Event Type");

            packet.WriteGuid("Creator GUID", creatorGuid);
            packet.WriteGuid("Guild Guid", guildGuid);
        }

        [Parser(Opcode.SMSG_CALENDAR_EVENT_UPDATED_ALERT)]
        public static void HandleCalendarEventUpdateAlert(Packet packet)
        {
            packet.ReadInt32E<CalendarFlag>("Event Flags");
            packet.ReadByteE<CalendarEventType>("Event Type");
            packet.ReadInt32("Unk int32 (UpdatedAlert)");
            packet.ReadPackedTime("Event Time");
            packet.ReadInt64("Event ID");
            packet.ReadPackedTime("Time2");
            packet.ReadInt32<LFGDungeonId>("Dungeon ID");
            packet.ReadBit("unk");
            var deskrLen = packet.ReadBits(11);
            var titleLen = packet.ReadBits(8);

            packet.ReadWoWString("Title", titleLen);
            packet.ReadWoWString("Description", deskrLen);
        }
    }
}
