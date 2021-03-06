﻿using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class AccountDataHandler
    {
        [Parser(Opcode.CMSG_REPORT_CLIENT_VARIABLES)]
        public static void HandleSaveClientVarables(Packet packet)
        {
            var varablesCount = packet.ReadUInt32("VarablesCount");

            for (var i = 0; i < varablesCount; ++i)
            {
                var variableNameLen = packet.ReadBits(6);
                var valueLen = packet.ReadBits(10);

                packet.WriteLine($"[{ i.ToString() }] VariableName: \"{ packet.ReadWoWString((int)variableNameLen) }\" Value: \"{ packet.ReadWoWString((int)valueLen) }\"");
            }
        }

        [Parser(Opcode.CMSG_REQUEST_ACCOUNT_DATA)]
        public static void HandleRequestAccountData(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
            packet.ReadBitsE<AccountDataType>("Data Type", 3);
        }

        [Parser(Opcode.CMSG_REPORT_ENABLED_ADDONS)]
        public static void HandleSaveEnabledAddons(Packet packet)
        {
            var enableAddonsCount = packet.ReadUInt32("EnableAddonsCount");

            for (var i = 0; i < enableAddonsCount; ++i)
            {
                packet.ResetBitReader();

                var addonNameLen = packet.ReadBits(7);
                var versionLen = packet.ReadBits(6);

                packet.ReadBit("Loaded", i);
                packet.ReadBit("Disabled", i);

                if (addonNameLen > 1)
                    packet.ReadCString("AddonName", i);
                if (versionLen > 1)
                    packet.ReadCString("Version", i);
            }
        }

        [Parser(Opcode.CMSG_UPDATE_ACCOUNT_DATA)]
        public static void HandleClientUpdateAccountData(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
            packet.ReadTime("Time");

            var decompCount = packet.ReadInt32();
            packet.ResetBitReader();
            packet.ReadBitsE<AccountDataType>("Data Type", 3);
            var compCount = packet.ReadInt32();

            var pkt = packet.Inflate(compCount, decompCount, false);

            var data = pkt.ReadWoWString(decompCount);

            packet.AddValue("CompressedData", data);
        }

        [Parser(Opcode.SMSG_ACCOUNT_DATA_TIMES)]
        public static void HandleAccountDataTimes(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
            packet.ReadTime("Server Time");

            for (var i = 0; i < 8; ++i)
            {
                packet.ReadTime("[" + (AccountDataType)i + "]" + " Time");
            }
        }

        [Parser(Opcode.SMSG_CACHE_INFO)]
        public static void HandleCacheInfo(Packet packet)
        {
            var cacheInfoCount = packet.ReadUInt32("CacheInfoCount");

            packet.ResetBitReader();

            var signatureLen = packet.ReadBits(6);

            for (var i = 0; i < cacheInfoCount; ++i)
            {
                packet.ResetBitReader();

                var variableNameLen = packet.ReadBits(6);
                var valueLen = packet.ReadBits(6);

                packet.WriteLine($"[{ i.ToString() }] VariableName: \"{ packet.ReadWoWString((int)variableNameLen) }\" Value: \"{ packet.ReadWoWString((int)valueLen) }\"");
            }

            packet.ReadWoWString("Signature", signatureLen);
        }

        [Parser(Opcode.SMSG_GET_ACCOUNT_CHARACTER_LIST_RESULT)]
        public static void HandleGetAccountCharacterListResult(Packet packet)
        {
            packet.ReadInt32("unk1");
            var cnt = packet.ReadInt32("Count");
            packet.ReadBit("unk2");
            for (var i = 0; i < cnt; ++i)
            {
                packet.ReadPackedGuid128("Acc", i);
                packet.ReadPackedGuid128("Player", i);
                packet.ReadInt32("VirtualRealmAddress", i);
                packet.ReadByte("unk4", i);
                packet.ReadByteE<Class>("ClassID", i);
                packet.ReadByteE<Gender>("Gender", i);
                packet.ReadByte("Level", i);
                packet.ReadInt32("unk8", i);

                packet.ResetBitReader();
                var len1 = packet.ReadBits(6);
                var len2 = packet.ReadBits(9);
                packet.ReadWoWString("Name", len1, i);
                packet.ReadWoWString("str2", len2, i);
            }
        }

        [Parser(Opcode.SMSG_UPDATE_ACCOUNT_DATA)]
        public static void HandleServerUpdateAccountData(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
            packet.ReadTime("Time");

            var decompCount = packet.ReadInt32();
            packet.ResetBitReader();
            packet.ReadBitsE<AccountDataType>("Data Type", 3);
            var compCount = packet.ReadInt32();

            var pkt = packet.Inflate(compCount, decompCount, false);
            var data = pkt.ReadWoWString(decompCount);

            packet.AddValue("Account Data", data);
        }
    }
}
