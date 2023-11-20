using System;
using System.Net.Http;

namespace VenturaSQL
{

    /// <summary>
    /// Global settings for VenturaSQL.
    /// </summary>
    public static class VenturaSqlConfig
    {
        private static Connector _default_connector = null;
        private static Func<HttpConnector, HttpClient> _factory;

        private static Func<HttpConnector, HttpClient> _default_factory = (HttpConnector connector) => new HttpClient();


        static VenturaSqlConfig()
        {
            _factory = _default_factory;
        }

        /// <summary>
        /// When Recordset.ExecSqlAsync(), Recordset.SaveChangesAsync(), Transactional.ExecSqlAsync() or Transactional.SaveChangesAsync()
        /// is called without setting the connector parameter, VenturaSQL will use the VenturaSqlConfig.DefaultConnector.
        /// </summary>
        public static Connector DefaultConnector
        {
            get { return _default_connector; }
            set { _default_connector = value; }
        }

        internal static HttpClient GetHttpClientFromFactory(HttpConnector connector)
        {
            return _factory(connector);
        }

        public static void ResetHttpClientFactory()
        {
            _factory = _default_factory;
        }

        public static void SetHttpClientFactory(Func<HttpConnector, HttpClient> factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            _factory = factory;
        }

    }
}


