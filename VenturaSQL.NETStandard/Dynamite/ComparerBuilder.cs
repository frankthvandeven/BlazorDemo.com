using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using VenturaSQL.Dynamite.Parsing;

namespace VenturaSQL.Dynamite
{
    /// <summary>
    /// Delegate that sorts a specified sequence in a specific order.
    /// </summary>
    /// <param name="source">Sequence to be sorted.</param>
    /// <returns>An ordered sequence.</returns>
    public delegate IOrderedEnumerable<T> OrderByFunction<T>(IEnumerable<T> source);

    /// <summary>
    /// Helper class that compares object based on a number of field comparisons given as Comparison delegates.
    /// </summary>
    public sealed class TypeComparer<T> : IComparer<T>, System.Collections.IComparer
    {
        private readonly Comparison<T>[] comparisons;

        public TypeComparer(Comparison<T>[] comparisons)
        {
            this.comparisons = comparisons;
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {

            for (int i = 0; i < comparisons.Length; i++)
            {
                int res = comparisons[i](x, y);
                if (res != 0)
                {
                    return res;
                }
            }
            return 0;
        }

        #endregion

        #region IComparer Members

        public int Compare(object x, object y)
        {
            return Compare((T)x, (T)y);
        }

        #endregion
    }

    /// <summary>
    /// Base class of all ComparerBuilder that contains common members and logic.
    /// </summary>
    public class ComparerBuilderBase
    {
        protected class StringListComparer : IEqualityComparer<List<String>>
        {
            #region IEqualityComparer<List<String>> Members

            public bool Equals(List<String> x, List<String> y)
            {
                if (x.Count != y.Count) return false;
                for (int i = 0; i < x.Count; i++)
                {
                    if (x[i].Equals(y[i], StringComparison.OrdinalIgnoreCase) == false)
                    {
                        return false;
                    }
                }
                return true;
            }

            public int GetHashCode(List<String> obj)
            {
                int hash = 0;
                for (int i = 0; i < obj.Count; i++)
                {
                    hash = 31 * hash + StringComparer.OrdinalIgnoreCase.GetHashCode(obj[i]);
                }
                return hash;
            }

            #endregion
        }

        /// <summary>
        /// MethodInfo of String.Compare method used to compare string instances.
        /// </summary>
        protected static readonly MethodInfo stringCompareMethod = new Func<String, String, StringComparison, int>(String.Compare).Method; //  typeof(String).GetMethod("Compare", new Type[] { typeof(String), typeof(String), typeof(StringComparison) });
        
        /// <summary>
        /// MethodInfo of Nullable.Compare used to compare Nullable instances.
        /// </summary>
        protected static readonly MethodInfo nullableGenericCompareMethod = typeof(Nullable).GetMethod("Compare");

        /// <summary>
        /// MethodInfo for IComparable.CompareTo interface method.
        /// </summary>
        protected static readonly MethodInfo nonGenericCompareToMethod = typeof(IComparable).GetMethod("CompareTo");
        
        /// <summary>
        /// MethodInfo of ComparerBuilderBase.Compare method used to compare objects where the comparison cannot be infered at dynamic compile time.
        /// </summary>
        protected static readonly MethodInfo generalCompareMethod = new Func<Object, Object, int>(Compare).Method;
        
        /// <summary>
        /// Helper that returns the implementation of the given interface method for a given source type.
        /// </summary>
        /// <param name="sourceType">Type to search for an implementation in.</param>
        /// <param name="interfaceMethod">Interface method to search for.</param>
        /// <returns>The implementing method in sourceType or null if given interface method not implemented in source class.</returns>
        protected static MethodInfo GetMethodImplementation(Type sourceType, MethodInfo interfaceMethod)
        {
            Type interfaceType = interfaceMethod.ReflectedType;
            if (interfaceType.IsAssignableFrom(sourceType))
            {
                InterfaceMapping map = sourceType.GetInterfaceMap(interfaceMethod.ReflectedType);
                int index = Array.IndexOf(map.InterfaceMethods, interfaceMethod);
                if (index < 0)
                {
                    return null;
                }
                return map.TargetMethods[index];

            }
            return null;

        }

