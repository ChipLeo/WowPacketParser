﻿using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.TotemCategory, HasIndexInData = false)]
    public class TotemCategoryEntry
    {
        public string Name { get; set; }
        public uint CategoryMask { get; set; }
        public byte CategoryType { get; set; }
    }
}
