using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class BattlePayHandler
    {
        [Parser(Opcode.CMSG_BATTLE_PAY_START_PURCHASE)]
        public static void HandleBattlePayStartPurchase(Packet packet)
        {
            packet.ReadInt32("ProductID");   // 28
            packet.ReadInt32("ClientToken"); // 24
            var guid = packet.StartBitStream(2, 3, 5, 1, 4, 7, 0, 6);
            packet.ParseBitStream(guid, 5, 3, 7, 1, 4, 0, 6, 2);
            packet.WriteGuid("TargetCharacter", guid);
        }

        [Parser(Opcode.SMSG_BATTLE_PAY_ACK_FAILED)]
        public static void HandleBattlePayAckFailed(Packet packet)
        {
            packet.ReadInt32("Int32");
            packet.ReadInt32("Int28");
            packet.ReadInt32("Int24");
            packet.ReadInt64("QW16");
        }

        [Parser(Opcode.SMSG_BATTLE_PAY_DISTRIBUTION_UPDATE)]
        public static void HandleBattlePayDistributionUpdate(Packet packet)
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

        [Parser(Opcode.SMSG_BATTLE_PAY_PURCHASE_UPDATE)]
        public static void HandleBattlePayPurchaseUpdate(Packet packet)
        {
            var battlePayPurchaseCount = packet.ReadBits("BattlePayPurchaseCount", 19);
            var len = new uint[battlePayPurchaseCount];
            for (var i = 0; i < battlePayPurchaseCount; i++)
                len[i] = packet.ReadBits(8);
            for (var i = 0; i < battlePayPurchaseCount; i++)
            {
                packet.ReadInt32("unk68", i); // 68
                packet.ReadInt64("PurchaseID", i); // 20
                packet.ReadInt32("unk84", i); // 84
                packet.ReadInt32("unk52", i); // 52
                packet.ReadWoWString("WalletName", len[i], i);
            }
        }

        [Parser(Opcode.SMSG_BATTLE_PAY_GET_DISTRIBUTION_LIST_RESPONSE)]
        public static void HandleBattlePayGetDistributionListResponse(Packet packet)
        {
            packet.ReadUInt32("Result");

            var int16 = packet.ReadBits("BattlePayDistributionObjectCount", 19);

            var guid20 = new byte[int16][];
            var guid36 = new byte[int16][];
            var has5268 = new Bit[int16];
            var unk276 = new uint[int16];
            var has5168 = new Bit[int16][];
            var len549 = new uint[int16][];
            var has16 = new Bit[int16][];
            var has24 = new Bit[int16][];
            var has32 = new Bit[int16][];
            var len36 = new uint[int16][];
            var has5164 = new Bit[int16][];
            var has5180 = new Bit[int16][];
            var len1062 = new uint[int16][];
            var has5260 = new Bit[int16];
            var has5264 = new Bit[int16];
            var len645 = new uint[int16];
            var has120 = new Bit[int16];
            var has112 = new Bit[int16];
            var len132 = new uint[int16];
            var has128 = new Bit[int16];
            var len1138 = new uint[int16];

            for (uint index = 0; index < int16; index++)
            {
                guid20[index] = new byte[8];
                guid36[index] = new byte[8];
                packet.StartBitStream(guid20[index], 6, 7, 5, 0);
                guid36[index][2] = packet.ReadBit();
                guid20[index][4] = packet.ReadBit();
                packet.ReadBit("unk5276", index);//5276
                guid20[index][2] = packet.ReadBit();
                guid36[index][7] = packet.ReadBit();
                guid36[index][3] = packet.ReadBit();
                guid20[index][3] = packet.ReadBit();
                has5268[index] = packet.ReadBit("unk5268", index);//5268
                guid36[index][0] = packet.ReadBit();
                if (has5268[index])
                {
                    unk276[index] = packet.ReadBits("unk276", 20, index);
                    has5168[index] = new Bit[unk276[index]];
                    len549[index] = new uint[unk276[index]];
                    has32[index] = new Bit[unk276[index]];
                    has24[index] = new Bit[unk276[index]];
                    len1062[index] = new uint[unk276[index]];
                    len36[index] = new uint[unk276[index]];
                    has5164[index] = new Bit[unk276[index]];
                    has16[index] = new Bit[unk276[index]];
                    has5180[index] = new Bit[unk276[index]];
                    for (var j = 0; j < unk276[index]; ++j)
                    {
                        packet.ReadBit("unk5172", index);
                        has5168[index][j] = packet.ReadBit("unk5168", index);
                        if (has5168[index][j])
                        {
                            len549[index][j] = packet.ReadBits("unk549", 10, index);
                            has32[index][j] = packet.ReadBit("unk32", index);//+73*4
                            has24[index][j] = packet.ReadBit("unk24", index);
                            len1062[index][j] = packet.ReadBits("unk1062", 13, index);
                            len36[index][j] = packet.ReadBits("unk36", 10, index);
                            has5164[index][j] = packet.ReadBit("unk5164", index);
                            has16[index][j] = packet.ReadBit("unk16", index);
                        }
                        has5180[index][j] = packet.ReadBit("unk5180", index);
                        if (has5180[index][j])
                            packet.ReadBits("unk5176", 4, index);
                        packet.ReadBit("unk5184", index);
                    }
                    packet.ReadBits("unk100", 2, index);
                    has5264[index] = packet.ReadBit("unk5264", index);
                    if (has5264[index])
                    {
                        has120[index] = packet.ReadBit("unk120", index);
                        len132[index] = packet.ReadBits("unk132", 10, index);
                        has128[index] = packet.ReadBit("unk128", index);
                        len1138[index] = packet.ReadBits("unk1138", 13, index);
                        has112[index] = packet.ReadBit("unk112", index);
                        len645[index] = packet.ReadBits("unk645", 10, index);
                        has5260[index] = packet.ReadBit("unk5260", index);
                    }
                }
                guid36[index][1] = packet.ReadBit();
                guid20[index][1] = packet.ReadBit();
                guid36[index][4] = packet.ReadBit();
                guid36[index][6] = packet.ReadBit();
                guid36[index][5] = packet.ReadBit();
            }

            for (uint index = 0; index < int16; index++)
            {
                packet.ParseBitStream(guid36[index], 6);
                packet.ReadInt64("unk32", index);
                if (has5268[index])
                {
                    for (var j = 0; j < unk276[index]; ++j)
                    {
                        packet.ReadInt32("unk88", index);
                        if (has5168[index][j])
                        {
                            packet.ReadWoWString("unk549", len549[index][j], index);
                            if (has24[index][j])
                                packet.ReadInt32("unk20", index);
                            if (has5164[index][j])
                                packet.ReadInt32("unk5160", index);
                            if (has32[index][j])
                                packet.ReadInt32("unk28", index);
                            if (has16[index][j])
                                packet.ReadInt32("unk12", index);
                            packet.ReadWoWString("str36", len36[index][j], index);
                            packet.ReadWoWString("str1062", len1062[index][j], index);
                        }
                        packet.ReadInt32("unk296", index);
                        packet.ReadInt32("unk292", index);
                    }
                    packet.ReadInt32("unk356", index);
                    if (has5264[index])
                    {
                        if (has5260[index])
                            packet.ReadInt32("unk5256", index);
                        packet.ReadWoWString("unk645", len645[index], index);
                        if (has120[index])
                            packet.ReadInt32("unk116", index);
                        if (has112[index])
                            packet.ReadInt32("unk108", index);
                        packet.ReadWoWString("unk132", len132[index], index);
                        if (has128[index])
                            packet.ReadInt32("unk124", index);
                        packet.ReadWoWString("unk1138", len1138[index], index);
                    }
                    packet.ReadByte("unk101", index);
                    packet.ReadInt64("unk56", index);
                    packet.ReadInt32("unk60", index);
                    packet.ReadInt64("unk68", index);
                }
                packet.ParseBitStream(guid36[index], 2);
                packet.ParseBitStream(guid20[index], 5, 2);
                packet.ParseBitStream(guid36[index], 1);
                packet.ReadInt32("unk48", index);
                packet.ParseBitStream(guid36[index], 5);
                packet.ReadInt32("unk52", index);
                packet.ParseBitStream(guid20[index], 1);
                packet.ParseBitStream(guid36[index], 4);
                packet.ReadInt32("unk116", index);
                packet.ParseBitStream(guid20[index], 6, 5);
                packet.ReadInt32("unk68", index);
                packet.ParseBitStream(guid20[index], 7, 4);
                packet.ParseBitStream(guid36[index], 7, 0, 3);
                packet.ParseBitStream(guid20[index], 3);

                packet.WriteGuid("guid20", guid20[index], index);
                packet.WriteGuid("guid36", guid36[index], index);
            }
        }

        [Parser(Opcode.SMSG_BATTLE_PAY_START_PURCHASE_RESPONSE)]
        public static void HandleBattlePayStartPurchaseResponse(Packet packet)
        {
            packet.ReadUInt64("PurchaseID");
            packet.ReadInt32("PurchaseResult"); //28
            packet.ReadInt32("ClientToken");    //24
        }
    }
}
