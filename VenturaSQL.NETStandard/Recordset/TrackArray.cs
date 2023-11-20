using System;

namespace VenturaSQL
{

    /// <summary>
    /// This class is used to store changed data from a Record.
    /// It is used by VenturaSQL as an intermediate between a Record and the DBMS.
    /// TrackArray is related to INSERT/UPDATE/DELETE Sql statements.
    /// </summary>
    public class TrackArray
    {
        private VenturaSqlSchema _schema;

        private TrackArrayStatus _status;

        private short _data_count;
        private object[] _data_values;
        private short[] _data_ordinals;

        private short _prikey_count;
        private object[] _prikey_values;
        private short[] _prikey_ordinals;

        public TrackArray(VenturaSqlSchema schema)
        {
            _schema = schema;

            _status = TrackArrayStatus.Empty;

            _data_count = 0;
            _data_values = new object[schema.Count];
            _data_ordinals = new short[schema.Count];

            _prikey_count = 0;
            _prikey_values = new object[schema.Count];
            _prikey_ordinals = new short[schema.Count];
        }

        public void Reset()
        {
            _status = TrackArrayStatus.Empty;
            _data_count = 0;
            _prikey_count = 0;
        }

        public TrackArrayStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public bool HasData
        {
            get { return _data_count > 0 || _prikey_count > 0; }
        }

        public void AppendDataValue(short ordinal, object value)
        {
            _data_values[_data_count] = value;
            _data_ordinals[_data_count] = ordinal;

            _data_count++;
        }

        public void AppendPrikeyValue(short ordinal, object value)
        {
            _prikey_values[_prikey_count] = value;
            _prikey_ordinals[_prikey_count] = ordinal;

            _prikey_count++;
        }

        /// <summary>
        /// The number of DataValues/DataValueColumns that are in use.
        /// </summary>
        public short DataValueCount
        {
            get { return _data_count; }
        }

        public object[] DataValues
        {
            get { return _data_values; }
        }

        public short[] DataValueOrdinals
        {
            get { return _data_ordinals; }
        }

        /// <summary>
        /// The number of PrikeyValues/PrikeyColumns that are in use.
        /// </summary>
        public short PrikeyCount
        {
            get { return _prikey_count; }
        }

        public object[] PrikeyValues
        {
            get { return _prikey_values; }
        }

        public short[] PrikeyOrdinals
        {
            get { return _prikey_ordinals; }
        }

        public void WriteToFrame(int rowindex, FrameWriter framewriter)
        {
            if (_status == TrackArrayStatus.Empty)
                return; // Nothing to do.

            framewriter.StartFrame(FrameType.TrackArray);

            framewriter.Write(rowindex);

            framewriter.Write((byte)_status); // INSERT/UPDATE/DELETE

            // Step 1. Data value nulls
            for (short i = 0; i < _data_count; i++)
            {
                if (_data_values[i] == null)
                    framewriter.Write(_data_ordinals[i]);
            }

            framewriter.Write(short.MaxValue);

            // Step 2. Data values (the not-nulls)
            for (int i = 0; i < _data_count; i++)
            {
                if (_data_values[i] != null)
                {
                    framewriter.Write(_data_ordinals[i]);
                    _schema.ObjectValue2Frame(framewriter, _data_ordinals[i], _data_values[i]); // Write the column data.
                }
            }

            framewriter.Write(short.MaxValue);

            // Step 3. Prikey nulls
            for (int i = 0; i < _prikey_count; i++)
            {
                if (_prikey_values[i] == null)
                    framewriter.Write(_prikey_ordinals[i]);
            }

            framewriter.Write(short.MaxValue);

            // Step 4. Prikey values (the not-nulls)
            for (int i = 0; i < _prikey_count; i++)
            {
                if (_prikey_values[i] != null)
                {
                    framewriter.Write(_prikey_ordinals[i]);
                    _schema.ObjectValue2Frame(framewriter, _prikey_ordinals[i], _prikey_values[i]); // Write the column data.
                }
            }

            framewriter.Write(short.MaxValue);

            framewriter.EndFrame();

        }

        public void ReadFrame(byte[] ReceiveBuffer, ref int PayloadOffset, out int RowIndex)
        {
            RowIndex = BitConverter.ToInt32(ReceiveBuffer, PayloadOffset);
            PayloadOffset += 4;

            _status = (TrackArrayStatus)ReceiveBuffer[PayloadOffset];
            PayloadOffset++;

            _data_count = 0;
            _prikey_count = 0;

            // Step 1. Data values nulls
            while (true)
            {
                short ordinal = BitConverter.ToInt16(ReceiveBuffer, PayloadOffset);
                PayloadOffset += 2;

                if (ordinal == short.MaxValue)
                    break;

                _data_values[_data_count] = null;
                _data_ordinals[_data_count] = ordinal;
                _data_count++;

            } // end while loop

            // Step 2. Data values (the not-nulls)
            while (true)
            {
                short ordinal = BitConverter.ToInt16(ReceiveBuffer, PayloadOffset);
                PayloadOffset += 2;

                if (ordinal == short.MaxValue)
                    break;

                _data_values[_data_count] = _schema.Frame2ObjectValue(ordinal, ReceiveBuffer, ref PayloadOffset);
                _data_ordinals[_data_count] = ordinal;
                _data_count++;

            } // end while loop

            // Step 3. Prikey nulls
            while (true)
            {
                short ordinal = BitConverter.ToInt16(ReceiveBuffer, PayloadOffset);
                PayloadOffset += 2;

                if (ordinal == short.MaxValue) // 32767 indicates we are done receiving ordinals.
                    break;

                _prikey_values[_prikey_count] = null;
                _prikey_ordinals[_prikey_count] = ordinal;
                _prikey_count++;

            } // end while loop

            // Step 4. Prikey values (the not-nulls)
            while (true)
            {
                short ordinal = BitConverter.ToInt16(ReceiveBuffer, PayloadOffset);
                PayloadOffset += 2;

                if (ordinal == short.MaxValue) // 32767 indicates we are done receiving ordinals.
                    break;

                _prikey_values[_prikey_count] = _schema.Frame2ObjectValue(ordinal, ReceiveBuffer, ref PayloadOffset);
                _prikey_ordinals[_prikey_count] = ordinal;
                _prikey_count++;

            } // end while loop

        }

    } // end of class
} // end of namespace
