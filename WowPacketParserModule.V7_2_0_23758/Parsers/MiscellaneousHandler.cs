using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
{
    public static class MiscellaneousHandler
    {
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

        public static void ReadClientSessionAlertConfig(Packet packet, params object[] idx)
        {
            packet.ReadInt32("Delay", idx);
            packet.ReadInt32("Period", idx);
            packet.ReadInt32("DisplayTime", idx);
        }

        [Parser(Opcode.CMSG_ENGINE_SURVEY)]
        public static void HandleEngineSurvey(Packet packet)
        {
            packet.ReadInt64("unk1");
            packet.ReadInt64("unk2");
            packet.ReadInt64("unk3");
            packet.ReadInt64("unk4");
            packet.ReadUInt32("GPUVendorID");
            packet.ReadUInt32("GPUModelID");
            packet.ReadUInt32("Unk1C");
            packet.ReadUInt32("Unk10");
            packet.ReadUInt32("Unk38");
            packet.ReadUInt32("DisplayResWidth");
            packet.ReadUInt32("DisplayResHeight");
            packet.ReadUInt32("Unk2C");
            packet.ReadUInt32("MemoryCapacity");
            packet.ReadUInt32("Unk30");
            packet.ReadUInt32("Unk18");
            packet.ReadUInt32("Unk30");
            packet.ReadUInt32("Unk18");
            packet.ReadUInt32("Unk30");
            packet.ReadInt16("unk5");
            packet.ReadInt16("unk6");
            packet.ReadByte("HasHDPlayerModels");
            packet.ReadByte("Is64BitSystem");
            packet.ReadByte("Unk3C");
            packet.ReadByte("Unk3F");
            packet.ReadByte("Unk3E");
            packet.ReadByte("Unk3F");
            packet.ReadByte("Unk3E");
        }

        [Parser(Opcode.CMSG_QUERY_PAGE_TEXT)]
        public static void HandlePageTextQuery(Packet packet)
        {
            packet.ReadInt32("Entry");
            packet.ReadPackedGuid128("Guid");
        }

        [Parser(Opcode.CMSG_TUTORIAL_FLAG)]
        public static void HandleTutorialFlag(Packet packet)
        {
            var action = packet.ReadBitsE<TutorialAction>("TutorialAction", 2);

            if (action == TutorialAction.Update)
                packet.ReadInt32E<Tutorial>("TutorialBit");
        }

        [Parser(Opcode.CMSG_SET_SELECTION)]
        [Parser(Opcode.SMSG_FORCE_OBJECT_RELINK)]
        public static void HandleSetSelection(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
        }

        [Parser(Opcode.CMSG_UNK_322A)]
        public static void HandleUnk322A(Packet packet)
        {
            packet.ReadInt32("unk32");
        }

        [Parser(Opcode.CMSG_UNK_36BA)]
        [Parser(Opcode.CMSG_UNK_36BB)]
        public static void HandleUnk36BB(Packet packet)
        {
            packet.ReadInt32("unk32");
            packet.ReadBit("unk");
        }

        [Parser(Opcode.SMSG_CUSTOM_LOAD_SCREEN)]
        public static void HandleCustomLoadScreen(Packet packet)
        {
            packet.ReadInt32("TeleportSpellID");
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.SMSG_FEATURE_SYSTEM_STATUS_GLUE_SCREEN)]
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
            packet.ReadBit("CompetitiveModeEnabled");
            packet.ReadBit("NoHandler"); // not accessed in handler
            packet.ReadBit("TrialBoostEnabled");
            packet.ReadBit("unk");
            packet.ReadInt32("TokenPollTimeSeconds");
            packet.ReadInt32E<ConsumableTokenRedeem>("TokenRedeemIndex");
            packet.ReadInt64("TokenBalanceAmount");
        }

        [Parser(Opcode.SMSG_INITIAL_SETUP)]
        public static void HandleInitialSetup(Packet packet)
        {
            packet.ReadByte("ServerExpansionLevel");
            packet.ReadByte("ServerExpansionTier");
        }

        [Parser(Opcode.SMSG_LEVEL_UPDATE)]
        public static void HandleLevelUpdate(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
            packet.ReadBit("unk");
        }

        [Parser(Opcode.SMSG_MODIFY_CHARGE_RECOVERY_SPEED)]
        public static void HandleModifyChargeRecoverySpeed(Packet packet)
        {
            packet.ReadInt32("unk1");
            packet.ReadInt32("unk2");
        }

        [Parser(Opcode.SMSG_PLAY_ONE_SHOT_ANIM_KIT)]
        public static void HandlePlayOneShotAnimKit(Packet packet)
        {
            packet.ReadPackedGuid128("Unit");
            packet.ReadUInt16("AnimKitID");
        }

        [Parser(Opcode.SMSG_WORLD_SERVER_INFO)]
        public static void HandleWorldServerInfo(Packet packet)
        {
            packet.ReadInt32("DifficultyID");
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

        [Parser(Opcode.SMSG_ACCOUNT_TOYS_UPDATE)]
        public static void HandleAccountToysUpdate(Packet packet)
        {
            packet.ReadBit("IsFullUpdate");

            var int32 = packet.ReadInt32("ToyItemIDsCount");
            var int16 = packet.ReadInt32("ToyIsFavoriteCount");

            for (int i = 0; i < int32; i++)
                packet.ReadInt32("ToyItemID", i);

            packet.ResetBitReader();

            for (int i = 0; i < int16; i++)
                packet.ReadBit("ToyIsFavorite", i);
        }

        [Parser(Opcode.SMSG_ARTIFACT_XP_GAIN)]
        public static void HandleArtifactXPGain(Packet packet)
        {
            packet.ReadPackedGuid128("Item");
            packet.ReadInt32("Quantity");
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
            }
        }

        [Parser(Opcode.CMSG_QUICK_JOIN_AUTO_ACCEPT_REQUESTS)]
        public static void HandleQuickJoinAutoAcceptRequests(Packet packet)
        {
            packet.ReadBool("unk");
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

        [Parser(Opcode.SMSG_DISPLAY_PROMOTION)]
        public static void HandleDisplayPromotion(Packet packet)
        {
            packet.ReadUInt32("PromotionID");
        }

        [Parser(Opcode.SMSG_SET_ALL_TASK_PROGRESS)]
        [Parser(Opcode.SMSG_UPDATE_TASK_PROGRESS)]
        public static void HandleSetAllTaskProgress(Packet packet)
        {
            var int4 = packet.ReadInt32("TaskProgressCount");
            for (int i = 0; i < int4; i++)
            {
                packet.ReadUInt32("TaskID", i);
                packet.ReadUInt32("FailureTime", i);
                packet.ReadUInt32("Flags", i);

                var int3 = packet.ReadInt32("ProgressCounts", i);
                for (int j = 0; j < int3; j++)
                    packet.ReadInt16("Counts", i, j);
            }
        }

        [Parser(Opcode.SMSG_PLAY_OBJECT_SOUND)]
        public static void HandlePlayObjectSound(Packet packet)
        {
            uint sound = packet.ReadUInt32("SoundId");
            packet.ReadPackedGuid128("SourceObjectGUID");
            packet.ReadPackedGuid128("TargetObjectGUID");
            packet.ReadVector3("Position");

            Storage.Sounds.Add(sound, packet.TimeSpan);
        }

        [Parser(Opcode.SMSG_SHOW_ADVENTURE_MAP)]
        public static void HandleShowAdventureMap(Packet packet)
        {
            packet.ReadPackedGuid128("Creature");
        }

        [Parser(Opcode.SMSG_START_ELAPSED_TIMERS)]
        public static void HandleStartElapsedTimers(Packet packet)
        {
            var int3 = packet.ReadInt32("ElaspedTimerCounts");
            for (int i = 0; i < int3; i++)
            {
                packet.ReadUInt32("TimerID", i);
                packet.ReadTime("CurrentDuration", i);
            }
        }

        [Parser(Opcode.SMSG_TRANSMOG_COLLECTION_UPDATE)]
        public static void HandleTransmogCollectionUpdate(Packet packet)
        {
            packet.ReadBit("unk");
            packet.ReadBit("unk2");
            var cnt = packet.ReadInt32("Count");
            for (int i = 0; i < cnt; i++)
                packet.ReadInt32("unk1", i);
        }

        [Parser(Opcode.SMSG_TWITTER_STATUS)]
        public static void HandleTwitterStatus(Packet packet)
        {
            packet.ReadUInt32("StatusInt");
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
                Parsers.MiscellaneousHandler.ReadClientSessionAlertConfig(packet, "SessionAlert");

            if (hasRaceClassExpansionLevels)
            {
                var int88 = packet.ReadInt32("RaceClassExpansionLevelsCount");
                for (int i = 0; i < int88; i++)
                    packet.ReadByte("RaceClassExpansionLevels", i);
            }

            packet.ResetBitReader();

            if (hasEuropaTicketSystemStatus)
                Parsers.MiscellaneousHandler.ReadCliEuropaTicketConfig(packet, "EuropaTicketSystemStatus");
        }

        [Parser(Opcode.SMSG_REQUEST_CEMETERY_LIST_RESPONSE)]
        public static void HandleRequestCemeteryListResponse(Packet packet)
        {
            packet.ReadBit("IsTriggered");

            var count = packet.ReadUInt32("Count");
            for (int i = 0; i < count; ++i)
                packet.ReadInt32("CemeteryID", i);
        }

        [Parser(Opcode.CMSG_REQUEST_ARTIFACT_COMPLETION_HISTORY)]
        [Parser(Opcode.CMSG_TWITTER_CHECK_STATUS)]
        [Parser(Opcode.SMSG_FORCED_DEATH_UPDATE)]
        [Parser(Opcode.SMSG_BAG_CLEANUP_FINISHED)]
        public static void HandleMiscZero(Packet packet)
        {
        }
    }
}
