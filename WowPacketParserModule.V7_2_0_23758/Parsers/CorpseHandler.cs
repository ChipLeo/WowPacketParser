using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class CorpseHandler
    {
        [Parser(Opcode.CMSG_QUERY_CORPSE_LOCATION_FROM_CLIENT)]
        public static void HandleCorpseQueryLocation(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
        }
        [Parser(Opcode.SMSG_CORPSE_LOCATION)]
        public static void HandleCorpseLocation(Packet packet)
        {
            packet.ReadBit("Valid");
            packet.ReadPackedGuid128("Guid");
            packet.ReadInt32("ActualMapID");
            packet.ReadVector3("Position");
            packet.ReadInt32("MapID");
            packet.ReadPackedGuid128("Transport");
        }
    }
}
