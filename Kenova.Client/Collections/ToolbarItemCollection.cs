using System;

namespace Kenova.Client.Components
{
    public class ToolbarItemCollection : MonitoredCollection<ToolbarItem>
    {
        /// <summary>
        /// The priority for the AutoFocus of Toolbar buttons.
        /// The default value is 100.
        /// The AutoFocusPriority is for all buttons in the Toolbar and is not set per button.
        /// </summary>
        public int AutoFocusPriority { get; set; } = 100;

        public void Add(string caption, Action clicked, Func<bool> enabledExpression = null, IconKind icon_kind = IconKind.None, string icon_data = null)
        {
            if (enabledExpression == null)
                enabledExpression = () => true;

            var item = new ToolbarItem();

            item.Caption = caption;
            item.Clicked = clicked;
            item.EnabledExpression = enabledExpression;
            item.Icon.IconKind = icon_kind;
            item.Icon.IconData = icon_data;
            item.Kind = ButtonKind.Regular;

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

        public string FocusID
        {
            get
            {
                if (_current == null)
                    throw new InvalidOperationException("No item selected");

                return _current.FocusID;
            }
            set
            {
                if (_current == null)
                    throw new InvalidOperationException("No item selected");

                _current.FocusID = value;
            }
        }

        public ButtonKind ButtonKind
        {
            get
            {
                if (_current == null)
                    throw new InvalidOperationException("No item selected");

                return _current.Kind;
            }
            set
            {
                if (_current == null)
                    throw new InvalidOperationException("No item selected");

                _current.Kind = value;
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


    public enum ButtonKind
    {
        Regular,
        Default,
        Cancel
    }


    public class ToolbarItem
    {
        public string Caption { get; set; }

        public Action Clicked { get; set; }

        public Func<bool> EnabledExpression { get; set; }

        public IconDefinition Icon { get; } = new IconDefinition();

        public ButtonKind Kind { get; set; } = ButtonKind.Regular;

        public string FocusID { get; set; } = null;

        public bool AutoFocus { get; set; } = false;

        public string ContainerID { get; private set; } = KenovaClientConfig.GetUniqueElementID();

    }

}