        /// <summary>
        /// Helper to get a member (field or property) with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">Name of member to search for</param>
        /// <returns>MemberInfo representing the field or property.</returns>
        /// <remarks>
        /// Only public properties and fields are included in the search. 
        /// </remarks>
        protected static MemberInfo GetMemberByName(Type type, String name)
        {
            PropertyInfo property = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property != null && property.GetGetMethod() != null)
            {
                return property;
            }
            FieldInfo field = type.GetField(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            return field;
        }

        /// <summary>
        /// Creates an expression that represents a getter for the specified property expression.
        /// </summary>
        /// <paramref name="propParts"/> represents a property expression prop0.prop1.prop2 etc where "prop0" is stored in propParts[0] and 
        /// "prop1" is stored in propParts[1], etc.</param>
        /// <param name="baseExpression">Base expression to retrieve the given property or field from, e.g. a ParameterExpression</param>
        /// <returns>An Expression to retrieve the specified property from baseExpression.</returns>
        public static Expression BuildNullSafeGetter(List<String> propParts, Expression baseExpression)
        {
            if (propParts == null) throw new ArgumentNullException("propParts");
            if (propParts.Count == 0) throw new ArgumentException("propParts must not be empty.");
            if (baseExpression == null) throw new ArgumentNullException("baseExpression");
            return BuildNullSafeGetter(propParts, 0, baseExpression);
        }

        /// <summary>
        /// Helper method to build an expression that represents a getter for the specified property expression (recursively).
        /// </summary>
        /// <param name="propParts">Represents the property to get as a list of property names.</param>
        /// <param name="partIndex">Current index in propParts list to build for.</param>
        /// <param name="baseExpression">Base expression to retrieve the given property or field from, e.g. a ParameterExpression</param>
        /// <returns>An Expression to retrieve the specified property.</returns>
        /// <exception cref="System.ArgumentException">If one of the items in propParts is not recognized as a public field or property.</exception>
        protected static Expression BuildNullSafeGetter(List<String> propParts, int partIndex, Expression baseExpression)
        {
            String memberName = propParts[partIndex];
            
            MemberInfo member = GetMemberByName(baseExpression.Type, memberName);
            if (member == null) throw new ArgumentException("'"+propParts[partIndex]+"' not a public property or field.");

            Expression memberExpr = Expression.MakeMemberAccess(baseExpression, member);

            Type memberType = memberExpr.Type;
            if (partIndex == propParts.Count - 1)
            {
                Type nullableUnderlyingType = Nullable.GetUnderlyingType(memberType);
                if (nullableUnderlyingType != null && nullableUnderlyingType.IsEnum)
                {
                    return Expression.Convert(memberExpr, typeof(Nullable<>).MakeGenericType(Enum.GetUnderlyingType(nullableUnderlyingType)));
                }
                else if (memberType.IsEnum)
                {
                    // Treat enums as its underlying type to avoid boxing when using non-generic IComparable interface.
                    return Expression.Convert(memberExpr, Enum.GetUnderlyingType(memberExpr.Type));
                }
                return memberExpr;
            }
            else
            {

                if (memberType.IsValueType)
                {
                    if (memberType.IsGenericType && memberType.GetGenericTypeDefinition() == typeof(Nullable<>)) // If Nullable
                    {
                        ParameterExpression param = Expression.Parameter(memberType, memberName);
                        Expression childExpr = BuildNullSafeGetter(propParts, partIndex + 1, Expression.Call(param, "GetValueOrDefault", Type.EmptyTypes));
                        Type childType = childExpr.Type;

                        // Lift to nullable if necessary.
                        if (childType.IsValueType && Nullable.GetUnderlyingType(childType) == null)
                        {
                            childType = typeof(Nullable<>).MakeGenericType(childType);
                            childExpr = Expression.Convert(childExpr, childType);
                        }

                        return Expression.Invoke(Expression.Lambda(
                                                        Expression.Condition(Expression.Property(param, "HasValue"),
                                                                             childExpr,
                                                                             Expression.Constant(null, childType)),
                                                        param),
                                                     memberExpr);
                    }
                    else
                    {
                        return BuildNullSafeGetter(propParts, partIndex + 1, memberExpr);
                    }
                }
                else // if reference type
                {
                    ParameterExpression param = Expression.Parameter(memberType, memberName);
                    Expression childExpr = BuildNullSafeGetter(propParts, partIndex + 1, param);

                    Type childType = childExpr.Type;

                    // Lift to nullable if necessary, i.e. if the child expression is a non-nullable type.
                    if (childType.IsValueType && Nullable.GetUnderlyingType(childType) == null)
                    {
                        childType = typeof(Nullable<>).MakeGenericType(childType);
                        childExpr = Expression.Convert(childExpr, childType);
                    }

                    return Expression.Invoke(Expression.Lambda(
                                               Expression.Condition(Expression.Equal(param, Expression.Constant(null)),
                                                                    Expression.Constant(null, childType),
                                                                    childExpr),
                                               param),
                                             memberExpr);

                }
            }

        }

