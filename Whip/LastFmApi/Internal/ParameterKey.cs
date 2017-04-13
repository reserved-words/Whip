using System;

namespace LastFmApi.Internal
{
    internal enum ParameterKey
    {
        [ParameterName("method")]
        Method,
        [ParameterName("api_key")]
        ApiKey,
        [ParameterName("api_sig")]
        ApiSig,
        [ParameterName("token")]
        Token,
        [ParameterName("artist")]
        Artist,
        [ParameterName("track")]
        Track,
        [ParameterName("album")]
        Album,
        [ParameterName("albumArtist")]
        AlbumArtist,
        [ParameterName("duration")]
        Duration,
        [ParameterName("sk")]
        SessionKey,
        [ParameterName("timestamp")]
        Timestamp,
        [ParameterName("username")]
        Username
    }

    internal class ParameterNameAttribute : Attribute
    {
        public ParameterNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
