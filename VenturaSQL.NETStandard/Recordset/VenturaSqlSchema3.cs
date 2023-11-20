using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary; // Contains BinaryFormatter class.

namespace VenturaSQL
{
    public partial class VenturaSqlSchema
    {
        private byte[] _buffer = new byte[16];

        private int _intvalue;
        private long _longvalue;
        private short _shortvalue;

        /// <summary>
        /// Writes all values in an object array to a FrameStream.
        /// The object array must exactly match the columns in this VenturaSqlSchema.
        /// This method is used for tranmitting the full record with all columns.
        ///
        /// This method is the opposite of Frame2ObjectArray().
        /// </summary>
        public void ObjectArray2Frame(FrameType frametype, FrameWriter framewriter, object[] columnvalues)
        {
            framewriter.StartFrame(frametype);

            for (short ordinal = 0; ordinal < _list.Length; ordinal++)
            {
                object value_object = columnvalues[ordinal];

                if (value_object != null) /* nulls don't need to be sent as this is the default value of a column */
                {
                    framewriter.Write(ordinal); // write column ordinal
                    ObjectValue2Frame(framewriter, ordinal, value_object);
                }
            }

            framewriter.Write(short.MaxValue); // write MaxValue to indicate to parser that is has reached the end

            framewriter.EndFrame();

        } // end of method

        /// <summary>
        /// Converts an object containing a value to it's binary representation
        /// in a FrameStream using the VenturaSqlCode.
        /// Warning: cannot handle nulls!
        /// </summary>
        public void ObjectValue2Frame(FrameWriter framewriter, short ordinal, object valueObject)
        {
            if (valueObject == null)
                throw new ArgumentNullException("valueObject");

            switch (_schemacodes[ordinal])
            {
                case SchemaCode.String:
                    framewriter.WriteString32((string)valueObject);
                    break;
                case SchemaCode.DateTime:
                    framewriter.WriteDateTime((DateTime)valueObject);
                    break;
                case SchemaCode.Boolean:
                    framewriter.WriteByte((byte)(((bool)valueObject) == true ? 1 : 0));
                    break;
                case SchemaCode.Byte:
                    framewriter.WriteByte((byte)valueObject);
                    break;
                case SchemaCode.Decimal:
                    framewriter.Write((decimal)valueObject);
                    break;
                case SchemaCode.Single:
                    float floatvalue = (float)valueObject;

                    unsafe
                    {
                        fixed (byte* local1 = this._buffer)
                        {
                            *((float*)local1) = floatvalue;
                        }
                        framewriter.Write(this._buffer, 0, 4);
                    }
                    break;
                case SchemaCode.Double:
                    double doublevalue = (double)valueObject;

                    unsafe
                    {
                        fixed (byte* local1 = this._buffer)
                        {
                            *((double*)local1) = doublevalue;
                        }
                        framewriter.Write(this._buffer, 0, 8);
                    }
                    break;
                case SchemaCode.Int16:
                    this._shortvalue = (short)valueObject;

                    this._buffer[0] = ((byte)((int)this._shortvalue));
                    this._buffer[1] = ((byte)(this._shortvalue >> 8));
                    framewriter.Write(this._buffer, 0, 2);
                    break;
                case SchemaCode.Int32:
                    this._intvalue = (int)valueObject;
                    this._buffer[0] = ((byte)this._intvalue);
                    this._buffer[1] = ((byte)(this._intvalue >> 8));
                    this._buffer[2] = ((byte)(this._intvalue >> 16));
                    this._buffer[3] = ((byte)(this._intvalue >> 24));
                    framewriter.Write(this._buffer, 0, 4);
                    break;
                case SchemaCode.Int64:
                    this._longvalue = (long)valueObject;

                    this._buffer[0] = ((byte)((int)this._longvalue));
                    this._buffer[1] = ((byte)(this._longvalue >> 8));
                    this._buffer[2] = ((byte)(this._longvalue >> 16));
                    this._buffer[3] = ((byte)(this._longvalue >> 24));
                    this._buffer[4] = ((byte)(this._longvalue >> 32));
                    this._buffer[5] = ((byte)(this._longvalue >> 40));
                    this._buffer[6] = ((byte)(this._longvalue >> 48));
                    this._buffer[7] = ((byte)(this._longvalue >> 56));
                    framewriter.Write(this._buffer, 0, 8);
                    break;
                case SchemaCode.Guid:
                    framewriter.Write((byte[])((Guid)valueObject).ToByteArray(), 0, 16);
                    break;
                case SchemaCode.Bytes:
                    byte[] value_bytearray = (byte[])valueObject;
                    this._intvalue = value_bytearray.Length;

                    this._buffer[0] = ((byte)this._intvalue);
                    this._buffer[1] = ((byte)(this._intvalue >> 8));
                    this._buffer[2] = ((byte)(this._intvalue >> 16));
                    this._buffer[3] = ((byte)(this._intvalue >> 24));
                    framewriter.Write(this._buffer, 0, 4);

                    framewriter.Write(value_bytearray, 0, this._intvalue);
                    break;
                case SchemaCode.Object:
                    framewriter.WriteObject(valueObject);
                    break;
                case SchemaCode.TimeSpan:
                    framewriter.WriteTimeSpan((TimeSpan)valueObject);
                    break;
                case SchemaCode.DateTimeOffset:
                    framewriter.WriteDateTimeOffset((DateTimeOffset)valueObject);
                    break;

            } // end of switch

        } // end of method

