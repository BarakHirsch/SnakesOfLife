using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SnakesOfLife.Models
{
    public class PropertyValidator
    {
        public bool CanSetPropertyValue(string propertyName, int currentValue, Params currParams)
        {
            if (GetName(x => x.SnakeLengthToStop) == propertyName)
            {
                return currParams.SnakeLengthForSplit / 2 >= currentValue;
            }
            
            if (GetName(x => x.SnakeLengthForSplit) == propertyName)
            {
                return currParams.SnakeLengthToStop * 2 < currentValue && 2 <= currentValue;
            }

            return currentValue >= 1 && currentValue < 1000;
        }

        public string GetName(Expression<Func<Params, int>> func)
        {
            return GetMemberInfo(func).Name;
        }

        public static MemberInfo GetMemberInfo<T, U>(Expression<Func<T, U>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member != null)
                return member.Member;

            throw new ArgumentException("Expression is not a member access", "expression");
        }
    }
}