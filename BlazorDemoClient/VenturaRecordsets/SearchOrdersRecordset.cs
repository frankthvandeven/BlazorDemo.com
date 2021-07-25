/*
	Project file: "C:\Active\Kenova\Projects\BikeStores.venproj"
	Target platform: NETStandard
	Generator version: 4.0.138
	Generated on: Tuesday, June 8, 2021 at 7:29:20 AM
	At the bottom of this file you find a template for extending Recordsets with calculated columns for XAML data binding.
*/
using System;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using VenturaSQL;

namespace BlazorDemo.Client.VenturaRecordsets
{
    /// <summary>
    /// The updateable table is [sales].[orders]. Updateable table column information:
    /// • 8 out of 8 table columns are present in the resultset.
    /// • All primary key columns are present in the resultset: order_id.
    /// • Non-primary key columns present in the resultset: customer_id, order_status, order_date, required_date, shipped_date,
    ///   store_id and staff_id.
    /// Recordset item automatically created by VenturaSQL Studio.
    /// </summary>
    public partial class SearchOrdersRecordset : ResultsetObservable<SearchOrdersRecordset, SearchOrdersRecord>, IRecordsetBase
    {
        private IResultsetBase[] _resultsets;
        private object[] _inputparametervalues;
        private InputParamHolder _inputparamholder;
        private VenturaSqlSchema _parameterschema;
        private int _rowlimit = 500;
        private const string CRLF = "\r\n";

        public SearchOrdersRecordset()
        {
            _resultsets = new IResultsetBase[] { this };


            _inputparametervalues = new object[2];
            _inputparamholder = new InputParamHolder(_inputparametervalues);

            ColumnArrayBuilder param_array = new ColumnArrayBuilder();

            param_array.AddParameterColumn("@SearchText", typeof(string), true, false, DbType.String, null, null, null);
            param_array.AddParameterColumn("@order_id", typeof(int), true, false, DbType.Int32, null, null, null);

            _parameterschema = new VenturaSqlSchema(param_array);

            ColumnArrayBuilder schema_array = new ColumnArrayBuilder();

            schema_array.Add(new VenturaSqlColumn("order_id", typeof(int), false)
            {
                ColumnSize = 4,
                NumericPrecision = 10,
                IsKey = true,
                IsIdentity = true,
                IsAutoIncrement = true
            });

            schema_array.Add(new VenturaSqlColumn("customer_id", typeof(int), true)
            {
                Updateable = true,
                ColumnSize = 4,
                NumericPrecision = 10
            });

            schema_array.Add(new VenturaSqlColumn("order_status", typeof(byte), false)
            {
                Updateable = true,
                ColumnSize = 1,
                NumericPrecision = 3
            });

            schema_array.Add(new VenturaSqlColumn("order_date", typeof(DateTime), false)
            {
                Updateable = true,
                ColumnSize = 3
            });

            schema_array.Add(new VenturaSqlColumn("required_date", typeof(DateTime), false)
            {
                Updateable = true,
                ColumnSize = 3
            });

            schema_array.Add(new VenturaSqlColumn("shipped_date", typeof(DateTime), true)
            {
                Updateable = true,
                ColumnSize = 3
            });

            schema_array.Add(new VenturaSqlColumn("store_id", typeof(int), false)
            {
                Updateable = true,
                ColumnSize = 4,
                NumericPrecision = 10
            });

            schema_array.Add(new VenturaSqlColumn("staff_id", typeof(int), false)
            {
                Updateable = true,
                ColumnSize = 4,
                NumericPrecision = 10
            });

            schema_array.Add(new VenturaSqlColumn("first_name", typeof(string), true)
            {
                ColumnSize = 255
            });

            schema_array.Add(new VenturaSqlColumn("last_name", typeof(string), true)
            {
                ColumnSize = 255
            });

            schema_array.Add(new VenturaSqlColumn("city", typeof(string), true)
            {
                ColumnSize = 50
            });

            ((IResultsetBase)this).Schema = new VenturaSqlSchema(schema_array);
            ((IResultsetBase)this).UpdateableTablename = "[sales].[orders]";
        }

