using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.ItemArmorTotal, HasIndexInData = false)]
    public class ItemArmorTotalEntry
    {
        [HotfixArray(4)]
        public float[] Value { get; set; }
        public ushort ItemLevel { get; set; }
    }
}
