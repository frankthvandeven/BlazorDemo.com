using System;
using System.Collections.Generic;
using VenturaSQL.Dynamite;

namespace VenturaSQL
{
    public abstract partial class ResultsetData<TResultset, TRecord>
    {
        // Test data for sorting:
        // A 3
        // B 1
        // A 2
        // Correct outcome: A2, A3, B1 
        // Incorrect: A 3, A 2, B1
        // Incorrect: B1 A2 A3 (only sorted on the numbers)

        /// <summary>
        /// A sort expression is the easiest way to sort a resultset.
        /// </summary>
        /// <param name="sort_expression">For example "Firstname, Lastname desc" or "Name.Length"</param>
        public void Sort(string sort_expression)
        {
            Comparison<TRecord> comparison = ComparerBuilder<TRecord>.CreateTypeComparison(sort_expression);

            Sort(0, _recordcount, comparison);
        }

        public void Sort(IComparer<TRecord> comparer)
        {
            Sort(0, _recordcount, comparer);
        }

        public void Sort(int index, int count, IComparer<TRecord> comparer)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException();

            if (count < 0)
                throw new ArgumentOutOfRangeException();

            if (_recordcount - index < count)
                throw new ArgumentException();

            TRecord current = this.CurrentRecord;

            Array.Sort<TRecord>(_records, index, count, comparer);

            OnRecordArraySorted();

            // This will re-set the CurrentRecordIndex
            CurrentRecord = current;
        }

        public void Sort(Comparison<TRecord> comparison)
        {
            Sort(0, _recordcount, comparison);
        }

        public void Sort(int index, int count, Comparison<TRecord> comparison)
        {
            if (comparison == null)
                throw new ArgumentNullException();

            IComparer<TRecord> comparer = new FunctorComparer<TRecord>(comparison);

            Sort(index, count, comparer);
        }


    } // end of class

    internal sealed class FunctorComparer<T> : IComparer<T>
    {
        private readonly Comparison<T> _comparison;

        public FunctorComparer(Comparison<T> comparison)
        {
            _comparison = comparison;
        }

        public int Compare(T x, T y)
        {
            return _comparison(x, y);
        }

    } // end of class

} // end of namespace
