using System.Collections.ObjectModel;

namespace Kenova.Client.Components
{
    public abstract class MonitoredCollection<ItemType> : ObservableCollection<ItemType>
    {
        protected ItemType _current = default;

        protected override void InsertItem(int index, ItemType item)
        {
            _current = item;
            base.InsertItem(index, item);
        }

        protected override void ClearItems()
        {
            _current = default;
            base.ClearItems();
        }


    }
}
