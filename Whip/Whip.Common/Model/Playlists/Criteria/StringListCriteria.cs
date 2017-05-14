using System;
using System.Collections.Generic;

namespace Whip.Common.Model.Playlists.Criteria
{
    public abstract class StringListCriteria<T> : Criteria<T, List<string>, string>
    {
        public StringListCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString, Func<T, List<string>> function)
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
                        return t => _function(t).Contains(Value);
                    default:
                        throw new InvalidOperationException("Invalid criteria type");
                }
            }
        }
    }
}
