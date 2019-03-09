using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.BattlePetBreedState, HasIndexInData = false)]
    public class BattlePetBreedStateEntry
    {
        public short Value { get; set; }
        public byte BreedID { get; set; }
        public byte State { get; set; }
    }
}
