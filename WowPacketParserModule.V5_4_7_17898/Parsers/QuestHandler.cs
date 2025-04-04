﻿using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V5_4_7_17898.Parsers
{
    public static class QuestHandler
    {
        [Parser(Opcode.CMSG_QUEST_POI_QUERY)]
        public static void HandleQuestPoiQuery(Packet packet)
        {
            var quest = new int[50];
            for (var i = 0; i < 50; i++)
                quest[i] = packet.ReadInt32();

            var count = packet.ReadInt32("Count");

            for (var i = 0; i < count; i++)
                packet.AddValue("Quest ID", StoreGetters.GetName(StoreNameType.Quest, quest[i]));
        }

        [Parser(Opcode.CMSG_QUERY_QUEST_COMPLETION_NPCS)]
        public static void HandleQuestNpcQuery(Packet packet)
        {
            var count = packet.ReadBits("Count", 22);

            for (var i = 0; i < count; i++)
                packet.ReadInt32<QuestId>("Quest ID", i);
        }

        [Parser(Opcode.SMSG_QUEST_COMPLETION_NPC_RESPONSE)]
        public static void HandleQuestNpcQueryResponse(Packet packet)
        {
            var bits10 = (int)packet.ReadBits(21);

            var bits4 = new uint[bits10];

            for (var i = 0; i < bits10; ++i)
                bits4[i] = packet.ReadBits(22);

            for (var i = 0; i < bits10; ++i)
            {
                for (var j = 0; j < bits4[i]; ++j)
                    packet.ReadInt32("Creature", i, j);

                packet.ReadInt32("Quest Id", i);
            }
        }

        [Parser(Opcode.SMSG_QUEST_POI_QUERY_RESPONSE)]
        public static void HandleQuestPoiQueryResponse(Packet packet)
        {
            uint count = packet.ReadBits("Count", 20);

            var poiCounter = new uint[count];
            var pointsSize = new uint[count][];

            for (int i = 0; i < count; ++i)
            {
                poiCounter[i] = packet.ReadBits("POI Counter", 18, i);
                pointsSize[i] = new uint[poiCounter[i]];

                for (int j = 0; j < poiCounter[i]; ++j)
                    pointsSize[i][j] = packet.ReadBits("Points Counter", 21, i, j);
            }

            for (int i = 0; i < count; ++i)
            {
                var questPOIs = new List<QuestPOI>();
                var questPoiPointsForQuest = new List<QuestPOIPoint>();

                for (int j = 0; j < poiCounter[i]; ++j)
                {
                    QuestPOI questPoi = new QuestPOI();

                    packet.ReadInt32("Flags", i, j);
                    packet.ReadInt32("World Effect ID", i, j);
                    packet.ReadInt32("Player Condition ID", i, j);
                    packet.ReadInt32("QuestObjectID", i, j);

                    var questPoiPoints = new List<QuestPOIPoint>();
                    for (int k = 0; k < pointsSize[i][j]; ++k)
                    {
                        QuestPOIPoint questPoiPoint = new QuestPOIPoint
                        {
                            Idx2 = k,
                            X = packet.ReadInt32("Point X", i, j, k),
                            Y = packet.ReadInt32("Point Y", i, j, k)
                        };
                        questPoiPoints.Add(questPoiPoint);
                    }

                    questPoi.MapID = (int)packet.ReadUInt32<MapId>("Map Id", i, j);
                    packet.ReadInt32("Floor", i, j);
                    packet.ReadInt32("Priority", i, j);
                    questPoi.Floor = (int)packet.ReadUInt32("NumPoints", i, j);
                    questPoi.WorldMapAreaId = (int)packet.ReadUInt32("WorldMapAreaID", i, j);

                    int idx = packet.ReadInt32("BlobIndex", i, j);
                    questPoi.ID = idx;

                    questPoi.ObjectiveIndex = packet.ReadInt32("Objective Index", i, j);

                    questPoiPoints.ForEach(p =>
                    {
                        p.Idx1 = idx;
                        questPoiPointsForQuest.Add(p);
                    });

                    questPOIs.Add(questPoi);
                }

                packet.ReadInt32("NumBlobs", i);
                int questId = packet.ReadInt32<QuestId>("Quest ID", i);

                questPoiPointsForQuest.ForEach(q =>
                {
                    q.QuestID = questId;
                    Storage.QuestPOIPoints.Add(q, packet.TimeSpan);
                });

                questPOIs.ForEach(q =>
                {
                    q.QuestID = questId;
                    Storage.QuestPOIs.Add(q, packet.TimeSpan);
                });
            }

            packet.ReadInt32("Count?");
        }

        [Parser(Opcode.CMSG_QUEST_GIVER_STATUS_QUERY)]
        public static void HandleQuestgiverStatusQuery(Packet packet)
        {
            var guid = packet.StartBitStream(2, 7, 3, 1, 6, 0, 4, 5);
            packet.ParseBitStream(guid, 2, 3, 6, 5, 4, 1, 0, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_QUEST_GIVER_STATUS)]
        public static void HandleQuestgiverStatus(Packet packet)
        {
            var guid = new byte[8];

            packet.StartBitStream(guid, 1, 5, 2, 0, 4, 3, 7, 6);
            packet.ParseBitStream(guid, 7, 0, 4);
            packet.ReadInt32E<QuestGiverStatus4x>("Status");
            packet.ParseBitStream(guid, 2, 1, 6, 3, 5);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_QUEST_GIVER_STATUS_MULTIPLE)]
        public static void HandleQuestgiverStatusMultiple(Packet packet)
        {
            var count = packet.ReadBits("Count", 21);

            var guid = new byte[count][];

            for (var i = 0; i < count; ++i)
            {
                guid[i] = new byte[8];

                packet.StartBitStream(guid[i], 7, 0, 6, 2, 5, 1, 4, 3);
            }

            for (var i = 0; i < count; ++i)
            {
                packet.ReadXORByte(guid[i], 5);
                packet.ReadInt32E<QuestGiverStatus4x>("Status");
                packet.ReadXORByte(guid[i], 4);
                packet.ReadXORByte(guid[i], 2);
                packet.ReadXORByte(guid[i], 3);
                packet.ReadXORByte(guid[i], 6);
                packet.ReadXORByte(guid[i], 1);
                packet.ReadXORByte(guid[i], 7);
                packet.ReadXORByte(guid[i], 0);

                packet.WriteGuid("Guid", guid[i], i);
            }
        }

        [Parser(Opcode.CMSG_QUEST_LOG_REMOVE_QUEST)]
        public static void HandleQuestRemoveQuest(Packet packet)
        {
            packet.ReadByte("Slot");
        }

        [Parser(Opcode.CMSG_QUEST_GIVER_CHOOSE_REWARD)]
        public static void HandleQuestChooseReward(Packet packet)
        {
            var chooseReward = packet.Holder.ClientQuestGiverChooseReward = new();
            var guid = new byte[8];

            chooseReward.Item = packet.ReadUInt32("Reward");
            chooseReward.QuestId = (uint)packet.ReadInt32<QuestId>("Quest ID");
            guid[2] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[3] = packet.ReadBit();

            packet.ParseBitStream(guid, 2, 0, 7, 5, 6, 4, 1, 3);

            chooseReward.QuestGiver = packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_QUEST_GIVER_COMPLETE_QUEST)]
        public static void HandleQuestCompleteQuest(Packet packet)
        {
            var questGiverCompleteQuest = packet.Holder.QuestGiverCompleteQuestRequest = new();
            var guid = new byte[8];

            questGiverCompleteQuest.QuestId = (uint)packet.ReadInt32<QuestId>("Quest ID");
            guid[6] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            packet.ReadBit("bit1C");
            guid[1] = packet.ReadBit();
            guid[2] = packet.ReadBit();

            packet.ParseBitStream(guid, 0, 5, 3, 2, 4, 6, 1, 7);

            questGiverCompleteQuest.QuestGiver = packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_QUEST_GIVER_QUEST_COMPLETE)]
        public static void HandleQuestCompleted510(Packet packet)
        {
            var questComplete = packet.Holder.QuestGiverQuestComplete = new();
            packet.ReadInt32("RewSkillId");
            questComplete.QuestId = (uint) packet.ReadInt32<QuestId>("Quest ID");
            packet.ReadInt32("Talent Points");
            packet.ReadInt32("RewSkillPoints");
            packet.ReadInt32("Money");
            packet.ReadInt32("XP");
            packet.ReadBit("Unk Bit 1"); // if true EVENT_QUEST_FINISHED is fired, target cleared and gossip window is open
            packet.ReadBit("Unk Bit 2");
        }

        [Parser(Opcode.CMSG_QUEST_GIVER_ACCEPT_QUEST)]
        public static void HandleQuestgiverAcceptQuest(Packet packet)
        {
            var questGiverAcceptQuest = packet.Holder.QuestGiverAcceptQuest = new();
            var guid = new byte[8];

            questGiverAcceptQuest.QuestId = packet.ReadUInt32<QuestId>("Quest ID");
            guid[3] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit("bit18");
            guid[5] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();

            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 0);

            questGiverAcceptQuest.QuestGiver = packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_QUEST_GIVER_REQUEST_REWARD)]
        public static void HandleQuestRequestReward(Packet packet)
        {
            var guid = new byte[8];

            packet.ReadUInt32<QuestId>("Quest ID");

            packet.StartBitStream(guid, 4, 1, 7, 0, 3, 2, 6, 5);
            packet.ParseBitStream(guid, 7, 2, 6, 4, 3, 1, 5, 0);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_QUEST_GIVER_QUEST_DETAILS)]
        public static void HandleQuestgiverDetails(Packet packet)
        {
            packet.ReadInt32("QuestGiver Portrait?");
            packet.ReadInt32("Reward Item Id?"); // SMSG_ITEM_PUSH_RESULT is sent with this id in sniffs

            for (var i = 0; i < 5; i++)
            {
                packet.ReadUInt32("Reputation Value Id", i);
                packet.ReadUInt32("Reputation Faction", i);
                packet.ReadInt32("Reputation Value", i);
            }

            packet.ReadInt32("Quest XP");

            for (var i = 0; i < 4; i++)
            {
                packet.ReadUInt32("Currency Id", i);
                packet.ReadUInt32("Currency Count", i);
            }

            packet.ReadInt32("unk1");
            packet.ReadInt32("unk2");
            packet.ReadInt32("unk3");
            packet.ReadUInt32<QuestId>("Quest ID");
            packet.ReadInt32("unk5");
            packet.ReadInt32("unk6");
            packet.ReadInt32("unk7");
            packet.ReadInt32("unk8");
            packet.ReadInt32("unk9");
            packet.ReadInt32("unk10");
            packet.ReadUInt32E<QuestFlagsEx>("Quest Flags 2");
            packet.ReadUInt32E<QuestFlags>("Quest Flags");
            packet.ReadInt32("unk13");
            packet.ReadInt32("unk14");
            packet.ReadInt32("Money");
            packet.ReadInt32("unk16");
            packet.ReadInt32("unk17");
            packet.ReadInt32("unk18");
            packet.ReadInt32("unk19");
            packet.ReadInt32("unk20");
            packet.ReadInt32("unk21");
            packet.ReadInt32("unk22");
            packet.ReadInt32("unk23");
            packet.ReadInt32("unk24");
            packet.ReadInt32("unk25");
            packet.ReadInt32("unk26");
            packet.ReadInt32<SpellId>("Spell Cast Id");
            packet.ReadInt32("unk28");
            packet.ReadInt32("unk29");
            packet.ReadInt32("unk30");
            packet.ReadInt32("unk31");
            packet.ReadInt32("unk32");
            packet.ReadInt32("unk33");
            packet.ReadInt32("unk34");
            packet.ReadInt32("unk35");
            packet.ReadInt32("unk36");
            packet.ReadInt32("unk37");
            packet.ReadInt32("unk38");
            packet.ReadInt32("unk39");
            packet.ReadInt32("unk40");
            packet.ReadInt32("unk41");
            packet.ReadInt32("unk42");
            packet.ReadInt32("unk43");
            packet.ReadInt32("unk44");
            packet.ReadInt32("unk45");

            var guid1 = new byte[8];
            var guid2 = new byte[8];

            guid2[5] = packet.ReadBit();

            var str3len = (int)packet.ReadBits(8);
            var str1len = (int)packet.ReadBits(8);

            var counter5 = (int)packet.ReadBits(22);

            var bit3454 = !packet.ReadBit("!bit0");
            guid2[1] = packet.ReadBit();

            var str6len = (int)packet.ReadBits(12);
            guid2[0] = packet.ReadBit();
            guid1[7] = packet.ReadBit();
            guid1[1] = packet.ReadBit();

            var str5len = (int)packet.ReadBits(10);
            var counter3 = (int)packet.ReadBits(21);
            var counter4 = (int)packet.ReadBits(20);

            guid1[3] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            var bit11234 = !packet.ReadBit("!bit1");
            guid1[2] = packet.ReadBit();
            guid2[2] = packet.ReadBit();

            var str0len = (int)packet.ReadBits(10);//ok

            var str2len = (int)packet.ReadBits(9);//ok

            var str4len = (int)packet.ReadBits(12);//ok was 1

            guid2[4] = packet.ReadBit();
            guid1[5] = packet.ReadBit();
            guid1[4] = packet.ReadBit();
            guid1[6] = packet.ReadBit();
            guid1[0] = packet.ReadBit();
            var bit34354 = !packet.ReadBit("!bit2");
            guid2[6] = packet.ReadBit();
            guid2[7] = packet.ReadBit();

            for (var i = 0; i < counter3; i++) // 3308
            {
                packet.ReadInt32("IntED", i);
                packet.ReadInt32("IntED", i);
            }

            packet.ReadXORByte(guid2, 2);
            packet.ReadXORByte(guid1, 7);
            packet.ReadXORByte(guid2, 1);
            packet.ReadWoWString("string0", str0len);

            for (var i = 0; i < counter4; i++)
            {
                packet.ReadUInt32("Required Count", i);
                var entry = packet.ReadUInt32();
                var type = packet.ReadByte("Type", i);
                switch (type)
                {
                    case 0: packet.AddValue("Required NPC", entry, i); break;
                    case 1: packet.AddValue("Required Item", entry, i); break;
                    default: packet.AddValue("Required Entry", entry, i); break;
                }
                packet.ReadInt32("unk", i);
            }

            packet.ReadXORByte(guid2, 7);
            packet.ReadWoWString("QuestGiver Target Name", str1len);
            packet.ReadWoWString("Title", str2len);
            packet.ReadWoWString("string3", str3len);
            packet.ReadXORByte(guid1, 2);
            packet.ReadXORByte(guid1, 3);
            packet.ReadXORByte(guid1, 1);
            packet.ReadXORByte(guid2, 3);
            packet.ReadXORByte(guid1, 6);
            packet.ReadXORByte(guid2, 5);

            for (int i = 0; i < counter5; i++)
                packet.ReadInt32("Int", i);

            packet.ReadXORByte(guid2, 4);
            packet.ReadXORByte(guid2, 0);
            packet.ReadXORByte(guid2, 6);
            packet.ReadXORByte(guid1, 4);
            packet.ReadWoWString("Objectives", str4len);
            packet.ReadWoWString("string5", str5len);
            packet.ReadWoWString("Details", str6len);
            packet.ReadXORByte(guid1, 5);
            packet.ReadXORByte(guid1, 0);

            packet.WriteGuid("Guid1", guid1);
            packet.WriteGuid("Guid2", guid2);
        }

        [Parser(Opcode.SMSG_QUEST_GIVER_OFFER_REWARD_MESSAGE)]
        public static void HandleQuestOfferReward(Packet packet)
        {
            var guid1 = new byte[8];

            var strlen = packet.ReadBits(10);
            guid1[6] = packet.ReadBit();
            guid1[0] = packet.ReadBit();
            var len2 = packet.ReadBits(21);
            var len3 = packet.ReadBits(8);
            guid1[4] = packet.ReadBit();
            var len4 = packet.ReadBits(8);
            var len5 = packet.ReadBits(12);
            guid1[5] = packet.ReadBit();
            var bit4312 = !packet.ReadBit();
            guid1[3] = packet.ReadBit();
            guid1[2] = packet.ReadBit();
            guid1[1] = packet.ReadBit();
            var len6 = packet.ReadBits(9);
            var len7 = packet.ReadBits(10);
            guid1[7] = packet.ReadBit();

            packet.ReadInt32("unk1");
            packet.ReadInt32("unk2");
            packet.ReadInt32("unk3");


            for (var i = 0; i < 4; i++)
            {
                packet.ReadUInt32("Currency Id", i);
                packet.ReadUInt32("Currency Count", i);
            }

            packet.ReadWoWString("str1", strlen);

            packet.ReadInt32("unk4");
            packet.ReadUInt32("Money");
            packet.ReadInt32("unk6");
            packet.ReadInt32("unk7");
            packet.ReadInt32("unk8");
            packet.ReadInt32("unk9");
            packet.ReadInt32("unk10");

            packet.ReadXORByte(guid1, 3);
            packet.ReadXORByte(guid1, 6);
            packet.ReadXORByte(guid1, 2);

            packet.ReadInt32("unk11");

            for (var i = 0; i < 5; i++)
            {
                packet.ReadUInt32("Reputation Faction", i);
                packet.ReadUInt32("Reputation Value Id", i);
                packet.ReadInt32("Reputation Value (x100)", i);
            }


            packet.ReadInt32("unk12");
            packet.ReadInt32("unk13");
            packet.ReadWoWString("str2", len3);
            packet.ReadInt32("unk14");
            packet.ReadInt32("unk15");
            packet.ReadWoWString("Completion Text", len5);
            packet.ReadInt32("unk16");
            packet.ReadInt32("unk17");
            packet.ReadInt32("unk18");
            packet.ReadWoWString("str4", len4);
            packet.ReadUInt32E<QuestFlags>("Quest Flags");
            packet.ReadInt32("unk20");
            packet.ReadInt32("unk21");
            packet.ReadInt32("unk22");
            packet.ReadInt32("unk23");
            packet.ReadInt32("unk24");
            packet.ReadWoWString("str5", len7);
            packet.ReadInt32("unk25");
            packet.ReadInt32("unk26");
            packet.ReadInt32("unk27");

            for (var i = 0; i < len2; i++) // len2 is guessed
            {
                packet.ReadUInt32E<EmoteType>("Emote Id", i);
                packet.ReadUInt32("Emote Delay", i);
            }
            packet.ReadInt32("unk28");
            packet.ReadXORByte(guid1, 1);

            packet.ReadInt32("unk29");
            packet.ReadUInt32<QuestId>("Quest ID");
            packet.ReadInt32("unk31");
            packet.ReadInt32("unk32");
            packet.ReadInt32("unk33");
            packet.ReadInt32("unk34");
            packet.ReadInt32("unk35");
            packet.ReadInt32("unk36");
            packet.ReadInt32("unk37");
            packet.ReadInt32("unk38");
            packet.ReadXORByte(guid1, 0);
            packet.ReadInt32("unk39");
            packet.ReadXORByte(guid1, 4);
            packet.ReadInt32("unk40");
            packet.ReadXORByte(guid1, 7);
            packet.ReadXORByte(guid1, 5);
            packet.ReadInt32("unk41");
            packet.ReadInt32("unk42");
            packet.ReadInt32("unk43");
            packet.ReadWoWString("Title", len6);
            packet.ReadInt32("unk44");
            packet.ReadInt32("unk45");
            packet.ReadUInt32("XP");
            packet.ReadInt32("unk47");
            packet.ReadInt32("unk48");
            packet.ReadInt32("unk49");


            packet.WriteGuid("Guid1", guid1);

            /*




            // --------------------


            packet.ReadGuid("GUID");
            var entry = packet.ReadUInt32<QuestId>("Quest ID");
            packet.ReadCString("Title");
            var text = packet.ReadCString("Text");

            Storage.QuestOffers.Add((uint)entry, new QuestOffer { OfferRewardText = text }, null);

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V4_0_1_13164))
            {
                packet.ReadCString("QuestGiver Text Window");
                packet.ReadCString("QuestGiver Target Name");
                packet.ReadCString("QuestTurn Text Window");
                packet.ReadCString("QuestTurn Target Name");
                packet.ReadUInt32("QuestGiverPortrait");
                packet.ReadUInt32("QuestTurnInPortrait");
            }

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V3_3_0_10958))
                packet.ReadBool("Auto Finish");
            else
                packet.ReadBool("Auto Finish", TypeCode.Int32);


            packet.ReadUInt32("Suggested Players");

            var count1 = packet.ReadUInt32("Emote Count");
            for (var i = 0; i < count1; i++)
            {
                packet.ReadUInt32("Emote Delay", i);
                packet.ReadUInt32E<EmoteType>("Emote Id", i);
            }

            // extra info

            packet.ReadUInt32("Choice Item Count");
            for (var i = 0; i < 6; i++)
            {
                packet.ReadUInt32<ItemId>("Choice Item Id", i);
                packet.ReadUInt32("Choice Item Count", i);
                packet.ReadUInt32("Choice Item Display Id", i);
            }

            packet.ReadUInt32("Reward Item Count");

            for (var i = 0; i < 4; i++)
                packet.ReadUInt32<ItemId>("Reward Item Id", i);
            for (var i = 0; i < 4; i++)
                packet.ReadUInt32("Reward Item Count", i);
            for (var i = 0; i < 4; i++)
                packet.ReadUInt32("Reward Item Display Id", i);



            packet.ReadUInt32("Title Id");
            packet.ReadUInt32("Bonus Talents");
            packet.ReadUInt32("Reward Reputation Mask");


            packet.ReadInt32<SpellId>("Spell Id");
            packet.ReadInt32<SpellId>("Spell Cast Id");


            packet.ReadUInt32("Reward SkillId");
            packet.ReadUInt32("Reward Skill Points");*/
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_QUERY_QUEST_INFO_RESPONSE)]
        public static void HandleQuestQueryResponce(Packet packet)
        {
            var id = packet.ReadEntry("Quest ID");
            if (id.Value) // entry is masked
                return;

            bool QuestIsntAutoComplete = packet.ReadBit("Quest Isn't AutoComplete");

            if (QuestIsntAutoComplete)
            {
                var QuestEndTextLen = packet.ReadBits(9);
                var QuestTitleLen = packet.ReadBits(9);
                var QuestGiverTextWindowLen = packet.ReadBits(10);
                var QuestTurnTargetNameLen = packet.ReadBits(8);
                var QuestObjectivesLen = packet.ReadBits(12);
                var QuestGiverTargetNameLen = packet.ReadBits(8);
                var QuestDetailsLen = packet.ReadBits(12);
                var QuestTurnTextWindowLen = packet.ReadBits(10);
                var QuestCompletedTextLen = packet.ReadBits(11);
                var QuestObjectivesCount = packet.ReadBits("Objectives Count", 19);
                uint[,] ObjectivesCounts = new uint[QuestObjectivesCount,2];

                for (var i = 0; i < QuestObjectivesCount; ++i)
                {
                    ObjectivesCounts[i,0] = packet.ReadBits(22);
                    ObjectivesCounts[i,1] = packet.ReadBits(8);
                }


                packet.ResetBitReader();

                for (var i = 0; i < QuestObjectivesCount; ++i)
                {
                    packet.ReadUInt32("Unk UInt32", i);
                    packet.ReadWoWString("Objective Text", ObjectivesCounts[i, 1], i);
                    var reqType = packet.ReadByteE<QuestRequirementType>("Requirement Type", i);
                    packet.ReadByte("Unk Byte5", i);
                    packet.ReadUInt32("Requirement Count ", i);

                    for (var j = 0; j < ObjectivesCounts[i, 0]; j++)
                    {
                        packet.ReadUInt32("Unk Looped DWROD", i, j);
                    }

                    packet.ReadUInt32("Unk2 UInt32", i);

                    switch (reqType)
                    {
                        case QuestRequirementType.CreatureKill:
                        case QuestRequirementType.CreatureInteract:
                        case QuestRequirementType.PetBattleDefeatCreature:
                            packet.ReadInt32<UnitId>("Required Creature ID", i);
                            break;
                        case QuestRequirementType.Item:
                            packet.ReadInt32<ItemId>("Required Item ID", i);
                            break;
                        case QuestRequirementType.GameObject:
                            packet.ReadInt32<GOId>("Required GameObject ID", i);
                            break;
                        case QuestRequirementType.Currency:
                            packet.ReadUInt32("Required Currency ID", i);
                            break;
                        case QuestRequirementType.Spell:
                            packet.ReadInt32<SpellId>("Required Spell ID", i);
                            break;
                        case QuestRequirementType.FactionRepHigher:
                        case QuestRequirementType.FactionRepLower:
                            packet.ReadUInt32("Required Faction ID", i);
                            break;
                        default:
                            packet.ReadInt32("Required ID", i);
                            break;
                    }
                }

                packet.ReadUInt32("Reward ItemID Count4");
                packet.ReadUInt32("dword2E50");
                packet.ReadUInt32("Rewarded Spell");
                packet.ReadSingle("Point X");
                packet.ReadUInt32("NextQuestInChain");
                packet.ReadUInt32("dword2E68");
                packet.ReadSingle("RewardHonorMultiplier");
                packet.ReadUInt32("Reward ItemID 4");
                var type = packet.ReadInt32E<QuestType>("Type");
                packet.ReadUInt32("dword2E94");
                packet.ReadUInt32("Reward ItemID Count2");
                var QuestGiverTargetName = packet.ReadWoWString("QuestGiverTargetName", QuestGiverTargetNameLen);
                packet.ReadUInt32("dword2E84");
                packet.ReadUInt32("RepObjectiveValue2");
                packet.ReadUInt32("dword2E58");
                packet.ReadUInt32("dword2E40");

                QuestTemplate quest = new QuestTemplate
                {
                    QuestType = QuestIsntAutoComplete ? QuestType.Normal : QuestType.AutoComplete,
                    QuestLevel = packet.ReadInt32("Level")
                };

                packet.ReadUInt32("BonusTalents");
                quest.QuestCompletionLog = packet.ReadWoWString("QuestCompletedText", QuestCompletedTextLen);
                quest.AreaDescription = packet.ReadWoWString("QuestEndText", QuestEndTextLen);
                packet.ReadUInt32("dword2E08");
                packet.ReadUInt32("dword2E04");
                packet.ReadUInt32("XPId");
                packet.ReadUInt32("dword2E60");
                packet.ReadUInt32("dword2E0C");
                packet.ReadSingle("Point Y");
                quest.RewardBonusMoney = packet.ReadUInt32("Reward Money Max Level");
                packet.ReadUInt32("PointOpt");
                quest.QuestGiverTextWindow = packet.ReadWoWString("QuestGiverTextWindow", QuestGiverTextWindowLen);

                quest.RewardCurrencyID = new uint?[4];
                quest.RewardCurrencyCount = new uint?[4];
                for (var i = 0; i < 4; ++i)
                {
                    quest.RewardCurrencyID[i] = packet.ReadUInt32("Reward Currency ID", i);
                    quest.RewardCurrencyCount[i] = packet.ReadUInt32("Reward Currency Count", i);
                }

                quest.LogDescription = packet.ReadWoWString("QuestObjectives", QuestObjectivesLen);
                packet.ReadUInt32("dword2E4C");
                packet.ReadUInt32("dword2E54");
                packet.ReadUInt32("RewardSpellCasted");

                const int repCount = 5;
                quest.RewardFactionID = new uint?[repCount];
                quest.RewardFactionValue = new int?[repCount];
                quest.RewardFactionOverride = new int?[repCount];

                for(var i = 0; i < repCount; i++)
                {
                    quest.RewardFactionValue[i] = packet.ReadInt32("Reward Reputation ID", i);
                    quest.RewardFactionOverride[i] = (int)packet.ReadUInt32("Reward Reputation ID Override", i);
                    quest.RewardFactionID[i] = packet.ReadUInt32("Reward Faction ID", i);
                }

                quest.QuestTurnTargetName = packet.ReadWoWString("QuestTurnTargetName", QuestTurnTargetNameLen);
                packet.ReadUInt32("dword2E78");
                packet.ReadUInt32("RewSkillID");
                packet.ReadUInt32("CharTittleID");
                packet.ReadUInt32("Reward Item1 ID");
                quest.QuestSortID = packet.ReadInt32E<QuestSort>("Sort");
                packet.ReadUInt32("RewardHonorAddition");
                packet.ReadUInt32("dword2E8C");
                quest.RewardMoney = packet.ReadInt32("Reward Money");
                packet.ReadUInt32("dword2E48");
                quest.QuestDescription = packet.ReadWoWString("QuestDetails", QuestDetailsLen);
                packet.ReadUInt32("RewSkillPoints");
                packet.ReadUInt32("RepObjectiveFaction");
                packet.ReadUInt32("dword2E7C");
                packet.ReadUInt32("SoundAccept");
                packet.ReadUInt32("dword2E74");
                packet.ReadUInt32("MinimapTargetMask");
                packet.ReadUInt32("MinLevel");
                packet.ReadUInt32("dword2E44");
                packet.ReadUInt32("PlayersSlain");
                packet.ReadUInt32("Unk");
                packet.ReadUInt32("RequiredSpellID");
                packet.ReadUInt32("dword2E90");
                packet.ReadUInt32("SourceItemID");
                packet.ReadUInt32("dword2E80");
                packet.ReadUInt32("Reward ItemID Count1");
                packet.ReadUInt32("dword2E70");
                packet.ReadUInt32("Suggested Players");
                packet.ReadUInt32("PointMapId");
                quest.LogTitle = packet.ReadWoWString("QuestTitle", QuestTitleLen);
                packet.ReadUInt32("Reward ItemID 3");
                packet.ReadUInt32("dword2E98");
                packet.ReadUInt32("dword2E5C");
                packet.ReadUInt32("SoundTurnIn");
                packet.ReadWoWString("QuestTurnTextWindow", QuestTurnTextWindowLen);
                packet.ReadUInt32("dword2E9C");
                packet.ReadUInt32("Reward ItemID 2");
                packet.ReadUInt32("Reward ItemID Count3");
                packet.ReadUInt32("dword2E64");
                quest.Flags = packet.ReadUInt32E<QuestFlags>("Flags");
                packet.ReadUInt32("RewArenaPoints");
                packet.ReadUInt32("dword2E6C");
                packet.ReadUInt32("RepObjectiveFaction2");
                packet.ReadUInt32("dword2E88");

                packet.AddSniffData(StoreNameType.Quest, id.Key, "QUERY_RESPONSE");

                Storage.QuestTemplates.Add(quest, packet.TimeSpan);
            }
        }
    }
}