        /// <summary>
        /// Parses a byte array, and writes all values found into an object array.
        /// The object array containing values matches the columns in this VenturaSqlSchema.
        /// This method is used for Recordset.Load() on the Client side.
        ///
        /// This method is the opposite of ObjectArray2Frame().
        /// </summary>
        public object[] Frame2ObjectArray(byte[] ReceiveBuffer, ref int PayloadOffset)
        {
            object[] returndata = new object[_list.Length];

            while (true)
            {
                short ordinal = BitConverter.ToInt16(ReceiveBuffer, PayloadOffset);
                PayloadOffset += 2;

                if (ordinal == short.MaxValue)
                    break;

                returndata[ordinal] = Frame2ObjectValue(ordinal, ReceiveBuffer, ref PayloadOffset);
            } // end while loop

            return returndata;
        } // end of method

        /// <summary>
        /// Parses a byte array, and writes all values found into an object array.
        /// The object array containing values matches the columns in this VenturaSqlSchema.
        /// This method is used for Recordset.Load() on the Client side.
        ///
        /// This method is the opposite of ObjectArray2Frame().
        /// </summary>
        /// <param name="data_array">An object array with the same length a the number of columns in this Schema.</param>
        public void Frame2ObjectArray(object[] data_array, byte[] ReceiveBuffer, ref int PayloadOffset)
        {
            if (data_array.Length != _list.Length)
                throw new ArgumentOutOfRangeException($"data", $"Length must match column length. Expected {_list.Length} but got {data_array.Length}.");

            // Set all to null
            for (int i = 0; i < data_array.Length; i++)
                data_array[i] = null;

            while (true)
            {
                short ordinal = BitConverter.ToInt16(ReceiveBuffer, PayloadOffset);
                PayloadOffset += 2;

                if (ordinal == short.MaxValue)
                    break;

                data_array[ordinal] = Frame2ObjectValue(ordinal, ReceiveBuffer, ref PayloadOffset);
            } // end while loop

        } // end of method

        /// <summary>
        /// Converts a single binary stored value in a byte array to a .Net value.
        /// </summary>
        internal object Frame2ObjectValue(short ordinal, byte[] ReceiveBuffer, ref int PayloadOffset)
        {

            int byte_length = 0;
            byte[] byte_array;
            object returndata = null;

