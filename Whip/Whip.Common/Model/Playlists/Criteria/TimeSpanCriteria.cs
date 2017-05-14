using System;

namespace Whip.Common.Model.Playlists.Criteria
{
    public abstract class TimeSpanCriteria<T> : Criteria<T, TimeSpan, TimeSpan>
    {
        public TimeSpanCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString, Func<T, TimeSpan> function)
            : base(propertyName, criteriaType, valueString, function)
        {
        }

        protected override TimeSpan ConvertValueString(string valueString)
        {
            return TimeSpan.Parse(valueString);
        }

        public override Predicate<T> Predicate
        {
            get
            {
                switch (CriteriaType)
                {
                    case CriteriaType.IsEqualTo:
                        return t => _function(t).Equals(Value);
                    case CriteriaType.IsLessThan:
                        return t => _function(t) < Value;
                    case CriteriaType.IsGreaterThan:
                        return t => _function(t) > Value;
                    default:
                        throw new InvalidOperationException("Invalid criteria type");
                }
            }
        }
    }
}
