
// BUILD ACTION SET TO NONE!!


using System;
using System.Collections.Generic;
using System.Linq;

namespace VenturaSQL.Dynamite.Extensions
{
    /// <summary>
    /// Class containing extension methods for comparison and sorting using textual sort expressions.
    /// </summary>
    public static class ComparerExtensions
    {
        /// <summary>
        /// Sort the elements of a list according to the specified sort expression.
        /// </summary>
        /// <typeparam name="T">List item type</typeparam>
        /// <param name="list">List to be sorted</param>
        /// <param name="sortExpression">A SQL-like sort expression with comma separated property names (and optional direction specifiers) (e.g. "Age DESC, Name")</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="list"/> is null or <paramref name="sortExpression"/> is null</exception>
        /// <exception cref="Dynamite.Parsing.ParserException">If <paramref name="sortExpression"/> is an invalid sort expression.</exception>
        public static void Sort<T>(this List<T> list, String sortExpression)
        {
            Comparison<T> comparison = ComparerBuilder<T>.CreateTypeComparison(sortExpression);
            list.Sort(comparison);
        }

        /// <summary>
        /// Sorts the elements in a range of elements in a list using the specified sort expression 
        /// </summary>
        /// <typeparam name="T">List item type</typeparam>
        /// <param name="list">List to be sorted</param>
        /// <param name="index">Index of first item to be sorted</param>
        /// <param name="count">Number of items to be sorted.</param>
        /// <param name="sortExpression">A SQL-like sort expression with comma separated property names (and optional direction specifiers) (e.g. "Age DESC, Name")</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="list"/> is null or <paramref name="sortExpression"/> is null</exception>
        /// <exception cref="Dynamite.Parsing.ParserException">If <paramref name="sortExpression"/> is an invalid sort expression.</exception>
        /// <exception cref="System.ArgumentOutOfRangeExcpetion"><paramref name="index"/> not a valid list index or <paramref name="count"/> out of range.</exception>
        public static void Sort<T>(this List<T> list, int index, int count, String sortExpression)
        {
            IComparer<T> comparer = ComparerBuilder<T>.CreateTypeComparer(sortExpression);
            list.Sort(index, count, comparer);
        }

        /// <summary>
        /// Sort the elements in an array according specified sort expression.
        /// </summary>
        /// <typeparam name="T">Type of array items</typeparam>
        /// <param name="array">Array to be sorted</param>
        /// <param name="sortExpression">A SQL-like sort expression with comma separated property names (and optional direction specifiers) (e.g. "Age DESC, Name")</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="array"/> is null or <paramref name="sortExpression"/> is null</exception>
        /// <exception cref="Dynamite.Parsing.ParserException">If <paramref name="sortExpression"/> is an invalid sort expression.</exception>

        public static void Sort<T>(this T[] array, String sortExpression)
        {
            if (array == null)
            {
                throw new ArgumentNullException("sortExpression");
            }
            if (sortExpression == null)
            {
                throw new ArgumentNullException("sortExpression");
            }
            Comparison<T> comparison = ComparerBuilder<T>.CreateTypeComparison(sortExpression);
            Array.Sort(array, comparison);
            
        }

        /// <summary>
        /// Sorts the elements in a range of elements in an array using the specified sort expression.
        /// </summary>
        /// <typeparam name="T">Type of elements in array</typeparam>
        /// <param name="array">Array to be sorted.</param>
        /// <param name="index">Index of first element to be sorted</param>
        /// <param name="length">Number of elements to be sorted.</param>
        /// <param name="sortExpression">A SQL-like sort expression with comma separated property names (and optional direction specifiers) (e.g. "Age DESC, Name")</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="array"/> is null or <paramref name="sortExpression"/> is null</exception>
        /// <exception cref="Dynamite.Parsing.ParserException">If <paramref name="sortExpression"/> is an invalid sort expression.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> or <paramref name="length" /> out of range.</exception>
        public static void Sort<T>(this T[] array, int index, int length, String sortExpression)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (sortExpression == null)
            {
                throw new ArgumentNullException("sortExpression");
            }
            IComparer<T> comparer = ComparerBuilder<T>.CreateTypeComparer(sortExpression);
            Array.Sort(array, index, length, comparer);
        }

