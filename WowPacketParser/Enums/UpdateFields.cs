﻿using WowPacketParser.Parsing;

namespace WowPacketParser.Enums
{
    // ReSharper disable InconsistentNaming, UnusedMember.Global
    public enum ObjectField
    {
        [UpdateField(UpdateFieldType.Guid)]
        OBJECT_FIELD_GUID,
        [UpdateField(UpdateFieldType.Uint)]
        OBJECT_FIELD_TYPE,
        [UpdateField(UpdateFieldType.Uint)]
        OBJECT_FIELD_ENTRY,
        [UpdateField(UpdateFieldType.Float)]
        OBJECT_FIELD_SCALE_X,
        OBJECT_FIELD_PADDING,
        OBJECT_DYNAMIC_FLAGS,
        OBJECT_END,
        OBJECT_FIELD_DATA
    }

    public enum ObjectDynamicField
    {
        OBJECT_DYNAMIC_END
    }

    public enum ItemField
    {
        ITEM_END,
        ITEM_FIELD_APPEARANCE_MOD_ID,
        ITEM_FIELD_ARTIFACT_XP,
        ITEM_FIELD_CONTAINED,
        ITEM_FIELD_CONTEXT,
        ITEM_FIELD_CREATE_PLAYED_TIME,
        [UpdateField(UpdateFieldType.Guid)]
        ITEM_FIELD_CREATOR,
        ITEM_FIELD_DURABILITY,
        ITEM_FIELD_DURATION,
        ITEM_FIELD_ENCHANTMENT,
        ITEM_FIELD_ENCHANTMENT_10_1,
        ITEM_FIELD_ENCHANTMENT_10_3,
        ITEM_FIELD_ENCHANTMENT_11_1,
        ITEM_FIELD_ENCHANTMENT_11_3,
        ITEM_FIELD_ENCHANTMENT_12_1,
        ITEM_FIELD_ENCHANTMENT_12_3,
        ITEM_FIELD_ENCHANTMENT_13_1,
        ITEM_FIELD_ENCHANTMENT_13_3,
        ITEM_FIELD_ENCHANTMENT_14_1,
        ITEM_FIELD_ENCHANTMENT_14_3,
        ITEM_FIELD_ENCHANTMENT_15_1,
        ITEM_FIELD_ENCHANTMENT_15_3,
        ITEM_FIELD_ENCHANTMENT_1_1,
        ITEM_FIELD_ENCHANTMENT_1_3,
        ITEM_FIELD_ENCHANTMENT_2_1,
        ITEM_FIELD_ENCHANTMENT_2_3,
        ITEM_FIELD_ENCHANTMENT_3_1,
        ITEM_FIELD_ENCHANTMENT_3_3,
        ITEM_FIELD_ENCHANTMENT_4_1,
        ITEM_FIELD_ENCHANTMENT_4_3,
        ITEM_FIELD_ENCHANTMENT_5_1,
        ITEM_FIELD_ENCHANTMENT_5_3,
        ITEM_FIELD_ENCHANTMENT_6_1,
        ITEM_FIELD_ENCHANTMENT_6_3,
        ITEM_FIELD_ENCHANTMENT_7_1,
        ITEM_FIELD_ENCHANTMENT_7_3,
        ITEM_FIELD_ENCHANTMENT_8_1,
        ITEM_FIELD_ENCHANTMENT_8_3,
        ITEM_FIELD_ENCHANTMENT_9_1,
        ITEM_FIELD_ENCHANTMENT_9_3,
        ITEM_FIELD_FLAGS,
        [UpdateField(UpdateFieldType.Guid)]
        ITEM_FIELD_GIFTCREATOR,
        ITEM_FIELD_ITEM_TEXT_ID,
        ITEM_FIELD_MAXDURABILITY,
        ITEM_FIELD_MODIFIERS_MASK,
        [UpdateField(UpdateFieldType.Guid)]
        ITEM_FIELD_OWNER,
        ITEM_FIELD_PAD,
        ITEM_FIELD_PROPERTY_SEED,
        ITEM_FIELD_RANDOM_PROPERTIES_ID,
        ITEM_FIELD_SPELL_CHARGES,
        ITEM_FIELD_STACK_COUNT
    }

    public enum ItemDynamicField
    {
        ITEM_DYNAMIC_FIELD_MODIFIERS,
        ITEM_DYNAMIC_FIELD_BONUSLIST_IDS,
        ITEM_DYNAMIC_FIELD_ARTIFACT_POWERS,
        ITEM_DYNAMIC_FIELD_GEMS,
        ITEM_DYNAMIC_FIELD_RELIC_TALENT_DATA,
        ITEM_DYNAMIC_END
    }

    public enum ContainerField
    {
        CONTAINER_ALIGN_PAD,
        CONTAINER_END,
        CONTAINER_FIELD_NUM_SLOTS,
        [UpdateField(UpdateFieldType.Guid)]
        CONTAINER_FIELD_SLOT_1
    }

    public enum ContainerDynamicField
    {
        CONTAINER_DYNAMIC_END
    }

    public enum AzeriteEmpoweredItemField
    {
        AZERITE_EMPOWERED_ITEM_FIELD_SELECTIONS,
        AZERITE_EMPOWERED_ITEM_END
    };

    public enum AzeriteEmpoweredItemDynamicField
    {
        AZERITE_EMPOWERED_ITEM_DYNAMIC_END
    };

    public enum AzeriteItemField
    {
        AZERITE_ITEM_FIELD_XP,
        AZERITE_ITEM_FIELD_LEVEL,
        AZERITE_ITEM_FIELD_AURA_LEVEL,
        AZERITE_ITEM_FIELD_KNOWLEDGE_LEVEL,
        AZERITE_ITEM_FIELD_DEBUG_KNOWLEDGE_WEEK,
        AZERITE_ITEM_END
    };

    public enum AzeriteItemDynamicField
    {
        AZERITE_ITEM_DYNAMIC_END
    };

