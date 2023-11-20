using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Kenova.Client.Core
{
    internal static class ExpressionEx
    {
        public static bool CanAssign(this Expression exp)
        {
            MemberExpression memberExpression = exp as MemberExpression;
            if (memberExpression != null)
            {
                PropertyInfo member = memberExpression.Member as PropertyInfo;
                if (member != null)
                {
                    return member.CanWrite;
                }
            }
            return false;
        }

        public static Expression Convert(Expression exp, Type type)
        {
            Expression expression = exp;
            if (exp.Type != type)
            {
                try
                {
                    if (!type.IsNullableType() || !(type.GetNonNullableType() == exp.Type))
                    {
                        expression = Expression.Call(typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(Object), typeof(Type), typeof(IFormatProvider) }), new Expression[] { exp, Expression.Constant(type, typeof(Type)), Expression.Constant(null, typeof(IFormatProvider)) });
                        expression = Expression.Convert(expression, type);
                    }
                    else
                    {
                        expression = Expression.Convert(expression, type);
                    }
                }
                catch
                {
                }
            }
            return expression;
        }

        public static string GetPropertyPath<T, P>(Expression<Func<T, P>> expression)
        {
            IList<string> strs = new List<string>();
            Expression body = expression.Body;
            while (body.NodeType != ExpressionType.Parameter)
            {
                ExpressionType nodeType = body.NodeType;
                if (nodeType == ExpressionType.Call)
                {
                    MethodCallExpression methodCallExpression = (MethodCallExpression)body;
                    if (methodCallExpression.Method.Name != "get_Item")
                    {
                        throw new InvalidOperationException(String.Concat("The member '", methodCallExpression.Method.Name, "' is a method call but a Property or Field was expected."));
                    }
                    strs.Add(String.Concat("[", methodCallExpression.Arguments.First<Expression>().ToString(), "]"));
                    body = methodCallExpression.Object;
                }
                else
                {
                    if (nodeType != ExpressionType.Convert && nodeType != ExpressionType.MemberAccess)
                    {
                        ExpressionType expressionType = body.NodeType;
                        throw new InvalidOperationException(String.Concat("The expression NodeType '", expressionType.ToString(), "' is not supported, expected MemberAccess, Convert, or Call."));
                    }
                    MemberExpression memberExpression = (body.NodeType == ExpressionType.MemberAccess ? (MemberExpression)body : (MemberExpression)((UnaryExpression)body).Operand);
                    if (memberExpression.Member.MemberType != MemberTypes.Property && memberExpression.Member.MemberType != MemberTypes.Field)
                    {
                        String[] name = new String[] { "The member '", memberExpression.Member.Name, "' is a ", null, null };
                        name[3] = memberExpression.Member.MemberType.ToString();
                        name[4] = " but a Property or Field is expected.";
                        throw new InvalidOperationException(String.Concat(name));
                    }
                    strs.Add(memberExpression.Member.Name);
                    body = memberExpression.Expression;
                }
            }
            return String.Join(".", strs.Reverse<string>().ToArray<string>());
        }

        public static Expression GetPropertyPathExpression(Expression param, string propertyPath)
        {
            if (String.IsNullOrWhiteSpace(propertyPath))
            {
                return param;
            }
            Expression propertyPathExpression = param;
            MatchCollection matchCollections = ObjectEx.PropertyPathRegEx.Matches(propertyPath);
            if (matchCollections.Count > 1)
            {
                Match item = matchCollections[0];
                string value = item.Value;
                string str = propertyPath.Substring(item.Index + item.Length);
                if (str.StartsWith("."))
                {
                    str = str.Substring(1);
                }
                propertyPathExpression = ExpressionEx.GetPropertyPathExpression(ExpressionEx.GetPropertyPathExpression(propertyPathExpression, value), str);
            }
            else if (propertyPath.Contains("["))
            {
                string str1 = propertyPath.Substring(0, propertyPath.IndexOf('['));
                object obj = propertyPath.Substring(propertyPath.IndexOf('[') + 1, propertyPath.IndexOf(']') - propertyPath.IndexOf('[') - 1);
                PropertyInfo indexerProperty = ObjectEx.GetIndexerProperty(param.Type, str1, ref obj);
                if (indexerProperty == null)
                {
                    propertyPathExpression = ExpressionEx.GetPropertyPathExpression(Expression.Property(propertyPathExpression, str1), propertyPath.Substring(propertyPath.IndexOf('['), propertyPath.IndexOf(']') - propertyPath.IndexOf('[') + 1));
                }
                else
                {
                    propertyPathExpression = Expression.Call(propertyPathExpression, indexerProperty.GetMethod, new Expression[] { Expression.Constant(obj) });
                }
            }
            else if (!propertyPathExpression.Type.IsInterfaceType())
            {
                propertyPathExpression = Expression.Property(propertyPathExpression, propertyPath);
            }
            else
            {
                Type type = TypeEx.GetImplementedInterfaces(propertyPathExpression.Type, true).FirstOrDefault<Type>((Type i) => i.GetProperty(propertyPath) != null);
                propertyPathExpression = Expression.Property(propertyPathExpression, type, propertyPath);
            }
            return propertyPathExpression;
        }
    }
}