using System.Collections.ObjectModel;

namespace Kenova.Client.Components
{
    public class TreeViewItem
    {
        private readonly ObservableCollection<TreeViewItem> _children;

        private TreeViewData _owner;
        private TreeViewItem _parent;
        //private TreeKind _kind;
        private string _name;
        private object _data;

        bool _isExpanded = true;
        bool _isSelected = false;

        /// <summary>
        /// The TreeViewItem is considered a folder only when "data" is null.
        /// </summary>
        public TreeViewItem(TreeViewData owner, TreeViewItem parent, string name, object data = null)
        {
            _owner = owner;
            _parent = parent;
            //_kind = kind;
            _name = name;
            _data = data;

            _children = new ObservableCollection<TreeViewItem>();

            //_children.CollectionChanged += _children_CollectionChanged;
        }

        public TreeViewItem Parent
        {
            get { return _parent; }
        }

        public bool IsFolder
        {
            get { return _data == null; }
        }

        public string Name
        {
            get { return _name; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
            }
        }

        public ObservableCollection<TreeViewItem> Children
        {
            get { return _children; }
        }

    }

    //public enum TreeKind
    //{
    //    RootItem = 0,
    //    FolderItem = 10,
    //    DataItem = 20
    //}

}
