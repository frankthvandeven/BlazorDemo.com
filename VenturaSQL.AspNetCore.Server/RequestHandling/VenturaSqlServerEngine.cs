using System;
using System.IO;

namespace VenturaSQL.AspNetCore.Server.RequestHandling
{
    public class VenturaSqlServerEngine
    {
        public FrameReaderCallbacks CallBacks { get; } = new FrameReaderCallbacks();

        /// <summary>
        /// The request data.
        /// </summary>
        public byte[] RequestData { get; set; } = null;

        /// <summary>
        /// The buffer is likely larger than the actual length of the response data.
        /// Only transmit ResponseLength bytes of the ResponseBuffer back to the client.
        /// </summary>
        public byte[] ResponseBuffer { get; private set; } = null;

        /// <summary>
        /// The length of the response data in bytes. The ResponseBuffer is likely larger than ResponseLenght.
        /// Use GetResponseData() to copy ResponseBuffer into a new byte array with the exact length.
        /// </summary>
        public int ResponseLength { get; private set; } = 0;

        public bool ThrewException { get; private set; } = false;

        public Exception Exception { get; private set; } = null;

        /// <summary>
        /// Copies the ResponseBuffer into an array with the correct length (ResponseLength)
        /// </summary>
        public byte[] GetResponseData()
        {
            byte[] copy = new byte[ResponseLength];
            Buffer.BlockCopy(ResponseBuffer, 0, copy, 0, ResponseLength);
            return copy;
        }

        public VenturaSqlServerEngine()
        {
        }

        public void Exec()
        {
            if (this.RequestData is null)
                throw new InvalidOperationException("RequestData is null");

            ResponseLength = 0;
            ResponseBuffer = null;
            ThrewException = false;
            Exception = null;

            // Start processing the request and build the response data.
            MemoryStream responseMemoryStream = new MemoryStream(FrameWriter.MEMORYSTREAM_SIZE);

            FrameWriter frameWriter = new FrameWriter(responseMemoryStream);

            try
            {
                var frameReader = new ServerFrameReader(CallBacks, frameWriter);

                frameReader.Exec(this.RequestData);

            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debugger.Break();

                frameWriter.ZapFrames();

                frameWriter.AddExceptionFrame(ex);

                ThrewException = true;
                Exception = ex;
            }

            frameWriter.Close();

            ResponseBuffer = responseMemoryStream.GetBuffer();
            ResponseLength = (int)responseMemoryStream.Length;

        }
    }
}
