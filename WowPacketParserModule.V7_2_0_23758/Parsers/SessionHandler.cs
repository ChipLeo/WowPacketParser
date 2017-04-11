using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class SessionHandler
    {
        public static void ReadClientSettings(Packet packet, params object[] idx)
        {
            packet.ReadSingle("FarClip", idx);
        }

        public static void ReadMethodCall(Packet packet, params object[] idx)
        {
            packet.ReadUInt64("Type", idx);
            packet.ReadUInt64("ObjectId", idx);
            packet.ReadUInt32("Token", idx);
        }

        [Parser(Opcode.CMSG_BATTLENET_REQUEST)]
        public static void HandleBattlenetRequest(Packet packet)
        {
            ReadMethodCall(packet, "Method");

            int protoSize = packet.ReadInt32();
            packet.ReadBytesTable("Data", protoSize);
        }

        [Parser(Opcode.CMSG_PLAYER_LOGIN)]
        public static void HandlePlayerLogin(Packet packet)
        {
            var guid = packet.ReadPackedGuid128("Guid");
            ReadClientSettings(packet, "ClientSettings");
            CoreParsers.SessionHandler.LoginGuid = guid;
        }

        [Parser(Opcode.CMSG_QUERY_REALM_NAME)]
        public static void HandleRealmQuery(Packet packet)
        {
            packet.ReadInt32("VirtualRealmAddress");
        }

        [Parser(Opcode.SMSG_BATTLENET_NOTIFICATION)]
        public static void HandleBattlenetNotification(Packet packet)
        {
            ReadMethodCall(packet, "Method");

            int protoSize = packet.ReadInt32();
            packet.ReadBytesTable("Data", protoSize);
        }

        [Parser(Opcode.SMSG_BATTLENET_RESPONSE)]
        public static void HandleBattlenetResponse(Packet packet)
        {
            packet.ReadInt32E<BattlenetRpcErrorCode>("BnetStatus");
            ReadMethodCall(packet, "Method");

            int protoSize = packet.ReadInt32();
            packet.ReadBytesTable("Data", protoSize);
        }

        [Parser(Opcode.SMSG_BATTLENET_SET_SESSION_STATE)]
        public static void HandleBattlenetSetSessionState(Packet packet)
        {
            packet.ReadBits("State", 2); // TODO: enum
        }

        [Parser(Opcode.SMSG_DANCE_STUDIO_CREATE_RESULT)]
        public static void HandleDanceStudioCreateResult(Packet packet)
        {
            packet.ReadBit("Enable");

            for (int i = 0; i < 4; i++)
                packet.ReadInt32("Secrets", i);
        }

        [Parser(Opcode.SMSG_QUERY_TIME_RESPONSE)]
        public static void HandleQueryTimeResponse(Packet packet)
        {
            packet.ReadTime("CurrentTime");
        }

        [Parser(Opcode.SMSG_ENABLE_ENCRYPTION)]
        public static void HandleSessionZero(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_LOGOUT_COMPLETE, ClientVersionBuild.V7_1_0_22900)]
        public static void HandleLogoutComplete(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_LOGOUT_REQUEST, ClientVersionBuild.V7_1_0_22900)]
        public static void HandleLogoutRequest(Packet packet)
        {
            packet.ReadBit("IdleLogout");
        }

        [Parser(Opcode.SMSG_MOTD)]
        public static void HandleMessageOfTheDay(Packet packet)
        {
            var lineCount = packet.ReadBits("Line Count", 4);

            packet.ResetBitReader();
            for (var i = 0; i < lineCount; i++)
            {
                var lineLength = (int)packet.ReadBits(7);
                packet.ResetBitReader();
                packet.ReadWoWString("Line", lineLength, i);
            }
        }

        [Parser(Opcode.SMSG_REALM_QUERY_RESPONSE)]
        public static void HandleRealmQueryResponse(Packet packet)
        {
            packet.ReadUInt32("VirtualRealmAddress");

            var state = packet.ReadByte("LookupState");
            if (state == 0)
            {
                packet.ResetBitReader();

                packet.ReadBit("IsLocal");
                packet.ReadBit("Unk bit");

                var bits2 = packet.ReadBits(8);
                var bits258 = packet.ReadBits(8);
                packet.ReadBit();

                packet.ReadWoWString("RealmNameActual", bits2);
                packet.ReadWoWString("RealmNameNormalized", bits258);
            }
        }

        [Parser(Opcode.SMSG_SUSPEND_TOKEN)]
        [Parser(Opcode.SMSG_RESUME_TOKEN)]
        public static void HandleResumeTokenPacket(Packet packet)
        {
            packet.ReadUInt32("Sequence");
            packet.ReadBits("Reason", 2);
        }

        [Parser(Opcode.SMSG_SET_TIME_ZONE_INFORMATION)]
        public static void HandleSetTimeZoneInformation(Packet packet)
        {
            var len1 = packet.ReadBits(7);
            var len2 = packet.ReadBits(7);

            packet.ReadWoWString("ServerTimeTZ", len1);
            packet.ReadWoWString("GameTimeTZ", len2);
        }
    }
}
