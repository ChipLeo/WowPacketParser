using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class BattlePayHandler
    {
        //65BA82 22996
        private static byte ReadBattlePayProduct(Packet packet, params object[] idx)
        {
            packet.ReadInt32("ProductID", idx);

            packet.ReadByte("Type", idx);

            packet.ReadInt64("NormalPriceFixedPoint", idx);
            packet.ReadInt64("CurrentPriceFixedPoint", idx);

            packet.ReadInt32("Flags", idx);

            packet.ResetBitReader();

            packet.ReadBit("HasPet", idx);
            var bit32 = packet.ReadBit("HasBATTLEPETRESULT", idx);

            var int40 = packet.ReadBits("BattlepayProductItemCount", 7, idx);

            var bit5228 = packet.ReadBit("HasBattlepayDisplayInfo", idx);
            byte v10 = bit5228;
                //packet.ReadInt32("ID", idx, j);
                //packet.ReadInt32("ItemID", idx, j);
                //packet.ReadInt32("Quantity", idx, j);

            if (bit32)
                    packet.ReadBits("PetResult", 4, idx);

            for (var i = 0; i < int40; ++i)
                v10 = sub_65BBCD(packet, idx, i);

            if (bit5228)
                ReadBattlepayDisplayInfo(packet, idx, "DisplayInfo");

            return v10;
        }

        //65BBCD 22996
        private static byte sub_65BBCD(Packet packet, params object[] idx)
        {
            packet.ReadInt32("unk", idx);
            packet.ReadByte("unk", idx);
            packet.ReadInt32("unk", idx);
            packet.ReadInt32("unk", idx);
            packet.ReadInt32("unk", idx);
            packet.ReadInt32("unk", idx);
            packet.ResetBitReader();
            packet.ReadBit("unk", idx);
            var byte32 = packet.ReadBit("unk", idx);
            var byte5208 = packet.ReadBit("unk", idx);
            byte v5 = byte5208;
            if (byte32)
                v5 = (byte)packet.ReadBits("unk", 4, idx);
            if (byte5208)
                ReadBattlepayDisplayInfo(packet, idx, "DisplayInfo");
            return v5;
        }

        //65BCC2 22996
        private static byte ReadBattlePayDistributionObject(Packet packet, params object[] index)
        {
            packet.ReadInt64("DistributionID", index);

            packet.ReadInt32("Status", index);
            packet.ReadInt32("ProductID", index);

            packet.ReadPackedGuid128("TargetPlayer", index);
            packet.ReadInt32("TargetVirtualRealm", index);
            packet.ReadInt32("TargetNativeRealm", index);

            packet.ReadInt64("PurchaseID", index);

            packet.ResetBitReader();

            var bit5280 = packet.ReadBit("HasBattlePayProduct", index);

            var result = packet.ReadBit("Revoked", index);

            if (bit5280)
                ReadBattlePayProduct(packet, index, "Product");

            return result;
        }

        [Parser(Opcode.CMSG_BATTLE_PAY_GET_PRODUCT_LIST)]
        [Parser(Opcode.CMSG_BATTLE_PAY_GET_PURCHASE_LIST)]
        [Parser(Opcode.CMSG_UPDATE_VAS_PURCHASE_STATES)]
        public static void HandleZeroLengthPackets(Packet packet)
        {
        }

        //65C08C 22996
        private static void ReadBattlepayDisplayInfo(Packet packet, params object[] idx)
        {
            packet.ResetBitReader();

            var bit4 = packet.ReadBit("HasCreatureDisplayInfoID", idx);
            var bit12 = packet.ReadBit("HasFileDataID", idx);

            var bits16 = packet.ReadBits(10);
            var bits529 = packet.ReadBits(10);
            var bits1042 = packet.ReadBits(13);

            var bit5144 = packet.ReadBit("HasFlags", idx);

            var bit5152 = packet.ReadBit("unk5152", idx);

            var bit5160 = packet.ReadBit("unk5160", idx);

            var bit5168 = packet.ReadBit("unk5168", idx);

            if (bit4)
                packet.ReadInt32("CreatureDisplayInfoID", idx);

            if (bit12)
                packet.ReadInt32("FileDataID", idx);

            packet.ReadWoWString("Name1", bits16, idx);
            packet.ReadWoWString("Name2", bits529, idx);
            packet.ReadWoWString("Name3", bits1042, idx);

            if (bit5144)
                packet.ReadInt32("Flags", idx);

            if (bit5152)
                packet.ReadInt32("unk5152", idx);

            if (bit5160)
                packet.ReadInt32("unk5160", idx);

            if (bit5168)
                packet.ReadInt32("unk5168", idx);
        }

        [Parser(Opcode.SMSG_BATTLE_PAY_GET_PRODUCT_LIST_RESPONSE)]
        public static void HandletBattlePayGetProductListResponse(Packet packet)
        {
            packet.ReadUInt32("Result");
            packet.ReadUInt32("CurrencyID");

            var int24 = packet.ReadUInt32("int24Count");
            var int40 = packet.ReadUInt32("int40Count");
            var int56 = packet.ReadUInt32("int56Count");
            var int72 = packet.ReadUInt32("int72Count");

            //sub_65BD92 22996
            for (int i = 0; i < int24; ++i)
            {
                packet.ReadInt32("unk1", i);
                packet.ReadInt64("unk2", i);
                packet.ReadInt64("unk3", i);
                var int24i = packet.ReadInt32("int24i", i);
                packet.ReadInt32("unk5", i);
                for (int j = 0; j < int24i; ++j)
                    packet.ReadInt32("unk6", j, i);
                packet.ResetBitReader();
                packet.ReadBits("unk7", 7, i);
                var bit5220 = packet.ReadBit("bit5220", i);

                if (bit5220)
                    ReadBattlepayDisplayInfo(packet, i, "DisplayInfo");
            }

            //sub_65BA82 22996
            for (int i = 0; i < int40; ++i)
            {
                packet.ReadInt32("unk8", i);
                packet.ReadByte("unk9", i);
                packet.ReadInt32("unk10", i);
                packet.ReadInt32("unk11", i);
                packet.ReadInt32("unk12", i);
                packet.ReadInt32("unk13", i);
                packet.ReadInt32("unk14", i);

                packet.ResetBitReader();
                packet.ReadBit("unk15", i);
                var bit32 = packet.ReadBit("unk16", i);

                var bits40 = packet.ReadBits("unk17", 7, i);

                var bit5228 = packet.ReadBit("unk18", i);

                if (bit32)
                    packet.ReadBits("unk19", 4, i);

                for (int j = 0; i < bits40; ++i)
                {
                    packet.ReadInt32("unk20", j, i);
                    packet.ReadByte("unk21", j, i);
                    packet.ReadInt32("unk22", j, i);
                    packet.ReadInt32("unk23", j, i);
                    packet.ReadInt32("unk24", j, i);
                    packet.ReadInt32("unk25", j, i);

                    packet.ResetBitReader();
                    packet.ReadBit("unk26", j, i);
                    var bit32j = packet.ReadBit("unk32j", j, i);

                    var bit5208 = packet.ReadBit("unk28", j, i);

                    if (bit32j)
                        packet.ReadBits("unk29", 4, j, i);

                    if (bit5208)
                        ReadBattlepayDisplayInfo(packet, j, i);
                }

                if (bit5228)
                    ReadBattlepayDisplayInfo(packet, i, "DisplayInfo");
            }

            //sub_65BE65 22996
            for (int i = 0; i < int56; ++i)
            {
                packet.ReadInt32("unk30", i);
                packet.ReadInt32("unk31", i);
                packet.ReadByte("unk32", i);
                packet.ReadInt32("unk33", i);
                packet.ResetBitReader();
                packet.ReadWoWString("str34", packet.ReadBits(8), i);
            }

            //sub_65BF92 22996
            for (int i = 0; i < int72; ++i)
            {
                packet.ReadInt32("unk40", i);
                packet.ReadInt32("unk41", i);
                packet.ReadInt32("unk42", i);
                packet.ReadInt32("unk43", i);
                packet.ReadInt32("unk44", i);
                packet.ReadByte("unk45", i);
                packet.ResetBitReader();
                if (packet.ReadBit())
                    ReadBattlepayDisplayInfo(packet, i, "DisplayInfo");
            }
        }

        [Parser(Opcode.SMSG_BATTLE_PAY_DISTRIBUTION_UPDATE)]
        public static void HandleBattlePayDistributionUpdate(Packet packet)
        {
            ReadBattlePayDistributionObject(packet);
        }

        [Parser(Opcode.SMSG_BATTLE_PAY_ACK_FAILED)]
        [Parser(Opcode.SMSG_BATTLE_PAY_BATTLE_PET_DELIVERED)]
        [Parser(Opcode.SMSG_BATTLE_PAY_COLLECTION_ITEM_DELIVERED)]
        [Parser(Opcode.SMSG_BATTLE_PAY_CONFIRM_PURCHASE)]
        [Parser(Opcode.SMSG_BATTLE_PAY_DELIVERY_ENDED)]
        [Parser(Opcode.SMSG_BATTLE_PAY_DELIVERY_STARTED)]
        [Parser(Opcode.SMSG_BATTLE_PAY_DISTRIBUTION_UNREVOKED)]
        [Parser(Opcode.SMSG_BATTLE_PAY_GET_DISTRIBUTION_LIST_RESPONSE)]
        [Parser(Opcode.SMSG_BATTLE_PAY_GET_PURCHASE_LIST_RESPONSE)]
        [Parser(Opcode.SMSG_BATTLE_PAY_MOUNT_DELIVERED)]
        [Parser(Opcode.SMSG_BATTLE_PAY_PURCHASE_UPDATE)]
        [Parser(Opcode.SMSG_BATTLE_PAY_START_CHECKOUT)]
        [Parser(Opcode.SMSG_BATTLE_PAY_START_DISTRIBUTION_ASSIGN_TO_TARGET_RESPONSE)]
        [Parser(Opcode.SMSG_BATTLE_PAY_START_PURCHASE_RESPONSE)]
        [Parser(Opcode.SMSG_BATTLE_PAY_VALIDATE_PURCHASE_RESPONSE)]
        [Parser(Opcode.CMSG_BATTLE_PAY_START_VAS_PURCHASE)]
        [Parser(Opcode.CMSG_GET_VAS_ACCOUNT_CHARACTER_LIST)]
        [Parser(Opcode.CMSG_GET_VAS_TRANSFER_TARGET_REALM_LIST)]
        [Parser(Opcode.CMSG_VAS_CHECK_TRANSFER_OK)]
        [Parser(Opcode.CMSG_VAS_GET_QUEUE_MINUTES)]
        [Parser(Opcode.CMSG_VAS_GET_SERVICE_STATUS)]
        [Parser(Opcode.SMSG_BATTLE_PAY_VAS_GUILD_FOLLOW_INFO)]
        [Parser(Opcode.SMSG_BATTLE_PAY_VAS_GUILD_MASTER_LIST)]
        [Parser(Opcode.SMSG_GET_VAS_ACCOUNT_CHARACTER_LIST_RESULT)]
        [Parser(Opcode.SMSG_GET_VAS_TRANSFER_TARGET_REALM_LIST_RESULT)]
        [Parser(Opcode.SMSG_VAS_CHECK_TRANSFER_OK_RESPONSE)]
        [Parser(Opcode.SMSG_VAS_GET_QUEUE_MINUTES_RESPONSE)]
        [Parser(Opcode.SMSG_VAS_GET_SERVICE_STATUS_RESPONSE)]
        [Parser(Opcode.SMSG_VAS_PURCHASE_COMPLETE)]
        [Parser(Opcode.SMSG_VAS_PURCHASE_STATE_UPDATE)]
        public static void HandleBattlePay(Packet packet)
        {
            packet.ReadToEnd();
        }
    }
}
