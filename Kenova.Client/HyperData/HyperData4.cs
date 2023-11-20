using System;
using System.Collections.Generic;

//
// MULTISELECT / CHECKBOX functionality.
//

namespace Kenova.Client.Components
{
    public partial class HyperData<ItemType>
    {
        private List<ItemType> _checkedItems = new();
        private int _checkedItemsCount = 0;

        private MultiCheck _useMultiCheck = MultiCheck.Off;

        public Action CheckedItemsChanged { get; set; }

        public MultiCheck UseMultiCheck
        {
            get { return _useMultiCheck; }
            set
            {
                if (_useMultiCheck == value)
                    return;

                if (value == MultiCheck.ItemField)
                {
                    if (IsChecked == null || SetChecked == null || SetUnchecked == null)
                        throw new Exception("Set the IsChecked, SetChecked and SetUnchecked parameters before selecting MultiCheck.ItemField mode.");
                }


                _useMultiCheck = value;

                //if (value == MultiCheck.ItemField && this._items_source != null)
                //{
                //    this.RecalculateCheckedItemsCount();
                //}

                //if (value != MultiCheck.List )
                //    _checkedItems.Clear();

            }
        }

        public int CheckedItemsCount
        {
            get { return _checkedItemsCount; }
            set { _checkedItemsCount = value; }
        }

        public List<ItemType> CheckedItems
        {
            get { return _checkedItems; }
            set
            {
                _checkedItems = value;
            }
        }

        /// <summary>
        /// Return true if the item is to be displayed as checked.<para />
        /// For example: "i => i.Category == 1"<para />
        /// IsChecked, SetChecked and SetUnchecked are a trinity.
        /// </summary>
        public Func<ItemType, bool> IsChecked { get; set; }

        /// <summary>
        /// This action is executed after the checkbox was ticked.<para />
        /// For example: "i => i.Category = 1"<para />
        /// IsChecked, SetChecked and SetUnchecked are a trinity.
        /// </summary>
        public Action<ItemType> SetChecked { get; set; }

        /// <summary>
        /// This action is executed after the checkbox was cleared.<para />
        /// For example: "i => i.Category = 0"<para />
        /// IsChecked, SetChecked and SetUnchecked are a trinity.
        /// </summary>
        public Action<ItemType> SetUnchecked { get; set; }

        public bool GetItemIsChecked(ItemType item)
        {
            if (_useMultiCheck == MultiCheck.List)
            {
                return CheckedItems.Contains(item);
            }
            else if (_useMultiCheck == MultiCheck.ItemField)
            {
                return IsChecked(item);
            }

            return false;
        }

        public void CheckAll()
        {
            if (_useMultiCheck == MultiCheck.List)
            {
                _checkedItems.Clear();
                for (int i = 0; i < _display_items.Count; i++)
                    _checkedItems.Add(_display_items[i]);

                _checkedItemsCount = _display_items.Count;
            }
            else if (_useMultiCheck == MultiCheck.ItemField)
            {
                // Select all...
                for (int i = 0; i < _display_items.Count; i++)
                    CheckItem(_display_items[i]);
            }

            if (CheckedItemsChanged != null)
                CheckedItemsChanged();
        }

        public void UncheckAll()
        {
            if (_useMultiCheck == MultiCheck.List)
            {
                _checkedItems.Clear();
                _checkedItemsCount = 0;
            }
            else if (_useMultiCheck == MultiCheck.ItemField)
            {
                for (int i = 0; i < _display_items.Count; i++)
                    UncheckItem(_display_items[i]);
            }

            if (CheckedItemsChanged != null)
                CheckedItemsChanged();
        }

        /// <summary>
        /// Check the item if it was not checked already.
        /// This method updates the CheckedItemsCount.
        /// </summary>
        public void CheckItem(ItemType item)
        {
            if (_useMultiCheck == MultiCheck.List)
            {
                if (!CheckedItems.Contains(item))
                {
                    CheckedItems.Add(item);
                    _checkedItemsCount++;
                }
            }
            else if (_useMultiCheck == MultiCheck.ItemField)
            {
                if (!IsChecked(item))
                {
                    SetChecked(item);
                    _checkedItemsCount++;
                }
            }

        }

        /// <summary>
        /// Uncheck the item if it was not unchecked already.
        /// This method updates the CheckedItemsCount.
        /// </summary>
        public void UncheckItem(ItemType item)
        {
            if (_useMultiCheck == MultiCheck.List)
            {
                if (CheckedItems.Contains(item))
                {
                    CheckedItems.Remove(item);
                    _checkedItemsCount--;
                }
            }
            else if (_useMultiCheck == MultiCheck.ItemField)
            {
                if (IsChecked(item))
                {
                    SetUnchecked(item);
                    _checkedItemsCount--;
                }
            }
        }

        /// <summary>
        /// Toggle the item check status.
        /// This method updates the CheckedItemsCount.
        /// </summary>
        public void ToggleCheck(ItemType item)
        {
            if (_useMultiCheck == MultiCheck.List)
            {
                if (CheckedItems.Contains(item))
                {
                    CheckedItems.Remove(item);
                    _checkedItemsCount--;
                }
                else
                {
                    CheckedItems.Add(item);
                    _checkedItemsCount++;
                }
            }
            else if (_useMultiCheck == MultiCheck.ItemField)
            {
                if (IsChecked(item))
                {
                    SetUnchecked(item);
                    _checkedItemsCount--;
                }
                else
                {
                    SetChecked(item);
                    _checkedItemsCount++;
                }
            }

            if (CheckedItemsChanged != null)
                CheckedItemsChanged();
        }

        public void RecalculateCheckedItemsCount()
        {
            _checkedItemsCount = 0;

            // Count the checked display items (limited by filter)

            if (_useMultiCheck == MultiCheck.List)
            {
                _checkedItemsCount = CheckedItems.Count;
            }
            else if (_useMultiCheck == MultiCheck.ItemField)
            {
                for (int i = 0; i < _display_items.Count; i++)
                {
                    if (IsChecked(_display_items[i]))
                        _checkedItemsCount++;
                }
            }
        }

    }

    public enum MultiCheck
    {
        Off,
        List,
        ItemField

    }

}
