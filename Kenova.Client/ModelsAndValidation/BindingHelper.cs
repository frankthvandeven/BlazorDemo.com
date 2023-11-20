using System;
using System.Linq.Expressions;

namespace Kenova.Client.Components
{
    public static class BindingHelper
    {
        public static string GetFieldName(Expression<Func<object>> expr)
        {
            var lmda = expr as LambdaExpression;
            return fieldNameFromLambda(lmda);
        }

        public static string GetFieldName<TModel>(Expression<Func<TModel, object>> expr) where TModel : ModelBase
        {
            var lmda = expr as LambdaExpression;
            return fieldNameFromLambda(lmda);
        }

        private static string fieldNameFromLambda(LambdaExpression lmda)
        {
            var accessorBody = lmda.Body;

            // Unwrap casts to object
            if (accessorBody is UnaryExpression unaryExpression
                && unaryExpression.NodeType == ExpressionType.Convert
                && unaryExpression.Type == typeof(object))
            {
                accessorBody = unaryExpression.Operand;
            }

            if (!(accessorBody is MemberExpression memberExpression))
            {
                throw new ArgumentException($"The provided expression contains a {accessorBody.GetType().Name} which is not supported. {nameof(GetFieldName)} only supports simple member accessors (fields, properties) of an object.");
            }

            // Identify the field name. We don't mind whether it's a property or field, or even something else.
            return memberExpression.Member.Name;
        }

        public static void ParseAccessor<T>(Expression<Func<T>> accessor, out object model, out string fieldName)
        {
            var accessorBody = accessor.Body;

            // Unwrap casts to object
            if (accessorBody is UnaryExpression unaryExpression
                && unaryExpression.NodeType == ExpressionType.Convert
                && unaryExpression.Type == typeof(object))
            {
                accessorBody = unaryExpression.Operand;
            }

            if (!(accessorBody is MemberExpression memberExpression))
            {
                throw new ArgumentException($"The provided expression contains a {accessorBody.GetType().Name} which is not supported. {nameof(ParseAccessor)} only supports simple member accessors (fields, properties) of an object.");
            }

            // Identify the field name. We don't mind whether it's a property or field, or even something else.
            fieldName = memberExpression.Member.Name;

            // Get a reference to the model object
            // i.e., given an value like "(something).MemberName", determine the runtime value of "(something)",
            if (memberExpression.Expression is ConstantExpression constantExpression)
            {
                if (constantExpression.Value is null)
                {
                    throw new ArgumentException("The provided expression must evaluate to a non-null value.");
                }
                model = constantExpression.Value;
            }
            else if (memberExpression.Expression != null)
            {
                // It would be great to cache this somehow, but it's unclear there's a reasonable way to do
                // so, given that it embeds captured values such as "this". We could consider special-casing
                // for "() => something.Member" and building a cache keyed by "something.GetType()" with values
                // of type Func<object, object> so we can cheaply map from "something" to "something.Member".
                var modelLambda = Expression.Lambda(memberExpression.Expression);
                var modelLambdaCompiled = (Func<object>)modelLambda.Compile();
                var result = modelLambdaCompiled();
                if (result is null)
                {
                    throw new ArgumentException("The provided expression must evaluate to a non-null value.");
                }
                model = result;
            }
            else
            {
                throw new ArgumentException($"The provided expression contains a {accessorBody.GetType().Name} which is not supported. {nameof(ParseAccessor)} only supports simple member accessors (fields, properties) of an object.");
            }
        }


        /// <summary>
        /// Creates an Action to call to set value.
        /// For example: var setvalue = ModelHelper.ValueSetter(() => Model.Address.MoreData.MoreDepthString)
        /// </summary>
        public static Action<TValue> CreateValueSetter<TValue>(Expression<Func<TValue>> fieldExpression)
        {
            //Expression exp = fieldExpression.Body;
            Expression exp = fieldExpression.Body is UnaryExpression ? ((UnaryExpression)fieldExpression.Body).Operand : fieldExpression.Body;

            var param = Expression.Parameter(typeof(TValue), "value");

            var conv = Expression.Convert(param, exp.Type);

            var binaryExpr = Expression.Assign(exp, conv);

            Expression<Action<TValue>> setterExpr = Expression.Lambda<Action<TValue>>(binaryExpr, param);

            return setterExpr.Compile();

            //Previous solution without Convert:
            //Expression exp = fieldExpression.Body;
            //var param = Expression.Parameter(typeof(TValue), "value");
            //var binaryExpr = Expression.Assign(exp, param);
            //Expression<Action<TValue>> setterExpr = Expression.Lambda<Action<TValue>>(binaryExpr, param);
            //return setterExpr.Compile();
        }

    }
}

