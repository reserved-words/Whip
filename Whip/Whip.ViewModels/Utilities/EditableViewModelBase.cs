using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Whip.ViewModels.Utilities
{
    public abstract class EditableViewModelBase : ViewModelBase, IDataErrorInfo
    {
        private bool _modified;

        public bool Modified
        {
            get { return _modified; }
            set { Set(ref _modified, value); }
        }

        public virtual string Error => this[string.Empty];

        public string this[string propertyName]
        {
            get
            {
                var properties = string.IsNullOrEmpty(propertyName)
                    ? GetType().GetProperties().Where(p => p.CustomAttributes.Any()).ToList()
                    : new List<PropertyInfo> { GetType().GetProperty(propertyName) };

                return string.Join(Environment.NewLine, properties.Select(p => GetErrorMessage(p)).Where(s => !string.IsNullOrEmpty(s)));
            }
        }

        protected void SetModified<T>(string propertyName, ref T property, T value)
        {
            Set(propertyName, ref property, value);
            Modified = true;
        }

        private string GetErrorMessage(PropertyInfo property)
        {
            var validationResults = new List<ValidationResult>();
            var value = property.GetValue(this);
            var validationContext = new ValidationContext(this)
            {
                MemberName = property.Name
            };

            if (Validator.TryValidateProperty(value, validationContext, validationResults))
                return null;

            return validationResults.First().ErrorMessage;
        }
    }
}
