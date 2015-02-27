using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class MailHandler
    {
        [Parser(Opcode.CMSG_GET_MAIL_LIST)]
        public static void HandleGetMailList(Packet packet)
        {
            var guid = packet.StartBitStream(6, 3, 7, 5, 4, 1, 2, 0);
            packet.ParseBitStream(guid, 7, 1, 6, 5, 4, 2, 3, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_MAIL_CREATE_TEXT_ITEM)]
        public static void HandleMailCreate(Packet packet)
        {
            packet.ReadInt32("Mail ID");
            var guid = packet.StartBitStream(4, 1, 6, 2, 5, 3, 0, 7);
            packet.ParseBitStream(guid, 6, 5, 4, 3, 0, 7, 2, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_MAIL_DELETE)]
        public static void HandleMailDelete(Packet packet)
        {
            packet.ReadUInt32("Template Id");
            packet.ReadUInt32("Mail Id");
        }

        [Parser(Opcode.CMSG_MAIL_MARK_AS_READ)]
        public static void HandleMarkMail(Packet packet)
        {
            packet.ReadInt32("Mail ID");
            var guid = new byte[8];
            guid[0] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            packet.ReadBit("unk16"); // 16
            guid[4] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[5] = packet.ReadBit();

            packet.ParseBitStream(guid, 1, 7, 2, 5, 6, 3, 4, 0);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_MAIL_RETURN_TO_SENDER)]
        public static void HandleMailReturnToSender(Packet packet)
        {
            packet.ReadUInt32("Mail Id");

            var guid = new byte[8];

            packet.StartBitStream(guid, 2, 0, 4, 6, 3, 1, 7, 5);
            packet.ParseBitStream(guid, 5, 6, 2, 0, 3, 1, 4, 7);
            packet.WriteGuid("Mailbox Guid", guid);
        }

        [Parser(Opcode.CMSG_MAIL_TAKE_ITEM)]
        public static void HandleMailTakeItem(Packet packet)
        {
            packet.ReadInt32("Mail ID"); // 24
            packet.ReadInt32("Item low Guid"); // 28
            var guid = packet.StartBitStream(6, 5, 2, 3, 0, 1, 4, 7);
            packet.ParseBitStream(guid, 0, 1, 4, 2, 5, 6, 3, 7);
            packet.WriteGuid("Mailbox Guid", guid);
        }

        [Parser(Opcode.CMSG_MAIL_TAKE_MONEY)]
        public static void HandleTakeMoney(Packet packet)
        {
            packet.ReadInt32("Mail ID");
            packet.ReadInt64("Money");
            var guid = packet.StartBitStream(7, 6, 3, 2, 4, 5, 0, 1);
            packet.ParseBitStream(guid, 7, 1, 4, 0, 3, 2, 6, 5);
            packet.WriteGuid("Mailbox GUID", guid);
        }

        [Parser(Opcode.CMSG_QUERY_NEXT_MAIL_TIME)]
        public static void HandleCQueryNextMailTime(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_SEND_MAIL)]
        public static void HandleSendMail(Packet packet)
        {
            packet.ReadInt32("unk2588"); // always 41
            packet.ReadInt32("unk2592");
            packet.ReadInt64("COD");
            packet.ReadInt64("Money");
            var guid = new byte[8];
            guid[0] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[1] = packet.ReadBit();

            var unk587 = packet.ReadBits(11);

            guid[3] = packet.ReadBit();

            var unk24 = packet.ReadBits(9);
            guid[7] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var unk2596 = packet.ReadBits("Item count", 5);

            var guid2600 = new byte[unk2596][];
            for (var i = 0; i < unk2596; i++)
                guid2600[i] = packet.StartBitStream(1, 7, 2, 5, 0, 6, 3, 4);

            var unk330 = packet.ReadBits(9);
            guid[2] = packet.ReadBit();
            for (var i = 0; i < unk2596; i++)
            {
                packet.ReadByte("Slot", i);
                packet.ParseBitStream(guid2600[i], 3, 0, 2, 1, 6, 5, 7, 4);
                packet.WriteGuid("Item Guid", guid2600[i], i);
            }
            packet.ParseBitStream(guid, 1);
            packet.ReadWoWString("Body", unk587);
            packet.ParseBitStream(guid, 0);
            packet.ReadWoWString("Subject", unk330);
            packet.ParseBitStream(guid, 2, 6, 5, 7, 3, 4);
            packet.ReadWoWString("Receiver", unk24);
            packet.WriteGuid("Mailbox Guid", guid);
        }

        [Parser(Opcode.SMSG_MAIL_LIST_RESULT)]
        public static void HandleSMailListResult(Packet packet)
        {
            packet.ReadUInt32("Total Mails");

            var count = packet.ReadBits("Count", 18);

            var guid = new byte[count][];

            var bit1C = new bool[count];
            var bit24 = new bool[count];
            var sender = new bool[count];
            var bit2C = new bool[count];

            var subjectLength = new uint[count];
            var itemCount = new uint[count];
            var bodyLength = new uint[count];
            var bit84 = new uint[count][];

            for (var i = 0; i < count; ++i)
            {
                bit2C[i] = packet.ReadBit();
                subjectLength[i] = packet.ReadBits(8);
                bodyLength[i] = packet.ReadBits(13);
                bit24[i] = packet.ReadBit();
                bit1C[i] = packet.ReadBit();
                itemCount[i] = packet.ReadBits(17);
                sender[i] = packet.ReadBit();
                if (sender[i])
                {
                    guid[i] = new byte[8];
                    packet.StartBitStream(guid[i], 2, 6, 7, 0, 5, 3, 1, 4);
                }

                bit84[i] = new uint[itemCount[i]];

                for (var j = 0; j < itemCount[i]; ++j)
                    bit84[i][j] = packet.ReadBit();
            }

            for (var i = 0; i < count; ++i)
            {

                for (var j = 0; j < itemCount[i]; ++j)
                {
                    packet.ReadInt32("GuidLow", i, j);

                    var len = packet.ReadInt32();

                    packet.ReadBytes(len);

                    packet.ReadInt32("MaxDurability", i, j);
                    packet.ReadInt32("SuffixFactor", i, j);
                    for (var k = 0; k < 8; ++k)
                    {
                        packet.ReadInt32("EnchantmentDuration", i, j, k);
                        packet.ReadInt32("EnchantmentId", i, j, k);
                        packet.ReadInt32("EnchantmentCharges", i, j, k);
                    }
                    packet.ReadInt32("ItemRandomPropertyId", i, j);
                    packet.ReadInt32("SpellCharges", i, j);
                    packet.ReadInt32("Durability", i, j);
                    packet.ReadInt32("Count", i, j);
                    packet.ReadByte("Slot", i, j);
                    packet.AddValue("bit84", bit84[i][j], i, j);
                    packet.ReadUInt32<ItemId>("Item Id", i, j);
                }

                packet.ReadWoWString("Body", bodyLength[i], i);
                packet.ReadInt32("MessageID", i);
                if (sender[i])
                {
                    packet.ParseBitStream(guid[i], 4, 0, 5, 3, 1, 7, 2, 6);
                    packet.WriteGuid("Guid", guid[i]);
                }
                packet.ReadInt32("Unk1", i);
                packet.ReadInt64("COD", i);
                packet.ReadWoWString("Subject", subjectLength[i], i);
                packet.ReadInt32("Unk2", i);
                packet.ReadSingle("Time", i);
                packet.ReadInt64("Money", i);
                packet.ReadInt32("Flags", i);

                if (bit2C[i])
                    packet.ReadInt32("Unk4", i);

                packet.ReadByte("MessageType", i);
                packet.ReadInt32("Unk5", i);

                if (bit24[i])
                    packet.ReadInt32("RealmId1", i);

                if (bit1C[i])
                    packet.ReadInt32("RealmId2", i);

            }
        }

        [Parser(Opcode.SMSG_QUERY_NEXT_MAIL_TIME)]
        public static void HandleSQueryNextMailTime(Packet packet)
        {
            var cnt20 = packet.ReadBits("cnt20", 20);
            var guid = new byte[cnt20][];
            var unk12 = new bool[cnt20];
            var unk20 = new bool[cnt20];
            for (var i = 0; i < cnt20; i++)
            {
                guid[i] = new byte[8];
                guid[i][3] = packet.ReadBit();
                unk12[i] = packet.ReadBit("unk12", i);
                guid[i][2] = packet.ReadBit();
                unk20[i] = packet.ReadBit("unk20", i);
                guid[i][6] = packet.ReadBit();
                guid[i][1] = packet.ReadBit();
                guid[i][4] = packet.ReadBit();
                guid[i][0] = packet.ReadBit();
                guid[i][5] = packet.ReadBit();
                guid[i][7] = packet.ReadBit();
            }

            for (var i = 0; i < cnt20; i++)
            {
                packet.ReadInt32("unk52", i);
                packet.ParseBitStream(guid[i], 5, 4, 6, 1);
                packet.ReadByte("unk56");
                packet.ParseBitStream(guid[i], 0);
                packet.ReadSingle("unk24");
                if (unk20[i])
                    packet.ReadInt32("unk88");
                packet.ReadInt32("unk176");
                packet.ParseBitStream(guid[i], 3, 2);
                if (unk12[i])
                    packet.ReadInt32("unk56");
                packet.ParseBitStream(guid[i], 7);
            }
            packet.ReadSingle("unk16");
        }

        [Parser(Opcode.SMSG_SEND_MAIL_RESULT)]
        public static void HandleSSendMailResult(Packet packet)
        {
            packet.ReadUInt32("Mail Id");
            var error = packet.ReadUInt32E<MailErrorType>("Mail Error");
            packet.ReadUInt32("Equip Error");
            var action = packet.ReadUInt32E<MailActionType>("Mail Action");
            packet.ReadUInt32("Item Low GUID");
            packet.ReadUInt32("Item count");
        }
    }
}