        /// <summary>
        /// Compares two objects whose type was not known at dynamic compile time.
        /// </summary>
        /// <param name="o1">First object to compare</param>
        /// <param name="o2">Second object to compare</param>
        /// <returns>A value less than zero if o1 &lt; o2, zero if o1 = o2, A value greater than zero if o1 &gt; o2.</returns>
        public static int Compare(Object o1, Object o2)
        {
            if (o1 == null)
            {
                if( o2 != null ) return -1;
                return 0;
            }
            else
            {
                if (o2 == null) return 1;
                if (o1 is String && o2 is String)
                {
                    return String.Compare((String)o1, (String)o2, StringComparison.CurrentCultureIgnoreCase);
                }
                IComparable c1 = o1 as IComparable;
                if (c1 != null)
                {
                    return c1.CompareTo(o2);
                }
                return 0;
            }
        }
        
    }

    /// <summary>
    /// Utility class that dynamically create comparer based on property names and implements sort based on these. 
    /// </summary>
    /// <typeparam name="T">Type to be compared and sorted.</typeparam>
    /// <seealso cref="Dynamite.Extensions.ComparerExtensions"/>
    public class ComparerBuilder<T> : ComparerBuilderBase
    {
        #region Private fields

        /// <summary>
        /// Cache of previously created Comparisons where keys represent property name(s) and
        /// values are Comparision delegates corresponding to these properties.
        /// </summary>
        private static readonly ThreadSafeDictionary<List<String>, Comparison<T>> comparisonCache = new ThreadSafeDictionary<List<String>, Comparison<T>>(new StringListComparer());
       
        #endregion

        #region Constructors

        /// <summary>
        /// Private constructor to prevent creating instances of this static class. 
        /// </summary>
        private ComparerBuilder()
        {
        }

        #endregion

        #region Public Constants

        public const String ASC = "ASC";
        public const String ASCENDING = "ASCENDING";
        public const String DESC = "DESC";
        public const String DESCENDING = "DESCENDING";

        #endregion

        #region Public methods
              

        /// <summary>
        /// Builds a new comparer that compares to instances based on properties specified in a sort expression.
        /// </summary>
        /// <param name="sortExpression">A SQL-like sort expression with comma separated property names (and optional direction specifiers) (e.g. "Age DESC, Name")</param>
        /// <returns>A TypeComparer that implements both IComparer&lt;T&gt; and non-generic IComparer interface."/></returns>
        /// <exception cref="System.ArgumentNullExcpetion">If <paramref name="sortExpression"/> is null.</exception>
        /// <exception cref="Dynamite.Parsing.ParserException">If <paramref name="sortExpression"/> is an invalid sort expression.</exception>
        public static TypeComparer<T> CreateTypeComparer(String sortExpression)
        {
            if (sortExpression == null) throw new ArgumentNullException("sortExpression");
            
            List<Comparison<T>> comparisons = GetFieldComparisons(sortExpression);

            TypeComparer<T> mFieldComparer = new TypeComparer<T>(comparisons.ToArray());
            return mFieldComparer;
        }
        
