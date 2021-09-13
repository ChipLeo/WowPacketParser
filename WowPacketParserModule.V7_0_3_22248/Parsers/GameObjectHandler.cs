using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class GameObjectHandler
    {
        [Parser(Opcode.SMSG_GAME_OBJECT_SET_STATE_LOCAL)]
        public static void HandleGameObjectSetStateLocal(Packet packet)
        {
            packet.ReadPackedGuid128("GUID");
            packet.ReadByte("State");
        }

        [Parser(Opcode.SMSG_GAME_OBJECT_UI_ACTION)]
        public static void HandleGameObjectUIAction(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
            packet.ReadInt32("Action");
        }
    }
}