        /// <summary>
        /// Database Column NotUpdateable. Table [sales].[orders]. PrimaryKey. NotReadonly. NotNull. IsIdentity. AutoIncrement.
        /// </summary>
        public int order_id
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.order_id; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.order_id = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[orders]. NotReadonly. AllowNull.
        /// </summary>
        public int? customer_id
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.customer_id; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.customer_id = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[orders]. NotReadonly. NotNull.
        /// </summary>
        public byte order_status
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.order_status; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.order_status = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[orders]. NotReadonly. NotNull.
        /// </summary>
        public DateTime order_date
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.order_date; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.order_date = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[orders]. NotReadonly. NotNull.
        /// </summary>
        public DateTime required_date
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.required_date; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.required_date = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[orders]. NotReadonly. AllowNull.
        /// </summary>
        public DateTime? shipped_date
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.shipped_date; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.shipped_date = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[orders]. NotReadonly. NotNull.
        /// </summary>
        public int store_id
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.store_id; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.store_id = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[orders]. NotReadonly. NotNull.
        /// </summary>
        public int staff_id
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.staff_id; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.staff_id = value; }
        }

        /// <summary>
        /// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. AllowNull.
        /// </summary>
        public string first_name
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.first_name; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.first_name = value; }
        }

        /// <summary>
        /// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. AllowNull.
        /// </summary>
        public string last_name
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.last_name; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.last_name = value; }
        }

        /// <summary>
        /// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. AllowNull.
        /// </summary>
        public string city
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.city; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.city = value; }
        }

        public void ResetToUnmodified()
        {
            if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET);
            CurrentRecord.ResetToUnmodified();
        }

        public void ResetToUnmodifiedExisting()
        {
            if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET);
            CurrentRecord.ResetToUnmodifiedExisting();
        }

        public void ResetToExisting()
        {
            if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET);
            CurrentRecord.ResetToExisting();
        }

        public void Append()
        {
            int index = this.RecordCount;
            this.InsertItem(index, new SearchOrdersRecord());
            this.CurrentRecordIndex = index;
        }

        public void Append(SearchOrdersRecord record)
        {
            int index = this.RecordCount;
            this.InsertItem(index, record);
            this.CurrentRecordIndex = index;
        }

        public SearchOrdersRecord NewRecord()
        {
            return new SearchOrdersRecord();
        }

        protected override SearchOrdersRecord InternalCreateExistingRecordObject(object[] columnvalues) => new SearchOrdersRecord(columnvalues);

        byte[] IRecordsetBase.Hash
        {
            get { return new byte[] { 240, 116, 161, 68, 181, 161, 118, 191, 202, 32, 85, 248, 28, 0, 95, 116 }; }
        }

        string IRecordsetBase.HashString
        {
            get { return "F074A144B5A176BFCA2055F81C005F74"; }
        }

        VenturaSqlPlatform IRecordsetBase.GeneratorTarget
        {
            get { return VenturaSqlPlatform.NETStandard; }
        }

        Version IRecordsetBase.GeneratorVersion
        {
            get { return new Version(4, 0, 138); }
        }

        DateTime IRecordsetBase.GeneratorTimestamp
        {
            get { return new DateTime(2021, 6, 8, 7, 29, 20); } // Tuesday, June 8, 2021 at 7:29:20 AM
        }

        string IRecordsetBase.GeneratorProviderInvariantName
        {
            get { return null; }
        }

        IResultsetBase[] IRecordsetBase.Resultsets
        {
            get { return _resultsets; }
        }

        string IRecordsetBase.SqlScript
        {
            get { return null; }
        }

        VenturaSqlSchema IRecordsetBase.ParameterSchema
        {
            get { return _parameterschema; }
        }

        /// <summary>
        /// For internal use by VenturaSQL only. Use SetExecSqlParams() instead.
        /// </summary>
        object[] IRecordsetBase.InputParameterValues
        {
            get { return _inputparametervalues; }
        }

        /// <summary>
        /// For internal use by VenturaSQL only. Use Output property instead.
        /// </summary>
        object[] IRecordsetBase.OutputParameterValues
        {
            get { return null; }
        }

        public InputParamHolder InputParam
        {
            get { return _inputparamholder; }
        }

        public int RowLimit
        {
            get { return _rowlimit; }
            set { _rowlimit = value; }
        }

        public void SetExecSqlParams(string SearchText, int? order_id)
        {
            _inputparametervalues[0] = SearchText;
            _inputparametervalues[1] = order_id;
        }

        public void ExecSql(string SearchText, int? order_id)
        {
            _inputparametervalues[0] = SearchText;
            _inputparametervalues[1] = order_id;
            Transactional.ExecSql(VenturaSqlConfig.DefaultConnector, new IRecordsetBase[] { this });
        }

        public void ExecSql(Connector connector, string SearchText, int? order_id)
        {
            _inputparametervalues[0] = SearchText;
            _inputparametervalues[1] = order_id;
            Transactional.ExecSql(connector, new IRecordsetBase[] { this });
        }

        public async Task ExecSqlAsync(string SearchText, int? order_id)
        {
            _inputparametervalues[0] = SearchText;
            _inputparametervalues[1] = order_id;
            await Transactional.ExecSqlAsync(VenturaSqlConfig.DefaultConnector, new IRecordsetBase[] { this });
        }

        public async Task ExecSqlAsync(Connector connector, string SearchText, int? order_id)
        {
            _inputparametervalues[0] = SearchText;
            _inputparametervalues[1] = order_id;
            await Transactional.ExecSqlAsync(connector, new IRecordsetBase[] { this });
        }

        public void SaveChanges()
        {
            Transactional.SaveChanges(VenturaSqlConfig.DefaultConnector, new IRecordsetBase[] { this });
        }

        public void SaveChanges(Connector connector)
        {
            Transactional.SaveChanges(connector, new IRecordsetBase[] { this });
        }

        public async Task SaveChangesAsync()
        {
            await Transactional.SaveChangesAsync(VenturaSqlConfig.DefaultConnector, new IRecordsetBase[] { this });
        }

        public async Task SaveChangesAsync(Connector connector)
        {
            await Transactional.SaveChangesAsync(connector, new IRecordsetBase[] { this });
        }

        public class InputParamHolder
        {
            private object[] _values;

            public InputParamHolder(object[] values)
            {
                _values = values;
            }

            public string SearchText
            {
                get { return (string)_values[0]; }
                set { _values[0] = value; }
            }

            public int? order_id
            {
                get { return (int?)_values[1]; }
                set { _values[1] = value; }
            }

        }

    }

    public sealed partial class SearchOrdersRecord : IRecordBase, INotifyPropertyChanged
    {
        private DataRecordStatus _recordstatus;
        private bool _started_with_dbvalues;

        private int _cur__order_id; private int _ori__order_id; private bool _mod__order_id;
        private int? _cur__customer_id; private int? _ori__customer_id; private bool _mod__customer_id;
        private byte _cur__order_status; private byte _ori__order_status; private bool _mod__order_status;
        private DateTime _cur__order_date; private DateTime _ori__order_date; private bool _mod__order_date;
        private DateTime _cur__required_date; private DateTime _ori__required_date; private bool _mod__required_date;
        private DateTime? _cur__shipped_date; private DateTime? _ori__shipped_date; private bool _mod__shipped_date;
        private int _cur__store_id; private int _ori__store_id; private bool _mod__store_id;
        private int _cur__staff_id; private int _ori__staff_id; private bool _mod__staff_id;
        private string _cur__first_name; private string _ori__first_name; private bool _mod__first_name;
        private string _cur__last_name; private string _ori__last_name; private bool _mod__last_name;
        private string _cur__city; private string _ori__city; private bool _mod__city;


        public SearchOrdersRecord()
        {
            _cur__order_id = 0;
            _cur__customer_id = null;
            _cur__order_status = 0;
            _cur__order_date = new DateTime(1900, 1, 1);
            _cur__required_date = new DateTime(1900, 1, 1);
            _cur__shipped_date = null;
            _cur__store_id = 0;
            _cur__staff_id = 0;
            _cur__first_name = null;
            _cur__last_name = null;
            _cur__city = null;
            _started_with_dbvalues = false;
            _recordstatus = DataRecordStatus.New;
        }

        public SearchOrdersRecord(object[] columnvalues)
        {
            _cur__order_id = (int)columnvalues[0];
            _cur__customer_id = (int?)columnvalues[1];
            _cur__order_status = (byte)columnvalues[2];
            _cur__order_date = (DateTime)columnvalues[3];
            _cur__required_date = (DateTime)columnvalues[4];
            _cur__shipped_date = (DateTime?)columnvalues[5];
            _cur__store_id = (int)columnvalues[6];
            _cur__staff_id = (int)columnvalues[7];
            _cur__first_name = (string)columnvalues[8];
            _cur__last_name = (string)columnvalues[9];
            _cur__city = (string)columnvalues[10];
            _started_with_dbvalues = true;
            _recordstatus = DataRecordStatus.Existing;
        }

        /// <summary>
        /// Database Column NotUpdateable. Table [sales].[orders]. PrimaryKey. NotReadonly. NotNull. IsIdentity. AutoIncrement.
        /// </summary>
        public int order_id
        {
            get { return _cur__order_id; }
            set
            {
                if (_started_with_dbvalues == false) _mod__order_id = true;
                if (_cur__order_id == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__order_id == false) { _ori__order_id = _cur__order_id; _mod__order_id = true; } // existing record and column is not modified
                    else { if (value == _ori__order_id) { _ori__order_id = default(int); _mod__order_id = false; } } // existing record and column is modified
                }
                _cur__order_id = value; OnPropertyChanged("order_id"); OnAfterPropertyChanged("order_id");
            }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[orders]. NotReadonly. AllowNull.
        /// </summary>
        public int? customer_id
        {
            get { return _cur__customer_id; }
            set
            {
                if (_started_with_dbvalues == false) _mod__customer_id = true;
                if (_cur__customer_id == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__customer_id == false) { _ori__customer_id = _cur__customer_id; _mod__customer_id = true; } // existing record and column is not modified
                    else { if (value == _ori__customer_id) { _ori__customer_id = default(int?); _mod__customer_id = false; } } // existing record and column is modified
                }
                _cur__customer_id = value; OnPropertyChanged("customer_id"); OnAfterPropertyChanged("customer_id");
            }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[orders]. NotReadonly. NotNull.
        /// </summary>
        public byte order_status
        {
            get { return _cur__order_status; }
            set
            {
                if (_started_with_dbvalues == false) _mod__order_status = true;
                if (_cur__order_status == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__order_status == false) { _ori__order_status = _cur__order_status; _mod__order_status = true; } // existing record and column is not modified
                    else { if (value == _ori__order_status) { _ori__order_status = default(byte); _mod__order_status = false; } } // existing record and column is modified
                }
                _cur__order_status = value; OnPropertyChanged("order_status"); OnAfterPropertyChanged("order_status");
            }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[orders]. NotReadonly. NotNull.
        /// </summary>
        public DateTime order_date
        {
            get { return _cur__order_date; }
            set
            {
                if (_started_with_dbvalues == false) _mod__order_date = true;
                if (_cur__order_date == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__order_date == false) { _ori__order_date = _cur__order_date; _mod__order_date = true; } // existing record and column is not modified
                    else { if (value == _ori__order_date) { _ori__order_date = default(DateTime); _mod__order_date = false; } } // existing record and column is modified
                }
                _cur__order_date = value; OnPropertyChanged("order_date"); OnAfterPropertyChanged("order_date");
            }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[orders]. NotReadonly. NotNull.
        /// </summary>
        public DateTime required_date
        {
            get { return _cur__required_date; }
            set
            {
                if (_started_with_dbvalues == false) _mod__required_date = true;
                if (_cur__required_date == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__required_date == false) { _ori__required_date = _cur__required_date; _mod__required_date = true; } // existing record and column is not modified
                    else { if (value == _ori__required_date) { _ori__required_date = default(DateTime); _mod__required_date = false; } } // existing record and column is modified
                }
                _cur__required_date = value; OnPropertyChanged("required_date"); OnAfterPropertyChanged("required_date");
            }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[orders]. NotReadonly. AllowNull.
        /// </summary>
        public DateTime? shipped_date
        {
            get { return _cur__shipped_date; }
            set
            {
                if (_started_with_dbvalues == false) _mod__shipped_date = true;
                if (_cur__shipped_date == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__shipped_date == false) { _ori__shipped_date = _cur__shipped_date; _mod__shipped_date = true; } // existing record and column is not modified
                    else { if (value == _ori__shipped_date) { _ori__shipped_date = default(DateTime?); _mod__shipped_date = false; } } // existing record and column is modified
                }
                _cur__shipped_date = value; OnPropertyChanged("shipped_date"); OnAfterPropertyChanged("shipped_date");
            }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[orders]. NotReadonly. NotNull.
        /// </summary>
        public int store_id
        {
            get { return _cur__store_id; }
            set
            {
                if (_started_with_dbvalues == false) _mod__store_id = true;
                if (_cur__store_id == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__store_id == false) { _ori__store_id = _cur__store_id; _mod__store_id = true; } // existing record and column is not modified
                    else { if (value == _ori__store_id) { _ori__store_id = default(int); _mod__store_id = false; } } // existing record and column is modified
                }
                _cur__store_id = value; OnPropertyChanged("store_id"); OnAfterPropertyChanged("store_id");
            }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[orders]. NotReadonly. NotNull.
        /// </summary>
        public int staff_id
        {
            get { return _cur__staff_id; }
            set
            {
                if (_started_with_dbvalues == false) _mod__staff_id = true;
                if (_cur__staff_id == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__staff_id == false) { _ori__staff_id = _cur__staff_id; _mod__staff_id = true; } // existing record and column is not modified
                    else { if (value == _ori__staff_id) { _ori__staff_id = default(int); _mod__staff_id = false; } } // existing record and column is modified
                }
                _cur__staff_id = value; OnPropertyChanged("staff_id"); OnAfterPropertyChanged("staff_id");
            }
        }

        /// <summary>
        /// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. AllowNull.
        /// </summary>
        public string first_name
        {
            get { return _cur__first_name; }
            set
            {
                if (_started_with_dbvalues == false) _mod__first_name = true;
                if (_cur__first_name == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__first_name == false) { _ori__first_name = _cur__first_name; _mod__first_name = true; } // existing record and column is not modified
                    else { if (value == _ori__first_name) { _ori__first_name = default(string); _mod__first_name = false; } } // existing record and column is modified
                }
                _cur__first_name = value; OnPropertyChanged("first_name"); OnAfterPropertyChanged("first_name");
            }
        }

        /// <summary>
        /// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. AllowNull.
        /// </summary>
        public string last_name
        {
            get { return _cur__last_name; }
            set
            {
                if (_started_with_dbvalues == false) _mod__last_name = true;
                if (_cur__last_name == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__last_name == false) { _ori__last_name = _cur__last_name; _mod__last_name = true; } // existing record and column is not modified
                    else { if (value == _ori__last_name) { _ori__last_name = default(string); _mod__last_name = false; } } // existing record and column is modified
                }
                _cur__last_name = value; OnPropertyChanged("last_name"); OnAfterPropertyChanged("last_name");
            }
        }

        /// <summary>
        /// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. AllowNull.
        /// </summary>
        public string city
        {
            get { return _cur__city; }
            set
            {
                if (_started_with_dbvalues == false) _mod__city = true;
                if (_cur__city == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__city == false) { _ori__city = _cur__city; _mod__city = true; } // existing record and column is not modified
                    else { if (value == _ori__city) { _ori__city = default(string); _mod__city = false; } } // existing record and column is modified
                }
                _cur__city = value; OnPropertyChanged("city"); OnAfterPropertyChanged("city");
            }
        }

        public bool IsModified(string column_name)
        {
            if (column_name == "order_id") return _mod__order_id;
            if (column_name == "customer_id") return _mod__customer_id;
            if (column_name == "order_status") return _mod__order_status;
            if (column_name == "order_date") return _mod__order_date;
            if (column_name == "required_date") return _mod__required_date;
            if (column_name == "shipped_date") return _mod__shipped_date;
            if (column_name == "store_id") return _mod__store_id;
            if (column_name == "staff_id") return _mod__staff_id;
            if (column_name == "first_name") return _mod__first_name;
            if (column_name == "last_name") return _mod__last_name;
            if (column_name == "city") return _mod__city;
            throw new ArgumentOutOfRangeException(String.Format(VenturaSqlStrings.UNKNOWN_COLUMN_NAME, column_name));
        }

        public int ModifiedColumnCount()
        {
            int count = 0;
            if (_mod__order_id == true) count++;
            if (_mod__customer_id == true) count++;
            if (_mod__order_status == true) count++;
            if (_mod__order_date == true) count++;
            if (_mod__required_date == true) count++;
            if (_mod__shipped_date == true) count++;
            if (_mod__store_id == true) count++;
            if (_mod__staff_id == true) count++;
            if (_mod__first_name == true) count++;
            if (_mod__last_name == true) count++;
            if (_mod__city == true) count++;
            return count;
        }

        public bool PendingChanges()
        {
            if (_recordstatus == DataRecordStatus.New || _recordstatus == DataRecordStatus.ExistingDelete) return true;
            int count = 0;
            if (_mod__customer_id == true) count++;
            if (_mod__order_status == true) count++;
            if (_mod__order_date == true) count++;
            if (_mod__required_date == true) count++;
            if (_mod__shipped_date == true) count++;
            if (_mod__store_id == true) count++;
            if (_mod__staff_id == true) count++;
            return count > 0;
        }

        public DataRecordStatus RecordStatus()
        {
            return _recordstatus;
        }

        DataRecordStatus IRecordBase.RecordStatus
        {
            get { return _recordstatus; }
            set { _recordstatus = value; }
        }

        void IRecordBase.ValidateBeforeSaving(int record_index_to_display)
        {
        }

        void IRecordBase.WriteChangesToTrackArray(TrackArray track_array)
        {
            if (_recordstatus == DataRecordStatus.New)
            {
                if (_cur__customer_id != null) track_array.AppendDataValue(1, _cur__customer_id);
                track_array.AppendDataValue(2, _cur__order_status);
                track_array.AppendDataValue(3, _cur__order_date);
                track_array.AppendDataValue(4, _cur__required_date);
                if (_cur__shipped_date != null) track_array.AppendDataValue(5, _cur__shipped_date);
                track_array.AppendDataValue(6, _cur__store_id);
                track_array.AppendDataValue(7, _cur__staff_id);
            }
            else if (_recordstatus == DataRecordStatus.Existing)
            {
                if (_mod__customer_id) track_array.AppendDataValue(1, _cur__customer_id);
                if (_mod__order_status) track_array.AppendDataValue(2, _cur__order_status);
                if (_mod__order_date) track_array.AppendDataValue(3, _cur__order_date);
                if (_mod__required_date) track_array.AppendDataValue(4, _cur__required_date);
                if (_mod__shipped_date) track_array.AppendDataValue(5, _cur__shipped_date);
                if (_mod__store_id) track_array.AppendDataValue(6, _cur__store_id);
                if (_mod__staff_id) track_array.AppendDataValue(7, _cur__staff_id);
                if (track_array.HasData == false) return;
            }

            if (_recordstatus == DataRecordStatus.Existing || _recordstatus == DataRecordStatus.ExistingDelete)
            {
                track_array.AppendPrikeyValue(0, (_mod__order_id && _started_with_dbvalues) ? _ori__order_id : _cur__order_id);
            }

            if (_recordstatus == DataRecordStatus.New) track_array.Status = TrackArrayStatus.DataForINSERT;
            else if (_recordstatus == DataRecordStatus.Existing) track_array.Status = TrackArrayStatus.DataForUPDATE;
            else if (_recordstatus == DataRecordStatus.ExistingDelete) track_array.Status = TrackArrayStatus.DataForDELETE;
        }

        public bool StartedWithDbValues()
        {
            return _started_with_dbvalues;
        }

        /// <summary>
        /// Resets all columns to not-modified status.
        /// </summary>
        public void ResetToUnmodified()
        {
            if (_started_with_dbvalues == true)
            {
                if (_mod__order_id) _ori__order_id = default(int);
                if (_mod__customer_id) _ori__customer_id = default(int?);
                if (_mod__order_status) _ori__order_status = default(byte);
                if (_mod__order_date) _ori__order_date = default(DateTime);
                if (_mod__required_date) _ori__required_date = default(DateTime);
                if (_mod__shipped_date) _ori__shipped_date = default(DateTime?);
                if (_mod__store_id) _ori__store_id = default(int);
                if (_mod__staff_id) _ori__staff_id = default(int);
                if (_mod__first_name) _ori__first_name = default(string);
                if (_mod__last_name) _ori__last_name = default(string);
                if (_mod__city) _ori__city = default(string);
            }
            _mod__order_id = false;
            _mod__customer_id = false;
            _mod__order_status = false;
            _mod__order_date = false;
            _mod__required_date = false;
            _mod__shipped_date = false;
            _mod__store_id = false;
            _mod__staff_id = false;
            _mod__first_name = false;
            _mod__last_name = false;
            _mod__city = false;
        }

        /// <summary>
        /// Resets the record to DataRecordStatus.Existing. Like it was freshly loaded from the database.
        /// </summary>
        public void ResetToUnmodifiedExisting()
        {
            ResetToUnmodified();
            _recordstatus = DataRecordStatus.Existing;
        }

        /// <summary>
        /// Resets the record to DataRecordStatus.Existing.
        /// </summary>
        public void ResetToExisting()
        {
            _recordstatus = DataRecordStatus.Existing;
        }

        void IRecordBase.SetIdentityColumnValue(object value)
        {
            _cur__order_id = (int)value;
            OnPropertyChanged("order_id");
            OnAfterPropertyChanged("order_id");
        }

        partial void OnAfterPropertyChanged(string propertyName);

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

// The following commented out code is a template for implementing calculated columns.
//
// How to guide: https://docs.sysdev.nl/CalculatedColumns.html

/*
namespace BlazorDemo.Client.VenturaRecordsets
{
	public partial class SearchOrdersRecord
	{
		partial void OnAfterPropertyChanged(string propertyName)
		{
			if (propertyName == "FirstName" || propertyName == "LastName")
				this.OnPropertyChanged("FirstNameLastName");
		}

		public string FirstNameLastName
		{
			get
			{
				return this.FirstName + " " + this.LastName;
			}
		}
	}

}
*/
