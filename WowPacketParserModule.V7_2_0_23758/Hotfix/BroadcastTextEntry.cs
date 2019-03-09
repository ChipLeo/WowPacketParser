using WowPacketParser.Enums;
using WowPacketParser.Hotfix;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.BroadcastText, HasIndexInData = false)]
    public class BroadcastTextEntry : IBroadcastTextEntry
    {
        public string Text { get; set; }
        public string Text1 { get; set; }
        [HotfixArray(3)]
        public ushort[] EmoteID { get; set; }
        [HotfixArray(3)]
        public ushort[] EmoteDelay { get; set; }
        public ushort UnkEmoteID { get; set; }
        public byte Language { get; set; }
        public byte Type { get; set; }
        [HotfixArray(2)]
        public uint[] SoundID { get; set; }
        public uint PlayerConditionID { get; set; }
    }
}
