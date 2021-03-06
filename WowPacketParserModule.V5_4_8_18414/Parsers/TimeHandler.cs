﻿using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class TimeHandler
    {
        [Parser(Opcode.SMSG_GAME_TIME_UPDATE)]
        public static void HandleGameTimeUpdate(Packet packet)
        {
            packet.ReadPackedTime("Int28"); // 28
            packet.ReadPackedTime("Int16"); // 16
            packet.ReadInt32("Int20"); // 20
            packet.ReadInt32("Int24"); // 24
        }

        [Parser(Opcode.SMSG_LOGIN_SET_TIME_SPEED)]
        public static void HandleLoginSetTimeSpeed(Packet packet)
        {
            packet.ReadInt32("unk32");
            packet.ReadPackedTime("Time1");
            packet.ReadInt32("unk20");
            packet.ReadPackedTime("Time2");
            packet.ReadSingle("unk28");
        }

        [Parser(Opcode.SMSG_START_TIMER)]
        public static void HandleStartTimer(Packet packet)
        {
            packet.ReadInt32("unk20");
            packet.ReadInt32("unk24");
            packet.ReadInt32("unk16");
        }

        [Parser(Opcode.SMSG_TIME_ADJUSTMENT)]
        public static void HandleTimeAdjustement(Packet packet)
        {
            packet.ReadSingle("unk1");
            packet.ReadInt32("unk2");
        }

        [Parser(Opcode.SMSG_PLAY_TIME_WARNING)]
        [Parser(Opcode.SMSG_SERVER_TIME)]
        public static void HandleServerTime(Packet packet)
        {
            packet.ReadPackedTime("Server game time");
            packet.ReadUInt32("Server last tick");
        }
    }
}