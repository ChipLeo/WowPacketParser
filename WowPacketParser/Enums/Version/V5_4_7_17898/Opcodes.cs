using WowPacketParser.Misc;

namespace WowPacketParser.Enums.Version.V5_4_7_17898
{
    public static class Opcodes_5_4_7
    {
        public static BiDictionary<Opcode, int> Opcodes(Direction direction)
        {
            switch (direction)
            {
                case Direction.ClientToServer:
                case Direction.BNClientToServer:
                    return ClientOpcodes;
                case Direction.ServerToClient:
                case Direction.BNServerToClient:
                    return ServerOpcodes;
                default:
                    return MiscOpcodes;
            }
        }

        private static readonly BiDictionary<Opcode, int> ClientOpcodes = new BiDictionary<Opcode, int>
        {
            {Opcode.CMSG_ACCEPT_TRADE, 0x1E73}, // 5.4.7 18019
            {Opcode.CMSG_ACTIVATE_TAXI, 0x0756}, // 5.4.7 18019
            {Opcode.CMSG_ACTIVATE_TAXI_EXPRESS, 0x0576}, // 5.4.7 18019
            {Opcode.CMSG_ADDON_REGISTERED_PREFIXES, 0x1C40}, // 5.4.7 18019
            {Opcode.CMSG_ADD_FRIEND, 0x064F}, // 5.4.7 18019
            {Opcode.CMSG_ADD_IGNORE, 0x126D}, // 5.4.7 18019
            {Opcode.CMSG_ALTER_APPEARANCE, 0x00F6}, // 5.4.7 18019
            {Opcode.CMSG_AREA_SPIRIT_HEALER_QUERY, 0x0376}, // 5.4.7 18019
            {Opcode.CMSG_AREA_SPIRIT_HEALER_QUEUE, 0x04D5}, // 5.4.7 18019
            {Opcode.CMSG_AREA_TRIGGER, 0x155A}, // 5.4.7 18019
            {Opcode.CMSG_ATTACK_STOP, 0x1E13}, // 5.4.7 18019
            {Opcode.CMSG_ATTACK_SWING, 0x1513}, // 5.4.7 18019
            {Opcode.CMSG_AUCTION_HELLO_REQUEST, 0x047F}, // 5.4.7 18019
            {Opcode.CMSG_AUCTION_LIST_BIDDER_ITEMS, 0x04D6}, // 5.4.7 18019
            {Opcode.CMSG_AUCTION_LIST_ITEMS, 0x105F}, // 5.4.7 18019
            {Opcode.CMSG_AUCTION_LIST_OWNER_ITEMS, 0x105E}, // 5.4.7 18019
            {Opcode.CMSG_AUCTION_LIST_PENDING_SALES, 0x1055}, // 5.4.7 18019
            {Opcode.CMSG_AUCTION_PLACE_BID, 0x10F5}, // 5.4.7 18019
            {Opcode.CMSG_AUCTION_REMOVE_ITEM, 0x0754}, // 5.4.7 18019
            {Opcode.CMSG_AUCTION_SELL_ITEM, 0x07F5}, // 5.4.7 18019
            {Opcode.CMSG_AUTH_CONTINUED_SESSION, 0x1A5B}, // 5.4.7 18019 Not implemented - CHECK!
            {Opcode.CMSG_AUTH_SESSION, 0x1A51}, // 5.4.7 18019
            {Opcode.CMSG_AUTOBANK_ITEM, 0x1C42}, // 5.4.7 18019
            {Opcode.CMSG_AUTOSTORE_BANK_ITEM, 0x176A}, // 5.4.7 18019
            {Opcode.CMSG_AUTOSTORE_LOOT_ITEM, 0x1F7A}, // 5.4.7 18019
            {Opcode.CMSG_AUTO_DECLINE_GUILD_INVITES, 0x0174}, // 5.4.7 18019
            {Opcode.CMSG_AUTO_EQUIP_ITEM, 0x166B}, // 5.4.7 18019
            {Opcode.CMSG_AUTO_EQUIP_ITEM_SLOT, 0x154A}, // 5.4.7 18019
            {Opcode.CMSG_AUTO_STORE_BAG_ITEM, 0x162B}, // 5.4.7 18019
            {Opcode.CMSG_BANKER_ACTIVATE, 0x02FD}, // 5.4.7 18019
            {Opcode.CMSG_BATTLEFIELD_JOIN, 0x1C53}, // 5.4.7 18019
            {Opcode.CMSG_BATTLEFIELD_LEAVE, 0x1C5B}, // 5.4.7 18019
            {Opcode.CMSG_BATTLEFIELD_LIST, 0x1412}, // 5.4.7 18019
            {Opcode.CMSG_BATTLEFIELD_PORT, 0x045F}, // 5.4.7 18019
            {Opcode.CMSG_BATTLEFIELD_STATUS, 0x04A2}, // client sends 0x19B0 before status - CHECK!
            {Opcode.CMSG_BATTLEMASTER_JOIN, 0x06DD}, // 5.4.7 18019
            {Opcode.CMSG_BATTLEMASTER_JOIN_ARENA, 0x0557}, // 5.4.7 18019
            {Opcode.CMSG_BATTLEMASTER_JOIN_RATED, 0x13AB}, // 5.4.7 18019
            {Opcode.CMSG_BATTLE_PET_REQUEST_JOURNAL, 0x12C1}, // 5.4.7 18019
            {Opcode.CMSG_BATTLE_PET_SUMMON_COMPANION, 0x17C0}, // 5.4.7 18019
            {Opcode.CMSG_BEGIN_TRADE, 0x1D52}, // 5.4.7 18019 - CHECK!
            {Opcode.CMSG_BINDER_ACTIVATE, 0x0477}, // 5.4.7 18019
            {Opcode.CMSG_BLACK_MARKET_BID_ON_ITEM, 0x03FE}, // 5.4.7 18019
            {Opcode.CMSG_BLACK_MARKET_OPEN, 0x0374}, // 5.4.7 18019
            {Opcode.CMSG_BLACK_MARKET_REQUEST_ITEMS, 0x06D4}, // 5.4.7 18019
            {Opcode.CMSG_BUG, 0x139B}, // 5.4.7 18019 - CHECK!
            {Opcode.CMSG_BUY_BACK_ITEM, 0x07D7}, // 5.4.7 18019
            {Opcode.CMSG_BUY_BANK_SLOT, 0x00FE}, // 5.4.7 18019
            {Opcode.CMSG_BUY_ITEM, 0x1077}, // 5.4.7 18019
            {Opcode.CMSG_CALENDAR_ADD_EVENT, 0x16D0}, // 5.4.7 18019
            {Opcode.CMSG_CALENDAR_EVENT_INVITE, 0x1551}, // 5.4.7 18019
            {Opcode.CMSG_CALENDAR_GET_CALENDAR, 0x19A3}, // 5.4.7 18019
            {Opcode.CMSG_CALENDAR_GET_NUM_PENDING, 0x18B0}, // 5.4.7 18019
            {Opcode.CMSG_CALENDAR_REMOVE_EVENT, 0x1DA1}, // 5.4.7 18019 - CHECK!
            {Opcode.CMSG_CANCEL_AURA, 0x16E1}, // 5.4.7 18019
            {Opcode.CMSG_CANCEL_AUTO_REPEAT_SPELL, 0x025E}, // 5.4.7 18019
            {Opcode.CMSG_CANCEL_CAST, 0x12EB}, // 5.4.7 18019
            {Opcode.CMSG_CANCEL_CHANNELLING, 0x13F0}, // 5.4.7 18019
            {Opcode.CMSG_CANCEL_MOUNT_AURA, 0x1552}, // 5.4.7 18019
            {Opcode.CMSG_CANCEL_TRADE, 0x1D32}, // 5.4.7 18019
            {Opcode.CMSG_CAST_SPELL, 0x1E5B}, // 5.4.7 18019
            {Opcode.CMSG_CHANGE_SEATS_ON_CONTROLLED_VEHICLE, 0x053B}, // 5.4.7 18019
            {Opcode.CMSG_CHARACTER_RENAME_REQUEST, 0x1391}, // 5.4.7 18019
            {Opcode.CMSG_CHAR_CUSTOMIZE, 0x1880}, // 5.4.7 18019
            {Opcode.CMSG_CHAR_DELETE, 0x113B}, // 5.4.7 18019
            {Opcode.CMSG_CHAR_RACE_OR_FACTION_CHANGE, 0x0DB8}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_ADDON_MESSAGE_GUILD, 0x0461}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_ADDON_MESSAGE_PARTY, 0x0C40}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_ADDON_MESSAGE_RAID, 0x1740}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_ADDON_MESSAGE_WHISPER, 0x0D09}, // 5.4.7 18019 Not implemented - CHECK!
            {Opcode.CMSG_CHAT_CHANNEL_ANNOUNCEMENTS, 0x0623}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_CHANNEL_BAN, 0x1E69}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_CHANNEL_INVITE, 0x1D68}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_CHANNEL_KICK, 0x1C68}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_CHANNEL_LIST, 0x1D08}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_CHANNEL_MODERATOR, 0x0F03}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_CHANNEL_MUTE, 0x0E42}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_CHANNEL_OWNER, 0x1508}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_CHANNEL_PASSWORD, 0x0F69}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_CHANNEL_UNBAN, 0x1F09}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_CHANNEL_UNMODERATOR, 0x1C01}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_CHANNEL_UNMUTE, 0x0723}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_JOIN_CHANNEL, 0x1D20},
            {Opcode.CMSG_CHAT_LEAVE_CHANNEL, 0x0E0B},
            {Opcode.CMSG_CHAT_MESSAGE_AFK, 0x0422},
            {Opcode.CMSG_CHAT_MESSAGE_BATTLEGROUND, 0x0F02}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_MESSAGE_CHANNEL, 0x0904},
            {Opcode.CMSG_CHAT_MESSAGE_DND, 0x1E21},
            {Opcode.CMSG_CHAT_MESSAGE_EMOTE, 0x0520},
            {Opcode.CMSG_CHAT_MESSAGE_GUILD, 0x070B},
            {Opcode.CMSG_CHAT_MESSAGE_OFFICER, 0x1F69},
            {Opcode.CMSG_CHAT_MESSAGE_PARTY, 0x0642}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_MESSAGE_RAID, 0x054B},
            {Opcode.CMSG_CHAT_MESSAGE_RAID_WARNING, 0x0423},
            {Opcode.CMSG_CHAT_MESSAGE_SAY, 0x0C41},
            {Opcode.CMSG_CHAT_MESSAGE_WHISPER, 0x0D60},
            {Opcode.CMSG_CHAT_MESSAGE_YELL, 0x0C43},
            {Opcode.CMSG_CHAT_REPORT_IGNORED, 0x0720}, // 5.4.7 18019
            {Opcode.CMSG_CHAT_UNREGISTER_ALL_ADDON_PREFIXES, 0x072B}, // 5.4.7 18019
            {Opcode.CMSG_CLEAR_RAID_MARKER, 0x14E9}, // 5.4.7 18019
            {Opcode.CMSG_CLEAR_TRADE_ITEM, 0x10E0}, // 5.4.7 18019
            {Opcode.CMSG_CLIENT_PORT_GRAVEYARD, 0x0257},
            //{Opcode.CMSG_COMMENTATOR_SKIRMISH_QUEUE_COMMAND, 0x051B}, //?? movement opcode
            {Opcode.CMSG_COMPLAINT, 0x1791}, // 5.4.7 18019
            {Opcode.CMSG_COMPLETE_CINEMATIC, 0x12F8}, // 5.4.7 18019
            {Opcode.CMSG_COMPLETE_MOVIE, 0x055E}, // 5.4.7 18019
            {Opcode.CMSG_CONFIRM_RESPEC_WIPE, 0x1712}, // 5.4.7 18019
            {Opcode.CMSG_CONNECT_TO_FAILED, 0x16C8},
            {Opcode.CMSG_CONTACT_LIST, 0x1186},
            {Opcode.CMSG_CORPSE_MAP_POSITION_QUERY, 0x0DA2}, // 5.4.7 18019
            {Opcode.CMSG_CORPSE_QUERY, 0x129B},
            {Opcode.CMSG_CREATE_CHARACTER, 0x09B9}, // 5.4.7 18019
            {Opcode.CMSG_CUF_PROFILES_SAVE, 0x0C88}, // CMSG_SAVE_CUF_PROFILES
            {Opcode.CMSG_DB_QUERY_BULK, 0x16C2},
            {Opcode.CMSG_DEL_FRIEND, 0x1707},
            {Opcode.CMSG_DEL_IGNORE, 0x0385},
            {Opcode.CMSG_DESTROY_ITEM, 0x1F12},
            {Opcode.CMSG_DF_BOOT_PLAYER_VOTE, 0x1CBA}, // CMSG_LFG_SET_BOOT_VOTE
            {Opcode.CMSG_DF_JOIN, 0x18B8}, // CMSG_LFG_JOIN
            {Opcode.CMSG_DF_LEAVE, 0x0D83},
            {Opcode.CMSG_DF_PROPOSAL_RESPONSE, 0x1C99}, // CMSG_LFG_PROPOSAL_RESULT
            {Opcode.CMSG_DF_SET_ROLES, 0x1A9B}, // CMSG_LFG_SET_ROLES
            {Opcode.CMSG_DF_TELEPORT, 0x1999}, // CMSG_LFG_TELEPORT
            {Opcode.CMSG_DISMISS_CONTROLLED_VEHICLE, 0x0979}, // 5.4.7 18019
            {Opcode.CMSG_DO_READY_CHECK, 0x0D88},
            {Opcode.CMSG_DUEL_PROPOSED, 0x19B3}, // 5.4.7 18019
            {Opcode.CMSG_DUEL_RESPONSE, 0x07FC}, // 5.4.7 18019
            {Opcode.CMSG_EJECT_PASSENGER, 0x04EA}, // 5.4.7 18019
            {Opcode.CMSG_EMOTE, 0x12C5}, // 5.4.7 18019
            {Opcode.CMSG_ENUM_CHARACTERS, 0x12C2}, // 5.4.7 18019
            {Opcode.CMSG_EQUIPMENT_SET_DELETE, 0x01DC}, // 5.4.7 18019
            {Opcode.CMSG_EQUIPMENT_SET_USE, 0x1402},
            {Opcode.CMSG_FAR_SIGHT, 0x01D5}, // 5.4.7 18019
            {Opcode.CMSG_FORCE_MOVE_ROOT_ACK, 0x0118},
            {Opcode.CMSG_FORCE_MOVE_UNROOT_ACK, 0x0458}, // 5.4.7 18019
            {Opcode.CMSG_GAME_OBJ_REPORT_USE, 0x06DF},
            {Opcode.CMSG_GAME_OBJ_USE, 0x055F},
            {Opcode.CMSG_GAME_STORE_BUY, 0x1A83}, // correct name?
            {Opcode.CMSG_GAME_STORE_LIST, 0x1993},
            {Opcode.CMSG_GENERATE_RANDOM_CHARACTER_NAME, 0x1DB9},
            {Opcode.CMSG_GET_ITEM_PURCHASE_DATA, 0x10DC},
            {Opcode.CMSG_GET_MIRROR_IMAGE_DATA, 0x12F9}, // 5.4.7 18019
            {Opcode.CMSG_GM_TICKET_CREATE, 0x103B}, // 5.4.7 18019
            {Opcode.CMSG_GM_TICKET_DELETE_TICKET, 0x17C8}, // 5.4.7 18019
            {Opcode.CMSG_GM_TICKET_GET_SYSTEM_STATUS, 0x128A}, // 5.4.7 18019
            {Opcode.CMSG_GM_TICKET_GET_TICKET, 0x1CA1}, // 5.4.7 18019
            {Opcode.CMSG_GM_TICKET_GET_WEB_TICKET, 0x1389}, // 5.4.7 18019
            {Opcode.CMSG_GM_TICKET_RESPONSE_RESOLVE, 0x1CA9}, // 5.4.7 18019
            {Opcode.CMSG_GM_TICKET_UPDATE_TEXT, 0x0DA0}, // 5.4.7 18019
            {Opcode.CMSG_GOSSIP_HELLO, 0x05F6},
            {Opcode.CMSG_GOSSIP_SELECT_OPTION, 0x02D7},
            {Opcode.CMSG_GROUP_ASSIGNMENT, 0x18A2},
            {Opcode.CMSG_GROUP_ASSISTANT_LEADER, 0x0DBB}, // 5.4.7 18019
            {Opcode.CMSG_GROUP_CHANGE_SUB_GROUP, 0x17D9}, // 5.4.7 18019
            {Opcode.CMSG_GROUP_DISBAND, 0x0DB2},
            {Opcode.CMSG_GROUP_INVITE, 0x1990},
            {Opcode.CMSG_GROUP_INVITE_RESPONSE, 0x1C51},
            {Opcode.CMSG_GROUP_RAID_CONVERT, 0x19A0},
            {Opcode.CMSG_GROUP_REQUEST_JOIN_UPDATES, 0x1792},
            {Opcode.CMSG_GROUP_SET_LEADER, 0x1383},
            {Opcode.CMSG_GROUP_SET_ROLES, 0x1C93},
            {Opcode.CMSG_GROUP_SWAP_SUB_GROUP, 0x1D80},
            {Opcode.CMSG_GROUP_UNINVITE_GUID, 0x0989},
            {Opcode.CMSG_GUILD_ACCEPT, 0x18A3},
            {Opcode.CMSG_GUILD_ACHIEVEMENT_PROGRESS_QUERY, 0x1B3D}, // 5.4.7 18019
            {Opcode.CMSG_GUILD_ADD_RANK, 0x1935},
            {Opcode.CMSG_GUILD_ASSIGN_MEMBER_RANK, 0x198F}, // 5.4.7 18019
            {Opcode.CMSG_GUILD_BANK_ACTIVATE, 0x02F6},
            {Opcode.CMSG_GUILD_BANK_BUY_TAB, 0x02D6},
            {Opcode.CMSG_GUILD_BANK_DEPOSIT_MONEY, 0x03FC}, // 5.4.7 18019
            {Opcode.CMSG_GUILD_BANK_LOG_QUERY, 0x1D97}, // 5.4.7 18019
            {Opcode.CMSG_GUILD_BANK_QUERY_TAB, 0x07DC},
            {Opcode.CMSG_GUILD_BANK_REMAINING_WITHDRAW_MONEY_QUERY, 0x1BB7}, // 5.4.7 18019
            {Opcode.CMSG_GUILD_BANK_SWAP_ITEMS, 0x02DF}, // 5.4.7 18019
            {Opcode.CMSG_GUILD_BANK_TEXT_QUERY, 0x19A6}, // 5.4.7 18019
            {Opcode.CMSG_GUILD_BANK_UPDATE_TAB, 0x1054}, // 5.4.7 18019
            {Opcode.CMSG_GUILD_BANK_WITHDRAW_MONEY, 0x06F4}, // 5.4.7 18019
            {Opcode.CMSG_GUILD_DECLINE_INVITATION, 0x1B05},
            {Opcode.CMSG_GUILD_DELETE_RANK, 0x1D3C},
            {Opcode.CMSG_GUILD_DEMOTE_MEMBER, 0x1B1C},
            {Opcode.CMSG_GUILD_DISBAND, 0x190E},
            {Opcode.CMSG_GUILD_EVENT_LOG_QUERY, 0x1D17}, // 5.4.7 18019
            {Opcode.CMSG_GUILD_GET_RANKS, 0x1BBC},
            {Opcode.CMSG_GUILD_GET_ROSTER, 0x19BC},
            {Opcode.CMSG_GUILD_INFO_TEXT, 0x1DAD}, // 5.4.7 18019
            {Opcode.CMSG_GUILD_INVITE, 0x188B}, // 5.4.7 18019
            {Opcode.CMSG_GUILD_LEAVE, 0x193D},
            {Opcode.CMSG_GUILD_MOTD, 0x1DB4},
            {Opcode.CMSG_GUILD_NEWS_UPDATE_STICKY, 0x1984},
            {Opcode.CMSG_GUILD_OFFICER_REMOVE_MEMBER, 0x1D9F},
            {Opcode.CMSG_GUILD_PERMISSIONS_QUERY, 0x1D0F},
            {Opcode.CMSG_GUILD_PROMOTE_MEMBER, 0x19B5},
            {Opcode.CMSG_GUILD_QUERY_NEWS, 0x1D35},
            {Opcode.CMSG_GUILD_QUERY_RECIPES, 0x1DBC}, // 5.4.7 18019
            {Opcode.CMSG_GUILD_REQUEST_CHALLENGE_UPDATE, 0x198C}, // 5.4.7 18019 //0x192E
            {Opcode.CMSG_GUILD_SET_ACHIEVEMENT_TRACKING, 0x1BA7}, // 5.4.7 18019
            {Opcode.CMSG_GUILD_SET_GUILD_MASTER, 0x189B},
            {Opcode.CMSG_GUILD_SET_NOTE, 0x1905}, // 5.4.7 18019
            {Opcode.CMSG_GUILD_SET_RANK_PERMISSIONS, 0x19BD},
            {Opcode.CMSG_GUILD_SWITCH_RANK, 0x199C}, // 5.4.7 18019
            {Opcode.CMSG_HEARTH_AND_RESURRECT, 0x065F}, // 5.4.7 18019
            {Opcode.CMSG_IGNORE_TRADE, 0x0C9A}, // 5.4.7 18019
            {Opcode.CMSG_INITIATE_TRADE, 0x12BB}, // 5.4.7 18019
            {Opcode.CMSG_INSPECT, 0x01D4},
            {Opcode.CMSG_INSTANCE_LOCK_RESPONSE, 0x067F}, // 5.4.7 18019
            {Opcode.CMSG_ITEM_PURCHASE_REFUND, 0x05FC}, // 5.4.7 18019
            {Opcode.CMSG_ITEM_TEXT_QUERY, 0x131B}, // 5.4.7 18019
            {Opcode.CMSG_ITEM_UPGRADE, 0x11E9}, // CMSG_UPGRADE_ITEM
            {Opcode.CMSG_KEEP_ALIVE, 0x13C8}, // 5.4.7 18019
            {Opcode.CMSG_LEARN_PET_SPECIALIZATION_GROUP, 0x145B}, // 5.4.7 18019
            //{Opcode.CMSG_LEARN_SPELL, 0x0010}, //??
            {Opcode.CMSG_LEARN_TALENTS, 0x1F5A}, // 5.4.7 18019
            {Opcode.CMSG_LFG_GET_SYSTEM_INFO, 0x133B}, // 5.4.7 18019
            {Opcode.CMSG_LFG_LEAVE, 0x0D83}, // 5.4.7 18019
            {Opcode.CMSG_LFG_PLAYER_LOCK_INFO_REQUEST, 0x19AA}, // 5.4.7 18019
            {Opcode.CMSG_LFG_SET_COMMENT, 0x1C88},
            {Opcode.CMSG_LF_GUILD_ADD_RECRUIT, 0x1DAA}, // 5.4.7 18019
            {Opcode.CMSG_LF_GUILD_BROWSE, 0x0992}, // 5.4.7 18019
            {Opcode.CMSG_LF_GUILD_DECLINE_RECRUIT, 0x190C}, // 5.4.7 18019
            {Opcode.CMSG_LF_GUILD_GET_APPLICATIONS, 0x1B86}, // 5.4.7 18019
            {Opcode.CMSG_LF_GUILD_GET_RECRUITS, 0x1B3E}, // 5.4.7 18019
            {Opcode.CMSG_LF_GUILD_POST_REQUEST, 0x192E}, // 5.4.7 18019
            {Opcode.CMSG_LF_GUILD_REMOVE_RECRUIT, 0x1D05}, // 5.4.7 18019
            {Opcode.CMSG_LF_GUILD_SET_GUILD_POST, 0x13C2}, // 5.4.7 18019
            {Opcode.CMSG_LIST_INVENTORY, 0x10DD},
            {Opcode.CMSG_LOADING_SCREEN_NOTIFY, 0x1691},
            {Opcode.CMSG_LOGOUT_CANCEL, 0x11D4},
            {Opcode.CMSG_LOGOUT_REQUEST, 0x0476},
            {Opcode.CMSG_LOG_DISCONNECT, 0x1A13},
            {Opcode.CMSG_LOOT_MASTER_GIVE, 0x14A1}, // 5.4.7 18019
            {Opcode.CMSG_LOOT_MONEY, 0x15A0},
            {Opcode.CMSG_LOOT_RELEASE, 0x12F0},
            {Opcode.CMSG_LOOT_ROLL, 0x1F53}, // 5.4.7 18019
            {Opcode.CMSG_LOOT_UNIT, 0x1E52},
            {Opcode.CMSG_MAIL_CREATE_TEXT_ITEM, 0x0254}, // 5.4.7 18019
            {Opcode.CMSG_MAIL_DELETE, 0x13A3}, // 5.4.7 18019
            {Opcode.CMSG_MAIL_GET_LIST, 0x07DD},
            {Opcode.CMSG_MAIL_MARK_AS_READ, 0x027F}, // 5.4.7 18019
            {Opcode.CMSG_MAIL_RETURN_TO_SENDER, 0x1C8A}, // 5.4.7 18019
            {Opcode.CMSG_MAIL_TAKE_ITEM, 0x06F6}, // 5.4.7 18019
            {Opcode.CMSG_MAIL_TAKE_MONEY, 0x0676}, // 5.4.7 18019
            {Opcode.CMSG_MINIMAP_PING, 0x1A93}, // 5.4.7 18019
            {Opcode.CMSG_MOUNT_SPECIAL_ANIM, 0x1F32}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_CHANGE_TRANSPORT, 0x0052}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_FALL_LAND, 0x055B}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_FORCE_FLIGHT_BACK_SPEED_CHANGE_ACK, 0x095A}, // 5.4.7 18019 - CHECK!
            {Opcode.CMSG_MOVE_FORCE_FLIGHT_SPEED_CHANGE_ACK, 0x0839}, // 5.4.7 18019 - CHECK!
            {Opcode.CMSG_MOVE_FORCE_RUN_BACK_SPEED_CHANGE_ACK, 0x0512}, // 5.4.7 18019 - CHECK!
            {Opcode.CMSG_MOVE_FORCE_RUN_SPEED_CHANGE_ACK, 0x0018}, // 5.4.7 18019 - CHECK!
            {Opcode.CMSG_MOVE_FORCE_SWIM_BACK_SPEED_CHANGE_ACK, 0x0070}, // 5.4.7 18019 - CHECK!
            {Opcode.CMSG_MOVE_FORCE_SWIM_SPEED_CHANGE_ACK, 0x0811}, // 5.4.7 18019 - CHECK!
            {Opcode.CMSG_MOVE_FORCE_WALK_SPEED_CHANGE_ACK, 0x0831}, // 5.4.7 18019 - CHECK!
            {Opcode.CMSG_MOVE_HEARTBEAT, 0x017B}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_JUMP, 0x0438}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_KNOCK_BACK_ACK, 0x053A}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_SET_CAN_FLY_ACK, 0x0952}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_SET_COLLISION_HEIGHT_ACK, 0x0031}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_SET_FACING, 0x005A}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_SET_FLY, 0x0551}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_SET_PITCH, 0x047A}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_SET_RUN_MODE, 0x0878}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_SET_WALK_MODE, 0x0138}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_SPLINE_DONE, 0x0833}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_START_ASCEND, 0x0430}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_START_BACKWARD, 0x0459}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_START_DESCEND, 0x0132}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_START_FORWARD, 0x041B}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_START_PITCH_DOWN, 0x093B}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_START_PITCH_UP, 0x0079}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_START_STRAFE_LEFT, 0x0873}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_START_STRAFE_RIGHT, 0x0C12}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_START_SWIM, 0x0871}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_START_TURN_LEFT, 0x011B}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_START_TURN_RIGHT, 0x0411}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_STOP, 0x0570}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_STOP_ASCEND, 0x0012}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_STOP_PITCH, 0x0071}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_STOP_STRAFE, 0x0171}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_STOP_SWIM, 0x0578}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_STOP_TURN, 0x0530}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_TELEPORT_ACK, 0x0978}, // 5.4.7 18019
            {Opcode.CMSG_MOVE_TIME_SKIPPED, 0x0152},
            {Opcode.CMSG_MOVE_WATER_WALK_ACK, 0x0519}, // 5.4.7 18019 - CHECK!
            {Opcode.CMSG_NAME_QUERY, 0x0DB3},
            {Opcode.CMSG_NEXT_CINEMATIC_CAMERA, 0x05CC}, // 5.4.7 18019
            {Opcode.CMSG_OBJECT_UPDATE_FAILED, 0x0882},
            {Opcode.CMSG_OBJECT_UPDATE_RESCUED, 0x042E}, // 5.4.7 18019
            {Opcode.CMSG_OFFER_PETITION, 0x1992}, // 5.4.7 18019
            {Opcode.CMSG_OPENING_CINEMATIC, 0x0146}, // 5.4.7 18019 Not implemented - CHECK!
            {Opcode.CMSG_OPEN_ITEM, 0x146C}, // 5.4.7 18019
            {Opcode.CMSG_PETITION_BUY, 0x07D6}, // 5.4.7 18019
            {Opcode.CMSG_PETITION_DECLINE, 0x03D6}, // 5.4.7 18019
            {Opcode.CMSG_PETITION_QUERY, 0x15A2}, // 5.4.7 18019
            {Opcode.CMSG_PETITION_RENAME, 0x1288}, // 5.4.7 18019
            {Opcode.CMSG_PETITION_SHOW_LIST, 0x0775}, // 5.4.7 18019
            {Opcode.CMSG_PETITION_SHOW_SIGNATURES, 0x11FC}, // 5.4.7 18019
            {Opcode.CMSG_PETITION_SIGN, 0x0555}, // 5.4.7 18019
            {Opcode.CMSG_PET_ABANDON, 0x05D7}, // 5.4.7 18019
            {Opcode.CMSG_PET_ACTION, 0x04D4}, // 5.4.7 18019
            {Opcode.CMSG_PET_CANCEL_AURA, 0x01F5}, // 5.4.7 18019
            {Opcode.CMSG_PET_CAST_SPELL, 0x00E8}, // 5.4.7 18019
            {Opcode.CMSG_PET_RENAME, 0x133A}, // 5.4.7 18019
            {Opcode.CMSG_PET_SET_ACTION, 0x07DE}, // 5.4.7 18019
            {Opcode.CMSG_PET_STOP_ATTACK, 0x07F7}, // 5.4.7 18019
            {Opcode.CMSG_PING, 0x1070},
            {Opcode.CMSG_PLAYER_DIFFICULTY_CHANGE, 0x09A8}, // 5.4.7 18019 Not implemented - CHECK!
            {Opcode.CMSG_PLAYER_LOGIN, 0x17D3},
            {Opcode.CMSG_PLAYER_LOGOUT, 0x0010}, // 5.4.7 18019
            {Opcode.CMSG_PLAYER_VEHICLE_ENTER, 0x13E8}, // 5.4.7 18019
            {Opcode.CMSG_PUSH_QUEST_TO_PARTY, 0x127C}, // 5.4.7 18019
            {Opcode.CMSG_PVP_LOG_DATA, 0x1F3A}, // 5.4.7 18019
            {Opcode.CMSG_QUERY_BATTLEFIELD_STATE, 0x1D33}, // 5.4.7 18019
            {Opcode.CMSG_QUERY_BATTLE_PET_NAME, 0x16E0}, // 5.4.7 18019 Not implemented - CHECK!
            {Opcode.CMSG_QUERY_CREATURE, 0x1E72},
            {Opcode.CMSG_QUERY_GAME_OBJECT, 0x14EA},
            {Opcode.CMSG_QUERY_GUILD_INFO, 0x1280},
            {Opcode.CMSG_QUERY_INSPECT_ACHIEVEMENTS, 0x047E},
            {Opcode.CMSG_QUERY_NEXT_MAIL_TIME, 0x05F7}, // 5.4.7 18019
            {Opcode.CMSG_QUERY_NPC_TEXT, 0x12FA},
            {Opcode.CMSG_QUERY_PAGE_TEXT, 0x13B1},
            {Opcode.CMSG_QUERY_PET_NAME, 0x16A3},
            {Opcode.CMSG_QUERY_QUEST_COMPLETION_NPCS, 0x16B8}, // CMSG_QUEST_NPC_QUERY
            {Opcode.CMSG_QUERY_QUEST_INFO, 0x1F52},
            {Opcode.CMSG_QUERY_REALM_NAME, 0x1899},
            {Opcode.CMSG_QUERY_TIME, 0x03FD},
            {Opcode.CMSG_QUERY_VOID_STORAGE, 0x01E1}, // 5.4.7 18019
            {Opcode.CMSG_QUEST_CONFIRM_ACCEPT, 0x0277}, // 5.4.7 18019
            {Opcode.CMSG_QUEST_GIVER_ACCEPT_QUEST, 0x0356},
            {Opcode.CMSG_QUEST_GIVER_CHOOSE_REWARD, 0x075F},
            {Opcode.CMSG_QUEST_GIVER_COMPLETE_QUEST, 0x115E},
            {Opcode.CMSG_QUEST_GIVER_HELLO, 0x1056}, // 5.4.7 18019
            {Opcode.CMSG_QUEST_GIVER_QUERY_QUEST, 0x0474}, // 5.4.7 18019
            {Opcode.CMSG_QUEST_GIVER_REQUEST_REWARD, 0x107E},
            {Opcode.CMSG_QUEST_GIVER_STATUS_MULTIPLE_QUERY, 0x0275},
            {Opcode.CMSG_QUEST_GIVER_STATUS_QUERY, 0x05FD},
            {Opcode.CMSG_QUEST_LOG_REMOVE_QUEST, 0x0655},
            {Opcode.CMSG_QUEST_POI_QUERY, 0x1DA8},
            {Opcode.CMSG_RANDOM_ROLL, 0x1891}, // 5.4.7 18019
            {Opcode.CMSG_READY_CHECK_RESPONSE, 0x13D9},
            {Opcode.CMSG_READY_FOR_ACCOUNT_DATA_TIMES, 0x13CB},
            {Opcode.CMSG_READ_ITEM, 0x14C5}, // 5.4.7 18019
            {Opcode.CMSG_REALM_SPLIT, 0x1282}, // 5.4.7 18019
            {Opcode.CMSG_RECLAIM_CORPSE, 0x065C},
            {Opcode.CMSG_REFORGE_ITEM, 0x1632},
            {Opcode.CMSG_REORDER_CHARACTERS, 0x1892},
            {Opcode.CMSG_REPAIR_ITEM, 0x0577},
            {Opcode.CMSG_REPOP_REQUEST, 0x04FC},
            {Opcode.CMSG_REPORT_PVP_PLAYER_AFK, 0x075D}, // 5.4.7 18019
            {Opcode.CMSG_REQUEST_ACCOUNT_DATA, 0x1410}, // 5.4.7 18019
            {Opcode.CMSG_REQUEST_CATEGORY_COOLDOWNS, 0x177B}, // 5.4.7 18019
            {Opcode.CMSG_REQUEST_CEMETERY_LIST, 0x14A9}, // 5.4.7 18019
            {Opcode.CMSG_REQUEST_FORCED_REACTIONS, 0x00EB}, // 5.4.7 18019
            {Opcode.CMSG_REQUEST_GUILD_PARTY_STATE, 0x14A8},
            {Opcode.CMSG_REQUEST_GUILD_REWARDS_LIST, 0x171B},
            {Opcode.CMSG_REQUEST_GUILD_XP, 0x1D37},
            {Opcode.CMSG_REQUEST_HONOR_STATS, 0x141A}, // CMSG_INSPECT_HONOR_STATS
            {Opcode.CMSG_REQUEST_INSPECT_RATED_BG_STATS, 0x1CB3},
            {Opcode.CMSG_REQUEST_PARTY_MEMBER_STATS, 0x1333},
            {Opcode.CMSG_REQUEST_PLAYED_TIME, 0x173A},
            {Opcode.CMSG_REQUEST_PVP_REWARDS, 0x0C82}, // 5.4.7 18019
            {Opcode.CMSG_REQUEST_RAID_INFO, 0x1980}, // 5.4.7 18019
            {Opcode.CMSG_REQUEST_RATED_BG_INFO, 0x1789}, // 5.4.7 18019
            {Opcode.CMSG_REQUEST_RATED_BG_STATS, 0x0D93}, // 5.4.7 18019
            {Opcode.CMSG_REQUEST_RESEARCH_HISTORY, 0x13FB}, // 5.4.7 18019
            {Opcode.CMSG_REQUEST_STABLED_PETS, 0x045D}, // 5.4.7 18019
            {Opcode.CMSG_REQUEST_VEHICLE_EXIT, 0x15EB}, // 5.4.7 18019
            {Opcode.CMSG_REQUEST_VEHICLE_NEXT_SEAT, 0x11EA}, // 5.4.7 18019 - CHECK!
            {Opcode.CMSG_REQUEST_VEHICLE_PREV_SEAT, 0x1C32}, // 5.4.7 18019 - CHECK!
            {Opcode.CMSG_REQUEST_VEHICLE_SWITCH_SEAT, 0x16EB}, // 5.4.7 18019
            {Opcode.CMSG_RESET_FACTION_CHEAT, 0x1B5A},
            {Opcode.CMSG_RESET_INSTANCES, 0x169B}, // 5.4.7 18019
            {Opcode.CMSG_RESURRECT_RESPONSE, 0x00A0}, // 5.4.7 18019
            {Opcode.CMSG_ROLE_POLL_BEGIN, 0x0D90}, // 5.4.7 18019
            {Opcode.CMSG_SAVE_EQUIPMENT_SET, 0x115C},
            {Opcode.CMSG_SAVE_GUILD_EMBLEM, 0x0C81}, // 5.4.7 18019
            {Opcode.CMSG_SELECT_FACTION, 0x1C7A}, // 5.4.7 18019
            {Opcode.CMSG_SELF_RES, 0x10F4}, // 5.4.7 18019
            {Opcode.CMSG_SELL_ITEM, 0x115F}, // 5.4.7 18019
            {Opcode.CMSG_SEND_MAIL, 0x01A9}, // 5.4.7 18019
            {Opcode.CMSG_SEND_TEXT_EMOTE, 0x037D}, // 5.4.7 18019
            {Opcode.CMSG_SET_ACTION_BAR_TOGGLES, 0x03F5}, // 5.4.7 18019
            {Opcode.CMSG_SET_ACTION_BUTTON, 0x1393}, // 5.4.7 18019
            {Opcode.CMSG_SET_ACTIVE_MOVER, 0x091B}, // 5.4.7 18019
            {Opcode.CMSG_SET_CONTACT_NOTES, 0x03A4}, // 5.4.7 18019
            {Opcode.CMSG_SET_DUNGEON_DIFFICULTY, 0x1898}, // 5.4.7 18019
            {Opcode.CMSG_SET_EVERYONE_IS_ASSISTANT, 0x1C81}, // 5.4.7 18019
            {Opcode.CMSG_SET_FACTION_AT_WAR, 0x03F6}, // 5.4.7 18019
            {Opcode.CMSG_SET_FACTION_INACTIVE, 0x057C}, // 5.4.7 18019
            {Opcode.CMSG_SET_FACTION_NOT_AT_WAR, 0x0077}, // 5.4.7 18019
            {Opcode.CMSG_SET_GUILD_BANK_TEXT, 0x1B24}, // 5.4.7 18019
            {Opcode.CMSG_SET_LOOT_METHOD, 0x1C10},
            {Opcode.CMSG_SET_LOOT_SPECIALIZATION, 0x0176}, // 5.4.7 18019
            {Opcode.CMSG_SET_PET_SLOT, 0x12A2}, // 5.4.7 18019
            {Opcode.CMSG_SET_PLAYER_DECLINED_NAMES, 0x1281}, // 5.4.7 18019
            {Opcode.CMSG_SET_RAID_DIFFICULTY, 0x16A6}, // 5.4.7 18019 //  0x08A3, //0274??
            {Opcode.CMSG_SET_SAVED_INSTANCE_EXTEND, 0x1A98}, // 5.4.7 18019
            {Opcode.CMSG_SET_SELECTION, 0x10D5}, // 5.4.7 18019
            {Opcode.CMSG_SET_SHEATHED, 0x007D}, // 5.4.7 18019
            {Opcode.CMSG_SET_SPECIALIZATION, 0x04AA}, // 5.4.7 18019
            {Opcode.CMSG_SET_TITLE, 0x13E2}, // 5.4.7 18019
            {Opcode.CMSG_SET_TRADE_GOLD, 0x0C93}, // 5.4.7 18019
            {Opcode.CMSG_SET_TRADE_ITEM, 0x0C8A}, // 5.4.7 18019
            {Opcode.CMSG_SET_WATCHED_FACTION, 0x12D5}, // 5.4.7 18019
            {Opcode.CMSG_SHOWING_CLOAK, 0x1276}, // 5.4.7 18019
            {Opcode.CMSG_SHOWING_HELM, 0x117F}, // 5.4.7 18019
            {Opcode.CMSG_SOCKET_GEMS, 0x0375}, // 5.4.7 18019
            {Opcode.CMSG_SPELL_CLICK, 0x11FF}, // 5.4.7 18019
            {Opcode.CMSG_SPIRIT_HEALER_ACTIVATE, 0x05D4}, // 5.4.7 18019
            {Opcode.CMSG_SPLIT_ITEM, 0x140A}, // 5.4.7 18019
            {Opcode.CMSG_STAND_STATE_CHANGE, 0x157A}, // 5.4.7 18019
            {Opcode.CMSG_SUMMON_RESPONSE, 0x121A}, // 5.4.7 18019
            {Opcode.CMSG_SWAP_INV_ITEM, 0x1403}, // 5.4.7 18019
            {Opcode.CMSG_SWAP_ITEM, 0x150A}, // 5.4.7 18019
            {Opcode.CMSG_SWAP_VOID_ITEM, 0x01E2}, // 5.4.7 18019
            {Opcode.CMSG_TAXI_NODE_STATUS_QUERY, 0x01FF}, // 5.4.7 18019
            {Opcode.CMSG_TAXI_QUERY_AVAILABLE_NODES, 0x0656}, // 5.4.7 18019
            {Opcode.CMSG_TIME_SYNC_RESPONSE, 0x0413}, // 5.4.7 18019
            {Opcode.CMSG_TOGGLE_PVP, 0x16BA}, // 5.4.7 18019
            {Opcode.CMSG_TOTEM_DESTROYED, 0x1154}, // 5.4.7 18019
            //{Opcode.CMSG_TRADE_INFO, 0x19AB}, // 5.4.7 18019
            {Opcode.CMSG_TRAINER_BUY_SPELL, 0x0274}, // 5.4.7 18019
            {Opcode.CMSG_TRAINER_LIST, 0x075E}, // 5.4.7 18019
            {Opcode.CMSG_TRANSMOGRIFY_ITEMS, 0x13F8}, // 5.4.7 18019
            {Opcode.CMSG_TURN_IN_PETITION, 0x04F7}, // 5.4.7 18019
            {Opcode.CMSG_TUTORIAL_CLEAR, 0x104F}, // 5.4.7 18019
            {Opcode.CMSG_TUTORIAL_FLAG, 0x07A4}, // 5.4.7 18019
            {Opcode.CMSG_TUTORIAL_RESET, 0x024F}, // 5.4.7 18019
            {Opcode.CMSG_UI_TIME_REQUEST, 0x1CA3}, // 5.4.7 18019
            {Opcode.CMSG_UNACCEPT_TRADE, 0x1473}, // 5.4.7 18019
            {Opcode.CMSG_UNLEARN_SKILL, 0x025D}, // 5.4.7 18019
            {Opcode.CMSG_UNLOCK_VOID_STORAGE, 0x13F2}, // 5.4.7 18019
            {Opcode.CMSG_UPDATE_ACCOUNT_DATA, 0x18B2}, // 5.4.7 18019
            {Opcode.CMSG_UPDATE_RAID_TARGET, 0x13C3}, // 5.4.7 18019
            {Opcode.CMSG_USE_ITEM, 0x15E3}, // 5.4.7 18019
            {Opcode.CMSG_VIOLENCE_LEVEL, 0x05A0}, // 5.4.7 18019
            {Opcode.CMSG_VOICE_SESSION_ENABLE, 0x17C2}, // 5.4.7 18019
            {Opcode.CMSG_VOID_STORAGE_TRANSFER, 0x1F73}, // 5.4.7 18019
            {Opcode.CMSG_WARDEN_DATA, 0x1681}, // 5.4.7 18019
            {Opcode.CMSG_WHO, 0x13C1}, // 5.4.7 18019
            {Opcode.CMSG_WRAP_ITEM, 0x1623} // 5.4.7 18019
        };

        private static readonly BiDictionary<Opcode, int> ServerOpcodes = new BiDictionary<Opcode, int>
        {
            //Opcode.PET_BATTLE_REQUEST_FAILED, 0x0000},  // 5.4.7 17930 PET_BATTLE NYI (NOT SURE)
            {Opcode.SMSG_ACCOUNT_CRITERIA_UPDATE, 0x12F9}, // 5.4.7 18019 - CHECK!
            {Opcode.SMSG_ACCOUNT_DATA_TIMES, 0x0F40}, // 5.4.7 18019
            {Opcode.SMSG_ACHIEVEMENT_EARNED, 0x1D49}, // 5.4.7 18019
            {Opcode.SMSG_ACTIVATE_TAXI_REPLY, 0x1D72}, // 5.4.7 18019
            {Opcode.SMSG_ADDON_INFO, 0x10E2}, // 5.4.7 18019
            {Opcode.SMSG_ADD_RUNE_POWER, 0x1528}, // 5.4.7 18019
            {Opcode.SMSG_AI_REACTION, 0x0721}, // 5.4.7 18019
            {Opcode.SMSG_ALL_ACCOUNT_CRITERIA, 0x13F0}, // 5.4.7 18019 - CHECK!
            {Opcode.SMSG_ALL_ACHIEVEMENT_DATA, 0x072B}, // 5.4.7 18019
            {Opcode.SMSG_AREA_SPIRIT_HEALER_TIME, 0x0441}, // 5.4.7 18019
            {Opcode.SMSG_ARENA_PREP_OPPONENT_SPECIALIZATIONS, 0x0E3D}, // 5.4.7 18019
            {Opcode.SMSG_ATTACKER_STATE_UPDATE, 0x0540}, // 5.4.7 18019
            {Opcode.SMSG_ATTACK_START, 0x0403}, // 5.4.7 18019
            {Opcode.SMSG_ATTACK_STOP, 0x1448}, // 5.4.7 18019
            {Opcode.SMSG_ATTACK_SWING_ERROR, 0x04E0}, // 5.4.7 18019 SMSG_CANCEL_COMBAT
            {Opcode.SMSG_AUCTION_BIDDER_NOTIFICATION, 0x0892}, // 5.4.7 18019
            {Opcode.SMSG_AUCTION_COMMAND_RESULT, 0x1C40}, // 5.4.7 18019
            {Opcode.SMSG_AUCTION_HELLO_RESPONSE, 0x04E9}, // 5.4.7 18019
            {Opcode.SMSG_AUCTION_LIST_BIDDER_ITEMS_RESULT, 0x134E}, // 5.4.7 18019
            {Opcode.SMSG_AUCTION_LIST_OWNER_ITEMS_RESULT, 0x048F}, // 5.4.7 18019
            {Opcode.SMSG_AUCTION_LIST_PENDING_SALES, 0x11EB}, // 5.4.7 18019
            {Opcode.SMSG_AUCTION_LIST_RESULT, 0x0504}, // 5.4.7 18019
            {Opcode.SMSG_AUCTION_OUTBID_NOTIFICATION, 0x11E8}, // 5.4.7 18019
            {Opcode.SMSG_AUCTION_OWNER_BID_NOTIFICATION, 0x0C42}, // 5.4.7 18019
            {Opcode.SMSG_AURA_POINTS_DEPLETED, 0x0151},
            {Opcode.SMSG_AURA_UPDATE, 0x1B8D}, // 5.4.7 18019
            {Opcode.SMSG_AUTH_CHALLENGE, 0x14B8}, // 5.4.7 18019
            {Opcode.SMSG_AUTH_RESPONSE, 0x15A0}, // 5.4.7 18019
            {Opcode.SMSG_BARBER_SHOP_RESULT, 0x01EB}, // 5.4.7 18019
            {Opcode.SMSG_BATTLEFIELD_LIST, 0x1408}, // 5.4.7 18019
            {Opcode.SMSG_BATTLEFIELD_MGR_QUEUE_REQUEST_RESPONSE, 0x1E72}, // 5.4.7 18019
            {Opcode.SMSG_BATTLEFIELD_RATED_INFO, 0x088A}, // 5.4.7 18019
            {Opcode.SMSG_BATTLEFIELD_STATUS, 0x15EB}, // 5.4.7 18019
            {Opcode.SMSG_BATTLEFIELD_STATUS_ACTIVE, 0x10EA}, // 5.4.7 18019
            {Opcode.SMSG_BATTLEFIELD_STATUS_FAILED, 0x1E40}, // 5.4.7 18019
            {Opcode.SMSG_BATTLEFIELD_STATUS_NEED_CONFIRMATION, 0x147A}, // 5.4.7 18019
            {Opcode.SMSG_BATTLEFIELD_STATUS_QUEUED, 0x15E8}, // 5.4.7 18019
            {Opcode.SMSG_BATTLEGROUND_PLAYER_JOINED, 0x15E0}, // 5.4.7 18019
            {Opcode.SMSG_BATTLEGROUND_PLAYER_LEFT, 0x0C82}, // 5.4.7 18019
            {Opcode.SMSG_BATTLEGROUND_PLAYER_POSITIONS, 0x15AA}, // 5.4.7 18019
            //{Opcode.SMSG_BATTLEGROUND_STATUS_WARGAMES, 0x1701}, // 5.4.7 18019
            {Opcode.SMSG_BATTLEPET_CAGE_DATA_ERROR, 0x14A1}, // 5.4.7 18019 Not implemented - CHECK! These concern actual pets.
            {Opcode.SMSG_BATTLE_PETS_HEALED, 0x1C3B},
            {Opcode.SMSG_BATTLE_PET_DELETED, 0x13F1}, // 5.4.7 18019 Not implemented - CHECK!
            {Opcode.SMSG_BATTLE_PET_ERROR, 0x1C12}, // 5.4.7 18019 Not implemented - CHECK!
            {Opcode.SMSG_BATTLE_PET_JOURNAL, 0x14A0},
            {Opcode.SMSG_BATTLE_PET_JOURNAL_LOCK_ACQUIRED, 0x1C7A},
            {Opcode.SMSG_BATTLE_PET_JOURNAL_LOCK_DENIED, 0x13E3},
            {Opcode.SMSG_BATTLE_PET_LICENSE_CHANGED, 0x11E0},
            {Opcode.SMSG_BATTLE_PET_TRAP_LEVEL, 0x13AA},
            {Opcode.SMSG_BATTLE_PET_UPDATES, 0x04E3},
            {Opcode.SMSG_BINDER_CONFIRM, 0x0F22},
            {Opcode.SMSG_BIND_POINT_UPDATE, 0x11E2},
            {Opcode.SMSG_BLACK_MARKET_BID_ON_ITEM_RESULT, 0x064B},
            {Opcode.SMSG_BLACK_MARKET_OPEN_RESULT, 0x15E2},
            {Opcode.SMSG_BLACK_MARKET_OUTBID, 0x12A3},
            {Opcode.SMSG_BLACK_MARKET_REQUEST_ITEMS_RESULT, 0x165A},
            {Opcode.SMSG_BLACK_MARKET_WON, 0x04A9},
            {Opcode.SMSG_BREAK_TARGET, 0x141A}, // 5.4.7 18019 // 0152
            {Opcode.SMSG_BUY_FAILED, 0x165B}, // 5.4.7 18019
            {Opcode.SMSG_BUY_SUCCEEDED, 0x0763},
            {Opcode.SMSG_CACHE_VERSION, 0x1E41},
            {Opcode.SMSG_CALENDAR_SEND_EVENT, 0x05E0}, // 5.4.7 18019 - CHECK!
            {Opcode.SMSG_CALENDAR_SEND_NUM_PENDING, 0x1C21}, // 5.4.7 18019
            {Opcode.SMSG_CANCEL_AUTO_REPEAT, 0x12B0},
            {Opcode.SMSG_CANCEL_COMBAT, 0x1E48}, // 5.4.7 18019
            {Opcode.SMSG_CAST_FAILED, 0x0560},
            {Opcode.SMSG_CATEGORY_COOLDOWN, 0x053B}, // SMSG_SPELL_CATEGORY_COOLDOWN
            {Opcode.SMSG_CHALLENGE_MODE_ALL_MAP_STATS, 0x1621},
            {Opcode.SMSG_CHALLENGE_MODE_COMPLETE, 0x1D53},
            {Opcode.SMSG_CHALLENGE_MODE_DELETE_LEADER_RESULT, 0x05A2},
            {Opcode.SMSG_CHALLENGE_MODE_MAP_STATS_UPDATE, 0x0C9A}, // NEW IN MOP
            {Opcode.SMSG_CHALLENGE_MODE_NEW_PLAYER_RECORD, 0x0C80},
            {Opcode.SMSG_CHALLENGE_MODE_REQUEST_LEADERS_RESULT, 0x0668},
            {Opcode.SMSG_CHALLENGE_MODE_REWARDS, 0x1413},
            {Opcode.SMSG_CHANNEL_LIST, 0x06EE},
            {Opcode.SMSG_CHANNEL_NOTIFY, 0x11C5},
            {Opcode.SMSG_CHAR_CUSTOMIZE, 0x1F60}, // 5.4.7 18019
            {Opcode.SMSG_CHAT, 0x0E60},
            {Opcode.SMSG_CHAT_SERVER_MESSAGE, 0x026E}, // 5.4.7 18019
            {Opcode.SMSG_CLEAR_COOLDOWN, 0x0C08},
            {Opcode.SMSG_CLEAR_COOLDOWNS, 0x1926}, // 5.4.7 18019
            {Opcode.SMSG_CLEAR_TARGET, 0x12EB}, // 5.4.7 18019
            {Opcode.SMSG_COMPLAINT_RESULT, 0x0D62}, // 5.4.7 18019
            {Opcode.SMSG_COMPRESSED_ACHIEVEMENT_DATA, 0x0062}, // 5.4.7 18019
            {Opcode.SMSG_CONNECT_TO, 0x05B9},
            {Opcode.SMSG_CONTACT_LIST, 0x15CF},
            {Opcode.SMSG_CONTROL_UPDATE, 0x01EA},
            {Opcode.SMSG_CONVERT_RUNE, 0x1E09}, // 5.4.7 18019
            {Opcode.SMSG_COOLDOWN_EVENT, 0x1C5B},
            {Opcode.SMSG_CORPSE_MAP_POSITION_QUERY_RESPONSE, 0x1C73},
            //{Opcode.SMSG_CORPSE_NOT_IN_INSTANCE, 0x1C3B}, // 5.4.7 18019
            {Opcode.SMSG_CORPSE_QUERY_RESPONSE, 0x1F32},
            {Opcode.SMSG_CORPSE_RECLAIM_DELAY, 0x1E73},
            {Opcode.SMSG_CREATE_CHAR, 0x1469},
            {Opcode.SMSG_CRITERIA_DELETED, 0x0622}, // 5.4.7 18019
            {Opcode.SMSG_CRITERIA_UPDATE, 0x13B2},
            {Opcode.SMSG_DB_REPLY, 0x1F01},
            {Opcode.SMSG_DEATH_RELEASE_LOC, 0x1672},
            {Opcode.SMSG_DEFENSE_MESSAGE, 0x01E8},
            {Opcode.SMSG_DELETE_CHAR, 0x1529},
            {Opcode.SMSG_DESTROY_OBJECT, 0x1D69},
            {Opcode.SMSG_DESTRUCTIBLE_BUILDING_DAMAGE, 0x0562}, // 5.4.7 18019
            {Opcode.SMSG_DISENCHANT_CREDIT, 0x0749}, // 5.4.7 18019
            {Opcode.SMSG_DISMOUNT, 0x12EA}, // 5.4.7 18019
            {Opcode.SMSG_DISMOUNT_RESULT, 0x0642}, // 5.4.7 18019
            {Opcode.SMSG_DUEL_COMPLETE, 0x0C8A}, // 5.4.7 18019
            {Opcode.SMSG_DUEL_COUNTDOWN, 0x0C91}, // 5.4.7 18019
            {Opcode.SMSG_DUEL_IN_BOUNDS, 0x0E43}, // 5.4.7 18019
            {Opcode.SMSG_DUEL_OUT_OF_BOUNDS, 0x0461}, // 5.4.7 18019
            {Opcode.SMSG_DUEL_REQUESTED, 0x0C2A}, // 5.4.7 18019
            {Opcode.SMSG_DUEL_WINNER, 0x064A}, // 5.4.7 18019
            {Opcode.SMSG_DURABILITY_DAMAGE_DEATH, 0x1713}, // 5.4.7 18019
            {Opcode.SMSG_EMOTE, 0x022F},
            {Opcode.SMSG_ENABLE_BARBER_SHOP, 0x0F6A}, // 5.4.7 18019
            {Opcode.SMSG_ENUM_CHARACTERS_RESULT, 0x040A},
            {Opcode.SMSG_ENVIRONMENTAL_DAMAGE_LOG, 0x0032}, // 5.4.7 18019
            {Opcode.SMSG_EQUIPMENT_SET_SAVED, 0x11E9}, // 5.4.7 18019
            {Opcode.SMSG_EXPLORATION_EXPERIENCE, 0x13A1}, // 5.4.7 18019
            {Opcode.SMSG_FAILED_PLAYER_CONDITION, 0x0F21},
            {Opcode.SMSG_FEATURE_SYSTEM_STATUS, 0x1560},
            {Opcode.SMSG_FLIGHT_SPLINE_SYNC, 0x0992},
            {Opcode.SMSG_FRIEND_STATUS, 0x0707},
            {Opcode.SMSG_GAMEOBJECT_DESPAWN_ANIM, 0x0609},
            {Opcode.SMSG_GAME_OBJECT_ACTIVATE_ANIM_KIT, 0x0462}, // 5.4.7 18019
            {Opcode.SMSG_GAME_OBJECT_CUSTOM_ANIM, 0x1E52},
            //{Opcode.SMSG_GAME_OBJECT_PAGE_TEXT, 0x0549}, // 5.4.7 18019
            {Opcode.SMSG_GAME_OBJECT_PLAY_SPELL_VISUAL, 0x0E68}, // 5.4.7 18019 - CHECK!
            {Opcode.SMSG_GAME_STORE_AUTH_BUY_FAILED, 0x0C40}, // CORRECT NAME?
            {Opcode.SMSG_GAME_STORE_BUY_RESULT, 0x12AB}, // CORRECT NAME?
            {Opcode.SMSG_GAME_STORE_INGAME_BUY_FAILED, 0x145A}, // CORRECT NAME?
            {Opcode.SMSG_GAME_STORE_LIST, 0x1C29},
            {Opcode.SMSG_GAME_TIME_SET, 0x037E}, // 5.4.7 18019 - CHECK!
            {Opcode.SMSG_GAME_TIME_UPDATE, 0x00A3}, // 5.4.7 18019
            {Opcode.SMSG_GENERATE_RANDOM_CHARACTER_NAME_RESULT, 0x074B},
            {Opcode.SMSG_GM_TICKET_GET_TICKET, 0x054B},
            {Opcode.SMSG_GM_TICKET_GET_TICKET_RESPONSE, 0x0C4B}, // 5.4.7 18019
            {Opcode.SMSG_GM_TICKET_STATUS_UPDATE, 0x14AB}, // 5.4.7 18019
            {Opcode.SMSG_GM_TICKET_SYSTEM_STATUS, 0x04A2}, // 5.4.7 18019
            {Opcode.SMSG_GOSSIP_COMPLETE, 0x0D45}, // 5.4.7 18019
            {Opcode.SMSG_GOSSIP_MESSAGE, 0x0E52},
            {Opcode.SMSG_GOSSIP_POI, 0x058F},
            {Opcode.SMSG_GROUP_DECLINE, 0x108F},
            {Opcode.SMSG_GROUP_DESTROYED, 0x1564},
            {Opcode.SMSG_GROUP_INVITE, 0x1472},
            {Opcode.SMSG_GROUP_LIST, 0x1E61},
            {Opcode.SMSG_GROUP_SET_LEADER, 0x15A2}, // 5.4.7 18019
            {Opcode.SMSG_GUILD_ACHIEVEMENT_DATA, 0x1B5A}, // 5.4.7 18019
            {Opcode.SMSG_GUILD_ACHIEVEMENT_EARNED, 0x1B1B}, // 5.4.7 18019
            {Opcode.SMSG_GUILD_BANK_LOG_QUERY_RESULTS, 0x1B53}, // 5.4.7 18019
            {Opcode.SMSG_GUILD_BANK_MONEY_WITHDRAWN, 0x1A70}, // 5.4.7 18019
            {Opcode.SMSG_GUILD_BANK_QUERY_RESULTS, 0x1B7B},
            {Opcode.SMSG_GUILD_BANK_TEXT_QUERY_RESULT, 0x1B3B}, // 5.4.7 18019
            {Opcode.SMSG_GUILD_CHALLENGE_COMPLETED, 0x1A10}, // 5.4.7 18019 Not implemented - CHECK!.
            {Opcode.SMSG_GUILD_CHALLENGE_UPDATE, 0x1A33},
            {Opcode.SMSG_GUILD_COMMAND_RESULT, 0x1A13},
            {Opcode.SMSG_GUILD_EVENT_LOG_QUERY_RESULTS, 0x1853}, // 5.4.7 18019
            {Opcode.SMSG_GUILD_EVENT_PLAYER_JOINED, 0x1A3B}, // 5.4.7 18019
            {Opcode.SMSG_GUILD_EVENT_RANKS_UPDATED, 0x1070}, // 5.4.7 18019
            {Opcode.SMSG_GUILD_INVITE, 0x185A},
            {Opcode.SMSG_GUILD_KNOWN_RECIPES, 0x1A12}, // 5.4.7 18019 // sub_2A6ECC
            {Opcode.SMSG_GUILD_LEAVE, 0x1870}, // 5.4.7 18019
            {Opcode.SMSG_GUILD_NEWS_TEXT, 0x1850}, // SMSG_GUILD_MOTD
            {Opcode.SMSG_GUILD_NEWS_UPDATE, 0x187B},
            {Opcode.SMSG_GUILD_PARTY_STATE, 0x1A52},
            {Opcode.SMSG_GUILD_PERMISSIONS_QUERY_RESULTS, 0x1A32},
            {Opcode.SMSG_GUILD_RANKS, 0x1271},
            {Opcode.SMSG_GUILD_REPUTATION_WEEKLY_CAP, 0x1913}, // 5.4.7 18019
            {Opcode.SMSG_GUILD_REWARD_LIST, 0x1010},
            {Opcode.SMSG_GUILD_ROSTER, 0x1231},
            //{Opcode.SMSG_GUILD_SEND_PLAYER_LOGIN_STATUS, 0x1A1A}, // 5.4.7 18019
            {Opcode.SMSG_GUILD_XP, 0x1A51},
            {Opcode.SMSG_GUILD_XP_GAIN, 0x1A11},
            {Opcode.SMSG_HIGHEST_THREAT_UPDATE, 0x0761},
            {Opcode.SMSG_HOTFIX_NOTIFY_BLOB, 0x0C81},
            {Opcode.SMSG_INITIALIZE_FACTIONS, 0x11E1},
            {Opcode.SMSG_INITIAL_SETUP, 0x12E0}, // 5.4.7 18019
            {Opcode.SMSG_INIT_WORLD_STATES, 0x0F03},
            {Opcode.SMSG_INSPECT_HONOR_STATS, 0x1429},
            {Opcode.SMSG_INSPECT_RATED_BG_STATS, 0x046B},
            {Opcode.SMSG_INSPECT_TALENT, 0x1E49}, // 5.4.7 18019
            {Opcode.SMSG_INSTANCE_INFO, 0x0C21}, // SMSG_RAID_INSTANCE_INFO
            {Opcode.SMSG_INSTANCE_RESET, 0x1F72}, // 5.4.7 18019
            {Opcode.SMSG_INSTANCE_SAVE_CREATED, 0x04A3}, // 5.4.7 18019
            {Opcode.SMSG_INVENTORY_CHANGE_FAILURE, 0x0F49},
            {Opcode.SMSG_ITEM_ENCHANT_TIME_UPDATE, 0x0660},
            {Opcode.SMSG_ITEM_PURCHASE_REFUND_RESULT, 0x153A}, // 5.4.7 18019
            {Opcode.SMSG_ITEM_PUSH_RESULT, 0x04A1},
            {Opcode.SMSG_ITEM_REFUND_INFO_RESPONSE, 0x04A0}, // 5.4.7 18019
            {Opcode.SMSG_ITEM_TIME_UPDATE, 0x1F5B}, // 5.4.7 18019
            {Opcode.SMSG_ITEM_UPGRADE_RESULT, 0x0888},
            {Opcode.SMSG_LEARNED_SPELLS, 0x0C99},
            {Opcode.SMSG_LEVEL_UP_INFO, 0x0E6A},
            {Opcode.SMSG_LFG_BOOT_PLAYER, 0x1521}, // SMSG_LFG_BOOT_PROPOSAL_UPDATE
            {Opcode.SMSG_LFG_BOOT_PROPOSAL_UPDATE, 0x1521}, // 5.4.7 18019
            {Opcode.SMSG_LFG_DISABLED, 0x171A}, // 5.4.7 18019
            {Opcode.SMSG_LFG_JOIN_RESULT, 0x12E9},
            {Opcode.SMSG_LFG_OFFER_CONTINUE, 0x0F0A}, // 5.4.7 18019
            {Opcode.SMSG_LFG_PARTY_INFO, 0x1F28},
            {Opcode.SMSG_LFG_PLAYER_INFO, 0x13B0},
            {Opcode.SMSG_LFG_PLAYER_REWARD, 0x0D21}, // 5.4.7 18019
            {Opcode.SMSG_LFG_PROPOSAL_UPDATE, 0x0C23}, // 5.4.7 18019
            {Opcode.SMSG_LFG_QUEUE_STATUS, 0x0D40},
            {Opcode.SMSG_LFG_ROLE_CHECK_UPDATE, 0x0541},
            {Opcode.SMSG_LFG_ROLE_CHOSEN, 0x0701}, // 5.4.7 18019
            {Opcode.SMSG_LFG_SLOT_INVALID, 0x1F7B}, // 5.4.7 18019 // 3x uint32
            {Opcode.SMSG_LFG_TELEPORT_DENIED, 0x1EA3}, // 5.4.7 18019
            {Opcode.SMSG_LFG_UPDATE_STATUS, 0x1661},
            {Opcode.SMSG_LF_GUILD_BROWSE_UPDATED, 0x1852}, // 5.4.7 18019
            {Opcode.SMSG_LF_GUILD_COMMAND_RESULT, 0x1833}, // 5.4.7 18019
            {Opcode.SMSG_LF_GUILD_MEMBERSHIP_LIST_UPDATED, 0x1952}, // 5.4.7 18019
            {Opcode.SMSG_LF_GUILD_POST_UPDATED, 0x1972}, // 5.4.7 18019
            {Opcode.SMSG_LF_GUILD_RECRUIT_LIST_UPDATED, 0x1051}, // 5.4.7 18019
            {Opcode.SMSG_LOAD_CUF_PROFILES, 0x15A9}, // 5.4.7 18019
            {Opcode.SMSG_LOAD_EQUIPMENT_SET, 0x1520},
            {Opcode.SMSG_LOGIN_SET_TIME_SPEED, 0x0F4A},
            {Opcode.SMSG_LOGIN_VERIFY_WORLD, 0x0603},
            {Opcode.SMSG_LOGOUT_CANCEL_ACK, 0x0E0A},
            {Opcode.SMSG_LOGOUT_COMPLETE, 0x0429},
            {Opcode.SMSG_LOGOUT_RESPONSE, 0x0D2B},
            {Opcode.SMSG_LOG_XP_GAIN, 0x1613},
            {Opcode.SMSG_LOOT_ALL_PASSED, 0x0126}, // 5.4.7 18019
            {Opcode.SMSG_LOOT_CLEAR_MONEY, 0x0C89}, // SMSG_COIN_REMOVED
            {Opcode.SMSG_LOOT_MONEY_NOTIFY, 0x1F49},
            {Opcode.SMSG_LOOT_RELEASE, 0x14A2},
            {Opcode.SMSG_LOOT_REMOVED, 0x0D00},
            {Opcode.SMSG_LOOT_RESPONSE, 0x1F41},
            {Opcode.SMSG_LOOT_ROLL, 0x0422}, // 5.4.7 18019
            {Opcode.SMSG_LOOT_ROLLS_COMPLETE, 0x1473}, // 5.4.7 18019
            {Opcode.SMSG_LOOT_ROLL_WON, 0x12F2}, // 5.4.7 18019
            {Opcode.SMSG_LOOT_START_ROLL, 0x1420}, // 5.4.7 18019
            {Opcode.SMSG_MAIL_COMMAND_RESULT, 0x0702}, // 5.4.7 18019
            {Opcode.SMSG_MAIL_LIST_RESULT, 0x0401},
            {Opcode.SMSG_MASTER_LOOT_CANDIDATE_LIST, 0x1D5B}, // 5.4.7 18019
            {Opcode.SMSG_MINIMAP_PING, 0x1501}, // 5.4.7 18019
            {Opcode.SMSG_MIRROR_IMAGE_CREATURE_DATA, 0x1917}, // 5.4.7 18019
            {Opcode.SMSG_MODIFY_COOLDOWN, 0x1D08}, // 5.4.7 18019
            {Opcode.SMSG_MONEY_NOTIFY, 0x05E2},
            {Opcode.SMSG_MOTD, 0x0E20},
            {Opcode.SMSG_MOUNT_SPECIAL_ANIM, 0x0E4B}, // 5.4.7 18019
            {Opcode.SMSG_MOVE_APPLY_MOVEMENT_FORCE, 0x1232},
            {Opcode.SMSG_MOVE_FEATHER_FALL, 0x16D2}, // 5.4.7 18019
            {Opcode.SMSG_MOVE_KNOCK_BACK, 0x1999}, // 5.4.7 18019
            {Opcode.SMSG_MOVE_LAND_WALK, 0x1E9B}, // 5.4.7 18019
            {Opcode.SMSG_MOVE_NORMAL_FALL, 0x0D89}, // 5.4.7 18019
            {Opcode.SMSG_MOVE_REMOVE_MOVEMENT_FORCE, 0x133B},
            {Opcode.SMSG_MOVE_ROOT, 0x198B},
            {Opcode.SMSG_MOVE_SET_ACTIVE_MOVER, 0x129A},
            {Opcode.SMSG_MOVE_SET_CAN_FLY, 0x01F4}, // 5.4.7 17956
            {Opcode.SMSG_MOVE_SET_COLLISION_HEIGHT, 0x0DB0}, // 5.4.7 18019
            {Opcode.SMSG_MOVE_SET_FLIGHT_BACK_SPEED, 0x13D2}, // 5.4.7 18019
            {Opcode.SMSG_MOVE_SET_FLIGHT_SPEED, 0x02DC}, // 5.4.7 17956
            {Opcode.SMSG_MOVE_SET_HOVERING, 0x1D50}, // 5.4.7 18019
            {Opcode.SMSG_MOVE_SET_PITCH_RATE, 0x138B}, // 5.4.7 18019
            {Opcode.SMSG_MOVE_SET_RUN_BACK_SPEED, 0x0D93}, // 5.4.7 18019
            {Opcode.SMSG_MOVE_SET_RUN_SPEED, 0x1B9B}, // 5.4.7 17956
            {Opcode.SMSG_MOVE_SET_SWIM_BACK_SPEED, 0x015D}, // 5.4.7 18019
            {Opcode.SMSG_MOVE_SET_SWIM_SPEED, 0x01D4}, // 5.4.7 17956
            {Opcode.SMSG_MOVE_SET_TURN_RATE, 0x1DB0}, // 5.4.7 18019
            {Opcode.SMSG_MOVE_SET_WALK_SPEED, 0x00A0}, // 5.4.7 17956
            {Opcode.SMSG_MOVE_SPLINE_ROOT, 0x1EC3},
            {Opcode.SMSG_MOVE_SPLINE_SET_FLIGHT_SPEED, 0x1AD3},
            {Opcode.SMSG_MOVE_SPLINE_SET_RUN_BACK_SPEED, 0x09B8},
            {Opcode.SMSG_MOVE_SPLINE_SET_RUN_SPEED, 0x1A90},
            {Opcode.SMSG_MOVE_SPLINE_SET_SWIM_SPEED, 0x0254},
            {Opcode.SMSG_MOVE_SPLINE_SET_WALK_BACK_SPEED, 0x0155}, // SMSG_SPLINE_MOVE_SET_WALK_SPEED
            {Opcode.SMSG_MOVE_SPLINE_UNROOT, 0x1698}, // SMSG_SPLINE_MOVE_UNROOT
            {Opcode.SMSG_MOVE_TELEPORT, 0x00D5},
            {Opcode.SMSG_MOVE_UNROOT, 0x1D8A},
            {Opcode.SMSG_MOVE_UNSET_CAN_FLY, 0x1D81}, // 5.4.7 17956
            {Opcode.SMSG_MOVE_UNSET_HOVERING, 0x1AC0}, // 5.4.7 18019
            {Opcode.SMSG_MOVE_UPDATE, 0x1CB2},
            {Opcode.SMSG_MOVE_UPDATE_COLLISION_HEIGHT, 0x179A},
            {Opcode.SMSG_MOVE_UPDATE_FLIGHT_SPEED, 0x17D9},
            {Opcode.SMSG_MOVE_UPDATE_RUN_BACK_SPEED, 0x1BC2},
            {Opcode.SMSG_MOVE_UPDATE_RUN_SPEED, 0x1388},
            {Opcode.SMSG_MOVE_UPDATE_SWIM_SPEED, 0x1893},
            {Opcode.SMSG_MOVE_UPDATE_WALK_SPEED, 0x139A},
            {Opcode.SMSG_MOVE_WATER_WALK, 0x1290}, // 5.4.7 18019
            {Opcode.SMSG_NEW_TAXI_PATH, 0x13A3}, // 5.4.7 18019
            {Opcode.SMSG_NEW_WORLD, 0x05AB},
            {Opcode.SMSG_NOTIFICATION, 0x15A1}, // 5.4.7 18019
            {Opcode.SMSG_ON_CANCEL_EXPECTED_RIDE_VEHICLE_AURA, 0x0C0A}, // 5.4.7 18019
            {Opcode.SMSG_ON_MONSTER_MOVE, 0x12D8},
            {Opcode.SMSG_PARTY_COMMAND_RESULT, 0x1787}, // 5.4.7 18019
            {Opcode.SMSG_PARTY_KILL_LOG, 0x0F23},
            {Opcode.SMSG_PARTY_MEMBER_STATS, 0x0E61}, // 5.4.7 18019
            {Opcode.SMSG_PARTY_UPDATE, 0x1E61}, // 5.4.7 18019
            {Opcode.SMSG_PAUSE_MIRROR_TIMER, 0x1C7B}, // 5.4.7 18019
            {Opcode.SMSG_PENDING_RAID_LOCK, 0x1C52}, // 5.4.7 18019
            {Opcode.SMSG_PETITION_ALREADY_SIGNED, 0x1409}, // 5.4.7 18019
            {Opcode.SMSG_PETITION_DECLINED, 0x1E69}, // 5.4.7 18019
            {Opcode.SMSG_PETITION_QUERY_RESPONSE, 0x1732}, // 5.4.7 18019
            {Opcode.SMSG_PETITION_RENAME_RESPONSE, 0x01E0}, // 5.4.7 18019
            {Opcode.SMSG_PETITION_SHOW_LIST, 0x1C32}, // 5.4.7 18019
            {Opcode.SMSG_PETITION_SHOW_SIGNATURES, 0x0899}, // 5.4.7 18019
            {Opcode.SMSG_PET_ACTION_FEEDBACK, 0x14E8}, // 5.4.7 18019
            {Opcode.SMSG_PET_ACTION_SOUND, 0x15E1}, // 5.4.7 18019
            {Opcode.SMSG_PET_BATTLE_CHAT_RESTRICTED, 0x1F53},  // 5.4.7 17930 PET_BATTLE NYI SMSG_BATTLE_PET_CHAT_RESTRICTED
            {Opcode.SMSG_PET_BATTLE_DEBUG_QUEUE_DUMP_RESPONSE, 0x13E9},  // 5.4.7 17930 PET_BATTLE NYI SMSG_BATTLE_PET_DEBUG_QUEUE_DUMP_RESPONSE
            {Opcode.SMSG_PET_BATTLE_FINAL_ROUND, 0x12F0},  // 5.4.7 17930 PET_BATTLE NYI // DONE SMSG_BATTLE_PET_FINAL_ROUND
            {Opcode.SMSG_PET_BATTLE_FINISHED, 0x1E33},  // 5.4.7 17930 PET_BATTLE NYI SMSG_BATTLE_PET_FINISHED
            {Opcode.SMSG_PET_BATTLE_FIRST_ROUND, 0x1612},  // 5.4.7 17930 PET_BATTLE NYI //DONE SMSG_BATTLE_PET_FIRST_ROUND
            {Opcode.SMSG_PET_BATTLE_FULL_UPDATE, 0x01E3},  // 5.4.7 17930 PET_BATTLE NYI SMSG_BATTLE_PET_FULL_UPDATE
            {Opcode.SMSG_PET_BATTLE_MAX_GAME_LENGTH_WARNING, 0x12BB},  // 5.4.7 17930 PET_BATTLE NYI SMSG_BATTLE_PET_MAX_GAME_LENGTH_WARNING
            {Opcode.SMSG_PET_BATTLE_PVP_CHALLENGE, 0x1C08},  // 5.4.7 17930 PET_BATTLE NYI SMSG_BATTLE_PET_PVP_CHALLENGE
            {Opcode.SMSG_PET_BATTLE_QUEUE_PROPOSE_MATCH, 0x0E62},  // 5.4.7 17930 PET_BATTLE NYI SMSG_BATTLE_PET_QUEUE_PROPOSE_MATCH
            {Opcode.SMSG_PET_BATTLE_QUEUE_STATUS, 0x1540},  // 5.4.7 17930 PET_BATTLE NYI SMSG_BATTLE_PET_QUEUE_STATUS
            {Opcode.SMSG_PET_BATTLE_REPLACEMENTS_MADE, 0x0891},  // 5.4.0 17399 PET_BATTLE NYI SMSG_BATTLE_PET_REPLACEMENTS_MADE
            {Opcode.SMSG_PET_BATTLE_ROUND_RESULT, 0x0709},  // 5.4.7 17930 PET_BATTLE NYI SMSG_BATTLE_PET_ROUND_RESULT
            {Opcode.SMSG_PET_BATTLE_SLOT_UPDATES, 0x0421},  // 5.4.7 17930 PET_BATTLE NYI SMSG_BATTLE_PET_SLOT_UPDATE
            {Opcode.SMSG_PET_CAST_FAILED, 0x1D33},
            {Opcode.SMSG_PET_NAME_INVALID, 0x0442}, // 5.4.7 18019
            {Opcode.SMSG_PET_REMOVED_SPELL, 0x1761}, // 5.4.7 18019
            {Opcode.SMSG_PET_SLOT_UPDATED, 0x0C83}, // 5.4.7 18019 Not implemented - CHECK!
            //{Opcode.SMSG_PET_SPELLS, 0x0D79}, // 5.4.7 18019
            {Opcode.SMSG_PET_STABLE_LIST, 0x0F09}, // 5.4.7 18019
            {Opcode.SMSG_PET_TAME_FAILURE, 0x0722}, // 5.4.7 18019
            {Opcode.SMSG_PHASE_SHIFT_CHANGE, 0x1D52},
            {Opcode.SMSG_PLAYED_TIME, 0x1C69},
            {Opcode.SMSG_PLAYER_BOUND, 0x00E8},
            {Opcode.SMSG_PLAYER_DIFFICULTY_CHANGE, 0x1F1A}, // 5.4.7 18019 Not implemented - CHECK!
            {Opcode.SMSG_PLAYER_SAVE_GUILD_EMBLEM, 0x04E8}, // 5.4.7 18019
            {Opcode.SMSG_PLAYER_TABARD_VENDOR_ACTIVATE, 0x12B8}, // 5.4.7 18019
            {Opcode.SMSG_PLAYER_VEHICLE_DATA, 0x1F29},
            {Opcode.SMSG_PLAY_OBJECT_SOUND, 0x0C43}, // 5.4.7 18019
            {Opcode.SMSG_PLAY_SCENE, 0x1748},
            {Opcode.SMSG_PLAY_SOUND, 0x0E22},
            {Opcode.SMSG_PLAY_SPELL_VISUAL, 0x1F3B},
            {Opcode.SMSG_PLAY_SPELL_VISUAL_KIT, 0x0500},
            {Opcode.SMSG_PONG, 0x15B1},
            {Opcode.SMSG_POWER_UPDATE, 0x1441},
            {Opcode.SMSG_PRE_RESSURECT, 0x1F52}, // 5.4.7 18019
            {Opcode.SMSG_PVP_CREDIT, 0x13BB}, // 5.4.7 18019
            {Opcode.SMSG_PVP_LOG_DATA, 0x076A},
            {Opcode.SMSG_PVP_OPTIONS_ENABLED, 0x1460}, // 5.4.7 18019
            {Opcode.SMSG_PVP_SEASON, 0x00E1},
            {Opcode.SMSG_QUERY_BATTLE_PET_NAME_RESPONSE, 0x0D01}, // SMSG_BATTLE_PET_QUERY_NAME_RESPONSE
            {Opcode.SMSG_QUERY_CREATURE_RESPONSE, 0x00E0},
            {Opcode.SMSG_QUERY_GAME_OBJECT_RESPONSE, 0x066A},
            {Opcode.SMSG_QUERY_GUILD_INFO_RESPONSE, 0x1953},
            {Opcode.SMSG_QUERY_ITEM_TEXT_RESPONSE, 0x06AD}, // 5.4.7 18019
            {Opcode.SMSG_QUERY_NEXT_MAIL_TIME, 0x1C20}, // 5.4.7 18019
            {Opcode.SMSG_QUERY_NPC_TEXT_RESPONSE, 0x10E0},
            {Opcode.SMSG_QUERY_PAGE_TEXT_RESPONSE, 0x1653},
            {Opcode.SMSG_QUERY_PET_NAME_RESPONSE, 0x1F08},
            {Opcode.SMSG_QUERY_PLAYER_NAME_RESPONSE, 0x1E5B},
            {Opcode.SMSG_QUERY_QUEST_INFO_RESPONSE, 0x0F13},
            {Opcode.SMSG_QUERY_TIME_RESPONSE, 0x0E2A},
            {Opcode.SMSG_QUEST_COMPLETION_NPC_RESPONSE, 0x0957}, // SMSG_QUEST_NPC_QUERY_RESPONSE
            {Opcode.SMSG_QUEST_CONFIRM_ACCEPT, 0x0351}, // 5.4.7 18019
            {Opcode.SMSG_QUEST_GIVER_INVALID_QUEST, 0x1944}, // 5.4.7 18019 //0F4D
            {Opcode.SMSG_QUEST_GIVER_OFFER_REWARD_MESSAGE, 0x0F77},
            {Opcode.SMSG_QUEST_GIVER_QUEST_COMPLETE, 0x0D54},
            {Opcode.SMSG_QUEST_GIVER_QUEST_DETAILS, 0x0966},
            {Opcode.SMSG_QUEST_GIVER_QUEST_FAILED, 0x0B70}, // 5.4.7 18019
            {Opcode.SMSG_QUEST_GIVER_QUEST_LIST_MESSAGE, 0x0733}, // 5.4.7 18019
            {Opcode.SMSG_QUEST_GIVER_REQUEST_ITEMS, 0x0A32},
            {Opcode.SMSG_QUEST_GIVER_STATUS, 0x0D7E},
            {Opcode.SMSG_QUEST_GIVER_STATUS_MULTIPLE, 0x0F79},
            {Opcode.SMSG_QUEST_LOG_FULL, 0x0B19}, // 5.4.7 18019
            {Opcode.SMSG_QUEST_POI_QUERY_RESPONSE, 0x0F5F},
            {Opcode.SMSG_QUEST_PUSH_RESULT, 0x0D66}, // 5.4.7 18019
            {Opcode.SMSG_QUEST_UPDATE_ADD_CREDIT, 0x0B1A}, // 5.4.7 18019
            {Opcode.SMSG_QUEST_UPDATE_ADD_CREDIT_SIMPLE, 0x0232}, // 5.4.7 18019 Not implemented - CHECK!
            {Opcode.SMSG_QUEST_UPDATE_ADD_PVP_CREDIT, 0x0F4D}, // 5.4.7 18019
            {Opcode.SMSG_QUEST_UPDATE_COMPLETE, 0x071B}, // 5.4.7 18019
            {Opcode.SMSG_QUEST_UPDATE_FAILED, 0x0271}, // 5.4.7 18019
            {Opcode.SMSG_QUEST_UPDATE_FAILED_TIMER, 0x0F7F}, // 5.4.7 18019
            {Opcode.SMSG_RAID_DIFFICULTY_SET, 0x16A6}, // 5.4.7 18019
            {Opcode.SMSG_RAID_INSTANCE_INFO, 0x0C21}, // 5.4.7 18019
            {Opcode.SMSG_RAID_INSTANCE_MESSAGE, 0x14E1}, // 5.4.7 18019
            {Opcode.SMSG_RAID_MARKERS_CHANGED, 0x14E0}, // 5.4.7 18019
            {Opcode.SMSG_RANDOM_ROLL, 0x0529}, // 5.4.7 18019
            {Opcode.SMSG_READY_CHECK_COMPLETED, 0x177A}, // 5.4.7 18019
            {Opcode.SMSG_READY_CHECK_RESPONSE, 0x1641},
            {Opcode.SMSG_READY_CHECK_STARTED, 0x072A},
            {Opcode.SMSG_READ_ITEM_RESULT_OK, 0x022D}, // 5.4.7 18019
            {Opcode.SMSG_REALM_QUERY_RESPONSE, 0x1652},
            {Opcode.SMSG_REALM_SPLIT, 0x145A}, // 0x0708, // 5.4.7 18019 - CHECK!
            {Opcode.SMSG_RECEIVED_MAIL, 0x116E}, // 0x0D60, // 5.4.7 18019 - CHECK!
            {Opcode.SMSG_REFORGE_RESULT, 0x1601},
            {Opcode.SMSG_REQUEST_CEMETERY_LIST_RESPONSE, 0x1C49}, // 5.4.7 18019
            {Opcode.SMSG_REQUEST_PVP_REWARDS_RESPONSE, 0x042B}, // 5.4.7 18019
            {Opcode.SMSG_RESEARCH_COMPLETE, 0x0F6B}, // 5.4.7 18019
            {Opcode.SMSG_RESPEC_WIPE_CONFIRM, 0x0E40}, // 5.4.7 18019
            {Opcode.SMSG_RESPOND_INSPECT_ACHIEVEMENTS, 0x04E1},
            {Opcode.SMSG_RESUME_CAST_BAR, 0x0850},
            {Opcode.SMSG_RESUME_COMMS, 0x01B9},
            {Opcode.SMSG_RESUME_TOKEN, 0x12FB}, // 5.4.7 18019
            {Opcode.SMSG_RESURRECT_REQUEST, 0x1668}, // 5.4.7 18019
            {Opcode.SMSG_RESYNC_RUNES, 0x0561}, // 5.4.7 18019
            {Opcode.SMSG_ROLE_CHANGED_INFORM, 0x0890}, // 5.4.7 18019
            {Opcode.SMSG_ROLE_POLL_INFORM, 0x1F61}, // 5.4.7 18019
            {Opcode.SMSG_SCENARIO_POIS, 0x10E9},
            {Opcode.SMSG_SCENARIO_PROGRESS_UPDATE, 0x167B},
            {Opcode.SMSG_SCENARIO_STATE, 0x1E20},
            {Opcode.SMSG_SELL_ITEM, 0x1468}, // 5.4.7 18019
            {Opcode.SMSG_SEND_KNOWN_SPELLS, 0x1B05},
            {Opcode.SMSG_SEND_RAID_TARGET_UPDATE_ALL, 0x1620}, // 5.4.7 18019
            {Opcode.SMSG_SEND_RAID_TARGET_UPDATE_SINGLE, 0x1F3A}, // 5.4.7 18019
            {Opcode.SMSG_SEND_UNLEARN_SPELLS, 0x1B3E},
            {Opcode.SMSG_SERVER_TIME, 0x047E}, // 5.4.7 18019 - CHECK!
            {Opcode.SMSG_SETUP_CURRENCY, 0x1E3A},
            {Opcode.SMSG_SETUP_RESEARCH_HISTORY, 0x054A}, // 5.4.7 18019
            {Opcode.SMSG_SET_DUNGEON_DIFFICULTY, 0x1500}, // 5.4.7 18019
            {Opcode.SMSG_SET_FACTION_STANDING, 0x0E6B}, // 5.4.7 18019
            {Opcode.SMSG_SET_FLAT_SPELL_MODIFIER, 0x0179},
            {Opcode.SMSG_SET_FORCED_REACTIONS, 0x060A}, // 5.4.7 18019
            {Opcode.SMSG_SET_PCT_SPELL_MODIFIER, 0x193C},
            {Opcode.SMSG_SET_PET_SPECIALIZATION, 0x1640}, // 5.4.7 18019
            {Opcode.SMSG_SET_PLAYER_DECLINED_NAMES_RESULT, 0x00E9}, // 5.4.7 18019
            {Opcode.SMSG_SET_PLAY_HOVER_ANIM, 0x0729},
            {Opcode.SMSG_SET_PROFICIENCY, 0x1E3B},
            {Opcode.SMSG_SET_TIME_ZONE_INFORMATION, 0x0C2B},
            {Opcode.SMSG_SHOW_BANK, 0x060B},
            {Opcode.SMSG_SHOW_NEUTRAL_PLAYER_FACTION_SELECT_UI, 0x0C98}, // 5.4.7 18019
            {Opcode.SMSG_SHOW_TAXI_NODES, 0x14A3}, // 5.4.7 18019
            {Opcode.SMSG_SOCKET_GEMS, 0x13BA}, // 5.4.7 18019
            {Opcode.SMSG_SPELL_CHANNEL_START, 0x1B15},
            {Opcode.SMSG_SPELL_CHANNEL_UPDATE, 0x087B}, // 5.4.7 18019
            {Opcode.SMSG_SPELL_COOLDOWN, 0x1B14},
            {Opcode.SMSG_SPELL_DELAYED, 0x015B}, // 5.4.7 18019
            {Opcode.SMSG_SPELL_DISPELL_LOG , 0x0919},
            {Opcode.SMSG_SPELL_ENERGIZE_LOG, 0x0071},
            {Opcode.SMSG_SPELL_EXECUTE_LOG, 0x19B4},
            {Opcode.SMSG_SPELL_FAILED_OTHER, 0x1E7A},
            {Opcode.SMSG_SPELL_FAILURE, 0x0E03},
            {Opcode.SMSG_SPELL_GO, 0x0851},
            {Opcode.SMSG_SPELL_HEAL_LOG, 0x1BBF},
            {Opcode.SMSG_SPELL_INSTAKILL_LOG, 0x0D7A},
            {Opcode.SMSG_SPELL_INTERRUPT_LOG, 0x091A},
            {Opcode.SMSG_SPELL_NON_MELEE_DAMAGE_LOG, 0x0172},
            {Opcode.SMSG_SPELL_OR_DAMAGE_IMMUNE, 0x1B9F}, // 5.4.7 18019
            {Opcode.SMSG_SPELL_PERIODIC_AURA_LOG, 0x051B},
            {Opcode.SMSG_SPELL_START, 0x0130},
            {Opcode.SMSG_SPELL_UPDATE_CHAIN_TARGETS, 0x1B96},
            {Opcode.SMSG_SPIRIT_HEALER_CONFIRM, 0x171B}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_ROOT, 0x1EC3}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_SET_FEATHER_FALL, 0x1032}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_SET_FLIGHT_BACK_SPEED, 0x01AA}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_SET_FLIGHT_SPEED, 0x1AD3}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_SET_FLYING, 0x1A83}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_SET_HOVER, 0x1C10}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_SET_LAND_WALK, 0x17C1}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_SET_NORMAL_FALL, 0x0998}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_SET_PITCH_RATE, 0x0175}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_SET_RUN_BACK_SPEED, 0x09B8}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_SET_RUN_MODE, 0x1CB9}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_SET_RUN_SPEED, 0x1A90}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_SET_SWIM_BACK_SPEED, 0x1393}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_SET_SWIM_SPEED, 0x0254}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_SET_TURN_RATE, 0x0DAB}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_SET_WALK_MODE, 0x01FE}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_SET_WATER_WALK, 0x0DB3}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_UNSET_FLYING, 0x1798}, // 5.4.7 18019
            {Opcode.SMSG_SPLINE_MOVE_UNSET_HOVER, 0x0076}, // 5.4.7 18019
            {Opcode.SMSG_STABLE_RESULT, 0x0E0B}, // 5.4.7 18019
            {Opcode.SMSG_STAND_STATE_UPDATE, 0x0C48},
            {Opcode.SMSG_START_ELAPSED_TIMER, 0x0760}, // 5.4.7 18019 Not implemented - CHECK!
            {Opcode.SMSG_START_MIRROR_TIMER, 0x0D48}, // 5.4.7 18019
            {Opcode.SMSG_START_TIMER, 0x12B3}, // 5.4.7 18019
            {Opcode.SMSG_STOP_MIRROR_TIMER, 0x1E01}, // 5.4.7 18019
            {Opcode.SMSG_SUMMON_CANCEL, 0x12FA}, // 5.4.7 18019 Not implemented - CHECK!
            {Opcode.SMSG_SUMMON_REQUEST, 0x0D29}, // 5.4.7 18019
            {Opcode.SMSG_SUPERCEDED_SPELLS, 0x052A}, // 5.4.7 18019
            {Opcode.SMSG_SUSPEND_COMMS, 0x10B0},
            {Opcode.SMSG_TAXI_NODE_STATUS, 0x0E23}, // 5.4.7 18019
            {Opcode.SMSG_TEXT_EMOTE, 0x0522},
            {Opcode.SMSG_THREAT_CLEAR, 0x1D7A},
            {Opcode.SMSG_THREAT_REMOVE, 0x1E29},
            {Opcode.SMSG_THREAT_UPDATE, 0x1708},
            {Opcode.SMSG_TIME_SYNC_REQUEST, 0x12F1},
            {Opcode.SMSG_TITLE_EARNED, 0x0F28}, // 5.4.7 18019
            {Opcode.SMSG_TITLE_LOST, 0x13E0}, // 5.4.7 18019
            {Opcode.SMSG_TOTEM_CREATED, 0x0608},
            {Opcode.SMSG_TRADE_STATUS, 0x05E8}, // 5.4.7 18019
            {Opcode.SMSG_TRADE_STATUS_EXTENDED, 0x15EA}, // 5.4.7 18019
            {Opcode.SMSG_TRADE_UPDATED, 0x14E9}, // 5.4.7 18019
            {Opcode.SMSG_TRAINER_LIST, 0x1509},
            //{Opcode.SMSG_TRAINER_SERVICE, 0x157B}, // 5.4.7 18019
            {Opcode.SMSG_TRANSFER_ABORTED, 0x0648},
            {Opcode.SMSG_TRANSFER_PENDING, 0x0440},
            {Opcode.SMSG_TRIGGER_CINEMATIC, 0x04CC},
            {Opcode.SMSG_TRIGGER_MOVIE, 0x0D0A}, // 5.4.7 18019
            {Opcode.SMSG_TURN_IN_PETITION_RESULT, 0x0509}, // 5.4.7 18019
            {Opcode.SMSG_TUTORIAL_FLAGS, 0x10A7},
            {Opcode.SMSG_UI_TIME, 0x0C22},
            {Opcode.SMSG_UNLEARNED_SPELLS, 0x05E3},
            {Opcode.SMSG_UPDATE_ACCOUNT_DATA, 0x13B9}, // 5.4.7 18019
            {Opcode.SMSG_UPDATE_ACTION_BUTTONS, 0x1768},
            {Opcode.SMSG_UPDATE_COMBO_POINTS, 0x0C88}, // 5.4.7 18019
            {Opcode.SMSG_UPDATE_CURRENCY, 0x173B}, // 5.4.7 18019
            {Opcode.SMSG_UPDATE_CURRENCY_WEEK_LIMIT, 0x0F42}, // 5.4.7 18019
            {Opcode.SMSG_UPDATE_INSTANCE_ENCOUNTER_UNIT, 0x16E5}, // 5.4.7 18019
            {Opcode.SMSG_UPDATE_INSTANCE_OWNERSHIP, 0x0F68}, // 5.4.7 18019
            {Opcode.SMSG_UPDATE_LAST_INSTANCE, 0x0600}, // 5.4.7 18019
            {Opcode.SMSG_UPDATE_OBJECT, 0x1725},
            {Opcode.SMSG_UPDATE_TALENT_DATA, 0x0C68},
            {Opcode.SMSG_UPDATE_WORLD_STATE, 0x1D13},
            {Opcode.SMSG_USE_EQUIPMENT_SET_RESULT, 0x12AA}, // SMSG_DUMP_OBJECTS_DATA
            {Opcode.SMSG_VENDOR_INVENTORY, 0x0D2A},
            {Opcode.SMSG_VIGNETTE_UPDATE, 0x05A1}, // SMSG_SET_VIGNETTE
            {Opcode.SMSG_VOID_ITEM_SWAP_RESPONSE, 0x0E2B},
            {Opcode.SMSG_VOID_STORAGE_CONTENTS, 0x0C93},
            {Opcode.SMSG_VOID_STORAGE_FAILED, 0x1569},
            {Opcode.SMSG_VOID_STORAGE_TRANSFER_CHANGES, 0x05A8},
            {Opcode.SMSG_VOID_TRANSFER_RESULT, 0x00E3},
            {Opcode.SMSG_WARDEN_DATA, 0x14EB},
            {Opcode.SMSG_WEATHER, 0x0F41},
            {Opcode.SMSG_WEEKLY_RESET_CURRENCY, 0x0620}, // 5.4.7 18019 - CHECK!
            {Opcode.SMSG_WEEKLY_SPELL_USAGE, 0x1D04},
            {Opcode.SMSG_WHO, 0x0460},
            {Opcode.SMSG_WHO_IS, 0x1513}, // 5.4.7 18019
            {Opcode.SMSG_WORLD_SERVER_INFO, 0x1D01},
            {Opcode.SMSG_XP_GAIN_ABORTED, 0x1E32},
            {Opcode.SMSG_ZONE_UNDER_ATTACK, 0x1400} // 5.4.7 18019
        };

        private static readonly BiDictionary<Opcode, int> MiscOpcodes = new BiDictionary<Opcode, int>
        {
            {Opcode.MSG_CHANNEL_UPDATE, 0x087B},
            {Opcode.MSG_MOVE_FALL_LAND, 0x055B},
            {Opcode.MSG_MOVE_HEARTBEAT, 0x017B},
            {Opcode.MSG_MOVE_JUMP, 0x0438},
            {Opcode.MSG_MOVE_SET_FACING, 0x005A}, // 5.4.7 17956
            {Opcode.MSG_MOVE_SET_PITCH, 0x047A}, // 5.4.7 17956
            {Opcode.MSG_MOVE_SET_RUN_MODE, 0x0878}, // 5.4.7 17956
            {Opcode.MSG_MOVE_SET_WALK_MODE, 0x0138}, // 5.4.7 17956
            {Opcode.MSG_MOVE_START_ASCEND, 0x0430}, // 5.4.7 17956
            {Opcode.MSG_MOVE_START_BACKWARD, 0x0459}, // 5.4.7 17956
            {Opcode.MSG_MOVE_START_DESCEND, 0x0132}, // 5.4.7 17956
            {Opcode.MSG_MOVE_START_FORWARD, 0x041B}, // 5.4.7 17956
            {Opcode.MSG_MOVE_START_PITCH_DOWN, 0x093B}, // 5.4.7 17956
            {Opcode.MSG_MOVE_START_PITCH_UP, 0x0079}, // 5.4.7 17956
            {Opcode.MSG_MOVE_START_STRAFE_LEFT, 0x0873}, // 5.4.7 17956
            {Opcode.MSG_MOVE_START_STRAFE_RIGHT, 0x0C12}, // 5.4.7 17956
            {Opcode.MSG_MOVE_START_SWIM, 0x0871}, // 5.4.7 17956
            {Opcode.MSG_MOVE_START_TURN_LEFT, 0x011B}, // 5.4.7 17956
            {Opcode.MSG_MOVE_START_TURN_RIGHT, 0x0411}, // 5.4.7 17956
            {Opcode.MSG_MOVE_STOP, 0x0570}, // 5.4.7 17956
            {Opcode.MSG_MOVE_STOP_ASCEND, 0x0012}, // 5.4.7 17956
            //{Opcode.MSG_MOVE_STOP_PITCH, 0x0071}, // 5.4.7 17956
            {Opcode.MSG_MOVE_STOP_STRAFE, 0x0171}, // 5.4.7 17956
            {Opcode.MSG_MOVE_STOP_SWIM, 0x0578}, // 5.4.7 17956
            {Opcode.MSG_MOVE_STOP_TURN, 0x0530}, // 5.4.7 17956
            {Opcode.MSG_MOVE_TELEPORT_ACK, 0x0978}, // 5.4.7 17956
            {Opcode.MSG_MOVE_WORLDPORT_ACK, 0x18BB},
            {Opcode.MSG_VERIFY_CONNECTIVITY, 0x4F57},
        };
        /* Note:
            0x1231, 0x12FB - CMSG / SMSG - Count opcode
            0x0F43         - SMSG        - Spell opcode
            */
    }
}
