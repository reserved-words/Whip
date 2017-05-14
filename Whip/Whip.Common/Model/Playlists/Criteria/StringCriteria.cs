using System;

namespace Whip.Common.Model.Playlists.Criteria
{
    public abstract class StringCriteria<T> : Criteria<T, string, string>
    {
        public StringCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString, Func<T, string> function)
            : base(propertyName, criteriaType, valueString, function)
        {
        }

        protected override string ConvertValueString(string valueString)
        {
            return valueString;
        }

        public override Predicate<T> Predicate
        {
            get
            {
                switch (CriteriaType)
                {
                    case CriteriaType.IsEqualTo:
                        return t => _function(t).Equals(Value);
                    case CriteriaType.Contains:
                        return t => Value.Contains(_function(t));
                    default:
                        throw new InvalidOperationException("Invalid criteria type");
                }
            }
        }
    }
}
