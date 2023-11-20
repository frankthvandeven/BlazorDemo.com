using System;

namespace Kenova.Client.Components
{
    public partial class HyperData<ItemType>
    {
        private int _pagesize = 15;
        private int _currentpage = 1;

        /// <summary>
        /// Regular, Virtualization or Pagination.
        /// The default value if this parameter is DisplayMode.Regular.
        /// </summary>
        public DisplayMode Mode { get; set; } = DisplayMode.Regular;

        /// <summary>
        /// The initial scroll position for the data.
        /// The default value is InitialScrollTo.SelectedItem
        /// </summary>
        public InitialScrollTo InitialScrollTo { get; set; } = InitialScrollTo.SelectedItem;


        /// <summary>
        /// PageSize must be 1 or greater.
        /// The default value of this parameter is 15.
        /// </summary>
        public int PageSize
        {
            get { return _pagesize; }
            set
            {
                if (_pagesize == value)
                    return;

                if (_pagesize < 1)
                    _pagesize = 1;

                _pagesize = value;
            }
        }

        public int CurrentPage
        {
            get { return _currentpage; }
        }

        public int PageStartIndex
        {
            get
            {
                if (this.Mode != DisplayMode.Pagination || this._display_items.Count == 0)
                    return 0;

                return (this._currentpage - 1) * this._pagesize;
            }
        }

        public int PageItemCount
        {
            get
            {
                if (this._display_items.Count == 0)
                    return 0;

                if (this.Mode != DisplayMode.Pagination)
                    return _display_items.Count;

                return Math.Min(this._pagesize, this._display_items.Count - this.PageStartIndex);
            }
        }

        public int GetPageCount()
        {
            if (_display_items.Count == 0)
                return 1;

            double i = _display_items.Count / (double)PageSize;

            return (int)Math.Ceiling(i);
        }

        public void SetPagenumberFromItem(ItemType item)
        {
            int index = _display_items.IndexOf(item);

            if (index == -1)
                return;

            SetPagenumberFromIndex(index);
        }

        public void SetPagenumberFromIndex(int index)
        {
            double fraction = index / PageSize;
            int page = (int)Math.Floor(fraction);

            _currentpage = page + 1;

        }

        public void PreviousPage()
        {
            if (_currentpage == 1)
                return;

            _currentpage--;
        }

        public void NextPage()
        {
            if (_currentpage >= GetPageCount())
                return;

            _currentpage++;
        }

        public void FirstPage()
        {
            _currentpage = 1;
        }

        public void LastPage()
        {
            _currentpage = GetPageCount();
        }

    }

    public enum DisplayMode
    {
        /// <summary>
        /// Render all items.
        /// </summary>
        Regular,

        /// <summary>
        /// Render only the items that are in view.
        /// </summary>
        Virtualization,

        /// <summary>
        /// Use pagination.
        /// </summary>
        Pagination
    }

    public enum InitialScrollTo
    {
        None,
        SelectedItem,
        Bottom
    }

}
