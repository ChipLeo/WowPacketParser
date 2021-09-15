using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using CoreParsers = WowPacketParser.Parsing.Parsers;
using TutorialAction703 = WowPacketParser.Enums.Version.V7_0_3_22248.TutorialAction;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class MiscellaneousHandler
    {
        public static void ReadClientSessionAlertConfig(Packet packet, params object[] idx)
        {
            packet.ReadInt32("Delay", idx);
            packet.ReadInt32("Period", idx);
            packet.ReadInt32("DisplayTime", idx);
        }
        public static void ReadCliSavedThrottleObjectState(Packet packet, params object[] idx)
        {
            packet.ReadUInt32("MaxTries", idx);
            packet.ReadUInt32("PerMilliseconds", idx);
            packet.ReadUInt32("TryCount", idx);
            packet.ReadUInt32("LastResetTimeBeforeNow", idx);
        }

        public static void ReadCliEuropaTicketConfig(Packet packet, params object[] idx)
        {
            packet.ReadBit("TicketsEnabled", idx);
            packet.ReadBit("BugsEnabled", idx);
            packet.ReadBit("ComplaintsEnabled", idx);
            packet.ReadBit("SuggestionsEnabled", idx);

            ReadCliSavedThrottleObjectState(packet, idx, "ThrottleState");
        }

        [Parser(Opcode.CMSG_RIDE_VEHICLE_INTERACT)]
        public static void HandleRideVehicleInteract(Packet packet)
        {
            packet.ReadPackedGuid128("Player");
        }

        [Parser(Opcode.CMSG_CHANGE_BANK_BAG_SLOT_FLAG)]
        public static void HandleSetBankBagSlotFlag(Packet packet)
        {
            packet.ReadInt32("unk1");
            packet.ReadInt32("unk2");
            packet.ReadBit("unkb");
        }

        [Parser(Opcode.CMSG_TIME_ADJUSTMENT_RESPONSE)]
        public static void HandleTimeAdjustementResp(Packet packet)
        {
            packet.ReadSingle("unk1");
            packet.ReadInt32("unk2");
        }

        [Parser(Opcode.CMSG_TUTORIAL_FLAG)]
        public static void HandleTutorialFlag(Packet packet)
        {
            var action = packet.ReadBitsE<TutorialAction>("TutorialAction", 2);

            if (action == TutorialAction.Update)
                packet.ReadInt32E<Tutorial>("TutorialBit");
        }

        [Parser(Opcode.CMSG_UNK_322A)]
        public static void HandleUnk322A(Packet packet)
        {
            packet.ReadInt32("unk32");
        }

        [Parser(Opcode.CMSG_UNK_353A)]
        public static void HandleUnk353A(Packet packet)
        {
            packet.ReadPackedGuid128("guid");
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.CMSG_UNK_36BB)]
        public static void HandleUnk36BB(Packet packet)
        {
            packet.ReadInt32("unk32");
            packet.ReadBit("unk");
        }

        [Parser(Opcode.SMSG_CUSTOM_LOAD_SCREEN)]
        public static void HandleCustomLoadScreen(Packet packet)
        {
            packet.ReadUInt32("TeleportSpellID");
            packet.ReadUInt32("LoadingScreenID");
        }

        [Parser(Opcode.SMSG_FEATURE_SYSTEM_STATUS_GLUE_SCREEN, ClientVersionBuild.V7_0_3_22248, ClientVersionBuild.V7_1_0_22900)]
        public static void HandleFeatureSystemStatusGlueScreen(Packet packet)
        {
            packet.ReadBit("BpayStoreEnabled");
            packet.ReadBit("BpayStoreAvailable");
            packet.ReadBit("BpayStoreDisabledByParentalControls");
            packet.ReadBit("CharUndeleteEnabled");
            packet.ReadBit("CommerceSystemEnabled");
            packet.ReadBit("Unk14");
            packet.ReadBit("WillKickFromWorld");
            packet.ReadBit("IsExpansionPreorderInStore");
            packet.ReadBit("KioskModeEnabled");
            packet.ReadBit("NoHandler"); // not accessed in handler
            packet.ReadBit("TrialBoostEnabled");

            packet.ReadInt32("TokenPollTimeSeconds");
            packet.ReadInt32E<ConsumableTokenRedeem>("TokenRedeemIndex");
        }

        [Parser(Opcode.SMSG_FEATURE_SYSTEM_STATUS_GLUE_SCREEN, ClientVersionBuild.V7_1_0_22900, ClientVersionBuild.V7_1_5_23360)]
        public static void HandleFeatureSystemStatusGlueScreen710(Packet packet)
        {
            packet.ReadBit("BpayStoreEnabled");
            packet.ReadBit("BpayStoreAvailable");
            packet.ReadBit("BpayStoreDisabledByParentalControls");
            packet.ReadBit("CharUndeleteEnabled");
            packet.ReadBit("CommerceSystemEnabled");
            packet.ReadBit("Unk14");
            packet.ReadBit("WillKickFromWorld");
            packet.ReadBit("IsExpansionPreorderInStore");
            packet.ReadBit("KioskModeEnabled");
            packet.ReadBit("CompetitiveModeEnabled");
            packet.ReadBit("NoHandler"); // not accessed in handler
            packet.ReadBit("TrialBoostEnabled");

            packet.ReadInt32("TokenPollTimeSeconds");
            packet.ReadInt32E<ConsumableTokenRedeem>("TokenRedeemIndex");
        }

        [Parser(Opcode.SMSG_INITIAL_SETUP)]
        public static void HandleInitialSetup(Packet packet)
        {
            packet.ReadByte("ServerExpansionLevel");
            packet.ReadByte("ServerExpansionTier");
        }

        [Parser(Opcode.SMSG_MODIFY_CHARGE_RECOVERY_SPEED)]
        public static void HandleModifyChargeRecoverySpeed(Packet packet)
        {
            packet.ReadInt32("unk1");
            packet.ReadInt32("unk2");
        }

        [Parser(Opcode.SMSG_WORLD_SERVER_INFO)]
        public static void HandleWorldServerInfo(Packet packet)
        {
            CoreParsers.MovementHandler.CurrentDifficultyID = packet.ReadUInt32<DifficultyId>("DifficultyID");
            packet.ReadByte("IsTournamentRealm");

            packet.ReadBit("XRealmPvpAlert");
            var hasRestrictedAccountMaxLevel = packet.ReadBit("HasRestrictedAccountMaxLevel");
            var hasRestrictedAccountMaxMoney = packet.ReadBit("HasRestrictedAccountMaxMoney");
            var hasInstanceGroupSize = packet.ReadBit("HasInstanceGroupSize");

            if (hasRestrictedAccountMaxLevel)
                packet.ReadInt32("RestrictedAccountMaxLevel");

            if (hasRestrictedAccountMaxMoney)
                packet.ReadInt32("RestrictedAccountMaxMoney");

            if (hasInstanceGroupSize)
                packet.ReadInt32("InstanceGroupSize");
        }

        [Parser(Opcode.SMSG_ACCOUNT_MOUNT_UPDATE)]
        public static void HandleAccountMountUpdate(Packet packet)
        {
            packet.ReadBit("IsFullUpdate");

            var mountSpellIDsCount = packet.ReadInt32("MountSpellIDsCount");

            for (int i = 0; i < mountSpellIDsCount; i++)
            {
                packet.ReadInt32("MountSpellIDs", i);

                packet.ResetBitReader();
                packet.ReadBits("MountIsFavorite", 2, i);
            }
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_QUERY_PAGE_TEXT_RESPONSE)]
        public static void HandlePageTextResponse(Packet packet)
        {
            packet.ReadUInt32("PageTextID");
            packet.ResetBitReader();

            Bit hasData = packet.ReadBit("Allow");
            if (!hasData)
                return; // nothing to do

            var pagesCount = packet.ReadInt32("PagesCount");

            for (int i = 0; i < pagesCount; i++)
            {
                PageText pageText = new PageText();

                uint entry = packet.ReadUInt32("ID", i);
                pageText.ID = entry;
                pageText.NextPageID = packet.ReadUInt32("NextPageID", i);

                pageText.PlayerConditionID = packet.ReadInt32("PlayerConditionID", i);
                pageText.Flags = packet.ReadByte("Flags", i);

                packet.ResetBitReader();
                uint textLen = packet.ReadBits(12);
                pageText.Text = packet.ReadWoWString("Text", textLen, i);

                packet.AddSniffData(StoreNameType.PageText, (int)entry, "QUERY_RESPONSE");
                Storage.PageTexts.Add(pageText, packet.TimeSpan);

                if (ClientLocale.PacketLocale != LocaleConstant.enUS && pageText.Text != string.Empty)
                {
                    PageTextLocale localesPageText = new PageTextLocale
                    {
                        ID = pageText.ID,
                        Text = pageText.Text
                    };
                    Storage.LocalesPageText.Add(localesPageText, packet.TimeSpan);
                }
            }
        }

        [Parser(Opcode.CMSG_QUICK_JOIN_SIGNAL_TOAST_DISPLAYED)]
        public static void HandleQuickJoinSignalToastDisplayed(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
            packet.ReadSingle("unk1");
            var cnt = packet.ReadInt32("cnt");
            for (var i = 0; i < cnt; ++i)
                packet.ReadPackedGuid128("Guid2", i);
            packet.ResetBitReader();
            packet.ReadBit("unk2");
            packet.ReadBit("unk3");
        }

        [Parser(Opcode.SMSG_FEATURE_SYSTEM_STATUS, ClientVersionBuild.V7_1_0_22900, ClientVersionBuild.V7_1_5_23360)]
        public static void HandleFeatureSystemStatus710(Packet packet)
        {
            packet.ReadByte("ComplaintStatus");

            packet.ReadUInt32("ScrollOfResurrectionRequestsRemaining");
            packet.ReadUInt32("ScrollOfResurrectionMaxRequestsPerDay");
            packet.ReadUInt32("CfgRealmID");
            packet.ReadInt32("CfgRealmRecID");
            packet.ReadUInt32("TwitterPostThrottleLimit");
            packet.ReadUInt32("TwitterPostThrottleCooldown");
            packet.ReadUInt32("TokenPollTimeSeconds");
            packet.ReadUInt32E<ConsumableTokenRedeem>("TokenRedeemIndex");

            packet.ResetBitReader();

            packet.ReadBit("VoiceEnabled");
            var hasEuropaTicketSystemStatus = packet.ReadBit("HasEuropaTicketSystemStatus");
            packet.ReadBit("ScrollOfResurrectionEnabled");
            packet.ReadBit("BpayStoreEnabled");
            packet.ReadBit("BpayStoreAvailable");
            packet.ReadBit("BpayStoreDisabledByParentalControls");
            packet.ReadBit("ItemRestorationButtonEnabled");
            packet.ReadBit("BrowserEnabled");
            var hasSessionAlert = packet.ReadBit("HasSessionAlert");
            packet.ReadBit("RecruitAFriendSendingEnabled");
            packet.ReadBit("CharUndeleteEnabled");
            packet.ReadBit("RestrictedAccount");
            packet.ReadBit("TutorialsEnabled");
            packet.ReadBit("NPETutorialsEnabled");
            packet.ReadBit("TwitterEnabled");
            packet.ReadBit("CommerceSystemEnabled");
            packet.ReadBit("Unk67");
            packet.ReadBit("WillKickFromWorld");
            packet.ReadBit("KioskModeEnabled");
            packet.ReadBit("CompetitiveModeEnabled");
            var hasRaceClassExpansionLevels = packet.ReadBit("RaceClassExpansionLevels");

            {
                packet.ResetBitReader();
                packet.ReadBit("ToastsDisabled");
                packet.ReadSingle("ToastDuration");
                packet.ReadSingle("DelayDuration");
                packet.ReadSingle("QueueMultiplier");
                packet.ReadSingle("PlayerMultiplier");
                packet.ReadSingle("PlayerFriendValue");
                packet.ReadSingle("PlayerGuildValue");
                packet.ReadSingle("ThrottleInitialThreshold");
                packet.ReadSingle("ThrottleDecayTime");
                packet.ReadSingle("ThrottlePrioritySpike");
                packet.ReadSingle("ThrottleMinThreshold");
                packet.ReadSingle("ThrottlePvPPriorityNormal");
                packet.ReadSingle("ThrottlePvPPriorityLow");
                packet.ReadSingle("ThrottlePvPHonorThreshold");
                packet.ReadSingle("ThrottleLfgListPriorityDefault");
                packet.ReadSingle("ThrottleLfgListPriorityAbove");
                packet.ReadSingle("ThrottleLfgListPriorityBelow");
                packet.ReadSingle("ThrottleLfgListIlvlScalingAbove");
                packet.ReadSingle("ThrottleLfgListIlvlScalingBelow");
                packet.ReadSingle("ThrottleRfPriorityAbove");
                packet.ReadSingle("ThrottleRfIlvlScalingAbove");
                packet.ReadSingle("ThrottleDfMaxItemLevel");
                packet.ReadSingle("ThrottleDfBestPriority");
            }

            if (hasSessionAlert)
                V6_0_2_19033.Parsers.MiscellaneousHandler.ReadClientSessionAlertConfig(packet, "SessionAlert");

            if (hasRaceClassExpansionLevels)
            {
                var int88 = packet.ReadInt32("RaceClassExpansionLevelsCount");
                for (int i = 0; i < int88; i++)
                    packet.ReadByte("RaceClassExpansionLevels", i);
            }

            packet.ResetBitReader();

            if (hasEuropaTicketSystemStatus)
                V6_0_2_19033.Parsers.MiscellaneousHandler.ReadCliEuropaTicketConfig(packet, "EuropaTicketSystemStatus");
        }

        [Parser(Opcode.SMSG_SHOW_ADVENTURE_MAP)]
        public static void HandleShowAdventureMap(Packet packet)
        {
            packet.ReadPackedGuid128("Creature");
        }

        [Parser(Opcode.SMSG_TIME_ADJUSTMENT)]
        public static void HandleTimeAdjustement(Packet packet)
        {
            packet.ReadSingle("unk1");
            packet.ReadInt32("unk2");
        }

        [Parser(Opcode.SMSG_WOW_TOKEN_CAN_VETERAN_BUY_RESULT)]
        public static void HandleWOWTokenCanVeteranBuyResult(Packet packet)
        {
            packet.ReadInt64("unk1");
            packet.ReadInt32("unk2");
            packet.ReadInt32("unk3");
        }

        [Parser(Opcode.SMSG_WOW_TOKEN_DISTRIBUTION_GLUE_UPDATE)]
        public static void HandleWOWTokenDistributionGlueUpdate(Packet packet)
        {
            packet.ReadInt32("unk1");
            packet.ReadInt32("unk2");
            var cnt1 = packet.ReadInt32("Count1");
            var cnt2 = packet.ReadInt32("Count2");
            for (int i = 0; i < cnt1; i++)
                packet.ReadInt64("unk3", i);
            for (int i = 0; i < cnt2; i++)
                packet.ReadInt64("unk4", i);
        }

        [Parser(Opcode.SMSG_WOW_TOKEN_DISTRIBUTION_UPDATE)]
        public static void HandleWOWTokenDistributionUpdate(Packet packet)
        {
            var cnt1 = packet.ReadInt32("Count1");
            var cnt2 = packet.ReadInt32("Count2");
            for (int i = 0; i < cnt1; i++)
                packet.ReadInt64("unk1", i);
            for (int i = 0; i < cnt2; i++)
                packet.ReadInt64("unk2", i);
        }

        [Parser(Opcode.SMSG_UNK_CLIENT_25C8)]
        public static void HandleUnkClient25C8(Packet packet)
        {
            packet.ReadInt32("unk32");
            packet.ReadByte("unk1");
        }

        [Parser(Opcode.SMSG_WHO)]
        public static void HandleWho(Packet packet)
        {
            var bits568 = packet.ReadBits("List count", 6);

            for (var i = 0; i < bits568; ++i)
            {
                {//065D123 22996
                    packet.ResetBitReader();
                    packet.ReadBit("IsDeleted", i);
                    var bits15 = packet.ReadBits(6);

                    var declinedNamesLen = new int[5];
                    for (var j = 0; j < 5; ++j)
                        declinedNamesLen[j] = (int)packet.ReadBits(7);

                    for (var j = 0; j < 5; ++j)
                        packet.ReadWoWString("DeclinedNames", declinedNamesLen[j], i, j);

                    packet.ReadPackedGuid128("AccountID", i);
                    packet.ReadPackedGuid128("BnetAccountID", i);
                    packet.ReadPackedGuid128("GuidActual", i);

                    packet.ReadInt32("VirtualRealmAddress", i);

                    packet.ReadByteE<Race>("Race", i);
                    packet.ReadByteE<Gender>("Sex", i);
                    packet.ReadByteE<Class>("ClassId", i);
                    packet.ReadByte("Level", i);

                    packet.ReadWoWString("Name", bits15, i);
                }

                packet.ReadPackedGuid128("GuildGUID", i);

                packet.ReadInt32("GuildVirtualRealmAddress", i);
                packet.ReadInt32("AreaID", i);

                packet.ResetBitReader();
                var bits460 = packet.ReadBits(7);
                packet.ReadBit("IsGM", i);

                packet.ReadWoWString("GuildName", bits460, i);
            }
        }

        [Parser(Opcode.SMSG_FEATURE_SYSTEM_STATUS, ClientVersionBuild.V7_1_5_23360)]
        public static void HandleFeatureSystemStatus715(Packet packet)
        {
            packet.ReadByte("ComplaintStatus");

            packet.ReadUInt32("ScrollOfResurrectionRequestsRemaining");
            packet.ReadUInt32("ScrollOfResurrectionMaxRequestsPerDay");
            packet.ReadUInt32("CfgRealmID");
            packet.ReadInt32("CfgRealmRecID");
            packet.ReadUInt32("TwitterPostThrottleLimit");
            packet.ReadUInt32("TwitterPostThrottleCooldown");
            packet.ReadUInt32("TokenPollTimeSeconds");
            packet.ReadUInt32E<ConsumableTokenRedeem>("TokenRedeemIndex");
            packet.ReadUInt64("TokenBalanceAmount");
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_3_0_24920))
                packet.ReadUInt32("BpayStoreProductDeliveryDelay");

            packet.ResetBitReader();

            packet.ReadBit("VoiceEnabled");
            var hasEuropaTicketSystemStatus = packet.ReadBit("HasEuropaTicketSystemStatus");
            packet.ReadBit("ScrollOfResurrectionEnabled");
            packet.ReadBit("BpayStoreEnabled");
            packet.ReadBit("BpayStoreAvailable");
            packet.ReadBit("BpayStoreDisabledByParentalControls");
            packet.ReadBit("ItemRestorationButtonEnabled");
            packet.ReadBit("BrowserEnabled");
            var hasSessionAlert = packet.ReadBit("HasSessionAlert");
            packet.ReadBit("RecruitAFriendSendingEnabled");
            packet.ReadBit("CharUndeleteEnabled");
            packet.ReadBit("RestrictedAccount");
            packet.ReadBit("TutorialsEnabled");
            packet.ReadBit("NPETutorialsEnabled");
            packet.ReadBit("TwitterEnabled");
            packet.ReadBit("CommerceSystemEnabled");
            packet.ReadBit("Unk67");
            packet.ReadBit("WillKickFromWorld");
            packet.ReadBit("KioskModeEnabled");
            packet.ReadBit("CompetitiveModeEnabled");
            var hasRaceClassExpansionLevels = packet.ReadBit("RaceClassExpansionLevels");
            packet.ReadBit("TokenBalanceEnabled");

            {
                packet.ResetBitReader();
                packet.ReadBit("ToastsDisabled");
                packet.ReadSingle("ToastDuration");
                packet.ReadSingle("DelayDuration");
                packet.ReadSingle("QueueMultiplier");
                packet.ReadSingle("PlayerMultiplier");
                packet.ReadSingle("PlayerFriendValue");
                packet.ReadSingle("PlayerGuildValue");
                packet.ReadSingle("ThrottleInitialThreshold");
                packet.ReadSingle("ThrottleDecayTime");
                packet.ReadSingle("ThrottlePrioritySpike");
                packet.ReadSingle("ThrottleMinThreshold");
                packet.ReadSingle("ThrottlePvPPriorityNormal");
                packet.ReadSingle("ThrottlePvPPriorityLow");
                packet.ReadSingle("ThrottlePvPHonorThreshold");
                packet.ReadSingle("ThrottleLfgListPriorityDefault");
                packet.ReadSingle("ThrottleLfgListPriorityAbove");
                packet.ReadSingle("ThrottleLfgListPriorityBelow");
                packet.ReadSingle("ThrottleLfgListIlvlScalingAbove");
                packet.ReadSingle("ThrottleLfgListIlvlScalingBelow");
                packet.ReadSingle("ThrottleRfPriorityAbove");
                packet.ReadSingle("ThrottleRfIlvlScalingAbove");
                packet.ReadSingle("ThrottleDfMaxItemLevel");
                packet.ReadSingle("ThrottleDfBestPriority");
            }

            if (hasSessionAlert)
                MiscellaneousHandler.ReadClientSessionAlertConfig(packet, "SessionAlert");

            if (hasRaceClassExpansionLevels)
            {
                var int88 = packet.ReadInt32("RaceClassExpansionLevelsCount");
                for (int i = 0; i < int88; i++)
                    packet.ReadByte("RaceClassExpansionLevels", i);
            }

            packet.ResetBitReader();

            if (hasEuropaTicketSystemStatus)
                MiscellaneousHandler.ReadCliEuropaTicketConfig(packet, "EuropaTicketSystemStatus");
        }

        [Parser(Opcode.SMSG_FEATURE_SYSTEM_STATUS_GLUE_SCREEN, ClientVersionBuild.V7_1_5_23360)]
        public static void HandleFeatureSystemStatusGlueScreen715(Packet packet)
        {
            packet.ReadBit("BpayStoreEnabled");
            packet.ReadBit("BpayStoreAvailable");
            packet.ReadBit("BpayStoreDisabledByParentalControls");
            packet.ReadBit("CharUndeleteEnabled");
            packet.ReadBit("CommerceSystemEnabled");
            packet.ReadBit("Unk14");
            packet.ReadBit("WillKickFromWorld");
            packet.ReadBit("IsExpansionPreorderInStore");
            packet.ReadBit("KioskModeEnabled");
            packet.ReadBit("CompetetiveModeEnabled");
            packet.ReadBit("NoHandler"); // not accessed in handler
            packet.ReadBit("TrialBoostEnabled");
            packet.ReadBit("TokenBalanceEnabled");
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_3_0_24920))
            {
                packet.ReadBit("LiveRegionCharacterListEnabled");
                packet.ReadBit("LiveRegionCharacterCopyEnabled");
                packet.ReadBit("LiveRegionAccountCopyEnabled");
            }
        }

        [Parser(Opcode.SMSG_CAMERA_EFFECT)]
        public static void HandleCameraEffect(Packet packet)
        {
            packet.ReadPackedGuid128("guid1");
            packet.ReadInt32("unk1");
            packet.ReadInt32("unk2");
        }

        [Parser(Opcode.SMSG_UNK_CLIENT_2578)]
        public static void HandleUnkClient2578(Packet packet)
        {
            packet.ReadInt32("MapID");
        }

        [Parser(Opcode.SMSG_SPELL_VISUAL_LOAD_SCREEN)]
        public static void HandleUnkClient25E0(Packet packet)
        {
            packet.ReadInt32("unk1");
            packet.ReadInt32("unk2");
        }

        [Parser(Opcode.SMSG_UNK_CLIENT_2639)]
        public static void HandleUnkClient2639(Packet packet)
        {
            packet.ReadPackedGuid128("guid1");
            packet.ReadInt32("unk1");
            packet.ReadBit("unkb");
        }

        [Parser(Opcode.SMSG_UNK_SOCIAL_2FF8)]
        public static void HandleUnkSocial2FF8(Packet packet)
        {
            packet.ReadInt32("unk1");
            packet.ReadInt16("unk2");
        }

        [Parser(Opcode.SMSG_UNK_SOCIAL_2FFA)]
        public static void HandleUnkSocial2FFA(Packet packet)
        {
            int cnt = (int)packet.ReadBits(9);
            packet.ReadBytes(cnt);
        }

        [Parser(Opcode.CMSG_TWITTER_CONNECT)]
        [Parser(Opcode.SMSG_ARTIFACT_POWERS_UPDATED)]
        [Parser(Opcode.SMSG_UNK_CLIENT_2819)]
        [Parser(Opcode.SMSG_UNK_CLIENT_2844)]
        public static void HandleMiscZero(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_ALLIED_RACE_DETAILS)]
        public static void HandleAlliedRaceDetails(Packet packet)
        {
            packet.ReadPackedGuid128("GUID"); // Creature or GameObject
            packet.ReadInt32("RaceID");
        }

        public static void ReadAreaPoiData(Packet packet, params object[] idx)
        {
            packet.ReadTime("StartTime", idx);
            packet.ReadInt32("AreaPoiID", idx);
            packet.ReadInt32("DurationSec", idx);
            packet.ReadUInt32("WorldStateVariableID", idx);
            packet.ReadUInt32("WorldStateValue", idx);
        }

        [Parser(Opcode.SMSG_AREA_POI_UPDATE_RESPONSE)]
        public static void HandleAreaPOIUpdateResponse(Packet packet)
        {
            var count = packet.ReadInt32("Count");

            for (var i = 0; i < count; i++)
                ReadAreaPoiData(packet, i);
        }

        [Parser(Opcode.CMSG_REQUEST_AREA_POI_UPDATE)]
        public static void HandleAreaPoiZero(Packet packet) { }

        [Parser(Opcode.SMSG_SET_MOVEMENT_ANIM_KIT)]
        public static void HandlePlayOneShotAnimKit(Packet packet)
        {
            packet.ReadPackedGuid128("Unit");
            packet.ReadUInt16("AnimKitID");
        }
    }
}
