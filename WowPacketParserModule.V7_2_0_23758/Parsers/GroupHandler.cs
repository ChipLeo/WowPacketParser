using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class GroupHandler
    {
        [Parser(Opcode.SMSG_PARTY_MEMBER_STATE)]
        public static void HandlePartyMemberState(Packet packet)
        {
            packet.ReadBit("ForEnemy");

            {//65CF2F 22996
                for (var i = 0; i < 2; i++)
                    packet.ReadByte("PartyType", i);

                packet.ReadInt16E<GroupMemberStatusFlag>("Flags");

                packet.ReadByte("PowerType");
                packet.ReadInt16("OverrideDisplayPower");
                packet.ReadInt32("CurrentHealth");
                packet.ReadInt32("MaxHealth");
                packet.ReadInt16("MaxPower");
                packet.ReadInt16("MaxPower");
                packet.ReadInt16("Level");
                packet.ReadInt16("Spec");
                packet.ReadInt16("AreaID");

                packet.ReadInt16("WmoGroupID");
                packet.ReadInt32("WmoDoodadPlacementID");

                packet.ReadInt16("PositionX");
                packet.ReadInt16("PositionY");
                packet.ReadInt16("PositionZ");

                packet.ReadInt32("VehicleSeatRecID");
                var auraCount = packet.ReadInt32("AuraCount");

                packet.ReadInt32("PhaseShiftFlags");
                var int4 = packet.ReadInt32("PhaseCount");
                packet.ReadPackedGuid128("PersonalGUID");
                for (int i = 0; i < int4; i++)
                {
                    packet.ReadInt16("PhaseFlags", i);
                    packet.ReadInt16("Id", i);
                }

                for (int i = 0; i < auraCount; i++)
                {
                    packet.ReadInt32<SpellId>("Aura", i);
                    packet.ReadByte("Flags", i);
                    packet.ReadInt32("ActiveFlags", i);
                    var byte3 = packet.ReadInt32("PointsCount", i);

                    for (int j = 0; j < byte3; j++)
                        packet.ReadSingle("Points", i, j);
                }

                packet.ResetBitReader();

                var hasPet = packet.ReadBit("HasPet");
                if (hasPet) // Pet
                {//65CE39 22996
                    packet.ReadPackedGuid128("PetGuid");
                    packet.ReadInt32("PetDisplayID");
                    packet.ReadInt32("PetMaxHealth");
                    packet.ReadInt32("PetHealth");

                    var petAuraCount = packet.ReadInt32("PetAuraCount");
                    for (int i = 0; i < petAuraCount; i++)
                    {
                        packet.ReadInt32<SpellId>("PetAura", i);
                        packet.ReadByte("PetFlags", i);
                        packet.ReadInt32("PetActiveFlags", i);
                        var byte3 = packet.ReadInt32("PetPointsCount", i);

                        for (int j = 0; j < byte3; j++)
                            packet.ReadSingle("PetPoints", i, j);
                    }

                    packet.ResetBitReader();

                    var len = packet.ReadBits(8);
                    packet.ReadWoWString("PetName", len);
                }
            }
            packet.ReadPackedGuid128("MemberGuid");
        }

        [Parser(Opcode.CMSG_PARTY_INVITE, ClientVersionBuild.V7_1_0_22900)]
        public static void HandleClientPartyInvite(Packet packet)
        {
            packet.ReadByte("PartyIndex");
            packet.ReadInt32("ProposedRoles");
            packet.ReadPackedGuid128("TargetGuid");

            packet.ResetBitReader();

            var lenTargetName = packet.ReadBits(9);
            var lenTargetRealm = packet.ReadBits(9);

            packet.ReadWoWString("TargetName", lenTargetName);
            packet.ReadWoWString("TargetRealm", lenTargetRealm);
        }

        [Parser(Opcode.SMSG_PARTY_MEMBER_STATE_UPDATE)]
        public static void HandlePartyMemberStateUpdate(Packet packet)
        {
            packet.ReadBit("unk16");
            packet.ReadBit("unk17");
            var has42 = packet.ReadBit("unk42");
            var has46 = packet.ReadBit("unk46");
            var has49 = packet.ReadBit("unk49");
            var has52 = packet.ReadBit("unk52");
            var has60 = packet.ReadBit("unk60");
            var has68 = packet.ReadBit("unk68");
            var has74 = packet.ReadBit("unk74");
            var has78 = packet.ReadBit("unk78");
            var has82 = packet.ReadBit("unk82");
            var has86 = packet.ReadBit("unk86");
            var has90 = packet.ReadBit("unk90");
            var has94 = packet.ReadBit("unk94");
            var has100 = packet.ReadBit("unk100");
            var has110 = packet.ReadBit("unk110");
            var has116 = packet.ReadBit("unk116");
            var has348 = packet.ReadBit("unk348");
            var has768 = packet.ReadBit("unk768");
            var has1056 = packet.ReadBit("unk1056");
            if (has768)
            {//679E41 22996
                packet.ResetBitReader();
                var has16 = packet.ReadBit("has16");
                var has153 = packet.ReadBit("has153");
                var has160 = packet.ReadBit("has160");
                var has168 = packet.ReadBit("has168");
                var has176 = packet.ReadBit("has176");
                var has408 = packet.ReadBit("has408");
                if (has153)
                {//679E02 22996
                    packet.ResetBitReader();
                    packet.ReadWoWString("Name", packet.ReadBits(8));
                }
                if (has16)
                    packet.ReadPackedGuid128("guid16");
                if (has160)
                    packet.ReadInt32("unk160");
                if (has168)
                    packet.ReadInt32("unk168");
                if (has176)
                    packet.ReadInt32("unk176");
                if (has408)
                {//679D9E 22996
                    var petAuraCount = packet.ReadInt32("PetAuraCount408");
                    for (int i = 0; i < petAuraCount; i++)
                    {
                        packet.ReadInt32<SpellId>("PetAura", i);
                        packet.ReadByte("PetFlags", i);
                        packet.ReadInt32("PetActiveFlags", i);
                        var byte3 = packet.ReadInt32("PetPointsCount", i);

                        for (int j = 0; j < byte3; j++)
                            packet.ReadSingle("PetPoints", i, j);
                    }
                }
            }
            packet.ReadPackedGuid128("guid");
            if (has42)
            {//679F67 22996
                packet.ReadByte("unk42");
                packet.ReadByte("unk43");
            }
            if (has46)
                packet.ReadInt16("unk_46");
            if (has49)
                packet.ReadByte("unk_49");
            if (has52)
                packet.ReadInt16("unk_52");
            if (has60)
                packet.ReadInt32("unk_60");
            if (has68)
                packet.ReadInt32("unk_68");
            if (has74)
                packet.ReadInt16("unk_74");
            if (has78)
                packet.ReadInt16("unk_78");
            if (has82)
                packet.ReadInt16("unk_82");
            if (has86)
                packet.ReadInt16("unk_86");
            if (has90)
                packet.ReadInt16("unk_90");
            if (has94)
                packet.ReadInt16("unk_94");
            if (has100)
                packet.ReadInt32("unk_100");
            if (has110)
            {//65D3DB 22996
                packet.ReadInt16("unk110");
                packet.ReadInt16("unk112");
                packet.ReadInt16("unk114");
            }
            if (has116)
                packet.ReadInt32("unk_116");
            if (has348)
            {//679D9E 22996
                var petAuraCount = packet.ReadInt32("PetAuraCount348");
                for (int i = 0; i < petAuraCount; i++)
                {
                    packet.ReadInt32<SpellId>("PetAura", i);
                    packet.ReadByte("PetFlags", i);
                    packet.ReadInt32("PetActiveFlags", i);
                    var byte3 = packet.ReadInt32("PetPointsCount", i);

                    for (int j = 0; j < byte3; j++)
                        packet.ReadSingle("PetPoints", i, j);
                }
            }
            if (has1056)
            {//65DA38 22996
                packet.ReadInt32("unk1056");
                var cnt1060 = packet.ReadInt32("cnt1060");
                packet.ReadPackedGuid128("guid1064");
                for (var i = 0; i < cnt1060; ++i)
                {//68C71A 22996
                    packet.ReadInt16("unk1", i);
                    packet.ReadInt16("unk2", i);
                }
            }
        }

        [Parser(Opcode.SMSG_PARTY_INVITE)]
        public static void HandlePartyInvite(Packet packet)
        {
            packet.ReadBit("CanAccept");
            packet.ReadBit("MightCRZYou");
            packet.ReadBit("IsXRealm");
            packet.ReadBit("MustBeBNetFriend");
            packet.ReadBit("AllowMultipleRoles");
            var len = packet.ReadBits(6);

            packet.ResetBitReader();
            packet.ReadInt32("InviterVirtualRealmAddress");
            packet.ReadBit("IsLocal");
            packet.ReadBit("Unk2");
            var bits2 = packet.ReadBits(8);
            var bits258 = packet.ReadBits(8);
            packet.ReadWoWString("InviterRealmNameActual", bits2);
            packet.ReadWoWString("InviterRealmNameNormalized", bits258);

            packet.ReadPackedGuid128("InviterGuid");
            packet.ReadPackedGuid128("InviterBNetAccountID");
            packet.ReadInt16("Unk1");
            packet.ReadInt32("ProposedRoles");
            var lfgSlots = packet.ReadInt32();
            packet.ReadInt32("LfgCompletedMask");
            packet.ReadWoWString("InviterName", len);
            for (int i = 0; i < lfgSlots; i++)
                packet.ReadInt32("LfgSlots", i);
        }

        [Parser(Opcode.SMSG_PARTY_UPDATE, ClientVersionBuild.V7_1_0_22900)]
        public static void HandlePartyUpdate(Packet packet)
        {
            packet.ReadUInt16("PartyFlags");
            packet.ReadByte("PartyIndex");
            packet.ReadByte("PartyType");

            packet.ReadInt32("MyIndex");
            packet.ReadPackedGuid128("PartyGUID");
            packet.ReadInt32("SequenceNum");
            packet.ReadPackedGuid128("LeaderGUID");

            var playerCount = packet.ReadInt32("PlayerListCount");
            var hasLFG = packet.ReadBit("HasLfgInfo");
            var hasLootSettings = packet.ReadBit("HasLootSettings");
            var hasDifficultySettings = packet.ReadBit("HasDifficultySettings");

            for (int i = 0; i < playerCount; i++)
            {
                packet.ResetBitReader();
                var playerNameLength = packet.ReadBits(6);
                packet.ReadBit("FromSocialQueue", i);

                packet.ReadPackedGuid128("Guid", i);

                packet.ReadByte("Status", i);
                packet.ReadByte("Subgroup", i);
                packet.ReadByte("Flags", i);
                packet.ReadByte("RolesAssigned", i);
                packet.ReadByteE<Class>("Class", i);

                packet.ReadWoWString("Name", playerNameLength, i);
            }

            packet.ResetBitReader();

            if (hasLootSettings)
            {
                packet.ReadByte("Method", "PartyLootSettings");
                packet.ReadPackedGuid128("LootMaster", "PartyLootSettings");
                packet.ReadByte("Threshold", "PartyLootSettings");
            }

            if (hasDifficultySettings)
            {
                packet.ReadInt32("DungeonDifficultyID");
                packet.ReadInt32("RaidDifficultyID");
                packet.ReadInt32("LegacyRaidDifficultyID");
            }

            if (hasLFG)
            {
                packet.ResetBitReader();

                packet.ReadByte("MyFlags");
                packet.ReadInt32("Slot");
                packet.ReadInt32("MyRandomSlot");
                packet.ReadByte("MyPartialClear");
                packet.ReadSingle("MyGearDiff");
                packet.ReadByte("MyStrangerCount");
                packet.ReadByte("MyKickVoteCount");
                packet.ReadByte("BootCount");
                packet.ReadBit("Aborted");
                packet.ReadBit("MyFirstReward");
            }
        }
    }
}
