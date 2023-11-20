using System.Collections.Generic;

namespace Kenova.Client.Components
{
    public static class NotificationManager
    {
        private static List<NotificationItem> _list = new();


        static NotificationManager()
        {
            _list.Add(new NotificationItem { Message = "Welcome to Kenova. Toast messages are under construction." });
            _list.Add(new NotificationItem { Message = "The message will disappear after 4 seconds." });

        }


        public static List<NotificationItem> NotificationList
        {
            get { return _list; }
        }

    }

}

