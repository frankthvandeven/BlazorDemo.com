using System;

namespace VenturaSQL
{
    public class HttpConnector : Connector
    {
        private string _remoteName;
        private string _url; // max 512 characters

        public HttpConnector(string remoteName, string url)
        {
            _remoteName = remoteName;
            _url = url;
        }

        /// <summary>
        /// 
        /// </summary>
        public string RemoteName
        {
            get { return _remoteName; }
        }

        public string Url
        {
            get { return _url; }
        }

    }
}
