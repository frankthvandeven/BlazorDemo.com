using System;
using System.Data.Common;
using System.Reflection;

// Info on how to detect when parameter is specified as '?' only.
// https://www.codeproject.com/Articles/52076/Using-Information-from-the-NET-DataProvider

namespace VenturaSQL
{
    public enum ProviderCodes
    {
        Unspecified = 0,
        SqlServer = 1,
        Npgsql = 2
    }

    public class AdoConnector : Connector
    {
        private string _connection_string; // max 512 characters
        private DbProviderFactory _factory;
        private char _parameter_prefix;
        private string _provider_invariant_name;
        private ProviderCodes _provider_code = ProviderCodes.Unspecified;

        private string _quote_prefix;
        private string _quote_suffix;

        //private static readonly Func<DbCommandBuilder, int, string> _getParameterName;
        private static readonly Func<DbCommandBuilder, int, string> _getParameterPlaceholder;

        static AdoConnector()
        {
            //_getParameterName = (Func<DbCommandBuilder, int, string>)Delegate.CreateDelegate(typeof(Func<DbCommandBuilder, int, string>), typeof(DbCommandBuilder).GetMethod("GetParameterName", BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, new Type[] { typeof(Int32) }, null));
            _getParameterPlaceholder = (Func<DbCommandBuilder, int, string>)Delegate.CreateDelegate(typeof(Func<DbCommandBuilder, int, string>), typeof(DbCommandBuilder).GetMethod("GetParameterPlaceholder", BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, new Type[] { typeof(Int32) }, null));
        }

        public AdoConnector(DbProviderFactory factory, string connection_string = null)
        {
            _provider_invariant_name = factory.GetType().Namespace;
            _factory = factory;
            _connection_string = connection_string;

            if (_provider_invariant_name == "System.Data.SqlClient")
                _provider_code = ProviderCodes.SqlServer;
            else if (_provider_invariant_name == "Npgsql") // PostgreSQL
                _provider_code = ProviderCodes.Npgsql;

            set_prefix_suffix();
        }

        /// <summary>
        /// It is a bit tricky, but assume we find the the parameter prefix like '@' this way.
        /// </summary>
        private void set_prefix_suffix()
        {
            // The factory for Microsoft.Data.Sqlite does not have a CreateCommandBuilder method.
            if (_provider_invariant_name == "Microsoft.Data.Sqlite")
            {
                _parameter_prefix = '@';
                _quote_prefix = "[";
                _quote_suffix = "]";
                return;
            }

            DbCommandBuilder builder = _factory.CreateCommandBuilder();

            if (builder == null)
                throw new InvalidOperationException("The provider factory is lacking a CreateCommandBuilder method. Contact support.");

            string name = _getParameterPlaceholder(builder, 0);

            if (name.Length == 0)
                throw new InvalidOperationException("_getParameterPlaceholder() returns an empty string. Cannot find the ADO.NET Provider parameter prefix. Contact support.");

            _parameter_prefix = name[0];
            _quote_prefix = builder.QuotePrefix;
            _quote_suffix = builder.QuoteSuffix;
        }

        public string ConnectionString
        {
            get { return _connection_string; }
        }

        // For example "@" for SQL Server.
        public char ParameterPrefix
        {
            get { return _parameter_prefix; }
        }

        /// <summary>
        /// For SQL Server this is the [ character.
        /// </summary>
        public string QuotePrefix
        {
            get { return _quote_prefix; }
        }

        /// <summary>
        /// For SQL Server this is the ] character.
        /// </summary>
        public string QuoteSuffix
        {
            get { return _quote_suffix; }
        }

        public DbConnection OpenConnection()
        {
            if (_connection_string == null)
                throw new InvalidOperationException("Cannot open a connection as the connection string was set to null.");

            DbConnection connection = _factory.CreateConnection();
            connection.ConnectionString = _connection_string;

            connection.Open();

            return connection;
        }

        public DbCommand CreateCommand()
        {
            DbCommand command = _factory.CreateCommand();
            return command;
        }

        public DbCommand CreateCommand(string cmdText)
        {
            DbCommand command = _factory.CreateCommand();
            command.CommandText = cmdText;
            return command;
        }

        public DbCommand CreateCommand(string cmdText, DbConnection connection)
        {
            DbCommand command = _factory.CreateCommand();
            command.CommandText = cmdText;
            command.Connection = connection;
            return command;
        }

        public DbCommand CreateCommand(string cmdText, DbConnection connection, DbTransaction transaction)
        {
            DbCommand command = _factory.CreateCommand();
            command.CommandText = cmdText;
            command.Connection = connection;
            command.Transaction = transaction;
            return command;
        }

        public DbParameter CreateParameter()
        {
            DbParameter parameter = _factory.CreateParameter();
            return parameter;
        }

        public DbParameter CreateParameter(string parameterName)
        {
            DbParameter parameter = _factory.CreateParameter();
            parameter.ParameterName = parameterName;
            return parameter;
        }

        public DbParameter CreateParameter(string parameterName, object value)
        {
            DbParameter parameter = _factory.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            return parameter;
        }

        public DbCommandBuilder CreateCommandBuilder()
        {
            DbCommandBuilder builder = _factory.CreateCommandBuilder();
            return builder;
        }

        public string ProviderInvariantName
        {
            get { return _provider_invariant_name; }
        }

        public ProviderCodes ProviderCode
        {
            get { return _provider_code; }
        }

#region Static methods

        /// <summary>
        /// Returns the data in a column as a byte array.
        /// Optionally you can limit the number of bytes to read.
        /// </summary>
        /// <param name="maxbytes">-1  means no limit.</param>
        public static byte[] GetBytes(DbDataReader reader, int ordinal, int maxbytes = -1)
        {
            const int BUFFER_SIZE = 8192; // was 1024

            if (reader.IsDBNull(ordinal) == true)
                return null;

            // Get the actual  length of data. 
            int size = (int)reader.GetBytes(ordinal, 0, null, 0, 0);

            if (maxbytes > 0)
            {
                // Limit the size if requested
                size = Math.Min(size, maxbytes);
            }

            byte[] result = new byte[size];

            int start_pos = 0;

            while (start_pos < size)
            {
                int block_length = Math.Min(BUFFER_SIZE, (size - start_pos));
                reader.GetBytes(ordinal, start_pos, result, start_pos, block_length);
                start_pos += block_length;
            }

            return result;
        }

#endregion
    }
}






//public static bool TableExists(SqlConnection sqlconnection, string tablename)
//{
//          string sql = "SELECT COUNT(*) FROM information_Schema.tables WHERE (table_name = '" + tablename + "')";
//	        SqlCommand command = new SqlCommand(sql,sqlconnection);
//          int count = (int)command.ExecuteScalar();
//          return (count > 0);
//}