using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.MountCapability)]
    public class MountCapabilityEntry
    {
        public uint RequiredSpell { get; set; }
        public uint SpeedModSpell { get; set; }
        public ushort RequiredRidingSkill { get; set; }
        public ushort RequiredArea { get; set; }
        public short RequiredMap { get; set; }
        public byte Flags { get; set; }
        public uint ID { get; set; }
        public uint RequiredAura { get; set; }
    }
}
