using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using CoreObjects = WowPacketParser.Store.Objects;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class ActionBarHandler
    {
        [Parser(Opcode.CMSG_SET_ACTION_BAR_TOGGLES)]
        public static void HandleSetActionBarToggles(Packet packet)
        {
            packet.ReadByte("Action Bar");
        }

        [Parser(Opcode.CMSG_SET_ACTION_BUTTON)]
        public static void HandleSetActionButton(Packet packet)
        {
            packet.ReadByte("Slot Id");
            var actionId = packet.StartBitStream(7, 0, 5, 2, 1, 6, 3, 4);
            packet.ParseBitStream(actionId, 6, 7, 3, 5, 2, 1, 4, 0);
            packet.AddValue("Action Id", BitConverter.ToUInt32(actionId, 0));
        }

        [Parser(Opcode.SMSG_UPDATE_ACTION_BUTTONS)]
        public static void HandleActionButtons(Packet packet)
        {
            const int buttonCount = 132;

            var buttons = new byte[buttonCount][];

            for (var i = 0; i < buttonCount; i++)
            {
                buttons[i] = new byte[8];
                buttons[i][4] = packet.ReadBit();
            }

            for (var i = 0; i < buttonCount; i++)
                buttons[i][5] = packet.ReadBit();
            for (var i = 0; i < buttonCount; i++)
                buttons[i][3] = packet.ReadBit();
            for (var i = 0; i < buttonCount; i++)
                buttons[i][1] = packet.ReadBit();
            for (var i = 0; i < buttonCount; i++)
                buttons[i][6] = packet.ReadBit();
            for (var i = 0; i < buttonCount; i++)
                buttons[i][7] = packet.ReadBit();
            for (var i = 0; i < buttonCount; i++)
                buttons[i][0] = packet.ReadBit();
            for (var i = 0; i < buttonCount; i++)
                buttons[i][2] = packet.ReadBit();

            for (var i = 0; i < buttonCount; i++)
                packet.ReadXORByte(buttons[i], 0);
            for (var i = 0; i < buttonCount; i++)
                packet.ReadXORByte(buttons[i], 1);
            for (var i = 0; i < buttonCount; i++)
                packet.ReadXORByte(buttons[i], 4);
            for (var i = 0; i < buttonCount; i++)
                packet.ReadXORByte(buttons[i], 6);
            for (var i = 0; i < buttonCount; i++)
                packet.ReadXORByte(buttons[i], 7);
            for (var i = 0; i < buttonCount; i++)
                packet.ReadXORByte(buttons[i], 2);
            for (var i = 0; i < buttonCount; i++)
                packet.ReadXORByte(buttons[i], 5);
            for (var i = 0; i < buttonCount; i++)
                packet.ReadXORByte(buttons[i], 3);

            packet.ReadByte("Packet Type");

            for (int i = 0; i < buttonCount; i++)
            {
                var actionId = BitConverter.ToInt32(buttons[i], 0);

                if (actionId == 0)
                    continue;

                PlayerCreateInfoAction action = new PlayerCreateInfoAction
                {
                    Button = (uint)i,
                    Action = (uint)actionId,
                    Type = 0 // removed in MoP
                };

                packet.AddValue("Action " + i, action.Action);

                WoWObject character;
                if (Storage.Objects.TryGetValue(CoreParsers.SessionHandler.LoginGuid, out character))
                {
                    Player player = character as Player;
                    if (player != null && player.FirstLogin)
                    {
                        action.Race = player.Race;
                        action.Class = player.Class;
                        Storage.StartActions.Add(action, packet.TimeSpan);
                    }
                }
            }
        }

        [Parser(Opcode.SMSG_DONT_AUTO_PUSH_SPELLS_TO_ACTION_BAR)]
        public static void HandleActionBarNull(Packet packet)
        {
        }
    }
}
