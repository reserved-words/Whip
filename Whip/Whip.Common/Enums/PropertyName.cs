using System;
using Whip.Common.Model;
using Whip.Common.Utilities;

namespace Whip.Common
{
    public enum PropertyName
    {
        [CriteriaProperty("Album Title", PropertyType.FreeText, PropertyOwner.Album, CriteriaType.IsEqualTo, CriteriaType.Contains)]
        AlbumTitle,
        [CriteriaProperty("Album Track Count", PropertyType.Int, PropertyOwner.Album, CriteriaType.IsEqualTo, CriteriaType.IsLessThan, CriteriaType.IsGreaterThan)]
        AlbumTrackCount,
        [CriteriaProperty("City", PropertyType.StringFromOptions, PropertyOwner.Artist, CriteriaType.IsEqualTo)]
        City,
        [CriteriaProperty("Country", PropertyType.StringFromOptions, PropertyOwner.Artist, CriteriaType.IsEqualTo)]
        Country,
        [CriteriaProperty("Date Added", PropertyType.DateTime, PropertyOwner.Track, CriteriaType.IsEqualTo, CriteriaType.IsBefore, CriteriaType.IsAfter)]
        DateAdded,
        [CriteriaProperty("Date Modified", PropertyType.DateTime, PropertyOwner.Track, CriteriaType.IsEqualTo, CriteriaType.IsBefore, CriteriaType.IsAfter)]
        DateModified,
        [CriteriaProperty("Disc Count", PropertyType.Int, PropertyOwner.Album, CriteriaType.IsEqualTo, CriteriaType.IsLessThan, CriteriaType.IsGreaterThan)]
        DiscCount,
        [CriteriaProperty("Disc No", PropertyType.Int, PropertyOwner.Disc, CriteriaType.IsEqualTo, CriteriaType.IsLessThan, CriteriaType.IsGreaterThan)]
        DiscNo,
        [CriteriaProperty("Disc Track Count", PropertyType.Int, PropertyOwner.Disc, CriteriaType.IsEqualTo, CriteriaType.IsLessThan, CriteriaType.IsGreaterThan)]
        DiscTrackCount,
        [CriteriaProperty("Duration", PropertyType.TimeSpan, PropertyOwner.Track, CriteriaType.IsEqualTo, CriteriaType.IsLessThan, CriteriaType.IsGreaterThan)]
        Duration,
        [CriteriaProperty("Genre", PropertyType.FreeText, PropertyOwner.Artist, CriteriaType.IsEqualTo, CriteriaType.Contains)]
        Genre,
        [CriteriaProperty("Grouping", PropertyType.StringFromOptions, PropertyOwner.Artist, CriteriaType.IsEqualTo)]
        Grouping,
        [CriteriaProperty("Release Type", PropertyType.ReleaseType, PropertyOwner.Album, CriteriaType.IsEqualTo)]
        ReleaseType,
        [CriteriaProperty("Release Year", PropertyType.Year, PropertyOwner.Album, CriteriaType.IsEqualTo, CriteriaType.IsBefore, CriteriaType.IsAfter)]
        ReleaseYear,
        [CriteriaProperty("State", PropertyType.StringFromOptions, PropertyOwner.Artist, CriteriaType.IsEqualTo)]
        State,
        [CriteriaProperty("Tags", PropertyType.StringFromOptions, PropertyOwner.Track, CriteriaType.Contains)]
        Tags,
        [CriteriaProperty("Track No", PropertyType.Int, PropertyOwner.Track, CriteriaType.IsEqualTo, CriteriaType.IsLessThan, CriteriaType.IsGreaterThan)]
        TrackNo,
        [CriteriaProperty("Track Title", PropertyType.FreeText, PropertyOwner.Track, CriteriaType.IsEqualTo, CriteriaType.Contains)]
        TrackTitle,
        [CriteriaProperty("Track Year", PropertyType.Year, PropertyOwner.Track, CriteriaType.IsEqualTo, CriteriaType.IsBefore, CriteriaType.IsAfter)]
        TrackYear,
    }

    public class CriteriaPropertyAttribute : MetaDataAttribute
    {
        public CriteriaPropertyAttribute(string displayName, PropertyType propertyType, PropertyOwner propertyOwner, params CriteriaType[] criteriaTypes)
            :base(displayName)
        {
            PropertyType = propertyType;
            PropertyOwner = propertyOwner;
            CriteriaTypes = criteriaTypes;
        }

        public CriteriaType[] CriteriaTypes { get; private set; }

        public PropertyType PropertyType { get; private set; }

        public PropertyOwner PropertyOwner { get; private set; }
    }
}
