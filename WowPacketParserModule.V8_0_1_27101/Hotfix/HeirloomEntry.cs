using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V8_0_1_27101.Hotfix
{
    [HotfixStructure(DB2Hash.Heirloom, ClientVersionBuild.V8_0_1_27101, ClientVersionBuild.V8_1_5_29683)]
    public class HeirloomEntry
    {
        public string SourceText { get; set; }
        public uint ID { get; set; }
        public int ItemID { get; set; }
        public int LegacyUpgradedItemID { get; set; }
        public int StaticUpgradedItemID { get; set; }
        public sbyte SourceTypeEnum { get; set; }
        public byte Flags { get; set; }
        public int LegacyItemID { get; set; }
        [HotfixArray(3)]
        public int[] UpgradeItemID { get; set; }
        [HotfixArray(3)]
        public ushort[] UpgradeItemBonusListID { get; set; }
    }
}
