﻿using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class GameObjectHandler
    {
        [Parser(Opcode.CMSG_GAME_OBJ_REPORT_USE)]
        [Parser(Opcode.CMSG_GAME_OBJ_USE)]
        [Parser(Opcode.SMSG_PAGE_TEXT)]
        public static void HandleGoMisc(Packet packet)
        {
            packet.ReadPackedGuid128("GameObjectGUID");
        }

        [Parser(Opcode.CMSG_QUERY_GAME_OBJECT)]
        public static void HandleGameObjectQuery(Packet packet)
        {
            packet.ReadInt32("Entry");
            packet.ReadPackedGuid128("GUID");
        }

        [Parser(Opcode.SMSG_GAME_OBJECT_DESPAWN)]
        public static void HandleGameObjectDespawn(Packet packet)
        {
            packet.ReadPackedGuid128("ObjectGUID");
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_QUERY_GAME_OBJECT_RESPONSE)]
        public static void HandleGameObjectQueryResponse(Packet packet)
        {
            var entry = packet.ReadEntry("Entry");
            if (entry.Value) // entry is masked
                return;

            GameObjectTemplate gameObject = new GameObjectTemplate
            {
                Entry = (uint)entry.Key
            };

            packet.ReadBit("Allow");

            int dataSize = packet.ReadInt32("DataSize");
            if (dataSize == 0)
                return;

            gameObject.Type = packet.ReadInt32E<GameObjectType>("Type");

            gameObject.DisplayID = packet.ReadUInt32("Display ID");

            var name = new string[4];
            for (int i = 0; i < 4; i++)
                name[i] = packet.ReadCString("Name", i);
            gameObject.Name = name[0];

            gameObject.IconName = packet.ReadCString("Icon Name");
            gameObject.CastCaption = packet.ReadCString("Cast Caption");
            gameObject.UnkString = packet.ReadCString("Unk String");

            gameObject.Data = new int?[33];
            for (int i = 0; i < gameObject.Data.Length; i++)
                gameObject.Data[i] = packet.ReadInt32("Data", i);

            gameObject.Size = packet.ReadSingle("Size");

            byte questItemsCount = packet.ReadByte("QuestItemsCount");
            for (uint i = 0; i < questItemsCount; i++)
            {
                GameObjectTemplateQuestItem questItem = new GameObjectTemplateQuestItem
                {
                    GameObjectEntry = (uint)entry.Key,
                    Idx = i,
                    ItemId = packet.ReadUInt32<ItemId>("QuestItem", i)
                };

                Storage.GameObjectTemplateQuestItems.Add(questItem, packet.TimeSpan);
            }

            gameObject.RequiredLevel = packet.ReadInt32("RequiredLevel");

            Storage.GameObjectTemplates.Add(gameObject, packet.TimeSpan);

            ObjectName objectName = new ObjectName
            {
                ObjectType = StoreNameType.GameObject,
                ID = entry.Key,
                Name = gameObject.Name
            };

            Storage.ObjectNames.Add(objectName, packet.TimeSpan);
        }
    }
}
