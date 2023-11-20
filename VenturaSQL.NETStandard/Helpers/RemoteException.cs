using System;
//using System.Text;
//using System.Runtime.InteropServices;

namespace VenturaSQL
{

	public class RemoteException : Exception
	{
        private string _stacktrace;

        
        internal RemoteException(string message, string source, string stacktrace, Exception innerException) : base(message, innerException)
        {
            _stacktrace = stacktrace;

            this.Source = source;
        }


        public override string StackTrace
        {
            get
            {
                return _stacktrace;
            }
        }



	}


}


// message 
// stacktrace
// this.WriteString32(exception.Source); // source 
//    this.WriteString32(exception.TargetSite.ToString()); // targetsite

// Exception
// InnerException
// BaseException