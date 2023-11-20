namespace Kenova.Client.Components
{
    public class NotificationItem
    {
        private string _key = KenovaClientConfig.GetUniqueElementID();

        public string Key
        {
            get { return _key; }
        }

        public string Message;

        public bool ToastVisible = true;


    }
}