    public enum UnitField
    {
        [UpdateField(UpdateFieldType.Int)]
        UNIT_CHANNEL_SPELL,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_CREATED_BY_SPELL,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_DYNAMIC_FLAGS,
        UNIT_END,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_FIELD_ATTACK_POWER,
        UNIT_FIELD_ATTACK_POWER_MODS,
        UNIT_FIELD_ATTACK_POWER_MOD_NEG,
        UNIT_FIELD_ATTACK_POWER_MOD_POS,
        [UpdateField(UpdateFieldType.Float)]
        UNIT_FIELD_ATTACK_POWER_MULTIPLIER,
        UNIT_FIELD_ATTACK_SPEED_AURA,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_FIELD_AURASTATE,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_FIELD_BASEATTACKTIME,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_FIELD_BASE_HEALTH,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_FIELD_BASE_MANA,
        UNIT_FIELD_BATTLEPET_COMPANION_NAME_TIMESTAMP,
        UNIT_FIELD_BATTLE_PET_COMPANION_GUID,
        UNIT_FIELD_BATTLE_PET_DB_ID,
        UNIT_FIELD_BONUS_RESISTANCE_MODS,
        [UpdateField(UpdateFieldType.Float)]
        UNIT_FIELD_BOUNDINGRADIUS,
        [UpdateField(UpdateFieldType.Bytes)]
        UNIT_FIELD_BYTES_0,
        [UpdateField(UpdateFieldType.Bytes)]
        UNIT_FIELD_BYTES_1,
        [UpdateField(UpdateFieldType.Bytes)]
        UNIT_FIELD_BYTES_2,
        UNIT_FIELD_CHANNEL_DATA,
        [UpdateField(UpdateFieldType.Guid)]
        UNIT_FIELD_CHANNEL_OBJECT,
        [UpdateField(UpdateFieldType.Guid)]
        UNIT_FIELD_CHARM,
        [UpdateField(UpdateFieldType.Guid)]
        UNIT_FIELD_CHARMEDBY,
        UNIT_FIELD_CONTENT_TUNING_ID,
        [UpdateField(UpdateFieldType.Float)]
        UNIT_FIELD_COMBATREACH,
        [UpdateField(UpdateFieldType.Guid)]
        UNIT_FIELD_CREATEDBY,
        [UpdateField(UpdateFieldType.Guid)]
        UNIT_FIELD_CRITTER,
        [UpdateField(UpdateFieldType.Guid)]
        UNIT_FIELD_DEMON_CREATOR,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_FIELD_DISPLAYID,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_FIELD_DISPLAY_POWER,
        UNIT_FIELD_DISPLAY_SCALE,
        UNIT_FIELD_EFFECTIVE_LEVEL,
        UNIT_FIELD_END,
        [UpdateField(UpdateFieldType.Custom)]
        UNIT_FIELD_FACTIONTEMPLATE,
        [UpdateField(UpdateFieldType.Uint)]
        UNIT_FIELD_FLAGS,
        [UpdateField(UpdateFieldType.Uint)]
        UNIT_FIELD_FLAGS_2,
        [UpdateField(UpdateFieldType.Uint)]
        UNIT_FIELD_FLAGS_3,
        UNIT_FIELD_GUILD_GUID,
        [UpdateField(UpdateFieldType.Uint)]
        UNIT_FIELD_HEALTH,
        [UpdateField(UpdateFieldType.Float)]
        UNIT_FIELD_HOVERHEIGHT,
        UNIT_FIELD_INTERACT_SPELLID,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_FIELD_LEVEL,
        UNIT_FIELD_LOOKS_LIKE_CREATURE_ID,
        UNIT_FIELD_LOOKS_LIKE_MOUNT_ID,
        UNIT_FIELD_LOOK_AT_CONTROLLER_ID,
        UNIT_FIELD_LOOK_AT_CONTROLLER_TARGET,
        UNIT_FIELD_LIFESTEAL,
        UNIT_FIELD_MAIN_HAND_WEAPON_ATTACK_POWER,
        UNIT_FIELD_MAXDAMAGE,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_FIELD_MAXHEALTH,
        UNIT_FIELD_MAXHEALTHMODIFIER,
        UNIT_FIELD_MAXITEMLEVEL,
        UNIT_FIELD_MAXOFFHANDDAMAGE,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_FIELD_MAXPOWER,
        UNIT_FIELD_MAXPOWER1,
        UNIT_FIELD_MAXPOWER10,
        UNIT_FIELD_MAXPOWER11,
        UNIT_FIELD_MAXPOWER2,
        UNIT_FIELD_MAXPOWER3,
        UNIT_FIELD_MAXPOWER4,
        UNIT_FIELD_MAXPOWER5,
        UNIT_FIELD_MAXPOWER6,
        UNIT_FIELD_MAXPOWER7,
        UNIT_FIELD_MAXPOWER8,
        UNIT_FIELD_MAXPOWER9,
        UNIT_FIELD_MAXRANGEDDAMAGE,
        UNIT_FIELD_MINDAMAGE,
        UNIT_FIELD_MINOFFHANDDAMAGE,
        UNIT_FIELD_MINRANGEDDAMAGE,
        UNIT_FIELD_MIN_ITEM_LEVEL,
        UNIT_FIELD_MIN_ITEM_LEVEL_CUTOFF,
        UNIT_FIELD_MOD_BONUS_ARMOR,
        [UpdateField(UpdateFieldType.Float)]
        UNIT_FIELD_MOD_HASTE,
        [UpdateField(UpdateFieldType.Float)]
        UNIT_FIELD_MOD_HASTE_REGEN,
        [UpdateField(UpdateFieldType.Float)]
        UNIT_FIELD_MOD_RANGED_HASTE,
        [UpdateField(UpdateFieldType.Float)]
        UNIT_FIELD_MOD_TIME_RATE,
        [UpdateField(UpdateFieldType.Uint)]
        UNIT_FIELD_MOUNTDISPLAYID,
        [UpdateField(UpdateFieldType.Uint)]
        UNIT_FIELD_NATIVEDISPLAYID,
        UNIT_FIELD_NATIVE_X_DISPLAY_SCALE,
        UNIT_FIELD_NEGSTAT,
        UNIT_FIELD_NEGSTAT0,
        UNIT_FIELD_NEGSTAT1,
        UNIT_FIELD_NEGSTAT2,
        UNIT_FIELD_NEGSTAT3,
        UNIT_FIELD_NEGSTAT4,
        UNIT_FIELD_OFF_HAND_WEAPON_ATTACK_POWER,
        UNIT_FIELD_OVERRIDE_DISPLAY_POWER_ID,
        UNIT_FIELD_PADDING,
        UNIT_FIELD_PETEXPERIENCE,
        UNIT_FIELD_PETNEXTLEVELEXP,
        UNIT_FIELD_PETNUMBER,
        UNIT_FIELD_PET_NAME_TIMESTAMP,
        UNIT_FIELD_POSSTAT,
        UNIT_FIELD_POSSTAT0,
        UNIT_FIELD_POSSTAT1,
        UNIT_FIELD_POSSTAT2,
        UNIT_FIELD_POSSTAT3,
        UNIT_FIELD_POSSTAT4,
        UNIT_FIELD_POWER,
        UNIT_FIELD_POWER1,
        UNIT_FIELD_POWER10,
        UNIT_FIELD_POWER11,
        UNIT_FIELD_POWER2,
        UNIT_FIELD_POWER3,
        UNIT_FIELD_POWER4,
        UNIT_FIELD_POWER5,
        UNIT_FIELD_POWER6,
        UNIT_FIELD_POWER7,
        UNIT_FIELD_POWER8,
        UNIT_FIELD_POWER9,
        UNIT_FIELD_POWER_COST_MODIFIER,
        UNIT_FIELD_POWER_COST_MULTIPLIER,
        UNIT_FIELD_POWER_COST_MULTIPLIER1,
        UNIT_FIELD_POWER_COST_MULTIPLIER2,
        UNIT_FIELD_POWER_COST_MULTIPLIER3,
        UNIT_FIELD_POWER_COST_MULTIPLIER4,
        UNIT_FIELD_POWER_COST_MULTIPLIER5,
        UNIT_FIELD_POWER_COST_MULTIPLIER6,
        UNIT_FIELD_POWER_COST_MULTIPLIER7,
        UNIT_FIELD_POWER_REGEN_FLAT_MODIFIER,
        UNIT_FIELD_POWER_REGEN_INTERRUPTED_FLAT_MODIFIER,
        UNIT_FIELD_RANGEDATTACKTIME,
        UNIT_FIELD_RANGED_ATTACK_POWER,
        UNIT_FIELD_RANGED_ATTACK_POWER_MODS,
        UNIT_FIELD_RANGED_ATTACK_POWER_MOD_NEG,
        UNIT_FIELD_RANGED_ATTACK_POWER_MOD_POS,
        UNIT_FIELD_RANGED_ATTACK_POWER_MULTIPLIER,
        UNIT_FIELD_RANGED_HAND_WEAPON_ATTACK_POWER,
        UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE,
        UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_FIELD_RESISTANCES,
        UNIT_FIELD_SANDBOX_SCALING_ID,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_FIELD_SCALE_DURATION,
        UNIT_FIELD_SCALING_DAMAGE_ITEM_LEVEL_CURVE_ID,
        UNIT_FIELD_SCALING_FACTION_GROUP,
        UNIT_FIELD_SCALING_HEALTH_ITEM_LEVEL_CURVE_ID,
        UNIT_FIELD_SCALING_LEVEL_MIN,
        UNIT_FIELD_SCALING_LEVEL_MAX,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_FIELD_SCALING_LEVEL_DELTA,
        UNIT_FIELD_STAT,
        UNIT_FIELD_STAT0,
        UNIT_FIELD_STAT1,
        UNIT_FIELD_STAT2,
        UNIT_FIELD_STAT3,
        UNIT_FIELD_STAT4,
        UNIT_FIELD_STATE_ANIM_ID,
        UNIT_FIELD_STATE_ANIM_KIT_ID,
        UNIT_FIELD_STATE_SPELL_VISUAL_ID,
        UNIT_FIELD_STATE_WORLD_EFFECT_ID,
        [UpdateField(UpdateFieldType.Guid)]
        UNIT_FIELD_SUMMON,
        [UpdateField(UpdateFieldType.Guid)]
        UNIT_FIELD_SUMMONEDBY,
        UNIT_FIELD_SUMMONED_BY_HOME_REALM,
        [UpdateField(UpdateFieldType.Guid)]
        UNIT_FIELD_TARGET,
        UNIT_FIELD_UNK63,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_FIELD_WILD_BATTLEPET_LEVEL,
        [UpdateField(UpdateFieldType.Float)]
        UNIT_MOD_CAST_HASTE,
        [UpdateField(UpdateFieldType.Float)]
        UNIT_MOD_CAST_SPEED,
        [UpdateField(UpdateFieldType.Int)]
        UNIT_NPC_EMOTESTATE,
        [UpdateField(UpdateFieldType.Uint)]
        UNIT_NPC_FLAGS,
        [UpdateField(UpdateFieldType.Uint)]
        UNIT_VIRTUAL_ITEM_SLOT_ID
    }

