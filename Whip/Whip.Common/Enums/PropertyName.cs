using Whip.Common.Utilities;

namespace Whip.Common
{
    public enum PropertyName
    {
        [CriteriaProperty("Album Title", PropertyType.FreeText, CriteriaType.IsEqualTo, CriteriaType.Contains)]
        AlbumTitle,
        [CriteriaProperty("Album Track Count", PropertyType.Int, CriteriaType.IsEqualTo, CriteriaType.IsLessThan, CriteriaType.IsGreaterThan)]
        AlbumTrackCount,
        [CriteriaProperty("City", PropertyType.StringFromOptions, CriteriaType.IsEqualTo)]
        City,
        [CriteriaProperty("Country", PropertyType.StringFromOptions, CriteriaType.IsEqualTo)]
        Country,
        [CriteriaProperty("Date Added", PropertyType.DateTime, CriteriaType.IsEqualTo, CriteriaType.IsBefore, CriteriaType.IsAfter)]
        DateAdded,
        [CriteriaProperty("Date Modified", PropertyType.DateTime, CriteriaType.IsEqualTo, CriteriaType.IsBefore, CriteriaType.IsAfter)]
        DateModified,
        [CriteriaProperty("Disc Count", PropertyType.Int, CriteriaType.IsEqualTo, CriteriaType.IsLessThan, CriteriaType.IsGreaterThan)]
        DiscCount,
        [CriteriaProperty("Disc No", PropertyType.Int, CriteriaType.IsEqualTo, CriteriaType.IsLessThan, CriteriaType.IsGreaterThan)]
        DiscNo,
        [CriteriaProperty("Disc Track Count", PropertyType.Int, CriteriaType.IsEqualTo, CriteriaType.IsLessThan, CriteriaType.IsGreaterThan)]
        DiscTrackCount,
        [CriteriaProperty("Duration", PropertyType.TimeSpan, CriteriaType.IsEqualTo, CriteriaType.IsLessThan, CriteriaType.IsGreaterThan)]
        Duration,
        [CriteriaProperty("Genre", PropertyType.FreeText, CriteriaType.IsEqualTo, CriteriaType.Contains)]
        Genre,
        [CriteriaProperty("Grouping", PropertyType.StringFromOptions, CriteriaType.IsEqualTo)]
        Grouping,
        [CriteriaProperty("Release Type", PropertyType.ReleaseType, CriteriaType.IsEqualTo)]
        ReleaseType,
        [CriteriaProperty("Release Year", PropertyType.Year, CriteriaType.IsEqualTo, CriteriaType.IsBefore, CriteriaType.IsAfter)]
        ReleaseYear,
        [CriteriaProperty("State", PropertyType.StringFromOptions, CriteriaType.IsEqualTo)]
        State,
        [CriteriaProperty("Tags", PropertyType.StringFromOptions, CriteriaType.Contains)]
        Tags,
        [CriteriaProperty("Track No", PropertyType.Int, CriteriaType.IsEqualTo, CriteriaType.IsLessThan, CriteriaType.IsGreaterThan)]
        TrackNo,
        [CriteriaProperty("Track Title", PropertyType.FreeText, CriteriaType.IsEqualTo, CriteriaType.Contains)]
        TrackTitle,
        [CriteriaProperty("Track Year", PropertyType.Year, CriteriaType.IsEqualTo, CriteriaType.IsBefore, CriteriaType.IsAfter)]
        TrackYear,
    }

    public class CriteriaPropertyAttribute : MetaDataAttribute
    {
        public CriteriaPropertyAttribute(string displayName, PropertyType propertyType, params CriteriaType[] criteriaTypes)
            :base(displayName)
        {
            PropertyType = propertyType;
            CriteriaTypes = criteriaTypes;
        }

        public CriteriaType[] CriteriaTypes { get; private set; }

        public PropertyType PropertyType { get; private set; }
    }
}
