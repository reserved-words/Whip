namespace LastFmApi
{
    public class AlbumInfo
    {
        public AlbumInfo(string artworkUrl)
        {
            ArtworkUrl = artworkUrl;
        }

        public string ArtworkUrl { get; private set; }
    }
}
