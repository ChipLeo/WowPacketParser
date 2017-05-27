using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class BattlePetHandler
    {
        public static void ReadPetBattleActiveAbility(Packet packet, params object[] idx)
        {
            packet.ReadInt32("AbilityID", idx);
            packet.ReadInt16("CooldownRemaining", idx);
            packet.ReadInt16("LockdownRemaining", idx);
            packet.ReadByte("AbilityIndex", idx);
            packet.ReadByte("Pboid", idx);
        }

        public static void ReadClientBattlePet(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("BattlePetGUID", idx);

            packet.ReadInt32("SpeciesID", idx);
            packet.ReadInt32("DisplayID", idx);
            packet.ReadInt32("CollarID", idx);

            packet.ReadInt16("BreedID", idx);
            packet.ReadInt16("Level", idx);
            packet.ReadInt16("Xp", idx);
            packet.ReadInt16("BattlePetDBFlags", idx);

            packet.ReadInt32("Power", idx);
            packet.ReadInt32("Health", idx);
            packet.ReadInt32("MaxHealth", idx);
            packet.ReadInt32("Speed", idx);

            packet.ReadByte("BreedQuality", idx);

            packet.ResetBitReader();

            var customNameLength = packet.ReadBits(7);
            var hasOwnerInfo = packet.ReadBit("HasOwnerInfo", idx);
            packet.ReadBit("NoRename", idx);

            packet.ReadWoWString("CustomName", customNameLength, idx);

            if (hasOwnerInfo)
                V6_0_2_19033.Parsers.BattlePetHandler.ReadClientBattlePetOwnerInfo(packet, "OwnerInfo", idx);
        }

        private static readonly int[] _petBattleEffectTargets624 = { 0, 1, 2, 3, 4, 5, 6, 7 };

        public static void ReadPetBattleEffectTarget(Packet packet, int[] targetTypeValueMap, params object[] idx)
        {
            packet.ResetBitReader();
            var type = packet.ReadBits("Type", 3, idx); // enum PetBattleEffectTargetEx

            packet.ResetBitReader();

            packet.ReadByte("Petx", idx);

            switch (targetTypeValueMap[type])
            {
                case 1:
                    packet.ReadInt32("AuraInstanceID", idx);
                    packet.ReadInt32("AuraAbilityID", idx);
                    packet.ReadInt32("RoundsRemaining", idx);
                    packet.ReadInt32("CurrentRound", idx);
                    break;
                case 2:
                    packet.ReadInt32("StateID", idx);
                    packet.ReadInt32("StateValue", idx);
                    break;
                case 3:
                    packet.ReadInt32("Health", idx);
                    break;
                case 4:
                    packet.ReadInt32("NewStatValue", idx);
                    break;
                case 5:
                    packet.ReadInt32("TriggerAbilityID", idx);
                    break;
                case 6:
                    packet.ReadInt32("ChangedAbilityID", idx);
                    packet.ReadInt32("CooldownRemaining", idx);
                    packet.ReadInt32("LockdownRemaining", idx);
                    break;
                case 7:
                    packet.ReadInt32("BroadcastTextID", idx);
                    break;
            }
        }

        public static void ReadPetBattleEffect(Packet packet, params object[] idx)
        {
            packet.ReadUInt32("AbilityEffectID", idx);
            packet.ReadUInt16("Flags", idx);
            packet.ReadUInt16("SourceAuraInstanceID", idx);
            packet.ReadUInt16("TurnInstanceID", idx);
            packet.ReadSByte("PetBattleEffectType", idx);
            packet.ReadByte("CasterPBOID", idx);
            packet.ReadByte("StackDepth", idx);

            var targetsCount = packet.ReadInt32("TargetsCount", idx);

            var targetTypeValueMap = _petBattleEffectTargets624;

            for (var i = 0; i < targetsCount; ++i)
                ReadPetBattleEffectTarget(packet, targetTypeValueMap, i);
        }

        public static void ReadPetBattleRoundResult(Packet packet, params object[] idx)
        {
            packet.ReadInt32("CurRound", idx);
            packet.ReadSByte("NextPetBattleState", idx);

            var effectsCount = packet.ReadInt32("EffectsCount", idx);

            for (var i = 0; i < 2; ++i)
            {
                packet.ReadByte("NextInputFlags", idx, i);
                packet.ReadSByte("NextTrapStatus", idx, i);
                packet.ReadUInt16("RoundTimeSecs", idx, i);
            }

            var cooldownsCount = packet.ReadInt32("CooldownsCount", idx);

            for (var i = 0; i < cooldownsCount; ++i)
                ReadPetBattleActiveAbility(packet, idx, "Cooldowns", i);

            packet.ResetBitReader();

            var petXDiedCount = packet.ReadBits("PetXDied", 3, idx);

            packet.ResetBitReader();

            for (var i = 0; i < effectsCount; ++i)
                ReadPetBattleEffect(packet, idx, "Effects", i);

            for (var i = 0; i < petXDiedCount; ++i)
                packet.ReadSByte("PetXDied", idx, i);
        }

        [Parser(Opcode.CMSG_BATTLE_PET_UPDATE_NOTIFY)]
        public static void HandleBattlePetUpdateNotify(Packet packet)
        {
            packet.ReadPackedGuid128("guid");
        }

        [Parser(Opcode.SMSG_BATTLE_PET_JOURNAL)]
        public static void HandleBattlePetJournal(Packet packet)
        {
            packet.ReadInt16("TrapLevel");

            var slotsCount = packet.ReadInt32("SlotsCount");
            var petsCount = packet.ReadInt32("PetsCount");
            packet.ReadInt32("MaxPets");

            packet.ReadBit("HasJournalLock");
            packet.ResetBitReader();

            for (var i = 0; i < slotsCount; i++)
                V6_0_2_19033.Parsers.BattlePetHandler.ReadClientPetBattleSlot(packet, i);

            for (var i = 0; i < petsCount; i++)
                ReadClientBattlePet(packet, i);
        }

        [Parser(Opcode.SMSG_BATTLE_PET_UPDATES)]
        public static void HandleBattlePetUpdates(Packet packet)
        {
            var petsCount = packet.ReadInt32("PetsCount");
            packet.ReadBit("AddedPet");
            packet.ResetBitReader();

            for (var i = 0; i < petsCount; ++i)
                ReadClientBattlePet(packet, i);
        }

        [Parser(Opcode.SMSG_PET_BATTLE_SLOT_UPDATES)]
        public static void HandlePetBattleSlotUpdates(Packet packet)
        {
            var petBattleSlotCount = packet.ReadInt32("PetBattleSlotCount");

            packet.ReadBit("NewSlotUnlocked");
            packet.ReadBit("AutoSlotted");

            for (int i = 0; i < petBattleSlotCount; i++)
                V6_0_2_19033.Parsers.BattlePetHandler.ReadClientPetBattleSlot(packet, i, "PetBattleSlot");
        }

        [Parser(Opcode.SMSG_BATTLE_PET_ERROR)]
        public static void HandleBattlePetError(Packet packet)
        {
            packet.ReadBits("Result", 3);
            packet.ReadInt32("CreatureID");
        }

        [Parser(Opcode.CMSG_BATTLE_PET_MODIFY_NAME, ClientVersionBuild.V7_2_0_23826)]
        public static void HandleBattlePetModifyName(Packet packet)
        {
            packet.ReadPackedGuid128("BattlePetGUID");

            packet.ResetBitReader();

            var nameLen = packet.ReadBits(7);
            var hasDeclinedNames = packet.ReadBit("HasDeclinedNames");

            if (hasDeclinedNames)
            {
                var declinedNamesLen = new uint[5];
                for (int i = 0; i < 5; i++)
                    declinedNamesLen[i] = packet.ReadBits(7);

                for (int i = 0; i < 5; i++)
                    packet.ReadWoWString("DeclinedNames", declinedNamesLen[i]);
            }

            packet.ReadWoWString("Name", nameLen);
        }

        [Parser(Opcode.SMSG_PET_BATTLE_FIRST_ROUND)]
        [Parser(Opcode.SMSG_PET_BATTLE_ROUND_RESULT)]
        [Parser(Opcode.SMSG_PET_BATTLE_REPLACEMENTS_MADE)]
        public static void HandlePetBattleRound(Packet packet)
        {
            ReadPetBattleRoundResult(packet, "MsgData");
        }
    }
}
