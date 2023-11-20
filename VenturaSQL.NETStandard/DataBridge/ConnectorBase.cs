namespace VenturaSQL
{
    public abstract class Connector
    {
        private object _user_data = null;

        /// <summary>
        /// Gets or sets an object that provides additional data about the connector.
        /// </summary>
        public object Tag
        {
            get { return _user_data; }
            set { _user_data = value; }
        }

    } // class
} // namespace
