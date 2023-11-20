
using System;

namespace Kenova.Client.Components
{
    public class TabItemCollection : MonitoredCollection<TabItem>
    {


        public void Add(string identifier, string caption, Func<bool> enabledExpression = null)
        {
            if (enabledExpression == null)
                enabledExpression = () => true;

            var item = new TabItem();

            item.Identifier = identifier;
            item.Caption = caption;
            item.EnabledExpression = enabledExpression;

            this.Add(item);
        }

    }

    public class TabItem
    {
        public string Identifier { get; set; }

        public string Caption { get; set; }

        public Func<bool> EnabledExpression { get; set; }

        public string ContainerID { get; private set; } = KenovaClientConfig.GetUniqueElementID();

        public string StagingAreaID { get; private set; } = KenovaClientConfig.GetUniqueElementID();


    }

}
