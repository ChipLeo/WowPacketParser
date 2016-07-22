using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V6_0_2_19033.Parsers
{
    public static class BattleNetHandler
    {
        [Parser(Opcode.CMSG_BATTLENET_REQUEST_REALM_LIST_TICKET)]
        public static void HandleBattleNetRequestRealmListTicket(Packet packet)
        {
            packet.ReadUInt32("unk");

            for (var i = 0; i < 32; ++i)
            {
                packet.ReadByte("unkb", i);
            }
        }
    }
}
