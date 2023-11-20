using System;
using System.Data.Common;

namespace VenturaSQL
{
    /// <summary>
    /// Ado to Framestream.
    /// </summary>
    public class RowLoaderFramestream : RowLoaderBase
    {
        private IRecordsetBase _loader;
        private IRecordsetIncremental _incr_loader;
        private IResultsetBase _currentresultset;
        private VenturaSqlSchema _currentschema;

        private int _rowcount;
        private FrameWriter _framewriter;

        public RowLoaderFramestream()
        {
        }

        public void Exec(AdoConnector connector, DbConnection dbconnection, DbTransaction transaction, IRecordsetBase loader, FrameWriter framewriter)
        {
            _loader = loader;
            _incr_loader = loader as IRecordsetIncremental;

            _framewriter = framewriter;

            base.ExecuteQuery(connector, dbconnection, transaction, loader);
        }

        private long _recordcount_position;

        public override void ExecuteStarted()
        {
        }

        public override void OnSelectResultset(int index)
        {
            _currentresultset = _loader.Resultsets[index];
            _currentschema = _currentresultset.Schema;
            _rowcount = 0;

            _framewriter.StartFrame(FrameType.SelectResultset);
            _framewriter.Write(index);
            _framewriter.EndFrame();

            _framewriter.StartFrame(FrameType.IncreaseResultsetCapacity);

            // remember position in stream, so we can update this number later
            _recordcount_position = _framewriter.Position;

            _framewriter.Write((int)0); /* reserved for number of records in resultset */

            _framewriter.EndFrame();
        }

        public override void ProcessSingleRow(object[] columnvalues)
        {
            _currentschema.ObjectArray2Frame(FrameType.Record_Unchanged, _framewriter, columnvalues);
            _rowcount++;
        }

        public override void OnUnselectResultset(int index)
        {
            // Update the recordcount earlier in the framestream.
            long position = _framewriter.Position;
            _framewriter.Position = _recordcount_position;
            _framewriter.Write((int)_rowcount);

            _framewriter.Position = position;

            if(_incr_loader != null)
            {
                _framewriter.StartFrame(FrameType.SetRowOffset);
                _framewriter.Write(_incr_loader.IncrementalOffset + _rowcount);
                _framewriter.EndFrame();
            }

            _framewriter.StartFrame(FrameType.UnselectResultset);
            _framewriter.EndFrame();
        }

        public override void ProcessOutputParameters()
        {
            _loader.ParameterSchema.ObjectArray2Frame(FrameType.SetOutputParameters, _framewriter, _loader.OutputParameterValues);

        }

        public override void ExecuteFinished()
        {
        }

    }
}
