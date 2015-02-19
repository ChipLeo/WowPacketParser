using System;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using WowPacketParserModule.V5_4_8_18414.Enums;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class TalentHandler
    {
        [Parser(Opcode.CMSG_LEARN_TALENT)]
        public static void HandleLearnTalent(Packet packet)
        {
            var talentCount = packet.ReadBits("Talent Count", 23);

            for (int i = 0; i < talentCount; i++)
                packet.ReadUInt16("Talent Id", i);
        }

        [Parser(Opcode.CMSG_SET_PRIMARY_TALENT_TREE)]
        public static void HandleSetPrimaryTalentTreeSpec(Packet packet)
        {
            packet.ReadUInt32("Spec Tab Id");
        }

        [Parser(Opcode.SMSG_INSPECT_TALENT)]
        public static void HandleSInspectTalent(Packet packet)
        {
            var guid = new byte[8];
            var hasGuild = packet.ReadBit("hasGuild"); // 40
            guid[2] = packet.ReadBit();
            var guid40 = new byte[8];
            if (hasGuild)
                guid40 = packet.StartBitStream(7, 0, 5, 3, 2, 4, 6, 1);
            guid[4] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var slotCount = packet.ReadBits("slotCount", 20); // 64
            guid[0] = packet.ReadBit();
            var guid64 = new byte[slotCount][];
            var unk82 = new bool[slotCount];
            var enchCount = new uint[slotCount];
            var unk88 = new bool[slotCount];
            for (var i = 0; i < slotCount; i++)
            {
                guid64[i] = new byte[8];
                guid64[i][1] = packet.ReadBit();
                unk88[i] = packet.ReadBit("unk88", i); // 88
                packet.ReadBit("unk112", i); // 112
                guid64[i][3] = packet.ReadBit();
                enchCount[i] = packet.ReadBits("enchCount", 21, i); // 96
                guid64[i][2] = packet.ReadBit();
                guid64[i][6] = packet.ReadBit();
                guid64[i][4] = packet.ReadBit();
                unk82[i] = packet.ReadBit("unk82", i); // 82
                guid64[i][0] = packet.ReadBit();
                guid64[i][5] = packet.ReadBit();
                guid64[i][7] = packet.ReadBit();
            }
            var glyphCount = packet.ReadBits("glyphCount", 23); // 48
            var talentCount = packet.ReadBits("talentCount", 23); // 96
            guid[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();

            packet.ParseBitStream(guid, 1, 4, 2);
            var unkv42 = 0;
            for (var i = 0; i < slotCount; i++)
            {
                if (unk82[i])
                    packet.ReadInt16("unk80", i); // 80

                packet.ParseBitStream(guid64[i], 3);

                unkv42 = packet.ReadInt32("unkv42", i); // 42
                packet.WriteLine("[{0}] {1}", i, Utilities.ByteArrayToHexString(packet.ReadBytes(unkv42)));
                for (var j = 0; j < enchCount[i]; j++)
                {
                    packet.ReadInt32("unk100", i, j); // 100
                    packet.ReadByte("unk104", i, j); // 104
                }
                packet.ReadEntry<UInt32>(StoreNameType.Item, "Item Entry", i); // 76
                packet.ParseBitStream(guid64[i], 6, 4, 7, 2);
                if (unk88[i])
                    packet.ReadInt32("unk84", i); // 84
                packet.ParseBitStream(guid64[i], 5);
                packet.ReadByteE<EquipmentSlotType>("Slot", i); // 92
                packet.ParseBitStream(guid64[i], 0, 1);
                packet.WriteGuid("Creator GUID", guid64[i], i); // 64
            }
            if (hasGuild)
            {
                packet.ParseBitStream(guid40, 6, 2, 5, 0);
                packet.ReadInt32("Guild Members"); // 36
                packet.ParseBitStream(guid40, 4, 7);
                packet.ReadInt64("Guild Xp"); // 24
                packet.ParseBitStream(guid40, 1);
                packet.ReadInt32("Guild Level"); // 32
                packet.ParseBitStream(guid40, 3);
                packet.WriteGuid("Guild Guid", guid40);
            }

            packet.ParseBitStream(guid, 5);

            for (var i = 0; i < glyphCount; i++)
                packet.ReadInt16("GlyphID", i); // 52

            packet.ParseBitStream(guid, 0);
            packet.ReadInt32("unk80");

            for (var i = 0; i < talentCount; i++)
                packet.ReadInt16("TalentID", i); // 100

            packet.ParseBitStream(guid, 7, 3, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_TALENTS_INFO)]
        public static void ReadTalentInfo(Packet packet)
        {
            packet.ReadByte("Active Spec Group");
            var specCount = packet.ReadBits("Spec Group count", 19);
            var wpos = new UInt32[specCount];
            for (var i = 0; i < specCount; ++i)
            {
                wpos[i] = packet.ReadBits("TalentCount", 23, i);
            }

            for (var i = 0; i < specCount; ++i)
            {
                for (var j = 0; j < 6; ++j)
                    packet.ReadUInt16("Glyph", i, j);

                for (var j = 0; j < wpos[i]; ++j)
                    packet.ReadUInt16("TalentID", i, j);

                packet.ReadUInt32("Spec Id", i);
            }
        }
    }
}