        /// <summary>
        /// Builds a new Comparison delegate that can compare instances based on properties specified in a sort expression
        /// </summary>
        /// <param name="sortExpression">A SQL-like sort expression with comma separated property names (and optional direction specifiers) (e.g. "Age DESC, Name")</param>
        /// <returns>A Comparison delegate based on the given sort criteria.</returns>
        /// <exception cref="System.ArgumentNullExcpetion">If <paramref name="sortExpression"/> is null.</exception>
        /// <exception cref="Dynamite.Parsing.ParserException">If <paramref name="sortExpression"/> is an invalid sort expression.</exception>
        public static Comparison<T> CreateTypeComparison(String sortExpression)
        {
            if (sortExpression == null)
            {
                throw new ArgumentNullException("sortExpression");
            }
            List<Comparison<T>> comparisons = GetFieldComparisons(sortExpression);

            if (comparisons.Count == 1)
            {
                return comparisons[0];
            }
            else
            {
                TypeComparer<T> mFieldComparer = new TypeComparer<T>(comparisons.ToArray());
                return new Comparison<T>(mFieldComparer.Compare);
            }
        }

        

        /// <summary>
        /// Sorts the elements of a queryable sequence based on the given search criteria. 
        /// </summary>
        /// <param name="source">The IQueryable sequence to be sorted.</param>
        /// <param name="sortExpression">A SQL-like sort expression with comma separated property names (and optional direction specifiers) (e.g. "Age DESC, Name.Length")</param>
        /// <returns>A queryable sequence sorted according to the sort expression</returns>
        /// <exception cref="System.ArgumentNullException">source or sortExpression is null.</exception>
        /// <exception cref="Dynamite.Parsing.ParserException">if sortExpression is not properly formatted or contains unrecognized property or field names..</exception>
        public static IOrderedQueryable<T> OrderBy(IQueryable<T> source, String sortExpression)
        {
            if (source == null) throw new ArgumentNullException("source");
            
            if (sortExpression == null) throw new ArgumentNullException("sortExpression");
            
            SimpleTokenizer tokenizer = new SimpleTokenizer(sortExpression);

            IQueryable<T> result = source;

            do
            {
                ParameterExpression param = Expression.Parameter(typeof(T), "o");
                
                // Create (nested) member access expression.
                Expression body = param;
                do
                {
                    String property = tokenizer.ReadIdentity();
                    if (property.Length == 0) throw new ParserException(tokenizer.Position, sortExpression, "Property or field expected.");

                    // Implicitely call Value for Nullable properties/fields.
                    if (Nullable.GetUnderlyingType(body.Type) != null)
                    {
                        body = Expression.Property(body, "Value");
                    }

                    MemberInfo member = GetMemberByName(body.Type, property);
                    if( member == null ) throw new ParserException(tokenizer.Position, sortExpression, property + " not a public property or field.");

                    body = Expression.MakeMemberAccess(body, member);
                    
                } 
                while (tokenizer.AdvanceIfSymbol('.'));

                LambdaExpression keySelectorLambda = Expression.Lambda(body, param);

                bool ascending = true;
                if( tokenizer.AdvanceIfIdent(DESC) || tokenizer.AdvanceIfIdent(DESCENDING) )
                {
                    ascending = false;
                }
                else if( tokenizer.AdvanceIfIdent(ASC) || tokenizer.AdvanceIfIdent(ASCENDING) )
                {
                    ascending = true;
                }
                String queryMethod;
                if( result == source )
                {
                    queryMethod = ascending ? "OrderBy" : "OrderByDescending";
                }
                else
                {
                    queryMethod = ascending ? "ThenBy" : "ThenByDescending";
                }
                
                result = result.Provider.CreateQuery<T>(Expression.Call(typeof(Queryable), queryMethod, 
                                                                         new Type[] { typeof(T), body.Type }, 
                                                                         result.Expression, 
                                                                         Expression.Quote(keySelectorLambda))); 
                
            }
            while( tokenizer.AdvanceIfSymbol(","));
            tokenizer.ExpectEnd();
            return (IOrderedQueryable<T>)result;
        }

