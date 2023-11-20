using System;
using System.Linq.Expressions;
using System.Reflection;

/*
 * https://expressiontree-tutorial.net/getter-setter-expression
 * 
 * 
 * 
 */



namespace Kenova.Client.Core
{

    public static class ExpressionUtils
    {
        public static Action<TEntity, TProperty> CreateSetter<TEntity, TProperty>(string name) where TEntity : class
        {
            PropertyInfo propertyInfo = typeof(TEntity).GetProperty(name);

            ParameterExpression instance = Expression.Parameter(typeof(TEntity), "instance");
            ParameterExpression propertyValue = Expression.Parameter(typeof(TProperty), "propertyValue");

            var body = Expression.Assign(Expression.Property(instance, name), propertyValue);

            return Expression.Lambda<Action<TEntity, TProperty>>(body, instance, propertyValue).Compile();
        }

        public static Func<TEntity, TProperty> CreateGetter<TEntity, TProperty>(string name) where TEntity : class
        {
            ParameterExpression instance = Expression.Parameter(typeof(TEntity), "instance");
            //Expression.Convert()
            var body = Expression.Property(instance, name);
            // Expression.Call
            return Expression.Lambda<Func<TEntity, TProperty>>(body, instance).Compile();
        }
    }

}