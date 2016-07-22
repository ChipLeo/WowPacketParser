using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("gameobject_template")]
    public sealed class GameObjectTemplate : IDataModel
    {
        [DBFieldName("entry", true)]
        public uint? Entry;

        [DBFieldName("type")]
        public GameObjectType? Type;

        [DBFieldName("displayId")]
        public uint? DisplayID;

        [DBFieldName("name")]
        public string Name;

        [DBFieldName("IconName")]
        public string IconName;

        [DBFieldName("castBarCaption")]
        public string CastCaption;

        [DBFieldName("unk1")]
        public string UnkString;

        [DBFieldName("faction")]
        public uint? Faction;

        [DBFieldName("flags")]
        public GameObjectFlag? Flags;

        [DBFieldName("size")]
        public float? Size;

        //TODO: move to gameobject_questitem
        public uint?[] QuestItems;

        [DBFieldName("Data", TargetedDatabase.Zero, TargetedDatabase.Cataclysm, 24, true)]
        [DBFieldName("Data", TargetedDatabase.Cataclysm, TargetedDatabase.WarlordsOfDraenor, 32, true)]
        [DBFieldName("Data", TargetedDatabase.WarlordsOfDraenor, 33, true)]
        public int?[] Data;

        [DBFieldName("Expansion", TargetedDatabase.Cataclysm)]
        public uint? Expansion;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
