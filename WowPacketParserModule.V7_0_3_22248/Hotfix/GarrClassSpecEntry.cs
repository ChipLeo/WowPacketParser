﻿using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_0_3_22248.Hotfix
{
    [HotfixStructure(DB2Hash.GarrClassSpec, ClientVersionBuild.V7_0_3_22248, ClientVersionBuild.V7_2_0_23826)]
    public class GarrClassSpecEntry
    {
        public string NameMale { get; set; }
        public string NameFemale { get; set; }
        public string NameGenderless { get; set; }
        public ushort ClassAtlasID { get; set; }
        public byte GarrFollItemSetID { get; set; }
        public byte Limit { get; set; }
        public byte Flags { get; set; }
        public uint ID { get; set; }
    }
}
