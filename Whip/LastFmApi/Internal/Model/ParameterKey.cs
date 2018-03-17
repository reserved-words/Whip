using System;
using LastFmApi.Internal.Helpers;

namespace LastFmApi.Internal
{
    internal enum ParameterKey
    {
        [StringValue("method")]
        Method,
        [StringValue("api_key")]
        ApiKey,
        [StringValue("api_sig")]
        ApiSig,
        [StringValue("token")]
        Token,
        [StringValue("artist")]
        Artist,
        [StringValue("limit")]
        Limit,
        [StringValue("track")]
        Track,
        [StringValue("album")]
        Album,
        [StringValue("albumArtist")]
        AlbumArtist,
        [StringValue("duration")]
        Duration,
        [StringValue("sk")]
        SessionKey,
        [StringValue("period")]
        Period,
        [StringValue("timestamp")]
        Timestamp,
        [StringValue("user")]
        User,
        [StringValue("username")]
        Username
    }
}
