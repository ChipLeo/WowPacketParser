using System.Reflection.Emit;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class GarrisonHandler
    {
        private static void ReadGarrisonMission(Packet packet, params object[] indexes)
        {
            packet.ReadInt64("DbID", indexes);
            packet.ReadInt32("MissionRecID", indexes);

            packet.ReadTime("OfferTime", indexes);
            packet.ReadTime("StartTime", indexes);
            packet.ReadUInt32("OfferDuration", indexes);
            packet.ReadUInt32("TravelDuration", indexes);
            packet.ReadUInt32("MissionDuration", indexes);

            packet.ReadInt32("MissionState", indexes);
        }

        private static void ReadGarrisonBuildingInfo(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("GarrPlotInstanceID", indexes);
            packet.ReadInt32("GarrBuildingID", indexes);
            packet.ReadTime("TimeBuilt", indexes);
            packet.ReadInt32("CurrentGarSpecID", indexes);
            packet.ReadTime("TimeSpecCooldown", indexes);

            packet.ResetBitReader();

            packet.ReadBit("Active", indexes);
        }

        private static void ReadGarrisonFollower(Packet packet, params object[] indexes)
        {
            packet.ReadInt64("DbID", indexes);

            packet.ReadInt32("GarrFollowerID", indexes);
            packet.ReadInt32("Quality", indexes);
            packet.ReadInt32("FollowerLevel", indexes);
            packet.ReadInt32("ItemLevelWeapon", indexes);
            packet.ReadInt32("ItemLevelArmor", indexes);
            packet.ReadInt32("Xp", indexes);
            packet.ReadInt32("CurrentBuildingID", indexes);
            packet.ReadInt32("CurrentMissionID", indexes);
            var int40 = packet.ReadInt32("AbilityCount", indexes);
            packet.ReadInt32("FollowerStatus", indexes);

            for (int i = 0; i < int40; i++)
                packet.ReadInt32("AbilityID", indexes, i);

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V6_2_0_20173))
            {
                packet.ResetBitReader();

                var len = packet.ReadBits(7);
                packet.ReadWoWString("CustomName", len, indexes);
            }
        }

        public static void ReadGarrisonPlotInfo(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("GarrPlotInstanceID", indexes);
            packet.ReadVector4("PlotPos", indexes);
            packet.ReadInt32("PlotType", indexes);
        }

        private static void ReadCharacterShipment60x(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("ShipmentRecID", indexes);
            packet.ReadInt64("ShipmentID", indexes);
            packet.ReadTime("CreationTime", indexes);
            packet.ReadInt32("ShipmentDuration", indexes);
        }

        private static void ReadCharacterShipment61x(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("ShipmentRecID", indexes);
            packet.ReadInt64("ShipmentID", indexes);
            packet.ReadInt64("Unk2", indexes);
            packet.ReadTime("CreationTime", indexes);
            packet.ReadInt32("ShipmentDuration", indexes);
            packet.ReadInt32("Unk8", indexes);
        }

        public static void ReadGarrisonMissionAreaBonus(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("GarrMssnBonusAbilityID", indexes);
            packet.ReadInt32("StartTime", indexes);
        }

        [Parser(Opcode.CMSG_GET_GARRISON_INFO)]
        [Parser(Opcode.CMSG_GARRISON_REQUEST_LANDING_PAGE_SHIPMENT_INFO)]
        [Parser(Opcode.CMSG_GARRISON_REQUEST_BLUEPRINT_AND_SPECIALIZATION_DATA)]
        [Parser(Opcode.CMSG_GARRISON_CHECK_UPGRADEABLE)]
        [Parser(Opcode.CMSG_GARRISON_UNK1)]
        [Parser(Opcode.CMSG_GARRISON_GET_BUILDING_LANDMARKS)]
        public static void HandleGarrisonZero(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_DISPLAY_TOAST)]
        public static void HandleDisplayToast(Packet packet)
        {
            packet.ReadBit("unk16");
            var unk20 = packet.ReadBits("unk20", 2);
            var unk52 = !packet.ReadBit("!unk52");
            if (unk20 == 0 && !packet.ReadBit("unk44"))
            {
                packet.ReadInt32("unk40"); // 40
                packet.ReadInt32("Suffix factor"); // 24
                packet.ReadInt32("unk36"); // 36
                packet.ReadInt32("unk32"); // 32
                packet.ReadUInt32<ItemId>("Entry"); // 48
                packet.ReadInt32("unk28"); // 28
            }
            packet.ReadInt32("unk56"); // 56
            if (unk52)
                packet.ReadByte("unk52"); // 52
            if (unk20 == 2)
                packet.ReadInt32("unk60"); // 60
        }
    }
}