        /// <summary>
        /// Tests whether the elements in the given sequence is sorted according to the specified fields.
        /// </summary>
        /// <typeparam name="T">Type of items in sequence</typeparam>
        /// <param name="source">Sequence to test</param>
        /// <param name="sortExpression">A SQL-like sort expression with comma separated property names (and optional direction specifiers) (e.g. "Age DESC, Name")</param>
        /// <returns>True if all elements in sequence is sorted according to the specified fields (or if sequence is empty), otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="source"/> is null or <paramref name="sortExpression"/> is null</exception>
        /// <exception cref="Dynamite.Parsing.ParserException">If <paramref name="sortExpression"/> is an invalid sort expression.</exception>
        public static bool IsOrderedBy<T>(this IEnumerable<T> source, String sortExpression)
        {
            if (source == null) throw new ArgumentNullException("source");
            
            if (sortExpression == null) throw new ArgumentNullException("sortExpression");
            
            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext() == false)
                {
                    return true;
                }
                T lastItem = enumerator.Current;
                Comparison<T> comparison = ComparerBuilder<T>.CreateTypeComparison(sortExpression);
                while (enumerator.MoveNext())
                {
                    T thisItem = enumerator.Current;
                    if (comparison(lastItem, thisItem) > 0) return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Checks that the given sort expression is valid for the given sequence.
        /// </summary>
        /// <remarks>
        /// If you do not have a reference to an instance of the expression you can directly call 
        /// ComparerBuilder&lt;T&gt;.CreateTypeComparison(sortExpression) to validate a sort expression.
        /// </remarks>
        /// <typeparam name="T">Type of items in sequence</typeparam>
        /// <param name="source">Sequence to be tested</param>
        /// <param name="sortExpression">Sort expression to be verified.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="sortExpression"/> is null</exception>
        /// <exception cref="Dynamite.Parsing.ParserException">If sort expression is not valid.</exception>
        public static void ValidateSortExpression<T>(this IEnumerable<T> source, String sortExpression)
        {
            ComparerBuilder<T>.CreateTypeComparison(sortExpression);
        }

        /// <summary>
        /// Sort the elements of a sequence according to the specified textual sort expression.
        /// </summary>
        /// <typeparam name="T">Type in sequence</typeparam>
        /// <param name="source">Sequence to be sorted</param>
        /// <param name="sortExpression">A SQL-like sort expression with comma separated property names (and optional direction specifiers) (e.g. "Age DESC, Name")</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="source"/> is null or <paramref name="sortExpression"/> is null</exception>
        /// <exception cref="Dynamite.Parsing.ParserException">If <paramref name="sortExpression"/> is an invalid sort expression.</exception>
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, String sortExpression)
        {
            if (source == null)  throw new ArgumentNullException("source");
            if (sortExpression == null) throw new ArgumentNullException("sortExpression");
            
            OrderByFunction<T> orderBy = ComparerBuilder<T>.GetOrderByFunction(sortExpression);
            return orderBy(source);
        }

        /// <summary>
        /// Sort the element of a queryable sequence by a given sort expression.
        /// </summary>
        /// <typeparam name="T">Type of items in sequence.</typeparam>
        /// <param name="source">Sequence to be sorted.</param>
        /// <param name="sortExpression">A SQL-like sort expression with comma separated property names (and optional direction specifiers) (e.g. "Age DESC, Name")</param>
        /// <returns>A queryable object that can enumerate the elements in the input sequence ordered according to the given sort expression.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="source"/> is null or <paramref name="sortExpression"/> is null</exception>
        /// <exception cref="Dynamite.Parsing.ParserException">If <paramref name="sortExpression"/> is an invalid sort expression.</exception>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, String sortExpression)
        {
            return ComparerBuilder<T>.OrderBy(source, sortExpression);
            
        }
          


    }
}