    public enum UnitDynamicField
    {
        UNIT_DYNAMIC_FIELD_CHANNEL_OBJECTS,
        UNIT_DYNAMIC_FIELD_PASSIVE_SPELLS,
        UNIT_DYNAMIC_FIELD_WORLD_EFFECTS,
        UNIT_DYNAMIC_END
    }

    public enum PlayerField
    {
        PLAYER_AMMO_ID,
        PLAYER_AMPLIFY,
        PLAYER_AVOIDANCE,
        PLAYER_BLOCK_PERCENTAGE,
        [UpdateField(UpdateFieldType.Bytes)]
        PLAYER_BYTES,
        [UpdateField(UpdateFieldType.Bytes)]
        PLAYER_BYTES_2,
        [UpdateField(UpdateFieldType.Bytes)]
        PLAYER_BYTES_3,
        [UpdateField(UpdateFieldType.Bytes)]
        PLAYER_BYTES_4,
        PLAYER_CHARACTER_POINTS,
        PLAYER_CHARACTER_POINTS1,
        PLAYER_CHARACTER_POINTS2,
        PLAYER_CHOSEN_TITLE,
        PLAYER_CLEAVE,
        PLAYER_CRIT_PERCENTAGE,
        PLAYER_DODGE_PERCENTAGE,
        PLAYER_DODGE_PERCENTAGE_FROM_ATTRIBUTE,
        [UpdateField(UpdateFieldType.Guid)]
        PLAYER_DUEL_ARBITER,
        PLAYER_DUEL_TEAM,
        PLAYER_END,
        PLAYER_EXPERTISE,
        PLAYER_EXPLORED_ZONES_1,
        PLAYER_FAKE_INEBRIATION,
        [UpdateField(UpdateFieldType.Guid)]
        PLAYER_FARSIGHT,
        PLAYER_FIELD_ARENA_CURRENCY,
        PLAYER_FIELD_ARENA_TEAM_INFO_1_1,
        PLAYER_FIELD_AVG_ITEM_LEVEL,
        PLAYER_FIELD_AVG_ITEM_LEVEL_EQUIPPED,
        PLAYER_FIELD_AVG_ITEM_LEVEL_TOTAL,
        PLAYER_FIELD_BAG_SLOT_FLAGS,
        PLAYER_FIELD_BANKBAG_SLOT_1,
        PLAYER_FIELD_BANK_BAG_SLOT_FLAGS,
        PLAYER_FIELD_BANK_SLOT_1,
        PLAYER_FIELD_BATTLEGROUND_RATING,
        PLAYER_FIELD_BUYBACK_PRICE_1,
        PLAYER_FIELD_BUYBACK_PRICE_10,
        PLAYER_FIELD_BUYBACK_PRICE_11,
        PLAYER_FIELD_BUYBACK_PRICE_12,
        PLAYER_FIELD_BUYBACK_PRICE_2,
        PLAYER_FIELD_BUYBACK_PRICE_3,
        PLAYER_FIELD_BUYBACK_PRICE_4,
        PLAYER_FIELD_BUYBACK_PRICE_5,
        PLAYER_FIELD_BUYBACK_PRICE_6,
        PLAYER_FIELD_BUYBACK_PRICE_7,
        PLAYER_FIELD_BUYBACK_PRICE_8,
        PLAYER_FIELD_BUYBACK_PRICE_9,
        PLAYER_FIELD_BUYBACK_TIMESTAMP_1,
        PLAYER_FIELD_BUYBACK_TIMESTAMP_10,
        PLAYER_FIELD_BUYBACK_TIMESTAMP_11,
        PLAYER_FIELD_BUYBACK_TIMESTAMP_12,
        PLAYER_FIELD_BUYBACK_TIMESTAMP_2,
        PLAYER_FIELD_BUYBACK_TIMESTAMP_3,
        PLAYER_FIELD_BUYBACK_TIMESTAMP_4,
        PLAYER_FIELD_BUYBACK_TIMESTAMP_5,
        PLAYER_FIELD_BUYBACK_TIMESTAMP_6,
        PLAYER_FIELD_BUYBACK_TIMESTAMP_7,
        PLAYER_FIELD_BUYBACK_TIMESTAMP_8,
        PLAYER_FIELD_BUYBACK_TIMESTAMP_9,
        [UpdateField(UpdateFieldType.Bytes)]
        PLAYER_FIELD_BYTES,
        [UpdateField(UpdateFieldType.Bytes)]
        PLAYER_FIELD_BYTES2,
        [UpdateField(UpdateFieldType.Bytes)]
        PLAYER_FIELD_BYTES3,
        PLAYER_FIELD_COINAGE,
        PLAYER_FIELD_COMBAT_RATING_1,
        PLAYER_FIELD_COMBAT_RATING_EXPERTISE,
        PLAYER_FIELD_CURRENCYTOKEN_SLOT_1,
        PLAYER_FIELD_CURRENT_BATTLE_PET_BREED_QUALITY,
        PLAYER_FIELD_CURRENT_SPEC_ID,
        PLAYER_FIELD_DAILY_QUESTS_1,
        PLAYER_FIELD_GLYPHS_1,
        PLAYER_FIELD_GLYPHS_2,
        PLAYER_FIELD_GLYPHS_3,
        PLAYER_FIELD_GLYPHS_4,
        PLAYER_FIELD_GLYPHS_5,
        PLAYER_FIELD_GLYPHS_6,
        PLAYER_FIELD_GLYPH_SLOTS_1,
        PLAYER_FIELD_GLYPH_SLOTS_2,
        PLAYER_FIELD_GLYPH_SLOTS_3,
        PLAYER_FIELD_GLYPH_SLOTS_4,
        PLAYER_FIELD_GLYPH_SLOTS_5,
        PLAYER_FIELD_GLYPH_SLOTS_6,
        PLAYER_FIELD_HOME_REALM_TIME_OFFSET,
        PLAYER_FIELD_HONOR,
        PLAYER_FIELD_HONOR_CURRENCY,
        PLAYER_FIELD_HONOR_LEVEL,
        PLAYER_FIELD_HONOR_NEXT_LEVEL,
        PLAYER_FIELD_INSERT_ITEMS_LEFT_TO_RIGHT,
        PLAYER_FIELD_INV_SLOT_FIXME1,
        PLAYER_FIELD_INV_SLOT_FIXME10,
        PLAYER_FIELD_INV_SLOT_FIXME11,
        PLAYER_FIELD_INV_SLOT_FIXME12,
        PLAYER_FIELD_INV_SLOT_FIXME13,
        PLAYER_FIELD_INV_SLOT_FIXME14,
        PLAYER_FIELD_INV_SLOT_FIXME15,
        PLAYER_FIELD_INV_SLOT_FIXME16,
        PLAYER_FIELD_INV_SLOT_FIXME17,
        PLAYER_FIELD_INV_SLOT_FIXME18,
        PLAYER_FIELD_INV_SLOT_FIXME19,
        PLAYER_FIELD_INV_SLOT_FIXME2,
        PLAYER_FIELD_INV_SLOT_FIXME20,
        PLAYER_FIELD_INV_SLOT_FIXME21,
        PLAYER_FIELD_INV_SLOT_FIXME22,
        PLAYER_FIELD_INV_SLOT_FIXME3,
        PLAYER_FIELD_INV_SLOT_FIXME4,
        PLAYER_FIELD_INV_SLOT_FIXME5,
        PLAYER_FIELD_INV_SLOT_FIXME6,
        PLAYER_FIELD_INV_SLOT_FIXME7,
        PLAYER_FIELD_INV_SLOT_FIXME8,
        PLAYER_FIELD_INV_SLOT_FIXME9,
        PLAYER_FIELD_INV_SLOT_HEAD,
        PLAYER_FIELD_ITEM_LEVEL_DELTA,
        PLAYER_FIELD_KEYRING_SLOT_1,
        PLAYER_FIELD_KILLS,
        PLAYER_FIELD_KNOWN_CURRENCIES,
        PLAYER_FIELD_KNOWN_TITLES,
        PLAYER_FIELD_KNOWN_TITLES1,
        PLAYER_FIELD_KNOWN_TITLES2,
        PLAYER_FIELD_LFG_BONUS_FACTION_ID,
        PLAYER_FIELD_LIFETIME_HONORABLE_KILLS,
        PLAYER_FIELD_LOCAL_FLAGS,
        PLAYER_FIELD_LOOT_SPEC_ID,
        PLAYER_FIELD_MAX_CREATURE_SCALING_LEVEL,
        PLAYER_FIELD_MAX_LEVEL,
        PLAYER_FIELD_MAX_TALENT_TIERS,
        PLAYER_FIELD_MOD_DAMAGE_DONE_NEG,
        PLAYER_FIELD_MOD_DAMAGE_DONE_PCT,
        PLAYER_FIELD_MOD_DAMAGE_DONE_PCT1,
        PLAYER_FIELD_MOD_DAMAGE_DONE_PCT2,
        PLAYER_FIELD_MOD_DAMAGE_DONE_PCT3,
        PLAYER_FIELD_MOD_DAMAGE_DONE_PCT4,
        PLAYER_FIELD_MOD_DAMAGE_DONE_PCT5,
        PLAYER_FIELD_MOD_DAMAGE_DONE_PCT6,
        PLAYER_FIELD_MOD_DAMAGE_DONE_PCT7,
        PLAYER_FIELD_MOD_DAMAGE_DONE_POS,
        PLAYER_FIELD_MOD_HASTE,
        PLAYER_FIELD_MOD_HASTE_REGEN,
        PLAYER_FIELD_MOD_HEALING_DONE_PCT,
        PLAYER_FIELD_MOD_HEALING_DONE_POS,
        PLAYER_FIELD_MOD_HEALING_PCT,
        PLAYER_FIELD_MOD_PERIODIC_HEALING_DONE_PERCENT,
        PLAYER_FIELD_MOD_PET_HASTE,
        PLAYER_FIELD_MOD_RANGED_HASTE,
        PLAYER_FIELD_MOD_RESILIENCE_PERCENT,
        PLAYER_FIELD_MOD_SPELL_POWER_PCT,
        PLAYER_FIELD_MOD_TARGET_PHYSICAL_RESISTANCE,
        PLAYER_FIELD_MOD_TARGET_RESISTANCE,
        PLAYER_FIELD_OVERRIDE_AP_BY_SPELL_POWER_PERCENT,
        PLAYER_FIELD_OVERRIDE_SPELL_POWER_BY_AP_PCT,
        PLAYER_FIELD_OVERRIDE_ZONE_PVP_TYPE,
        PLAYER_FIELD_PACK_SLOT_1,
        PLAYER_FIELD_PADDING,
        PLAYER_FIELD_PAD_0,
        PLAYER_FIELD_PRESTIGE,
        PLAYER_FIELD_PVP_MEDALS,
        PLAYER_FIELD_PVP_POWER_DAMAGE,
        PLAYER_FIELD_PVP_POWER_HEALING,
        PLAYER_FIELD_QUEST_COMPLETED,
        PLAYER_FIELD_RANGED_EXPERTISE,
        PLAYER_FIELD_RESEARCHING_1,
        PLAYER_FIELD_RESERACH_SITE_1,
        PLAYER_FIELD_REST_INFO,
        PLAYER_FIELD_SCALING_PLAYER_LEVEL_DELTA,
        PLAYER_FIELD_SUMMONED_BATTLE_PET_ID,
        PLAYER_FIELD_TAXI_MOUNT_ANIM_KIT_ID,
        PLAYER_FIELD_TODAY_CONTRIBUTION,
        PLAYER_FIELD_UI_HIT_MODIFIER,
        PLAYER_FIELD_UI_SPELL_HIT_MODIFIER,
        PLAYER_FIELD_VENDORBUYBACK_SLOT_1,
        PLAYER_FIELD_VIRTUAL_PLAYER_REALM,
        PLAYER_FIELD_WATCHED_FACTION_INDEX,
        PLAYER_FIELD_WEAPON_ATK_SPEED_MULTIPLIERS,
        PLAYER_FIELD_WEAPON_DMG_MULTIPLIERS,
        PLAYER_FIELD_YESTERDAY_CONTRIBUTION,
        PLAYER_FLAGS,
        PLAYER_FLAGS_EX,
        PLAYER_GLYPHS_ENABLED,
        PLAYER_GUILDDELETE_DATE,
        PLAYER_GUILDID,
        PLAYER_GUILDLEVEL,
        PLAYER_GUILDRANK,
        PLAYER_GUILD_TIMESTAMP,
        PLAYER_LIFESTEAL,
        PLAYER_LOOT_TARGET_GUID,
        PLAYER_MASTERY,
        PLAYER_MULTISTRIKE,
        PLAYER_MULTISTRIKE_EFFECT,
        PLAYER_NEXT_LEVEL_XP,
        PLAYER_NO_REAGENT_COST_1,
        PLAYER_OFFHAND_CRIT_PERCENTAGE,
        PLAYER_OFFHAND_EXPERTISE,
        PLAYER_PARRY_PERCENTAGE,
        PLAYER_PARRY_PERCENTAGE_FROM_ATTRIBUTE,
        PLAYER_PET_SPELL_POWER,
        PLAYER_PROFESSION_SKILL_LINE_1,
        PLAYER_QUEST_LOG,
        PLAYER_QUEST_LOG_10_1,
        PLAYER_QUEST_LOG_10_2,
        PLAYER_QUEST_LOG_10_3,
        PLAYER_QUEST_LOG_10_5,
        PLAYER_QUEST_LOG_11_1,
        PLAYER_QUEST_LOG_11_2,
        PLAYER_QUEST_LOG_11_3,
        PLAYER_QUEST_LOG_11_5,
        PLAYER_QUEST_LOG_12_1,
        PLAYER_QUEST_LOG_12_2,
        PLAYER_QUEST_LOG_12_3,
        PLAYER_QUEST_LOG_12_5,
        PLAYER_QUEST_LOG_13_1,
        PLAYER_QUEST_LOG_13_2,
        PLAYER_QUEST_LOG_13_3,
        PLAYER_QUEST_LOG_13_5,
        PLAYER_QUEST_LOG_14_1,
        PLAYER_QUEST_LOG_14_2,
        PLAYER_QUEST_LOG_14_3,
        PLAYER_QUEST_LOG_14_5,
        PLAYER_QUEST_LOG_15_1,
        PLAYER_QUEST_LOG_15_2,
        PLAYER_QUEST_LOG_15_3,
        PLAYER_QUEST_LOG_15_5,
        PLAYER_QUEST_LOG_16_1,
        PLAYER_QUEST_LOG_16_2,
        PLAYER_QUEST_LOG_16_3,
        PLAYER_QUEST_LOG_16_5,
        PLAYER_QUEST_LOG_17_1,
        PLAYER_QUEST_LOG_17_2,
        PLAYER_QUEST_LOG_17_3,
        PLAYER_QUEST_LOG_17_5,
        PLAYER_QUEST_LOG_18_1,
        PLAYER_QUEST_LOG_18_2,
        PLAYER_QUEST_LOG_18_3,
        PLAYER_QUEST_LOG_18_5,
        PLAYER_QUEST_LOG_19_1,
        PLAYER_QUEST_LOG_19_2,
        PLAYER_QUEST_LOG_19_3,
        PLAYER_QUEST_LOG_19_5,
        PLAYER_QUEST_LOG_1_1,
        PLAYER_QUEST_LOG_1_2,
        PLAYER_QUEST_LOG_1_3,
        PLAYER_QUEST_LOG_1_4,
        PLAYER_QUEST_LOG_1_5,
        PLAYER_QUEST_LOG_20_1,
        PLAYER_QUEST_LOG_20_2,
        PLAYER_QUEST_LOG_20_3,
        PLAYER_QUEST_LOG_20_5,
        PLAYER_QUEST_LOG_21_1,
        PLAYER_QUEST_LOG_21_2,
        PLAYER_QUEST_LOG_21_3,
        PLAYER_QUEST_LOG_21_5,
        PLAYER_QUEST_LOG_22_1,
        PLAYER_QUEST_LOG_22_2,
        PLAYER_QUEST_LOG_22_3,
        PLAYER_QUEST_LOG_22_5,
        PLAYER_QUEST_LOG_23_1,
        PLAYER_QUEST_LOG_23_2,
        PLAYER_QUEST_LOG_23_3,
        PLAYER_QUEST_LOG_23_5,
        PLAYER_QUEST_LOG_24_1,
        PLAYER_QUEST_LOG_24_2,
        PLAYER_QUEST_LOG_24_3,
        PLAYER_QUEST_LOG_24_5,
        PLAYER_QUEST_LOG_25_1,
        PLAYER_QUEST_LOG_25_2,
        PLAYER_QUEST_LOG_25_3,
        PLAYER_QUEST_LOG_25_5,
        PLAYER_QUEST_LOG_26_1,
        PLAYER_QUEST_LOG_26_2,
        PLAYER_QUEST_LOG_26_3,
        PLAYER_QUEST_LOG_26_5,
        PLAYER_QUEST_LOG_27_1,
        PLAYER_QUEST_LOG_27_2,
        PLAYER_QUEST_LOG_27_3,
        PLAYER_QUEST_LOG_27_5,
        PLAYER_QUEST_LOG_28_1,
        PLAYER_QUEST_LOG_28_2,
        PLAYER_QUEST_LOG_28_3,
        PLAYER_QUEST_LOG_28_5,
        PLAYER_QUEST_LOG_29_1,
        PLAYER_QUEST_LOG_29_2,
        PLAYER_QUEST_LOG_29_3,
        PLAYER_QUEST_LOG_29_5,
        PLAYER_QUEST_LOG_2_1,
        PLAYER_QUEST_LOG_2_2,
        PLAYER_QUEST_LOG_2_3,
        PLAYER_QUEST_LOG_2_5,
        PLAYER_QUEST_LOG_30_1,
        PLAYER_QUEST_LOG_30_2,
        PLAYER_QUEST_LOG_30_3,
        PLAYER_QUEST_LOG_30_5,
        PLAYER_QUEST_LOG_31_1,
        PLAYER_QUEST_LOG_31_2,
        PLAYER_QUEST_LOG_31_3,
        PLAYER_QUEST_LOG_31_5,
        PLAYER_QUEST_LOG_32_1,
        PLAYER_QUEST_LOG_32_2,
        PLAYER_QUEST_LOG_32_3,
        PLAYER_QUEST_LOG_32_5,
        PLAYER_QUEST_LOG_33_1,
        PLAYER_QUEST_LOG_33_2,
        PLAYER_QUEST_LOG_33_3,
        PLAYER_QUEST_LOG_33_5,
        PLAYER_QUEST_LOG_34_1,
        PLAYER_QUEST_LOG_34_2,
        PLAYER_QUEST_LOG_34_3,
        PLAYER_QUEST_LOG_34_5,
        PLAYER_QUEST_LOG_35_1,
        PLAYER_QUEST_LOG_35_2,
        PLAYER_QUEST_LOG_35_3,
        PLAYER_QUEST_LOG_35_5,
        PLAYER_QUEST_LOG_36_1,
        PLAYER_QUEST_LOG_36_2,
        PLAYER_QUEST_LOG_36_3,
        PLAYER_QUEST_LOG_36_5,
        PLAYER_QUEST_LOG_37_1,
        PLAYER_QUEST_LOG_37_2,
        PLAYER_QUEST_LOG_37_3,
        PLAYER_QUEST_LOG_37_5,
        PLAYER_QUEST_LOG_38_1,
        PLAYER_QUEST_LOG_38_2,
        PLAYER_QUEST_LOG_38_3,
        PLAYER_QUEST_LOG_38_5,
        PLAYER_QUEST_LOG_39_1,
        PLAYER_QUEST_LOG_39_2,
        PLAYER_QUEST_LOG_39_3,
        PLAYER_QUEST_LOG_39_5,
        PLAYER_QUEST_LOG_3_1,
        PLAYER_QUEST_LOG_3_2,
        PLAYER_QUEST_LOG_3_3,
        PLAYER_QUEST_LOG_3_5,
        PLAYER_QUEST_LOG_40_1,
        PLAYER_QUEST_LOG_40_2,
        PLAYER_QUEST_LOG_40_3,
        PLAYER_QUEST_LOG_40_5,
        PLAYER_QUEST_LOG_41_1,
        PLAYER_QUEST_LOG_41_2,
        PLAYER_QUEST_LOG_41_3,
        PLAYER_QUEST_LOG_41_5,
        PLAYER_QUEST_LOG_42_1,
        PLAYER_QUEST_LOG_42_2,
        PLAYER_QUEST_LOG_42_3,
        PLAYER_QUEST_LOG_42_5,
        PLAYER_QUEST_LOG_43_1,
        PLAYER_QUEST_LOG_43_2,
        PLAYER_QUEST_LOG_43_3,
        PLAYER_QUEST_LOG_43_5,
        PLAYER_QUEST_LOG_44_1,
        PLAYER_QUEST_LOG_44_2,
        PLAYER_QUEST_LOG_44_3,
        PLAYER_QUEST_LOG_44_5,
        PLAYER_QUEST_LOG_45_1,
        PLAYER_QUEST_LOG_45_2,
        PLAYER_QUEST_LOG_45_3,
        PLAYER_QUEST_LOG_45_5,
        PLAYER_QUEST_LOG_46_1,
        PLAYER_QUEST_LOG_46_2,
        PLAYER_QUEST_LOG_46_3,
        PLAYER_QUEST_LOG_46_5,
        PLAYER_QUEST_LOG_47_1,
        PLAYER_QUEST_LOG_47_2,
        PLAYER_QUEST_LOG_47_3,
        PLAYER_QUEST_LOG_47_5,
        PLAYER_QUEST_LOG_48_1,
        PLAYER_QUEST_LOG_48_2,
        PLAYER_QUEST_LOG_48_3,
        PLAYER_QUEST_LOG_48_5,
        PLAYER_QUEST_LOG_49_1,
        PLAYER_QUEST_LOG_49_2,
        PLAYER_QUEST_LOG_49_3,
        PLAYER_QUEST_LOG_49_5,
        PLAYER_QUEST_LOG_4_1,
        PLAYER_QUEST_LOG_4_2,
        PLAYER_QUEST_LOG_4_3,
        PLAYER_QUEST_LOG_4_5,
        PLAYER_QUEST_LOG_50_1,
        PLAYER_QUEST_LOG_50_2,
        PLAYER_QUEST_LOG_50_3,
        PLAYER_QUEST_LOG_50_5,
        PLAYER_QUEST_LOG_5_1,
        PLAYER_QUEST_LOG_5_2,
        PLAYER_QUEST_LOG_5_3,
        PLAYER_QUEST_LOG_5_5,
        PLAYER_QUEST_LOG_6_1,
        PLAYER_QUEST_LOG_6_2,
        PLAYER_QUEST_LOG_6_3,
        PLAYER_QUEST_LOG_6_5,
        PLAYER_QUEST_LOG_7_1,
        PLAYER_QUEST_LOG_7_2,
        PLAYER_QUEST_LOG_7_3,
        PLAYER_QUEST_LOG_7_5,
        PLAYER_QUEST_LOG_8_1,
        PLAYER_QUEST_LOG_8_2,
        PLAYER_QUEST_LOG_8_3,
        PLAYER_QUEST_LOG_8_5,
        PLAYER_QUEST_LOG_9_1,
        PLAYER_QUEST_LOG_9_2,
        PLAYER_QUEST_LOG_9_3,
        PLAYER_QUEST_LOG_9_5,
        PLAYER_RANGED_CRIT_PERCENTAGE,
        PLAYER_READINESS,
        PLAYER_REST_STATE_EXPERIENCE,
        PLAYER_RUNE_REGEN_1,
        PLAYER_RUNE_REGEN_2,
        PLAYER_RUNE_REGEN_3,
        PLAYER_RUNE_REGEN_4,
        PLAYER_SELF_RES_SPELL,
        PLAYER_SHIELD_BLOCK,
        PLAYER_SHIELD_BLOCK_CRIT_PERCENTAGE,
        PLAYER_SKILL_INFO_1_1,
        PLAYER_SKILL_LINEID,
        PLAYER_SPEED,
        PLAYER_SPELL_CRIT_PERCENTAGE1,
        PLAYER_SPELL_CRIT_PERCENTAGE2,
        PLAYER_SPELL_CRIT_PERCENTAGE3,
        PLAYER_SPELL_CRIT_PERCENTAGE4,
        PLAYER_SPELL_CRIT_PERCENTAGE5,
        PLAYER_SPELL_CRIT_PERCENTAGE6,
        PLAYER_SPELL_CRIT_PERCENTAGE7,
        PLAYER_STURDINESS,
        PLAYER_TRACK_CREATURES,
        PLAYER_TRACK_RESOURCES,
        PLAYER_TRIAL_XP,
        PLAYER_VERSATILITY,
        PLAYER_VERSATILITY_BONUS,
        PLAYER_VISIBLE_ITEM,
        PLAYER_VISIBLE_ITEM_10_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_10_ENTRYID,
        PLAYER_VISIBLE_ITEM_11_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_11_ENTRYID,
        PLAYER_VISIBLE_ITEM_12_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_12_ENTRYID,
        PLAYER_VISIBLE_ITEM_13_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_13_ENTRYID,
        PLAYER_VISIBLE_ITEM_14_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_14_ENTRYID,
        PLAYER_VISIBLE_ITEM_15_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_15_ENTRYID,
        PLAYER_VISIBLE_ITEM_16_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_16_ENTRYID,
        PLAYER_VISIBLE_ITEM_17_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_17_ENTRYID,
        PLAYER_VISIBLE_ITEM_18_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_18_ENTRYID,
        PLAYER_VISIBLE_ITEM_19_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_19_ENTRYID,
        PLAYER_VISIBLE_ITEM_1_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_1_ENTRYID,
        PLAYER_VISIBLE_ITEM_2_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_2_ENTRYID,
        PLAYER_VISIBLE_ITEM_3_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_3_ENTRYID,
        PLAYER_VISIBLE_ITEM_4_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_4_ENTRYID,
        PLAYER_VISIBLE_ITEM_5_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_5_ENTRYID,
        PLAYER_VISIBLE_ITEM_6_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_6_ENTRYID,
        PLAYER_VISIBLE_ITEM_7_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_7_ENTRYID,
        PLAYER_VISIBLE_ITEM_8_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_8_ENTRYID,
        PLAYER_VISIBLE_ITEM_9_ENCHANTMENT,
        PLAYER_VISIBLE_ITEM_9_ENTRYID,
        PLAYER_WOW_ACCOUNT,
        PLAYER_XP,
        PLAYER__FIELD_KNOWN_TITLES,
        PLAYER__FIELD_KNOWN_TITLES1,
        PLAYER__FIELD_KNOWN_TITLES2
    }

