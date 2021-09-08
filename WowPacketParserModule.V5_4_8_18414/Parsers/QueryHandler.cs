using System;
using System.Diagnostics.CodeAnalysis;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    [SuppressMessage("ReSharper", "UseObjectOrCollectionInitializer")]
    public static class QueryHandler
    {
        [Parser(Opcode.CMSG_QUERY_CREATURE)]
        public static void HandleCreatureQuery(Packet packet)
        {
            packet.ReadUInt32("Entry");
        }

        [Parser(Opcode.CMSG_NAME_QUERY)]
        public static void HandleNameQuery(Packet packet)
        {
            var guid = new byte[8];
            guid[4] = packet.ReadBit();
            var hasRealmID1 = packet.ReadBit("hasRealmID1"); // 20
            guid[6] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasRealmID2 = packet.ReadBit("hasRealmID2"); // 28
            guid[5] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            packet.ParseBitStream(guid, 7, 5, 1, 2, 6, 3, 0, 4);
            packet.WriteGuid("Guid", guid);

            if (hasRealmID1)
                packet.ReadInt32("RealmID1"); // 20

            if (hasRealmID2)
                packet.ReadInt32("RealmID2"); // 28
        }

        [Parser(Opcode.CMSG_QUERY_PAGE_TEXT)]
        public static void HandlePageTextQuery(Packet packet)
        {
            var guid = new byte[8];

            packet.ReadInt32("Entry");

            packet.StartBitStream(guid, 2, 1, 3, 7, 6, 4, 0, 5);
            packet.ParseBitStream(guid, 0, 6, 3, 5, 1, 7, 4, 2);

            packet.WriteGuid("GUID", guid);
        }

        [Parser(Opcode.SMSG_CORPSE_MAP_POSITION_QUERY_RESPONSE)]
        public static void HandleCorpseMapPositionQueryResponce(Packet packet)
        {
            packet.ReadSingle("Y");
            packet.ReadSingle("X");
            packet.ReadSingle("O");
            packet.ReadSingle("Z");
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_QUERY_CREATURE_RESPONSE)]
        public static void HandleCreatureQueryResponse(Packet packet)
        {
            var entry = packet.ReadEntry("Entry");

            CreatureTemplate creature = new CreatureTemplate
            {
                Entry = (uint)entry.Key
            };

            var hasData = packet.ReadBit("hasData");

            if (!hasData)
                return; // nothing to do

            uint lengthSubname = packet.ReadBits("Subname length", 11); //7
            uint qItemCount = packet.ReadBits("itemCount", 22);//18
            uint lengthTitleAlt =  packet.ReadBits("titleAlt length", 11); //9
			
            var lengthName = new int[4][];
            for (var i = 0; i < 4; i++)
            {
                lengthName[i] = new int[2];
                lengthName[i][0] = (int)packet.ReadBits("Name length male", 11); //10
                lengthName[i][1] = (int)packet.ReadBits("Name length female", 11); //42
            }

            creature.RacialLeader = packet.ReadBit("Racial Leader"); //17

            uint lengthIconName = packet.ReadBits("iconName length", 6);// ^ 1; //11

            if (lengthTitleAlt > 1)
                creature.TitleAlt = packet.ReadCString("TitleAlt");//8

            creature.KillCredits = new uint?[2];
            creature.KillCredits[0] = packet.ReadUInt32("Kill Credit 1");//27
			
            creature.ModelIDs = new uint?[4];
            creature.ModelIDs[3] = packet.ReadUInt32("Display Id 4");//32
            creature.ModelIDs[1] = packet.ReadUInt32("Display Id 2");//31

            //creature.Expansion = 
            packet.ReadUInt32E<ClientType>("Expansion");//24
			
            creature.Type = packet.ReadInt32E<CreatureType>("Type");//12
			
            creature.HealthModifier = packet.ReadSingle("Modifier Health");//15

            creature.TypeFlags = packet.ReadUInt32E<CreatureTypeFlag>("Type Flags");//25
			
            creature.TypeFlags2 = packet.ReadUInt32("Creature Type Flags 2");//26

            creature.Rank = packet.ReadInt32E<CreatureRank>("Rank");//14
            creature.MovementID = packet.ReadUInt32("Movement Id");//23

            var name = new string[8];
            for (var i = 0; i < 4; i++)
            {
                if (lengthName[i][1] > 1)
                    name[i+4] = packet.ReadCString("Female Name", i);

                if (lengthName[i][0] > 1)
                    name[i] = packet.ReadCString("Male name", i);
            }
            creature.Name = name[0];
            creature.FemaleName = name[4];

            if (lengthSubname > 1)
                creature.SubName = packet.ReadCString("Sub Name");
				
			
            creature.ModelIDs[0] = packet.ReadUInt32("Display Id 1");//29
				
            creature.ModelIDs[2] = packet.ReadUInt32("Display Id 3");//30
			
            if (lengthIconName > 1)
                creature.IconName = packet.ReadCString("Icon Name");//10
			
            //creature.QuestItem = new uint[qItemCount];
            for (var i = 0; i < qItemCount; i++)
                /*creature.QuestItems[i] = (uint)*/packet.ReadUInt32<ItemId>("Quest Item", i);			
			
            creature.KillCredits[1] = packet.ReadUInt32("Kill Credit 2");//28

            creature.ManaModifier = packet.ReadSingle("Modifier Mana");//16

            creature.Family = packet.ReadInt32E<CreatureFamily>("Family");//13

            for (int i = 0; i < 4; ++i)
                packet.AddValue("Display ID", creature.ModelIDs[i], i);
            for (int i = 0; i < 2; ++i)
                packet.AddValue("Kill Credit", creature.KillCredits[i], i);

            packet.AddSniffData(StoreNameType.Unit, entry.Key, "QUERY_RESPONSE");

            Storage.CreatureTemplates.Add(creature.Entry.Value, creature, packet.TimeSpan);

            if (ClientLocale.PacketLocale != LocaleConstant.enUS)
            {
                CreatureTemplateLocale localesCreature = new CreatureTemplateLocale
                {
                    ID = (uint)entry.Key,
                    Name = creature.Name,
                    NameAlt = creature.FemaleName,
                    Title = creature.SubName,
                    TitleAlt = creature.TitleAlt
                };

                Storage.LocalesCreatures.Add(localesCreature, packet.TimeSpan);
            }

            ObjectName objectName = new ObjectName
            {
                ObjectType = StoreNameType.Unit,
                ID = entry.Key,
                Name = creature.Name
            };
            Storage.ObjectNames.Add(objectName, packet.TimeSpan);
        }

        [Parser(Opcode.SMSG_QUERY_PLAYER_NAME_RESPONSE)]
        public static void HandleNameQueryResponse(Packet packet)
        {
            var guid = packet.StartBitStream(3, 6, 7, 2, 5, 4, 0, 1);
            packet.ParseBitStream(guid, 5, 4, 7, 6, 1, 2);

            var nameData = !packet.ReadBool("!nameData");
            if (nameData)
            {
                packet.ReadInt32("RealmID"); // 108
                packet.ReadInt32("unk36");
                packet.ReadByteE<Class>("Class");
                packet.ReadByteE<Race>("Race");
                packet.ReadByte("Level");
                packet.ReadByteE<Gender>("Gender");
            }
            packet.ParseBitStream(guid, 0, 3);

            packet.WriteGuid("Guid", guid);

            if (!nameData)
                return;

            var guid2 = new byte[8];
            var guid3 = new byte[8];

            guid2[2] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid3[7] = packet.ReadBit();
            guid3[2] = packet.ReadBit();
            guid3[0] = packet.ReadBit();
            var unk32 = packet.ReadBit();
            guid2[4] = packet.ReadBit();
            guid3[5] = packet.ReadBit();
            guid2[1] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            guid2[0] = packet.ReadBit();

            var len = new uint[5];
            for (var i = 5; i > 0; i--)
                len[i - 1] = packet.ReadBits(7);

            guid3[6] = packet.ReadBit();
            guid3[3] = packet.ReadBit();
            guid2[5] = packet.ReadBit();
            guid3[1] = packet.ReadBit();
            guid3[4] = packet.ReadBit();

            var len56 = packet.ReadBits(6);

            guid2[6] = packet.ReadBit();

            packet.ReadXORByte(guid3, 6);
            packet.ReadXORByte(guid3, 0);

            var name = packet.ReadWoWString("Name", len56);
            var playerGuid = new WowGuid64(BitConverter.ToUInt64(guid, 0));
            StoreGetters.AddOrUpdateName(playerGuid, name);

            packet.ReadXORByte(guid2, 5);
            packet.ReadXORByte(guid2, 2);
            packet.ReadXORByte(guid3, 3);
            packet.ReadXORByte(guid2, 4);
            packet.ReadXORByte(guid2, 3);
            packet.ReadXORByte(guid3, 4);
            packet.ReadXORByte(guid3, 2);
            packet.ReadXORByte(guid2, 7);

            for (var i = 5; i > 0; i--)
                packet.ReadWoWString("str", len[i - 1], i);

            packet.ReadXORByte(guid2, 6);
            packet.ReadXORByte(guid3, 7);
            packet.ReadXORByte(guid3, 1);
            packet.ReadXORByte(guid2, 1);
            packet.ReadXORByte(guid3, 5);
            packet.ReadXORByte(guid2, 0);

            packet.WriteLine("Account: {0}", BitConverter.ToUInt64(guid2, 0));
            packet.WriteGuid("Guid3", guid3);

            var objectName = new ObjectName
            {
                ObjectType = StoreNameType.Player,
                Name = name,
                ID = (int)playerGuid.GetLow()
            };
            Storage.ObjectNames.Add(objectName, packet.TimeSpan);
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_QUERY_PAGE_TEXT_RESPONSE)]
        public static void HandlePageTextResponse(Packet packet)
        {
            var pageText = new PageText();

            var hasData = packet.ReadBit();
            if (!hasData)
                return; // nothing to do

            var textLen = packet.ReadBits(12);

            pageText.NextPageID = packet.ReadUInt32("Next Page");
            packet.ReadUInt32("Entry");

            pageText.Text = packet.ReadWoWString("Page Text", textLen);

            var entry = packet.ReadUInt32("Entry");
            pageText.ID = entry;

            packet.AddSniffData(StoreNameType.PageText, (int)entry, "QUERY_RESPONSE");
            Storage.PageTexts.Add(pageText, packet.TimeSpan);
        }

        [Parser(Opcode.SMSG_QUERY_TIME_RESPONSE)]
        public static void HandleQueryTimeResponse(Packet packet)
        {
            packet.ReadTime("Current Time");
            packet.ReadInt32("Daily Quest Reset");
        }

        [Parser(Opcode.SMSG_REALM_QUERY_RESPONSE)]
        public static void HandleQueryRealmNameResponse(Packet packet)
        {
            var hasData = !packet.ReadBool("!HasData");
            packet.ReadInt32("RealmID");
            if (hasData)
            {
                var len278 = packet.ReadBits(8);
                packet.ReadBit("unk21");
                var len88 = packet.ReadBits(8);
                packet.ReadWoWString("RealmName", len88);
                packet.ReadWoWString("RealmName2", len278);
            }
        }
    }
}
