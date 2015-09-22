using System;

namespace Logic.Models
{
    public class ParamsCreator
    {
        private readonly Random _random;
        private readonly PropertyValidator _propertyValidator;

        public ParamsCreator(Random random)
        {
            _random = random;
            _propertyValidator = new PropertyValidator();
        }

        public Params Create()
        {
            var newParams = new Params();

            var propertyInfos = newParams.GetType().GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                int randValue;

                do
                {
                    randValue = _random.Next(1, 100);
                } while (!_propertyValidator.CanSetPropertyValue(propertyInfo.Name, randValue, newParams));

                propertyInfo.SetValue(newParams, randValue);
            }

            return newParams;
        }
    }
}