            switch (_schemacodes[ordinal])
            {
                case SchemaCode.String:
                    byte_length = BitConverter.ToInt32(ReceiveBuffer, PayloadOffset);
                    PayloadOffset += 4;
                    returndata = Encoding.UTF8.GetString(ReceiveBuffer, PayloadOffset, byte_length);
                    PayloadOffset += byte_length;
                    break;
                case SchemaCode.Boolean:
                    returndata = BitConverter.ToBoolean(ReceiveBuffer, PayloadOffset);
                    PayloadOffset++;
                    break;
                case SchemaCode.Byte:
                    returndata = ReceiveBuffer[PayloadOffset];
                    PayloadOffset++;
                    break;
                case SchemaCode.DateTime:
                    returndata = new DateTime(BitConverter.ToInt64(ReceiveBuffer, PayloadOffset));
                    PayloadOffset += 8;
                    break;
                case SchemaCode.Decimal:
                    /* There is no BitConverter.ToDecimal, so we solve the problem with pointers */
                    byte_array = new byte[16];
                    Buffer.BlockCopy(ReceiveBuffer, PayloadOffset, byte_array, 0, 16);
                    PayloadOffset += 16;

                    unsafe
                    {
                        fixed (byte* pb = byte_array)
                            returndata = *((decimal*)pb);
                    }
                    break;
                case SchemaCode.Single:
                    returndata = BitConverter.ToSingle(ReceiveBuffer, PayloadOffset);
                    PayloadOffset += 4;
                    break;
                case SchemaCode.Double:
                    returndata = BitConverter.ToDouble(ReceiveBuffer, PayloadOffset);
                    PayloadOffset += 8;
                    break;
                case SchemaCode.Int16:
                    returndata = BitConverter.ToInt16(ReceiveBuffer, PayloadOffset);
                    PayloadOffset += 2;
                    break;
                case SchemaCode.Int32:
                    returndata = BitConverter.ToInt32(ReceiveBuffer, PayloadOffset);
                    PayloadOffset += 4;
                    break;
                case SchemaCode.Int64:
                    returndata = BitConverter.ToInt64(ReceiveBuffer, PayloadOffset);
                    PayloadOffset += 8;
                    break;
                case SchemaCode.Guid:
                    byte_array = new byte[16];
                    Buffer.BlockCopy(ReceiveBuffer, PayloadOffset, byte_array, 0, 16);
                    PayloadOffset += 16;

                    returndata = new Guid(byte_array);
                    break;
                case SchemaCode.Bytes:
                    byte_length = BitConverter.ToInt32(ReceiveBuffer, PayloadOffset);
                    PayloadOffset += 4;

                    byte_array = new byte[byte_length];
                    Buffer.BlockCopy(ReceiveBuffer, PayloadOffset, byte_array, 0, byte_length);
                    PayloadOffset += byte_length;

                    returndata = byte_array;
                    break;
                case SchemaCode.Object: /* Deserialize a CLR-UDT */
                    byte_length = BitConverter.ToInt32(ReceiveBuffer, PayloadOffset);
                    PayloadOffset += 4;

                    MemoryStream memorystream = new MemoryStream(ReceiveBuffer, PayloadOffset, byte_length, false);
                    PayloadOffset += byte_length;

                    memorystream.Position = 0;

                    BinaryFormatter binaryformatter = new BinaryFormatter();
                   
                    returndata = binaryformatter.Deserialize(memorystream);

                    //Alternative: Instead of Deserialize, copy raw data into byte[]
                    //byte_length = BitConverter.ToInt32(ReceiveBuffer, PayloadOffset);
                    //PayloadOffset += 4;
                    //byte_array = new byte[byte_length];
                    //Buffer.BlockCopy(ReceiveBuffer, PayloadOffset, byte_array, 0, byte_length);
                    //PayloadOffset += byte_length;
                    //returndata = byte_array;
                    break;
                case SchemaCode.TimeSpan:
                    returndata = new TimeSpan(BitConverter.ToInt64(ReceiveBuffer, PayloadOffset));
                    PayloadOffset += 8;
                    break;
                case SchemaCode.DateTimeOffset:
                    long ticks = BitConverter.ToInt64(ReceiveBuffer, PayloadOffset);
                    PayloadOffset += 8;

                    short offset = BitConverter.ToInt16(ReceiveBuffer, PayloadOffset);
                    PayloadOffset += 2;

                    returndata = new DateTimeOffset(ticks, TimeSpan.FromMinutes(offset));
                    break;
            } // end of switch

            return returndata;
        } // end of method

    } // end of class
} // end of namespace