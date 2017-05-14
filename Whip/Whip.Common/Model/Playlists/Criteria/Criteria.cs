using System;

namespace Whip.Common.Model.Playlists.Criteria
{
    public class Criteria
    {
        public Criteria() { }

        public Criteria(PropertyName propertyName, CriteriaType criteriaType, string valueString)
        {
            PropertyName = propertyName;
            CriteriaType = criteriaType;
            ValueString = valueString;
        }

        public PropertyName PropertyName { get; set; }

        public CriteriaType CriteriaType { get; set; }

        public string ValueString { get; set; }
    }

    public abstract class Criteria<T> : Criteria
    {
        public Criteria(PropertyName propertyName, CriteriaType criteriaType, string valueString)
            :base(propertyName, criteriaType, valueString)
        {
        }
        
        public abstract Predicate<T> Predicate { get; }
    }

    public abstract class Criteria<T1, T2, T3> : Criteria<T1> where T3 : IComparable
    {
        protected readonly Func<T1, T2> _function;

        public Criteria(PropertyName propertyName, CriteriaType criteriaType, string valueString, Func<T1, T2> function)
            : base(propertyName, criteriaType, valueString)
        {
            _function = function;

            Value = ConvertValueString(valueString);
        }

        protected abstract T3 ConvertValueString(string valueString);
        
        public T3 Value { get; private set; }
    }
}
