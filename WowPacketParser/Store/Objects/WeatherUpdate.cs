﻿using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("weather_update")]
    public sealed record WeatherUpdate : IDataModel
    {
        [DBFieldName("map_id")]
        public uint? MapId;

        [DBFieldName("zone_id")]
        public uint? ZoneId;

        [DBFieldName("weather_state")]
        public WeatherState? State;

        [DBFieldName("grade")]
        public float? Grade;

        [DBFieldName("unk")]
        public byte? Unk;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
