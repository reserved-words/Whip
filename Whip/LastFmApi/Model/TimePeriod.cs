using LastFmApi.Internal.Helpers;

namespace LastFmApi.Model
{
    public enum TimePeriod
    {
        [StringValue("overall")]
        Overall,
        [StringValue("7day")]
        Week,
        [StringValue("1month")]
        Month,
        [StringValue("3month")]
        Quarter,
        [StringValue("6month")]
        SixMonths,
        [StringValue("12month")]
        Year
    }
}
