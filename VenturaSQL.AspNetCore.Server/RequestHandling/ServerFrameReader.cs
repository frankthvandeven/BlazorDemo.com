using System;
using System.Data.Common;

namespace VenturaSQL.AspNetCore.Server.RequestHandling
{
    /// <summary>
    /// Processes client requests.
    /// This class will be created new for every request.
    /// Note that this class is not ASP.NET aware. That means no HTTP.
    /// </summary>
    internal class ServerFrameReader : FrameReaderBase
    {
        private FrameWriter _response_framewriter;

        private AdoConnector _connector = null;

        private DbConnection _dbconnection = null;
        private DbTransaction _dbtransaction = null;
        private RowSaver _rowsaver_ado = null;

        private IRecordsetBase _current_loader = null;
        private IResultsetBase _current_resultset = null;

        private int _current_loaderindex; // this is a Client-side index
        private int _current_resultsetindex; // this is a Client-side index

        private TrackArray _trackarray;

        private FrameReaderCallbacks _callbacks;

        internal ServerFrameReader(FrameReaderCallbacks callbacks, FrameWriter response_framewriter)
        {
            _callbacks = callbacks;
            _response_framewriter = response_framewriter;
        }

        protected override void Init()
        {
        }

        protected override void Dispose()
        {
            if (_dbtransaction != null) // if there is a transaction object still alive at this point, it needs to be rolled back.
                _dbtransaction.Dispose();

            General.SmartClose(_dbconnection);
        }

        protected override void FrameHandler(FrameType frametype, int payloadlength)
        {

            switch (frametype)
            {
                case FrameType.TrackArray:

                    _trackarray.ReadFrame(_buffer, ref _position, out int client_rowindex);
                    _rowsaver_ado.Execute(_trackarray);

                    if (_rowsaver_ado.HasIdentityValue)
                    {
                        // Understanding identity columns: http://www.sqlteam.com/article/understanding-identity-columns
                        _response_framewriter.StartFrame(FrameType.IdentityColumnValue);
                        _response_framewriter.Write(client_rowindex);
                        if (_rowsaver_ado.IdentityValue != null)
                            _current_resultset.Schema.ObjectValue2Frame(_response_framewriter, _current_resultset.Schema.IdentityColumn.ColumnOrdinal, _rowsaver_ado.IdentityValue);
                        _response_framewriter.EndFrame();
                    }
                    break;

                case FrameType.SetInputParameters: /* Load parameter values into the currently selected loader. */

                    _current_loader.ParameterSchema.Frame2ObjectArray(_current_loader.InputParameterValues, _buffer, ref _position);
                    break;

                case FrameType.ExecuteSqlScript:

                    var i = _current_loader as IRecordsetIncremental;

                    if( i != null)
                        i.IncrementalOffset = this.ReadInt32();

                    _current_loader.RowLimit = this.ReadInt32();

                    new RowLoaderFramestream().Exec(_connector, _dbconnection, _dbtransaction, _current_loader, _response_framewriter);
                    break;

                case FrameType.InstantiateLoader: /* Instantiate a recordset object and selects it as the current recordset. */

                    byte[] hash = this.ReadBytes(16);
                    string fullclassname = this.ReadString16();
                    _current_loaderindex = this.ReadInt32();

                    // Find the matching recordset compiled into this application.
                    IRecordsetBase loader = RecordsetFinder.FindLoader(hash, fullclassname);

                    if (loader == null)
                        throw new VenturaSqlException($"Recordset {fullclassname} not found on remote system.");

                    _current_loader = loader;

                    _response_framewriter.StartFrame(FrameType.SelectLoader);
                    _response_framewriter.Write(_current_loaderindex);
                    _response_framewriter.EndFrame();

                    break;

                case FrameType.SelectResultset:

                    _current_resultsetindex = this.ReadInt32();
                    _current_resultset = _current_loader.Resultsets[_current_resultsetindex];

                    _response_framewriter.StartFrame(FrameType.SelectResultset);
                    _response_framewriter.Write(_current_resultsetindex);
                    _response_framewriter.EndFrame();
                    break;

                case FrameType.Instantiate_RowSaver_and_TrackArray:

                    _rowsaver_ado = new RowSaver(_connector, _dbconnection, _dbtransaction, _current_resultset.Schema, _current_resultset.UpdateableTablename);
                    _trackarray = new TrackArray(_current_resultset.Schema);
                    break;

                case FrameType.OpenSqlConnection:

                    OpenSqlConnection();
                    break;

                case FrameType.CloseSqlConnection:

                    _dbconnection.Close();
                    _connector = null;
                    break;

                case FrameType.StartTransaction:

                    _dbtransaction = _dbconnection.BeginTransaction();
                    break;

                case FrameType.CommitTransaction:

                    _dbtransaction.Commit();
                    _dbtransaction.Dispose();
                    _dbtransaction = null;
                    break;

            }

        }

        private void OpenSqlConnection()
        {
            string remote_connector_name = this.ReadString16();

            _connector = _callbacks.LookupAdoConnector(remote_connector_name);

            if (_connector == null)
                throw new InvalidOperationException("LookupAdoConnector returned null. Not allowed.");

            _dbconnection = _connector.OpenConnection();

            //_dbconnection.Open();
        }

    } // end of class
} // end of namespace

