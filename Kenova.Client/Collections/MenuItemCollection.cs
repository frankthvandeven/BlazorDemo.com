using System;

namespace Kenova.Client.Components
{
    public class MenuItemCollection : MonitoredCollection<MenuItem>
    {
        /// <summary>
        /// The priority for the AutoFocus of menu items.
        /// The default value is 100.
        /// The AutoFocusPriority is for all buttons in the Toolbar and is not set per button.
        /// </summary>
        public int AutoFocusPriority { get; set; } = 100;

        /// <summary>
        /// Add a MenuItem to the collection.
        /// </summary>
        /// <param name="identifier">If the focusID string starts with a '/' character it is considered an URL that will be navigated to directly when clicked.</param>
        public void Add(string caption, string identifier, Func<bool> enabledExpression = null, IconKind icon_kind = IconKind.None, string icon_data = null)
        {
            if (enabledExpression == null)
                enabledExpression = () => true;

            var item = new MenuItem();

            item.Identifier = identifier;
            item.Caption = caption;
            item.EnabledExpression = enabledExpression;
            item.Icon.IconKind = icon_kind;
            item.Icon.IconData = icon_data;

            this.Add(item);
        }

        public bool AutoFocus
        {
            get
            {
                if (_current == null)
                    throw new InvalidOperationException("No item selected");

                return _current.AutoFocus;
            }
            set
            {
                if (_current == null)
                    throw new InvalidOperationException("No item selected");

                _current.AutoFocus = value;
            }
        }

        public object Tag
        {
            get
            {
                if (_current == null)
                    throw new InvalidOperationException("No item selected");

                return _current.Tag;
            }
            set
            {
                if (_current == null)
                    throw new InvalidOperationException("No item selected");

                _current.Tag = value;
            }
        }

        public IconDefinition Icon
        {
            get
            {
                if (_current == null)
                    throw new InvalidOperationException("No item selected");

                return _current.Icon;
            }
        }

    }


    public class MenuItem
    {
        public string Identifier { get; set; }

        public string Caption { get; set; }

        public Func<bool> EnabledExpression { get; set; }

        public bool AutoFocus { get; set; }

        public IconDefinition Icon { get; } = new IconDefinition();

        public object Tag { get; set; }

        public string StagingAreaID { get; private set; } = KenovaClientConfig.GetUniqueElementID();


    }

}
