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

        [Parser(Opcode.SMSG_UNK_0B81)]
        public static void HandleSUnk0B81(Packet packet)
        {
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.SMSG_UNK_103E)]
        public static void HandleSUnk103E(Packet packet)
        {
            packet.ReadInt32("Int32");
            packet.ReadInt32("Int28");
            packet.ReadInt32("Int24");
            packet.ReadInt64("QW16");
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

        [Parser(Opcode.SMSG_UNK_162A)]
        public static void HandleSUnk162A(Packet packet)
        {
            /*
            unk16: False
            Spell ID: 8177
            Guid: Type: Player
             */
            var guid = new byte[8];
            guid[3] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var unk16 = packet.ReadBit("unk16"); // 16
            guid[7] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            packet.ParseBitStream(guid, 0, 1, 2, 7, 3, 6, 5);
            packet.ReadInt32<SpellId>("Spell ID"); // 20
            packet.ParseBitStream(guid, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_UNK_1904)]
        public static void HandleSUnk1904(Packet packet)
        {
            packet.ReadGuid("unk");
            packet.ReadInt32("unk4");
        }

        [Parser(Opcode.SMSG_CRITERIA_DELETED)]
        public static void HandleCriteriaDeleted(Packet packet)
        {
            packet.ReadInt32("CriteriaID");
        }

        [Parser(Opcode.SMSG_BATTLE_PAY_DISTRIBUTION_UPDATE)]
        public static void HandleSUnk1E1B(Packet packet)
        {
            var guid = new byte[8];
            var guid2 = new byte[8];

            guid[5] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var unk5264 = packet.ReadBit("unk5264");
            guid[1] = packet.ReadBit();
            guid2[4] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid2[0] = packet.ReadBit();
            var unk5272 = packet.ReadBit("unk5272");
            guid2[1] = packet.ReadBit();
            guid2[2] = packet.ReadBit();
            var unk96 = 0u;
            var unk80 = 0u;
            var unk5168 = new bool[1048576];
            var unk5180 = new bool[1048576];
            var unk5260 = false;
            var unk352 = new bool[1048576];
            var unk368 = new bool[1048576];
            var unk1398 = new uint[1048576];
            var unk5500 = new bool[1048576];
            var unk372 = new uint[1048576];
            var unk885 = new uint[1048576];
            var unk360 = new bool[1048576];
            var unk5508 = new bool[1048576];
            var unk5520 = new bool[1048576];
            var unk5176 = new uint[1048576];
            var unk641 = 0u;
            var unk108 = false;
            var unk128 = 0u;
            var unk116 = false;
            var unk5256 = false;
            var unk1154 = 0u;
            var unk124 = false;
            if (unk5264)
            {
                unk96 = packet.ReadBits("unk96", 2); // 96
                unk80 = packet.ReadBits("unk80", 20); // 80
                for (var i = 0; i < unk80; i++) // 80
                {
                    unk5168[i] = packet.ReadBit("unk5168", i); // 5168
                    if (unk5168[i]) // 5168
                    {
                        unk352[i] = packet.ReadBit("unk84*4+16", i); // 352
                        unk1398[i] = packet.ReadBits("unk84*4+1062", 13, i); // 1398
                        unk5500[i] = packet.ReadBit("unk84*4+5164", i); // 5500
                        unk368[i] = packet.ReadBit("unk84*4+32", i); // 368
                        unk372[i] = packet.ReadBits("unk84*4+36", 10, i); // 372
                        unk885[i] = packet.ReadBits("unk84*4+549", 10, i); // 885
                        unk360[i] = packet.ReadBit("unk84*4+24", i); // 360
                    }
                    unk5508[i] = packet.ReadBit("unk84*4+5172", i); // 5508
                    unk5520[i] = packet.ReadBit("unk84*4+5184", i); // 5520
                    unk5180[i] = packet.ReadBit("unk84*4+5180", i); // 5180
                    if (unk5180[i]) // 5180
                        unk5176[i] = packet.ReadBits("unk84*4+5176", 4, i); // 5176
                }
                unk5260 = packet.ReadBit("unk5260"); // 5260
                if (unk5260) // 5260
                {
                    unk641 = packet.ReadBits("unk641", 10); // 641
                    unk108 = packet.ReadBit("unk108"); // 108
                    unk128 = packet.ReadBits("unk128", 10); // 128
                    unk116 = packet.ReadBit("unk116"); // 116
                    unk5256 = packet.ReadBit("unk5256"); // 5256
                    unk1154 = packet.ReadBits("unk1154", 13); // 1154
                    unk124 = packet.ReadBit("unk124"); // 124
                }
            }

            guid[7] = packet.ReadBit();
            guid2[6] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid2[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            guid[4] = packet.ReadBit();

            if (unk5264) // 5264
            {
                for (var i = 0; i < unk80; i++) // 80
                {
                    packet.ReadInt32("unk84", i); // 84
                    if (unk5168[i]) // 5168
                    {
                        if (unk352[i]) // 352
                            packet.ReadInt32("unk348", i); // 348
                        if (unk368[i]) // 368
                            packet.ReadInt32("unk344", i); // 344
                        packet.ReadWoWString("str", unk372[i], i); // 372
                        if (unk5500[i]) // 5500
                            packet.ReadInt32("unk5496", i); // 5496
                        packet.ReadWoWString("str2", unk1398[i], i); // 1398
                        packet.ReadWoWString("str3", unk885[i], i); // 885
                        if (unk360[i]) // 360
                            packet.ReadInt32("unk356", i); // 356
                    }
                    packet.ReadInt32("unk344", i); // 344
                    packet.ReadInt32("unk340", i); // 340
                }
                packet.ReadInt32("unk56"); // 56
                packet.ReadInt64("unk72"); // 72
                if (unk5260) // 5260
                {
                    if (unk5256) // 5256
                        packet.ReadInt32("unk5252"); // 5252
                    packet.ReadWoWString("str1", unk1154);
                    packet.ReadWoWString("str2", unk128);
                    packet.ReadWoWString("str3", unk641);
                    if (unk116)
                        packet.ReadInt32("unk112"); // 112
                    if (unk124)
                        packet.ReadInt32("unk120"); // 120
                    if (unk108)
                        packet.ReadInt32("unk104"); // 104
                }
                packet.ReadInt64("unk64"); // 64
                packet.ReadByte("unk97"); // 97
                packet.ReadInt32("unk100"); // 100
            }
            packet.ReadInt32("unk28"); // 28
            packet.ParseBitStream(guid2, 4);
            packet.ReadInt64("unk48"); // 48
            packet.ParseBitStream(guid2, 1, 5);
            packet.ParseBitStream(guid, 2, 4, 1, 0);
            packet.ReadInt32("unk44");
            packet.ParseBitStream(guid, 7);
            packet.ParseBitStream(guid2, 0, 7);
            packet.ReadInt32("unk40");
            packet.ReadInt32("unk24");
            packet.ParseBitStream(guid2, 6);
            packet.ParseBitStream(guid, 5, 6, 3);
            packet.ParseBitStream(guid2, 3, 2);
            packet.WriteGuid("Guid", guid);
            packet.WriteGuid("Guid2", guid2);
        }

        [Parser(Opcode.SMSG_UNK_0332)]
        public static void HandleCoded(Packet packet)
        {
            packet.AsHex();
            var select = packet.ReadInt32("select");
            switch(select)
            {
                case 2:
                case 3:
                case 4:
                    packet.ReadPackedGuid("Guid");
                    packet.ReadByte("unk");
                    break;
                case 1:
                case 9:
                case 10:
                    break;
                case 7:
                    packet.ReadByte("unk1");
                    packet.ReadByte("unk2");
                    break;
                case 0:
                case 5:
                case 6:
                case 8:
                    packet.ReadByte("unk");
                    break;
            }
        }

        [Parser(Opcode.SMSG_UNK_043A)]
        [Parser(Opcode.SMSG_UNK_0E8E)]
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
        [Parser(Opcode.SMSG_NULL_1313)]
        public static void HandleUnkNull(Packet packet)
        {
        }
    }
}
