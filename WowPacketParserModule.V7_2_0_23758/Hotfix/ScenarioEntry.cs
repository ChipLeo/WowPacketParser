﻿using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.Scenario, HasIndexInData = false)]
    public class ScenarioEntry
    {
        public string Name { get; set; }
        public ushort Data { get; set; }
        public byte Flags { get; set; }
        public byte Type { get; set; }
    }
}
