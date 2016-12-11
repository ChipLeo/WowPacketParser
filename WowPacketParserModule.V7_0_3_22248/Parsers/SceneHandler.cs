using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class SceneHandler
    {
        [Parser(Opcode.SMSG_SCENARIO_STATE)]
        public static void HandleScenarioState(Packet packet)
        {
            packet.ReadInt32("ScenarioID");
            packet.ReadInt32("CurrentStep");
            packet.ReadInt32("DifficultyID");
            packet.ReadInt32("WaveCurrent");
            packet.ReadInt32("WaveMax");
            packet.ReadInt32("TimerDuration");

            var cnt44 = packet.ReadInt32("cnt44");
            var cnt60 = packet.ReadInt32("cnt60");

            var cnt76 = packet.ReadInt32("cnt76");
            var cnt92 = packet.ReadInt32("cnt92");

            for (int i = 0; i < cnt76; i++) //651881 22996
                packet.ReadInt32("Steps", i);

            packet.ResetBitReader();

            packet.ReadBit("ScenarioComplete");

            for (int i = 0; i < cnt44; i++) //6501A6 22996
            {
                packet.ReadInt32("unk1", i);
                packet.ReadInt64("unk2", i);
                packet.ReadPackedGuid128("Guid", i);
                packet.ReadPackedTime("Time", i);
                packet.ReadInt32("unk3", i);
                packet.ReadInt32("unk4", i);

                packet.ResetBitReader();
                packet.ReadBits("unk5", 4, i);
            }

            for (int i = 0; i < cnt60; i++) //64E96C 22996
            {
                packet.ReadInt32("BonusObjectiveID", i);

                packet.ResetBitReader();
                packet.ReadBit("ObjectiveComplete", i);
            }

            for (int i = 0; i < cnt92; i++) //64E96C 22996
            {
                packet.ReadInt32("BonusObjectiveID", i);

                packet.ResetBitReader();
                packet.ReadBit("ObjectiveComplete", i);
            }
        }
    }
}
