using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;
using WowPacketParser.Proto;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParser.Parsing.Parsers
{
    public static class ChatHandler
    {
        [Parser(Opcode.SMSG_CHAT_NOT_IN_PARTY)]
        public static void HandleChatNotInParty(Packet packet)
        {
            // length =8 for 230
            //packet.ReadInt32("Unk UInt32");
            packet.ReadGuid("Guid"); //230
        }

        [Parser(Opcode.SMSG_DEFENSE_MESSAGE)]
        public static void HandleDefenseMessage(Packet packet)
        {
            packet.ReadUInt32<ZoneId>("Zone Id");
            packet.ReadInt32("Message Length");
            packet.ReadCString("Message");
        }

        [Parser(Opcode.CMSG_CHAT_REPORT_IGNORED)]
        public static void HandleChatIgnored(Packet packet)
        {
            packet.ReadGuid("GUID");
            packet.ReadByte("Unk Byte");
        }

        [Parser(Opcode.CMSG_EMOTE)]
        public static void HandleEmoteClient(Packet packet)
        {
            packet.ReadInt32E<EmoteType>("Emote ID");
        }

        [Parser(Opcode.SMSG_EMOTE)]
        public static void HandleEmote(Packet packet)
        {
            PacketEmote packetEmote = packet.Holder.Emote = new PacketEmote();
            var emote = packet.ReadInt32E<EmoteType>("Emote ID");
            var guid = packet.ReadGuid("GUID");

            if (guid.GetObjectType() == ObjectType.Unit)
                Storage.Emotes.Add(guid, emote, packet.TimeSpan);

            packetEmote.Emote = (int) emote;
            packetEmote.Sender = guid.ToUniversalGuid();
        }

        [Parser(Opcode.CMSG_SEND_TEXT_EMOTE)]
        public static void HandleTextEmote(Packet packet)
        {
            packet.ReadInt32E<EmoteTextType>("Text Emote ID");
            packet.ReadInt32E<EmoteType>("Emote ID");
            packet.ReadGuid("GUID");
        }

        [Parser(Opcode.SMSG_TEXT_EMOTE)]
        public static void HandleTextEmoteServer(Packet packet)
        {
            packet.ReadGuid("GUID");
            packet.ReadInt32E<EmoteTextType>("Text Emote ID");
            packet.ReadInt32E<EmoteType>("Emote ID");
            packet.ReadInt32("Name length");
            packet.ReadCString("Name");
        }

        [Parser(Opcode.SMSG_CHAT_PLAYER_NOTFOUND)]
        public static void HandleChatPlayerNotFound(Packet packet)
        {
            packet.ReadCString("Name");
        }

        [Parser(Opcode.SMSG_CHAT)]
        [Parser(Opcode.SMSG_GM_MESSAGECHAT)]
        public static void HandleServerChatMessage(Packet packet)
        {
            PacketChat chatPacket = packet.Holder.Chat = new PacketChat();
            var text = new CreatureText
            {
                Type = packet.ReadByteE<ChatMessageType>("Type"),
                Language = packet.ReadInt32E<Language>("Language"),
                SenderGUID = packet.ReadGuid("GUID")
            };

            packet.ReadInt32("Constant time");

            switch (text.Type)
            {
                case ChatMessageType.Channel:
                {
                    packet.ReadCString("Channel Name");
                    goto case ChatMessageType.Say;
                }
                case ChatMessageType.Say:
                case ChatMessageType.Yell:
                case ChatMessageType.Party:
                case ChatMessageType.PartyLeader:
                case ChatMessageType.Raid:
                case ChatMessageType.RaidLeader:
                case ChatMessageType.RaidWarning:
                case ChatMessageType.Guild:
                case ChatMessageType.Officer:
                case ChatMessageType.Emote:
                case ChatMessageType.Whisper:
                case ChatMessageType.WhisperInform:
                case ChatMessageType.System:
                case ChatMessageType.Battleground:
                case ChatMessageType.BattlegroundLeader:
                case ChatMessageType.Achievement:
                case ChatMessageType.GuildAchievement:
                case ChatMessageType.Restricted:
                case ChatMessageType.Dnd:
                case ChatMessageType.Afk:
                case ChatMessageType.Ignored:
                case ChatMessageType.unk82:
                case ChatMessageType.unk83:
                case ChatMessageType.unk84:
                case ChatMessageType.unk87:
                case ChatMessageType.unk88:
                case ChatMessageType.unk92:
                {
                     packet.ReadGuid("Sender GUID");
                     break;
                }
                case ChatMessageType.BattlegroundNeutral:
                case ChatMessageType.BattlegroundAlliance:
                case ChatMessageType.BattlegroundHorde:
                {
                    var target = packet.ReadGuid("Sender GUID");
                    switch (target.GetHighType())
                    {
                        case HighGuidType.Creature:
                        case HighGuidType.Vehicle:
                        case HighGuidType.GameObject:
                        case HighGuidType.Transport:
                        case HighGuidType.Pet:
                            packet.ReadInt32("Sender Name Length");
                            packet.ReadCString("Sender Name");
                            break;
                    }
                    break;
                }
                case ChatMessageType.MonsterSay:
                case ChatMessageType.MonsterYell:
                case ChatMessageType.MonsterParty:
                case ChatMessageType.MonsterEmote:
                case ChatMessageType.MonsterWhisper:
                case ChatMessageType.RaidBossEmote:
                case ChatMessageType.RaidBossWhisper:
                case ChatMessageType.BattleNet:
                case ChatMessageType.TextEmote:
                case ChatMessageType.Skill:
                case ChatMessageType.unk90:
                case ChatMessageType.unk94:
                {
                    packet.ReadInt32("Name Length");
                    text.SenderName = packet.ReadCString("Name");
                    text.ReceiverGUID = packet.ReadGuid("Receiver GUID");
                        switch (text.ReceiverGUID.GetHighType())
                        {
                            case HighGuidType.Creature:
                            case HighGuidType.Vehicle:
                            case HighGuidType.GameObject:
                            case HighGuidType.Transport:
                            case HighGuidType.DynamicObject:
                            case HighGuidType.BattlePet:
                            //case HighGuidType.Pet:
                            case HighGuidType.Dragonkin:
                                packet.ReadInt32("Receiver Name Length");
                                text.ReceiverName = packet.ReadCString("Receiver Name");
                            break;
                    }
                    break;
                }
                case ChatMessageType.WhisperForeign:
                case ChatMessageType.unk89:
                    {
                        if (ClientVersion.AddedInVersion(ClientVersionBuild.V2_4_3_8606))
                        {
                            packet.ReadInt32("Name Length");
                            packet.ReadCString("Name");
                        }
                        packet.ReadGuid("Receiver GUID");
                        break;
                    }
                default:
                {
                    if (packet.Opcode == Opcodes.GetOpcode(Opcode.SMSG_GM_MESSAGECHAT, Direction.ServerToClient))
                    {
                        packet.ReadInt32("GMNameLength");
                        packet.ReadCString("GMSenderName");
                    }

                    if (text.Type == ChatMessageType.Channel)
                    {
                        packet.ReadCString("Channel Name");
                    }

                    packet.ReadGuid("Sender GUID");
                    break;
                }
            }

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V4_1_0_13914) && text.Language == Language.Addon)
                packet.ReadCString("Addon Message Prefix");

            var len = packet.ReadInt32("Text Length");
            if (text.Language == Language.Addon)
                text.Text = packet.ReadBytesTable("Text", len).ToString();
            else
                text.Text = packet.ReadCString("Text");

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V5_1_0_16309))
                packet.ReadInt16E<ChatTag>("Chat Tag");
            else
                packet.ReadByteE<ChatTag>("Chat Tag");

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V4_2_0_14333))
            {
                if (text.Type == ChatMessageType.RaidBossEmote || text.Type == ChatMessageType.RaidBossWhisper)
                {
                    packet.ReadSingle("Unk single");
                    packet.ReadByte("Unk byte");
                }
            }

            if (text.Type == ChatMessageType.Achievement || text.Type == ChatMessageType.GuildAchievement)
                packet.ReadInt32<AchievementId>("Achievement Id");

            uint entry = 0;
            if (text.SenderGUID.GetObjectType() == ObjectType.Unit)
                entry = text.SenderGUID.GetEntry();
            else if (text.ReceiverGUID != null && text.ReceiverGUID.GetObjectType() == ObjectType.Unit)
                entry = text.ReceiverGUID.GetEntry();

            if (entry != 0)
                Storage.CreatureTexts.Add(entry, text, packet.TimeSpan);

            chatPacket.Text = text.Text;
            chatPacket.Sender = text.SenderGUID.ToUniversalGuid();
            chatPacket.Target = text.ReceiverGUID?.ToUniversalGuid();
            chatPacket.Language = (int) text.Language;
            chatPacket.Type = (int) text.Type;
        }

        [Parser(Opcode.CMSG_MESSAGECHAT)]
        public static void HandleClientChatMessage(Packet packet)
        {
            var type = packet.ReadInt32E<ChatMessageType>("Type");

            var lng = packet.ReadInt32E<Language>("Language");

            switch (type)
            {
                case ChatMessageType.Whisper:
                case ChatMessageType.Yell:
                {
                    packet.ReadCString("Recipient");
                    break;
                }
                case ChatMessageType.Channel:
                case ChatMessageType.MonsterYell:
                {
                    packet.ReadCString("Channel");
                    break;
                }
            }

            if (lng == Language.Addon)
            {
                var msg = packet.ReadToEnd();
                packet.WriteLine("{0}", Utilities.ByteArrayToHexTable(msg, true));
            }
            else packet.ReadCString("Message");
        }

        [Parser(Opcode.CMSG_CHAT_MESSAGE_PARTY, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.CMSG_CHAT_MESSAGE_PARTY_LEADER)]
        public static void HandleMessageChatParty(Packet packet)
        {
            packet.ReadInt32E<Language>("Language");
            packet.ReadCString("Message");
        }

        [Parser(Opcode.CMSG_CHAT_MESSAGE_PARTY, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleMessageChatParty434(Packet packet)
        {
            packet.ReadInt32E<Language>("Language");
            var len = packet.ReadBits(9);
            packet.ReadWoWString("Message", len);
        }

        [Parser(Opcode.CMSG_CHAT_MESSAGE_RAID_WARNING, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleMessageChatRaidWarning434(Packet packet)
        {
            packet.ReadInt32E<Language>("Language");
            var len = packet.ReadBits(9);
            packet.ReadWoWString("Message", len);
        }

        [Parser(Opcode.CMSG_CHAT_MESSAGE_WHISPER, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleClientChatMessageWhisper(Packet packet)
        {
            packet.ReadUInt32E<ChatMessageType>("Type");
            packet.ReadCString("Message");
            packet.ReadCString("Receivers Name");
        }

        [Parser(Opcode.CMSG_CHAT_MESSAGE_WHISPER, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleClientChatMessageWhisper434(Packet packet)
        {
            packet.ReadUInt32E<ChatMessageType>("Type");
            var recvName = packet.ReadBits(10);
            var msgLen = packet.ReadBits(9);

            packet.ReadWoWString("Receivers Name", recvName);
            packet.ReadWoWString("Message", msgLen);
        }

        [Parser(Opcode.CMSG_CHAT_ADDON_MESSAGE_PARTY, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_3_15354)]
        [Parser(Opcode.CMSG_CHAT_ADDON_MESSAGE_GUILD, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_3_15354)]
        [Parser(Opcode.CMSG_CHAT_ADDON_MESSAGE_RAID, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_3_15354)]
        [Parser(Opcode.CMSG_CHAT_ADDON_MESSAGE_BATTLEGROUND, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_3_15354)]
        public static void HandleClientChatMessageAddon(Packet packet)
        {
            packet.ReadCString("Message");
            packet.ReadCString("Prefix");
        }

        [Parser(Opcode.CMSG_CHAT_ADDON_MESSAGE_GUILD, ClientVersionBuild.V4_3_3_15354)]
        [Parser(Opcode.CMSG_CHAT_ADDON_MESSAGE_BATTLEGROUND, ClientVersionBuild.V4_3_3_15354)]
        public static void HandleClientChatMessageAddon434(Packet packet)
        {
            var length1 = packet.ReadBits(9);
            var length2 = packet.ReadBits(5);
            packet.ReadWoWString("Message", length1);
            packet.ReadWoWString("Prefix", length2);
        }

        [Parser(Opcode.CMSG_CHAT_ADDON_MESSAGE_PARTY, ClientVersionBuild.V4_3_3_15354)]
        [Parser(Opcode.CMSG_CHAT_ADDON_MESSAGE_RAID, ClientVersionBuild.V4_3_3_15354)]
        [Parser(Opcode.CMSG_CHAT_ADDON_MESSAGE_OFFICER, ClientVersionBuild.V4_3_3_15354)]
        public static void HandleClientChatMessageAddonRaid434(Packet packet)
        {
            var length1 = packet.ReadBits(5);
            var length2 = packet.ReadBits(9);
            packet.ReadWoWString("Prefix", length1);
            packet.ReadWoWString("Message", length2);
        }

        [Parser(Opcode.CMSG_CHAT_ADDON_MESSAGE_WHISPER, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_3_15354)]
        public static void HandleClientChatMessageAddonWhisper(Packet packet)
        {
            packet.ReadCString("Prefix");
            packet.ReadCString("Target Name");
            packet.ReadCString("Message");
        }

        [Parser(Opcode.CMSG_CHAT_ADDON_MESSAGE_WHISPER, ClientVersionBuild.V4_3_3_15354)]
        public static void HandleClientChatMessageAddonWhisper434(Packet packet)
        {
            var msgLen = packet.ReadBits(9);
            var prefixLen = packet.ReadBits(5);
            var targetLen = packet.ReadBits(10);
            packet.ReadWoWString("Message", msgLen);
            packet.ReadWoWString("Prefix", prefixLen);
            packet.ReadWoWString("Target Name", targetLen);
        }

        [Parser(Opcode.CMSG_CHAT_MESSAGE_EMOTE, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleClientChatMessageEmote(Packet packet)
        {
            packet.ReadCString("Message");
        }

        [Parser(Opcode.CMSG_CHAT_MESSAGE_EMOTE, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleClientChatMessageEmote434(Packet packet)
        {
            var len = packet.ReadBits(9);
            packet.ReadWoWString("Message", len);
        }

        [Parser(Opcode.CMSG_CHAT_MESSAGE_GUILD)]
        [Parser(Opcode.CMSG_CHAT_MESSAGE_YELL)]
        [Parser(Opcode.CMSG_CHAT_MESSAGE_SAY)]
        [Parser(Opcode.CMSG_CHAT_MESSAGE_RAID)]
        [Parser(Opcode.CMSG_CHAT_MESSAGE_OFFICER)]
        public static void HandleClientChatMessageSay(Packet packet)
        {
            packet.ReadInt32E<Language>("Language");
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V4_3_0_15005))
                packet.ReadWoWString("Message", packet.ReadBits(9));
            else
                packet.ReadCString("Message");
        }

        [Parser(Opcode.CMSG_CHAT_MESSAGE_AFK, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleMessageChatAfk(Packet packet)
        {
            packet.ReadCString("Away Message");
        }

        [Parser(Opcode.CMSG_CHAT_MESSAGE_AFK, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleMessageChatAfk434(Packet packet)
        {
            var len = packet.ReadBits(9);
            packet.ReadWoWString("Away Message", len);
        }

        [Parser(Opcode.CMSG_CHAT_MESSAGE_BATTLEGROUND, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleMessageChatBattleground434(Packet packet)
        {
            packet.ReadInt32E<Language>("Language"); // not confirmed
            var len = packet.ReadBits(9);
            packet.ReadWoWString("Message", len);
        }

        [Parser(Opcode.CMSG_CHAT_MESSAGE_DND, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleMessageChatDND434(Packet packet)
        {
            var len = packet.ReadBits(9);
            packet.ReadWoWString("Message", len);
        }

        [Parser(Opcode.CMSG_CHAT_MESSAGE_CHANNEL, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleClientChatMessageChannel(Packet packet)
        {
            packet.ReadInt32E<Language>("Language");
            packet.ReadCString("Message");
            packet.ReadCString("Channel Name");
        }

        [Parser(Opcode.CMSG_CHAT_MESSAGE_CHANNEL, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleClientChatMessageChannel434(Packet packet)
        {
            packet.ReadInt32E<Language>("Language");
            var channelNameLen = packet.ReadBits(10);
            var msgLen = packet.ReadBits(9);

            packet.ReadWoWString("Message", msgLen);
            packet.ReadWoWString("Channel Name", channelNameLen);
        }

        [Parser(Opcode.SMSG_CHAT_RESTRICTED)]
        public static void HandleChatRestricted(Packet packet)
        {
            packet.ReadByteE<ChatRestrictionType>("Restriction");
        }
    }
}
