using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class GameShopHandler
    {
        [Parser(Opcode.CMSG_GAME_SHOP_QUERY)]
        public static void HandleClientGameShopQuery(Packet packet)
        {
        }

        // sub_74F354
        [Parser(Opcode.SMSG_BATTLE_PAY_GET_PRODUCT_LIST_RESPONSE)]
        public static void HandletBattlePayGetProductListResponse(Packet packet)
        {
            var BattlePayShopEntryCount = packet.ReadBits("BattlePayShopEntryCount", 19); // 36
            var BattlePayProductsCount = packet.ReadBits("BattlePayProductsCount", 19); // 16

            var BattlepayProductItemCount = new uint[BattlePayProductsCount];
            var HasBattlepayDisplayInfo = new bool[BattlePayProductsCount];

            var HasCreatureDisplayInfoID = new bool[BattlePayProductsCount];
            var TitleSize1 = new uint[BattlePayProductsCount];
            var Description = new uint[BattlePayProductsCount];
            var Title2 = new uint[BattlePayProductsCount];
            var HasFileDataID = new bool[BattlePayProductsCount];
            var HasFlags = new bool[BattlePayProductsCount];
            var HasUnk = new bool[BattlePayProductsCount];

            var HasBattlepayDisplayInfoForProduct = new bool[BattlePayProductsCount][];
            var HasCreatureDisplayInfoIDForProduct = new bool[BattlePayProductsCount][];
            var TitleSize1ForProduct = new uint[BattlePayProductsCount][];
            var DescriptionForProduct = new uint[BattlePayProductsCount][];
            var Title2ForProduct = new uint[BattlePayProductsCount][];
            var HasFileDataIDForProduct = new bool[BattlePayProductsCount][];
            var HasFlagsForProduct = new bool[BattlePayProductsCount][];
            var HasUnkForProduct = new bool[BattlePayProductsCount][];

            var HasBattlepayDisplayInfoForEntry = new bool[BattlePayShopEntryCount];
            var HasCreatureDisplayInfoIDForEntry = new bool[BattlePayShopEntryCount];
            var TitleSize1ForEntry = new uint[BattlePayShopEntryCount];
            var DescriptionForEntry = new uint[BattlePayShopEntryCount];
            var Title2ForEntry = new uint[BattlePayShopEntryCount];
            var HasFileDataIDForEntry = new bool[BattlePayShopEntryCount];
            var HasFlagsForEntry = new bool[BattlePayShopEntryCount];
            var HasUnkForEntry = new bool[BattlePayShopEntryCount];

            for (uint idx = 0; idx < BattlePayProductsCount; idx++)
            {
                packet.ReadBits("ChoiceType", 2, idx); // 60

                BattlepayProductItemCount[idx] = packet.ReadBits("BattlepayProductItemCount", 20, idx); // 44

                HasBattlepayDisplayInfoForProduct[idx] = new bool[BattlepayProductItemCount[idx]];
                HasCreatureDisplayInfoIDForProduct[idx] = new bool[BattlepayProductItemCount[idx]];
                TitleSize1ForProduct[idx] = new uint[BattlepayProductItemCount[idx]];
                DescriptionForProduct[idx] = new uint[BattlepayProductItemCount[idx]];
                Title2ForProduct[idx] = new uint[BattlepayProductItemCount[idx]];
                HasFileDataIDForProduct[idx] = new bool[BattlepayProductItemCount[idx]];
                HasFlagsForProduct[idx] = new bool[BattlepayProductItemCount[idx]];
                HasUnkForProduct[idx] = new bool[BattlepayProductItemCount[idx]];

                for (int j = 0; j < BattlepayProductItemCount[idx]; j++)
                {
                    packet.ReadBit("AlreadyHave1", idx, j); // 48+5172
                    packet.ReadBit("AlreadyHave2", idx, j); // 48+5184

                    HasBattlepayDisplayInfoForProduct[idx][j] = packet.ReadBit("HasBattlepayDisplayInfoForProduct", idx, j); // 48+5168
                    if (HasBattlepayDisplayInfoForProduct[idx][j])
                    {
                        TitleSize1ForProduct[idx][j] = packet.ReadBits("TitleSize1ForProduct", 10, idx, j); // 48+36
                        DescriptionForProduct[idx][j] = packet.ReadBits("DescriptionForProduct", 13, idx, j); // 48+1062
                        HasCreatureDisplayInfoIDForProduct[idx][j] = packet.ReadBit("HasCreatureDisplayInfoIDForProduct", idx, j); // 48+24
                        HasFileDataIDForProduct[idx][j] = packet.ReadBit("HasFileDataIDForProduct", idx, j); // 48+16
                        HasFlagsForProduct[idx][j] = packet.ReadBit("HasFlagsForProduct", idx, j); // 48+32
                        HasUnkForProduct[idx][j] = packet.ReadBit("HasUnkForProduct", idx, j); // 48+5164
                        Title2ForProduct[idx][j] = packet.ReadBits("TitleSize2ForProduct", 10, idx, j); // 48+549
                    }

                    var HasBATTLEPETRESULT = packet.ReadBit("HasBATTLEPETRESULT", idx, j); // 48+5180
                    if (HasBATTLEPETRESULT)
                        packet.ReadBits("PetResult", 4, idx, j); // 48+5176
                }

                HasBattlepayDisplayInfo[idx] = packet.ReadBit("HasBattlepayDisplayInfo", idx); // 20+5204
                if (HasBattlepayDisplayInfo[idx])
                {
                    HasCreatureDisplayInfoID[idx] = packet.ReadBit("HasCreatureDisplayInfoID", idx); // 20+52
                    TitleSize1[idx] = packet.ReadBits("TitleSize1", 10, idx); // 20+72
                    Description[idx] = packet.ReadBits("Description", 13, idx); // 20+1098
                    Title2[idx] = packet.ReadBits("Title2", 10, idx); // 20+585
                    HasFileDataID[idx] = packet.ReadBit("HasFileDataID", idx); // 20+60
                    HasFlags[idx] = packet.ReadBit("HasFlags", idx); // 20+5200
                    HasUnk[idx] = packet.ReadBit("HasUnk", idx); // 20+68
                }
            }

            var BattlePayGroupEntryCount = packet.ReadBits("BattlePayGroupEntryCount", 20); // 56
            for (uint i = 0; i < BattlePayShopEntryCount; i++)
            {
                HasBattlepayDisplayInfoForEntry[i] = packet.ReadBit("HasBattlepayDisplayInfoForEntry", i); // 40+5180

                if (HasBattlepayDisplayInfoForEntry[i])
                {
                    TitleSize1ForEntry[i] = packet.ReadBits("TitleSize1ForEntry", 10, i); // 40+48
                    HasCreatureDisplayInfoIDForEntry[i] = packet.ReadBit("HasCreatureDisplayInfoIDForEntry", i); // 40+5176
                    Title2ForEntry[i] = packet.ReadBits("Title2ForEntrySize", 10, i); // 40+561
                    DescriptionForEntry[i] = packet.ReadBits("DescriptionForEntry", 13, i); // 40+1074
                    HasFileDataIDForEntry[i] = packet.ReadBit("HasFileDataIDForEntry", i); // 40+28
                    HasFlagsForEntry[i] = packet.ReadBit("HasFlagsForEntry", i); // 40+44
                    HasUnkForEntry[i] = packet.ReadBit("HasUnkForEntry", i); // 40+36
                }
            }

            var GroupName = new uint[BattlePayGroupEntryCount];
            for (int i = 0; i < BattlePayGroupEntryCount; i++)
                GroupName[i] = packet.ReadBits("GroupNameSize", 8, i);  // 60+4
            for (int i = 0; i < BattlePayGroupEntryCount; i++)
            {
                packet.ReadInt32("GroupOrdering", i); // 60+268
                packet.ReadWoWString("GroupName", GroupName[i], i); // 60+4
                packet.ReadByte("GroupDisplayType", i); // 60+264
                packet.ReadInt32("GroupIconFileDataID", i); // 60+260
                packet.ReadInt32("GroupID", i); // 60
            }

            for (uint idx = 0; idx < BattlePayProductsCount; idx++)
            {
                packet.ReadByte("BattlePayProductType", idx); // 61

                for (int j = 0; j < BattlepayProductItemCount[idx]; j++)
                {
                    if (HasBattlepayDisplayInfoForProduct[idx][j]) // 48+5168
                    {
                        if (HasFileDataIDForProduct[idx][j]) // 48+16
                            packet.ReadInt32("BattlePayProductFileDataIDForProduct", idx, j);

                        if (HasFlagsForProduct[idx][j]) // 48+32
                            packet.ReadInt32("BattlePayProductFlagsForProduct", idx, j);

                        if (HasUnkForProduct[idx][j]) // 48+5164
                            packet.ReadInt32("BattlePayProductUnkForProduct", idx, j);

                        packet.ReadWoWString("BattlePayProductTitle1ForProduct", TitleSize1ForProduct[idx][j], idx, j);

                        if (HasCreatureDisplayInfoIDForProduct[idx][j]) // 48+24
                            packet.ReadInt32("BattlePayProductCreatureDisplayInfoIDForProduct", idx, j);

                        packet.ReadWoWString("BattlePayProductTitle2ForProduct", Title2ForProduct[idx][j], idx, j);
                        packet.ReadWoWString("BattlePayDescriptionForProduct", DescriptionForProduct[idx][j], idx, j);
                    }

                    packet.ReadInt32("BattlePayProductItemID", idx, j); // 48+4
                    packet.ReadInt32("BattlePayProductQuantity", idx, j); // 48+8
                    packet.ReadInt32("BattlePayProductID", idx, j); // 48
                }

                if (HasBattlepayDisplayInfo[idx])
                {
                    if (HasCreatureDisplayInfoID[idx]) // 20+52
                        packet.ReadInt32("BattlePayProductCreatureDisplayInfoID", idx);

                    if (HasUnk[idx]) // 20+68
                        packet.ReadInt32("BattlePayProductCreatureDisplayUnk", idx);

                    packet.ReadWoWString("BattlePayProductCreatureDisplayTitle2", Title2[idx], idx); // 20+585
                    packet.ReadWoWString("BattlePayProductCreatureDisplayTitle1", TitleSize1[idx], idx); // 20+72

                    if (HasFlags[idx]) // 20+5200
                        packet.ReadInt32("BattlePayProductCreatureDisplayFlags", idx);

                    if (HasFileDataID[idx]) // 20+60
                        packet.ReadInt32("BattlePayProductCreatureDisplayFileDataID", idx);

                    packet.ReadWoWString("BattlePayProductCreatureDisplayDescription", Description[idx], idx); // 20+1098
                }

                packet.ReadInt32("BattlePayProductProductID", idx); // 20
                packet.ReadInt32("BattlePayProductFlags", idx); // 20+44
                packet.ReadInt64("BattlePayProductNormalPriceFixedPoint", idx); // 8
                packet.ReadInt64("BattlePayProductCurrentPriceFixedPoint", idx); // 16
            }

            for (uint i = 0; i < BattlePayShopEntryCount; i++)
            {
                if (HasBattlepayDisplayInfoForEntry[i])
                {
                    if (HasCreatureDisplayInfoIDForEntry[i]) // 5176
                        packet.ReadInt32("BattlePayProductCreatureDisplayInfoIDForEntry", i);

                    packet.ReadWoWString("BattlePayProductCreatureDisplayTitle2ForEntry", Title2ForEntry[i], i); // 561

                    if (HasUnkForEntry[i]) // 36
                        packet.ReadInt32("BattlePayProductCreatureDisplayUnkForEntry", i);

                    if (HasFlagsForEntry[i]) // 44
                        packet.ReadInt32("BattlePayProductCreatureDisplayFlagsForEntry", i);

                    if (HasFileDataIDForEntry[i]) // 28
                        packet.ReadInt32("BattlePayProductCreatureDisplayFileDataIDForEntry", i);

                    packet.ReadWoWString("BattlePayProductCreatureDisplayDescriptionForEntry", DescriptionForEntry[i], i); // 1074
                    packet.ReadWoWString("BattlePayProductCreatureDisplayTitle1ForEntry", TitleSize1ForEntry[i], i); // 48
                }

                packet.ReadInt32("ShopFlags", i); // 16
                packet.ReadByte("ShopBannerType", i); // 20
                packet.ReadUInt32("ShopEntryID", i); // 0
                packet.ReadInt32("ShopOrdering", i); // 12
                packet.ReadUInt32("GroupID", i); // 4
                packet.ReadUInt32("ShopProductID", i); // 8
            }

            packet.ReadUInt32("CurrencyID"); //13
            packet.ReadUInt32("Result"); // 8
        }
    }
}
