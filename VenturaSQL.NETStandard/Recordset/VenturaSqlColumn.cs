using System;
using System.Data;
using System.Data.Common;

namespace VenturaSQL
{
    public partial class VenturaSqlColumn
    {
        // basic data (mandatory)
        private string _column_name; // The name of the column; this might not be unique. If this cannot be determined, a null value is returned. This name always reflects the most recent renaming of the column in the current view or command text. 
        private Type _column_type;
        private bool _isnullable;

        // the rest is optional.....

        // SQL related
        private DbType? _dbtype = null;
        private int? _column_size = null;
        private byte? _numericprecision = null; //If DbType is a numeric data type, this is the maximum precision of the column. The precision depends on the definition of the column. If ProviderType is not a numeric data type, this is a null value. 
        private byte? _numericscale = null;
        private int _providertype = 0;

        // SQL related - bool section
        private bool _isunique;
        private bool _iskey; //true: The column is one of a set of columns in the rowset that, taken together, uniquely identify the row. The set of columns with IsKey set to true must uniquely identify a row in the rowset. There is no requirement that this set of columns is a minimal set of columns. This set of columns may be generated from a base table primary key, a unique constraint or a unique index. false: The column is not required to uniquely identify the row. 
        private bool _isaliased; //true if the column name is an alias; otherwise false. 
        private bool _isexpression; //true if the column is an expression; otherwise false. 
        private bool _isidentity; //true if the column is an identity column; otherwise false. 
        private bool _isautoincrement; //true: The column assigns values to new rows in fixed increments. false: The column does not assign values to new rows in fixed increments. The default of this column is false. 
        private bool _islong; //Set if the column contains a Binary Long Object (BLOB) that contains very long data. The definition of very long data is provider-specific. 
        private bool _isreadonly;
        private bool _isrowguid; //Set if the column contains a persistent row identifier that cannot be written to, and has no meaningful value except to identity the row. 

        private string _xmlschemacollectiondatabase = "";
        private string _xmlschemacollectionowningschema = "";
        private string _xmlschemacollectionname = "";
        private string _udtassemblyqualifiedname = "";

        private string _baseservername = "";
        private string _basecatalogname = "";
        private string _baseschemaname = "";
        private string _basetablename = "";
        private string _basecolumnname = "";

        private string _fulltypename;

        private string _description = null;

        private bool _updateable = false; // for VenturaSql internal use. 

        public short ColumnOrdinal = -1;

        // Begin: VenturaSqlColumns are also used to store a list of Sql Parameters. This is for Sql Parameters only
        private bool _input;
        private bool _output;
        // End: This is for Sql Parameters only

        public VenturaSqlColumn(string column_name, Type column_type, bool null_able)
        {
            if (column_type == null)
                throw new ArgumentNullException("column_type");

            if (TypeTools.IsGenericTypeNullable(column_type))
                throw new ArgumentException($"Setting '{column_name}' to column_type 'generic Nullable<>' is not allowed.");
            
            _column_name = column_name;
            _column_type = column_type;
            _isnullable = null_able;

            _fulltypename = TypeTools.FullTypename(column_type);
        }

        public VenturaSqlColumn(string column_name, string fully_qualified_typename, bool null_able)
        {
            if (fully_qualified_typename == null)
                throw new ArgumentNullException("fully_qualified_typename");

            if (fully_qualified_typename.Length == 0)
                throw new ArgumentException("Empty string not allowed for parameter 'fully_qualified_typename'.");

            // Always returns a Type or else an Exception is thrown.
            // The TypeTools.GetType() can handle Nullable<> whereas Type.GetType() can not.
            Type column_type = TypeTools.GetType(fully_qualified_typename);

            if (TypeTools.IsGenericTypeNullable(column_type))
                throw new ArgumentException($"Setting '{column_name}' to column_type 'generic Nullable<>' is not allowed.");

            _column_name = column_name;
            _column_type = column_type;
            _isnullable = null_able;

            _fulltypename = fully_qualified_typename; TypeTools.FullTypename(column_type);
        }

        public string ColumnName
        {
            get { return _column_name; }
        }

        public Type ColumnType
        {
            get { return _column_type; }
        }

        public bool IsNullable
        {
            get { return _isnullable; }
        }

        /// <summary>
        /// For example 'System.DateTime' or 'System.String'
        /// </summary>
        public string FullTypename
        {
            get { return _fulltypename; }
        }

        /// <summary>
        /// Only used for setting Output parameters.
        /// </summary>
        public DbType? DbType
        {
            get { return _dbtype; }
            set { _dbtype = value; }
        }

        /// <summary>
        /// Only used for setting Output parameters.
        /// </summary>
        public int? ColumnSize
        {
            get { return _column_size; }
            set { _column_size = value; }
        }

        /// <summary>
        /// Only used for setting Output parameters.
        /// </summary>
        public byte? NumericPrecision
        {
            get { return _numericprecision; }
            set { _numericprecision = value; }
        }

        /// <summary>
        /// Only used for setting Output parameters.
        /// </summary>
        public byte? NumericScale
        {
            get { return _numericscale; }
            set { _numericscale = value; }
        }

        public bool IsUnique
        {
            get { return _isunique; }
            set { _isunique = value; }
        }

        public bool IsKey
        {
            get { return _iskey; }
            set { _iskey = value; }
        }

        public bool IsAliased
        {
            get { return _isaliased; }
            set { _isaliased = value; }
        }

        public bool IsExpression
        {
            get { return _isexpression; }
            set { _isexpression = value; }
        }

        public bool IsIdentity
        {
            get { return _isidentity; }
            set { _isidentity = value; }
        }

        public bool IsAutoIncrement
        {
            get { return _isautoincrement; }
            set { _isautoincrement = value; }
        }

