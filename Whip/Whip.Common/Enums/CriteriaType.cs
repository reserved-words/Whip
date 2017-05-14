using Whip.Common.Utilities;

namespace Whip.Common
{
    public enum CriteriaType
    {
        [MetaData("is equal to")]
        IsEqualTo,
        [MetaData("contains")]
        Contains,
        [MetaData("is less than")]
        IsLessThan,
        [MetaData("is more than")]
        IsGreaterThan,
        [MetaData("is before")]
        IsBefore,
        [MetaData("is after")]
        IsAfter
    }
}
