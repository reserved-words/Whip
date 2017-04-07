using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Utilities;

namespace Whip.Common.Enums
{
    [DefaultValue(Album)]
    public enum ReleaseType
    {
        [ReleaseTypeMetaData("Album", ReleaseTypeGrouping.Albums)]
        Album = 1,
        [ReleaseTypeMetaData("Best Of", ReleaseTypeGrouping.Compilations)]
        BestOf = 2,
        [ReleaseTypeMetaData("Live", ReleaseTypeGrouping.Compilations)]
        Live = 3,
        [ReleaseTypeMetaData("Compilation", ReleaseTypeGrouping.Compilations)]
        Compilation = 4,
        [ReleaseTypeMetaData("EP", ReleaseTypeGrouping.Singles)]
        EP = 5,
        [ReleaseTypeMetaData("Single", ReleaseTypeGrouping.Singles)]
        Single = 6,
        [ReleaseTypeMetaData("Other", ReleaseTypeGrouping.Other)]
        Other = 7,
        [ReleaseTypeMetaData("Various Artists", ReleaseTypeGrouping.ByOtherArtists)]
        VariousArtists = 8
    }

    public enum ReleaseTypeGrouping
    {
        [MetaData("Studio Albums")]
        Albums = 1,
        [MetaData("Compilations")]
        Compilations = 2,
        [MetaData("EPs & Singles")]
        Singles = 3,
        [MetaData("Other Releases")]
        Other = 4,
        [MetaData("By Other Artists")]
        ByOtherArtists = 5
    }

    public class ReleaseTypeMetaDataAttribute : MetaDataAttribute
    {
        public ReleaseTypeMetaDataAttribute(string displayName, ReleaseTypeGrouping grouping)
            : base(displayName)
        {
            Grouping = grouping;
        }

        public ReleaseTypeGrouping Grouping { get; private set; }
    }

    public static class ReleaseTypeExtensionMethods
    {
        public static ReleaseTypeGrouping GetReleaseTypeGrouping(this ReleaseType releaseType)
        {
            return releaseType.GetAttribute<ReleaseTypeMetaDataAttribute>()?.Grouping ?? ReleaseTypeGrouping.Albums;
        }

        public static string GetReleaseTypeGroupingDisplayName(this ReleaseType releaseType)
        {
            return releaseType.GetReleaseTypeGrouping().GetDisplayName();
        }
    }
}
