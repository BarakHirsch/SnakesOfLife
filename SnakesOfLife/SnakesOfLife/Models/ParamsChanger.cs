using System.Collections.Generic;
using System.Reflection;

namespace SnakesOfLife.Models
{
    public class ParamsChanger
    {
        private readonly Params _currParams;

        public Params GetCurrParams()
        {
            return _currParams.Clone();
        }

        private readonly IEnumerator<PropertyInfo> _propertiesEnumerator;

        private readonly IEnumerable<PropertyInfo> _propertyInfos = typeof(Params).GetProperties();
        private readonly PropertyValidator _propertyValidator;

        public ParamsChanger(Params currParams)
        {
            _currParams = currParams;

            _propertiesEnumerator = _propertyInfos.GetEnumerator();
            ResetCurrentProperty();
            _propertyValidator = new PropertyValidator();
        }

        public bool IncreseCurrentPropertyValue()
        {
            var currentProperty = _propertiesEnumerator.Current;
            var currentValue = (int)currentProperty.GetValue(_currParams);

            var newValue = currentValue + 1;

            if (!_propertyValidator.CanSetPropertyValue(currentProperty.Name, newValue, _currParams))
            {
                return false;
            }

            currentProperty.SetValue(_currParams, newValue);

            return true;
        }

        public bool DecreseCurrentPropertyValue()
        {
            var currentProperty = _propertiesEnumerator.Current;
            var currentValue = (int)currentProperty.GetValue(_currParams);

            var newValue = currentValue - 1;

            if (!_propertyValidator.CanSetPropertyValue(currentProperty.Name, newValue, _currParams))
            {
                return false;
            }

            currentProperty.SetValue(_currParams, newValue);

            return true;
        }

        public bool MoveToNextProperty()
        {
            return _propertiesEnumerator.MoveNext();
        }

        public void ResetCurrentProperty()
        {
            _propertiesEnumerator.Reset();
            _propertiesEnumerator.MoveNext();
        }
    }
}