using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class ChannelHandler
    {
        [Parser(Opcode.CMSG_CHANNEL_SILENCE_VOICE)]
        [Parser(Opcode.CMSG_CHANNEL_UNSILENCE_VOICE)]
        [Parser(Opcode.CMSG_CHANNEL_SILENCE_ALL)]
        [Parser(Opcode.CMSG_CHANNEL_UNSILENCE_ALL)]
        public static void HandleChannelSilencing(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_CHANNEL_LIST)]
        public static void HandleChannelList(Packet packet)
        {
            packet.ReadWoWString("Channel", packet.ReadBits(7));
        }

        [Parser(Opcode.CMSG_CHANNEL_OWNER)]
        [Parser(Opcode.CMSG_CHANNEL_ANNOUNCEMENTS)]
        [Parser(Opcode.CMSG_CHANNEL_VOICE_ON)]
        [Parser(Opcode.CMSG_CHANNEL_VOICE_OFF)]
        [Parser(Opcode.CMSG_SET_CHANNEL_WATCH)]
        [Parser(Opcode.CMSG_CHANNEL_DECLINE_INVITE)]
        [Parser(Opcode.CMSG_CHANNEL_DISPLAY_LIST)]
        public static void HandleChannelMisc(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_CHANNEL_LIST)]
        public static void HandleChannelSendList(Packet packet)
        {
            packet.ReadByte("Type");
            packet.ReadCString("Channel Name");
            packet.ReadByteE<ChannelFlag>("Flags");
            var count = packet.ReadInt32("Counter");
            for (var i = 0; i < count; i++)
            {
                packet.ReadGuid("Player GUID " + i);
                packet.ReadUInt32("Realm ID");
                packet.ReadByteE<ChannelMemberFlag>("Player Flags " + i);
            }
        }

        [Parser(Opcode.SMSG_CHANNEL_MEMBER_COUNT)]
        public static void HandleChannelMemberCount(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_CHANNEL_BAN)]
        public static void HandleChannelBan(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_JOIN_CHANNEL)]
        public static void HandleJoinChannel(Packet packet)
        {
            packet.ReadInt32("Channel Id");
            packet.ReadBit("Joined by zone update");
            var len1 = packet.ReadBits("len1", 7);
            var len2 = packet.ReadBits("len2", 7);
            packet.ReadBit("Has Voice");
            packet.ResetBitReader();
            packet.ReadWoWString("Channel Name", len1);
            packet.ReadWoWString("Channel Pass", len2);
        }

        [Parser(Opcode.CMSG_LEAVE_CHANNEL)]
        public static void HandleChannelLeave(Packet packet)
        {
            packet.ReadInt32("Channel Id");
            packet.ReadWoWString("Channel Name", packet.ReadBits(7));
        }

        [Parser(Opcode.SMSG_USERLIST_REMOVE)]
        public static void HandleChannelUserListRemove(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_USERLIST_ADD)]
        [Parser(Opcode.SMSG_USERLIST_UPDATE)]
        public static void HandleChannelUserListAdd(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_CHANNEL_PASSWORD)]
        public static void HandleChannelPassword(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_CHANNEL_INVITE)]
        public static void HandleChannelInvite(Packet packet)
        {
        }
    }
}
