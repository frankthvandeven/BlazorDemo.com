namespace VenturaSQL
{
    internal class ClientFrameReader : FrameReaderBase
    {
        private IRecordsetBase _current_loader; // the current loader
        private IResultsetBase _current_resultset; // the current recordset
        private IRecordsetBase[] _loaders;

        private IRecordsetIncremental _incr_loader;

        private int _local_rowindex = 0;
        private int _rowcount;

        internal ClientFrameReader(IRecordsetBase[] loaders)
        {
            _loaders = loaders;
        }

        protected override void Init()
        {

        }

        protected override void Dispose()
        {

        }

        protected override void FrameHandler(FrameType frametype, int payloadlength)
        {

            switch (frametype)
            {
                case FrameType.Record_Unchanged:
                    object[] columnvalues = _current_resultset.Schema.Frame2ObjectArray(_buffer, ref _position);

                    _current_resultset.OptimizedCreateAndSetExistingRecord(_local_rowindex, columnvalues);
                    _local_rowindex++;
                    _rowcount++;

                    break;
                case FrameType.IdentityColumnValue:
                    int client_index = this.ReadInt32(); // The server returns the client-index

                    object value_object = null;

                    if (payloadlength > 4) // It is not a null.
                        value_object = _current_resultset.Schema.Frame2ObjectValue(_current_resultset.Schema.IdentityColumn.ColumnOrdinal, _buffer, ref _position);

                    _current_resultset[client_index].SetIdentityColumnValue(value_object);

                    break;
                case FrameType.SelectLoader:
                    int loader_index = this.ReadInt32();

                    _current_loader = _loaders[loader_index];
                    _incr_loader = _current_loader as IRecordsetIncremental;
                    break;

                case FrameType.SelectResultset:
                    int resultset_index = this.ReadInt32();

                    _current_resultset = _current_loader.Resultsets[resultset_index];
                    _local_rowindex = _current_resultset.Length;
                    _rowcount = 0;

                    break;

                case FrameType.IncreaseResultsetCapacity:
                    int additional = this.ReadInt32();
                    _current_resultset.IncreaseCapacity(additional);
                    break;

                case FrameType.SetOutputParameters:
                    _current_loader.ParameterSchema.Frame2ObjectArray(_current_loader.OutputParameterValues, _buffer, ref _position);
                    break;


                case FrameType.UnselectResultset:

                    if (_incr_loader != null)
                    {
                        _incr_loader.LastExecCount = _rowcount;
                        _incr_loader.LastExecStartIndex = _rowcount == 0 ? -1 : (_local_rowindex - _rowcount);
                        _incr_loader.HasMoreRows = (_rowcount >= _current_loader.RowLimit) ? true : false;
                    }

                    break;
                case FrameType.SetRowOffset: // will only be set for IRecordsetIncremental 
                    int offset = this.ReadInt32(); ;

                    if (_incr_loader != null)
                    {
                        _incr_loader.IncrementalOffset = offset;
                    }

                    break;

                case FrameType.Exception:
                    throw this.ReadRemoteException();
            }
        }


    }
}
