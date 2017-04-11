using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.SpellShapeshiftForm, HasIndexInData = false)]
    public class SpellShapeshiftFormEntry
    {
        public string Name { get; set; }
        public float WeaponDamageVariance { get; set; }
        public uint Flags { get; set; }
        public ushort AttackIconID { get; set; }
        public ushort CombatRoundTime { get; set; }
        [HotfixArray(4)]
        public ushort[] CreatureDisplayID { get; set; }
        [HotfixArray(8)]
        public ushort[] PresetSpellID { get; set; }
        public ushort MountTypeID { get; set; }
        public sbyte CreatureType { get; set; }
        public byte BonusActionBar { get; set; }
    }
}
