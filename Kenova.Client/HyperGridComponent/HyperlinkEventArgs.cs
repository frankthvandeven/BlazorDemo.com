using System;

namespace Kenova.Client.Components
{
    public class HyperlinkEventArgs<ItemType> : EventArgs
    {
        private ItemType _item;
        private string _fieldName;
        private string _elementId;

        internal HyperlinkEventArgs(ItemType item, string fieldName, string elementId)
        {
            _item = item;
            _fieldName = fieldName;
            _elementId = elementId;
        }

        public ItemType Item
        {
            get { return _item; }
        }

        public string FieldName
        {
            get { return _fieldName; }
        }

        public string ElementId
        {
            get { return _elementId; }
        }

    }
}