        private static readonly ThreadSafeDictionary<String, OrderByFunction<T>> orderByCache = new ThreadSafeDictionary<string, OrderByFunction<T>>(StringComparer.CurrentCultureIgnoreCase);

        /// <summary>
        /// Gets an OrderByFunction&lt;T&gt; that orders an input sequence according to the specified sort expression.
        /// </summary>
        /// <param name="sortExpression">Expression to sort after.</param>
        /// <returns>An OrderByFunction&lt;T&gt; that orders an input sequence according to the specified sort expression</returns>
        /// <exception cref="System.ArgumentNullException">If sortExpression is null.</exception>
        /// <exception cref="Dynamite.Parsing.ParserException">If sortExpression is invalid.</exception>
        public static OrderByFunction<T> GetOrderByFunction(String sortExpression)
        {
            if (sortExpression == null) throw new ArgumentNullException("sortExpression");

            OrderByFunction<T> orderByFunc;
            if (orderByCache.TryGetValue(sortExpression, out orderByFunc) == false)
            {
                orderByFunc = CreateOrderByFunction(sortExpression);
                orderByCache[sortExpression] = orderByFunc;
            }
            return orderByFunc;
        }

        /// <summary>
        /// Creates an OrderByFunction&lt;T&gt; that orders an input sequence according to the specified sort expression.
        /// </summary>
        /// <param name="sortExpression">Expression to sort after.</param>
        /// <returns>An OrderByFunction&lt;T&gt; that orders an input sequence according to the specified sort expression</returns>
        /// <exception cref="System.ArgumentNullException">If sortExpression is null.</exception>
        /// <exception cref="Dynamite.Parsing.ParserException">If sortExpression is invalid.</exception>
        public static OrderByFunction<T> CreateOrderByFunction(String sortExpression)
        {
            if (sortExpression == null) throw new ArgumentNullException("sortExpression");

            SimpleTokenizer parser = new SimpleTokenizer(sortExpression);

            ParameterExpression sourceParam = Expression.Parameter(typeof(IEnumerable<T>), "source");

            Expression body = sourceParam;
            
            bool moreProperties;
            List<String> propertyParts = new List<string>(4);
            do
            {
                do
                {
                    String property = parser.ReadIdentity();
                    if (property.Length == 0) throw new ParserException(parser.Position, sortExpression, "Field or property expected");

                    propertyParts.Add(property);
                }
                while (parser.AdvanceIfSymbol('.'));

                moreProperties = parser.AdvanceIfSymbol(',');

                bool ascending = true;
                if (moreProperties == false)
                {
                    if (parser.AdvanceIfIdent(DESC) || parser.AdvanceIfIdent(DESCENDING))
                    {
                        ascending = false;
                        moreProperties = parser.AdvanceIfSymbol(',');
                    }
                    else if (parser.AdvanceIfIdent(ASC) || parser.AdvanceIfIdent(ASCENDING))
                    {
                        moreProperties = parser.AdvanceIfSymbol(',');
                    }

                }

                ParameterExpression keyParameter = Expression.Parameter(typeof(T),"x");
                Expression keySelectorBody;
                try
                {
                    keySelectorBody = BuildNullSafeGetter(propertyParts, keyParameter);
                }
                catch (ArgumentException ex)
                {
                    throw new ParserException(parser.Position, parser.Expression, ex.Message);
                }
                Expression comparer;
                if (keySelectorBody.Type == typeof(String))
                {
                    comparer = Expression.Call(typeof(StringComparer).GetProperty("CurrentCultureIgnoreCase").GetGetMethod());
                }
                else
                {
                    comparer = Expression.Constant(typeof(ExtendedComparer<>).MakeGenericType(keySelectorBody.Type).GetProperty("Default").GetGetMethod().Invoke(null,null));
                    
                }
                LambdaExpression keySelectorLambda = Expression.Lambda(keySelectorBody, keyParameter);
                String queryMethodName;
                if (body == sourceParam)
                {
                    queryMethodName = ascending ? "OrderBy" : "OrderByDescending";
                }
                else
                {
                    queryMethodName = ascending ? "ThenBy" : "ThenByDescending";
                }

                body = Expression.Call(typeof(Enumerable), queryMethodName, new Type[] { typeof(T), keySelectorBody.Type }, body, keySelectorLambda, comparer);
                
                propertyParts.Clear();

            }
            while (moreProperties);
            parser.ExpectEnd();

            Expression<OrderByFunction<T>> orderByFuncExpr = Expression.Lambda<OrderByFunction<T>>(body, sourceParam);
            OrderByFunction<T> orderByFunc = orderByFuncExpr.Compile();

            return orderByFunc;
        }
        
