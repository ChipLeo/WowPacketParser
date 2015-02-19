using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParserModule.V5_4_8_18414.Enums;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class UnkHandler
    {
        [Parser(Opcode.CMSG_UNK_0002)]
        public static void HandleUnk0002(Packet packet)
        {
            packet.ReadBit("unk");
        }

        [Parser(Opcode.CMSG_UNK_009A)]
        public static void HandleUnk009A(Packet packet)
        {
            var len1 = packet.ReadBits("len1", 5);
            var len2 = packet.ReadBits("len2", 8);
            packet.ReadWoWString("Str", len2);
            packet.ReadWoWString("Addon", len1);
        }

        [Parser(Opcode.CMSG_UNK_028E)]
        public static void HandleUnk028E(Packet packet)
        {
            var len33 = packet.ReadBits("len33", 8);
            var len16 = packet.ReadBits("len16", 5);
            packet.ReadWoWString("str33", len33);
            packet.ReadWoWString("str16", len16);
        }

        [Parser(Opcode.CMSG_UNK_0377)]
        public static void HandleUnk0377(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_UNK_03F1)]
        public static void HandleUnk03F1(Packet packet)
        {
            var guid = packet.StartBitStream(5, 6, 0, 4, 1, 2, 7, 3);
            packet.ParseBitStream(guid, 0, 2, 6, 7, 1, 5, 3, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_UNK_06C5)]
        public static void HandleUnk06C5(Packet packet)
        {
            for (var i = 0; i < 2; i++)
            {
                var poz = new Vector3();
                poz.X = packet.ReadSingle();
                poz.Z = packet.ReadSingle();
                poz.Y = packet.ReadSingle();
                packet.AddValue("Poz", poz, i);
            }
            var pos = new Vector4();
            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();

            var guid = new byte[8];
            guid[0] = packet.ReadBit();
            var hasO = !packet.ReadBit("!hasO");
            guid[6] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            var unk24 = !packet.ReadBit("unk24!=-1");

            packet.ParseBitStream(guid, 3, 6, 5, 2, 7, 1, 0, 4);
            if (hasO)
                pos.O = packet.ReadSingle();
            if (unk24)
                packet.ReadInt32("unk24");

            packet.AddValue("Position", pos);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_UNK_06C9)]
        public static void HandleUnk06C9(Packet packet)
        {
            packet.ReadInt32("unk16"); // 16
        }

        [Parser(Opcode.CMSG_UNK_08AF)]
        public static void HandleUnk08AF(Packet packet)
        {
            var len272 = packet.ReadBits(5);
            var len16 = packet.ReadBits(8);
            packet.ReadWoWString("Message", len16);
            packet.ReadWoWString("Addon", len272);
        }

        [Parser(Opcode.CMSG_UNK_0A16)]
        public static void HandleUnk0A16(Packet packet)
        {
            var guid = packet.StartBitStream(7, 6, 3, 0, 4, 1, 5, 2);
            packet.ParseBitStream(guid, 1, 6, 0, 5, 3, 2, 4, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_UNK_0E3B)]
        public static void HandleUnk0E3B(Packet packet)
        {
            var len33 = packet.ReadBits(8);
            var len16 = packet.ReadBits(5);
            packet.ReadWoWString("Prefix", len16);
            packet.ReadWoWString("Message", len33);
        }

        [Parser(Opcode.CMSG_UNK_10C3)]
        public static void HandleUnk10C3(Packet packet)
        {
            var guid = packet.StartBitStream(1, 5, 7, 2, 6, 3, 0, 4);
            packet.ParseBitStream(guid, 2, 5, 4, 6, 1, 0, 7, 3);
            packet.WriteGuid("Guid", guid);
        }

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

        [Parser(Opcode.CMSG_UNK_1341)]
        public static void HandleUnk1341(Packet packet)
        {
            packet.ReadBool("Unk");
        }

        [Parser(Opcode.CMSG_UNK_1370)]
        public static void HandleUnk1370(Packet packet)
        {
            packet.ReadEntry<UInt32>(StoreNameType.Quest, "Quest ID"); // 24
            packet.ReadByte("unk28"); // 28
            var guid = packet.StartBitStream(5, 3, 0, 6, 1, 2, 7, 4);
            packet.ParseBitStream(guid, 1, 2, 0, 5, 6, 4, 7, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_UNK_178A)]
        public static void HandleUnk178A(Packet packet)
        {
            packet.ReadByte("unk");
        }

        [Parser(Opcode.CMSG_UNK_1886)]
        public static void HandleUnk1886(Packet packet)
        {
            packet.ReadInt32("unk28");
            packet.ReadInt32("unk24");
            var guid = packet.StartBitStream(2, 3, 5, 1, 4, 7, 0, 6);
            packet.ParseBitStream(guid, 5, 3, 7, 1, 4, 0, 6, 2);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_UNK_1D36)]
        public static void HandleUnk1D36(Packet packet)
        {
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.SMSG_UNK_000F)]
        public static void HandleSUnk000F(Packet packet)
        {
            var guid = packet.StartBitStream(3, 2, 6, 0, 4, 5, 7, 1);
            packet.ParseBitStream(guid, 6, 1, 5, 7, 3, 4, 2, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_UNK_001B)]
        public static void HandleSUnk001B(Packet packet)
        {
            packet.ReadBit("unk16");
            var unk20 = packet.ReadBits("unk20", 2);
            var unk52 = !packet.ReadBit("!unk52");
            if (unk20==0 && !packet.ReadBit("unk44"))
            {
                packet.ReadInt32("unk40"); // 40
                packet.ReadInt32("Suffix factor"); // 24
                packet.ReadInt32("unk36"); // 36
                packet.ReadInt32("unk32"); // 32
                packet.ReadEntry<UInt32>(StoreNameType.Item, "Entry"); // 48
                packet.ReadInt32("unk28"); // 28
            }
            packet.ReadInt32("unk56"); // 56
            if (unk52)
                packet.ReadByte("unk52"); // 52
            if (unk20 == 2)
                packet.ReadInt32("unk60"); // 60
        }

        [Parser(Opcode.SMSG_UNK_0354)]
        public static void HandleSUnk0354(Packet packet)
        {
            packet.ReadInt32("Unk");
        }

        [Parser(Opcode.SMSG_UNK_0364)]
        public static void HandleSUnk0364(Packet packet)
        {
            var count = packet.ReadBits("Count", 22);
            for (var i = 0; i < count; i++)
                packet.ReadInt32Visible("Unk20", i);
        }

        [Parser(Opcode.SMSG_UNK_0612)]
        public static void HandleSUnk0612(Packet packet)
        {
            packet.ReadInt64("unk16");
            packet.ReadInt32("unk28");
            packet.ReadInt32("unk24");
        }

        [Parser(Opcode.SMSG_UNK_0B81)]
        public static void HandleSUnk0B81(Packet packet)
        {
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.SMSG_UNK_0EAA)]
        public static void HandleSUnk0EAA(Packet packet)
        {
            var guid = new byte[8];
            var unk64 = packet.ReadBits("unk64", 3); // 64
            var unk57 = !packet.ReadBit("!unk57"); // 57
            var unk68 = packet.ReadBit("unk68"); // 68
            guid = packet.StartBitStream(3, 1, 7, 6, 2, 4, 5, 0);
            var unk56 = !packet.ReadBit("!unk56"); // 56
            var unk60 = packet.ReadBits("unk60", 2); // 60
            packet.ParseBitStream(guid, 7);
            packet.ReadInt32("unk52"); // 52
            packet.ParseBitStream(guid, 5);
            packet.ReadInt32("unk24"); // 24
            packet.ReadInt32("Suffix factor"); // 48
            if (unk57)
                packet.ReadByte("unk57");
            packet.ParseBitStream(guid, 4, 0, 3, 2);
            packet.ReadBytes("unk", packet.ReadInt32());
            packet.ReadInt32("Item"); // 36
            packet.ReadByte("unk29"); // 29
            packet.ReadInt32("unk44"); // 44
            packet.ReadByte("unk28"); // 28
            packet.ReadInt32("unk32"); // 32
            if (unk56)
                packet.ReadByte("unk56");
            packet.ParseBitStream(guid, 6);
            packet.ReadInt32("unk40"); // 40
            packet.ParseBitStream(guid, 1);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_UNK_1006)]
        public static void HandleSUnk1006(Packet packet)
        {
            var guid = packet.StartBitStream(4, 3, 5, 1, 2, 0, 6, 7);
            packet.ReadInt32("unk44"); // 44
            packet.ParseBitStream(guid, 0);
            packet.ReadInt32("unk28"); // 28
            packet.ParseBitStream(guid, 4);
            packet.ReadInt32("unk24"); // 24
            for (var i = 0; i < 3; i++)
            {
                packet.ReadInt32("unk56", i); // 56
                packet.ReadByte("unk68", i); // 68
            }
            packet.ReadInt32("unk48"); // 48
            packet.ReadInt32("unk40"); // 40
            packet.ParseBitStream(guid, 1);
            packet.ReadInt32("unk16"); // 16
            packet.ParseBitStream(guid, 7, 2);
            packet.ReadInt32("unk20"); // 20
            packet.ParseBitStream(guid, 5, 3, 6);

            packet.WriteGuid("Guid", guid);
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

        [Parser(Opcode.SMSG_UNK_14E2)]
        public static void HandleSUnk14E2(Packet packet)
        {
            var count = packet.ReadBits("Count", 19);
            var len = new uint[count];
            for (var i = 0; i < count; i++)
                len[i] = packet.ReadBits(8);
            for (var i = 0; i < count; i++)
            {
                packet.ReadInt32("unk68", i);
                packet.ReadInt64("unk20", i);
                packet.ReadInt32("unk84", i);
                packet.ReadInt32("unk52", i);
                packet.ReadWoWString("str", len[i], i);
            }
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
            packet.ReadEntry<Int32>(StoreNameType.Spell, "Spell ID"); // 20
            packet.ParseBitStream(guid, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_UNK_1904)]
        public static void HandleSUnk1904(Packet packet)
        {
            packet.ReadGuid("unk");
            packet.ReadInt32("unk4");
        }

        [Parser(Opcode.SMSG_UNK_1C3F)]
        public static void HandleSUnk1C3F(Packet packet)
        {
            var guid48 = new byte[8];
            guid48[5] = packet.ReadBit();
            var unk24 = packet.ReadBit("unk24"); // 24
            var guid16 = new byte[8];
            if (unk24)
                guid16 = packet.StartBitStream(4, 6, 0, 7, 5, 2, 3, 1);
            guid48[1] = packet.ReadBit();
            var unk40 = packet.ReadBit("unk40"); // 40
            guid48[4] = packet.ReadBit();
            guid48[3] = packet.ReadBit();
            guid48[2] = packet.ReadBit();
            var guid32 = new byte[8];
            if (unk40)
                guid32 = packet.StartBitStream(2, 3, 4, 5, 6, 0, 1, 7);
            guid48[7] = packet.ReadBit();
            guid48[0] = packet.ReadBit();
            guid48[6] = packet.ReadBit();
            if (unk40)
            {
                packet.ParseBitStream(guid32, 7, 1, 0, 6, 5, 3, 4, 2);
                packet.WriteGuid("guid32", guid32);
            }
            if (unk24)
            {
                packet.ParseBitStream(guid16, 4, 5, 6, 3, 2, 7, 0, 1);
                packet.WriteGuid("guid16", guid16);
            }
            packet.ParseBitStream(guid48, 5, 1, 6, 2, 3, 0, 7, 4);

            packet.WriteGuid("guid48", guid48);
        }

        [Parser(Opcode.SMSG_UNK_1DBE)]
        public static void HandleSUnk1DBE(Packet packet)
        {
            var guid48 = new byte[8];
            guid48[2] = packet.ReadBit();
            guid48[3] = packet.ReadBit();
            packet.ReadBits("unk40", 2); // 40
            guid48[7] = packet.ReadBit();
            guid48[5] = packet.ReadBit();
            guid48[0] = packet.ReadBit();
            guid48[1] = packet.ReadBit();
            guid48[6] = packet.ReadBit();
            guid48[4] = packet.ReadBit();

            packet.ParseBitStream(guid48, 6);
            packet.ReadSingle("unk24"); // 24
            packet.ParseBitStream(guid48, 4);
            packet.ReadSingle("unk28"); // 28
            packet.ReadInt32("unk44"); // 44
            packet.ReadInt32("unk16"); // 16
            packet.ParseBitStream(guid48, 5);
            packet.ReadSingle("unk36"); // 36
            packet.ParseBitStream(guid48, 0, 7, 1, 3, 2);
            packet.ReadInt32("unk32"); // 32
            packet.ReadSingle("unk20"); // 20

            packet.WriteGuid("Guid", guid48);
        }

        [Parser(Opcode.SMSG_CRITERIA_DELETED)]
        public static void HandleCriteriaDeleted(Packet packet)
        {
            packet.ReadInt32("CriteriaID");
        }

        [Parser(Opcode.SMSG_UNK_1E1B)]
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

        [Parser(Opcode.SMSG_UNK_1EAE)]
        public static void HandleSUnk1EAE(Packet packet)
        {
            packet.ReadBool("unk");
        }

        [Parser(Opcode.CMSG_NULL_0060)]
        [Parser(Opcode.CMSG_NULL_0082)]
        [Parser(Opcode.CMSG_NULL_0141)]
        [Parser(Opcode.CMSG_NULL_01C0)]
        [Parser(Opcode.CMSG_NULL_02D6)]
        [Parser(Opcode.CMSG_NULL_02DA)]
        [Parser(Opcode.CMSG_NULL_032D)]
        [Parser(Opcode.CMSG_NULL_033D)]
        [Parser(Opcode.CMSG_NULL_0360)]
        [Parser(Opcode.CMSG_NULL_0365)]
        [Parser(Opcode.CMSG_NULL_03C4)]
        [Parser(Opcode.CMSG_NULL_05E1)]
        [Parser(Opcode.CMSG_NULL_0644)]
        [Parser(Opcode.CMSG_NULL_06D4)]
        [Parser(Opcode.CMSG_NULL_06F5)]
        [Parser(Opcode.CMSG_NULL_0813)]
        [Parser(Opcode.CMSG_NULL_0826)]
        [Parser(Opcode.CMSG_NULL_0A23)]
        [Parser(Opcode.CMSG_NULL_0A82)]
        [Parser(Opcode.CMSG_NULL_0A87)]
        [Parser(Opcode.CMSG_NULL_0C62)]
        [Parser(Opcode.CMSG_NULL_1063)]
        [Parser(Opcode.CMSG_NULL_1124)]
        [Parser(Opcode.CMSG_NULL_1207)]
        [Parser(Opcode.CMSG_NULL_135B)]
        [Parser(Opcode.CMSG_NULL_1362)]
        [Parser(Opcode.CMSG_NULL_1452)]
        [Parser(Opcode.CMSG_NULL_147B)]
        [Parser(Opcode.CMSG_NULL_14DB)]
        [Parser(Opcode.CMSG_NULL_14E0)]
        [Parser(Opcode.CMSG_NULL_15A8)]
        [Parser(Opcode.CMSG_NULL_15E2)]
        [Parser(Opcode.CMSG_NULL_18A2)]
        [Parser(Opcode.CMSG_NULL_1A23)]
        [Parser(Opcode.CMSG_NULL_1A87)]
        [Parser(Opcode.CMSG_NULL_1C45)]
        [Parser(Opcode.CMSG_NULL_1C5A)]
        [Parser(Opcode.CMSG_NULL_1D61)]
        [Parser(Opcode.CMSG_NULL_1DC3)]
        [Parser(Opcode.CMSG_NULL_1F34)]
        [Parser(Opcode.CMSG_NULL_1F89)]
        [Parser(Opcode.CMSG_NULL_1F8E)]
        [Parser(Opcode.CMSG_NULL_1F9E)]
        [Parser(Opcode.CMSG_NULL_1F9F)]
        [Parser(Opcode.SMSG_NULL_0C59)]
        [Parser(Opcode.SMSG_NULL_0C9A)]
        [Parser(Opcode.SMSG_NULL_0E2B)]
        [Parser(Opcode.SMSG_NULL_0E8B)]
        [Parser(Opcode.SMSG_NULL_0FE1)]
        [Parser(Opcode.SMSG_NULL_1313)]
        public static void HandleUnkNull(Packet packet)
        {
        }
    }
}
