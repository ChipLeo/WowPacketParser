using System;
using WowPacketParser.Misc;

namespace WowPacketParser.Enums.Version.V6_1_2_19728
{
    public static class Opcodes_6_1_2
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
            }
            return MiscOpcodes;
        }

        private static readonly BiDictionary<Opcode, int> ClientOpcodes = new BiDictionary<Opcode, int>
        {
            {Opcode.CMSG_ATTACKSTOP, 0x0A01},
            {Opcode.CMSG_ATTACKSWING, 0x0BF4},
            {Opcode.CMSG_AUTH_CONTINUED_SESSION, 0x0485},
            {Opcode.CMSG_AUTH_SESSION, 0x03DD},
            {Opcode.CMSG_BANKER_ACTIVATE, 0x0CA5},
            {Opcode.CMSG_BATTLEFIELD_JOIN, 0x1D36},
            {Opcode.CMSG_BATTLEFIELD_JOIN_RATED, 0x01AA},             // Unconfirmed
            {Opcode.CMSG_BATTLEFIELD_LEAVE, 0x0272},                  // Unconfirmed
            {Opcode.CMSG_BATTLEFIELD_PORT, 0x1D32},                   // Unconfirmed
            {Opcode.CMSG_BUY_BANK_SLOT, 0x1DE2},
            {Opcode.CMSG_BUY_ITEM, 0x1CE5},
            {Opcode.CMSG_CANCEL_AURA, 0x12FB},
            {Opcode.CMSG_CANCEL_CAST, 0x058A},
            {Opcode.CMSG_CAST_SPELL, 0x1274},
            {Opcode.CMSG_CHANGE_SUB_GROUP, 0x0AB7},                   // Unconfirmed
            {Opcode.CMSG_CHAR_CREATE, 0x1636},
            {Opcode.CMSG_CHAR_DELETE, 0x12B8},
            {Opcode.CMSG_CHAR_ENUM, 0x1696},
            {Opcode.CMSG_CONVERT_RAID, 0x0A98},                       // Unconfirmed
            {Opcode.CMSG_DEPOSIT_REAGENT_BANK, 0x002A},
            {Opcode.CMSG_DO_READY_CHECK, 0x139E},                     // Unconfirmed
            {Opcode.CMSG_DUEL_PROPOSED, 0x0A38},
            {Opcode.CMSG_DUEL_RESPONSE, 0x0A5B},                      // Unconfirmed
            {Opcode.CMSG_EMOTE, 0x01AD},
            {Opcode.CMSG_GUILD_BANK_DEPOSIT_MONEY, 0x0832},           // Unconfirmed
            {Opcode.CMSG_GUILD_DECLINE_INVITATION, 0x1967},           // Unconfirmed
            {Opcode.CMSG_JOIN_ARENA, 0x0865},                         // Unconfirmed
            {Opcode.CMSG_JOIN_ARENA_SKIRMISH, 0x1E01},
            {Opcode.CMSG_JOIN_CHANNEL, 0x152A},
            {Opcode.CMSG_LEARN_TALENTS, 0x0AAA},
            {Opcode.CMSG_LEAVE_CHANNEL, 0x113D},
            {Opcode.CMSG_LEAVE_GROUP, 0x179E},                        // Unconfirmed
            {Opcode.CMSG_LF_GUILD_BROWSE, 0x1A37},
            {Opcode.CMSG_LF_GUILD_DECLINE_RECRUIT, 0x1023},           // Unconfirmed
            {Opcode.CMSG_LFG_SET_COMMENT, 0x0615},                    // Unconfirmed
            {Opcode.CMSG_LIST_INVENTORY, 0x1922},
            {Opcode.CMSG_LOAD_SCREEN, 0x13C0},
            {Opcode.CMSG_LOGOUT_CANCEL, 0x0F8C},
            {Opcode.CMSG_LOGOUT_REQUEST, 0x0CA6},
            {Opcode.CMSG_LOOT_METHOD, 0x0E3E},                        // Unconfirmed
            {Opcode.CMSG_MESSAGECHAT_AFK, 0x185E},
            {Opcode.CMSG_MESSAGECHAT_CHANNEL, 0x1D8A},
            {Opcode.CMSG_MESSAGECHAT_DND, 0x183E},
            {Opcode.CMSG_MESSAGECHAT_EMOTE, 0x1DAA},
            {Opcode.CMSG_MESSAGECHAT_GUILD, 0x14E9},
            {Opcode.CMSG_MESSAGECHAT_OFFICER, 0x155A},
            {Opcode.CMSG_MESSAGECHAT_SAY, 0x192A},
            {Opcode.CMSG_MESSAGECHAT_WHISPER, 0x103A},
            {Opcode.CMSG_MESSAGECHAT_YELL, 0x1CB9},
            {Opcode.CMSG_MOVE_FALL_LAND, 0x095F},
            {Opcode.CMSG_MOVE_HEARTBEAT, 0x055C},
            {Opcode.CMSG_MOVE_JUMP, 0x0158},
            {Opcode.CMSG_MOVE_SET_FACING, 0x0803},
            {Opcode.CMSG_MOVE_SET_PITCH, 0x080F},
            {Opcode.CMSG_MOVE_START_ASCEND, 0x0510},
            {Opcode.CMSG_MOVE_START_BACKWARD, 0x0147},
            {Opcode.CMSG_MOVE_START_DESCEND, 0x0117},
            {Opcode.CMSG_MOVE_START_FORWARD, 0x0004},
            {Opcode.CMSG_MOVE_START_STRAFE_LEFT, 0x0844},
            {Opcode.CMSG_MOVE_START_STRAFE_RIGHT, 0x0957},
            {Opcode.CMSG_MOVE_STOP, 0x044B},
            {Opcode.CMSG_MOVE_STOP_ASCEND, 0x011C},
            {Opcode.CMSG_MOVE_STOP_STRAFE, 0x084B},
            {Opcode.CMSG_NAME_QUERY, 0x0BBD},
            {Opcode.CMSG_OPT_OUT_OF_LOOT, 0x1F89},                    // Unconfirmed
            {Opcode.CMSG_PARTY_INVITE, 0x12BD},
            {Opcode.CMSG_PARTY_INVITE_RESPONSE, 0x16BF},              // Unconfirmed
            {Opcode.CMSG_PARTY_UNINVITE, 0x02B6},                     // Unconfirmed
            {Opcode.CMSG_PET_ABANDON, 0x01CA},                        // Unconfirmed
            {Opcode.CMSG_PET_BATTLE_QUEUE_PROPOSE_MATCH_RESULT, 0x1ACF}, // Unconfirmed
            {Opcode.CMSG_PET_BATTLE_REQUEST_PVP, 0x16C8},             // Unconfirmed
            {Opcode.CMSG_PET_RENAME, 0x1618},                         // Unconfirmed
            {Opcode.CMSG_PET_SPELL_AUTOCAST, 0x0C75},                 // Unconfirmed
            {Opcode.CMSG_PET_STOP_ATTACK, 0x09A6},                    // Unconfirmed
            {Opcode.CMSG_PING, 0x12DE},
            {Opcode.CMSG_PLAYER_LOGIN, 0x0E98},
            {Opcode.CMSG_PORT_GRAVEYARD, 0x0C65},                     // Unconfirmed
            {Opcode.CMSG_READY_CHECK_RESPONSE, 0x07B5},               // Unconfirmed
            {Opcode.CMSG_REAGENT_BANK_BUY_TAB, 0x1D75},
            {Opcode.CMSG_REQUEST_ACCOUNT_DATA, 0x0798},
            {Opcode.CMSG_REQUEST_PVP_OPTIONS_ENABLED, 0x029E},        // Unconfirmed
            {Opcode.CMSG_REQUEST_PVP_REWARDS, 0x06DC},                // Unconfirmed
            {Opcode.CMSG_REQUEST_RAID_INFO, 0x0A96},                  // Unconfirmed
            {Opcode.CMSG_SET_ACHIEVEMENTS_HIDDEN, 0x16D0},
            {Opcode.CMSG_SET_ASSISTANT_LEADER, 0x0395},               // Unconfirmed
            {Opcode.CMSG_SET_EVERYONE_IS_ASSISTANT, 0x1716},          // Unconfirmed
            {Opcode.CMSG_SET_FACTION_INACTIVE, 0x1862},
            {Opcode.CMSG_SET_PARTY_LEADER, 0x131D},                   // Unconfirmed
            {Opcode.CMSG_SET_SELECTION, 0x0E8C},
            {Opcode.CMSG_SET_SPECIALIZATION, 0x0759},
            {Opcode.CMSG_SET_WATCHED_FACTION, 0x1E82},
            {Opcode.CMSG_SHOWING_CLOAK, 0x0F04},
            {Opcode.CMSG_SHOWING_HELM, 0x0C36},
            {Opcode.CMSG_SORT_BAGS, 0x0AF1},
            {Opcode.CMSG_SORT_REAGENT_BANK_BAGS, 0x06D2},
            {Opcode.CMSG_START_SPECTATOR_WAR_GAME, 0x16B5},           // Unconfirmed
            {Opcode.CMSG_START_WARGAME, 0x012BF},                     // Unconfirmed
            {Opcode.CMSG_SWAP_INV_ITEM, 0x003C},
            {Opcode.CMSG_SWAP_ITEM, 0x0438},
            {Opcode.CMSG_SWAP_SUB_GROUPS, 0x0F98},                    // Unconfirmed
            {Opcode.CMSG_TRANSMOGRIFY_ITEMS, 0x03F1},
            {Opcode.CMSG_UI_TIME_REQUEST, 0x0550},
            {Opcode.CMSG_UNLEARN_SPECIALIZATION, 0x0708},             // Unconfirmed
            {Opcode.CMSG_UNLEARN_TALENTS, 0x0FA9},
            {Opcode.CMSG_UPDATE_ACCOUNT_DATA, 0x1637},
            {Opcode.CMSG_USE_ITEM, 0x06D0},
            {Opcode.CMSG_VIOLENCE_LEVEL, 0x0F48},
            {Opcode.CMSG_VOID_STORAGE_TRANSFER, 0x0E07},
            {Opcode.CMSG_VOID_STORAGE_UNLOCK, 0x0AA1},
            {Opcode.CMSG_WARGAME_RESPONSE, 0x0E3F},                   // Unconfirmed
            {Opcode.CMSG_REQUEST_RATED_INFO, 0x0A40},                 // Unconfirmed
        };

        private static readonly BiDictionary<Opcode, int> ServerOpcodes = new BiDictionary<Opcode, int>
        {
            {Opcode.SMSG_ACCOUNT_DATA_TIMES, 0x16B8},
            {Opcode.SMSG_ADDON_INFO, 0x1715},
            {Opcode.SMSG_ATTACKERSTATEUPDATE, 0x0EBD},
            {Opcode.SMSG_ATTACKSTART, 0x1971},
            {Opcode.SMSG_AUTH_CHALLENGE, 0x007E},
            {Opcode.SMSG_AUTH_RESPONSE, 0x18F6},
            {Opcode.SMSG_CHANNEL_NOTIFY, 0x15EF},
            {Opcode.SMSG_CHANNEL_NOTIFY_JOINED, 0x14C3},
            {Opcode.SMSG_CHAR_CREATE, 0x16BA},
            {Opcode.SMSG_CHAR_DELETE, 0x06B8},
            {Opcode.SMSG_CHAR_ENUM, 0x18F1},
            {Opcode.SMSG_CLIENTCACHE_VERSION, 0x0E09},
            {Opcode.SMSG_FEATURE_SYSTEM_STATUS, 0x0B3E},
            {Opcode.SMSG_INITIAL_SETUP, 0x0238},
            {Opcode.SMSG_MESSAGECHAT, 0x11E7},
            {Opcode.SMSG_MOVE_UPDATE, 0x0F2C},
            {Opcode.SMSG_NAME_QUERY_RESPONSE, 0x0C71},
            {Opcode.SMSG_ON_MONSTER_MOVE, 0x0EA9},
            {Opcode.SMSG_PONG, 0x011D},
            {Opcode.SMSG_REDIRECT_CLIENT, 0x0119},
            {Opcode.SMSG_SET_TIME_ZONE_INFORMATION, 0x073A},
            {Opcode.SMSG_SPELL_GO, 0x1CB9},
            {Opcode.SMSG_SPELL_NON_MELEE_DAMAGE_LOG, 0x14E9},
            {Opcode.SMSG_SPELL_START, 0x14BA},
            {Opcode.SMSG_TUTORIAL_FLAGS, 0x0E82},
            {Opcode.SMSG_UPDATE_ACCOUNT_DATA, 0x1698},
            {Opcode.SMSG_UPDATE_OBJECT, 0x1CB2},
        };

        private static readonly BiDictionary<Opcode, int> MiscOpcodes = new BiDictionary<Opcode, int>();
    }
}