    public enum PlayerDynamicField
    {
        PLAYER_DYNAMIC_END,
        PLAYER_DYNAMIC_FIELD_ARENA_COOLDOWNS,
        PLAYER_DYNAMIC_FIELD_AVAILABLE_QUEST_LINE_X_QUEST_ID,
        PLAYER_DYNAMIC_FIELD_CHARACTER_RESTRICTIONS,
        PLAYER_DYNAMIC_FIELD_CONDITIONAL_TRANSMOG,
        PLAYER_DYNAMIC_FIELD_DAILY_QUESTS,
        PLAYER_DYNAMIC_FIELD_HEIRLOOM_FLAGS,
        PLAYER_DYNAMIC_FIELD_HEIRLOOMS,
        PLAYER_DYNAMIC_FIELD_RESEARCH_SITE_PROGRESS,
        PLAYER_DYNAMIC_FIELD_RESERACH_SITE,
        PLAYER_DYNAMIC_FIELD_SELF_RES_SPELLS,
        PLAYER_DYNAMIC_FIELD_SPELL_FLAT_MOD_BY_LABEL,
        PLAYER_DYNAMIC_FIELD_SPELL_PCT_MOD_BY_LABEL,
        PLAYER_DYNAMIC_FIELD_TOYS,
        PLAYER_DYNAMIC_FIELD_TRANSMOG
    }

