using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class EquipmentSetHandler
    {
        private const int NumSlots = 19;

        [Parser(Opcode.SMSG_EQUIPMENT_SET_LIST)]
        public static void HandleEquipmentSetList(Packet packet)
        {
            var count = packet.ReadBits("count", 19);
            var guid = new byte[count][];
            var guid2 = new byte[count][][];
            var len68 = new uint[count];
            var len580 = new uint[count];
            for (var i = 0; i < count; i++)
            {
                guid[i] = new byte[8];

                guid[i][4] = packet.ReadBit("unk", i);
                guid2[i] = new byte[20][];
                for (var j = 0; j < NumSlots; j++)
                {
                    guid2[i][j] = new byte[8];
                    guid2[i][j] = packet.StartBitStream(3, 5, 7, 2, 6, 0, 4, 1);
                }
                guid[i][5] = packet.ReadBit();
                len580[i] = packet.ReadBits("unk580", 9, i);
                guid[i][1] = packet.ReadBit();
                guid[i][7] = packet.ReadBit();
                len68[i] = packet.ReadBits("unk68", 8, i);
                guid[i][3] = packet.ReadBit();
                guid[i][2] = packet.ReadBit();
                guid[i][6] = packet.ReadBit();
                guid[i][0] = packet.ReadBit();
            }
            for (var i = 0; i < count; i++)
            {
                for (var j = 0; j < 19; j++)
                {
                    packet.ParseBitStream(guid2[i][j], 2, 3, 7, 1, 6, 5, 0, 4);
                    packet.WriteGuid("Item GUID", guid2[i][j], i, j);
                }
                packet.ParseBitStream(guid[i], 7);
                packet.ReadInt32("Index", i);
                packet.ReadWoWString("Set Name", len68[i], i);
                packet.ParseBitStream(guid[i], 2, 6, 0, 3, 1, 5, 4);
                packet.ReadWoWString("Set Icon", len580[i], i);
                packet.WriteGuid("Guid", guid[i], i);
            }
        }

        [Parser(Opcode.CMSG_EQUIPMENT_SET_DELETE)]
        public static void HandleEquipmentSetDelete(Packet packet)
        {
            var guid = packet.StartBitStream(4, 2, 6, 0, 5, 1, 7, 3);
            packet.ParseBitStream(guid, 2, 0, 1, 6, 3, 4, 5, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_EQUIPMENT_SET_SAVE)]
        public static void HandleEquipmentSetSave(Packet packet)
        {
            var guid1 = new byte[NumSlots][];
            var guid = new byte[8];

            packet.ReadInt32("Index");

            for (var i = 0; i < NumSlots; i++)
                guid1[i] = packet.StartBitStream(5, 0, 1, 4, 6, 3, 7, 2);

            guid[7] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[0] = packet.ReadBit();

            var bits0 = packet.ReadBits(8);
            guid[6] = packet.ReadBit();
            var bits4 = packet.ReadBits(9);

            guid[4] = packet.ReadBit();

            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 0);

            for (var i = 0; i < NumSlots; i++)
            {
                packet.ParseBitStream(guid1[i], 1, 0, 7, 3, 6, 2, 4, 5);
                packet.WriteGuid("Item GUID", guid1[i], i);
            }

            packet.ReadWoWString("Set Icon", bits4);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            packet.ReadWoWString("Set Name", bits0);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 3);

            packet.WriteGuid("GUID", guid);
        }

        [Parser(Opcode.CMSG_EQUIPMENT_SET_USE)]
        public static void HandleEquipmentSetUse(Packet packet)
        {

            var itemGuids = new byte[NumSlots][];
            var slotsInfo = new byte[NumSlots][];

            for (var i = 0; i < NumSlots; i++)
            {
                slotsInfo[i] = new byte[2];
                slotsInfo[i][1] = packet.ReadByte();
                slotsInfo[i][0] = packet.ReadByte();
            }

            for (var i = 0; i < NumSlots; i++)
                itemGuids[i] = packet.StartBitStream(3, 1, 7, 4, 5, 6, 0, 2);

            var someCount = packet.ReadBits("Some count", 2);

            var unk1 = new byte[someCount];
            var unk2 = new byte[someCount];
            for (var i = 0; i < someCount; i++)
            {
                unk1[i] = packet.ReadBit();
                unk2[i] = packet.ReadBit();
            }

            for (var i = 0; i < NumSlots; ++i)
            {
                packet.ParseBitStream(itemGuids[i], 4, 7, 0, 3, 2, 5, 1, 6);
                packet.WriteGuid("ItemGUID", itemGuids[i], i);
                packet.AddValue("Source bag", slotsInfo[i][0], i);
                packet.AddValue("Source slot", slotsInfo[i][1], i);
            }

            for (var i = 0; i < someCount; i++)
            {
                if (unk1[i]>0)
                    unk1[i] = packet.ReadByte();
                if (unk2[i]>0)
                    unk2[i] = packet.ReadByte();
                packet.AddValue("Unk", "byte 1 " + unk1[i] + " byte 2" + unk2[i], i);
            }
        }
    }
}
