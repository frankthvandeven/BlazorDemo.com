using System;
using System.Data.Common;

namespace VenturaSQL
{
    /// <summary>
    /// Ado to Recordset.
    /// </summary>
    internal class RowLoaderRecordset : RowLoaderBase
    {

        public RowLoaderRecordset()
        {
        }

        private IRecordsetBase _loader;
        private IRecordsetIncremental _incr_loader;
        private IResultsetBase _currentresultset;

        private int _rowcount;

        public void Exec(AdoConnector connector, DbConnection dbconnection, DbTransaction transaction, IRecordsetBase loader)
        {
            _loader = loader;
            _incr_loader = loader as IRecordsetIncremental;

            base.ExecuteQuery(connector, dbconnection, transaction, loader);
        }

        public override void ExecuteStarted()
        {
        }

        public override void OnSelectResultset(int index)
        {
            _currentresultset = _loader.Resultsets[index];

            _rowcount = 0;
        }

        public override void ProcessSingleRow(object[] columnvalues)
        {
            _currentresultset.OptimizedCreateAndAppendExistingRecord(columnvalues);
            _rowcount++;
        }

        public override void OnUnselectResultset(int index)
        {

            if (_incr_loader != null)
            {
                _incr_loader.IncrementalOffset = _incr_loader.IncrementalOffset + _rowcount;
                _incr_loader.LastExecCount = _rowcount;
                _incr_loader.LastExecStartIndex = _rowcount == 0 ? -1 : (_currentresultset.Length - _rowcount);
                _incr_loader.HasMoreRows = (_rowcount >= _loader.RowLimit) ? true : false;
            }

        }

        public override void ProcessOutputParameters()
        {
        }

        public override void ExecuteFinished()
        {


        }

    }

}