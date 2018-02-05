using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class ScenarioHandler
    {
        [Parser(Opcode.CMSG_SCENE_PLAYBACK_COMPLETE)]
        public static void HandleMiscScene(Packet packet)
        {
            var val = packet.ReadBit("unkb");
            if (!val)
                packet.ReadInt32("unk");
        }

        [Parser(Opcode.CMSG_SCENE_TRIGGER_EVENT)]
        public static void HandleSceneTriggerEvent(Packet packet)
        {
            packet.ReadBit("unkb");
            packet.ReadWoWString("Event", packet.ReadBits(6));
            packet.ReadInt32("unk");
        }
    }
}
