using System;
using System.Collections;
using System.Collections.Generic;

namespace VenturaSQL
{
    public abstract partial class ResultsetData<TResultset, TRecord> : IResultsetBase, IList<TRecord>, IList, IReadOnlyList<TRecord>
        where TResultset : IResultsetBase
        where TRecord : IRecordBase
    {

        private VenturaSqlSchema _schema;
        private string _updateable_tablename;
        private TRecord[] _records;
        private int _currentrecord_index;
        private int _recordcount;

        public ResultsetData()
        {
            _schema = null;
            _updateable_tablename = null;
            _records = new TRecord[0x10];
            _recordcount = 0;
            _currentrecord_index = -1;
        }

        public TRecord this[int index]
        {
            get
            {
                if (index < 0 || index >= _recordcount)
                    throw new ArgumentOutOfRangeException("index");

                return _records[index];
            }
        }


        public void Delete()
        {
            if (_currentrecord_index == -1)
                throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET);

            Delete(_currentrecord_index);
        }


        public void Delete(int index)
        {
            if (index < 0 || index >= _recordcount)
                throw new ArgumentOutOfRangeException("index");

            if (_records[index].RecordStatus == DataRecordStatus.New)
            {
                RemoveItem(index); // Do a real delete.
                ResetCurrentRecordAfterRemove(index);
            }
            else
            {
                _records[index].RecordStatus = DataRecordStatus.ExistingDelete;
            }

        }

        public void Delete(TRecord record)
        {
            for (int i = 0; i < _recordcount; i++)
                if (_records[i].Equals(record))
                {
                    Delete(i);
                    break;
                }
        }

        public void RealDelete()
        {
            if (_currentrecord_index == -1)
                throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET);

