﻿using System.Globalization;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Proto;
using WowPacketParser.SQL;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V11_0_0_55666.Parsers
{
    public static class NpcHandler
    {
        [HasSniffData]
        [Parser(Opcode.SMSG_GOSSIP_MESSAGE)]
        public static void HandleNpcGossip(Packet packet)
        {
            PacketGossipMessage packetGossip = packet.Holder.GossipMessage = new();

            WowGuid guid = packet.ReadPackedGuid128("GossipGUID");
            packetGossip.GossipSource = guid;

            var menuId = packet.ReadInt32("GossipID");
            packetGossip.MenuId = (uint)menuId;

            var lfgDungeonsId = packet.ReadInt32("LfgDungeonsID");
            var friendshipFactionID = packet.ReadInt32("FriendshipFactionID");

            CoreParsers.NpcHandler.AddGossipAddon(packetGossip.MenuId, friendshipFactionID, lfgDungeonsId, guid, packet.TimeSpan);

            var optionsCount = packet.ReadUInt32("GossipOptionsCount");
            var questsCount = packet.ReadUInt32("GossipQuestsCount");

            var hasBroadcastTextID = packet.ReadBit("HasBroadcastTextID");
            var hasBroadcastTextID2 = packet.ReadBit("HasBroadcastTextID2");

            for (var i = 0u; i < optionsCount; ++i)
                packetGossip.Options.Add(V6_0_2_19033.Parsers.NpcHandler.ReadGossipOptionsData((uint)menuId, guid, packet, i, "GossipOptions"));

            uint broadcastTextID = 0;
            uint npcTextID = 0;

            if (hasBroadcastTextID)
                broadcastTextID = (uint)packet.ReadInt32("BroadcastTextID");
            else if (hasBroadcastTextID2)
                broadcastTextID = (uint)packet.ReadInt32("BroadcastTextID2");

            if (hasBroadcastTextID || hasBroadcastTextID2)
                npcTextID = SQLDatabase.GetNPCTextIDByMenuIDAndBroadcastText(menuId, broadcastTextID);

            if (npcTextID != 0)
            {
                GossipMenu gossip = new()
                {
                    MenuID = packetGossip.MenuId,
                    TextID = packetGossip.TextId = npcTextID,
                    ObjectType = guid.GetObjectType(),
                    ObjectEntry = guid.GetEntry()
                };

                Storage.Gossips.Add(gossip, packet.TimeSpan);
            }
            else if (hasBroadcastTextID || hasBroadcastTextID2)
                V9_0_1_36216.Parsers.NpcHandler.AddBroadcastTextToGossip(packetGossip.MenuId, broadcastTextID, guid);

            for (var i = 0u; i < questsCount; ++i)
                packetGossip.Quests.Add(V7_0_3_22248.Parsers.NpcHandler.ReadGossipQuestTextData(packet, i, "GossipQuests"));

            if (guid.GetObjectType() == ObjectType.Unit && !CoreParsers.NpcHandler.HasLastGossipOption(packet.TimeSpan, (uint)menuId))
            {
                CreatureTemplateGossip creatureTemplateGossip = new()
                {
                    CreatureID = guid.GetEntry(),
                    MenuID = (uint)menuId
                };
                Storage.CreatureTemplateGossips.Add(creatureTemplateGossip);
                Storage.CreatureDefaultGossips.Add(guid.GetEntry(), (uint)menuId);
            }

            CoreParsers.NpcHandler.UpdateLastGossipOptionActionMessage(packet.TimeSpan, (uint)menuId);

            packet.AddSniffData(StoreNameType.Gossip, menuId, guid.GetEntry().ToString(CultureInfo.InvariantCulture));
        }

        [Parser(Opcode.SMSG_VENDOR_INVENTORY)]
        public static void HandleVendorInventory(Packet packet)
        {
            uint entry = packet.ReadPackedGuid128("VendorGUID").GetEntry();
            packet.ReadInt32("Reason");
            uint count = packet.ReadUInt32("VendorItems");

            for (int i = 0; i < count; ++i)
            {
                packet.ReadUInt64("Price", i);
                NpcVendor vendor = new NpcVendor
                {
                    Entry = entry,
                    Slot = (int)packet.ReadUInt32("MuID", i),
                    Type = (uint)packet.ReadInt32("Type", i)
                };

                int buyCount = packet.ReadInt32("StackCount", i);
                int maxCount = packet.ReadInt32("Quantity", i);
                vendor.ExtendedCost = (uint)packet.ReadInt32("ExtendedCostID", i);
                vendor.PlayerConditionID = (uint)packet.ReadInt32("PlayerConditionFailed", i);
                packet.ResetBitReader();
                packet.ReadBit("Locked", i);
                vendor.IgnoreFiltering = packet.ReadBit("DoNotFilterOnVendor", i);
                packet.ReadBit("Refundable", i);
                packet.ResetBitReader();

                vendor.Item = Substructures.ItemHandler.ReadItemInstance1100(packet, i).ItemID;

                vendor.MaxCount = maxCount == -1 ? 0 : (uint)maxCount; // TDB
                if (vendor.Type == 2)
                    vendor.MaxCount = (uint)buyCount;

                Storage.NpcVendors.Add(vendor, packet.TimeSpan);
            }

            CoreParsers.NpcHandler.LastGossipOption.Reset();
            CoreParsers.NpcHandler.TempGossipOptionPOI.Reset();
        }
    }
}
