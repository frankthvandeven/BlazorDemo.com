
namespace Kenova.Client.Components
{
    public class OptionItemCollection : MonitoredCollection<OptionItem>
    {


        public void Add(string focusID, string caption, bool enabled = true)
        {
            var item = new OptionItem();

            item.FocusID = focusID;
            item.Caption = caption;
            item.Enabled = enabled;

            this.Add(item);
        }

    }

    public class OptionItem
    {
        public string FocusID { get; set; }

        public string Caption { get; set; }

        public bool Enabled { get; set; } = true;
    }

}
