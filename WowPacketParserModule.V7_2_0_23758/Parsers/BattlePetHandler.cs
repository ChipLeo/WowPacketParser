using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class BattlePetHandler
    {
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
                Parsers.BattlePetHandler.ReadClientBattlePetOwnerInfo(packet, "OwnerInfo", idx);
        }

        public static void ReadClientPetBattleSlot(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("BattlePetGUID", idx);

            packet.ReadInt32("CollarID", idx);
            packet.ReadByte("SlotIndex", idx);

            packet.ResetBitReader();

            packet.ReadBit("Locked", idx);
        }

        public static void ReadClientBattlePetOwnerInfo(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("Guid", idx);
            packet.ReadUInt32("PlayerVirtualRealm", idx);
            packet.ReadUInt32("PlayerNativeRealm", idx);
        }
        public static void ReadPetBattlePetUpdate(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("BattlePetGUID", idx);

            packet.ReadInt32("SpeciesID", idx);
            packet.ReadInt32("DisplayID", idx);
            packet.ReadInt32("CollarID", idx);

            packet.ReadInt16("Level", idx);
            packet.ReadInt16("Xp", idx);

            packet.ReadInt32("CurHealth", idx);
            packet.ReadInt32("MaxHealth", idx);
            packet.ReadInt32("Power", idx);
            packet.ReadInt32("Speed", idx);
            packet.ReadInt32("NpcTeamMemberID", idx);

            packet.ReadInt16("BreedQuality", idx);
            packet.ReadInt16("StatusFlags", idx);

            packet.ReadByte("Slot", idx);

            var abilitiesCount = packet.ReadInt32("AbilitiesCount", idx);
            var aurasCount = packet.ReadInt32("AurasCount", idx);
            var statesCount = packet.ReadInt32("StatesCount", idx);

            for (var i = 0; i < abilitiesCount; ++i)
                ReadPetBattleActiveAbility(packet, idx, "Abilities", i);

            for (var i = 0; i < aurasCount; ++i)
                ReadPetBattleActiveAura(packet, idx, "Auras", i);

            for (var i = 0; i < statesCount; ++i)
                ReadPetBattleActiveState(packet, idx, "States", i);

            packet.ResetBitReader();

            var bits57 = packet.ReadBits(7);
            packet.ReadWoWString("CustomName", bits57, idx);
        }

        public static void ReadPetBattlePlayerUpdate(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("CharacterID", idx);

            packet.ReadInt32("TrapAbilityID", idx);
            packet.ReadInt32("TrapStatus", idx);

            packet.ReadInt16("RoundTimeSecs", idx);

            packet.ReadSByte("FrontPet", idx);
            packet.ReadByte("InputFlags", idx);

            packet.ResetBitReader();

            var petsCount = packet.ReadBits("PetsCount", 2, idx);

            for (var i = 0; i < petsCount; ++i)
                ReadPetBattlePetUpdate(packet, idx, "Pets", i);
        }

        public static void ReadPetBattleActiveState(Packet packet, params object[] idx)
        {
            packet.ReadInt32("StateID", idx);
            packet.ReadInt32("StateValue", idx);
        }

        public static void ReadPetBattleActiveAbility(Packet packet, params object[] idx)
        {
            packet.ReadInt32("AbilityID", idx);
            packet.ReadInt16("CooldownRemaining", idx);
            packet.ReadInt16("LockdownRemaining", idx);
            packet.ReadByte("AbilityIndex", idx);
            packet.ReadByte("Pboid", idx);
        }

        public static void ReadPetBattleActiveAura(Packet packet, params object[] idx)
        {
            packet.ReadInt32("AbilityID", idx);
            packet.ReadInt32("InstanceID", idx);
            packet.ReadInt32("RoundsRemaining", idx);
            packet.ReadInt32("CurrentRound", idx);
            packet.ReadByte("CasterPBOID", idx);
        }

        public static void ReadPetBattleEnviroUpdate(Packet packet, params object[] idx)
        {
            var aurasCount = packet.ReadInt32("AurasCount", idx);
            var statesCount = packet.ReadInt32("StatesCount", idx);

            for (var i = 0; i < aurasCount; ++i) // Auras
                ReadPetBattleActiveAura(packet, idx, i);

            for (var i = 0; i < statesCount; ++i) // States
                ReadPetBattleActiveState(packet, idx, i);
        }

        public static void ReadPetBattleFullUpdate(Packet packet, params object[] idx)
        {
            for (var i = 0; i < 2; ++i)
                ReadPetBattlePlayerUpdate(packet, idx, "Players", i);

            for (var i = 0; i < 3; ++i)
                ReadPetBattleEnviroUpdate(packet, idx, "Enviros", i);

            packet.ReadInt16("WaitingForFrontPetsMaxSecs", idx);
            packet.ReadInt16("PvpMaxRoundTime", idx);

            packet.ReadInt32("CurRound", idx);
            packet.ReadInt32("NpcCreatureID", idx);
            packet.ReadInt32("NpcDisplayID", idx);

            packet.ReadByte("CurPetBattleState", idx);
            packet.ReadByte("ForfeitPenalty", idx);

            packet.ReadPackedGuid128("InitialWildPetGUID", idx);

            packet.ReadBit("IsPVP", idx);
            packet.ReadBit("CanAwardXP", idx);
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
                Parsers.BattlePetHandler.ReadClientPetBattleSlot(packet, i);

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
                Parsers.BattlePetHandler.ReadClientPetBattleSlot(packet, i, "PetBattleSlot");
        }

        [Parser(Opcode.SMSG_BATTLE_PET_ERROR)]
        public static void HandleBattlePetError(Packet packet)
        {
            packet.ReadBits("Result", 3);
            packet.ReadInt32("CreatureID");
        }

        [Parser(Opcode.SMSG_BATTLE_PET_JOURNAL_LOCK_ACQUIRED)]
        [Parser(Opcode.CMSG_BATTLE_PET_REQUEST_JOURNAL)]
        [Parser(Opcode.CMSG_BATTLE_PET_REQUEST_JOURNAL_LOCK)]
        [Parser(Opcode.SMSG_PET_BATTLE_FINISHED)]
        [Parser(Opcode.CMSG_PET_BATTLE_FINAL_NOTIFY)]
        [Parser(Opcode.CMSG_JOIN_PET_BATTLE_QUEUE)]
        [Parser(Opcode.SMSG_PET_BATTLE_QUEUE_PROPOSE_MATCH)]
        [Parser(Opcode.CMSG_PET_BATTLE_QUIT_NOTIFY)]
        [Parser(Opcode.SMSG_BATTLE_PETS_HEALED)]
        [Parser(Opcode.SMSG_BATTLE_PET_FINISHED)]
        [Parser(Opcode.SMSG_BATTLE_PET_LICENSE_CHANGED)]
        public static void HandleBattlePetNull(Packet packet)
        {
        }
    }
}