    public enum ActivePlayerField
    {
        ACTIVE_PLAYER_END,
        ACTIVE_PLAYER_FIELD_ARENA_TEAM_INFO,
        ACTIVE_PLAYER_FIELD_AVOIDANCE,
        ACTIVE_PLAYER_FIELD_BAG_SLOT_FLAGS,
        ACTIVE_PLAYER_FIELD_BANK_BAG_SLOT_FLAGS,
        ACTIVE_PLAYER_FIELD_BLOCK_PERCENTAGE,
        ACTIVE_PLAYER_FIELD_BUYBACK_PRICE,
        ACTIVE_PLAYER_FIELD_BUYBACK_TIMESTAMP,
        [UpdateField(UpdateFieldType.Bytes)]
        ACTIVE_PLAYER_FIELD_BYTES,
        [UpdateField(UpdateFieldType.Bytes)]
        ACTIVE_PLAYER_FIELD_BYTES2,
        [UpdateField(UpdateFieldType.Bytes)]
        ACTIVE_PLAYER_FIELD_BYTES3,
        ACTIVE_PLAYER_FIELD_CHARACTER_POINTS,
        ACTIVE_PLAYER_FIELD_COINAGE,
        ACTIVE_PLAYER_FIELD_COMBAT_RATING,
        ACTIVE_PLAYER_FIELD_COMBAT_RATING_EXPERTISE,
        ACTIVE_PLAYER_FIELD_CRIT_PERCENTAGE,
        ACTIVE_PLAYER_FIELD_DODGE_PERCENTAGE,
        ACTIVE_PLAYER_FIELD_DODGE_PERCENTAGE_FROM_ATTRIBUTE,
        ACTIVE_PLAYER_FIELD_EXPERTISE,
        ACTIVE_PLAYER_FIELD_EXPLORED_ZONES,
        ACTIVE_PLAYER_FIELD_FARSIGHT,
        ACTIVE_PLAYER_FIELD_HOME_REALM_TIME_OFFSET,
        ACTIVE_PLAYER_FIELD_HONOR,
        ACTIVE_PLAYER_FIELD_HONOR_NEXT_LEVEL,
        ACTIVE_PLAYER_FIELD_INSERT_ITEMS_LEFT_TO_RIGHT,
        ACTIVE_PLAYER_FIELD_INV_SLOT_HEAD,
        ACTIVE_PLAYER_FIELD_KILLS,
        ACTIVE_PLAYER_FIELD_KNOWN_TITLES,
        ACTIVE_PLAYER_FIELD_LFG_BONUS_FACTION_ID,
        ACTIVE_PLAYER_FIELD_LIFETIME_HONORABLE_KILLS,
        ACTIVE_PLAYER_FIELD_LOCAL_FLAGS,
        ACTIVE_PLAYER_FIELD_LOOT_SPEC_ID,
        ACTIVE_PLAYER_FIELD_MASTERY,
        ACTIVE_PLAYER_FIELD_MAX_CREATURE_SCALING_LEVEL,
        ACTIVE_PLAYER_FIELD_MAX_LEVEL,
        ACTIVE_PLAYER_FIELD_MAX_TALENT_TIERS,
        ACTIVE_PLAYER_FIELD_MOD_DAMAGE_DONE_NEG,
        ACTIVE_PLAYER_FIELD_MOD_DAMAGE_DONE_PCT,
        ACTIVE_PLAYER_FIELD_MOD_DAMAGE_DONE_POS,
        ACTIVE_PLAYER_FIELD_MOD_HEALING_DONE_PCT,
        ACTIVE_PLAYER_FIELD_MOD_HEALING_DONE_POS,
        ACTIVE_PLAYER_FIELD_MOD_HEALING_PCT,
        ACTIVE_PLAYER_FIELD_MOD_PERIODIC_HEALING_DONE_PERCENT,
        ACTIVE_PLAYER_FIELD_MOD_PET_HASTE,
        ACTIVE_PLAYER_FIELD_MOD_RESILIENCE_PERCENT,
        ACTIVE_PLAYER_FIELD_MOD_SPELL_POWER_PCT,
        ACTIVE_PLAYER_FIELD_MOD_TARGET_PHYSICAL_RESISTANCE,
        ACTIVE_PLAYER_FIELD_MOD_TARGET_RESISTANCE,
        ACTIVE_PLAYER_FIELD_NEXT_LEVEL_XP,
        ACTIVE_PLAYER_FIELD_NO_REAGENT_COST,
        ACTIVE_PLAYER_FIELD_OFFHAND_CRIT_PERCENTAGE,
        ACTIVE_PLAYER_FIELD_OFFHAND_EXPERTISE,
        ACTIVE_PLAYER_FIELD_OVERRIDE_AP_BY_SPELL_POWER_PERCENT,
        ACTIVE_PLAYER_FIELD_OVERRIDE_SPELL_POWER_BY_AP_PCT,
        ACTIVE_PLAYER_FIELD_OVERRIDE_ZONE_PVP_TYPE,
        ACTIVE_PLAYER_FIELD_PARRY_PERCENTAGE,
        ACTIVE_PLAYER_FIELD_PARRY_PERCENTAGE_FROM_ATTRIBUTE,
        ACTIVE_PLAYER_FIELD_PET_SPELL_POWER,
        ACTIVE_PLAYER_FIELD_PROFESSION_SKILL_LINE,
        ACTIVE_PLAYER_FIELD_PVP_LAST_WEEKS_TIER_MAX_FROM_WINS,
        ACTIVE_PLAYER_FIELD_PVP_MEDALS,
        ACTIVE_PLAYER_FIELD_PVP_POWER_DAMAGE,
        ACTIVE_PLAYER_FIELD_PVP_POWER_HEALING,
        ACTIVE_PLAYER_FIELD_PVP_TIER_MAX_FROM_WINS,
        ACTIVE_PLAYER_FIELD_QUEST_COMPLETED,
        ACTIVE_PLAYER_FIELD_RANGED_CRIT_PERCENTAGE,
        ACTIVE_PLAYER_FIELD_RANGED_EXPERTISE,
        ACTIVE_PLAYER_FIELD_REST_INFO,
        ACTIVE_PLAYER_FIELD_SCALING_PLAYER_LEVEL_DELTA,
        ACTIVE_PLAYER_FIELD_SHIELD_BLOCK,
        ACTIVE_PLAYER_FIELD_SHIELD_BLOCK_CRIT_PERCENTAGE,
        ACTIVE_PLAYER_FIELD_SKILL_LINEID,
        ACTIVE_PLAYER_FIELD_SPEED,
        ACTIVE_PLAYER_FIELD_SPELL_CRIT_PERCENTAGE1,
        ACTIVE_PLAYER_FIELD_STURDINESS,
        ACTIVE_PLAYER_FIELD_SUMMONED_BATTLE_PET_ID,
        ACTIVE_PLAYER_FIELD_TRACK_CREATURES,
        ACTIVE_PLAYER_FIELD_TRACK_RESOURCES,
        ACTIVE_PLAYER_FIELD_TRIAL_XP,
        ACTIVE_PLAYER_FIELD_UI_HIT_MODIFIER,
        ACTIVE_PLAYER_FIELD_UI_SPELL_HIT_MODIFIER,
        ACTIVE_PLAYER_FIELD_VERSATILITY,
        ACTIVE_PLAYER_FIELD_VERSATILITY_BONUS,
        ACTIVE_PLAYER_FIELD_WATCHED_FACTION_INDEX,
        ACTIVE_PLAYER_FIELD_WEAPON_ATK_SPEED_MULTIPLIERS,
        ACTIVE_PLAYER_FIELD_WEAPON_DMG_MULTIPLIERS,
        ACTIVE_PLAYER_FIELD_XP
    }

