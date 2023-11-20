using System;
using System.Threading.Tasks;
using System.Data.Common;
using System.Configuration;

namespace VenturaSQL
{
    public static partial class Transactional
    {

        private static async Task ExecSql_AdoAsync(AdoConnector connector, params IRecordsetBase[] loaders)
        {
            Action action = () =>
            {
                using (DbConnection connection = connector.OpenConnection())
                {
                    using (DbTransaction transaction = connection.BeginTransaction())
                    {
                        foreach (IRecordsetBase loader in loaders)
                            new RowLoaderRecordset().Exec(connector, connection, transaction, loader);

                        transaction.Commit();
                    }

                    connection.Close();
                }
            };

            await Task.Run(action);
        }

        private static void ExecSql_Ado(AdoConnector connector, params IRecordsetBase[] loaders)
        {
            using (DbConnection connection = connector.OpenConnection())
            {
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    foreach (IRecordsetBase loader in loaders)
                        new RowLoaderRecordset().Exec(connector, connection, transaction, loader);

                    transaction.Commit();
                }

                connection.Close();
            }
        }

        private static async Task SaveChanges_AdoAsync(AdoConnector connector, params IRecordsetBase[] loaders)
        {

            Action action = () =>
            {
                using (DbConnection connection = connector.OpenConnection())
                {
                    using (DbTransaction transaction = connection.BeginTransaction())
                    {
                        foreach (IRecordsetBase loader in loaders)
                            SaveChanges_Ado_ProcessOneLoader(connector, connection, transaction, loader);

                        transaction.Commit();
                    }

                    connection.Close();
                }
            };

            await Task.Run(action);
        }

        private static void SaveChanges_Ado(AdoConnector connector, params IRecordsetBase[] loaders)
        {
            using (DbConnection connection = connector.OpenConnection())
            {
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    foreach (IRecordsetBase loader in loaders)
                        SaveChanges_Ado_ProcessOneLoader(connector, connection, transaction, loader);

                    transaction.Commit();
                }

                connection.Close();
            }
        }

        private static void SaveChanges_Ado_ProcessOneLoader(AdoConnector connector, DbConnection connection, DbTransaction transaction, IRecordsetBase loader)
        {

            for (int resultset_index = 0; resultset_index < loader.Resultsets.Length; resultset_index++)
            {
                IResultsetBase resultset = loader.Resultsets[resultset_index];

                TrackArray trackarray = new TrackArray(resultset.Schema);

                RowSaver rowsaver = new RowSaver(connector, connection, transaction, resultset.Schema, resultset.UpdateableTablename);

                for (int index = 0; index < resultset.Length; index++)
                {
                    trackarray.Reset(); // Reset the TrackArray. Sets the status to Empty.
                    resultset[index].WriteChangesToTrackArray(trackarray);

                    rowsaver.Execute(trackarray);

                    if (rowsaver.HasIdentityValue)
                        resultset[index].SetIdentityColumnValue(rowsaver.IdentityValue);
                }

            }

            // end of work

        }

    } // end of class
} // end of namespace


//Action action = () =>
//       {
//            throw new NotImplementedException("This Client does not support direct Ado.Net connections.");
//      };
