using System;
//using System.Text;
//using System.Runtime.InteropServices;

namespace VenturaSQL
{

	public class VenturaSqlException : Exception
	{

		public VenturaSqlException() : base("VenturaSQL Exception")
		{
		}

		public VenturaSqlException ( string message ) : base ( message )
		{
		}

        public VenturaSqlException(string message, Exception innerException) : base(message, innerException)
        {
        }

	}


}