    public enum ActivePlayerDynamicField
    {
        ACTIVE_PLAYER_DYNAMIC_END,
        ACTIVE_PLAYER_DYNAMIC_FIELD_AVAILABLE_QUEST_LINE_X_QUEST_ID,
        ACTIVE_PLAYER_DYNAMIC_FIELD_CHARACTER_RESTRICTIONS,
        ACTIVE_PLAYER_DYNAMIC_FIELD_CONDITIONAL_TRANSMOG,
        ACTIVE_PLAYER_DYNAMIC_FIELD_DAILY_QUESTS,
        ACTIVE_PLAYER_DYNAMIC_FIELD_HEIRLOOM_FLAGS,
        ACTIVE_PLAYER_DYNAMIC_FIELD_HEIRLOOMS,
        ACTIVE_PLAYER_DYNAMIC_FIELD_RESEARCH_SITE_PROGRESS,
        ACTIVE_PLAYER_DYNAMIC_FIELD_RESERACH,
        ACTIVE_PLAYER_DYNAMIC_FIELD_RESERACH_SITE,
        ACTIVE_PLAYER_DYNAMIC_FIELD_SELF_RES_SPELLS,
        ACTIVE_PLAYER_DYNAMIC_FIELD_SPELL_FLAT_MOD_BY_LABEL,
        ACTIVE_PLAYER_DYNAMIC_FIELD_SPELL_PCT_MOD_BY_LABEL,
        ACTIVE_PLAYER_DYNAMIC_FIELD_TOYS,
        ACTIVE_PLAYER_DYNAMIC_FIELD_TRANSMOG,
    };

