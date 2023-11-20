using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace VenturaSQL
{
    public abstract class ResultsetObservable<TResultset, TRecord> : ResultsetData<TResultset, TRecord>, INotifyCollectionChanged, INotifyPropertyChanged
        where TResultset : IResultsetBase
        where TRecord : IRecordBase
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        protected override void ClearItems()
        {
            //this.CheckReentrancy();
            base.ClearItems();
            this.OnPropertyChanged("RecordCount");
            this.OnPropertyChanged("Item[]");
            this.OnCollectionReset();
        }

        protected override void InsertItem(int index, TRecord item)
        {
            //this.CheckReentrancy();
            base.InsertItem(index, item);
            this.OnPropertyChanged("RecordCount");
            this.OnPropertyChanged("Item[]");
            //Debug.WriteLine($"ResultServeObservable CollectionChanged.Add event fired.");
            this.OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        protected override void RemoveItem(int index)
        {
            //this.CheckReentrancy();
            TRecord item = base[index];
            base.RemoveItem(index);
            this.OnPropertyChanged("RecordCount");
            this.OnPropertyChanged("Item[]");
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, item, index);
        }

        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            //this.CheckReentrancy();
            TRecord item = base[oldIndex];
            base.RemoveItem(oldIndex);
            base.InsertItem(newIndex, item);
            this.OnPropertyChanged("Item[]");
            OnCollectionChanged(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex);
        }

        protected override void SetItem(int index, TRecord item)
        {
            //this.CheckReentrancy();
            TRecord t = base[index];
            base.SetItem(index, item);
            this.OnPropertyChanged("Item[]");
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, t, item, index);
        }

        #region Event helpers

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item, index));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
        }

        private void OnCollectionReset()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        /// <summary>
        /// This method is fired from RecordsetData.cs whenever the TRecord[] array was resized. (bulk updates to the array).
        /// </summary>
        protected override void OnRecordArrayResized()
        {
            base.OnRecordArrayResized();
            OnPropertyChanged("RecordCount");
            OnPropertyChanged("Item[]");
            OnCollectionReset();
        }

        protected override void OnRecordArraySorted()
        {
            base.OnRecordArraySorted();
            OnPropertyChanged("Item[]");
            OnCollectionReset();
        }

        protected override void OnRecordIndexChanged()
        {
            //Debug.WriteLine($"ResultsetObservable.OnRecordIndexChanged() NotifyPropertyChgd: CurrentRecordIndex={this.CurrentRecordIndex} recordCount={this.RecordCount}");

            base.OnRecordIndexChanged();
            OnPropertyChanged(nameof(CurrentRecord));
            OnPropertyChanged(nameof(CurrentRecordIndex));
        }

    } 
} 
