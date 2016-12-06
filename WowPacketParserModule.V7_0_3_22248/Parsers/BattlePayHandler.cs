using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class BattlePayHandler
    {
        [Parser(Opcode.CMSG_BATTLE_PAY_GET_PRODUCT_LIST)]
        [Parser(Opcode.CMSG_BATTLE_PAY_GET_PURCHASE_LIST)]
        [Parser(Opcode.CMSG_UPDATE_VAS_PURCHASE_STATES)]
        public static void HandleZeroLengthPackets(Packet packet)
        {
        }

        //65C08C 22996
        private static void ReadBattlepayDisplayInfo(Packet packet, params object[] idx)
        {
            packet.ResetBitReader();

            var bit4 = packet.ReadBit("HasCreatureDisplayInfoID", idx);
            var bit12 = packet.ReadBit("HasFileDataID", idx);

            var bits16 = packet.ReadBits(10);
            var bits529 = packet.ReadBits(10);
            var bits1042 = packet.ReadBits(13);

            var bit5144 = packet.ReadBit("HasFlags", idx);

            var bit5152 = packet.ReadBit("unk5152", idx);

            var bit5160 = packet.ReadBit("unk5160", idx);

            var bit5168 = packet.ReadBit("unk5168", idx);

            if (bit4)
                packet.ReadInt32("CreatureDisplayInfoID", idx);

            if (bit12)
                packet.ReadInt32("FileDataID", idx);

            packet.ReadWoWString("Name1", bits16, idx);
            packet.ReadWoWString("Name2", bits529, idx);
            packet.ReadWoWString("Name3", bits1042, idx);

            if (bit5144)
                packet.ReadInt32("Flags", idx);

            if (bit5152)
                packet.ReadInt32("unk5152", idx);

            if (bit5160)
                packet.ReadInt32("unk5160", idx);

            if (bit5168)
                packet.ReadInt32("unk5168", idx);
        }

        [Parser(Opcode.SMSG_BATTLE_PAY_GET_PRODUCT_LIST_RESPONSE)]
        public static void HandletBattlePayGetProductListResponse(Packet packet)
        {
            packet.ReadUInt32("Result");
            packet.ReadUInt32("CurrencyID");

            var int24 = packet.ReadUInt32("int24Count");
            var int40 = packet.ReadUInt32("int40Count");
            var int56 = packet.ReadUInt32("int56Count");
            var int72 = packet.ReadUInt32("int72Count");

            //sub_65BD92 22996
            for (int i = 0; i < int24; ++i)
            {
                packet.ReadInt32("unk1", i);
                packet.ReadInt64("unk2", i);
                packet.ReadInt64("unk3", i);
                var int24i = packet.ReadInt32("int24i", i);
                packet.ReadInt32("unk5", i);
                for (int j = 0; j < int24i; ++j)
                    packet.ReadInt32("unk6", j, i);
                packet.ResetBitReader();
                packet.ReadBits("unk7", 7, i);
                var bit5220 = packet.ReadBit("bit5220", i);

                if (bit5220)
                    ReadBattlepayDisplayInfo(packet, i);
            }

            //sub_65BA82 22996
            for (int i = 0; i < int40; ++i)
            {
                packet.ReadInt32("unk8", i);
                packet.ReadByte("unk9", i);
                packet.ReadInt32("unk10", i);
                packet.ReadInt32("unk11", i);
                packet.ReadInt32("unk12", i);
                packet.ReadInt32("unk13", i);
                packet.ReadInt32("unk14", i);

                packet.ResetBitReader();
                packet.ReadBit("unk15", i);
                var bit32 = packet.ReadBit("unk16", i);

                var bits40 = packet.ReadBits("unk17", 7, i);

                var bit5228 = packet.ReadBit("unk18", i);

                if (bit32)
                    packet.ReadBits("unk19", 4, i);

                for (int j = 0; i < bits40; ++i)
                {
                    packet.ReadInt32("unk20", j, i);
                    packet.ReadByte("unk21", j, i);
                    packet.ReadInt32("unk22", j, i);
                    packet.ReadInt32("unk23", j, i);
                    packet.ReadInt32("unk24", j, i);
                    packet.ReadInt32("unk25", j, i);

                    packet.ResetBitReader();
                    packet.ReadBit("unk26", j, i);
                    var bit32j = packet.ReadBit("unk32j", j, i);

                    var bit5208 = packet.ReadBit("unk28", j, i);

                    if (bit32j)
                        packet.ReadBits("unk29", 4, j, i);

                    if (bit5208)
                        ReadBattlepayDisplayInfo(packet, j, i);
                }

                if (bit5228)
                    ReadBattlepayDisplayInfo(packet, i);
            }

            //sub_65BE65 22996
            for (int i = 0; i < int56; ++i)
            {
                packet.ReadInt32("unk30", i);
                packet.ReadInt32("unk31", i);
                packet.ReadByte("unk32", i);
                packet.ReadInt32("unk33", i);
                packet.ResetBitReader();
                packet.ReadWoWString("str34", packet.ReadBits(8), i);
            }

            //sub_65BF92 22996
            for (int i = 0; i < int72; ++i)
            {
                packet.ReadInt32("unk40", i);
                packet.ReadInt32("unk41", i);
                packet.ReadInt32("unk42", i);
                packet.ReadInt32("unk43", i);
                packet.ReadInt32("unk44", i);
                packet.ReadByte("unk45", i);
                packet.ResetBitReader();
                if (packet.ReadBit())
                    ReadBattlepayDisplayInfo(packet, i);
            }
        }
    }
}
