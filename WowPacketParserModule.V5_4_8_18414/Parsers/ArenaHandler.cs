using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class ArenaHandler
    {
        [Parser(Opcode.SMSG_PVP_SEASON)]
        public static void HandlePvPSeason(Packet packet)
        {
            packet.ReadUInt32("Last Season");
            packet.ReadUInt32("Active Season");
        }

        [Parser(Opcode.SMSG_DESTROY_ARENA_UNIT)]
        public static void HandleDestroyArenaUnit(Packet packet)
        {
            var count = packet.ReadBits("count", 21);
            var guid = new byte[count][];
            for (var i = 0; i < count; i++)
                guid[i] = packet.StartBitStream(7, 1, 2, 0, 6, 5, 4, 3);
            for (var i = 0; i < count; i++)
            {
                packet.ParseBitStream(guid[i], 0, 7);
                packet.ReadInt32("Level", i);
                packet.ParseBitStream(guid[i], 2, 5, 3, 6, 1, 4);
                packet.WriteGuid("Guid", guid[i], i);
            }
        }
    }
}