            RemoveItem(_currentrecord_index); // Do a real delete.
            ResetCurrentRecordAfterRemove(_currentrecord_index);
        }

        public void RealDelete(int index)
        {
            if (index < 0 || index >= _recordcount)
                throw new ArgumentOutOfRangeException("index");

            RemoveItem(index); // Do a real delete.
            ResetCurrentRecordAfterRemove(index);
        }

        public void RealDelete(TRecord record)
        {
            for (int i = 0; i < _recordcount; i++)
                if (_records[i].Equals(record))
                {
                    RemoveItem(i);
                    ResetCurrentRecordAfterRemove(i);
                    break;
                }
        }

        #region Internal Helpers - All methods in this region cause data binding events!

        /// <summary>
        /// Inserts an element into the records array at the specified index.
        /// </summary>
        protected virtual void InsertItem(int index, TRecord item)
        {
            if (index > _recordcount)
                throw new ArgumentOutOfRangeException();

            if (_recordcount == _records.Length)
                EnsureCapacity(_recordcount + 1);

            if (index < _recordcount)
                Array.Copy(_records, index, _records, index + 1, _recordcount - index);

            _records[index] = item;
            _recordcount++;
        }

        private void ResetCurrentRecordAfterRemove(int remove_index)
        {
            // Currently selected record support
            if (_currentrecord_index != -1)
            {
                if (_currentrecord_index == remove_index)
                {
                    CurrentRecordIndex = -1; // unselect
                    CurrentRecordIndex = remove_index; // reselect
                }
                else if (_currentrecord_index > remove_index)
                    CurrentRecordIndex--;
            }

        }

        protected virtual void RemoveItem(int index)
        {
            if ((index < 0) || (index >= _recordcount))
                throw new ArgumentOutOfRangeException("index", "Argument out of range.");

            _recordcount--;

            // Currently selected record support
            //if (_currentrecord_index != -1)
            //{
            //    if (_currentrecord_index == index)
            //        CurrentRecordIndex = -1;
            //    else if (_currentrecord_index > index)
            //        CurrentRecordIndex--;
            //}

            if (index < _recordcount)
                Array.Copy(_records, index + 1, _records, index, _recordcount - index);

            _records[_recordcount] = default(TRecord); // null

        }

        protected virtual void SetItem(int index, TRecord item)
        {
            this._records[index] = item;

            /* The CurrentRecord did not change, but we still send out the notification for the UI. */
            if (_currentrecord_index == index)
                OnRecordIndexChanged();
        }

        protected virtual void ClearItems()
        {
            _records = new TRecord[0x10];
            _recordcount = 0;

            // Currently selected record support
            CurrentRecordIndex = -1;
        }

        #endregion

        public void Clear()
        {
            ClearItems();
        }

        public int RecordCount
        {
            get { return _recordcount; }
        }

        /// <summary>
        /// Calculates the number of records in the resultset that will be send to the database and
        /// result in an UPDATE/INSERT/DELETE when Recordset.SaveChanges() is called.
        /// </summary>
        public int PendingChangesCount()
        {
            int count = 0;

            for (int i = 0; i < this._recordcount; i++)
            {
                if (_records[i].PendingChanges() == true)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Reset all records to Existing. The modification status for each column is unaffected.
        /// </summary>
        public void ResetAllToExisting()
        {
            for (int i = 0; i < this._recordcount; i++)
                _records[i].ResetToExisting();
        }

        /// <summary>
        /// Reset all records to Unmodified.
        /// </summary>
        public void ResetAllToUnmodified()
        {
            for (int i = 0; i < this._recordcount; i++)
                _records[i].ResetToUnmodified();
        }


        /// <summary>
        /// Reset all records to Unmodified and Existing. Like they came fresh from the database.
        /// </summary>
        public void ResetAllToUnmodifiedExisting()
        {
            for (int i = 0; i < this._recordcount; i++)
                _records[i].ResetToUnmodifiedExisting();
        }

        /// <summary>
        /// Reset all records to Unmodified and Existing but skip all records with the specified record status.
        /// </summary>
        /// <param name="status_to_skip">Records with the specified status will be skipped from resetting.</param>
        public void ResetAllToUnmodifiedExistingExcept(DataRecordStatus status_to_skip)
        {
            for (int i = 0; i < this._recordcount; i++)
                if (_records[i].RecordStatus != status_to_skip)
                    _records[i].ResetToUnmodifiedExisting();
        }

        #region Currently selected record (RecordNumber) support

        public int CurrentRecordIndex
        {
            get { return _currentrecord_index; }
            set
            {
                //Commented out the value equals check, as we want the notification event to be forced.
                //if (value == _currentrecord_index)
                //    return;

                if ((value < -1) || (value >= _recordcount))
                    throw new ArgumentOutOfRangeException();

                // This is the ONLY place in the code where _currentrecord_index should be modified!
                _currentrecord_index = value;

                // Fire the propertychanged events for 'CurrentRecord' and 'CurrentRecordIndex'.
                OnRecordIndexChanged();
            }
        }

        public TRecord CurrentRecord
        {
            get
            {
                if (_currentrecord_index == -1)
                    return default(TRecord); // NULL

                return _records[_currentrecord_index];
            }
            set
            {
                if (value == null)
                {
                    CurrentRecordIndex = -1;
                    return;
                }

                bool found = false;

                for (int i = 0; i < _recordcount; i++)
                {
                    if (_records[i].Equals(value))
                    {
                        CurrentRecordIndex = i;
                        found = true;
                        break;
                    }
                }

                if (found == false)
                    throw new InvalidOperationException("The record was not found in the collection.");

            }
        }

        public void SelectFirstAsCurrentRecord()
        {
            if (_recordcount > 0)
                CurrentRecordIndex = 0;
            else
                CurrentRecordIndex = -1;
        }

        public void SelectLastAsCurrentRecord()
        {
            if (_recordcount > 0)
                CurrentRecordIndex = _recordcount - 1;
            else
                CurrentRecordIndex = -1;
        }

        #endregion

        /// <summary>
        /// Ensures that the RecordBase[] array is at least the specified capacity.
        /// Will create a new array and copy old array elements into it if needed.
        /// </summary>
        private void EnsureCapacity(int requested_capacity)
        {
            if (requested_capacity <= _records.Length)
                return;

            int proposedLength = (_records.Length == 0) ? 0x10 : (_records.Length * 2);

            if (proposedLength < requested_capacity)
                proposedLength = requested_capacity;

            if (proposedLength > _records.Length)
            {
                TRecord[] tempRows = new TRecord[proposedLength];

                if (_recordcount > 0)
                    Array.Copy(_records, tempRows, _records.Length);

                _records = tempRows;
            }

        } // end of method ensurecapacity


        #region re-implementation in progress

        // This is a method in the generated Recordset code.
        protected abstract TRecord InternalCreateExistingRecordObject(object[] columnvalues);

        // Call internally when the array was resized outside of CollectionChanged monitoring.
        protected virtual void OnRecordArrayResized()
        {
            // RecordsetObservable will override this method.
        }

        protected virtual void OnRecordArraySorted()
        {
            // RecordsetObservable will override this method.
        }

        protected virtual void OnRecordIndexChanged()
        {
            // RecordsetObservable will override this method.
        }

        #endregion

        #region predicates

        public bool FindAndSelectAsCurrent(Predicate<TRecord> match)
        {
            TRecord record = Find(match);

            if (record == null)
                return false;

            this.CurrentRecord = record;

            return true;
        }

        /// <summary>
        /// Returns null if not found.
        /// </summary>
        public TRecord Find(Predicate<TRecord> match)
        {
            if (match == null)
                throw new ArgumentNullException("match");

            for (int i = 0; i < this._recordcount; i++)
                if (match(this._records[i]))
                    return this._records[i];

            return default(TRecord);
        }

        /// <summary>
        /// Returns a List containing the records found.
        /// </summary>
        public List<TRecord> FindAll(Predicate<TRecord> match)
        {
            if (match == null)
                throw new ArgumentNullException("match");

            List<TRecord> list = new List<TRecord>();

            for (int i = 0; i < this._recordcount; i++)
                if (match(this._records[i]))
                    list.Add(this._records[i]);

            return list;
        }

        #endregion

        #region Interface IRecordsetBase - exposing internals to the VenturaSQL Runtime

        /// <summary>
        /// Returns the TRecord[] as an IRecordBase[] array.
        /// For internal use only.
        /// </summary>
        //IRecordBase[] IRecordsetBase.AsArray()
        //{
        //    IRecordBase[] arr = new IRecordBase[_recordcount];
        //    Array.Copy(_records, arr, _recordcount);
        //    return arr;
        //}

        /// <summary>
        /// Resets the Recordset to a newly initialized state.
        /// Eventual pending changes will be lost.
        /// </summary>
        void IResultsetBase.Clear()
        {
            ClearItems();
        }

        IRecordBase IResultsetBase.this[int index]
        {
            get
            {
                if (index < 0 || index >= _recordcount)
                    throw new ArgumentOutOfRangeException("index");

                return _records[index];
            }
        }

        int IResultsetBase.Length
        {
            get { return _recordcount; }
        }

        VenturaSqlSchema IResultsetBase.Schema
        {
            get { return _schema; }
            set { _schema = value; }
        }

        string IResultsetBase.UpdateableTablename
        {
            get { return _updateable_tablename; }
            set { _updateable_tablename = value; }
        }

        /// <summary>
        /// Ensures that the RecordBase[] array is at least the specified capacity.
        /// Will create a new array and copy old array elements into it if needed.
        /// Finally it sets the _recordcount to requested_capacity.
        /// Called from ClientFrameReader.cs. For internal use only.
        /// </summary>
        void IResultsetBase.IncreaseCapacity(int additional_capacity)
        {
            int slack = 0x10;
            int new_capacity = _recordcount + additional_capacity;

            EnsureCapacity(new_capacity + slack);

            _recordcount = new_capacity;
        }

        /// <summary>
        /// Used in RowLoaderRecordset.cs
        /// Expands the TRecord[] array if needed, and adds a Record to it, and then raises the RecordCount by 1.
        /// All this without triggering data binding events.
        /// For internal use only. For maximum performance.
        /// </summary>
        void IResultsetBase.OptimizedCreateAndAppendExistingRecord(object[] columnvalues)
        {
            EnsureCapacity(_recordcount + 1);
            _records[_recordcount] = InternalCreateExistingRecordObject(columnvalues);
            _recordcount++;
        }

        /// <summary>
        /// Used in ClientFrameReader.cs.
        /// For internal use only. For maximum performance.
        /// </summary>
        void IResultsetBase.OptimizedCreateAndSetExistingRecord(int index, object[] columnvalues)
        {
            _records[index] = InternalCreateExistingRecordObject(columnvalues);
        }


        void IResultsetBase.OnAfterExecSql() // Load
        {
            if (_recordcount > 0)
                CurrentRecordIndex = 0;

            OnRecordArrayResized();
        }

        void IResultsetBase.OnAfterSaveChanges() // Save
        {
            if (CurrentRecord != null)
            {
                if (CurrentRecord.RecordStatus == DataRecordStatus.ExistingDelete)
                    CurrentRecord = default(TRecord);
            }

            int new_index = 0;
            int total = _recordcount;

            _recordcount = 0;

            for (int index = 0; index < total; index++)
            {
                if (_records[index].RecordStatus != DataRecordStatus.ExistingDelete)
                {
                    _records[new_index] = _records[index];
                    new_index++;
                    _recordcount++;
                }
            }

            /// Reset all the rows to not modified status, just like after a Recordset.Load() call.
            for (int x = 0; x < _recordcount; x++)
                _records[x].ResetToUnmodifiedExisting();

            OnRecordArrayResized();
        }

        #endregion

        #region Here are the Interfaces

        int ICollection<TRecord>.Count
        {
            get { return _recordcount; }
        }

        bool ICollection<TRecord>.IsReadOnly
        {
            get { return false; }
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }

        int ICollection.Count /* needed for INotifyCollectionChanged to work */
        {
            get { return _recordcount; }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        int IReadOnlyCollection<TRecord>.Count
        {
            get { return _recordcount; }
        }

        TRecord IReadOnlyList<TRecord>.this[int index]
        {
            get { return _records[index]; }
        }

        object IList.this[int index] /* needed for INotifyCollectionChanged to work */
        {
            get { return _records[index]; }
            set
            {
                throw new NotImplementedException();
            }
        }

        TRecord IList<TRecord>.this[int index]
        {
            get { return _records[index]; }
            set { SetItem(index, value); }
        }

        int IList<TRecord>.IndexOf(TRecord item)
        {
            for (int x = 0; x < _recordcount; x++)
            {
                if (_records[x].Equals(item))
                    return x;
            }

            return -1;
        }

        void IList<TRecord>.Insert(int index, TRecord item)
        {
            this.InsertItem(index, item);
            this.CurrentRecordIndex = index;
        }

        void IList<TRecord>.RemoveAt(int index)
        {
            this.RemoveItem(index);
            this.ResetCurrentRecordAfterRemove(index);
        }

        void ICollection<TRecord>.Add(TRecord record)
        {
            this.InsertItem(_recordcount, record);
            this.CurrentRecordIndex = _recordcount;
        }

        void ICollection<TRecord>.Clear()
        {
            ClearItems();
        }

        bool ICollection<TRecord>.Contains(TRecord item)
        {
            throw new NotImplementedException();
        }

        void ICollection<TRecord>.CopyTo(TRecord[] array, int arrayIndex)
        {
            Array.Copy(_records, 0, array, arrayIndex, _recordcount);
        }

        bool ICollection<TRecord>.Remove(TRecord item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<TRecord> IEnumerable<TRecord>.GetEnumerator()
        {
            for (int x = 0; x < _recordcount; x++)
                yield return _records[x];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int x = 0; x < _recordcount; x++)
                yield return _records[x];
        }

        int IList.Add(object value)
        {
            throw new NotImplementedException();
        }

        void IList.Clear()
        {
            throw new NotImplementedException();
        }

        bool IList.Contains(object item)
        {
            if (item == null)
            {
                for (int i = 0; i < _recordcount; i++)
                    if (_records[i] == null)
                        return true;

                return false;
            }

            for (int j = 0; j < _recordcount; j++)
                if (_records[j] != null && _records[j].Equals(item))
                    return true;

            return false;
        }

        int IList.IndexOf(object value)
        {
            return Array.IndexOf(_records, value, 0, _recordcount);
        }

        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        void IList.Remove(object value)
        {
            throw new NotImplementedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Calculates and returns statistics.
        /// </summary>
        public override string ToString()
        {
            int existing_unmodified_count = 0;
            int existing_modified_count = 0;
            int existingdelete_count = 0;
            int new_count = 0;

            for (int i = 0; i < _recordcount; i++)
            {
                TRecord record = _records[i];

                DataRecordStatus rowstatus = record.RecordStatus;

                int modified_columns = record.ModifiedColumnCount();

                if (rowstatus == DataRecordStatus.Existing)
                {
                    if (modified_columns == 0)
                        existing_unmodified_count++;
                    else
                        existing_modified_count++;
                }
                else if (rowstatus == DataRecordStatus.ExistingDelete)
                    existingdelete_count++;
                else if (rowstatus == DataRecordStatus.New)
                    new_count++;
            }

            return $"{_recordcount} records: {existing_unmodified_count} existing unmodified, {existing_modified_count} existing modified, {new_count} new and {existingdelete_count} deleted (array length {_records.Length})";
        }

    } // end of class

} // end of namespace
