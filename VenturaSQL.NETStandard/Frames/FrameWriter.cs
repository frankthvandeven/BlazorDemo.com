using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace VenturaSQL
{

    /// <summary>
    /// Use the frameStream to prepare data frames for sending to Http. 
    /// </summary>
    public class FrameWriter : IDisposable
    {
        public const int MEMORYSTREAM_SIZE = 262144;

        private int _frameStartPos;
        private int _firstFrameStartPos;

        private bool _InsideFrame = false;

        private bool _closed = false;

        private MemoryStream _memorystream;

        private byte[] _buffer; // temp space for writing primitives to.
        private byte[] _largeByteBuffer; // temp space for writing large data to.
        private Encoding _encoding;
        private Encoder _encoder;
        private int _bytesPerCharacter;

        public FrameWriter(MemoryStream memorystream)
        {
            _memorystream = memorystream;

            _encoding = new UTF8Encoding(false, true);
            _encoder = _encoding.GetEncoder();
            _bytesPerCharacter = _encoding.GetMaxByteCount(1);

            _buffer = new byte[16];

            // this we do so we can skip the length check in WriteString8()
            int minBufSize = byte.MaxValue * _bytesPerCharacter;

            _largeByteBuffer = new byte[Math.Max(minBufSize, (int)4096)];

            // Beginning of header.

            // The marker FV (2 bytes)
            _memorystream.WriteByte((byte)70); // the file type marker
            _memorystream.WriteByte((byte)86); // the file type marker

            // This platform (1 byte)
            _memorystream.WriteByte((byte)VenturaSqlPlatform.NETStandard);

            // The version (12 bytes)
            Version vrsn = General.VenturaSqlVersion;
            this.Write(vrsn.Major);
            this.Write(vrsn.Minor);
            this.Write(vrsn.Build);

            // End of header

            _firstFrameStartPos = (int)_memorystream.Position;
        }


        /// <summary>
        /// Deletes all frames that were written so far.
        /// </summary>
        public void ZapFrames()
        {
            _InsideFrame = false;
            _memorystream.SetLength(_firstFrameStartPos);
        }

        public long Position
        {
            get { return _memorystream.Position; }
            set { _memorystream.Position = value; }
        }

        /// <summary>
        /// Starts a frame. 
        ///
        /// In a server situation you have to keep in mind that like 25 threads are building frames
        /// in parallel. Calculate the expected memory requirements. Don't go overboard.
        ///
        /// The length of the frame header is 5 bytes.
        /// </summary>
        public void StartFrame(FrameType frameType)
        {
            _InsideFrame = true;
            _frameStartPos = (int)_memorystream.Position;

            // Write the frametype as a byte
            _memorystream.WriteByte((byte)frameType);

            // Reserve 4 bytes for containing the length of the frame
            _memorystream.Write(new byte[4], 0, 4);

        } // end of function StartFrame


        /// <summary>
        /// Ends the frame.
        /// </summary>
        public void EndFrame()
        {
            _InsideFrame = false;

            int payloadlength = (int)_memorystream.Position - _frameStartPos - 5;

            byte[] temparr = BitConverter.GetBytes((int)payloadlength);

            // Tijdelijke vervanging:
            long position = _memorystream.Position;

            _memorystream.Position = _frameStartPos + 1; /* was 2 */
            _memorystream.Write(temparr, 0, 4);

            _memorystream.Position = position;

            // Rechtstreeks in buffer schrijven is uitgeschakeld wegens ontbreken van MemoryStream.GetBuffer() in WinRT
            //tempbuffer[ _frameStartPos + 2 ] = temparr[0];
            //tempbuffer[ _frameStartPos + 3 ] = temparr[1];
            //tempbuffer[ _frameStartPos + 4 ] = temparr[2];
            //tempbuffer[ _frameStartPos + 5 ] = temparr[3];

        } // end of EndFrame method

        /// <summary>
        /// Aborts the started frame.
        /// </summary>
        public void AbortCurrentFrame()
        {

            if (_InsideFrame == true)
            {
                _InsideFrame = false;

                // undo the open frame
                _memorystream.SetLength(_frameStartPos);
            } // end if

        } // end of function AbortCurrentFrame

        public void WriteString8(string value)
        {
            byte encodedlength = (byte)_encoding.GetBytes(value, 0, value.Length, _largeByteBuffer, 0);

            // Write the encoded length as an 8bit int
            _memorystream.WriteByte(encodedlength);

            // Write the encoded string
            _memorystream.Write(_largeByteBuffer, 0, encodedlength);
        }

        // TODO: make the string length unsigned
        public void WriteString16(string value)
        {
            int maxBufferNeeded = value.Length * _bytesPerCharacter;

            if (_largeByteBuffer.Length < maxBufferNeeded)
                _largeByteBuffer = new byte[maxBufferNeeded];

            UInt16 encodedlength = (UInt16)_encoding.GetBytes(value, 0, value.Length, _largeByteBuffer, 0);

            // Write the encoded length as a 16bit int
            _buffer[0] = ((byte)(encodedlength));
            _buffer[1] = ((byte)(encodedlength >> 8));
            _memorystream.Write(_buffer, 0, 2);

            // Write the encoded string
            _memorystream.Write(_largeByteBuffer, 0, encodedlength);
        }

        public void WriteString32(string value)
        {
            int maxBufferNeeded = value.Length * _bytesPerCharacter;

            if (_largeByteBuffer.Length < maxBufferNeeded)
                _largeByteBuffer = new byte[maxBufferNeeded];

            int encodedlength = _encoding.GetBytes(value, 0, value.Length, _largeByteBuffer, 0);

            // Write the encoded length as a 32bit int
            _buffer[0] = ((byte)(encodedlength));
            _buffer[1] = ((byte)(encodedlength >> 8));
            _buffer[2] = ((byte)(encodedlength >> 16));
            _buffer[3] = ((byte)(encodedlength >> 24));
            _memorystream.Write(_buffer, 0, 4);

            // Write the encoded string
            _memorystream.Write(_largeByteBuffer, 0, encodedlength);
        }

        public void WriteDateTime(DateTime data)
        {
            byte[] bytearray = BitConverter.GetBytes(data.Ticks);
            _memorystream.Write(bytearray, 0, 8);
        }

        public void WriteTimeSpan(TimeSpan data)
        {
            byte[] bytearray = BitConverter.GetBytes(data.Ticks);
            _memorystream.Write(bytearray, 0, 8);
        }

        public void WriteDateTimeOffset(DateTimeOffset dto)
        {
            byte[] bytearray = BitConverter.GetBytes(dto.Ticks);
            _memorystream.Write(bytearray, 0, 8);

            bytearray = BitConverter.GetBytes((short)dto.Offset.TotalMinutes);
            _memorystream.Write(bytearray, 0, 2);
        }

        /// <summary>
        /// Writes Exception information to the FrameStream
        /// </summary>
        private void WriteException(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception", "Parameter cannot be null.");

            this.WriteString32(exception.Message);

            if (exception.StackTrace != null)
                WriteString32(exception.StackTrace);
            else
                WriteString32("(no stacktrace)");

            if (exception.Source != null)
                WriteString32(exception.Source);
            else
                WriteString32("(no source)");

        }

        public void AddSuccessFrame(string message)
        {
            byte[] Value = Encoding.UTF8.GetBytes(message);
            short Length = (short)Value.Length;

            this.AbortCurrentFrame();
            this.StartFrame(FrameType.SuccessMessage);
            _memorystream.Write(BitConverter.GetBytes((short)Length), 0, 2);
            _memorystream.Write(Value, 0, (int)Length);
            this.EndFrame();

        } // end of function

        public void AddInfoFrame(string message)
        {

            byte[] Value = Encoding.UTF8.GetBytes(message);
            short Length = (short)Value.Length;

            this.AbortCurrentFrame();
            this.StartFrame(FrameType.InfoMessage);
            _memorystream.Write(BitConverter.GetBytes((short)Length), 0, 2);
            _memorystream.Write(Value, 0, (int)Length);
            this.EndFrame();

        } // end of function

        public void AddWarningFrame(string message)
        {
            byte[] Value = Encoding.UTF8.GetBytes(message);
            short Length = (short)Value.Length;

            this.AbortCurrentFrame();
            this.StartFrame(FrameType.WarningMessage);
            _memorystream.Write(BitConverter.GetBytes((short)Length), 0, 2);
            _memorystream.Write(Value, 0, (int)Length);
            this.EndFrame();

        } // end of function

        public void AddErrorFrame(string message)
        {
            byte[] Value = Encoding.UTF8.GetBytes(message);
            short Length = (short)Value.Length;

            this.AbortCurrentFrame();
            this.StartFrame(FrameType.ErrorMessage);
            _memorystream.Write(BitConverter.GetBytes((short)Length), 0, 2);
            _memorystream.Write(Value, 0, (int)Length);
            this.EndFrame();

        } // end of function


        /// <summary>
        /// 
        /// </summary>
        public void AddExceptionFrame(Exception exception)
        {

            this.AbortCurrentFrame();

            this.StartFrame(FrameType.Exception);

            if (exception.InnerException == null)
                _memorystream.WriteByte(0);
            else
            {
                _memorystream.WriteByte(1);
                this.WriteException(exception.InnerException);
            }

            this.WriteException(exception);
            this.EndFrame();

        } // end of function

        // Writes a boolean to this stream. A single byte is written to the stream
        // with the value 0 representing false or the value 1 representing true.
        public void Write(bool value)
        {
            _buffer[0] = (byte)(value ? 1 : 0);
            _memorystream.Write(_buffer, 0, 1);
        }

        /// <summary>
        /// Equal to Write(byte value). For compatibility with Stream
        /// </summary>
        public void WriteByte(byte value)
        {
            _memorystream.WriteByte(value);
        }

        // Writes a byte to this stream. The current position of the stream is
        // advanced by one.
        public void Write(byte value)
        {
            _memorystream.WriteByte(value);
        }

        // Writes a signed byte to this stream. The current position of the stream 
        // is advanced by one.
        public void Write(sbyte value)
        {
            _memorystream.WriteByte((byte)value);
        }

        // Writes a byte array to this stream.
        // 
        // This default implementation calls the Write(Object, int, int)
        // method to write the byte array.
        public void Write(byte[] buffer)
        {
            _memorystream.Write(buffer, 0, buffer.Length);
        }

        // Writes a section of a byte array to this stream.
        //
        // This default implementation calls the Write(Object, int, int)
        // method to write the byte array.
        public void Write(byte[] buffer, int index, int count)
        {
            _memorystream.Write(buffer, index, count);
        }

        /*
        // Writes a character to this stream. The current position of the stream is
        // advanced by two.
        // Note this method cannot handle surrogates properly in UTF-8.
        public unsafe void Write(char ch) {
            if (Char.IsSurrogate(ch))
                throw new ArgumentException("Arg_SurrogatesNotAllowedAsSingleChar");
 
            //Contract.Assert(_encoding.GetMaxByteCount(1) <= 16, "_encoding.GetMaxByteCount(1) <= 16)");

            int numBytes = 0;
            fixed (byte* pBytes = _buffer)
            {
                numBytes = _encoder.GetBytes(&ch, 1, pBytes, 16, true);
            }
            _memorystream.Write(_buffer, 0, numBytes);
        }
        */

        // Writes a character array to this stream.
        // 
        // This default implementation calls the Write(Object, int, int)
        // method to write the character array.
        public void Write(char[] chars)
        {
            byte[] bytes = _encoding.GetBytes(chars, 0, chars.Length);
            _memorystream.Write(bytes, 0, bytes.Length);
        }

        // Writes a section of a character array to this stream.
        //
        // This default implementation calls the Write(Object, int, int)
        // method to write the character array.
        public void Write(char[] chars, int index, int count)
        {
            byte[] bytes = _encoding.GetBytes(chars, index, count);
            _memorystream.Write(bytes, 0, bytes.Length);
        }


        // Writes a double to this stream. The current position of the stream is
        // advanced by eight.
        public unsafe void Write(double value)
        {
            ulong TmpValue = *(ulong*)&value;
            _buffer[0] = (byte)TmpValue;
            _buffer[1] = (byte)(TmpValue >> 8);
            _buffer[2] = (byte)(TmpValue >> 16);
            _buffer[3] = (byte)(TmpValue >> 24);
            _buffer[4] = (byte)(TmpValue >> 32);
            _buffer[5] = (byte)(TmpValue >> 40);
            _buffer[6] = (byte)(TmpValue >> 48);
            _buffer[7] = (byte)(TmpValue >> 56);
            _memorystream.Write(_buffer, 0, 8);
        }

        public void Write(decimal value)
        {

            unsafe
            {
                fixed (byte* local1 = this._buffer)
                    *((decimal*)local1) = value;

                _memorystream.Write(this._buffer, 0, 16);
            }

            // CLASSIC METHOD:

            //int[] temp = Decimal.GetBits(value);

            //int lo = temp[0];
            //int mid = temp[1];
            //int hi = temp[2];
            //int flags = temp[3];

            //_buffer[0] = (byte)lo;
            //_buffer[1] = (byte)(lo >> 8);
            //_buffer[2] = (byte)(lo >> 16);
            //_buffer[3] = (byte)(lo >> 24);

            //_buffer[4] = (byte)mid;
            //_buffer[5] = (byte)(mid >> 8);
            //_buffer[6] = (byte)(mid >> 16);
            //_buffer[7] = (byte)(mid >> 24);

            //_buffer[8] = (byte)hi;
            //_buffer[9] = (byte)(hi >> 8);
            //_buffer[10] = (byte)(hi >> 16);
            //_buffer[11] = (byte)(hi >> 24);

            //_buffer[12] = (byte)flags;
            //_buffer[13] = (byte)(flags >> 8);
            //_buffer[14] = (byte)(flags >> 16);
            //_buffer[15] = (byte)(flags >> 24);

            //_memorystream.Write(_buffer, 0, 16);
        }

        // Writes a two-byte signed integer to this stream. The current position of
        // the stream is advanced by two.
        // 
        public void Write(short value)
        {
            _buffer[0] = (byte)value;
            _buffer[1] = (byte)(value >> 8);
            _memorystream.Write(_buffer, 0, 2);
        }

        // Writes a two-byte unsigned integer to this stream. The current position
        // of the stream is advanced by two.
        public void Write(ushort value)
        {
            _buffer[0] = (byte)value;
            _buffer[1] = (byte)(value >> 8);
            _memorystream.Write(_buffer, 0, 2);
        }

        // Writes a four-byte signed integer to this stream. The current position
        // of the stream is advanced by four.
        public void Write(int value)
        {
            _buffer[0] = (byte)value;
            _buffer[1] = (byte)(value >> 8);
            _buffer[2] = (byte)(value >> 16);
            _buffer[3] = (byte)(value >> 24);
            _memorystream.Write(_buffer, 0, 4);
        }

        // Writes a four-byte unsigned integer to this stream. The current position
        // of the stream is advanced by four.
        public void Write(uint value)
        {
            _buffer[0] = (byte)value;
            _buffer[1] = (byte)(value >> 8);
            _buffer[2] = (byte)(value >> 16);
            _buffer[3] = (byte)(value >> 24);
            _memorystream.Write(_buffer, 0, 4);
        }

        // Writes an eight-byte signed integer to this stream. The current position
        // of the stream is advanced by eight.
        public void Write(long value)
        {
            _buffer[0] = (byte)value;
            _buffer[1] = (byte)(value >> 8);
            _buffer[2] = (byte)(value >> 16);
            _buffer[3] = (byte)(value >> 24);
            _buffer[4] = (byte)(value >> 32);
            _buffer[5] = (byte)(value >> 40);
            _buffer[6] = (byte)(value >> 48);
            _buffer[7] = (byte)(value >> 56);
            _memorystream.Write(_buffer, 0, 8);
        }

        // Writes an eight-byte unsigned integer to this stream. The current 
        // position of the stream is advanced by eight.
        public void Write(ulong value)
        {
            _buffer[0] = (byte)value;
            _buffer[1] = (byte)(value >> 8);
            _buffer[2] = (byte)(value >> 16);
            _buffer[3] = (byte)(value >> 24);
            _buffer[4] = (byte)(value >> 32);
            _buffer[5] = (byte)(value >> 40);
            _buffer[6] = (byte)(value >> 48);
            _buffer[7] = (byte)(value >> 56);
            _memorystream.Write(_buffer, 0, 8);
        }

        // Writes a float to this stream. The current position of the stream is
        // advanced by four.
        public unsafe void Write(float value)
        {
            uint TmpValue = *(uint*)&value;
            _buffer[0] = (byte)TmpValue;
            _buffer[1] = (byte)(TmpValue >> 8);
            _buffer[2] = (byte)(TmpValue >> 16);
            _buffer[3] = (byte)(TmpValue >> 24);
            _memorystream.Write(_buffer, 0, 4);
        }

        public void WriteObject(object value)
        {
            using (MemoryStream temp_memorystream = new MemoryStream())
            {
                BinaryFormatter binaryformatter = new BinaryFormatter();
                binaryformatter.Serialize(temp_memorystream, value);

                temp_memorystream.Position = 0;

                this.Write((int)temp_memorystream.Length);
                temp_memorystream.CopyTo(_memorystream);
            }
        }

        /* Alternative: Write the object as a byte array, skipping serialization
        public void WriteObject(object value)
        {
            if (!(value is byte[]))
                throw new Exception("FrameWriter: This platform expects a byte[] as the WriteObject() parameter.");

            byte[] value_array = (byte[])value;

            this.Write((int)value_array.Length);

            _memorystream.Write(value_array, 0, value_array.Length);
        }
        */

        /// <summary>
        /// Flushes the buffer and writes statistics into the header of the output stream.
        ///
        /// The MemoryStream.Position will be set to 0.
        /// 
        /// You cannot reuse a FrameWriter object after closing it.
        /// </summary>
        public void Close()
        {
            if (_closed == true)
                return;

            if (_InsideFrame == true)
                throw new InvalidOperationException("Can not call Close while a frame is open, close the frame first with EndFrame()");

            // Make sure cached data is flushed.
            _memorystream.Flush();

            // Reset position to zero.
            _memorystream.Position = 0;
            
            // Unlink the memorystream.
            _memorystream = null;

            // Framestream is finished now, cannot use this class instance anymore.
            _closed = true;

        }

        public void Dispose()
        {
            this.Close();
        }
    } // End of class
} // end of namespace
