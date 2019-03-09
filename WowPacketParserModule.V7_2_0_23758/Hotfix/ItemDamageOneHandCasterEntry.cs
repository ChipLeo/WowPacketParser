using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ItemDamageOneHandCaster, HasIndexInData = false)]
    public class ItemDamageOneHandCasterEntry
    {
        [HotfixArray(7)]
        public float[] DPS { get; set; }
        public ushort ItemLevel { get; set; }
    }
}