/* 
 * This is 'old' bindinghelper code. Do not discard it yet.
 * This code supports finding a model in nested syntax like "Model.CustomerData.SelectedItem"

namespace Kenova.Client.Components
{
    public static class BindingHelper2
    {
        /// <summary>
        /// Extract the object that was inherited from KenovaModelBase from the expression.
        /// </summary>
        public static ModelBase GetModel(Expression expr)
        {
            if (expr.NodeType != ExpressionType.Lambda)
                throw new ArgumentOutOfRangeException("expr", "Must be a Lambda expression");

            ModelBase model = null;

            var lmda = expr as LambdaExpression;

            SingleNode1(lmda.Body, ref model);

            return model;
        }

        private static void SingleNode1(Expression expr, ref ModelBase model)
        {
            if (expr.NodeType == ExpressionType.Convert)
            {
                var ure = expr as UnaryExpression;
                SingleNode1(ure.Operand, ref model);
            }
            else if (expr.NodeType == ExpressionType.MemberAccess)
            {
                // add
                var mex = expr as MemberExpression;
                bool inherited = InheritedFromModelBase(mex.Type);

                if (inherited)
                {
                    model = extractModel(mex);
                    return;
                }

                SingleNode1(mex.Expression, ref model);
            }
            else if (expr.NodeType == ExpressionType.Parameter)
            {
                var pex = expr as ParameterExpression;

                bool inherited = InheritedFromModelBase(pex.Type);

                if (inherited == true)
                {
                    model = extractModel(pex);
                    return;
                }


                return;
            }
            //else if (expr.NodeType == ExpressionType.Constant)
            //{
            //    var cns_expr = expr as ConstantExpression;
            //    object o = cns_expr.Value;
            //    if (o is MainWindow) { }
            //}
            else
            {
                //throw new Exception($"The provided expression contains a nodetype {expr.NodeType} which is not supported. {nameof(ModelHelper.GetMemberPath)} only supports simple member accessors (fields, properties) of an object.");
            }

        }

        private static ModelBase extractModel(Expression expr)
        {
            var modelLambda = Expression.Lambda(expr); // .Expression
            var modelLambdaCompiled = (Func<ModelBase>)modelLambda.Compile();

            var result = modelLambdaCompiled();

            if (result is null)
                throw new ArgumentException("The provided expression must evaluate to a non-null value.");

            return result;
        }

        /// <summary>
        /// This method finds the BaseModel in the expression, and returns everthing after the model
        /// as the member path.
        /// 
        /// For example: Model.Data.Car.Color returns "Data.Car.Color", Model.Filter returns "Filter"
        /// </summary>
        public static string GetMemberPath(Expression expr)
        {
            if (expr.NodeType != ExpressionType.Lambda)
                throw new ArgumentOutOfRangeException("expr", "Must be a Lambda expression");

            var sb = new StringBuilder(200);

            var lmda = expr as LambdaExpression;

            SingleNode2(lmda.Body, sb);

            if (KenovaClientConfig.Diagnostics) Console.WriteLine($"Got MemberPath {sb}");

            return sb.ToString();
        }

        private static void SingleNode2(Expression expr, StringBuilder sb)
        {
            if (expr.NodeType == ExpressionType.Convert)
            {
                var ure = expr as UnaryExpression;
                SingleNode2(ure.Operand, sb);
            }
            else if (expr.NodeType == ExpressionType.MemberAccess)
            {
                var mex = expr as MemberExpression;
                bool inherited = InheritedFromModelBase(mex.Type);

                if (inherited)
                    return;

                if (sb.Length > 0)
                    sb.Insert(0, ".");

                sb.Insert(0, mex.Member.Name);

                SingleNode2(mex.Expression, sb);
            }
            else if (expr.NodeType == ExpressionType.Parameter)
            {
                var pex = expr as ParameterExpression;

                bool inherited = InheritedFromModelBase(pex.Type);

                if (inherited == false)
                    throw new Exception("Parameter node must be of type ModelBase");

                return;
            }
            //else if (expr.NodeType == ExpressionType.Constant)
            //{
            //    var cns_expr = expr as ConstantExpression;
            //    object o = cns_expr.Value;
            //    if (o is MainWindow) { }
            //}
            else
            {
                //throw new Exception($"The provided expression contains a nodetype {expr.NodeType} which is not supported. {nameof(ModelHelper.GetMemberPath)} only supports simple member accessors (fields, properties) of an object.");
            }

        }

        /// <summary>
        /// Returns true when the Type has ModelBase as an ancestor.
        /// </summary>
        public static bool InheritedFromModelBase(Type typ)
        {
            if (typ.BaseType == null)
                return false;

            if (typ.BaseType == typeof(ModelBase))
                return true;

            return InheritedFromModelBase(typ.BaseType);
        }

    }
}

*/