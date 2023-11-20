using System;

namespace Kenova.Client.Components
{
    public class PortalMenuItemCollection : MonitoredCollection<PortalMenuItem>
    {

        public void Add(string caption, string url, Func<bool> enabledExpression = null, IconKind icon_kind = IconKind.None, string icon_data = null)
        {
            if (enabledExpression == null)
                enabledExpression = () => true;

            var item = new PortalMenuItem();

            item.Caption = caption;
            item.URL = url;
            item.EnabledExpression = enabledExpression;
            item.Icon.IconKind = icon_kind;
            item.Icon.IconData = icon_data;

            this.Add(item);
        }

        public void Add(string caption, Type component_type, Func<bool> enabledExpression = null, IconKind icon_kind = IconKind.None, string icon_data = null)
        {
            if (enabledExpression == null)
                enabledExpression = () => true;

            var item = new PortalMenuItem();

            item.Caption = caption;
            item.ComponentType = component_type;
            item.EnabledExpression = enabledExpression;
            item.Icon.IconKind = icon_kind;
            item.Icon.IconData = icon_data;

            this.Add(item);
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


    public class PortalMenuItem
    {
        public string Caption { get; set; }
        public string URL { get; set; }
        public Type ComponentType { get; set; }
        public Func<bool> EnabledExpression { get; set; }
        public IconDefinition Icon { get; } = new IconDefinition();

    }

}
