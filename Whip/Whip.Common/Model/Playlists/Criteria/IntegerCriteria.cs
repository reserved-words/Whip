using System;

namespace Whip.Common.Model.Playlists.Criteria
{
    public abstract class IntegerCriteria<T> : Criteria<T, int, int>
    {
        public IntegerCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString, Func<T, int> function)
            : base(propertyName, criteriaType, valueString, function)
        {
        }

        protected override int ConvertValueString(string valueString)
        {
            return int.Parse(valueString);
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
