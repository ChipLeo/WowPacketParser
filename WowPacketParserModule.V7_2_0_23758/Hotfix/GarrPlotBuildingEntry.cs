using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.GarrPlotBuilding, HasIndexInData = false)]
    public class GarrPlotBuildingEntry
    {
        public byte GarrPlotID { get; set; }
        public byte GarrBuildingID { get; set; }
    }
}