    public enum GameObjectField
    {
        [UpdateField(UpdateFieldType.Bytes)]
        GAMEOBJECT_BYTES_1,
        [UpdateField(UpdateFieldType.Int)]
        GAMEOBJECT_DISPLAYID,
        [UpdateField(UpdateFieldType.Short)]
        GAMEOBJECT_DYNAMIC,
        GAMEOBJECT_END,
        [UpdateField(UpdateFieldType.Int)]
        GAMEOBJECT_FACTION,
        [UpdateField(UpdateFieldType.Guid)]
        GAMEOBJECT_FIELD_CREATED_BY,
        GAMEOBJECT_FIELD_CUSTOM_PARAM,
        GAMEOBJECT_FIELD_GUILD_GUID,
        [UpdateField(UpdateFieldType.Uint)]
        GAMEOBJECT_FLAGS,
        [UpdateField(UpdateFieldType.Uint)]
        GAMEOBJECT_LEVEL,
        [UpdateField(UpdateFieldType.Quaternion)]
        [UpdateField(UpdateFieldType.PackedQuaternion, ClientVersionBuild.V3_0_2_9056)]
        GAMEOBJECT_ROTATION,
        GAMEOBJECT_PARENTROTATION,
        GAMEOBJECT_POS_X,
        GAMEOBJECT_POS_Y,
        GAMEOBJECT_POS_Z,
        GAMEOBJECT_SPELL_VISUAL_ID,
        GAMEOBJECT_ARTKIT,
        GAMEOBJECT_ANIMPROGRESS,
        GAMEOBJECT_PADDING,
        GAMEOBJECT_STATE_ANIM_ID,
        GAMEOBJECT_STATE_ANIM_KIT_ID,
        GAMEOBJECT_STATE_SPELL_VISUAL_ID,
        GAMEOBJECT_STATE_WORLD_EFFECT_ID
    }

