using System;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using Guid = WowPacketParser.Misc.WowGuid;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class CharacterHandler
    {
        [Parser(Opcode.CMSG_ALTER_APPEARANCE)]
        public static void HandleAlterAppearance(Packet packet)
        {
            packet.ReadInt32("Hair Style");
            packet.ReadInt32("Hair Color");
            packet.ReadInt32("Facial Hair");
            packet.ReadInt32("Skin Color");
        }

        [Parser(Opcode.CMSG_BATTLE_CHAR_BOOST)]
        public static void HandleBattleCharBoostClient(Packet packet)
        {
            byte[] guid = new byte[8];
            byte[] guid2 = new byte[8];

            packet.ReadInt32("unk int32");
            guid2[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid2[5] = packet.ReadBit();
            guid2[4] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid2[6] = packet.ReadBit();
            guid2[0] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid2[2] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var charInfo = !packet.ReadBit("Not Contains Char Info");
            guid[7] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[6] = packet.ReadBit();

            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid2, 0);
            packet.ParseBitStream(guid, 0, 7);
            packet.ReadXORByte(guid2, 7);
            packet.ReadXORByte(guid, 3);
            packet.ParseBitStream(guid2, 6, 4, 5);
            packet.ParseBitStream(guid, 1, 6, 4);
            packet.ParseBitStream(guid2, 1, 2, 3);
            packet.ReadXORByte(guid, 5);
            if (charInfo)
            {
                int hexOutput = packet.ReadInt32("Char Info");
                packet.WriteLine(string.Format("Hex Output {0:X}", hexOutput));
            }
            packet.WriteGuid("Player GUID", guid);
            packet.WriteGuid(guid2);
        }

        [Parser(Opcode.CMSG_CHAR_CREATE)]
        public static void HandleClientCharCreate(Packet packet)
        {
            packet.ReadByte("Outfit Id");
            packet.ReadByte("Facial Hair");
            packet.ReadByte("Skin");
            packet.ReadEnum<Race>("Race", TypeCode.Byte);
            packet.ReadByte("Hair Style");
            packet.ReadEnum<Class>("Class", TypeCode.Byte);
            packet.ReadByte("Face");
            packet.ReadEnum<Gender>("Gender", TypeCode.Byte);
            packet.ReadByte("Hair Color");

            var NameLenght = packet.ReadBits(6);
            var hasDword = packet.ReadBit();

            packet.ReadWoWString("Name", NameLenght);

            if (hasDword)
                packet.ReadUInt32("dword4C");
        }

        [Parser(Opcode.CMSG_CHAR_CUSTOMIZE)]
        public static void HandleClientCharCustomize(Packet packet)
        {
            packet.ReadByte("unk80"); // 80
            packet.ReadByte("unk82"); // 82
            packet.ReadByte("unk66"); // 66
            packet.ReadByte("unk16"); // 16
            packet.ReadByte("unk81"); // 81
            packet.ReadByte("unk83"); // 83
            var guid = new byte[8];
            guid[2] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var len = packet.ReadBits("len", 6);
            guid[4] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            packet.ParseBitStream(guid, 4);
            packet.ReadWoWString("Name", len);
            packet.ParseBitStream(guid, 0, 2, 6, 5, 3, 1, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_CHAR_DELETE)]
        public static void HandleClientCharDelete(Packet packet)
        {
            var guid = packet.StartBitStream(1, 3, 2, 7, 4, 6, 0, 5);
            packet.ParseBitStream(guid, 7, 1, 6, 0, 3, 4, 2, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_CHAR_FACTION_CHANGE)]
        public static void HandleClientCharFactionChange(Packet packet)
        {
            packet.ReadByte("unk18"); // 18
            packet.ReadByte("unk21"); // 21
            var guid80 = new byte[8];
            guid80[3] = packet.ReadBit();
            guid80[2] = packet.ReadBit();
            var unk27 = packet.ReadBit();
            var unk23 = packet.ReadBit();
            guid80[6] = packet.ReadBit();
            var len = packet.ReadBits("len", 6);
            var unk25 = packet.ReadBit();
            var unk20 = packet.ReadBit();
            guid80[4] = packet.ReadBit();
            guid80[1] = packet.ReadBit();
            guid80[0] = packet.ReadBit();
            guid80[5] = packet.ReadBit();
            var unk17 = packet.ReadBit();
            var unk28 = packet.ReadBit();
            guid80[7] = packet.ReadBit();

            packet.ParseBitStream(guid80, 2, 1, 4, 5, 0);
            packet.ReadWoWString("Name", len);
            packet.ParseBitStream(guid80, 6, 3, 7);

            if (unk17)
                packet.ReadByte("unk16"); // 16
            if (unk20)
                packet.ReadByte("unk19"); // 19
            if (unk27)
                packet.ReadByte("unk26"); // 26
            if (unk23)
                packet.ReadByte("unk22"); // 22
            if (unk25)
                packet.ReadByte("unk24"); // 24

            packet.WriteGuid("Guid", guid80);
        }

        [Parser(Opcode.CMSG_CHAR_RENAME)]
        public static void HandleClientCharRename(Packet packet)
        {
            var guid = new byte[8];
            guid[6] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var len = packet.ReadBits("len", 6);
            guid[1] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();

            packet.ParseBitStream(guid, 1, 6, 5);
            packet.ReadWoWString("Name", len);
            packet.ParseBitStream(guid, 2, 4, 3, 7, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_EMOTE)]
        public static void HandleEmote(Packet packet)
        {
            packet.ReadInt32("Emote");
        }

        [Parser(Opcode.CMSG_PLAYED_TIME)]
        public static void HandleCPlayedTime(Packet packet)
        {
            packet.ReadBoolean("Print in chat");
        }

        [Parser(Opcode.CMSG_RANDOMIZE_CHAR_NAME)]
        public static void HandleGenerateRandomCharacterNameQuery(Packet packet)
        {
            packet.ReadEnum<Race>("Race", TypeCode.Byte);
            packet.ReadEnum<Gender>("Sex", TypeCode.Byte);
        }

        [Parser(Opcode.CMSG_REORDER_CHARACTERS)]
        public static void HandleReorderCharacters(Packet packet)
        {
            var count = packet.ReadBits("Count", 9);

            var guids = new byte[count][];

            for (int i = 0; i < count; i++)
            {
                guids[i] = new byte[8];
                packet.StartBitStream(guids[i], 4, 2, 7, 6, 0, 5, 3, 1);
            }

            for (int i = 0; i < count; i++)
            {
                packet.ParseBitStream(guids[i], 1, 2, 7, 5, 4, 0, 3, 6);
                packet.ReadByte("Slot", i);
                packet.WriteGuid("Character Guid", guids[i], i);
            }
        }

        [Parser(Opcode.CMSG_SET_PLAYER_DECLINED_NAMES)]
        public static void HandleSetPlayerDeclinedNames(Packet packet)
        {
            var guid = packet.StartBitStream(0, 2, 1, 7, 5, 6, 4, 3);
            var len = new uint[5];
            for (var i = 0; i < 5; i++)
                len[i] = packet.ReadBits(7);
            for (var i = 0; i < 5; i++)
                packet.ReadWoWString("str", len[i]);
            packet.ParseBitStream(guid, 0, 7, 3, 6, 4, 2, 1, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_SHOWING_CLOAK)]
        [Parser(Opcode.CMSG_SHOWING_HELM)]
        public static void HandleShowingCloakAndHelm(Packet packet)
        {
            packet.ReadBoolean("Showing");
        }

        [Parser(Opcode.CMSG_STANDSTATECHANGE)]
        public static void HandleStandStateChange(Packet packet)
        {
            packet.ReadInt32("Standstate");
        }

        [Parser(Opcode.CMSG_TEXT_EMOTE)]
        public static void HandleTextEmote(Packet packet)
        {
            packet.ReadEnum<EmoteTextType>("Text Emote ID", TypeCode.Int32);
            packet.ReadEnum<EmoteType>("Emote ID", TypeCode.Int32);
            var guid = packet.StartBitStream(6, 7, 3, 2, 0, 5, 1, 4);
            packet.ResetBitReader();
            packet.ParseBitStream(guid, 0, 5, 1, 4, 2, 3, 7, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_BARBER_SHOP_RESULT)]
        public static void HandleBarberShopResult(Packet packet)
        {
            packet.ReadEnum<BarberShopResult>("Result", TypeCode.Int32);
        }

        [Parser(Opcode.SMSG_BINDER_CONFIRM)]
        public static void HandleBinderConfirm(Packet packet)
        {
            var guid = packet.StartBitStream(4, 6, 2, 1, 5, 3, 0, 7);
            packet.ParseBitStream(guid, 6, 2, 5, 0, 4, 7, 1, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_CHAR_ENUM)]
        public static void HandleCharEnum(Packet packet)
        {
            // імена не перевірено, лише послідовність типів данних
            var unkCounter = packet.ReadBits("Unk Counter", 21);//[DW5]
            var count = packet.ReadBits("Char count", 16);//[DW9]

            var charGuids = new byte[count][];
            var guildGuids = new byte[count][];
            var firstLogins = new bool[count];
            var nameLenghts = new uint[count];

            for (int c = 0; c < count; ++c)
            {
                charGuids[c] = new byte[8];
                guildGuids[c] = new byte[8];

                guildGuids[c][4] = packet.ReadBit();
                charGuids[c][0] = packet.ReadBit();
                guildGuids[c][3] = packet.ReadBit();
                charGuids[c][3] = packet.ReadBit();
                charGuids[c][7] = packet.ReadBit();
                packet.ReadBit("unk bit 124", c); //124
                firstLogins[c] = packet.ReadBit(); //108
                charGuids[c][6] = packet.ReadBit();
                guildGuids[c][6] = packet.ReadBit();
                nameLenghts[c] = packet.ReadBits(6);
                charGuids[c][1] = packet.ReadBit();
                guildGuids[c][1] = packet.ReadBit();
                guildGuids[c][0] = packet.ReadBit();
                charGuids[c][4] = packet.ReadBit();
                guildGuids[c][7] = packet.ReadBit();
                charGuids[c][2] = packet.ReadBit();
                charGuids[c][5] = packet.ReadBit();
                guildGuids[c][2] = packet.ReadBit();
                guildGuids[c][5] = packet.ReadBit();
            }//+=416

            //packet.ResetBitReader();
            packet.ReadBit("UnkB16");

            for (int c = 0; c < count; ++c)
            {
                packet.ReadInt32("DW132", c);
                packet.ReadXORByte(charGuids[c], 1);//1
                packet.ReadByte("Slot", c); //57
                packet.ReadByte("Hair Style", c); //63
                packet.ReadXORByte(guildGuids[c], 2);//90
                packet.ReadXORByte(guildGuids[c], 0);//88
                packet.ReadXORByte(guildGuids[c], 6);//94
                var name = packet.ReadWoWString("Name", (int)nameLenghts[c], c);
                packet.ReadXORByte(guildGuids[c], 3);//91
                var x = packet.ReadSingle("Position X", c); //4Ch
                packet.ReadInt32("DW104", c);
                packet.ReadByte("Face", c); //62
                var Class = packet.ReadEnum<Class>("Class", TypeCode.Byte, c); //59
                packet.ReadXORByte(guildGuids[c], 5); //93
             
                for (var itm = 0; itm < 23; itm++)
                {
                    packet.ReadInt32("Item EnchantID", c, itm); //140 prolly need to replace those 2
                    packet.ReadEnum<InventoryType>("Item InventoryType", TypeCode.Byte, c, itm); //144
                    packet.ReadInt32("Item DisplayID", c, itm); //136
                }

                packet.ReadEnum<CustomizationFlag>("CustomizationFlag", TypeCode.UInt32, c); //100
                packet.ReadXORByte(charGuids[c], 3); //3
                packet.ReadXORByte(charGuids[c], 5); //5
                packet.ReadInt32("PetFamily", c); //120
                packet.ReadXORByte(guildGuids[c], 4); //92
                var mapId = packet.ReadInt32("Map", c); //72
                var race = packet.ReadEnum<Race>("Race", TypeCode.Byte, c); //58
                packet.ReadByte("Skin", c); //61
                packet.ReadXORByte(guildGuids[c], 1); //89
                var level = packet.ReadByte("Level", c); //66
                packet.ReadXORByte(charGuids[c], 0); //0
                packet.ReadXORByte(charGuids[c], 2); //2
                packet.ReadByte("Hair Color", c); //64
                packet.ReadEnum<Gender>("Gender", TypeCode.Byte, c); //60
                packet.ReadByte("Facial Hair", c); //65
                packet.ReadInt32("Pet Level", c); //116
                packet.ReadXORByte(charGuids[c], 4); //4
                packet.ReadXORByte(charGuids[c], 7); //7
                var y = packet.ReadSingle("Position Y", c); //50h
                packet.ReadInt32("Pet DisplayID", c); //112
                packet.ReadInt32("DW128", c);
                packet.ReadXORByte(charGuids[c], 6); //6
                packet.ReadEnum<CharacterFlag>("CharacterFlag", TypeCode.Int32, c); //96
                var zone = packet.ReadEntry<UInt32>(StoreNameType.Zone, "Zone Id", c); //68
                packet.ReadXORByte(guildGuids[c], 7); //95
                var z = packet.ReadSingle("Position Z", c); //54h

                var playerGuid = new WowGuid64(BitConverter.ToUInt64(charGuids[c], 0));

                packet.WriteGuid("Character GUID", charGuids[c], c);
                packet.WriteGuid("Guild GUID", guildGuids[c], c);

                if (firstLogins[c])
                {
                    var startPos = new StartPosition();
                    startPos.Map = mapId;
                    startPos.Position = new Vector3(x, y, z);
                    startPos.Zone = (int)zone;

                    Storage.StartPositions.Add(new Tuple<Race, Class>(race, Class), startPos, packet.TimeSpan);
                }

                var playerInfo = new Player { Race = race, Class = Class, Name = name, FirstLogin = firstLogins[c], Level = level };
                if (Storage.Objects.ContainsKey(playerGuid))
                    Storage.Objects[playerGuid] = new Tuple<WoWObject, TimeSpan?>(playerInfo, packet.TimeSpan);
                else
                    Storage.Objects.Add(playerGuid, playerInfo, packet.TimeSpan);
                StoreGetters.AddName(playerGuid, name);
            }

            for (var i = 0; i < unkCounter; i++)
            {
                packet.ReadByte("Unk byte", i); // char_table+28+i*8
                packet.ReadUInt32("Unk int", i); // char_table+24+i*8
            }
        }

        [Parser(Opcode.SMSG_PLAYER_VEHICLE_DATA)]
        public static void HandlePlayerVehicleData(Packet packet)
        {
            var guid = new byte[8];

            packet.StartBitStream(guid, 0, 6, 1, 3, 7, 4, 5, 2);
            packet.ParseBitStream(guid, 6, 7, 0, 3);
            packet.ReadInt32("Vehicle Id");
            packet.ReadInt32("MCounter");
            packet.ParseBitStream(guid, 1, 5, 2, 4);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_RANDOMIZE_CHAR_NAME)]
        public static void HandleGenerateRandomCharacterNameResponse(Packet packet)
        {
            packet.ReadBit("Success");
            packet.ReadWoWString("Name", packet.ReadBits(6));
        }
        [Parser(Opcode.SMSG_TEXT_EMOTE)]
        public static void HandleSTextEmote(Packet packet)
        {
            var guid = new byte[8];
            var guid2 = new byte[8];

            guid[1] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid2[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid2[6] = packet.ReadBit();
            guid2[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid2[0] = packet.ReadBit();
            guid2[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            guid2[4] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[5] = packet.ReadBit();

            packet.ReadXORByte(guid2, 2);
            packet.ReadXORByte(guid2, 1);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid2, 7);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 2);
            packet.ReadEnum<EmoteTextType>("Text Emote ID", TypeCode.Int32);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid2, 0);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid2, 6);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid2, 3);
            packet.ReadXORByte(guid2, 5);
            packet.ReadXORByte(guid2, 4);
            packet.ReadEnum<EmoteType>("Emote ID", TypeCode.Int32);
            packet.WriteGuid("Caster", guid);
            packet.WriteGuid("Target", guid2);
        }

        [Parser(Opcode.SMSG_EXPLORATION_EXPERIENCE)]
        public static void HandleExplorationExpirience(Packet packet)
        {
            packet.ReadEntry<UInt32>(StoreNameType.Area, "Area ID");
            packet.ReadUInt32("Experience");
        }

        [Parser(Opcode.SMSG_INIT_CURRENCY)]
        public static void HandleInitCurrency(Packet packet)
        {
            var count = packet.ReadBits("Count", 21);

            var hasWeekCount = new bool[count];
            var hasWeekCap = new bool[count];
            var hasSeasonTotal = new bool[count];
            var flags = new uint[count];

            for (var i = 0; i < count; i++)
            {
                hasWeekCount[i] = packet.ReadBit();     // +28
                flags[i] = packet.ReadBits(5);          // +32
                hasWeekCap[i] = packet.ReadBit();       // +20
                hasSeasonTotal[i] = packet.ReadBit();   // +12
            }

            for (var i = 0; i < count; i++)
            {
                packet.AddValue("Flags", flags[i], i); // +32

                if (hasSeasonTotal[i])
                    packet.ReadUInt32("Season total earned", i);    // +12

                packet.ReadUInt32("Currency id", i);    // +5

                if (hasWeekCount[i])
                    packet.ReadUInt32("Weekly count", i);    // +28

                packet.ReadUInt32("Currency count", i);    // +4

                if (hasWeekCap[i])
                    packet.ReadUInt32("Weekly cap", i);    // +20
            }
        }

        [Parser(Opcode.SMSG_LEVELUP_INFO)]
        public static void HandleLevelUpInfo(Packet packet)
        {
            packet.ReadInt32("Talent Level"); // 0 - No Talent gain / 1 - Talent Point gain  // 64
            packet.ReadInt32("Health"); // 16
            for (var i = 0; i < 5; i++)
                packet.ReadInt32("Stat", (StatType)i); // 24
            packet.ReadInt32("Level"); // 20
            for (var i = 0; i < 5; i++)
                packet.ReadInt32("Power", (PowerType)i); // 44
        }

        [Parser(Opcode.SMSG_LOG_XPGAIN)]
        public static void HandleLogXPGain(Packet packet)
        {
            var guid = new byte[8];
            var hasBaseXP = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            packet.ReadBit("Unk Bit");
            guid[0] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasGroupRate = !packet.ReadBit();

            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 2);
            packet.ReadByte("XP type");

            if (hasGroupRate)
                packet.ReadSingle("Group rate");

            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 6);

            packet.ReadUInt32("Total XP");

            if (hasBaseXP)
                packet.ReadUInt32("Base XP");

            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_POWER_UPDATE)]
        public static void HandlePowerUpdate(Packet packet)
        {
            var guid = packet.StartBitStream(4, 6, 7, 5, 2, 3, 0, 1);

            var count = packet.ReadBits("Count", 21);

            packet.ParseBitStream(guid, 7, 0, 5, 3, 1, 2, 4);

            for (var i = 0; i < count; i++)
            {
                packet.ReadEnum<PowerType>("Power type", TypeCode.Byte, i); // Actually powertype for class
                packet.ReadInt32("Value", i);
            }

            packet.ReadXORByte(guid, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_SET_PLAYER_DECLINED_NAMES_RESULT)]
        public static void HandleSetPlayerDeclinedNamesResult(Packet packet)
        {
            var unk16 = !packet.ReadBit("!unk16");
            var guid = packet.StartBitStream(2, 0, 3, 1, 4, 6, 5, 7);
            packet.ParseBitStream(guid, 2, 7, 1, 0, 4, 3, 6, 5);
            packet.ReadInt32("unk24");
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_STANDSTATE_UPDATE)]
        public static void HandleStandStateUpdate(Packet packet)
        {
            packet.ReadByte("Standstate");
        }

        [Parser(Opcode.SMSG_TITLE_EARNED)]
        public static void HandleTitleEarned(Packet packet)
        {
            packet.ReadInt32("Title");
        }

        [Parser(Opcode.SMSG_UPDATE_CURRENCY)]
        public static void HandleUpdateCurrency(Packet packet)
        {
            packet.ReadInt32("unk44"); // 44
            packet.ReadInt32("unk16"); // 16
            packet.ReadInt32("unk24"); // 24
            var unk32 = packet.ReadBit("unk32"); // 32
            var unk20 = packet.ReadBit("unk20"); // 20
            var unk40 = packet.ReadBit("unk40"); // 40
            if (unk32)
                packet.ReadInt32("unk28"); // 28
            if (unk40)
                packet.ReadInt32("unk36"); // 36
        }

        [Parser(Opcode.SMSG_UPDATE_CURRENCY_WEEK_LIMIT)]
        public static void HandleUpdateCurrencyWeekLimit(Packet packet)
        {
            packet.ReadInt32("Int20"); // 20
            packet.ReadInt32("Int16"); // 15
        }

        [Parser(Opcode.SMSG_XP_GAIN_ABORTED)]
        public static void HandleXPGainAborted(Packet packet)
        {
            var guid = packet.StartBitStream(2, 3, 0, 5, 7, 6, 1, 4);
            packet.ParseBitStream(guid, 6, 5, 4);
            packet.ReadInt32("XP"); // 32
            packet.ParseBitStream(guid, 7, 1);
            packet.ReadInt32("unk24"); // 24
            packet.ParseBitStream(guid, 3);
            packet.ReadInt32("unk28"); // 28
            packet.ParseBitStream(guid, 2, 0);
            packet.WriteGuid("Guid", guid);
        }
    }
}
