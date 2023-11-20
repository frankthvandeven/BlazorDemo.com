
namespace VenturaSQL.AspNetCore.Server.RequestHandling
{
    public delegate AdoConnector LookupAdoConnectorDelegate(string requestedConnectorname);

    public class FrameReaderCallbacks
    {

        internal FrameReaderCallbacks()
        {
        }

        public LookupAdoConnectorDelegate LookupAdoConnector { get; set; }


    }
}
