using System;
using System.IO;
using System.Threading.Tasks;

namespace VenturaSQL
{
    public static partial class Transactional
    {
        public static void ExecSqlIncremental(IRecordsetIncremental recordset)
        {
            if (recordset == null)
                throw new ArgumentNullException("recordset");

            var recordsetbase = recordset as IRecordsetBase;
            var resultsetbase = recordset as IResultsetBase;

            Connector connector = recordset.IncrementalConnector;

            if (connector == null)
                return;

            if (connector is HttpConnector)
                throw new VenturaSqlException("The synchronous execution of HttpConnectors is not supported. Use ExecSqlIncrementalAsync instead.");
            else if (connector is AdoConnector)
                ExecSql_Ado((AdoConnector)connector, new IRecordsetBase[] { recordsetbase });
            else
                throw new VenturaSqlException($"Unhandled Connector type {connector.GetType().FullName}.");

            resultsetbase.OnAfterExecSql();

        } // end of method

        public static async Task ExecSqlIncrementalAsync(IRecordsetIncremental recordset)
        {
            if (recordset == null)
                throw new ArgumentNullException("recordset");

            var recordsetbase = recordset as IRecordsetBase;
            var resultsetbase = recordset as IResultsetBase;

            Connector connector = recordset.IncrementalConnector;

            if (connector == null)
                throw new ArgumentNullException("connector");

            if (connector is HttpConnector)
                await ExecSql_HttpAsync((HttpConnector)connector, new IRecordsetBase[] { recordsetbase });
            else if (connector is AdoConnector)
                await ExecSql_AdoAsync((AdoConnector)connector, new IRecordsetBase[] { recordsetbase });
            else
                throw new VenturaSqlException($"Unhandled Connector type {connector.GetType().FullName}.");

            resultsetbase.OnAfterExecSql();

        } // end of method

    } // end of class
} // end of namespace
