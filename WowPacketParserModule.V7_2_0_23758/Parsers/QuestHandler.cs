﻿using WowPacketParser.Enums;
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

        [Parser(Opcode.CMSG_QUERY_TREASURE_PICKER)]
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

        [Parser(Opcode.SMSG_DISPLAY_QUEST_POPUP)]
        public static void HandleQuestDisplayPopup(Packet packet)
        {
            packet.ReadInt32("QuestID");
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_QUERY_QUEST_INFO_RESPONSE)]
        public static void HandleQuestQueryResponse(Packet packet)
        {
            packet.ReadInt32("Entry");

            Bit hasData = packet.ReadBit("Has Data");
            if (!hasData)
                return; // nothing to do

            var id = packet.ReadEntry("Quest ID");

            QuestTemplate quest = new QuestTemplate
            {
                ID = (uint)id.Key
            };

            quest.QuestType = packet.ReadInt32E<QuestType>("QuestType");
            quest.QuestLevel = packet.ReadInt32("QuestLevel");
            quest.QuestPackageID = packet.ReadUInt32("QuestPackageID");
            quest.MinLevel = packet.ReadInt32("QuestMinLevel");
            quest.QuestSortID = (QuestSort)packet.ReadUInt32("QuestSortID");
            quest.QuestInfoID = packet.ReadInt32E<QuestInfo>("QuestInfoID");
            quest.SuggestedGroupNum = packet.ReadUInt32("SuggestedGroupNum");
            quest.RewardNextQuest = packet.ReadUInt32("RewardNextQuest");
            quest.RewardXPDifficulty = packet.ReadUInt32("RewardXPDifficulty");

            quest.RewardXPMultiplier = packet.ReadSingle("RewardXPMultiplier");

            quest.RewardMoney = packet.ReadInt32("RewardMoney");
            quest.RewardMoneyDifficulty = packet.ReadUInt32("RewardMoneyDifficulty");

            quest.RewardMoneyMultiplier = packet.ReadSingle("RewardMoneyMultiplier");

            quest.RewardBonusMoney = packet.ReadUInt32("RewardBonusMoney");

            quest.RewardDisplaySpellLegion = new uint?[3];
            for (int i = 0; i < 3; ++i)
                quest.RewardDisplaySpellLegion[i] = packet.ReadUInt32("RewardDisplaySpell", i);

            quest.RewardSpellWod = packet.ReadUInt32("RewardSpell");
            quest.RewardHonorWod = packet.ReadUInt32("RewardHonor");

            quest.RewardKillHonor = packet.ReadSingle("RewardKillHonor");

            quest.RewardArtifactXPDifficulty = packet.ReadUInt32("RewardArtifactXPDifficulty");
            quest.RewardArtifactXPMultiplier = packet.ReadSingle("RewardArtifactXPMultiplier");
            quest.RewardArtifactCategoryID = packet.ReadUInt32("RewardArtifactCategoryID");

            quest.StartItem = packet.ReadUInt32("StartItem");
            quest.Flags = packet.ReadUInt32E<QuestFlags>("Flags");
            quest.FlagsEx = packet.ReadUInt32E<QuestFlagsEx>("FlagsEx");

            quest.RewardItem = new uint?[4];
            quest.RewardAmount = new uint?[4];
            quest.ItemDrop = new uint?[4];
            quest.ItemDropQuantity = new uint?[4];
            for (int i = 0; i < 4; ++i)
            {
                quest.RewardItem[i] = packet.ReadUInt32("RewardItems", i);
                quest.RewardAmount[i] = packet.ReadUInt32("RewardAmount", i);
                quest.ItemDrop[i] = packet.ReadUInt32("ItemDrop", i);
                quest.ItemDropQuantity[i] = packet.ReadUInt32("ItemDropQuantity", i);
            }

            quest.RewardChoiceItemID = new uint?[6];
            quest.RewardChoiceItemQuantity = new uint?[6];
            quest.RewardChoiceItemDisplayID = new uint?[6];
            for (int i = 0; i < 6; ++i) // CliQuestInfoChoiceItem
            {
                quest.RewardChoiceItemID[i] = packet.ReadUInt32("RewardChoiceItemID", i);
                quest.RewardChoiceItemQuantity[i] = packet.ReadUInt32("RewardChoiceItemQuantity", i);
                quest.RewardChoiceItemDisplayID[i] = packet.ReadUInt32("RewardChoiceItemDisplayID", i);
            }

            quest.POIContinent = packet.ReadUInt32("POIContinent");

            quest.POIx = packet.ReadSingle("POIx");
            quest.POIy = packet.ReadSingle("POIy");

            quest.POIPriorityWod = packet.ReadInt32("POIPriority");
            quest.RewardTitle = packet.ReadUInt32("RewardTitle");
            quest.RewardArenaPoints = packet.ReadUInt32("RewardArenaPoints");
            quest.RewardSkillLineID = packet.ReadUInt32("RewardSkillLineID");
            quest.RewardNumSkillUps = packet.ReadUInt32("RewardNumSkillUps");
            quest.QuestGiverPortrait = packet.ReadUInt32("PortraitGiver");
            quest.QuestTurnInPortrait = packet.ReadUInt32("PortraitTurnIn");

            quest.RewardFactionID = new uint?[5];
            quest.RewardFactionOverride = new int?[5];
            quest.RewardFactionValue = new int?[5];
            quest.RewardFactionCapIn = new int?[5];
            for (int i = 0; i < 5; ++i)
            {
                quest.RewardFactionID[i] = packet.ReadUInt32("RewardFactionID", i);
                quest.RewardFactionValue[i] = packet.ReadInt32("RewardFactionValue", i);
                quest.RewardFactionOverride[i] = packet.ReadInt32("RewardFactionOverride", i);
                quest.RewardFactionCapIn[i] = packet.ReadInt32("RewardFactionCapIn", i);
            }

            quest.RewardFactionFlags = packet.ReadUInt32("RewardFactionFlags");

            quest.RewardCurrencyID = new uint?[4];
            quest.RewardCurrencyCount = new uint?[4];
            for (int i = 0; i < 4; ++i)
            {
                quest.RewardCurrencyID[i] = packet.ReadUInt32("RewardCurrencyID");
                quest.RewardCurrencyCount[i] = packet.ReadUInt32("RewardCurrencyQty");
            }

            quest.SoundAccept = packet.ReadUInt32("AcceptedSoundKitID");
            quest.SoundTurnIn = packet.ReadUInt32("CompleteSoundKitID");
            quest.AreaGroupID = packet.ReadUInt32("AreaGroupID");
            quest.TimeAllowed = packet.ReadUInt32("TimeAllowed");
            uint int2946 = packet.ReadUInt32("CliQuestInfoObjective");
            quest.AllowableRacesWod = (uint)packet.ReadInt32("AllowableRaces");
            quest.QuestRewardID = packet.ReadInt32("QuestRewardID");
            packet.ReadInt32("unk");

            packet.ResetBitReader();

            uint logTitleLen = packet.ReadBits(9);
            uint logDescriptionLen = packet.ReadBits(12);
            uint questDescriptionLen = packet.ReadBits(12);
            uint areaDescriptionLen = packet.ReadBits(9);
            uint questGiverTextWindowLen = packet.ReadBits(10);
            uint questGiverTargetNameLen = packet.ReadBits(8);
            uint questTurnTextWindowLen = packet.ReadBits(10);
            uint questTurnTargetNameLen = packet.ReadBits(8);
            uint questCompletionLogLen = packet.ReadBits(11);

            //sub_66D5FE
            for (int i = 0; i < int2946; ++i)
            {
                var objectiveId = packet.ReadEntry("Id", i);

                QuestObjective questInfoObjective = new QuestObjective
                {
                    ID = (uint)objectiveId.Key,
                    QuestID = (uint)id.Key
                };
                questInfoObjective.Type = packet.ReadByteE<QuestRequirementType>("Quest Requirement Type", i);
                questInfoObjective.StorageIndex = packet.ReadSByte("StorageIndex", i);
                questInfoObjective.ObjectID = packet.ReadInt32("ObjectID", i);
                questInfoObjective.Amount = packet.ReadInt32("Amount", i);
                questInfoObjective.Flags = packet.ReadUInt32("Flags", i);
                if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_1_0_22900))
                    questInfoObjective.Flags2 = packet.ReadUInt32("Flags2", i);
                questInfoObjective.ProgressBarWeight = packet.ReadSingle("ProgressBarWeight", i);

                int visualEffectsCount = packet.ReadInt32("VisualEffects", i);
                for (int j = 0; j < visualEffectsCount; ++j)
                {
                    QuestVisualEffect questVisualEffect = new QuestVisualEffect
                    {
                        ID = questInfoObjective.ID,
                        Index = (uint)j,
                        VisualEffect = packet.ReadInt32("VisualEffectId", i, j)
                    };

                    Storage.QuestVisualEffects.Add(questVisualEffect, packet.TimeSpan);
                }

                packet.ResetBitReader();

                uint bits6 = packet.ReadBits(8);
                questInfoObjective.Description = packet.ReadWoWString("Description", bits6, i);

                if (ClientLocale.PacketLocale != LocaleConstant.enUS && questInfoObjective.Description != string.Empty)
                {
                    QuestObjectivesLocale localesQuestObjectives = new QuestObjectivesLocale
                    {
                        ID = (uint)objectiveId.Key,
                        QuestId = (uint)id.Key,
                        StorageIndex = questInfoObjective.StorageIndex,
                        Description = questInfoObjective.Description
                    };

                    Storage.LocalesQuestObjectives.Add(localesQuestObjectives, packet.TimeSpan);
                }

                Storage.QuestObjectives.Add((uint)questInfoObjective.ID, questInfoObjective, packet.TimeSpan);
            }

            quest.LogTitle = packet.ReadWoWString("LogTitle", logTitleLen);
            quest.LogDescription = packet.ReadWoWString("LogDescription", logDescriptionLen);
            quest.QuestDescription = packet.ReadWoWString("QuestDescription", questDescriptionLen);
            quest.AreaDescription = packet.ReadWoWString("AreaDescription", areaDescriptionLen);
            quest.QuestGiverTextWindow = packet.ReadWoWString("PortraitGiverText", questGiverTextWindowLen);
            quest.QuestGiverTargetName = packet.ReadWoWString("PortraitGiverName", questGiverTargetNameLen);
            quest.QuestTurnTextWindow = packet.ReadWoWString("PortraitTurnInText", questTurnTextWindowLen);
            quest.QuestTurnTargetName = packet.ReadWoWString("PortraitTurnInName", questTurnTargetNameLen);
            quest.QuestCompletionLog = packet.ReadWoWString("QuestCompletionLog", questCompletionLogLen);

            if (ClientLocale.PacketLocale != LocaleConstant.enUS)
            {
                LocalesQuest localesQuest = new LocalesQuest
                {
                    ID = (uint)id.Key,
                    LogTitle = quest.LogTitle,
                    LogDescription = quest.LogDescription,
                    QuestDescription = quest.QuestDescription,
                    AreaDescription = quest.AreaDescription,
                    PortraitGiverText = quest.QuestGiverTextWindow,
                    PortraitGiverName = quest.QuestGiverTargetName,
                    PortraitTurnInText = quest.QuestTurnTextWindow,
                    PortraitTurnInName = quest.QuestTurnTargetName,
                    QuestCompletionLog = quest.QuestCompletionLog
                };

                Storage.LocalesQuests.Add(localesQuest, packet.TimeSpan);
            }

            Storage.QuestTemplates.Add(quest, packet.TimeSpan);
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
            questOfferReward.Emote = new int?[] { 0, 0, 0, 0 };
            questOfferReward.EmoteDelay = new uint?[] { 0, 0, 0, 0 };
            for (int i = 0; i < emotesCount; i++)
            {
                questOfferReward.Emote[i] = packet.ReadInt32("Type");
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

        [Parser(Opcode.SMSG_QUEST_GIVER_QUEST_COMPLETE)]
        public static void HandleQuestCompleted(Packet packet)
        {
            packet.ReadUInt32("QuestId");
            packet.ReadUInt32("SkillLineIDReward");
            packet.ReadUInt64("MoneyReward");
            //packet.ReadUInt32("NumSkillUpsReward");
            packet.ReadUInt32("XpReward");
            packet.ReadUInt32("TalentReward");

            packet.ResetBitReader();

            packet.ReadBit("UseQuestReward");
            packet.ReadBit("LaunchGossip");
            packet.ReadBit("unk1");
            packet.ReadBit("unk2");

            ItemHandler.ReadItemInstance(packet);
        }

        [Parser(Opcode.SMSG_QUEST_GIVER_QUEST_DETAILS)]
        public static void HandleQuestGiverQuestDetails(Packet packet)
        {
            packet.ReadPackedGuid128("QuestGiverGUID");
            packet.ReadPackedGuid128("InformUnit");

            int id = packet.ReadInt32("QuestID");
            QuestDetails questDetails = new QuestDetails
            {
                ID = (uint)id
            };

            packet.ReadInt32("QuestPackageID");
            packet.ReadInt32("PortraitGiver");
            packet.ReadInt32("SuggestedPartyMembers");

            for (int i = 0; i < 2; i++)
                packet.ReadInt32("QuestFlags", i);

            packet.ReadInt32("PortraitTurnIn");
            int learnSpellsCount = packet.ReadInt32("LearnSpellsCount");

            int descEmotesCount = packet.ReadInt32("DescEmotesCount");
            int objectivesCount = packet.ReadInt32("ObjectivesCount");
            packet.ReadInt32("QuestStartItemID");

            for (int i = 0; i < learnSpellsCount; i++)
                packet.ReadInt32("LearnSpells", i);

            questDetails.Emote = new uint?[] { 0, 0, 0, 0 };
            questDetails.EmoteDelay = new uint?[] { 0, 0, 0, 0 };
            for (int i = 0; i < descEmotesCount; i++)
            {
                questDetails.Emote[i] = (uint)packet.ReadInt32("Type", i);
                questDetails.EmoteDelay[i] = packet.ReadUInt32("Delay", i);
            }

            for (int i = 0; i < objectivesCount; i++)
            {
                packet.ReadInt32("ObjectID", i);
                packet.ReadInt32("ObjectID", i);
                packet.ReadInt32("Amount", i);
                packet.ReadByte("Type", i);
            }

            packet.ResetBitReader();

            uint questTitleLen = packet.ReadBits(9);
            uint descriptionTextLen = packet.ReadBits(12);
            uint logDescriptionLen = packet.ReadBits(12);
            uint portraitGiverTextLen = packet.ReadBits(10);
            uint portraitGiverNameLen = packet.ReadBits(8);
            uint portraitTurnInTextLen = packet.ReadBits(10);
            uint portraitTurnInNameLen = packet.ReadBits(8);

//            packet.ReadBit("DisplayPopup");
//            packet.ReadBit("StartCheat");
            packet.ReadBit("AutoLaunched");
            packet.ReadBit("CanIgnoreQuest");
            packet.ReadBit("IsQuestIgnored");

            ReadQuestRewards(packet, "QuestRewards");

            packet.ReadWoWString("QuestTitle", questTitleLen);
            packet.ReadWoWString("DescriptionText", descriptionTextLen);
            packet.ReadWoWString("LogDescription", logDescriptionLen);
            packet.ReadWoWString("PortraitGiverText", portraitGiverTextLen);
            packet.ReadWoWString("PortraitGiverName", portraitGiverNameLen);
            packet.ReadWoWString("PortraitTurnInText", portraitTurnInTextLen);
            packet.ReadWoWString("PortraitTurnInName", portraitTurnInNameLen);

            Storage.QuestDetails.Add(questDetails, packet.TimeSpan);
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

                    questPoi.SpawnTrackingID = packet.ReadInt32("WoDUnk1", i, j);

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
    }
}
