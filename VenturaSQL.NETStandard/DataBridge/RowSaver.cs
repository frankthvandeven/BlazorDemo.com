using System.Text;
using System;
using System.Data.Common;

namespace VenturaSQL
{
    /// <summary>
    /// Assembles Sql INSERT/UPDATE/DELETE statements and executes them.
    /// </summary>
    public class RowSaver
    {
        private AdoConnector _connector;
        private DbConnection _connection;
        private DbTransaction _transaction;
        private VenturaSqlSchema _schema;
        private string _updateableTablename;

        private StringBuilder _statement;

        private bool _has_identity_value;
        private object _identity_value;

        private char _parameter_prefix;

        private string _quotePrefix;
        private string _quoteSuffix;


        public RowSaver(AdoConnector connector, DbConnection sqlconnection, DbTransaction transaction, VenturaSqlSchema schema, string updateableTablename)
        {
            _connector = connector;
            _connection = sqlconnection;
            _transaction = transaction;
            _schema = schema;

            _parameter_prefix = _connector.ParameterPrefix;
            _quotePrefix = _connector.QuotePrefix;
            _quoteSuffix = _connector.QuoteSuffix;

            //_connector.QuotePrefix

            _updateableTablename = updateableTablename;

            _statement = new StringBuilder(2048);
        }

        public void Execute(TrackArray trackarray)
        {
            _has_identity_value = false;
            _identity_value = null;

            if (trackarray.Status == TrackArrayStatus.Empty)
                return;

            _statement.Length = 0;

            DbCommand command = _connector.CreateCommand();
            command.Connection = _connection;
            command.Transaction = _transaction;

            if (trackarray.Status == TrackArrayStatus.DataForUPDATE)
                GenerateUPDATE(command, trackarray);
            else if (trackarray.Status == TrackArrayStatus.DataForINSERT)
                GenerateINSERT(command, trackarray);
            else if (trackarray.Status == TrackArrayStatus.DataForDELETE)
                GenerateDELETE(command, trackarray);

        }

        private void GenerateUPDATE(DbCommand command, TrackArray trackarray)
        {
            _statement.Append("UPDATE " + _updateableTablename + " SET ");

            for (short x = 0; x < trackarray.DataValueCount; x++)
            {
                short ordinal = trackarray.DataValueOrdinals[x];

                if (x > 0)
                    _statement.Append(",");

                _statement.Append(_quotePrefix);
                _statement.Append(_schema[ordinal].BaseColumnName);
                _statement.Append(_quoteSuffix);
                _statement.Append($"={_parameter_prefix}L{x}");

                object value = trackarray.DataValues[x];

                if (value == null)
                    value = DBNull.Value;

                DbParameter parameter = _connector.CreateParameter($"{_parameter_prefix}L{x}", value);

                command.Parameters.Add(parameter);
            }

            _statement.Append(" WHERE ");

            for (short x = 0; x < trackarray.PrikeyCount; x++)
            {
                short ordinal = trackarray.PrikeyOrdinals[x];

                if (x > 0)
                    _statement.Append(" AND ");

                _statement.Append(_quotePrefix);
                _statement.Append(_schema[ordinal].BaseColumnName);
                _statement.Append(_quoteSuffix);
                _statement.Append($"={_parameter_prefix}R{x}");

                object value = trackarray.PrikeyValues[x];

                if (value == null)
                    value = DBNull.Value;

                DbParameter parameter = _connector.CreateParameter($"{_parameter_prefix}R{x}", value);

                command.Parameters.Add(parameter);
            }

            //System.Windows.Forms.MessageBox.Show(statement.ToString());
            command.CommandText = _statement.ToString();
            command.ExecuteNonQuery();

        } // end of method

