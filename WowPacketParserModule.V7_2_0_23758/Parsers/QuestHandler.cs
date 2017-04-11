using WowPacketParser.Enums;
using WowPacketParser.Loading;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class QuestHandler
    {
        public static void ReadGossipText(Packet packet, params object[] indexes)
        {
            packet.ReadUInt32("QuestID", indexes);
            packet.ReadUInt32("QuestType", indexes);
            packet.ReadUInt32("QuestLevel", indexes);

            for (int i = 0; i < 2; i++)
                packet.ReadUInt32("QuestFlags", indexes, i);

            packet.ResetBitReader();

            packet.ReadBit("Repeatable", indexes);
            packet.ReadBit("IsQuestIgnored", indexes);

            var guestTitleLen = packet.ReadBits(9);
            packet.ReadWoWString("QuestTitle", guestTitleLen, indexes);
        }

        public static void ReadQuestRewards(Packet packet, params object[] idx)
        {
            packet.ReadInt32("ChoiceItemCount", idx);

            for (var i = 0; i < 6; ++i)
            {
                packet.ReadInt32("ItemID", idx, i);
                packet.ReadInt32("Quantity", idx, i);
            }

            packet.ReadInt32("ItemCount", idx);

            for (var i = 0; i < 4; ++i)
            {
                packet.ReadInt32("ItemID", idx, i);
                packet.ReadInt32("ItemQty", idx, i);
            }

            packet.ReadInt32("XP", idx);
            packet.ReadInt32("ArtifactXP", idx);
            packet.ReadInt64("Money", idx);
            packet.ReadInt32("ArtifactCategoryID", idx);
            packet.ReadInt32("Honor", idx);
            packet.ReadInt32("Title", idx);
            packet.ReadInt32("FactionFlags", idx);

            for (var i = 0; i < 5; ++i)
            {
                packet.ReadInt32("FactionID", idx, i);
                packet.ReadInt32("FactionValue", idx, i);
                packet.ReadInt32("FactionOverride", idx, i);
                packet.ReadInt32("FactionCapIn", idx, i);
            }

            for (var i = 0; i < 3; ++i)
                packet.ReadInt32("SpellCompletionDisplayID", idx, i);

            packet.ReadInt32("SpellCompletionID", idx);

            for (var i = 0; i < 4; ++i)
            {
                packet.ReadInt32("CurrencyID", idx, i);
                packet.ReadInt32("CurrencyQty", idx, i);
            }

            packet.ReadInt32("SkillLineID", idx);
            packet.ReadInt32("NumSkillUps", idx);
            packet.ReadInt32("RewardID", idx);

            packet.ResetBitReader();

            packet.ReadBit("IsBoostSpell", idx);
        }

        public static void sub_67A776(Packet packet, params object[] idx)
        {//67A776 22996
            {//650B42 22996
                packet.ReadInt32("Id", idx);
                packet.ReadInt32("DisplayID", idx);
                packet.ReadInt32("Quantity", idx);

                packet.ResetBitReader();

                var bit32 = packet.ReadBit("HasBit32", idx);
                var bit56 = packet.ReadBit("HasBit56", idx);

                if (bit32)
                    ItemHandler.ReadBonuses(packet, idx);

                if (bit56)
                {
                    // sub_5ECDA0
                    var int4 = packet.ReadInt32("", idx);
                    packet.ReadWoWString("", int4, idx);
                }
            }

            packet.ReadInt32("unk60", idx);
        }
        [Parser(Opcode.CMSG_ADVENTURE_JOURNAL_OPEN_QUEST)]
        public static void HandleAdventureJournalOpenQuest(Packet packet)
        {
            packet.ReadInt32("QuestID");
        }

        [Parser(Opcode.CMSG_QUERY_QUEST_INFO)]
        public static void HandleQuestQuery(Packet packet)
        {
            packet.ReadInt32("Entry");
            packet.ReadPackedGuid128("Guid");
        }

        [Parser(Opcode.CMSG_QUERY_QUEST_REWARDS)]
        public static void HandleQuestQueryRewards(Packet packet)
        {
            packet.ReadInt32("QuestID");
            packet.ReadInt32("unk2");
        }

        [Parser(Opcode.CMSG_QUEST_GIVER_CHOOSE_REWARD)]
        public static void HandleQuestChooseReward(Packet packet)
        {
            packet.ReadPackedGuid128("QuestGiverGUID");
            packet.ReadInt32("QuestID");
            packet.ReadInt32("ItemChoiceID");
        }

        [Parser(Opcode.CMSG_QUEST_GIVER_STATUS_QUERY)]
        public static void HandleQuestgiverStatusQuery(Packet packet)
        {
            packet.ReadPackedGuid128("QuestGiverGUID");
        }

        [Parser(Opcode.CMSG_QUEST_POI_QUERY)]
        public static void HandleQuestPOIQuery(Packet packet)
        {
            packet.ReadUInt32("MissingQuestCount");

            for (var i = 0; i < 50; i++)
                packet.ReadInt32<QuestId>("MissingQuestPOIs", i);
        }

        [Parser(Opcode.CMSG_QUERY_QUEST_COMPLETION_NPCS)]
        public static void HandleQueryQuestCompletionNpcs(Packet packet)
        {
            var count = packet.ReadUInt32("Count");

            for (var i = 0; i < count; i++)
                packet.ReadInt32<QuestId>("QuestID", i);
        }

        [Parser(Opcode.SMSG_QUERY_QUEST_REWARD_RESPONSE)]
        public static void HandleQuestRewardResponce(Packet packet)
        {
            packet.ReadInt32("QuestID");
            packet.ReadInt32("unk2");
            {//67A0F2 22996
                var cnt1 = packet.ReadInt32("Cnt1");
                var cnt2 = packet.ReadInt32("cnt2");
                packet.ReadInt64("unk");
                for (var i = 0; i < cnt1; ++i)
                {//67A0A2 22996
                    packet.ReadInt32("Item", i);
                    packet.ReadInt32("unk5", i);
                    packet.ReadByte("unk6byte", i);
                }
                for (var i = 0; i < cnt2; ++i)
                {//65169D 22996
                    packet.ReadInt32("unk6", i);
                    packet.ReadInt32("unk7", i);
                }
            }
        }

        [Parser(Opcode.SMSG_DISPLAY_PLAYER_CHOICE)]
        public static void HandleDisplayPlayerChoice(Packet packet)
        {
            packet.ReadInt32("ChoiceID");
            var int5 = packet.ReadInt32("PlayerChoiceResponseCount");
            packet.ReadPackedGuid128("Guid");

            packet.ResetBitReader();

            var len = packet.ReadBits(8);

            for (int i = 0; i < int5; i++)
            {//67A481 22996
                packet.ReadInt32("ResponseID", i);
                packet.ReadInt32("ChoiceArtFileID", i);

                packet.ResetBitReader();

                var bits4 = packet.ReadBits(9);
                var bits404 = packet.ReadBits(9);
                var bits804 = packet.ReadBits(11);
                var bits2404 = packet.ReadBits(7);
                var bit2640 = packet.ReadBit("HasPlayerChoiceResponseReward", i);

                if (bit2640)
                {//67A5BD 22996
                    packet.ReadInt32("TitleID", i);
                    packet.ReadInt32("PackageID", i);
                    packet.ReadInt32("SkillLineID", i);
                    packet.ReadInt32("SkillPointCount", i);
                    packet.ReadInt32("ArenaPointCount", i);
                    packet.ReadInt32("HonorPointCount", i);
                    packet.ReadInt64("Money", i);
                    packet.ReadInt32("Xp", i);

                    var int36 = packet.ReadInt32("ItemsCount", i);
                    var int52 = packet.ReadInt32("CurrenciesCount", i);
                    var int68 = packet.ReadInt32("FactionsCount", i);
                    var int84 = packet.ReadInt32("ItemChoicesCount", i);

                    for (int j = 0; j < int36; j++) // @To-Do: need verification
                        sub_67A776(packet, j);

                    for (int j = 0; j < int52; j++)
                        sub_67A776(packet, j);

                    for (int j = 0; j < int68; j++)
                        sub_67A776(packet, j);

                    for (int j = 0; j < int84; j++)
                        sub_67A776(packet, j);
                }

                packet.ReadWoWString("Answer", bits4);
                packet.ReadWoWString("Description", bits404);
                packet.ReadWoWString("unks1", bits804);
                packet.ReadWoWString("unks2", bits2404);
            }

            packet.ReadWoWString("Question", len);
        }

        [Parser(Opcode.SMSG_DISPLAY_QUEST_POPUP)]
        public static void HandleQuestDisplayPopup(Packet packet)
        {
            packet.ReadInt32("QuestID");
        }

        [Parser(Opcode.SMSG_QUEST_GIVER_OFFER_REWARD_MESSAGE)]
        public static void HandleQuestGiverOfferReward(Packet packet)
        {
            packet.ReadPackedGuid128("QuestGiverGUID");

            packet.ReadInt32("QuestGiverCreatureID");
            int id = packet.ReadInt32("QuestID");

            QuestOfferReward questOfferReward = new QuestOfferReward
            {
                ID = (uint)id
            };

            for (int i = 0; i < 2; i++)
                packet.ReadInt32("QuestFlags", i);

            packet.ReadInt32("SuggestedPartyMembers");

            int emotesCount = packet.ReadInt32("EmotesCount");

            // QuestDescEmote
            questOfferReward.Emote = new uint?[] { 0, 0, 0, 0 };
            questOfferReward.EmoteDelay = new uint?[] { 0, 0, 0, 0 };
            for (int i = 0; i < emotesCount; i++)
            {
                questOfferReward.Emote[i] = (uint)packet.ReadInt32("Type");
                questOfferReward.EmoteDelay[i] = packet.ReadUInt32("Delay");
            }

            packet.ResetBitReader();

            packet.ReadBit("AutoLaunched");

            ReadQuestRewards(packet, "QuestRewards");

            packet.ReadInt32("PortraitTurnIn");
            packet.ReadInt32("PortraitGiver");
            packet.ReadInt32("QuestPackageID");

            packet.ResetBitReader();

            uint questTitleLen = packet.ReadBits(9);
            uint rewardTextLen = packet.ReadBits(12);
            uint portraitGiverTextLen = packet.ReadBits(10);
            uint portraitGiverNameLen = packet.ReadBits(8);
            uint portraitTurnInTextLen = packet.ReadBits(10);
            uint portraitTurnInNameLen = packet.ReadBits(8);

            packet.ReadWoWString("QuestTitle", questTitleLen);
            questOfferReward.RewardText = packet.ReadWoWString("RewardText", rewardTextLen);
            packet.ReadWoWString("PortraitGiverText", portraitGiverTextLen);
            packet.ReadWoWString("PortraitGiverName", portraitGiverNameLen);
            packet.ReadWoWString("PortraitTurnInText", portraitTurnInTextLen);
            packet.ReadWoWString("PortraitTurnInName", portraitTurnInNameLen);

            Storage.QuestOfferRewards.Add(questOfferReward, packet.TimeSpan);
        }

        [Parser(Opcode.SMSG_QUEST_GIVER_REQUEST_ITEMS)]
        public static void HandleQuestGiverRequestItems(Packet packet)
        {
            packet.ReadPackedGuid128("QuestGiverGUID");
            packet.ReadInt32("QuestGiverCreatureID");

            int id = packet.ReadInt32("QuestID");
            QuestRequestItems questRequestItems = new QuestRequestItems
            {
                ID = (uint)id
            };

            questRequestItems.EmoteOnCompleteDelay = (uint)packet.ReadInt32("CompEmoteDelay");
            questRequestItems.EmoteOnComplete = (uint)packet.ReadInt32("CompEmoteType");

            for (int i = 0; i < 2; i++)
                packet.ReadInt32("QuestFlags", i);

            packet.ReadInt32("SuggestPartyMembers");
            packet.ReadInt32("MoneyToGet");
            int collectCount = packet.ReadInt32("CollectCount");
            int currencyCount = packet.ReadInt32("CurrencyCount");
            packet.ReadInt32("StatusFlags");

            for (int i = 0; i < collectCount; i++)
            {
                packet.ReadInt32("ObjectID", i);
                packet.ReadInt32("Amount", i);
                packet.ReadUInt32("Flags", i);
            }

            for (int i = 0; i < currencyCount; i++)
            {
                packet.ReadInt32("CurrencyID", i);
                packet.ReadInt32("Amount", i);
            }

            packet.ResetBitReader();

            packet.ReadBit("AutoLaunched");
            packet.ReadBit("CanIgnoreQuest");
            packet.ReadBit("IsQuestIgnored");

            packet.ResetBitReader();

            uint questTitleLen = packet.ReadBits(9);
            uint completionTextLen = packet.ReadBits(12);

            packet.ReadWoWString("QuestTitle", questTitleLen);
            questRequestItems.CompletionText = packet.ReadWoWString("CompletionText", completionTextLen);

            Storage.QuestRequestItems.Add(questRequestItems, packet.TimeSpan);
        }

        [Parser(Opcode.CMSG_REQUEST_WORLD_QUEST_UPDATE)]
        public static void HandleQuestZero(Packet packet) { }

        [Parser(Opcode.SMSG_QUEST_COMPLETION_NPC_RESPONSE)]
        public static void HandleQuestCompletionNPCResponse(Packet packet)
        {
            var int1 = packet.ReadInt32("QuestCompletionNPCsCount");

            // QuestCompletionNPC
            for (var i = 0; i < int1; ++i)
            {
                packet.ReadInt32("Quest Id", i);

                var int4 = packet.ReadInt32("NpcCount", i);
                for (var j = 0; j < int4; ++j)
                    packet.ReadInt32("Npc", i, j);
            }
        }

        [Parser(Opcode.SMSG_QUEST_GIVER_QUEST_LIST_MESSAGE)]
        public static void HandleQuestGiverQuestList(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid128("QuestGiverGUID");

            QuestGreeting questGreeting = new QuestGreeting
            {
                ID = guid.GetEntry(),
                GreetEmoteDelay = packet.ReadUInt32("GreetEmoteDelay"),
                GreetEmoteType = packet.ReadUInt32("GreetEmoteType")
            };

            uint gossipTextCount = packet.ReadUInt32("GossipTextCount");
            packet.ResetBitReader();
            uint greetingLen = packet.ReadBits(11);

            for (int i = 0; i < gossipTextCount; i++)
                ReadGossipText(packet, i);

            questGreeting.Greeting = packet.ReadWoWString("Greeting", greetingLen);

            switch (guid.GetObjectType())
            {
                case ObjectType.Unit:
                    questGreeting.Type = 0;
                    break;
                case ObjectType.GameObject:
                    questGreeting.Type = 1;
                    break;
            }

            Storage.QuestGreetings.Add(questGreeting, packet.TimeSpan);
        }

        [Parser(Opcode.SMSG_QUEST_GIVER_STATUS)]
        public static void HandleQuestgiverStatus(Packet packet)
        {
            packet.ReadPackedGuid128("QuestGiverGUID");
            packet.ReadInt32E<QuestGiverStatus4x>("StatusFlags");
        }

        [Parser(Opcode.SMSG_QUEST_GIVER_STATUS_MULTIPLE)]
        public static void HandleQuestgiverStatusMultiple(Packet packet)
        {
            var int16 = packet.ReadInt32("QuestGiverStatusCount");
            for (var i = 0; i < int16; ++i)
            {
                packet.ReadPackedGuid128("Guid");
                packet.ReadInt32E<QuestGiverStatus4x>("Status");
            }
        }

        [Parser(Opcode.SMSG_QUEST_POI_QUERY_RESPONSE)]
        public static void HandleQuestPoiQueryResponse(Packet packet)
        {
            packet.ReadInt32("NumPOIs");
            int int4 = packet.ReadInt32("QuestPOIData");

            for (int i = 0; i < int4; ++i)
            {
                int questId = packet.ReadInt32("QuestID", i);

                if (ClientVersion.RemovedInVersion(ClientVersionBuild.V6_2_0_20173))
                    packet.ReadUInt32("NumBlobs", i);

                int int2 = packet.ReadInt32("QuestPOIBlobData", i);

                for (int j = 0; j < int2; ++j)
                {
                    QuestPOI questPoi = new QuestPOI
                    {
                        QuestID = questId,
                        ID = j,
                        BlobIndex = packet.ReadInt32("BlobIndex", i, j),
                        ObjectiveIndex = packet.ReadInt32("ObjectiveIndex", i, j),
                        QuestObjectiveID = packet.ReadInt32("QuestObjectiveID", i, j),
                        QuestObjectID = packet.ReadInt32("QuestObjectID", i, j),
                        MapID = packet.ReadInt32("MapID", i, j),
                        WorldMapAreaId = packet.ReadInt32("WorldMapAreaID", i, j),
                        Floor = packet.ReadInt32("Floor", i, j),
                        Priority = packet.ReadInt32("Priority", i, j),
                        Flags = packet.ReadInt32("Flags", i, j),
                        WorldEffectID = packet.ReadInt32("WorldEffectID", i, j),
                        PlayerConditionID = packet.ReadInt32("PlayerConditionID", i, j)
                    };

                    if (ClientVersion.RemovedInVersion(ClientVersionBuild.V6_2_0_20173))
                        packet.ReadInt32("NumPoints", i, j);

                    questPoi.WoDUnk1 = packet.ReadInt32("WoDUnk1", i, j);

                    int int13 = packet.ReadInt32("QuestPOIBlobPoint", i, j);
                    for (int k = 0; k < int13; ++k)
                    {
                        QuestPOIPoint questPoiPoint = new QuestPOIPoint
                        {
                            QuestID = questId,
                            Idx1 = j,
                            Idx2 = k,
                            X = packet.ReadInt32("X", i, j, k),
                            Y = packet.ReadInt32("Y", i, j, k)
                        };
                        Storage.QuestPOIPoints.Add(questPoiPoint, packet.TimeSpan);
                    }

                    Storage.QuestPOIs.Add(questPoi, packet.TimeSpan);
                }
            }
        }

        [Parser(Opcode.SMSG_QUEST_SPAWN_TRACKING_UPDATE)]
        public static void HandleQuestSpawnTrackingUpdate(Packet packet)
        {
            var count = packet.ReadInt32("Count");
            for (var i = 0; i < count; i++)
            {
                packet.ReadInt32("unk1", i);
                packet.ReadInt32("ObjectID", i);
                packet.ReadBit("unk3", i);
            }
        }

        [Parser(Opcode.SMSG_QUEST_UPDATE_ADD_CREDIT)]
        public static void HandleQuestUpdateAddCredit(Packet packet)
        {
            packet.ReadPackedGuid128("VictimGUID");

            packet.ReadInt32("QuestID");
            packet.ReadInt32("ObjectID");

            packet.ReadInt16("Count");
            packet.ReadInt16("Required");

            packet.ReadByte("ObjectiveType");
        }

        [Parser(Opcode.SMSG_QUEST_UPDATE_COMPLETE)]
        [Parser(Opcode.CMSG_QUEST_CLOSE_AUTOACCEPT_QUEST)]
        public static void HandleQuestForceRemoved(Packet packet)
        {
            packet.ReadInt32<QuestId>("QuestID");
        }

        [Parser(Opcode.SMSG_WORLD_QUEST_UPDATE)]
        public static void HandleWorldQuestUpdate(Packet packet)
        {
            var count = packet.ReadInt32("Count");

            for (int i = 0; i < count; i++)
            {
                packet.ReadTime("LastUpdate", i);
                packet.ReadUInt32<QuestId>("QuestID", i);
                packet.ReadUInt32("Timer", i);
                packet.ReadInt32("VariableID", i);
                packet.ReadInt32("Value", i);
            }
        }
    }
}
