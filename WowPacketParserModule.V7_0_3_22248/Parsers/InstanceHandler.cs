using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class InstanceHandler
    {
        public static void sub_650A48(Packet packet, params object[] idx)
        {
            packet.ReadInt32("unk", idx);
            packet.ReadInt32("unk4", idx);
            var unk8 = packet.ReadInt32("unk8", idx);
            var unk24 = packet.ReadInt32("unk24", idx);
            var unk40 = packet.ReadInt32("unk40", idx);
            for (var i = 0; i < unk8; ++i)
                packet.ReadInt32("unk12", idx, i);
            for (var i = 0; i < unk24; ++i)
                packet.ReadInt32("unk28", idx, i);
            for (var i = 0; i < unk40; ++i)
                sub_650A48(packet, idx, i);
        }

        [Parser(Opcode.SMSG_RAID_INSTANCE_MESSAGE)]
        public static void HandleRaidInstanceMessage(Packet packet)
        {
            packet.ReadByte("Type");

            packet.ReadUInt32<MapId>("MapID");
            packet.ReadUInt32("DifficultyID");

            packet.ReadBit("Locked");
            packet.ReadBit("Extended");
        }

        [Parser(Opcode.CMSG_SAVE_CUF_PROFILES)]
        [Parser(Opcode.SMSG_LOAD_CUF_PROFILES)]
        public static void HandleCUFProfiles(Packet packet)
        {
            var count = packet.ReadUInt32("Count");

            for (int i = 0; i < count; ++i)
            {
                var strlen = packet.ReadBits(7);

                packet.ReadBit("KeepGroupsTogether", i);
                packet.ReadBit("DisplayPets", i);
                packet.ReadBit("DisplayMainTankAndAssist", i);
                packet.ReadBit("DisplayHealPrediction", i);
                packet.ReadBit("DisplayAggroHighlight", i);
                packet.ReadBit("DisplayOnlyDispellableDebuffs", i);
                packet.ReadBit("DisplayPowerBar", i);
                packet.ReadBit("DisplayBorder", i);
                packet.ReadBit("UseClassColors", i);
                packet.ReadBit("HorizontalGroups", i);
                packet.ReadBit("DisplayNonBossDebuffs", i);
                packet.ReadBit("DynamicPosition", i);
                packet.ReadBit("Locked", i);
                packet.ReadBit("Shown", i);
                packet.ReadBit("AutoActivate2Players", i);
                packet.ReadBit("AutoActivate3Players", i);
                packet.ReadBit("AutoActivate5Players", i);
                packet.ReadBit("AutoActivate10Players", i);
                packet.ReadBit("AutoActivate15Players", i);
                packet.ReadBit("AutoActivate25Players", i);
                packet.ReadBit("AutoActivate40Players", i);
                packet.ReadBit("AutoActivateSpec1", i);
                packet.ReadBit("AutoActivateSpec2", i);
                packet.ReadBit("AutoActivatePvP", i);
                packet.ReadBit("AutoActivatePvE", i);
                packet.ReadBit("unk1", i);
                packet.ReadBit("unk2", i);

                packet.ReadInt16("FrameHeight", i);
                packet.ReadInt16("FrameWidth", i);

                packet.ReadByte("SortBy", i);
                packet.ReadByte("HealthText", i);
                packet.ReadByte("TopPoint", i);
                packet.ReadByte("BottomPoint", i);
                packet.ReadByte("LeftPoint", i);

                packet.ReadInt16("TopOffset", i);
                packet.ReadInt16("BottomOffset", i);
                packet.ReadInt16("LeftOffset", i);

                packet.ReadWoWString("Name", strlen, i);
            }
        }

        [Parser(Opcode.SMSG_ENCOUNTER_START)]
        public static void HandleEncounterStart(Packet packet)
        {
            packet.ReadInt32("EncounterID");
            packet.ReadInt32("DifficultyID");
            packet.ReadInt32("GroupSize");
            var cnt = packet.ReadInt32("Count");
            for (var k = 0; k < cnt; ++k)
            {//64E732 22996
                packet.ReadPackedGuid128("Guid", k);
                var unk16 = packet.ReadInt32("unk16", k);
                var unk32 = packet.ReadInt32("unk32", k);
                var unk48 = packet.ReadInt32("unk48", k);
                {//650404 22996
                    packet.ReadInt32("unk0", k);
                    var unk4 = packet.ReadInt32("unk4", k);
                    var unk20 = packet.ReadInt32("unk20", k);
                    for (var i = 0; i < unk4; ++i)
                        packet.ReadInt32("unk8", k, i);
                    for (var i = 0; i < unk20; ++i)
                        packet.ReadInt32("unk24", k, i);
                }
                var unk100 = packet.ReadInt32("unk100", k);
                var unk116 = packet.ReadInt32("unk116", k);
                for (var i = 0; i < unk16; ++i)
                    packet.ReadInt32("unk20", k, i);
                for (var j = 0; j < unk32; ++j)
                    packet.ReadInt32("unk36", k, j);
                for (var i = 0; i < unk48; ++i)
                {//658C5B 22996
                    packet.ReadPackedGuid128("guid", k, i);
                    packet.ReadInt32("unk", k, i);
                }
                for (var i = 0; i < unk100; ++i)
                {//689B7C 22996
                    packet.ReadInt32("unk", k, i);
                    packet.ReadByte("unk", k, i);
                }
                for (var i = 0; i < unk116; ++i)
                    sub_650A48(packet, k, i);
            }

        }
    }
}
