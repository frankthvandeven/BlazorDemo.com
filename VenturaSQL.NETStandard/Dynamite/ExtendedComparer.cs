using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq.Expressions;

namespace VenturaSQL.Dynamite
{
    /// <summary>
    /// A Comparer for Nullable types whose underlying type is not comparable.
    /// </summary>
    /// <typeparam name="T">Underlying type</typeparam>
    /// <remarks>This comparer simply orders items that does not have a value before those that have a value.</remarks>
    public class JustNullNullableComparer<T> : IComparer<T?> where T : struct
    {

        private static JustNullNullableComparer<T> _default;

        private JustNullNullableComparer()
        {

        }

        public static JustNullNullableComparer<T> Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new JustNullNullableComparer<T>();
                }
                return _default;
            }
        }

        #region IComparer<T?> Members

        public int Compare(T? x, T? y)
        {
            if (x.HasValue)
            {
                if (y.HasValue) return 0;
                return 1;
            }
            else
            {
                if (y.HasValue) return -1;
                return 0;
            }
        }

        #endregion

    }

    /// <summary>
    /// A comparer for reference typed properties or fields which does not declare a IComparer interface.  
    /// </summary>
    /// <typeparam name="T">Type to compare (must be a reference type)</typeparam>
    public sealed class LateBoundComparer<T> : IComparer<T> where T : class
    {
        private static LateBoundComparer<T> _default;

        private LateBoundComparer() { }

        public static LateBoundComparer<T> Default
        {
            get 
            {
                if (_default == null)
                {
                    _default = new LateBoundComparer<T>();
                }
                return _default;
            }
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {

            if (x != null)
            {
                if (y != null)
                {
                    IComparable xc = x as IComparable;
                    if (xc != null)
                    {
                        return xc.CompareTo(y);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (y != null) return -1;
                return 0;
            }
        }

        #endregion
    }

    /// <summary>
    /// A "dummy" comparer that regards all values the same.  
    /// </summary>
    /// <typeparam name="T">Type to compare</typeparam>
    public sealed class AlwaysEqualComparer<T> : IComparer<T>
    {
        private AlwaysEqualComparer() { }

        private static AlwaysEqualComparer<T> _default;

        public static AlwaysEqualComparer<T> Default 
        {
            get 
            {
                if (_default == null)
                {
                    _default = new AlwaysEqualComparer<T>();
                }
                return _default;
            }
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            return 0;
        }

        #endregion
    }

    /// <summary>
    /// A comparer that compares Enum instances without the need for boxing.
    /// </summary>
    /// <typeparam name="T">Type to compare (must be an Enum type).</typeparam>
    public sealed class EnumComparer<T> : IComparer<T>
    {
        private EnumComparer() { }

        private static EnumComparer<T> _default;

        private static Comparison<T> _comparison;

        private static Comparison<T> CreateComparison()
        {
            if (typeof(T).IsEnum == false) throw new InvalidOperationException("Cannot create an EnumComparer for a non-enum type."); 
            Type underlyingType = Enum.GetUnderlyingType(typeof(T));
            ParameterExpression x = Expression.Parameter(typeof(T), "x");
            ParameterExpression y = Expression.Parameter(typeof(T), "y");
            Expression body = Expression.Call(Expression.Convert(x,underlyingType), "CompareTo", Type.EmptyTypes, 
                                              Expression.Convert(y,underlyingType));
            return Expression.Lambda<Comparison<T>>(body, x, y).Compile();
        }

        /// <summary>
        /// Gets a Comparison&lt;T&gt; that compares two instances.
        /// </summary>
        public static Comparison<T> Comparison
        {
            get
            {
                if (_comparison == null)
                {
                    _comparison = CreateComparison();
                }
                return _comparison;
            }
        }

        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        public static EnumComparer<T> Default
        {
            get
            {
                if (_default == null)
                {
                    if (_comparison == null)
                    {
                        _comparison = CreateComparison();
                    }
                    _default = new EnumComparer<T>();
                }
                return _default;
            }
        }

        #region IComparer<T> Members

        /// <summary>
        /// Compares to instances.
        /// </summary>
        /// <param name="x">First instance to compare</param>
        /// <param name="y">Second comparand.</param>
        /// <returns></returns>
        public int Compare(T x, T y)
        {
            return _comparison(x, y);
        }

        #endregion
    }

    /// <summary>
    /// A comparer that compares Nullable types where the underlying type is an Enum type.
    /// </summary>
    /// <typeparam name="T">Underlying type to compare.</typeparam>
    public class NullableEnumComparer<T> : IComparer<T?> where T : struct
    {
        private NullableEnumComparer() { }

        private static NullableEnumComparer<T> _default;

        private static Comparison<T> _comparison;

        public static NullableEnumComparer<T> Default
        {
            get
            {
                if (_default == null)
                {
                    _comparison = EnumComparer<T>.Comparison;
                    _default = new NullableEnumComparer<T>();
                }
                return _default;
            }
        }

        #region IComparer<T> Members

        public int Compare(T? x, T? y)
        {
            if (x.HasValue)
            {
                if (y.HasValue)
                {
                    return _comparison(x.GetValueOrDefault(), y.GetValueOrDefault());
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (y.HasValue)
                {
                    return -1;
                }
                return 0;
            }
        }

        #endregion
    }

    /// <summary>
    /// An extended Comparer class that provides comparers that complements the normal System.Collections.Generic.Comparer&lt;T&gt; comparers. 
    /// </summary>
    /// <typeparam name="T">Type to compare.</typeparam>
    /// <remarks>
    /// This class extends the System.Collections.Generic.Comparer&lt;T&gt; by returning optimized versions of comparers for enum
    /// types (including Nullable enum types). For reference types that does not implement any IComparer interface a
    /// LateBoundComparer&lt;T&gt; is returned that defines null values before non-null values and compares non-values based on 
    /// IComparer implementation if they exists. For Nullable types whose underlying type is not comparable a 
    /// JustNullNullableComparer&lt;T&gt; is returned that simple defines those values that do not have a value before values that
    /// does have a value. Finally, for other types of value types that does not implement a IComparable interface a comparer that
    /// regards all values equal are returned.
    /// </remarks>
    public class ExtendedComparer<T>
    {
        static IComparer<T> comparer;

        public static IComparer<T> Default
        {
            get {
                if (comparer == null)
                {
                    comparer = CreateComparer();
                }
                return comparer;
            }
        }

        private static IComparer<T> CreateComparer()
        {
            Type propType = typeof(T);
            Type nullableUnderlyingType = Nullable.GetUnderlyingType(propType);
            if (nullableUnderlyingType != null)
            {
                propType = nullableUnderlyingType;
            }

            if (propType.IsEnum)
            {
                if (nullableUnderlyingType == null)
                {
                    return EnumComparer<T>.Default;
                }
                else
                {
                    return (IComparer<T>)typeof(NullableEnumComparer<>).MakeGenericType(propType).GetProperty("Default").GetGetMethod().Invoke(null, null);
                }
            }

            if (typeof(IComparer).IsAssignableFrom(propType) ||
                 typeof(IComparable<>).MakeGenericType(propType).IsAssignableFrom(propType))
            {
                return Comparer<T>.Default;
            }
            else
            {
                if (propType.IsValueType)
                {
                    if (nullableUnderlyingType != null)
                    {
                        return (IComparer<T>)typeof(JustNullNullableComparer<>).MakeGenericType(nullableUnderlyingType).GetProperty("Default").GetGetMethod().Invoke(null, null);
                    }
                    else
                    {
                        return AlwaysEqualComparer<T>.Default; 
                    }
                }
                else
                {
                    return (IComparer<T>)typeof(LateBoundComparer<>).MakeGenericType(propType).GetProperty("Default").GetGetMethod().Invoke(null, null);
                }
            }
        }
    }
}
