using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class VoiceChatHandler
    {
        [Parser(Opcode.CMSG_VOICE_SESSION_ENABLE)]
        public static void HandleVoiceSessionEnable(Packet packet)
        {
            packet.ReadBit("Voice Enabled"); // 16
            packet.ReadBit("Microphone Enabled"); // 17
        }
    }
}
