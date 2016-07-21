using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("hotfix_data")]
    public sealed class HotfixData : IDataModel
    {
        [DBFieldName("RecordID", true)]
        public int RecordID;

        [DBFieldName("TableHash", true)]
        public DB2Hash TableHash;

        [DBFieldName("Timestamp", true)]
        public uint Timestamp;

        [DBFieldName("Deleted")]
        public bool? Deleted;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
