using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class PetHandler
    {
        public static void ReadPetAction(Packet packet, params object[] indexes)
        {
            var action = packet.ReadUInt32();
            var spellID = action & 0xFFFFFF;
            var slot = (action >> 24);

            packet.AddValue("Action", slot, indexes);
            packet.AddValue("SpellID", StoreGetters.GetName(StoreNameType.Spell, (int)spellID), indexes);
        }

        public static void ReadPetFlags(Packet packet, params object[] idx)
        {
            var petModeFlag = packet.ReadUInt16();
            var reactState = packet.ReadByte();
            var flag = petModeFlag >> 16;
            var commandState = (petModeFlag & flag);

            packet.AddValue("ReactState", (ReactState)reactState, idx);
            packet.AddValue("CommandState", (CommandState)commandState, idx);
            packet.AddValue("Flag", flag, idx);
        }

        public static void ReadPetSpellCooldownData(Packet packet, params object[] idx)
        {
            packet.ReadInt32("SpellID", idx);
            packet.ReadInt32("Duration", idx);
            packet.ReadInt32("CategoryDuration", idx);
            packet.ReadSingle("ModRate", idx);
            packet.ReadInt16("Category", idx);
        }

        public static void ReadPetSpellHistoryData(Packet packet, params object[] idx)
        {
            packet.ReadInt32("CategoryID", idx);
            packet.ReadInt32("RecoveryTime", idx);
            packet.ReadSingle("ChargeModRate", idx);
            packet.ReadSByte("ConsumedCharges", idx);
        }

        [Parser(Opcode.CMSG_QUERY_PET_NAME)]
        public static void HandlePetNameQuery(Packet packet)
        {
            packet.ReadPackedGuid128("PetID");
        }

        [Parser(Opcode.CMSG_REQUEST_STABLED_PETS)]
        public static void HandleRequestStabledPets(Packet packet)
        {
            packet.ReadPackedGuid128("GUID");
        }

        [Parser(Opcode.SMSG_PET_SPELLS_MESSAGE)]
        public static void HandlePetSpells(Packet packet)
        {
            packet.ReadPackedGuid128("PetGUID");
            packet.ReadInt16("CreatureFamily");
            packet.ReadInt16("Specialization");
            packet.ReadInt32("TimeLimit");

            ReadPetFlags(packet, "PetModeAndOrders");

            const int maxCreatureSpells = 10;
            for (var i = 0; i < maxCreatureSpells; i++) // Read pet / vehicle spell ids
                Parsers.PetHandler.ReadPetAction(packet, "ActionButtons", i);

            var actionsCount = packet.ReadInt32("ActionsCount");
            var cooldownsCount = packet.ReadUInt32("CooldownsCount");
            var spellHistoryCount = packet.ReadUInt32("SpellHistoryCount");

            for (int i = 0; i < actionsCount; i++)
                Parsers.PetHandler.ReadPetAction(packet, i, "Actions");

            for (int i = 0; i < cooldownsCount; i++)
            {
                if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_1_0_22900))
                    ReadPetSpellCooldownData(packet, i, "PetSpellCooldown");
                else
                    Parsers.PetHandler.ReadPetSpellCooldownData(packet, i, "PetSpellCooldown");
            }

            for (int i = 0; i < spellHistoryCount; i++)
            {

                if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_1_0_22900))
                    ReadPetSpellHistoryData(packet, i, "PetSpellHistory");
                else
                    Parsers.PetHandler.ReadPetSpellHistoryData(packet, i, "PetSpellHistory");
            }
        }

        [Parser(Opcode.SMSG_PET_MODE)]
        public static void HandlePetMode(Packet packet)
        {
            packet.ReadPackedGuid128("PetGUID");
            ReadPetFlags(packet, "PetMode");
        }

        [Parser(Opcode.SMSG_PET_STABLE_RESULT)]
        public static void HandlePetStableResult(Packet packet)
        {
            packet.ReadByte("Result");
        }
    }
}
