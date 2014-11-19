using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class AddonHandler
    {
        [Parser(Opcode.CMSG_ADDON_REGISTERED_PREFIXES)]
        public static void HandleCAddonRegisteredPrefixes(Packet packet)
        {
            var count = packet.ReadBits("count", 24);
            var len = new uint[count];
            for (var i = 0; i < count; i++)
                len[i] = packet.ReadBits(5);
            for (var i = 0; i < count; i++)
                packet.ReadWoWString("Addon", len[i], i);
        }

        [Parser(Opcode.CMSG_MESSAGECHAT_ADDON_WHISPER)]
        public static void HandleMessageChatAddonWhisper(Packet packet)
        {
            var targetLen = packet.ReadBits(9);
            var messageLen = packet.ReadBits(8);
            var prefixLen = packet.ReadBits(5);
            packet.ReadWoWString("Target Name", targetLen);
            packet.ReadWoWString("Prefix", prefixLen);
            packet.ReadWoWString("Message", messageLen);
        }

        [Parser(Opcode.SMSG_ADDON_INFO)]
        public static void HandleServerAddonInfo(Packet packet)
        {
            var BannedAddonsCount = packet.ReadBits("Banned Addons Count", 18);
            var AddonsCount = packet.ReadBits("Addons Count", 23);
            uint[,] AddonsInfo = new uint[AddonsCount, 4];

            for (var i = 0; i < AddonsCount; ++i)
            {
                AddonsInfo[i, 2] = packet.ReadBit("Has URL", i);

                if (AddonsInfo[i, 2] == 1)
                    AddonsInfo[i, 3] = packet.ReadBits(8);
                else
                    AddonsInfo[i, 3] = 0;

                AddonsInfo[i, 0] = packet.ReadBit("Use CRC", i);
                AddonsInfo[i, 1] = packet.ReadBit("Has Public Key", i);
            }

            for (var i = 0; i < AddonsCount; ++i)
            {
                if (AddonsInfo[i, 1] == 1)
                    packet.ReadBytes(256); // the bytes order isn't 1,2,3,4.. they are mangled.

                if (AddonsInfo[i, 0] == 1)
                {
                    packet.ReadByte("Unk Byte1", i);
                    packet.ReadUInt32("CRC Summ", i);
                }

                if (AddonsInfo[i, 2] == 1 && AddonsInfo[i, 3] > 0)
                    packet.ReadWoWString("URL path", AddonsInfo[i, 3], i);

                packet.ReadByte("Addon State", i);
            }

            for (var i = 0; i < BannedAddonsCount; ++i)
            {
                var NameMD5 = new byte[16];
                var VersionMD5 = new byte[16];

                packet.ReadUInt32("unk5", i); // 5
                packet.ReadUInt32("unk13", i); // 13

                for (uint j = 0; j < 16; j += 4)
                {
                    Array.Copy(packet.ReadBytes(4), 0, NameMD5, j, 4);
                    Array.Copy(packet.ReadBytes(4), 0, VersionMD5, j, 4);
                }

                packet.ReadUInt32("unk9", i); // 9
            }
        }

        [Parser(Opcode.CMSG_UNREGISTER_ALL_ADDON_PREFIXES)]
        public static void HandleAddonNull(Packet packet)
        {
        }
    }
}
