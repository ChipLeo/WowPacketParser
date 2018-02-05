using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParserModule.V5_4_8_18414.Enums;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class UnkHandler
    {
        [Parser(Opcode.CMSG_UNK_10D3)]
        public static void HandleUnk10D3(Packet packet)
        {
            packet.ReadInt32("unk20"); // 20
            packet.ReadInt32("unk16"); // 16
        }

        [Parser(Opcode.CMSG_UNK_12B3)]
        public static void HandleUnk12B3(Packet packet)
        {
            // Str: DUNGEONS\TEXTURES\EFFECTS\BLUE_REFLECT..tga - (null)
            packet.ReadWoWString("Str", packet.ReadBits(9));
        }

        [Parser(Opcode.CMSG_UNK_1D8D)]
        public static void HandleUnk1D8D(Packet packet)
        {
            packet.ReadSingle("unk");
        }

        [Parser(Opcode.SMSG_UNK_000F)]
        public static void HandleSUnk000F(Packet packet)
        {
            var guid = packet.StartBitStream(3, 2, 6, 0, 4, 5, 7, 1);
            packet.ParseBitStream(guid, 6, 1, 5, 7, 3, 4, 2, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_UNK_1206)]
        public static void HandleSUnk1206(Packet packet)
        {
            var guid = new byte[8];
            var guid2 = new byte[8];

            guid2[1] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            guid2[4] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid2[0] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid2[2] = packet.ReadBit();
            guid2[5] = packet.ReadBit();
            guid2[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[2] = packet.ReadBit();

            packet.ParseBitStream(guid, 5, 4);
            packet.ParseBitStream(guid2, 3, 0, 5);
            packet.ParseBitStream(guid, 0);
            packet.ParseBitStream(guid2, 2);
            packet.ParseBitStream(guid, 1, 7, 6, 2);
            packet.ParseBitStream(guid2, 1, 6);

            packet.ReadByte("unk32"); // 32

            packet.ParseBitStream(guid2, 4, 7);
            packet.ParseBitStream(guid, 3);

            packet.WriteGuid("Guid", guid);
            packet.WriteGuid("Guid2", guid2);
        }

        [Parser(Opcode.SMSG_UNK_043A)]
        [Parser(Opcode.SMSG_UNK_10BA)]
        [Parser(Opcode.SMSG_UNK_142B)]
        [Parser(Opcode.SMSG_UNK_1E3F)]
        public static void HandleUnkBit(Packet packet)
        {
            packet.ReadBool("Result");
        }

        /*
        S 021B

            ReadInt32
        */

        [Parser(Opcode.CMSG_NULL_01C0)]
        [Parser(Opcode.CMSG_NULL_05E1)]
        [Parser(Opcode.CMSG_NULL_1063)]
        [Parser(Opcode.CMSG_NULL_135B)]
        [Parser(Opcode.CMSG_NULL_14E0)]
        [Parser(Opcode.CMSG_NULL_1A87)]
        [Parser(Opcode.CMSG_NULL_1C45)]
        [Parser(Opcode.CMSG_NULL_1C5A)]
        [Parser(Opcode.CMSG_NULL_1F8E)]
        public static void HandleUnkNull(Packet packet)
        {
        }
    }
}
