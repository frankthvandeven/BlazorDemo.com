using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Kenova.Client.Util
{
    public static class LambdaHelper
    {

        public static string GetMemberName(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)expression).Member.Name;
                case ExpressionType.Convert:
                    return GetMemberName(((UnaryExpression)expression).Operand);
                default:
                    throw new NotSupportedException(expression.NodeType.ToString());
            }
        }

        /// <summary>
        /// var myCustomerInstance = new Customer();
        /// myCustomerInstance.SetPropertyValue(item, c => c.Title, "Mr"); 
        /// </summary>
        public static void SetPropertyValue<T, TValue>(T target, Expression<Func<T, TValue>> memberLambda, TValue value)
        {
            var memberSelectorExpression = memberLambda.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(target, value, null);
                }
            }
        }

        /// <summary>
        /// Returns a Func that will return the property or field value converted to an object.<br/>
        /// <br />
        /// var getname = GetPropertySelector&lt;Person&gt;("Name");<br/>
        /// string n = getname(new Person());
        /// </summary>
        public static Func<T, object> GetPropertySelector<T>(string propertyName)
        {
            var arg = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(arg, propertyName); // was Expression.Property()

            //return the property as object
            var conv = Expression.Convert(property, typeof(object));
            var exp = Expression.Lambda<Func<T, object>>(conv, new ParameterExpression[] { arg });
            var func = exp.Compile();

            return func;
        }

    }
}

// Another setter.
//ParameterExpression parameterExpression1 = Expression.Parameter(typeof(Object), "value");
//this._valueSetter = Expression.Lambda<Action<object, object>>(Expression.Assign(propertyPathExpression, Expression.Convert(parameterExpression1, propertyPathExpression.Type)), new ParameterExpression[] { parameterExpression, parameterExpression1 }).Compile();
