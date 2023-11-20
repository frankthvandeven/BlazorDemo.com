using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace VenturaSQL
{
    /// <summary>
    /// Execute Sql statement and read rows from ADO.NET.
    /// This is a base class.
    /// </summary>
    public abstract class RowLoaderBase
    {
        private IResultsetBase _currentresultset;

        private int _currentresultsetindex;

        /// <summary>
        /// Class executes an Sql statement with parameters, and calls ProcessRow for each row to process.
        /// </summary>
        protected void ExecuteQuery(AdoConnector connector, DbConnection dbconnection, DbTransaction transaction, IRecordsetBase loader)
        {
            var inc_loader = loader as IRecordsetIncremental;

            ExecuteStarted();

            DbCommand dbcommand = connector.CreateCommand(loader.SqlScript, dbconnection, transaction);

            DbParameter system_parameter = connector.CreateParameter(connector.ParameterPrefix + "DesignMode", false);
            dbcommand.Parameters.Add(system_parameter);

            int param_row_offset = 0;

            if (inc_loader != null)
                param_row_offset = inc_loader.IncrementalOffset;

            system_parameter = connector.CreateParameter(connector.ParameterPrefix + "RowOffset", param_row_offset);
            dbcommand.Parameters.Add(system_parameter);

            system_parameter = connector.CreateParameter(connector.ParameterPrefix + "RowLimit", loader.RowLimit);
            dbcommand.Parameters.Add(system_parameter);

            if (loader.ParameterSchema != null)
            {
                VenturaSqlSchema parameterschema = loader.ParameterSchema;
                Object[] parametervalues = loader.InputParameterValues;

                // Set the Sql parameters.
                for (int x = 0; x < parameterschema.Count; x++)
                {
                    VenturaSqlColumn parameter = parameterschema[x];

                    DbParameter db_parameter = parameter.CreateSqlParameter(connector);

                    if (parameter.Input) /* We must set an initial value! */
                    {
                        if (parametervalues[x] == null)
                            db_parameter.Value = DBNull.Value;
                        else
                            db_parameter.Value = parametervalues[x];
                    }

                    dbcommand.Parameters.Add(db_parameter);
                }
            }

            using (DbDataReader dbdatareader = dbcommand.ExecuteReader())
            {
                if (loader.Resultsets.Length == 0)
                    goto CloseReader; // Skip all the reading as there are not going to be any resultsets.          //int vdis_fc = sqldatareader.VisibleFieldCount;

                _currentresultsetindex = 0;
                _currentresultset = loader.Resultsets[0];

                this.OnSelectResultset(0);

                while (true) // Resultset loop
                {
                    if (loader.RowLimit > 0)
                    {
                        int columncount = _currentresultset.Schema.Count;

                        int[] trimarray = CreateTrimArray(_currentresultset.Schema);

                        // Study this: http://stackoverflow.com/questions/19895047/is-there-any-performance-gain-from-commandbehavior-sequentialaccess

                        int row_count = 0;

                        while (dbdatareader.Read())
                        {
                            /* We need a fresh array initialised */
                            object[] columnvalues = new object[columncount];

                            /* This is the ONLY place in all VenturaSQL code where SqlDataReader.GetValues() is called */
                            dbdatareader.GetValues(columnvalues);                 //Alternative: ((IDataRecord)sqldatareader)[column_index]

                            /* Replace DBNull with a null */
                            for (int x = 0; x < columnvalues.Length; x++)
                                if (columnvalues[x] == DBNull.Value)
                                    columnvalues[x] = null;

                            /* Begin: Trim the trailing spaces of char and nchar type columns */
                            for (int y = 0; y < trimarray.Length; y++)
                            {
                                int ordinal = trimarray[y];
                                if (columnvalues[ordinal] != null)
                                {
                                    string t_string = (string)columnvalues[ordinal];
                                    int t_length = t_string.Length;
                                    if (t_length > 0 && t_string[t_length - 1] == ' ')
                                        columnvalues[ordinal] = t_string.TrimEnd(null);
                                }
                            }
                            /* End: Trim the trailing spaces of char and nchar type columns */

                            ProcessSingleRow(columnvalues);

                            row_count++;

                            if (row_count == loader.RowLimit)
                                break;

                        } // end row loop
                    }

                    OnUnselectResultset(_currentresultsetindex);

                    if (dbdatareader.NextResult() == false)
                        break;

                    _currentresultsetindex++;
                    _currentresultset = loader.Resultsets[_currentresultsetindex];

                    this.OnSelectResultset(_currentresultsetindex);
                }

            CloseReader:

                // Call Close when done reading.
                dbdatareader.Close();
            } // end of using

            /* begin: process output parameters */
            if (loader.ParameterSchema != null && loader.OutputParameterValues != null)
            {
                VenturaSqlSchema parameterschema = loader.ParameterSchema;

                for (int x = 0; x < parameterschema.Count; x++)
                {
                    if (parameterschema[x].Output == true)
                    {
                        object parameter_value = dbcommand.Parameters[x].Value;

                        if (parameter_value == DBNull.Value) /* translate DBNull back to null */
                            parameter_value = null;

                        loader.OutputParameterValues[x] = parameter_value;
                    }
                    //else
                    //    loader.OutputParameterValues[x] = null;
                }

                ProcessOutputParameters();
            }
            /* end: process output parameters */

            ExecuteFinished();

        } // end of method

        private int[] CreateTrimArray(VenturaSqlSchema schema)
        {
            List<int> columns2trim = new List<int>();

            /* find the columns that need to be R-Trimmed (char and nchar) */
            for (int x = 0; x < schema.Count; x++)
            {
                VenturaSqlColumn column = schema[x];
                if (column.ColumnType == typeof(string))
                    columns2trim.Add(column.ColumnOrdinal);
            }

            return columns2trim.ToArray();
        }

        public abstract void ExecuteStarted();

        public abstract void OnSelectResultset(int index);

        public abstract void ProcessSingleRow(object[] columnvalues);

        public abstract void OnUnselectResultset(int index);

        public abstract void ProcessOutputParameters();

        public abstract void ExecuteFinished();

    } // end of class
} // end of namespace


