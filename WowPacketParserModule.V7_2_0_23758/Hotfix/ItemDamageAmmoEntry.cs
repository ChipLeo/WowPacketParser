﻿using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ItemDamageAmmo, HasIndexInData = false)]
    public class ItemDamageAmmoEntry
    {
        [HotfixArray(7)]
        public float[] DPS { get; set; }
        public ushort ItemLevel { get; set; }
    }
}
