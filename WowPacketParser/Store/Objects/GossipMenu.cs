using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("gossip_menu")]
    public class GossipMenu : IDataModel
    {
        [DBFieldName("entry", true)]
        public uint? Entry;

        [DBFieldName("text_id", true)]
        public uint? TextID;

        public ObjectType ObjectType;

        public uint ObjectEntry;

        //public ICollection<GossipMenuOption> GossipOptions;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
