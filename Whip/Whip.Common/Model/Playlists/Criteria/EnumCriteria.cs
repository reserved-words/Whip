using System;

namespace Whip.Common.Model.Playlists.Criteria
{
    public abstract class EnumCriteria<T1, T2> : Criteria<T1, T2, T2> where T2 : struct, IComparable
    {
        public EnumCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString, Func<T1, T2> function)
            : base(propertyName, criteriaType, valueString, function)
        {
        }

        protected override T2 ConvertValueString(string valueString)
        {
            return (T2)Enum.Parse(typeof(T2), valueString);
        }

        public override Predicate<T1> Predicate
        {
            get
            {
                switch (CriteriaType)
                {
                    case CriteriaType.IsEqualTo:
                        return t => _function(t).Equals(Value);
                    default:
                        throw new InvalidOperationException("Invalid criteria type");
                }
            }
        }
    }
}
