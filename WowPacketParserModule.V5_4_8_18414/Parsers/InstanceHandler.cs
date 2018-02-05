using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using MapDifficulty = WowPacketParserModule.V5_4_8_18414.Enums.MapDifficulty;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class InstanceHandler
    {
        [Parser(Opcode.CMSG_SET_DUNGEON_DIFFICULTY)]
        [Parser(Opcode.MSG_SET_RAID_DIFFICULTY)]
        public static void HandleSetDifficulty(Packet packet)
        {
            packet.ReadInt32E<MapDifficulty>("Difficulty");
        }

        [Parser(Opcode.CMSG_SAVE_CUF_PROFILES)]
        public static void HandleSaveCUFProfiles(Packet packet)
        {
            var count = packet.ReadBits("Count", 19);

            var strlen = new uint[count];

            for (int i = 0; i < count; ++i)
            {
                packet.ReadBit("Talent spec 2", i);                    // 166
                packet.ReadBit("Main tank and assist", i);             // 136
                packet.ReadBit("Power bars", i);                       // 140
                packet.ReadBit("10 player group", i);                  // 161
                packet.ReadBit("3 player group", i);                   // 159
                packet.ReadBit("Unk 156", i);                          // 156
                packet.ReadBit("40 player group", i);                  // 164
                packet.ReadBit("2 player group", i);                   // 158
                packet.ReadBit("Keep groups together", i);             // 134
                packet.ReadBit("Class colors", i);                     // 142
                packet.ReadBit("25 player group", i);                  // 163
                packet.ReadBit("Unk 145", i);                          // 145
                strlen[i] = packet.ReadBits("String length", 7, i);    // 0
                packet.ReadBit("Pets", i);                             // 135
                packet.ReadBit("PvP", i);                              // 167
                packet.ReadBit("Dispellable debuffs", i);              // 139
                packet.ReadBit("Debuffs", i);                          // 144
                packet.ReadBit("15 player group", i);                  // 162
                packet.ReadBit("Unk 157", i);                          // 157
                packet.ReadBit("Border", i);                           // 141
                packet.ReadBit("Horizontal Groups", i);                // 143
                packet.ReadBit("Talent spec 1", i);                    // 165
                packet.ReadBit("5 player group", i);                   // 160
                packet.ReadBit("PvE", i);                              // 168
                packet.ReadBit("Incoming heals", i);                   // 137
                packet.ReadBit("Aggro highlight", i);                  // 138
            }

            for (int i = 0; i < count; ++i)
            {
                packet.ReadInt16("Frame height", i);             // 128
                packet.ReadByte("Unk 146", i);                   // 146
                packet.ReadByte("Health text", i);               // 133
                packet.ReadInt16("Frame width", i);              // 130
                packet.ReadByte("Unk 148", i);                   // 148
                packet.ReadByte("Sort by", i);                   // 132
                packet.ReadInt16("Unk 150", i);                  // 150
                packet.ReadWoWString("Name", (int)strlen[i], i); // 0
                packet.ReadByte("Unk 147", i);                   // 147
                packet.ReadInt16("Unk 152", i);                  // 152
                packet.ReadInt16("Unk 154", i);                  // 154
            }
        }

        [Parser(Opcode.SMSG_ENCOUNTER_END)]
        public static void HandleEncounterEnd(Packet packet)
        {
            packet.ReadBit("unk28"); // 28
            packet.ReadInt32("unk5"); // 5
            packet.ReadInt32("unk6"); // 6
            packet.ReadInt32("unk4"); // 4
        }

        [Parser(Opcode.SMSG_ENCOUNTER_START)]
        public static void HandleEncounterStart(Packet packet)
        {
            packet.ReadInt32("unk5"); // 5
            packet.ReadInt32("unk6"); // 6
            packet.ReadInt32("unk4"); // 4
        }

        [Parser(Opcode.SMSG_INSTANCE_INFO)]
        public static void HandleSInstanceInfo(Packet packet)
        {
            var count = packet.ReadBits("Count", 20); // 16
            var guid = new byte[count][];

            for (var i = 0; i < count; i++)
            {
                guid[i] = new byte[8];

                guid[i][1] = packet.ReadBit(); // 29
                guid[i][2] = packet.ReadBit(); // 30
                guid[i][6] = packet.ReadBit(); // 34
                packet.ReadBit("Locked", i); // 37
                guid[i][0] = packet.ReadBit(); // 28
                guid[i][5] = packet.ReadBit(); // 33
                guid[i][4] = packet.ReadBit(); // 32
                packet.ReadBit("Extended", i); // 36
                guid[i][7] = packet.ReadBit(); // 35
                guid[i][3] = packet.ReadBit(); // 31
            }
            for (var i = 0; i < count; i++)
            {
                packet.ParseBitStream(guid[i], 7, 6, 4, 2, 0);
                packet.ReadInt32("Reset time", i); // 40
                packet.ReadInt32("Completed_mask", i); // 44
                packet.ParseBitStream(guid[i], 1);
                packet.ReadInt32<MapId>("MapID", i); // 20
                packet.ReadInt32<DifficultyId>("DifficultyID", i); // 24
                packet.ParseBitStream(guid[i], 3, 5);

                packet.WriteGuid("Guid", guid[i], i);
            }
        }

        [Parser(Opcode.SMSG_INSTANCE_GROUP_SIZE_CHANGED)]
        public static void HandleInstanceGroupSizeChanged(Packet packet)
        {
            packet.ReadUInt32("GroupSize");
        }

        [Parser(Opcode.SMSG_INSTANCE_SAVE_CREATED)]
        public static void HandleInstanceSaveCreated(Packet packet)
        {
            packet.ReadBool("Gm");
        }

        [Parser(Opcode.SMSG_LOAD_CUF_PROFILES)]
        public static void HandleLoadCUFProfiles(Packet packet)
        {
            var count = packet.ReadBits("count", 19);

            var strlen = new uint[count];

            for (int i = 0; i < count; ++i)
            {
                packet.ReadBit("Talent spec 1", i);                         // 165
                packet.ReadBit("3 player group", i);                        // 159
                packet.ReadBit("Unk 157", i);                               // 157
                packet.ReadBit("10 player group", i);                       // 161
                packet.ReadBit("40 player group", i);                       // 164
                packet.ReadBit("Border", i);                                // 141
                packet.ReadBit("Class colors", i);                          // 142
                packet.ReadBit("Keep groups together", i);                  // 134
                packet.ReadBit("Display power bars", i);                    // 140
                strlen[i] = packet.ReadBits("String length", 7, i);         // 0
                packet.ReadBit("Pets", i);                                  // 135
                packet.ReadBit("Aggro highlight", i);                       // 138
                packet.ReadBit("Unk 145", i);                               // 145
                packet.ReadBit("PvP", i);                                   // 167
                packet.ReadBit("Unk 156", i);                               // 156
                packet.ReadBit("Main tank and assist", i);                  // 136
                packet.ReadBit("Debuffs", i);                               // 144
                packet.ReadBit("Horizontal groups", i);                     // 143
                packet.ReadBit("Talent spec 2", i);                         // 166
                packet.ReadBit("Incoming heals", i);                        // 137
                packet.ReadBit("Dispellable debuffs", i);                   // 139
                packet.ReadBit("25 player group", i);                       // 163
                packet.ReadBit("PvE", i);                                   // 168
                packet.ReadBit("5 player group", i);                        // 160
                packet.ReadBit("15 player group", i);                       // 162
                packet.ReadBit("2 player group", i);                        // 158
            }

            for (int i = 0; i < count; ++i)
            {
                packet.ReadInt16("Unk 152", i);                             // 152
                packet.ReadInt16("Unk 154", i);                             // 154
                packet.ReadByte("Health text", i); // 0 - none, 1 - remaining, 2 - lost, 3 - percentage 133
                packet.ReadWoWString("Name", (int)strlen[i], i);            // 172
                packet.ReadByte("Unk 147", i);                              // 147
                packet.ReadByte("Unk 146", i);                              // 146
                packet.ReadInt16("Frame height", i);                        // 128
                packet.ReadByte("Unk 148", i);                              // 148
                packet.ReadByte("Sort by", i); // 0 - role, 1 - group, 2 - alphabetical 132
                packet.ReadInt16("Frame width", i);                         // 130
                packet.ReadInt16("Unk 150", i);                             // 150
            }
        }

        [Parser(Opcode.SMSG_SET_DUNGEON_DIFFICULTY)]
        public static void HandleSetDungeonDifficulty(Packet packet)
        {
            packet.ReadInt32E<MapDifficulty>("Difficulty");
        }

        [Parser(Opcode.SMSG_RAID_INSTANCE_MESSAGE)]
        public static void HandleRaidInstanceMessage(Packet packet)
        {
            packet.ReadBit("unk16"); // 16
            packet.ReadBit("unk17"); // 17
            packet.ReadInt32("unk24"); // 24
            packet.ReadByte("unk28"); // 28
            packet.ReadInt32("unk20"); // 20
            packet.ReadInt32("unk32"); // 32
        }

        [Parser(Opcode.SMSG_UPDATE_INSTANCE_ENCOUNTER_UNIT)]
        public static void HandleUpdateInstanceEncounterUnit(Packet packet)
        {
            packet.AsHex();
            var select = packet.ReadInt32("select");
            switch (select)
            {
                case 2:
                case 3:
                case 4:
                    packet.ReadPackedGuid("Guid");
                    packet.ReadByte("unk");
                    break;
                case 1:
                case 9:
                case 10:
                    break;
                case 7:
                    packet.ReadByte("unk1");
                    packet.ReadByte("unk2");
                    break;
                case 0:
                case 5:
                case 6:
                case 8:
                    packet.ReadByte("unk");
                    break;
            }
        }

        [Parser(Opcode.CMSG_RESET_INSTANCES)]
        [Parser(Opcode.SMSG_AREA_TRIGGER_NO_CORPSE)]
        [Parser(Opcode.SMSG_DIFFERENT_INSTANCE_FROM_PARTY)]
        [Parser(Opcode.SMSG_RESET_FAILED_NOTIFY)]
        [Parser(Opcode.SMSG_UPDATE_DUNGEON_ENCOUNTER_FOR_LOOT)]
        public static void HandleInstanceNull(Packet packet)
        {
        }
    }
}