        private void GenerateINSERT(DbCommand command, TrackArray trackarray)
        {
            _statement.Append("INSERT INTO ");
            _statement.Append(_updateableTablename);
            _statement.Append(" (");

            for (short x = 0; x < trackarray.DataValueCount; x++)
            {
                short ordinal = trackarray.DataValueOrdinals[x];

                if (x > 0)
                    _statement.Append(",");

                _statement.Append(_quotePrefix);
                _statement.Append(_schema[ordinal].BaseColumnName);
                _statement.Append(_quoteSuffix);
            }

            _statement.Append(") VALUES (");

            for (short x = 0; x < trackarray.DataValueCount; x++)
            {
                if (x > 0)
                    _statement.Append(",");

                _statement.Append($"{_parameter_prefix}L{x}");

                object value = trackarray.DataValues[x];

                if (value == null)
                    value = DBNull.Value;

                DbParameter parameter = _connector.CreateParameter($"{_parameter_prefix}L{x}", value);

                command.Parameters.Add(parameter);
            }

            _statement.Append(")");


            if (_connector.ProviderCode == ProviderCodes.Unspecified || _schema.IdentityColumn == null) // No identity column.
            {
                command.CommandText = _statement.ToString();
                command.ExecuteNonQuery();
            }
            else if (_connector.ProviderCode == ProviderCodes.SqlServer)
            {
                // Currently only Sql Server is supported for sending identity column values back to the server.

                _statement.Append(" SELECT [");
                _statement.Append(_schema.IdentityColumn.BaseColumnName);
                _statement.Append("] FROM ");
                _statement.Append(_updateableTablename);
                _statement.Append(" WHERE @@ROWCOUNT > 0 AND [");
                _statement.Append(_schema.IdentityColumn.BaseColumnName);
                _statement.Append("] = scope_identity()");

                command.CommandText = _statement.ToString();

                _identity_value = command.ExecuteScalar();

                if (_identity_value == DBNull.Value)
                    _identity_value = null;

                _has_identity_value = true;
            }
            else if (_connector.ProviderCode == ProviderCodes.Npgsql)
            {
                _statement.Append($" RETURNING \"{_schema.IdentityColumn.BaseColumnName}\"");

                command.CommandText = _statement.ToString();

                _identity_value = command.ExecuteScalar();

                if (_identity_value == DBNull.Value)
                    _identity_value = null;

                _has_identity_value = true;
            }
            else
            {
                throw new InvalidOperationException("Don't know how to INSERT. Should never happen");
            }

            //System.Windows.Forms.MessageBox.Show(statement.ToString());

        } // end of method

        public bool HasIdentityValue
        {
            get { return _has_identity_value; }
        }

        public object IdentityValue
        {
            get { return _identity_value; }
        }

        private void GenerateDELETE(DbCommand command, TrackArray trackarray)
        {
            _statement.Append("DELETE FROM " + _updateableTablename + " WHERE ");

            for (short x = 0; x < trackarray.PrikeyCount; x++)
            {
                short ordinal = trackarray.PrikeyOrdinals[x];

                if (x > 0)
                    _statement.Append(" AND ");

                _statement.Append(_quotePrefix);
                _statement.Append(_schema[ordinal].BaseColumnName);
                _statement.Append(_quoteSuffix);
                _statement.Append($"={_parameter_prefix}R{x}");

                object value = trackarray.PrikeyValues[x];

                if (value == null)
                    value = DBNull.Value;

                DbParameter parameter = _connector.CreateParameter($"{_parameter_prefix}R{x}", value);

                command.Parameters.Add(parameter);
            }

            //System.Windows.Forms.MessageBox.Show(statement.ToString());

            command.CommandText = _statement.ToString();
            command.ExecuteNonQuery();

        } // end of method

    } // end of class
} // end of namespace






/*
About re-using SqlCommand:
    
There's very little benefit to reusing the command instance, unless you're planning to call Prepare.
If you're going to run the command many times (dozens or more), then you probably want to create the command,
prepare it, execute it in a loop, and then dispose it. The performance gains are significant if you're running
the command many times. (You would add the parameters once, though, before you prepare -- not delete and re-add
them every time like you're doing in your first code sample. You should change the parameters' values each time,
not create new parameters.)
If you're only going to be running the command a handful of times, performance isn't an issue, and you should go
with whichever style you prefer.Creating the command each time has the benefit that it's easy to extract into a
method so you don't repeat yourself.
*/
