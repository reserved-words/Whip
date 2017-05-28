
namespace Whip.Common
{
    public static class ApplicationSettings
    {
        public static string[] FileExtensions => new[] { ".mp3", ".m4a" };
        public static int TrackChangeDelay => 500;
        public static int MinutesBeforeRefreshNews => 30;
        public static int NumberOfSimilarArtistsToDisplay => 4;
        public static int DaysBeforeUpdatingArtistEvents => 7;
    }
}
