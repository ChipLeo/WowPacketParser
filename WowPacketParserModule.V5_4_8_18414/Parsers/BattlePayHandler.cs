using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class BattlePayHandler
    {
        [Parser(Opcode.CMSG_BATTLE_PAY_START_PURCHASE)]
        public static void HandleBattlePayStartPurchase(Packet packet)
        {
            packet.ReadInt32("ProductID");   // 28
            packet.ReadInt32("ClientToken"); // 24
            var guid = packet.StartBitStream(2, 3, 5, 1, 4, 7, 0, 6);
            packet.ParseBitStream(guid, 5, 3, 7, 1, 4, 0, 6, 2);
            packet.WriteGuid("TargetCharacter", guid);
        }

        [Parser(Opcode.SMSG_BATTLE_PAY_PURCHASE_UPDATE)]
        public static void HandleBattlePayPurchaseUpdate(Packet packet)
        {
            var battlePayPurchaseCount = packet.ReadBits("BattlePayPurchaseCount", 19);
            var len = new uint[battlePayPurchaseCount];
            for (var i = 0; i < battlePayPurchaseCount; i++)
                len[i] = packet.ReadBits(8);
            for (var i = 0; i < battlePayPurchaseCount; i++)
            {
                packet.ReadInt32("unk68", i); // 68
                packet.ReadInt64("PurchaseID", i); // 20
                packet.ReadInt32("unk84", i); // 84
                packet.ReadInt32("unk52", i); // 52
                packet.ReadWoWString("WalletName", len[i], i);
            }
        }

        [Parser(Opcode.SMSG_BATTLE_PAY_START_PURCHASE_RESPONSE)]
        public static void HandleBattlePayStartPurchaseResponse(Packet packet)
        {
            packet.ReadUInt64("PurchaseID");
            packet.ReadInt32("PurchaseResult"); //28
            packet.ReadInt32("ClientToken");    //24
        }
    }
}
