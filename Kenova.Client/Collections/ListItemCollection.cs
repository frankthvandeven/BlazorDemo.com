using System;

namespace Kenova.Client.Components
{
    /// <summary>
    /// A generic collection with Value and Text (for display) inherited from ObservableCollection.
    /// It is used by the DropdownListBasic component.
    /// </summary>
    public class ListItemCollection<ValueType> : MonitoredCollection<ListItem<ValueType>>
    {

        public void Add(ValueType value, string text, bool enabled = true)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            ListItem<ValueType> item = new ListItem<ValueType>
            {
                Value = value,
                Text = text,
                Enabled = enabled
            };

            this.Add(item);
        }

    }

    public class ListItem<ValueType>
    {
        public ValueType Value { get; set; }
        public string Text { get; set; }
        public bool Enabled { get; set; } = true;
    }

}
