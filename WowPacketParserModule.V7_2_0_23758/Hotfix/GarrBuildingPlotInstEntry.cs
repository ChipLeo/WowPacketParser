using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.GarrBuildingPlotInst)]
    public class GarrBuildingPlotInstEntry
    {
        [HotfixArray(2)]
        public float[] LandmarkOffset { get; set; }
        public ushort UiTextureAtlasMemberID { get; set; }
        public ushort GarrSiteLevelPlotInstID { get; set; }
        public byte GarrBuildingID { get; set; }
        public uint ID { get; set; }
    }
}
