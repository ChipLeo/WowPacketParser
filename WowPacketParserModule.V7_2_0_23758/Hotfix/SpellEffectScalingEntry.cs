using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.SpellEffectScaling, HasIndexInData = false)]
    public class SpellEffectScalingEntry
    {
        public float Coefficient { get; set; }
        public float Variance { get; set; }
        public float ResourceCoefficient { get; set; }
        public uint SpellEffectID { get; set; }
    }
}
