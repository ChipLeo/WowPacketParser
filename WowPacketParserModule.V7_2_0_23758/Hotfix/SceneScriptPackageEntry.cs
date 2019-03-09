using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.SceneScriptPackage, HasIndexInData = false)]
    public class SceneScriptPackageEntry
    {
        public string Name { get; set; }
    }
}
