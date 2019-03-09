using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class ReputationHandler
    {
        [Parser(Opcode.CMSG_REQUEST_FORCED_REACTIONS)]
        public static void HandleReputationZero(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_INITIALIZE_FACTIONS)]
        public static void HandleInitializeFactions(Packet packet)
        {
            for (var i = 0; i < 300; i++)
            {
                packet.ReadByteE<FactionFlag>("FactionFlags", i);
                packet.ReadUInt32E<ReputationRank>("FactionStandings", i);
            }

            for (var i = 0; i < 300; i++)
                packet.ReadBit("FactionHasBonus", i);
        }

        [Parser(Opcode.SMSG_SET_FACTION_STANDING)]
        public static void HandleSetFactionStanding(Packet packet)
        {
            packet.ReadSingle("BonusFromAchievementSystem");
            packet.ReadSingle("ReferAFriendBonus");

            var count = packet.ReadInt32("");
            for (int i = 0; i < count; i++)
            {
                packet.ReadInt32("Index");
                packet.ReadInt32("Standing");
            }

            packet.ResetBitReader();
            packet.ReadBit("ShowVisual");
        }

        [Parser(Opcode.SMSG_SET_FORCED_REACTIONS)]
        public static void HandleForcedReactions(Packet packet)
        {
            var counter = packet.ReadUInt32("ForcedReactionCount");

            for (var i = 0; i < counter; i++)
            {
                packet.ReadUInt32("Faction");
                packet.ReadUInt32("Reaction");
            }
        }
    }
}
