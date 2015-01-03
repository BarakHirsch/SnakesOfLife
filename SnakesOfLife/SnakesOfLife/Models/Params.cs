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

        public ParamsChanger(Params currParams)
        {
            _currParams = currParams;

            _propertiesEnumerator = _propertyInfos.GetEnumerator();
            ResetCurrentProperty();
        }

        public bool IncreseCurrentPropertyValue()
        {
            var currentProperty = _propertiesEnumerator.Current;
            var currentValue = (int)currentProperty.GetValue(_currParams);

            if (CanIncreseCurrentPropertyValue(currentProperty.Name, currentValue))
            {
                currentProperty.SetValue(_currParams, currentValue + 1);

                return true;
            }

            return false;
        }

        public bool DecreseCurrentPropertyValue()
        {
            var currentProperty = _propertiesEnumerator.Current;
            var currentValue = (int)currentProperty.GetValue(_currParams);

            if (CanDecreseCurrentPropertyValue(currentProperty.Name, currentValue))
            {
                currentProperty.SetValue(_currParams, currentValue - 1);

                return true;
            }

            return false;
        }

        private bool CanIncreseCurrentPropertyValue(string propertyName, int currentValue)
        {
            //TODO:
            return currentValue < 100;
        }

        public bool CanDecreseCurrentPropertyValue(string propertyName, int currentValue)
        {
            //TODO:
            return currentValue > 1;
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

    public class Params
    {
        public int NeededAliveNeighborsTurnsToGrow { get; set; }

        public int SnakeCellsForGrow { get; set; }

        public int SnakeLengthForSplit { get; set; }

        public int SnakeLengthToStop { get; set; }

        public int SnakeTurnToDie { get; set; }

        public int SnakeTurnsToShrink { get; set; }

        public Params Clone()
        {
            return (Params) MemberwiseClone();
        }
    }
}