    public enum GameObjectDynamicField
    {
        GAMEOBJECT_DYNAMIC_ENABLE_DOODAD_SETS,
        GAMEOBJECT_DYNAMIC_END
    }

    public enum DynamicObjectField
    {
        [UpdateField(UpdateFieldType.Bytes)]
        DYNAMICOBJECT_BYTES,
        [UpdateField(UpdateFieldType.Guid)]
        DYNAMICOBJECT_CASTER,
        DYNAMICOBJECT_CASTTIME,
        DYNAMICOBJECT_END,
        DYNAMICOBJECT_RADIUS,
        DYNAMICOBJECT_SPELLID,
        DYNAMICOBJECT_SPELL_X_SPELL_VISUAL_ID,
        DYNAMICOBJECT_TYPE
    }

    public enum DynamicObjectDynamicField
    {
        DYNAMICOBJECT_DYNAMIC_END
    }

    public enum CorpseField
    {
        CORPSE_END,
        [UpdateField(UpdateFieldType.Bytes)]
        CORPSE_FIELD_BYTES_1,
        [UpdateField(UpdateFieldType.Bytes)]
        CORPSE_FIELD_BYTES_2,
        CORPSE_FIELD_CUSTOM_DISPLAY_OPTION,
        CORPSE_FIELD_DISPLAY_ID,
        CORPSE_FIELD_DYNAMIC_FLAGS,
        CORPSE_FIELD_FACTIONTEMPLATE,
        CORPSE_FIELD_FLAGS,
        CORPSE_FIELD_GUILD,
        CORPSE_FIELD_GUILD_GUID,
        CORPSE_FIELD_ITEM,
        [UpdateField(UpdateFieldType.Guid)]
        CORPSE_FIELD_OWNER,
        CORPSE_FIELD_PAD,
        CORPSE_FIELD_PARTY
    }

    public enum CorpseDynamicField
    {
        CORPSE_DYNAMIC_END
    }

    public enum AreaTriggerField
    {
        AREATRIGGER_OVERRIDE_SCALE_CURVE,
        AREATRIGGER_EXTRA_SCALE_CURVE,
        AREATRIGGER_CASTER,
        AREATRIGGER_DURATION,
        AREATRIGGER_TIME_TO_TARGET,
        AREATRIGGER_TIME_TO_TARGET_SCALE,
        AREATRIGGER_TIME_TO_TARGET_EXTRA_SCALE,
        AREATRIGGER_SPELLID,
        AREATRIGGER_SPELL_FOR_VISUALS,
        AREATRIGGER_SPELL_X_SPELL_VISUAL_ID,
        AREATRIGGER_BOUNDS_RADIUS_2D,
        AREATRIGGER_DECAL_PROPERTIES_ID,
        AREATRIGGER_CREATING_EFFECT_GUID,
        AREATRIGGER_END
    }

    public enum AreaTriggerDynamicField
    {
        AREATRIGGER_DYNAMIC_END
    }

    public enum SceneObjectField
    {
        SCENEOBJECT_FIELD_CREATEDBY,
        SCENEOBJECT_FIELD_END,
        SCENEOBJECT_FIELD_RND_SEED_VAL,
        SCENEOBJECT_FIELD_SCENE_TYPE,
        SCENEOBJECT_FIELD_SCRIPT_PACKAGE_ID
    }

    public enum SceneObjectDynamicField
    {
        SCENEOBJECT_DYNAMIC_END
    }

    public enum ConversationField
    {
        CONVERSATION_FIELD_DUMMY,
        CONVERSATION_FIELD_LAST_LINE_DURATION,
        CONVERSATION_LAST_LINE_END_TIME,
        CONVERSATION_END
    }

    public enum ConversationDynamicField
    {
        CONVERSATION_DYNAMIC_FIELD_ACTORS,
        CONVERSATION_DYNAMIC_FIELD_LINES,
        CONVERSATION_DYNAMIC_END
    }

    // ReSharper restore InconsistentNaming, UnusedMember.Global
}
