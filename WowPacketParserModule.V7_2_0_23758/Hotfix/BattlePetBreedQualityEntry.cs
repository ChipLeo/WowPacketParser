using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.BattlePetBreedQuality, HasIndexInData = false)]
    public class BattlePetBreedQualityEntry
    {
        public float Modifier { get; set; }
        public byte Quality { get; set; }
    }
}
