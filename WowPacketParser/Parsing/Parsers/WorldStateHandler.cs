﻿using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Proto;
using WowPacketParser.SQL;

namespace WowPacketParser.Parsing.Parsers
{
    public static class WorldStateHandler
    {
        public static int CurrentAreaId = -1;
        public static int CurrentZoneId = -1;

        [Parser(Opcode.SMSG_INIT_WORLD_STATES)]
        public static void HandleInitWorldStates(Packet packet)
        {
            PacketInitWorldStates worldStatesPacket = packet.Holder.InitWorldStates = new PacketInitWorldStates();
            worldStatesPacket.MapId = packet.ReadInt32<MapId>("Map ID");
            worldStatesPacket.ZoneId = CurrentZoneId = packet.ReadInt32<ZoneId>("Zone Id");
            worldStatesPacket.AreaId = CurrentAreaId = packet.ReadInt32<AreaId>("Area Id");

            var numFields = packet.ReadInt16("Field Count");
            for (var i = 0; i < numFields; i++)
                worldStatesPacket.WorldStates.Add(ReadWorldStateBlock(packet, i));
        }

        public static WorldState ReadWorldStateBlock(Packet packet, params object[] indexes)
        {
            var worldStateId = packet.ReadInt32();
            var val = packet.ReadInt32();
            var comment = "";
            SQLDatabase.WorldStateNames.TryGetValue(worldStateId, out comment);
            packet.AddValue("WorldStateID", $"{worldStateId} - Value: {val} - {comment}", indexes);
            return new WorldState()
            {
                Id = worldStateId,
                Value = val
            };
        }

        [Parser(Opcode.SMSG_UPDATE_WORLD_STATE)]
        public static void HandleUpdateWorldState(Packet packet)
        {
            PacketUpdateWorldState updateWorldState = packet.Holder.UpdateWorldState = new PacketUpdateWorldState();
            updateWorldState.WorldState = ReadWorldStateBlock(packet);

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V4_2_2_14545))
                updateWorldState.Hidden = packet.ReadByte("Unk byte") != 0;
        }

        [Parser(Opcode.SMSG_UI_TIME)]
        [Parser(Opcode.SMSG_SERVER_TIME_OFFSET)]
        public static void HandleUITimer(Packet packet)
        {
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V9_0_5_37503) &&
                ClientVersion.Expansion != ClientType.Classic)
                packet.ReadTime64("Time");
            else
                packet.ReadTime("Time");
        }
    }
}
