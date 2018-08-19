using System;
using WowPacketParser.Enums;
using WowPacketParser.Hotfix;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class HotfixHandler
    {
        [Parser(Opcode.CMSG_DB_QUERY_BULK)]
        public static void HandleDbQueryBulk(Packet packet)
        {
            packet.ReadInt32E<DB2Hash>("DB2 File");

            var count = packet.ReadBits("Count", 13);
            for (var i = 0; i < count; ++i)
            {
                packet.ReadPackedGuid128("Guid", i);
                packet.ReadInt32("Entry", i);
            }
        }

        [Parser(Opcode.CMSG_HOTFIX_REQUEST)]
        public static void HandleHotfixRequest(Packet packet)
        {
            var hotfixCount = packet.ReadUInt32();
            for (var i = 0u; i < hotfixCount; ++i)
                packet.ReadInt32("HotfixID", i);
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_DB_REPLY)]
        public static void HandleDBReply(Packet packet)
        {
            var type = packet.ReadUInt32E<DB2Hash>("TableHash");
            var entry = packet.ReadInt32("RecordID");
            var timeStamp = packet.ReadUInt32();
            packet.AddValue("Timestamp", Utilities.GetDateTimeFromUnixTime(timeStamp));
            var allow = packet.ReadBit("Allow");

            var size = packet.ReadInt32("Size");
            var data = packet.ReadBytes(size);
            var db2File = new Packet(data, packet.Opcode, packet.Time, packet.Direction, packet.Number, packet.Writer, packet.FileName);

            if (entry < 0 || !allow)
            {
                packet.WriteLine("Row {0} has been removed.", -entry);
                HotfixStoreMgr.RemoveRecord(type, entry);
            }
            else
            {
                switch (type)
                {
                    case DB2Hash.BroadcastText:
                        {
                            var bct = new BroadcastText()
                            {
                                ID = (uint)entry,
                                Text = db2File.ReadCString("MaleText"),
                                Text1 = db2File.ReadCString("FemaleText"),
                            };

                            bct.EmoteID = new ushort?[3];
                            bct.EmoteDelay = new ushort?[3];

                            for (int i = 0; i < 3; ++i)
                                bct.EmoteID[i] = db2File.ReadUInt16("EmoteID", i);
                            for (int i = 0; i < 3; ++i)
                                bct.EmoteDelay[i] = db2File.ReadUInt16("EmoteDelay", i);

                            bct.EmotesID = db2File.ReadUInt16("EmotesID");
                            bct.LanguageID = db2File.ReadByte("LanguageID");
                            bct.Flags = db2File.ReadByte("Flags");

                            bct.SoundEntriesID = new uint?[2];
                            for (int i = 0; i < 2; ++i)
                                bct.SoundEntriesID[i] = db2File.ReadUInt32("SoundID", i);

                            bct.ConditionID = db2File.ReadUInt32("PlayerConditionID");

                            Storage.BroadcastTexts.Add(bct, packet.TimeSpan);
                            break;
                        }
                    default:
                        HotfixStoreMgr.AddRecord(type, entry, db2File);
                        break;
                }
                db2File.ClosePacket(false);
            }
        }

        [Parser(Opcode.SMSG_AVAILABLE_HOTFIXES)]
        public static void HandleHotfixList(Packet packet)
        {
            packet.ReadInt32("HotfixCacheVersion");
            var hotfixCount = packet.ReadUInt32();
            for (var i = 0u; i < hotfixCount; ++i)
                packet.ReadInt32("HotfixID", i);
        }

        static void ReadHotfixRecord(Packet packet, int hotfixId, params object[] indexes)
        {
            packet.ResetBitReader();
            var type = packet.ReadUInt32E<DB2Hash>("TableHash", indexes);
            var entry = packet.ReadInt32("RecordID", indexes);
            var allow = packet.ReadBit("Allow", indexes);
            var dataSize = packet.ReadInt32("Size", indexes);
            var data = packet.ReadBytes(dataSize);
            var db2File = new Packet(data, packet.Opcode, packet.Time, packet.Direction, packet.Number, packet.Writer, packet.FileName);

            if (!allow)
            {
                packet.WriteLine($"Row {entry} has been removed.");
                HotfixStoreMgr.RemoveRecord(type, entry);
            }
            else
            {
                packet.AddSniffData(StoreNameType.None, entry, type.ToString());
                HotfixStoreMgr.AddRecord(type, entry, db2File);
                if (db2File.Position != db2File.Length)
                {
                    db2File.WriteLine($"(Entry: {entry} TableHash: {type}) has missing structure");
                    db2File.AsHex();
                }
                db2File.ClosePacket(false);
            }

            HotfixData hotfixData = new HotfixData
            {
                ID = (uint)hotfixId,
                TableHash = type,
                RecordID = entry,
                Deleted = !allow
            };

            Storage.HotfixDatas.Add(hotfixData);
        }

        static void ReadHotfixData(Packet packet, params object[] indexes)
        {
            var hotfixId = packet.ReadInt32("HotfixID", indexes);
            var recordCount = packet.ReadUInt32();
            for (var i = 0u; i < recordCount; ++i)
                ReadHotfixRecord(packet, hotfixId, indexes, i, "HotfixRecord");
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_HOTFIX_MESSAGE)]
        [Parser(Opcode.SMSG_HOTFIX_RESPONSE)]
        public static void HandleHotixData(Packet packet)
        {
            var hotfixCount = packet.ReadUInt32();
            for (var i = 0u; i < hotfixCount; ++i)
                ReadHotfixData(packet, i, "HotfixData");
        }
    }
}
