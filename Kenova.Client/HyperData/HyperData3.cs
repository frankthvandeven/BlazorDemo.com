using System;
using System.Linq;

namespace Kenova.Client.Components
{
    public partial class HyperData<ItemType>
    {
        private string _filter_text = "";
        private Column<ItemType> _last_sort_column = null;
        private ColumnSortMode _last_sort_mode = ColumnSortMode.None;

        public void ResetFilterAndSorting()
        {
            _filter_text = "";
            _last_sort_column = null;
            _last_sort_mode = ColumnSortMode.None; // was Ascending, why?

            _display_items = _items_source_list;
        }

        public string FilterText
        {
            get { return _filter_text; }
            set
            {
                if (_filter_text == value)
                    return;

                if (value == null)
                    _filter_text = "";
                else
                    _filter_text = value;

                _currentpage = 1; // always start from first page as the list length is changing

                this.SelectedItem = default;

                RefreshDisplayItems();
            }
        }

        public ColumnSortMode LastSortMode
        {
            get { return _last_sort_mode; }
        }

        public Column<ItemType> LastSortColumn
        {
            get { return _last_sort_column; }
        }

        /// <summary>
        /// Event handler method for click on a column header div.
        /// </summary>
        public void ToggleSorting(Column<ItemType> column)
        {
            if (column == null)
                throw new ArgumentNullException("column");

            if (_last_sort_column != column)
            {
                _last_sort_column = column;
                _last_sort_mode = ColumnSortMode.Ascending;
            }
            else if (_last_sort_mode == ColumnSortMode.Ascending)
            {
                _last_sort_mode = ColumnSortMode.Descending;
            }
            else if (_last_sort_mode == ColumnSortMode.Descending)
            {
                _last_sort_column = null;
                _last_sort_mode = ColumnSortMode.None;
            }

            RefreshDisplayItems();
        }

        /// <summary>
        /// See "GetData()" in:
        /// C:\Bagger4\BlazorTable-master - best implementation\src\BlazorTable\Components\Table.razor.cs
        /// </summary>
        private void RefreshDisplayItems()
        {
            //if (KenovaClientConfig.Diagnostics) Console.WriteLine("HYPERDATA - RefreshDisplayItems called. Running filter query.");

            // Take the original list with items.

            IQueryable<ItemType> query_able = _items_source_list.AsQueryable();

            if (_filter_text.Length > 0)
            {
                // Global Search
                query_able = query_able.Where(i => FindTextInsideItem(i));
            }

            if (_last_sort_column != null)
            {
                var column = _last_sort_column;

                if (_last_sort_mode == ColumnSortMode.Ascending)
                {
                    query_able = query_able.OrderBy(_last_sort_column.FieldExpression);
                }
                else
                {
                    query_able = query_able.OrderByDescending(_last_sort_column.FieldExpression);
                }
            }

            _display_items = query_able.ToList();

            //if (this.UseMultiCheck && CheckedWasSilentyChanged)
            //    _ = this.CheckedItemsChanged.InvokeAsync();

        }

        /// <summary>
        /// Called by Linq.Where().
        /// When not in Dropdown mode: If  the text is not found in the item, the item will be unchecked. 
        /// </summary>
        private bool FindTextInsideItem(ItemType item)
        {
            for (int col_index = 0; col_index < _columns.Count; col_index++)
            {
                Column<ItemType> column = _columns[col_index];

                string html_string = column.ValueToHtml(item);

                int index = html_string.IndexOf(_filter_text, System.StringComparison.InvariantCultureIgnoreCase);

                if (index != -1)
                    return true;
            }

            // The search text was not found.

            if (this.UseMultiCheck != MultiCheck.Off && this.DropdownMode == false)
            {
                // Uncheck the item as it is not going to be visible
                UncheckItem(item);
            }

            return false;
        }


    }

    public enum ColumnSortMode
    {
        None,
        Ascending,
        Descending
    }

}
