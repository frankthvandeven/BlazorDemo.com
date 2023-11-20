using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Kenova.Client.Components
{
    public class TreeViewData
    {

        private List<TreeViewRenderItem> _renderlist = new();

        private ObservableCollection<TreeViewItem> _children = new();

        private TreeViewItem _root;

        public TreeViewData()
        {
            _root = new TreeViewItem(this, null, "");
        }

        public TreeViewItem Root
        {
            get { return _root; }
        }

        /// <summary>
        /// Will always return a TreeViewItem. If the item does not exist, the tree structure will be created.
        /// </summary>
        public TreeViewItem FetchOrCreateFolder(string folderpath) /* method could be moved to TreeViewItem, so it can also start in middle of a tree instead of root */
        {
            string[] parts = folderpath.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

            // We start scanning the object tree from the root level.
            TreeViewItem currentitem = _root;

            foreach (string path_part in parts)
            {
                TreeViewItem childfound = currentitem.Children.FirstOrDefault(child => child.IsFolder && child.Name == path_part);

                if (childfound != null)
                    currentitem = childfound;
                else
                {
                    TreeViewItem newitem = new TreeViewItem(this, currentitem, path_part);
                    insertFolder(currentitem.Children, newitem);
                    currentitem = newitem;
                }

            } // end of parts loop

            return currentitem;
        } // end of method

        private void insertFolder(ObservableCollection<TreeViewItem> children, TreeViewItem newitem)
        {
            bool inserted = false;

            for (int i = 0; i < children.Count; i++)
            {
                var item = children[i];

                if (item.IsFolder == true)
                {
                    if (string.Compare(item.Name, newitem.Name) >= 0)
                    {
                        children.Insert(i, newitem);
                        inserted = true;
                        break;
                    }
                }
            }

            if (!inserted)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    var item = children[i];

                    if (item.IsFolder == false)
                    {
                        children.Insert(i, newitem);
                        inserted = true;
                        break;
                    }
                }
            }

            if (!inserted)
            {
                children.Add(newitem);
            }

        }

        public void AddDataItem(string outputfolder, string name, object data_item)
        {
            if (data_item == null)
                throw new ArgumentNullException("data_item");

            var folderitem = FetchOrCreateFolder(outputfolder);

            var tvi = new TreeViewItem(this, folderitem, name, data_item);

            folderitem.Children.Add(tvi);
        }

        public List<TreeViewRenderItem> CreateRenderList()
        {
            _renderlist.Clear();

            IterateChildren(0, _root);

            return _renderlist;
        }

        /* Simplified reverse loop: for (int i = list.Length; i-- > 0;) */

        private void IterateChildren(int level, TreeViewItem currentitem)
        {
            foreach (TreeViewItem child in currentitem.Children)
            {
                var renderItem = new TreeViewRenderItem
                {
                    Level = level,
                    Name = child.Name,
                    IsFolder = child.IsFolder,
                    IsExpanded = child.IsExpanded,
                    ChildCount = child.Children.Count,
                    Item = child
                };

                _renderlist.Add(renderItem);

                if (child.IsExpanded == true)
                {
                    IterateChildren(level + 1, child);
                }

            }

        }

    }

    public class TreeViewRenderItem
    {
        public int Level;

        public string Name;

        public bool IsFolder;

        public int ChildCount;

        public bool IsExpanded;

        public TreeViewItem Item;

    }




}