        /// <summary>
        /// Gets a dynamically created Comparison delegate that compare instances based on a single named property 
        /// </summary>
        /// <param name="propertyName">Name of property or field to base comparison on</param>
        /// <param name="ascending">true to search in ascending order, false to sort in descending order</param>
        /// <returns>A Comparison delegate for the given property.</returns>
        /// <remarks>This class caches dynamically created Comparison delegates for best performance.</remarks>
        /// <exception cref="System.ArgumentNullException">if propertyName is null.</exception>
        /// <exception cref="Systen.ArgumentException">If propertyName is not recognized as a public property or field.</exception>
        public static Comparison<T> GetPropertyComparison(String propertyName, bool ascending)
        {
            if( propertyName == null ) throw new ArgumentNullException("propertyName");
            List<String> propParts = new List<string>(1);
            propParts.Add(propertyName);
            return GetPropertyComparison(propParts, ascending);
        }

        /// <summary>
        /// Gets a dynamically created Comparison delegate that compare instances based on a compound property expression
        /// </summary>
        /// <param name="properties">List of property names representing the property or fields to get. E.g. prop0.prop1.prop2 is represented as a list with items "prop0", "prop1" and "prop2".</param>
        /// <param name="ascending">true to search in ascending order, false to sort in descending order</param>
        /// <returns>A Comparison delegate for the given property.</returns>
        /// <exception cref="System.ArgumentNullException">if properties is null.</exception>
        /// <exception cref="Systen.ArgumentException">If any of the strings in properties is not recognized as a public property or field.</exception>
        public static Comparison<T> GetPropertyComparison(List<String> properties, bool ascending)
        {
            if (properties == null) throw new ArgumentNullException("properties");
            Comparison<T> comparison;
                            
            if (comparisonCache.TryGetValue(properties, out comparison) == false)
            {
                comparison = CreatePropertyComparison(properties, true);
                comparisonCache[new List<String>(properties)] = comparison;
            }               
           
            if (ascending)
            {
                return comparison;
            }
            else
            {
                return (Comparison<T>)delegate(T x, T y) { return -comparison(x, y); };
            }

        }

        /// <summary>
        /// Dynamically creates a Comparison delegate that compare instances based on a single named property or field.
        /// </summary>
        /// <param name="propertyName">Name of public property or field to base comparison on.</param>
        /// <param name="ascending">true to search in ascending order, false to sort in descending order</param>
        /// <returns>A Comparision delegate for the given property.</returns>
        /// <remarks>For higher performance, use GetPropertyComparer, which caches Comparisons once created.</remarks>
        /// <exception cref="System.ArgumentNullException">if propertyName is null.</exception>
        /// <exception cref="Systen.ArgumentException">If propertyName is not recognized as a public property or field.</exception>
        public static Comparison<T> CreatePropertyComparison(String propertyName, bool ascending)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");
            List<String> propParts = new List<string>(1);
            propParts.Add(propertyName);
            return CreatePropertyComparison(propParts, ascending);
        }

