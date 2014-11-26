using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class ArchaeologyHandler
    {
        [Parser(Opcode.SMSG_ARCHAEOLOGY_SURVERY_CAST)]
        public static void HandleArchaeologySyrveryCast(Packet packet)
        {
            packet.ReadInt32("unk24"); // 24
            packet.ReadInt32("unk20"); // 20
            packet.ReadInt32("unk16"); // 16
            packet.ReadBit("unk28"); // 28
        }
    }
}
