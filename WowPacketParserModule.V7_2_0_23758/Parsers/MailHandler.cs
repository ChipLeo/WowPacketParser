using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class MailHandler
    {
        [Parser(Opcode.CMSG_QUERY_NEXT_MAIL_TIME)]
        public static void HandleNullMail(Packet packet)
        {
        }

        public static void ReadMailAttachedItem(Packet packet, params object[] idx)
        {
            packet.ReadByte("Position", idx);
            packet.ReadInt32("AttachID", idx);
            packet.ReadInt32("Count", idx);
            packet.ReadInt32("Charges", idx);
            packet.ReadInt32("MaxDurability", idx);
            packet.ReadInt32("Durability", idx);

            // ItemInstance 650b42 22996
            ItemHandler.ReadItemInstance(packet, idx);

            packet.ResetBitReader();
            var cnt68 = packet.ReadBits(4);
            var cnt84 = packet.ReadBits(2);
            packet.ReadBit("Unlocked", idx);

            for (var i = 0; i < cnt84; ++i)
            {//650a17 22996
                packet.ReadByte("unkbyte", idx, i);
                ItemHandler.ReadItemInstance(packet, idx, i);
            }

            for (var i = 0; i < cnt68; ++i)
                ItemHandler.sub_6509B1(packet, idx);
        }

        public static void ReadMailListEntry(Packet packet, params object[] idx)
        {
            packet.ReadInt32("MailID", idx);
            packet.ReadByteE<MailType>("SenderType", idx);

            packet.ReadInt64("Cod", idx);

            packet.ReadInt32("StationeryID", idx);

            packet.ReadInt64("SentMoney", idx);

            packet.ReadInt32("MailTemplateID", idx);
            packet.ReadInt32("PackageID", idx);
            packet.ReadInt32("Flags", idx);
            //packet.ReadSingle("DaysLeft", idx);

            var attachmentsCount = packet.ReadInt32("AttachmentsCount", idx);

            packet.ResetBitReader();
            var bit24 = packet.ReadBit("HasSenderCharacter", idx);
            var bit36 = packet.ReadBit("HasAltSenderID", idx);

            var bits76 = packet.ReadBits(8);
            var bits332 = packet.ReadBits(13);
            for (var i = 0; i < attachmentsCount; ++i) // Attachments
                ReadMailAttachedItem(packet, idx, i, "MailAttachedItem");

            packet.ResetBitReader();

            if (bit24)
                packet.ReadPackedGuid128("SenderCharacter", idx);

            if (bit36)
                packet.ReadInt32("AltSenderID", idx);

            packet.ReadWoWString("Subject", bits76, idx);
            packet.ReadWoWString("Body", bits332, idx);
        }

        [Parser(Opcode.CMSG_MAIL_GET_LIST)]
        public static void HandleGetMailList(Packet packet)
        {
            packet.ReadPackedGuid128("Mailbox");
        }

        [Parser(Opcode.SMSG_MAIL_LIST_RESULT)]
        public static void HandleMailListResult(Packet packet)
        {
            var mailsCount = packet.ReadInt32("MailsCount");
            packet.ReadInt32("TotalNumRecords");

            for (var i = 0; i < mailsCount; ++i)
                ReadMailListEntry(packet, i, "MailListEntry");
        }

        [Parser(Opcode.SMSG_MAIL_QUERY_NEXT_TIME_RESULT)]
        public static void HandleMailQueryNextTimeResult(Packet packet)
        {
            packet.ReadSingle("NextMailTime");

            var int5 = packet.ReadInt32("NextCount");

            for (int i = 0; i < int5; i++)
            {
                packet.ReadPackedGuid128("SenderGUID", i);

                packet.ReadSingle("TimeLeft", i);
                packet.ReadInt32("AltSenderID", i);
                packet.ReadByte("AltSenderType", i);
                packet.ReadInt32("StationeryID", i);
            }
        }

        [Parser(Opcode.SMSG_NOTIFY_RECEIVED_MAIL)]
        public static void HandleNotifyReceivedMail(Packet packet)
        {
            packet.ReadSingle("Delay");
        }
    }
}
