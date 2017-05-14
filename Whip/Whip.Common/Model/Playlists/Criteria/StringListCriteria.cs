using System;
using System.Collections.Generic;
using System.Linq;

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
                        return t => _function(t).Select(st => st.ToLower()).Contains(Value.ToLower().Trim());
                    default:
                        throw new InvalidOperationException("Invalid criteria type");
                }
            }
        }
    }
}
