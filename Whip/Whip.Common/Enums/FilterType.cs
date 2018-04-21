using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Utilities;

namespace Whip.Common.Enums
{
    public enum FilterType
    {
        [MetaData("City")]
        City,
        [MetaData("Country")]
        Country,
        [MetaData("Date Added")]
        DateAdded,
        [MetaData("Genre")]
        Genre,
        [MetaData("Grouping")]
        Grouping,
        [MetaData("State")]
        State,
        [MetaData("Tag")]
        Tag
    }
}
