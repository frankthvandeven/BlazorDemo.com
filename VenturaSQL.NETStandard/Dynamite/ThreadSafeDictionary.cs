using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace VenturaSQL.Dynamite
{
    /// <summary>
    /// A dictionary implementation that offers thread-safe read and writes without any locking mechanism.
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="TValue">Type of value to store</typeparam>
    /// <remarks></remarks>
    public class ThreadSafeDictionary<TKey,TValue> : IDictionary<TKey,TValue>
    {
        private volatile Dictionary<TKey, TValue> current;

#pragma warning disable 0420

        public ThreadSafeDictionary()
        {
            current = new Dictionary<TKey, TValue>();
        }

        public ThreadSafeDictionary(IDictionary<TKey, TValue> dictionary)
        {
            current = new Dictionary<TKey, TValue>(dictionary);
        }

        public ThreadSafeDictionary(IEqualityComparer<TKey> comparer)
        {
            current = new Dictionary<TKey, TValue>(comparer);
        }

        public ThreadSafeDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            current = new Dictionary<TKey, TValue>(dictionary, comparer);
        }



        #region IDictionary<TKey,TValue> Members

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">Key to add</param>
        /// <param name="value">Value to add</param>
        /// <exception cref="System.ArgumentNullException">Key is null.</exception>
        /// <exception cref="System.ArgumentException">Key already exists in dictionary.</exception>
        public void Add(TKey key, TValue value)
        {
            Dictionary<TKey,TValue> oldDict;
            Dictionary<TKey, TValue> newDict;
            do
            {
                oldDict = current; 
                if (oldDict.ContainsKey(key)) throw new ArgumentException("Key already exists in dictionary");
                newDict = new Dictionary<TKey, TValue>(oldDict, oldDict.Comparer);
                newDict.Add(key, value);
                
            } while (Interlocked.CompareExchange(ref current, newDict, oldDict) != oldDict);

        }

        /// <summary>
        /// Determines if the specified key exists.
        /// </summary>
        /// <param name="key">Key to search for.</param>
        /// <returns>True if key exists.</returns>
        /// <remarks>
        /// Although this is thread-safe in the meaning that it will not crash if another thread writes to the dictionary,
        /// it is dangerous to assume that the result of this call is valid after a call because other threads might have
        /// changed the dictionary meanwhile.
        /// </remarks>
        public bool ContainsKey(TKey key)
        {
            return current.ContainsKey(key);
        }

        /// <summary>
        /// Gets a collection containing all keys in dictionary.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return current.Keys; }
        }

        /// <summary>
        /// Removes the value with the given key from the dictionary.
        /// </summary>
        /// <param name="key">Key to remove</param>
        /// <returns>True if this thread found and removed the given value from the dictionary, false otherwise.</returns>
        public bool Remove(TKey key)
        {
            Dictionary<TKey, TValue> oldDict;
            Dictionary<TKey, TValue> newDict;
            bool removed;
            do
            {
                oldDict = current;
                if (oldDict.ContainsKey(key) == false) return false;
                newDict = new Dictionary<TKey, TValue>(oldDict, oldDict.Comparer);
                removed = newDict.Remove(key);

            } while ( Interlocked.CompareExchange(ref current, newDict, oldDict) != oldDict);
            return removed;
        }

        /// <summary>
        /// Gets the value associated with the given key.
        /// </summary>
        /// <param name="key">Key for value to get.</param>
        /// <param name="value">Where the value will be stored if successful.</param>
        /// <returns>True if the key was found, false otherwise.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return current.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets a collection containing the values in the collection.
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return current.Values; }
        }

        /// <summary>
        /// Gets or sets the value for a specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The current value.</returns>
        public TValue this[TKey key]
        {
            get
            {
                return current[key];
            }
            set
            {
                Dictionary<TKey, TValue> oldDict;
                Dictionary<TKey, TValue> newDict;
               
                do
                {
                    oldDict = current;
                    
                    newDict = new Dictionary<TKey, TValue>(oldDict, oldDict.Comparer);
                    newDict[key] = value;

                } while (Interlocked.CompareExchange(ref current, newDict, oldDict) != oldDict);
                
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        void ICollection<KeyValuePair<TKey,TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            Dictionary<TKey, TValue> oldDict;
            Dictionary<TKey, TValue> newDict;
            
            do
            {
                oldDict = current;
                newDict = new Dictionary<TKey, TValue>(oldDict, oldDict.Comparer);
                ((ICollection<KeyValuePair<TKey, TValue>>)newDict).Add(item);

            } while (Interlocked.CompareExchange(ref current, newDict, oldDict) != oldDict);
                
        }

        /// <summary>
        /// Clears the dictionary.
        /// </summary>
        public void Clear()
        {
            current = new Dictionary<TKey, TValue>(current.Comparer);
            
        }

        /// <summary>
        /// Determines if the collection currently contains the given key-value pair.
        /// </summary>
        /// <param name="item">Value pair to look for.</param>
        /// <returns>True if the collection currently contains the given key-value pair.</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return current.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the dictionary to an array.
        /// </summary>
        /// <param name="array">Array to copy key value pairs to.</param>
        /// <param name="arrayIndex">Index of array index to start inserting elements.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)current).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements currently in dictionary.
        /// </summary>
        public int Count
        {
            get { return current.Count; }
        }

        /// <summary>
        /// Determines if this collection is read-only, i.e. does not permit changes.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Remove the given key-value pair from the collection.
        /// </summary>
        /// <param name="item">Key-value pair to remove</param>
        /// <returns>True if the pair was found and removed.</returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            Dictionary<TKey, TValue> oldDict;
            Dictionary<TKey, TValue> newDict;
            bool removed;
            do
            {
                oldDict = current;
                newDict = new Dictionary<TKey, TValue>(oldDict, oldDict.Comparer);
                removed = ((ICollection<KeyValuePair<TKey, TValue>>)newDict).Remove(item);

            } while (Interlocked.CompareExchange(ref current, newDict, oldDict) != oldDict);
            return removed;
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Returns an enumerator that iterates through the elements currently in the dictionary.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return current.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return current.GetEnumerator();
        }

        #endregion

#pragma warning restore 0420
    }
}
