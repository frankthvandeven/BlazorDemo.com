using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Kenova.Client.Components
{
    public partial class HyperData<ItemType> : IDisposable, INotifyPropertyChanged
    {
        private IList<ItemType> _items_source_list;
        private INotifyCollectionChanged _items_source_observable;
        private IList<ItemType> _display_items;

        private ColumnCollection<ItemType> _columns = new();

        private Expression<Func<ItemType, string>> _groupingExpression;
        private Func<ItemType, string> _groupingExpressionCompiled;

        private Expression<Func<ItemType, string>> _tooltipExpression;
        private Func<ItemType, string> _tooltipExpressionCompiled;

        public Func<ItemType, bool> RowEnabledExpression = (c) => true;

        public Action<ItemType> SelectedItemChanged { get; set; }

        /// <summary>
        /// This is the event to send signals from HyperData to the linked HyperGrid.
        /// </summary>
        public event HyperDataEventHandler<ItemType> DataChanged;

        /// <summary>
        /// HyperData also sends out a PropertyChanged event. 
        /// Currently only for SelectedItem.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private Expression<Func<ItemType>> _selectedItemExpression;
        private Func<ItemType> _selectedItemGetter;
        private Action<ItemType> _selectedItemSetter;
        private string _selectedItemFieldName;
        private INotifyPropertyChanged _selectedItemModel;

        /// <summary>
        /// For example "() => this.Model.CurrentCustomer"
        /// This parameter can only be set once. Any subsequent changes to the parameter value will be ignored.
        /// </summary>
        public Expression<Func<ItemType>> SelectedItemExpression
        {
            get { return _selectedItemExpression; }
            set
            {
                if (value == null && _selectedItemExpression == null)
                    return;

                if (value != null && _selectedItemExpression != null && _selectedItemExpression.Equals(value))
                    return;

                if( _selectedItemExpression != null)
                { 
                    _selectedItemExpression = null;
                    _selectedItemGetter = null;
                    _selectedItemSetter = null;

                    if (_selectedItemModel != null)
                        _selectedItemModel.PropertyChanged -= SelectedItemModel_PropertyChanged;

                    _selectedItemModel = null;
                    _selectedItemFieldName = null;
                }

                if (value == null)
                    return;

                _selectedItemExpression = value;
                _selectedItemGetter = value.Compile();
                _selectedItemSetter = BindingHelper.CreateValueSetter<ItemType>(value);

                BindingHelper.ParseAccessor(value, out object model, out _selectedItemFieldName);
                _selectedItemModel = model as INotifyPropertyChanged;

                if (_selectedItemModel != null)
                    _selectedItemModel.PropertyChanged += SelectedItemModel_PropertyChanged;

            }
        }

        private void SelectedItemModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _selectedItemFieldName) /* "CurrentRecord" for VenturaSQL */
            {
                //if (KenovaClientConfig.Diagnostics) Console.WriteLine($"🔴 HyperData - Received notification from databound SelectedItem ({_selectedItemFieldName})");
                // propagate to HyperGrid
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
            }
        }

        public ItemType SelectedItem
        {
            get
            {
                if (_selectedItemExpression == null)
                    return default;

                return _selectedItemGetter();
            }
            set
            {

                if (_selectedItemSetter != null)
                {
                    _selectedItemSetter(value);
                }

                if (SelectedItemChanged != null)
                {
                    SelectedItemChanged(value); // Action
                }

            }

        }


        public IList<ItemType> Items
        {
            get { return _items_source_list; }
            set
            {
                if (_items_source_list == value)
                    return;

                if (_items_source_observable != null)
                    _items_source_observable.CollectionChanged -= ItemsSourceObservable_CollectionChanged;

                _items_source_list = value;
                _items_source_observable = value as INotifyCollectionChanged;
                _display_items = value;

                if (_items_source_observable != null)
                    _items_source_observable.CollectionChanged += ItemsSourceObservable_CollectionChanged;

                //if (value != null && this._useMultiCheck == MultiCheck.ItemField)
                //{
                //    this.RecalculateCheckedItemsCount();
                //}

            }
        }

        /// <summary>
        /// A list of items after filtering and sorting.
        /// DisplayItems.Count can be less than Items.Count or be zero.
        /// </summary>
        public IList<ItemType> DisplayItems
        {
            get { return _display_items; }
        }

        /// <summary>
        /// Observing the linked datasource
        /// </summary>
        private void ItemsSourceObservable_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                ResetFilterAndSorting();
                DataChanged?.Invoke(new HyperDataEventArgs<ItemType>(HyperDataEventAction.Reset));
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ResetFilterAndSorting();
                DataChanged?.Invoke(new HyperDataEventArgs<ItemType>(HyperDataEventAction.Add));
            }
        }

        public ColumnCollection<ItemType> Columns
        {
            get { return _columns; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Columns");

                if (_columns == value)
                    return;

                _columns = value;
            }
        }

        /// <summary>
        /// When true, filtered items will not be unchecked.
        /// The default value of the DropdownMode field is false.
        /// </summary>
        public bool DropdownMode = false;

        /// <summary>
        /// The default value of the UseFilter field is true.
        /// </summary>
        public bool UseFilter = true;

        /// <summary>
        /// The default value of the UseHeader field is true.
        /// </summary>
        public bool UseHeader = true;

        public Expression<Func<ItemType, string>> GroupingExpression
        {
            get { return _groupingExpression; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("GroupingExpression");

                _groupingExpression = value;
                _groupingExpressionCompiled = value.Compile();
            }
        }

        public Expression<Func<ItemType, string>> TooltipExpression
        {
            get { return _tooltipExpression; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("TooltipExpression");

                _tooltipExpression = value;
                _tooltipExpressionCompiled = value.Compile();
            }
        }

        public string GetGroupingString(ItemType item)
        {
            return _groupingExpressionCompiled(item);
        }

        public string GetTooltipString(ItemType item)
        {
            return _tooltipExpressionCompiled(item);
        }

        public void Dispose()
        {
            if (_items_source_observable != null)
                _items_source_observable.CollectionChanged -= ItemsSourceObservable_CollectionChanged;

            if (_selectedItemModel != null)
                _selectedItemModel.PropertyChanged -= SelectedItemModel_PropertyChanged;

        }


    }

}
