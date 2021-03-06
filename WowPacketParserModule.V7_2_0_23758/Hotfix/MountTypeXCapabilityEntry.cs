﻿using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.MountTypeXCapability, HasIndexInData = false)]
    public class MountTypeXCapabilityEntry
    {
        public ushort MountTypeID { get; set; }
        public ushort MountCapabilityID { get; set; }
        public byte OrderIndex { get; set; }
    }
}
