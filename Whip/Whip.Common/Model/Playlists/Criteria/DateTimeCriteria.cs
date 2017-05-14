using System;

namespace Whip.Common.Model.Playlists.Criteria
{
    public abstract class DateTimeCriteria<T> : Criteria<T, DateTime, DateTime>
    {
        public DateTimeCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString, Func<T, DateTime> function)
            : base(propertyName, criteriaType, valueString, function)
        {
        }

        protected override DateTime ConvertValueString(string valueString)
        {
            return DateTime.Parse(valueString);
        }

        public override Predicate<T> Predicate
        {
            get
            {
                switch (CriteriaType)
                {
                    case CriteriaType.IsEqualTo:
                        return t => _function(t).Equals(Value);
                    case CriteriaType.IsBefore:
                        return t => _function(t) < Value;
                    case CriteriaType.IsAfter:
                        return t => _function(t) > Value;
                    default:
                        throw new InvalidOperationException("Invalid criteria type");
                }
            }
        }
    }
}