        /// <summary>
        /// Dynamically creates Comparison delegate that compare instances based on a compound property expression
        /// </summary>
        /// <param name="properties">List of property names representing the property or field expression to get. E.g. prop0.prop1.prop2 is represented as a list with items "prop0", "prop1" and "prop2".</param>
        /// <param name="ascending">true to search in ascending order, false to sort in descending order</param>
        /// <returns>A Comparison delegate for the given property.</returns>
        /// <remarks>For higher performance in repetitive calls use GetPropertyComparison, which caches results, instead.</remarks>
        /// <exception cref="System.ArgumentNullException">if properties is null.</exception>
        /// <exception cref="Systen.ArgumentException">If any of the strings in properties is not recognized as a public property or field.</exception>
        public static Comparison<T> CreatePropertyComparison(List<String> propertyExpression, bool ascending)
        {
            Expression<Comparison<T>> compareExpression = CreatePropertyComparisonExpression(propertyExpression, ascending);
            return compareExpression.Compile();
        }

        /// <summary>
        /// Dynamically creates a lambda expression representing a Comparison delegate that compare instances based on a single named property expression
        /// </summary>
        /// <param name="properties">List of property names representing the property expression to get. E.g. prop0.prop1.prop2 is represented as a list with items "prop0", "prop1" and "prop2".</param>
        /// <param name="ascending">true to search in ascending order, false to sort in descending order</param>
        /// <returns>A Comparison delegate expression for the given property.</returns>
        /// <exception cref="System.ArgumentNullException">if properties is null.</exception>
        /// <exception cref="Systen.ArgumentException">If any of the strings in properties is not recognized as a public property or field.</exception>
        public static Expression<Comparison<T>> CreatePropertyComparisonExpression(List<String> properties, bool ascending)
        {
            if (properties == null) throw new ArgumentNullException("properties");

            Type t = typeof(T);

            ParameterExpression param1 = ParameterExpression.Parameter(t, "o1");
            ParameterExpression param2 = ParameterExpression.Parameter(t, "o2");
            Expression body = null;

            Expression getProp1 = BuildNullSafeGetter(properties, 0, param1);
            Expression getProp2 = BuildNullSafeGetter(properties, 0, param2);

            Type propType = getProp1.Type;
            if (propType == typeof(String))
            {
                // Call String.Compare(o1,o2,StringComparison.CurrentCultureIgnoreCase)
                body = Expression.Call(stringCompareMethod, getProp1, getProp2, Expression.Constant(StringComparison.CurrentCultureIgnoreCase, typeof(StringComparison)));
            }
            else
            {
                Type nullableUnderlyingType = Nullable.GetUnderlyingType(propType);
                if( nullableUnderlyingType != null )
                {
                    propType = nullableUnderlyingType;
                    // In case of a Nullable<enum-type> cast to underlying type (see below) and use generic IComparable<T> to avoid boxing.
                    if (propType.IsEnum)
                    {
                        propType = Enum.GetUnderlyingType(propType);
                    }
                }
                // Try typed CompareTo (defined in IComparable<T> generic interface type) first.
                MethodInfo compareToMethod = null;
                Type genericIComparableType = typeof(IComparable<>).MakeGenericType(propType);
                if (genericIComparableType.IsAssignableFrom(propType))
                {
                    compareToMethod = genericIComparableType.GetMethod("CompareTo");
                }
                else
                {
                    // Then try non-generic IComparable interface.
                    if (typeof(IComparable).IsAssignableFrom(propType))
                    {
                        compareToMethod = nonGenericCompareToMethod;
                    }
                    else
                    {
                        if (propType.IsValueType)
                        {
                            // If no comparison method found, all values are considered equal
                            body = Expression.Constant(0, typeof(int));
                        }
                        else
                        {
                            // Call ComparerBuilderBase.Compare(object,object) 
                            body = Expression.Call(generalCompareMethod, 
                                                   Expression.Convert(getProp1, typeof(Object)), 
                                                   Expression.Convert(getProp2, typeof(Object)));

                        }
                     }
                }
                if (compareToMethod != null)
                {
                    if (nullableUnderlyingType != null)  // If a Nullable type.
                    {
                        body = Expression.Call(typeof(Nullable), "Compare", new Type[] { nullableUnderlyingType }, getProp1, getProp2);
                        
                    }
                    else if (propType.IsValueType)
                    {
                        // No null-checks is neccessary, simply call CompareTo method, boxing argument if calling non-generic IComparable.CompareTo(object) 
                        body = Expression.Call(getProp1, compareToMethod, compareToMethod == nonGenericCompareToMethod ? (Expression)Expression.Convert(getProp2, typeof(Object)) : getProp2);
                    }
                    else
                    {
                        // Reference type: Check for null values before calling CompareTo method.
                        ParameterExpression p1 = Expression.Parameter(getProp1.Type, "p1");
                        ParameterExpression p2 = Expression.Parameter(getProp1.Type, "p2");
                        body = Expression.Invoke(Expression.Lambda(Expression.Condition(Expression.NotEqual(p1, Expression.Constant(null)),
                                                                                     Expression.Condition(Expression.NotEqual(p2, Expression.Constant(null)),
                                                                                     Expression.Call(p1, compareToMethod, compareToMethod == nonGenericCompareToMethod ? (Expression)Expression.Convert(p2, typeof(Object)) : p2),
                                                                                         Expression.Constant(1, typeof(int))
                                                                                     ),
                                                                                     Expression.Condition(Expression.NotEqual(p2, Expression.Constant(null)),
                                                                                         Expression.Constant(-1, typeof(int)),
                                                                                         Expression.Constant(0, typeof(int))
                                                                                     )
                                                                                  ), p1, p2), getProp1, getProp2);
                    }
                }
            }
            if (ascending == false)
            {
                body = Expression.Negate(body);
            }
            return Expression.Lambda<Comparison<T>>(body, param1, param2);
        }


