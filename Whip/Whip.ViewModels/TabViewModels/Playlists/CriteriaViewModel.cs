using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Linq;
using Whip.Common;
using Whip.Common.ExtensionMethods;
using Whip.Common.Model.Playlists.Criteria;

namespace Whip.ViewModels.TabViewModels.Playlists
{
    public class CriteriaViewModel : ViewModelBase
    {
        private PropertyName? _propertyName;
        private CriteriaType? _criteriaType;

        private string _valueString;

        private List<CriteriaType> _criteriaTypes;
        private PropertyType _propertyType;

        public CriteriaViewModel()
        {
        }

        public CriteriaViewModel(Criteria criteria)
        {
            PropertyName = criteria.PropertyName;
            CriteriaType = criteria.CriteriaType;
            ValueString = criteria.ValueString;
        }

        public PropertyOwner? PropertyOwner => PropertyName?.GetAttribute<CriteriaPropertyAttribute>().PropertyOwner;

        public PropertyName? PropertyName
        {
            get { return _propertyName; }
            set
            {
                Set(ref _propertyName, value);
                OnPropertyNameUpdated();
            }
        }

        public CriteriaType? CriteriaType
        {
            get { return _criteriaType; }
            set { Set(ref _criteriaType, value); }
        }

        public List<CriteriaType> CriteriaTypes
        {
            get { return _criteriaTypes; }
            set { Set(ref _criteriaTypes, value); }
        }

        public PropertyType PropertyType
        {
            get { return _propertyType; }
            set { Set(ref _propertyType, value); }
        }

        public string ValueString
        {
            get { return _valueString; }
            set { Set(ref _valueString, value); }
        }

        private void OnPropertyNameUpdated()
        {
            ValueString = string.Empty;

            var propertyMetaData = PropertyName.GetAttribute<CriteriaPropertyAttribute>();

            CriteriaTypes = propertyMetaData.CriteriaTypes.ToList();

            CriteriaType = CriteriaTypes.Count == 1
                ? CriteriaTypes.Single()
                : (CriteriaType?)null;

            PropertyType = propertyMetaData.PropertyType;
        }
    }
}
