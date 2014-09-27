using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class LfgHandler
    {
        [Parser(Opcode.SMSG_LF_GUILD_APPLICATIONS_LIST_UPDATED)]
        public static void HandleLFGuildApplicationsListUpdated(Packet packet)
        {
            var count = packet.ReadBits("count", 19);
            var guids = new byte[count][];
            var unk32 = new uint[count];
            var unk152 = new uint[count];
            for (var i = 0; i < count; i++)
            {
                guids[i] = new byte[8];
                guids[i][0] = packet.ReadBit();
                guids[i][4] = packet.ReadBit();
                guids[i][2] = packet.ReadBit();
                guids[i][7] = packet.ReadBit();
                unk32[i] = packet.ReadBits("unk32", 7, i);
                guids[i][1] = packet.ReadBit();
                guids[i][3] = packet.ReadBit();
                unk152[i] = packet.ReadBits("unk152", 10, i);
                guids[i][6] = packet.ReadBit();
                guids[i][5] = packet.ReadBit();
            }
            for (var i = 0; i < count; i++)
            {
                packet.ReadInt32("unk136", i); // 136
                packet.ReadInt32("unk28", i); // 28
                packet.ReadWoWString("str1", unk32[i]); // 32
                packet.ParseBitStream(guids[i], 4);
                packet.ReadInt32("unk132", i); // 132
                packet.ParseBitStream(guids[i], 6, 5);
                packet.ReadInt32("unk144", i); // 144
                packet.ParseBitStream(guids[i], 1, 3, 0, 7, 2);
                packet.ReadInt32("unk148", i); // 148
                packet.ReadInt32("unk140", i); // 140
                packet.ReadWoWString("str2", unk152[i]);
                packet.WriteGuid("Guid", guids[i], i);
            }
            packet.ReadInt32("unk32");
        }

        [Parser(Opcode.CMSG_LFG_PLAYER_LOCK_INFO_REQUEST)]
        public static void HandleLFGPlayerLockInfoRequest(Packet packet)
        {
            packet.ReadByte("Unk17");
            packet.ReadBit("Unk16");
        }

        [Parser(Opcode.SMSG_LFG_PLAYER_INFO)]
        public static void HandleLfgPlayerLockInfoResponse(Packet packet)
        {
            var unk48 = packet.ReadBits("unk48", 20);
            var unk40 = packet.ReadBit("unk40");
            var unk16 = packet.ReadBits("unk16", 17);
            var unk84 = new uint[unk16];
            var unk144 = new uint[unk16];
            var unk112 = new uint[unk16];
            var unk128 = new uint[unk16];
            var unk116 = new uint[unk16][];
            var unk100 = new uint[unk16][];
            var unk132 = new uint[unk16][];
            for (var i = 0; i < unk16; i++)
            {
                packet.ReadBit("unk80", i);
                packet.ReadBit("unk24", i);
                unk144[i] = packet.ReadBits("unk144", 21, i);
                unk84[i] = packet.ReadBits("unk84", 19, i);
                unk112[i] = packet.ReadBits("unk112", 20, i);
                unk116[i] = new uint[unk84[i]];
                unk100[i] = new uint[unk84[i]];
                unk132[i] = new uint[unk84[i]];
                for (var j = 0; j < unk84[i]; j++)
                {
                    unk116[i][j] = packet.ReadBits("unk116", 21, i, j);
                    unk100[i][j] = packet.ReadBits("unk100", 20, i, j);
                    unk132[i][j] = packet.ReadBits("unk132", 21, i, j);
                }
                unk128[i] = packet.ReadBits("unk128", 21, i);
            }
            var guid32 = new byte[8];
            if (unk40)
            {
                guid32 = packet.StartBitStream(5, 1, 2, 7, 3, 0, 6, 4);
                packet.ParseBitStream(guid32, 7, 2, 3, 0, 4, 5, 6, 1);
                packet.WriteGuid("guid32", guid32);
            }

            for (var i = 0; i < unk16; i++)
            {
                packet.ReadInt32("unk108", i);
                for (var j = 0; j < unk84[i]; j++)
                {
                    packet.ReadInt32("unk92", i, j);
                    for (var k = 0; k < unk116[i][j]; k++)
                    {
                        packet.ReadInt32("unk", i, j, k);
                        packet.ReadInt32("unk2", i, j, k);
                    }
                    for (var k = 0; k < unk100[i][j]; k++)
                    {
                        packet.ReadInt32("unk", i, j, k);
                        packet.ReadInt32("unk2", i, j, k);
                        packet.ReadInt32("unk3", i, j, k);
                    }
                    packet.ReadInt32("unk88", i, j);
                    for (var k = 0; k < unk132[i][j]; k++)
                    {
                        packet.ReadInt32("unk", i, j, k);
                        packet.ReadInt32("unk2", i, j, k);
                    }
                    packet.ReadInt32("unk96", i, j);
                }
                packet.ReadInt32("unk40", i);
                for (var k = 0; k < unk144[i]; k++)
                {
                    packet.ReadInt32("unk", i, k);
                    packet.ReadInt32("unk2", i, k);
                }
                packet.ReadInt32("unk68", i);
                packet.ReadInt32("unk104", i);
                for (var k = 0; k < unk112[i]; k++)
                {
                    packet.ReadInt32("unk", i, k);
                    packet.ReadInt32("unk2", i, k);
                    packet.ReadInt32("unk3", i, k);
                }
                packet.ReadInt32("unk48", i);
                packet.ReadInt32("unk56", i);
                packet.ReadInt32("unk52", i);
                packet.ReadInt32("unk72", i);
                packet.ReadInt32("unk36", i);
                packet.ReadInt32("unk40", i);
                for (var k = 0; k < unk128[i]; k++)
                {
                    packet.ReadInt32("unk", i, k);
                    packet.ReadInt32("unk2", i, k);
                }
                packet.ReadInt32("unk60", i);
                packet.ReadInt32("unk100", i);
                packet.ReadInt32("unk64", i);
                packet.ReadInt32("unk32", i);
                packet.ReadInt32("unk44", i);
                packet.ReadInt32("unk76", i);
                packet.ReadInt32("unk28", i);
            }

            for (var i = 0; i < unk48; i++)
            {
                packet.ReadInt32("unk52", i);
                packet.ReadInt32("unk56", i);
                packet.ReadInt32("unk60", i);
                packet.ReadInt32("unk64", i);
            }
        }

        [Parser(Opcode.SMSG_LFG_QUEUE_STATUS)]
        public static void HandleLfgQueueStatusUpdate(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_LFG_ROLE_CHECK_UPDATE)]
        public static void HandLFGRoleCheckUpdate(Packet packet)
        {
            var guid = new byte[8];
            packet.ReadByte("unk65"); // 65
            packet.ReadEnum<LfgRoleCheckStatus>("Role Check Status", TypeCode.Byte); // 16
            var count36 = packet.ReadBits("Member count", 21); // 36
            var guid40 = new byte[count36][];
            for (var i = 0; i < count36; i++)
            {
                packet.ReadBit("Choosed", i); // 48
                guid40[i] = packet.StartBitStream(3, 0, 5, 2, 7, 1, 4, 6);
            }
            guid[3] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var count20 = packet.ReadBits("Entry count", 22);
            guid[0] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var unk64 = packet.ReadBit("Is Beginning"); // 64
            packet.ParseBitStream(guid, 0);
            for (var i = 0; i < count36; i++)
            {
                packet.ReadByte("Level", i); // 56
                packet.ParseBitStream(guid40[i], 3, 6);
                packet.ReadEnum<LfgRoleFlag>("Roles", TypeCode.Int32, i); // 88
                packet.ParseBitStream(guid40[i], 2, 4, 0, 1, 5, 7);
                packet.WriteGuid("Player", guid40[i]);
            }
            packet.ParseBitStream(guid, 1, 7, 6, 4, 3, 2, 5);
            packet.WriteGuid("Guid", guid);
            for (var i = 0; i < count20; i++)
                packet.ReadLfgEntry("LFG Entry", i); // 24
        }

        [Parser(Opcode.SMSG_ROLE_POLL_BEGIN)]
        public static void HandleRolePollBegin(Packet packet)
        {
            var guid = new byte[8];
            guid[6] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var unk32 = packet.ReadBit("unk32"); // 32
            guid[3] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[4] = packet.ReadBit();

            packet.ParseBitStream(guid, 0, 3, 6);
            packet.ReadInt32("unk64"); // 64
            packet.ParseBitStream(guid, 5, 1, 4, 2, 7);
            packet.WriteGuid("Guid", guid);
        }
    }
}
