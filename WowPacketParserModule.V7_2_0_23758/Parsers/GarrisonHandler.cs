using System.Reflection.Emit;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class GarrisonHandler
    {
        public static void ReadGarrisonMissionOvermaxReward(Packet packet, params object[] indexes) //65CD05 22996
        {
            packet.ReadInt32<ItemId>("ItemID", indexes);
            packet.ReadUInt32("Quantity", indexes);
            packet.ReadInt32("CurrencyID", indexes);
            packet.ReadUInt32("CurrencyQuantity", indexes);
            packet.ReadUInt32("FollowerXP", indexes);
            packet.ReadUInt32("BonusAbilityID", indexes);
            packet.ReadInt32("Unknown", indexes);
        }

        public static void ReadGarrisonMission(Packet packet, params object[] indexes)
        {
            packet.ReadUInt64("DbID", indexes);
            packet.ReadUInt32("MissionRecID", indexes);
            packet.ReadTime("OfferTime", indexes);
            packet.ReadUInt32("OfferDuration", indexes);
            packet.ReadTime("StartTime", indexes);
            packet.ReadUInt32("TravelDuration", indexes);
            packet.ReadUInt32("MissionDuration", indexes);
            packet.ReadUInt32("MissionState", indexes);
            packet.ReadUInt32("Unknown1", indexes);
            packet.ReadUInt32("Unknown2", indexes);
        }

        public static void ReadGarrisonMissionOvermaxRewards(Packet packet, params object[] indexes)
        {
            var missionRewardCount = packet.ReadInt32("MissionRewardCount", indexes);
            for (int i = 0; i < missionRewardCount; i++)
            {
                packet.ReadInt32<ItemId>("ItemID", indexes, i);
                packet.ReadUInt32("Quantity", indexes, i);
                packet.ReadInt32("CurrencyID", indexes, i);
                packet.ReadUInt32("CurrencyQuantity", indexes, i);
                packet.ReadUInt32("FollowerXP", indexes, i);
                packet.ReadUInt32("BonusAbilityID", indexes, i);
                packet.ReadInt32("Unknown", indexes, i);
            }
        }

        public static void ReadGarrisonBuildingInfo(Packet packet, params object[] indexes)
        {
            packet.ReadUInt32("GarrPlotInstanceID", indexes);
            packet.ReadUInt32("GarrBuildingID", indexes);
            packet.ReadUInt32("TimeBuilt", indexes);
            packet.ReadUInt32("CurrentGarSpecID", indexes);
            packet.ReadUInt32("TimeSpecCooldown", indexes);

            packet.ResetBitReader();

            packet.ReadBit("Active", indexes);
        }

        public static void ReadGarrisonFollower(Packet packet, params object[] indexes)
        {
            packet.ReadUInt64("DbID", indexes);
            packet.ReadUInt32("GarrFollowerID", indexes);
            packet.ReadUInt32("Quality", indexes);
            packet.ReadUInt32("FollowerLevel", indexes);
            packet.ReadUInt32("ItemLevelWeapon", indexes);
            packet.ReadUInt32("ItemLevelArmor", indexes);
            packet.ReadUInt32("Xp", indexes);
            packet.ReadUInt32("Durability", indexes);
            packet.ReadUInt32("CurrentBuildingID", indexes);
            packet.ReadUInt32("CurrentMissionID", indexes);
            var abilityCount = packet.ReadInt32("AbilityCount", indexes);
            packet.ReadInt32("ZoneSupportSpellID", indexes);
            packet.ReadInt32("FollowerStatus", indexes);

            for (int i = 0; i < abilityCount; i++)
                packet.ReadInt32("AbilityID", indexes, i);

            packet.ResetBitReader();

            packet.ReadWoWString("CustomName", packet.ReadBits(7), indexes);
        }

        public static void ReadGarrisonMissionAreaBonus(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("GarrMssnBonusAbilityID", indexes);
            packet.ReadInt32("StartTime", indexes);
        }

        public static void ReadGarrisonPlotInfo(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("GarrPlotInstanceID", indexes);
            packet.ReadVector4("PlotPos", indexes);
            packet.ReadInt32("PlotType", indexes);
        }

        public static void ReadGarrisonTalents(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("GarrTalentID", indexes);
            packet.ReadInt32("ResearchStartTime", indexes);
            packet.ReadInt32("Flags", indexes);
        }

        [Parser(Opcode.CMSG_GARRISON_CHECK_UPGRADEABLE)]
        public static void HandleGarrisonCheckUpgradeable(Packet packet)
        {
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.CMSG_GARRISON_RESEARCH_TALENT)]
        public static void HandleGarrisonResearchTalent(Packet packet)
        {
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.SMSG_DISPLAY_TOAST)]
        public static void HandleDisplayToast(Packet packet)
        {
            packet.ReadUInt64("Quantity");

            packet.ReadByte("DisplayToastMethod");
            packet.ReadUInt32("QuestID");

            packet.ResetBitReader();

            packet.ReadBit("Mailed");

            var type = packet.ReadBits("Type", 2);
            if (type == 0)
            {
                packet.ReadBit("BonusRoll");
                Parsers.ItemHandler.ReadItemInstance(packet);
                packet.ReadInt32("SpecializationID");
                packet.ReadInt32("ItemQuantity?");
            }

            if (type == 1)
                packet.ReadInt32("CurrencyID");
        }

        [Parser(Opcode.SMSG_GARRISON_ADD_FOLLOWER_RESULT)]
        public static void HandleGarrisonAddFollowerResult(Packet packet)
        {
            packet.ReadInt32("Result");
            packet.ReadInt32("unk");
            ReadGarrisonFollower(packet);
        }

        [Parser(Opcode.SMSG_GARRISON_ADD_MISSION_RESULT)]
        public static void HandleGarrisonAddMissionResult(Packet packet)
        {
            packet.ReadInt32("Result");
            packet.ReadByte("unk");
            packet.ReadInt32("unk2");
            ReadGarrisonMission(packet);
            var cnt1 = packet.ReadInt32("cnt1");
            var cnt2 = packet.ReadInt32("cnt2");
            for (var i = 0; i < cnt1; ++i)
                ReadGarrisonMissionOvermaxReward(packet, i);
            for (var i = 0; i < cnt2; ++i)
                ReadGarrisonMissionOvermaxReward(packet, i);
            packet.ReadBit("unkbit");
        }

        [Parser(Opcode.SMSG_GARRISON_FOLLOWER_CHANGED_XP)] // GARRISON_FOLLOWER_XP_CHANGED
        public static void HandleGarrisonUnk2(Packet packet)
        {
            packet.ReadInt32("Result");
            packet.ReadInt32("unk");
            ReadGarrisonFollower(packet);
            ReadGarrisonFollower(packet);
        }

        [Parser(Opcode.SMSG_GARRISON_LEARN_BLUEPRINT_RESULT)]
        public static void HandleGarrisonLearnBlueprintResult(Packet packet)
        {
            packet.ReadInt32("Result");
            packet.ReadInt32("BuildingID");
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.SMSG_GARRISON_MISSION_BONUS_ROLL_RESULT)]
        public static void HandleGarrisonMissionBonusRollResult(Packet packet)
        {
            ReadGarrisonMission(packet);

            packet.ReadInt32("MissionRecID");
            packet.ReadInt32("Result");
        }

        [Parser(Opcode.SMSG_GARRISON_OPEN_MISSION_NPC)]
        public static void HandleGarrisonOpenMissionNpc(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
            packet.ReadInt32("unk1");
        }

        [Parser(Opcode.SMSG_GARRISON_OPEN_TALENT_NPC)]
        public static void HandleGarrisonOpenTalentNpc(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
        }

        [Parser(Opcode.SMSG_GARRISON_PLACE_BUILDING_RESULT)]
        public static void HandleGarrisonPlaceBuildingResult(Packet packet)
        {
            packet.ReadInt32("Result");
            packet.ReadInt32("unk");
            ReadGarrisonBuildingInfo(packet, "BuildingInfo");

            packet.ResetBitReader();

            packet.ReadBit("UnkBit");
        }

        [Parser(Opcode.SMSG_GARRISON_REMOVE_FOLLOWER_RESULT)]
        public static void HandleGarrisonRemoveFollowerResult(Packet packet)
        {
            packet.ReadInt64("FollowerDBID");
            packet.ReadInt32("unk1");
            packet.ReadInt32("unk2");
            packet.ReadInt32("unk3");
        }

        [Parser(Opcode.SMSG_GARRISON_REQUEST_BLUEPRINT_AND_SPECIALIZATION_DATA_RESULT)]
        public static void HandleGarrisonRequestBlueprintAndSpecializationDataResult(Packet packet)
        {
            packet.ReadInt32("unk16");
            var int20 = packet.ReadInt32("SpecializationsKnownCount");
            var int36 = packet.ReadInt32("BlueprintsKnownCount");

            for (var i = 0; i < int20; i++)
                packet.ReadInt32("SpecializationsKnown", i);

            for (var i = 0; i < int36; i++)
                packet.ReadInt32("BlueprintsKnown", i);
        }

        [Parser(Opcode.SMSG_GARRISON_START_MISSION_RESULT)]
        public static void HandleGarrisonStartMissionResult(Packet packet)
        {
            packet.ReadInt32("Result");
            packet.ReadInt16("unk");
            ReadGarrisonMission(packet);

            var followerCount = packet.ReadInt32("FollowerCount");
            for (int i = 0; i < followerCount; i++)
                packet.ReadInt64("FollowerDBIDs");
        }
        public static void ReadFollowerSoftCapInfo(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("GarrFollowerTypeID", indexes);
            packet.ReadUInt32("Count", indexes);
        }

        [Parser(Opcode.SMSG_GARRISON_UNK4)]
        public static void HandleGarrisonUnk4(Packet packet)
        {
            packet.ReadInt32("unk1");
            packet.ReadInt32("unk2");
            packet.ReadInt32("unk3");
        }

        [Parser(Opcode.SMSG_GARRISON_MISSION_COMPLETE_RESPONSE)]
        public static void HandleGarrisonMissionCompleteResponse(Packet packet)
        {
            packet.ReadInt32("CurrentBuildingID");
            ReadGarrisonMission(packet);
            packet.ReadInt32("CurrentMissionID");
            var count = packet.ReadInt32("Count");
            for (var i = 0; i < count; i++)
            {
                packet.ReadInt64("DbID", i);
                packet.ReadInt32("unk5", i);
            }
            packet.ResetBitReader();
            packet.ReadBit("unk3");
        }

        [Parser(Opcode.CMSG_GET_GARRISON_INFO)]
        [Parser(Opcode.CMSG_GARRISON_REQUEST_LANDING_PAGE_SHIPMENT_INFO)]
        [Parser(Opcode.CMSG_GARRISON_REQUEST_BLUEPRINT_AND_SPECIALIZATION_DATA)]
        [Parser(Opcode.CMSG_GARRISON_UNK1)]
        public static void HandleGarrisonZero(Packet packet)
        {
        }
    }
}
