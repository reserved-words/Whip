using System;

namespace Whip.Common.Model.Playlists.Criteria
{
    public abstract class BooleanCriteria<T> : Criteria<T, bool, string>
    {
        public BooleanCriteria(PropertyName propertyName, CriteriaType criteriaType, Func<T, bool> function)
            : base(propertyName, criteriaType, null, function)
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
                    case CriteriaType.IsTrue:
                        return t => _function(t);
                    case CriteriaType.IsFalse:
                        return t => !_function(t);
                    default:
                        throw new InvalidOperationException("Invalid criteria type");
                }
            }
        }
    }
}