        public bool IsLong
        {
            get { return _islong; }
            set { _islong = value; }
        }

        public bool IsReadOnly
        {
            get { return _isreadonly; }
            set { _isreadonly = value; }
        }

        public bool IsRowGuid
        {
            get { return _isrowguid; }
            set { _isrowguid = value; }
        }

        public string BaseServerName
        {
            get { return _baseservername; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _baseservername = value;
            }
        }

        public string BaseCatalogName
        {
            get { return _basecatalogname; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _basecatalogname = value;
            }
        }
        public string BaseSchemaName
        {
            get { return _baseschemaname; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _baseschemaname = value;
            }
        }
        public string BaseTableName
        {
            get { return _basetablename; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _basetablename = value;
            }
        }

        public string BaseColumnName
        {
            get { return _basecolumnname; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _basecolumnname = value;
            }
        }

        /// <summary>
        /// Introduced for CData Software's providers. Null when not defined.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// Returns true if the column belongs to the updateable table
        /// and is not readonly.
        /// This property does not originate from the database.
        /// This column is part of the UPDATE statement.
        /// </summary>
        public bool Updateable
        {
            get { return _updateable; }
            set { _updateable = value; }
        }

        /// <summary>
        /// This is used for Sql parameters only. Not for Sql schema information.
        /// </summary>
        public bool Input
        {
            get { return _input; }
            set { _input = value; }
        }

        /// <summary>
        /// This is used for Sql parameters only. Not for Sql schema information.
        /// </summary>
        public bool Output
        {
            get { return _output; }
            set { _output = value; }
        }

        /// <summary>
        /// Creates a DbParameter without value, but with Input/Output already set.
        /// </summary>
        public DbParameter CreateSqlParameter(AdoConnector connector)
        {
            DbParameter sql_parameter = connector.CreateParameter(_column_name);

            if (_input == true && _output == false)
                sql_parameter.Direction = ParameterDirection.Input;
            else if (_input == false && _output == true)
                sql_parameter.Direction = ParameterDirection.Output;
            else if (_input == true && _output == true)
                sql_parameter.Direction = ParameterDirection.InputOutput;
            else
                throw new VenturaSqlException($"Sql Parameter {_column_name} error. Both Input and Output are false. Forgot a validation?");

            if (_dbtype != null)
                sql_parameter.DbType = _dbtype.Value;

            if (_column_size != null)
                sql_parameter.Size = _column_size.Value;

            if (_numericprecision != null)
                sql_parameter.Precision = _numericprecision.Value;

            if (_numericscale != null)
                sql_parameter.Scale = _numericscale.Value;

            return sql_parameter;
        }

        public int ProviderType
        {
            get { return _providertype; }
            set { _providertype = value; } // end of set
        } // end of property


        public string UdtAssemblyQualifiedName
        {
            get { return _udtassemblyqualifiedname; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _udtassemblyqualifiedname = value;
            }
        }

        public string ExtractTypeFromUdtAssemblyQualifiedName()
        {
            int x = _udtassemblyqualifiedname.IndexOf(',');

            if (x == -1)
                throw new InvalidOperationException("There is no comma in the qualified name.");

            return _udtassemblyqualifiedname.Substring(0, x);
        }

        public string XmlSchemaCollectionDatabase
        {
            get { return _xmlschemacollectiondatabase; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _xmlschemacollectiondatabase = value;
            }
        }

        public string XmlSchemaCollectionOwningSchema
        {
            get { return _xmlschemacollectionowningschema; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _xmlschemacollectionowningschema = value;
            }
        }

        public string XmlSchemaCollectionName
        {
            get { return _xmlschemacollectionname; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _xmlschemacollectionname = value;
            }
        }

        public VenturaSqlColumn Clone()
        {
            VenturaSqlColumn newcolumn = new VenturaSqlColumn(this.ColumnName, this.ColumnType, this.IsNullable);

            // sql related (needed for databridge)
            newcolumn.DbType = this.DbType;
            newcolumn.ColumnSize = this.ColumnSize;
            newcolumn.NumericPrecision = this.NumericPrecision;
            newcolumn.NumericScale = this.NumericScale;
            newcolumn.ProviderType = this.ProviderType;
            newcolumn.IsUnique = this.IsUnique;
            newcolumn.IsKey = this.IsKey;
            newcolumn.IsAliased = this.IsAliased;
            newcolumn.IsExpression = this.IsExpression;
            newcolumn.IsIdentity = this.IsIdentity;
            newcolumn.IsAutoIncrement = this.IsAutoIncrement;
            newcolumn.IsLong = this.IsLong;
            newcolumn.IsReadOnly = this.IsReadOnly;
            newcolumn.IsRowGuid = this.IsRowGuid;

            newcolumn.XmlSchemaCollectionDatabase = this.XmlSchemaCollectionDatabase;
            newcolumn.XmlSchemaCollectionOwningSchema = this.XmlSchemaCollectionOwningSchema;
            newcolumn.XmlSchemaCollectionName = this.XmlSchemaCollectionName;
            newcolumn.UdtAssemblyQualifiedName = this.UdtAssemblyQualifiedName;

            newcolumn.BaseServerName = this.BaseServerName;
            newcolumn.BaseCatalogName = this.BaseCatalogName;
            newcolumn.BaseSchemaName = this.BaseSchemaName;
            newcolumn.BaseTableName = this.BaseTableName;
            newcolumn.BaseColumnName = this.BaseColumnName;

            newcolumn.Description = this.Description;
            newcolumn.Updateable = this.Updateable;

            return newcolumn;

        } // end of Clone method

        public override string ToString()
        {
            if (_column_name == "")
                return base.ToString();

            return _column_name;
        }

    }
}

