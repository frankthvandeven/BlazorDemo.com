using System;
using System.IO;
using System.Threading.Tasks;

namespace VenturaSQL
{
    /// <summary>
    /// Load or Save data for multiple Recordsets in a single transaction.
    /// Single transaction means a single Sql Server transaction.
    /// A single request is send to the server.
    /// </summary>
    public static partial class Transactional
    {

        // Using async/await inside ASP.Net pages. Not what you expect:
        // http://www.hanselman.com/blog/TheMagicOfUsingAsynchronousMethodsInASPNET45PlusAnImportantGotcha.aspx

        public static void ExecSql(params IRecordsetBase[] loaders)
        {
            ExecSql(VenturaSqlConfig.DefaultConnector, loaders);
        }

        public static void ExecSql(Connector connector, params IRecordsetBase[] recordsets)
        {
            if (connector == null)
                throw new ArgumentNullException("connector");

            foreach (IRecordsetBase recordset in recordsets)
            {
                var i = recordset as IRecordsetIncremental;

                if (i != null)
                {
                    i.IncrementalConnector = connector;
                    i.IncrementalOffset = 0;
                }

                foreach (IResultsetBase resultset in recordset.Resultsets)
                    resultset.Clear();
            }

            if (connector is HttpConnector)
                throw new VenturaSqlException("The synchronous execution of HttpConnectors is not supported. Use ExecSqlAsync instead.");
            else if (connector is AdoConnector)
                ExecSql_Ado((AdoConnector)connector, recordsets);
            else
                throw new VenturaSqlException($"Unhandled Connector type {connector.GetType().FullName}.");

            foreach (IRecordsetBase recordset in recordsets)
                foreach (IResultsetBase resultset in recordset.Resultsets)
                    resultset.OnAfterExecSql();

        } // end of method

        public static async Task ExecSqlAsync(params IRecordsetBase[] recordsets)
        {
            await ExecSqlAsync(VenturaSqlConfig.DefaultConnector, recordsets);
        }

        public static async Task ExecSqlAsync(Connector connector, params IRecordsetBase[] recordsets)
        {
            if (connector == null)
                throw new ArgumentNullException("connector");

            foreach (IRecordsetBase recordset in recordsets)
            {
                var i = recordset as IRecordsetIncremental;

                if (i != null)
                {
                    i.IncrementalConnector = connector;
                    i.IncrementalOffset = 0;
                }

                foreach (IResultsetBase resultset in recordset.Resultsets)
                    resultset.Clear();
            }

            if (connector is HttpConnector)
                await ExecSql_HttpAsync((HttpConnector)connector, recordsets);
            else if (connector is AdoConnector)
                await ExecSql_AdoAsync((AdoConnector)connector, recordsets);
            else
                throw new VenturaSqlException($"Unhandled Connector type {connector.GetType().FullName}.");

            foreach (IRecordsetBase recordset in recordsets)
                foreach (IResultsetBase resultset in recordset.Resultsets)
                    resultset.OnAfterExecSql();

        } // end of method

        public static void SaveChanges(params IRecordsetBase[] recordsets)
        {
            SaveChanges(VenturaSqlConfig.DefaultConnector, recordsets);
        }

        public static void SaveChanges(Connector connector, params IRecordsetBase[] recordsets)
        {
            if (connector == null)
                throw new ArgumentNullException("connector");

            // Make sure every new Record has all primary keys filled out.
            foreach (IRecordsetBase recordset in recordsets)
                foreach (IResultsetBase resultset in recordset.Resultsets)
                {
                    for (int index = 0; index < resultset.Length; index++) // a for loop is faster
                        resultset[index].ValidateBeforeSaving(index);
                }

            if (connector is HttpConnector)
                throw new VenturaSqlException("The synchronous execution of HttpConnectors is not supported. Use SaveChangesAsync instead.");
            else if (connector is AdoConnector)
                SaveChanges_Ado((AdoConnector)connector, recordsets);
            else
                throw new VenturaSqlException($"Unhandled Connector type {connector.GetType().FullName}.");

            foreach (IRecordsetBase recordset in recordsets)
                foreach (IResultsetBase resultset in recordset.Resultsets)
                    resultset.OnAfterSaveChanges();

        } // end of method

        public static async Task SaveChangesAsync(params IRecordsetBase[] recordsets)
        {
            await SaveChangesAsync(VenturaSqlConfig.DefaultConnector, recordsets);
        }

        public static async Task SaveChangesAsync(Connector connector, params IRecordsetBase[] recordsets)
        {
            if (connector == null)
                throw new ArgumentNullException("connector");

            // Make sure every new Record has all primary keys filled out.
            foreach (IRecordsetBase recordset in recordsets)
                foreach (IResultsetBase resultset in recordset.Resultsets)
                {
                    for (int index = 0; index < resultset.Length; index++)
                        resultset[index].ValidateBeforeSaving(index);
                }

            if (connector is HttpConnector)
                await SaveChanges_HttpAsync((HttpConnector)connector, recordsets);
            else if (connector is AdoConnector)
                await SaveChanges_AdoAsync((AdoConnector)connector, recordsets);
            else
                throw new VenturaSqlException($"Unhandled Connector type {connector.GetType().FullName}.");

            foreach (IRecordsetBase recordset in recordsets)
                foreach (IResultsetBase resultset in recordset.Resultsets)
                    resultset.OnAfterSaveChanges();

        } // end of method

    } // end of class
} // end of namespace



//       public static int WordCount(this string str) => str.Split(new char[] { ' ', '.', '?' },
//                            StringSplitOptions.RemoveEmptyEntries).Length;