        #endregion

        #region Private helpers

        /// <summary>
        /// Private helper to parse a sort expression string and create Comparison delegates based on each property.
        /// </summary>
        /// <param name="sortExpression">Sort expression to parse.</param>
        /// <returns>List of Comparison delegates, one for each property.</returns>
        private static List<Comparison<T>> GetFieldComparisons(String sortExpression)
        {
            SimpleTokenizer parser = new SimpleTokenizer(sortExpression);
            List<Comparison<T>> comparisons = new List<Comparison<T>>(4);
            List<String> propertyParts = new List<string>(4);
            Boolean moreProperties;
            do
            {
                do
                {
                    String property = parser.ReadIdentity();
                    if (property.Length == 0) throw new ParserException(parser.Position, sortExpression, "Field or property expected");

                    propertyParts.Add(property);
                }
                while (parser.AdvanceIfSymbol('.'));
                
                moreProperties = parser.AdvanceIfSymbol(',');
                
                bool ascending = true;
                if (moreProperties == false)
                {
                    if (parser.AdvanceIfIdent(DESC) || parser.AdvanceIfIdent(DESCENDING))
                    {
                        ascending = false;
                        moreProperties = parser.AdvanceIfSymbol(',');
                    }
                    else if (parser.AdvanceIfIdent(ASC) || parser.AdvanceIfIdent(ASCENDING))
                    {
                        moreProperties = parser.AdvanceIfSymbol(',');
                    }
                    
                }

                try
                {
                    comparisons.Add(GetPropertyComparison(propertyParts, ascending));
                }
                catch (ArgumentException ex)
                {
                    throw new ParserException(parser.Position, parser.Expression, ex.Message);
                }
                propertyParts.Clear();

            }
            while (moreProperties);
            parser.ExpectEnd();
            return comparisons;
        }

        #endregion

    }

    
}
