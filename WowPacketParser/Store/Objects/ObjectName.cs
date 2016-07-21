using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("objectnames")]
    public sealed class ObjectName : IDataModel
    {
        [DBFieldName("ObjectType", true)]
        public ObjectType? ObjectType;

        [DBFieldName("Id", true)]
        public int? ID;

        [DBFieldName("Name")]
        public string Name;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
