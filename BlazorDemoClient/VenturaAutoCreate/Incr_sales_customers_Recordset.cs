/*
	Project file: "C:\Active\Kenova\Projects\BikeStores.venproj"
	Target platform: NETStandard
	Generator version: 4.0.138
	Generated on: Tuesday, June 8, 2021 at 7:29:20 AM
	At the bottom of this file you find a template for extending Recordsets with calculated columns for XAML data binding.
*/
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using VenturaSQL;

namespace BlazorDemo.Client.VenturaAutoCreate
{
    /// <summary>
    /// The updateable table is [sales].[customers]. Updateable table column information:
    /// • 9 out of 9 table columns are present in the resultset.
    /// • All primary key columns are present in the resultset: customer_id.
    /// • Non-primary key columns present in the resultset: first_name, last_name, phone, email, street, city, state and zip_code.
    /// Recordset item automatically created by VenturaSQL Studio.
    /// </summary>
    public partial class Incr_sales_customers_Recordset : ResultsetObservable<Incr_sales_customers_Recordset, Incr_sales_customers_Record>, IRecordsetBase, IRecordsetIncremental
    {
        private bool _has_more_rows = false;
        private Connector _incremental_connector = null;
        private int _incremental_offset = 0;
        private int _last_exec_startindex = -1;
        private int _last_exec_count = 0;
        private IResultsetBase[] _resultsets;
        private int _rowlimit = 500;
        private const string CRLF = "\r\n";

        public Incr_sales_customers_Recordset()
        {
            _resultsets = new IResultsetBase[] { this };


            ColumnArrayBuilder schema_array = new ColumnArrayBuilder();

            schema_array.Add(new VenturaSqlColumn("customer_id", typeof(int), false)
            {
                ColumnSize = 4,
                NumericPrecision = 10,
                IsKey = true,
                IsIdentity = true,
                IsAutoIncrement = true
            });

            schema_array.Add(new VenturaSqlColumn("first_name", typeof(string), false)
            {
                Updateable = true,
                ColumnSize = 255
            });

            schema_array.Add(new VenturaSqlColumn("last_name", typeof(string), false)
            {
                Updateable = true,
                ColumnSize = 255
            });

            schema_array.Add(new VenturaSqlColumn("phone", typeof(string), true)
            {
                Updateable = true,
                ColumnSize = 25
            });

            schema_array.Add(new VenturaSqlColumn("email", typeof(string), false)
            {
                Updateable = true,
                ColumnSize = 255
            });

            schema_array.Add(new VenturaSqlColumn("street", typeof(string), true)
            {
                Updateable = true,
                ColumnSize = 255
            });

            schema_array.Add(new VenturaSqlColumn("city", typeof(string), true)
            {
                Updateable = true,
                ColumnSize = 50
            });

            schema_array.Add(new VenturaSqlColumn("state", typeof(string), true)
            {
                Updateable = true,
                ColumnSize = 25
            });

            schema_array.Add(new VenturaSqlColumn("zip_code", typeof(string), true)
            {
                Updateable = true,
                ColumnSize = 5
            });

            ((IResultsetBase)this).Schema = new VenturaSqlSchema(schema_array);
            ((IResultsetBase)this).UpdateableTablename = "[sales].[customers]";
        }

        /// <summary>
        /// Database Column NotUpdateable. Table [sales].[customers]. PrimaryKey. NotReadonly. NotNull. IsIdentity. AutoIncrement.
        /// </summary>
        public int customer_id
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.customer_id; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.customer_id = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[customers]. NotReadonly. NotNull.
        /// </summary>
        public string first_name
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.first_name; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.first_name = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[customers]. NotReadonly. NotNull.
        /// </summary>
        public string last_name
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.last_name; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.last_name = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[customers]. NotReadonly. AllowNull.
        /// </summary>
        public string phone
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.phone; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.phone = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[customers]. NotReadonly. NotNull.
        /// </summary>
        public string email
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.email; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.email = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[customers]. NotReadonly. AllowNull.
        /// </summary>
        public string street
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.street; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.street = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[customers]. NotReadonly. AllowNull.
        /// </summary>
        public string city
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.city; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.city = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[customers]. NotReadonly. AllowNull.
        /// </summary>
        public string state
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.state; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.state = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[customers]. NotReadonly. AllowNull.
        /// </summary>
        public string zip_code
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.zip_code; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.zip_code = value; }
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
            this.InsertItem(index, new Incr_sales_customers_Record());
            this.CurrentRecordIndex = index;
        }

        public void Append(Incr_sales_customers_Record record)
        {
            int index = this.RecordCount;
            this.InsertItem(index, record);
            this.CurrentRecordIndex = index;
        }

        public Incr_sales_customers_Record NewRecord()
        {
            return new Incr_sales_customers_Record();
        }

        protected override Incr_sales_customers_Record InternalCreateExistingRecordObject(object[] columnvalues) => new Incr_sales_customers_Record(columnvalues);

        byte[] IRecordsetBase.Hash
        {
            get { return new byte[] { 116, 33, 13, 232, 118, 251, 60, 104, 254, 112, 248, 67, 223, 134, 174, 20 }; }
        }

        string IRecordsetBase.HashString
        {
            get { return "74210DE876FB3C68FE70F843DF86AE14"; }
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

        Connector IRecordsetIncremental.IncrementalConnector
        {
            get { return _incremental_connector; }
            set { _incremental_connector = value; }
        }

        int IRecordsetIncremental.IncrementalOffset
        {
            get { return _incremental_offset; }
            set { _incremental_offset = value; }
        }

        int IRecordsetIncremental.LastExecCount
        {
            get { return _last_exec_count; }
            set { _last_exec_count = value; }
        }

        int IRecordsetIncremental.LastExecStartIndex
        {
            get { return _last_exec_startindex; }
            set { _last_exec_startindex = value; }
        }

        bool IRecordsetIncremental.HasMoreRows
        {
            get { return _has_more_rows; }
            set { _has_more_rows = value; }
        }

        public bool HasMoreRows
        {
            get { return _has_more_rows; }
        }

        string IRecordsetBase.SqlScript
        {
            get { return null; }
        }

        VenturaSqlSchema IRecordsetBase.ParameterSchema
        {
            get { return null; }
        }

        /// <summary>
        /// For internal use by VenturaSQL only. Use SetExecSqlParams() instead.
        /// </summary>
        object[] IRecordsetBase.InputParameterValues
        {
            get { return null; }
        }

        /// <summary>
        /// For internal use by VenturaSQL only. Use Output property instead.
        /// </summary>
        object[] IRecordsetBase.OutputParameterValues
        {
            get { return null; }
        }

        public int RowLimit
        {
            get { return _rowlimit; }
            set { _rowlimit = value; }
        }

        public void ExecSql()
        {
            Transactional.ExecSql(VenturaSqlConfig.DefaultConnector, new IRecordsetBase[] { this });
        }

        public void ExecSql(Connector connector)
        {
            Transactional.ExecSql(connector, new IRecordsetBase[] { this });
        }

        public void ExecSqlIncremental()
        {
            Transactional.ExecSqlIncremental(this);
        }

        public async Task ExecSqlAsync()
        {
            await Transactional.ExecSqlAsync(VenturaSqlConfig.DefaultConnector, new IRecordsetBase[] { this });
        }

        public async Task ExecSqlAsync(Connector connector)
        {
            await Transactional.ExecSqlAsync(connector, new IRecordsetBase[] { this });
        }

        public async Task ExecSqlIncrementalAsync()
        {
            await Transactional.ExecSqlIncrementalAsync(this);
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

    }

    public sealed partial class Incr_sales_customers_Record : IRecordBase, INotifyPropertyChanged
    {
        private DataRecordStatus _recordstatus;
        private bool _started_with_dbvalues;

        private int _cur__customer_id; private int _ori__customer_id; private bool _mod__customer_id;
        private string _cur__first_name; private string _ori__first_name; private bool _mod__first_name;
        private string _cur__last_name; private string _ori__last_name; private bool _mod__last_name;
        private string _cur__phone; private string _ori__phone; private bool _mod__phone;
        private string _cur__email; private string _ori__email; private bool _mod__email;
        private string _cur__street; private string _ori__street; private bool _mod__street;
        private string _cur__city; private string _ori__city; private bool _mod__city;
        private string _cur__state; private string _ori__state; private bool _mod__state;
        private string _cur__zip_code; private string _ori__zip_code; private bool _mod__zip_code;


        public Incr_sales_customers_Record()
        {
            _cur__customer_id = 0;
            _cur__first_name = "";
            _cur__last_name = "";
            _cur__phone = null;
            _cur__email = "";
            _cur__street = null;
            _cur__city = null;
            _cur__state = null;
            _cur__zip_code = null;
            _started_with_dbvalues = false;
            _recordstatus = DataRecordStatus.New;
        }

        public Incr_sales_customers_Record(object[] columnvalues)
        {
            _cur__customer_id = (int)columnvalues[0];
            _cur__first_name = (string)columnvalues[1];
            _cur__last_name = (string)columnvalues[2];
            _cur__phone = (string)columnvalues[3];
            _cur__email = (string)columnvalues[4];
            _cur__street = (string)columnvalues[5];
            _cur__city = (string)columnvalues[6];
            _cur__state = (string)columnvalues[7];
            _cur__zip_code = (string)columnvalues[8];
            _started_with_dbvalues = true;
            _recordstatus = DataRecordStatus.Existing;
        }

        /// <summary>
        /// Database Column NotUpdateable. Table [sales].[customers]. PrimaryKey. NotReadonly. NotNull. IsIdentity. AutoIncrement.
        /// </summary>
        public int customer_id
        {
            get { return _cur__customer_id; }
            set
            {
                if (_started_with_dbvalues == false) _mod__customer_id = true;
                if (_cur__customer_id == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__customer_id == false) { _ori__customer_id = _cur__customer_id; _mod__customer_id = true; } // existing record and column is not modified
                    else { if (value == _ori__customer_id) { _ori__customer_id = default(int); _mod__customer_id = false; } } // existing record and column is modified
                }
                _cur__customer_id = value; OnPropertyChanged("customer_id"); OnAfterPropertyChanged("customer_id");
            }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[customers]. NotReadonly. NotNull.
        /// </summary>
        public string first_name
        {
            get { return _cur__first_name; }
            set
            {
                if (value == null) throw new ArgumentNullException("first_name", VenturaSqlStrings.SET_NULL_MSG);
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
        /// Database Column Updateable. Table [sales].[customers]. NotReadonly. NotNull.
        /// </summary>
        public string last_name
        {
            get { return _cur__last_name; }
            set
            {
                if (value == null) throw new ArgumentNullException("last_name", VenturaSqlStrings.SET_NULL_MSG);
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
        /// Database Column Updateable. Table [sales].[customers]. NotReadonly. AllowNull.
        /// </summary>
        public string phone
        {
            get { return _cur__phone; }
            set
            {
                if (_started_with_dbvalues == false) _mod__phone = true;
                if (_cur__phone == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__phone == false) { _ori__phone = _cur__phone; _mod__phone = true; } // existing record and column is not modified
                    else { if (value == _ori__phone) { _ori__phone = default(string); _mod__phone = false; } } // existing record and column is modified
                }
                _cur__phone = value; OnPropertyChanged("phone"); OnAfterPropertyChanged("phone");
            }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[customers]. NotReadonly. NotNull.
        /// </summary>
        public string email
        {
            get { return _cur__email; }
            set
            {
                if (value == null) throw new ArgumentNullException("email", VenturaSqlStrings.SET_NULL_MSG);
                if (_started_with_dbvalues == false) _mod__email = true;
                if (_cur__email == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__email == false) { _ori__email = _cur__email; _mod__email = true; } // existing record and column is not modified
                    else { if (value == _ori__email) { _ori__email = default(string); _mod__email = false; } } // existing record and column is modified
                }
                _cur__email = value; OnPropertyChanged("email"); OnAfterPropertyChanged("email");
            }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[customers]. NotReadonly. AllowNull.
        /// </summary>
        public string street
        {
            get { return _cur__street; }
            set
            {
                if (_started_with_dbvalues == false) _mod__street = true;
                if (_cur__street == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__street == false) { _ori__street = _cur__street; _mod__street = true; } // existing record and column is not modified
                    else { if (value == _ori__street) { _ori__street = default(string); _mod__street = false; } } // existing record and column is modified
                }
                _cur__street = value; OnPropertyChanged("street"); OnAfterPropertyChanged("street");
            }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[customers]. NotReadonly. AllowNull.
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

        /// <summary>
        /// Database Column Updateable. Table [sales].[customers]. NotReadonly. AllowNull.
        /// </summary>
        public string state
        {
            get { return _cur__state; }
            set
            {
                if (_started_with_dbvalues == false) _mod__state = true;
                if (_cur__state == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__state == false) { _ori__state = _cur__state; _mod__state = true; } // existing record and column is not modified
                    else { if (value == _ori__state) { _ori__state = default(string); _mod__state = false; } } // existing record and column is modified
                }
                _cur__state = value; OnPropertyChanged("state"); OnAfterPropertyChanged("state");
            }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[customers]. NotReadonly. AllowNull.
        /// </summary>
        public string zip_code
        {
            get { return _cur__zip_code; }
            set
            {
                if (_started_with_dbvalues == false) _mod__zip_code = true;
                if (_cur__zip_code == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__zip_code == false) { _ori__zip_code = _cur__zip_code; _mod__zip_code = true; } // existing record and column is not modified
                    else { if (value == _ori__zip_code) { _ori__zip_code = default(string); _mod__zip_code = false; } } // existing record and column is modified
                }
                _cur__zip_code = value; OnPropertyChanged("zip_code"); OnAfterPropertyChanged("zip_code");
            }
        }

        public bool IsModified(string column_name)
        {
            if (column_name == "customer_id") return _mod__customer_id;
            if (column_name == "first_name") return _mod__first_name;
            if (column_name == "last_name") return _mod__last_name;
            if (column_name == "phone") return _mod__phone;
            if (column_name == "email") return _mod__email;
            if (column_name == "street") return _mod__street;
            if (column_name == "city") return _mod__city;
            if (column_name == "state") return _mod__state;
            if (column_name == "zip_code") return _mod__zip_code;
            throw new ArgumentOutOfRangeException(String.Format(VenturaSqlStrings.UNKNOWN_COLUMN_NAME, column_name));
        }

        public int ModifiedColumnCount()
        {
            int count = 0;
            if (_mod__customer_id == true) count++;
            if (_mod__first_name == true) count++;
            if (_mod__last_name == true) count++;
            if (_mod__phone == true) count++;
            if (_mod__email == true) count++;
            if (_mod__street == true) count++;
            if (_mod__city == true) count++;
            if (_mod__state == true) count++;
            if (_mod__zip_code == true) count++;
            return count;
        }

        public bool PendingChanges()
        {
            if (_recordstatus == DataRecordStatus.New || _recordstatus == DataRecordStatus.ExistingDelete) return true;
            int count = 0;
            if (_mod__first_name == true) count++;
            if (_mod__last_name == true) count++;
            if (_mod__phone == true) count++;
            if (_mod__email == true) count++;
            if (_mod__street == true) count++;
            if (_mod__city == true) count++;
            if (_mod__state == true) count++;
            if (_mod__zip_code == true) count++;
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
                track_array.AppendDataValue(1, _cur__first_name);
                track_array.AppendDataValue(2, _cur__last_name);
                if (_cur__phone != null) track_array.AppendDataValue(3, _cur__phone);
                track_array.AppendDataValue(4, _cur__email);
                if (_cur__street != null) track_array.AppendDataValue(5, _cur__street);
                if (_cur__city != null) track_array.AppendDataValue(6, _cur__city);
                if (_cur__state != null) track_array.AppendDataValue(7, _cur__state);
                if (_cur__zip_code != null) track_array.AppendDataValue(8, _cur__zip_code);
            }
            else if (_recordstatus == DataRecordStatus.Existing)
            {
                if (_mod__first_name) track_array.AppendDataValue(1, _cur__first_name);
                if (_mod__last_name) track_array.AppendDataValue(2, _cur__last_name);
                if (_mod__phone) track_array.AppendDataValue(3, _cur__phone);
                if (_mod__email) track_array.AppendDataValue(4, _cur__email);
                if (_mod__street) track_array.AppendDataValue(5, _cur__street);
                if (_mod__city) track_array.AppendDataValue(6, _cur__city);
                if (_mod__state) track_array.AppendDataValue(7, _cur__state);
                if (_mod__zip_code) track_array.AppendDataValue(8, _cur__zip_code);
                if (track_array.HasData == false) return;
            }

            if (_recordstatus == DataRecordStatus.Existing || _recordstatus == DataRecordStatus.ExistingDelete)
            {
                track_array.AppendPrikeyValue(0, (_mod__customer_id && _started_with_dbvalues) ? _ori__customer_id : _cur__customer_id);
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
                if (_mod__customer_id) _ori__customer_id = default(int);
                if (_mod__first_name) _ori__first_name = default(string);
                if (_mod__last_name) _ori__last_name = default(string);
                if (_mod__phone) _ori__phone = default(string);
                if (_mod__email) _ori__email = default(string);
                if (_mod__street) _ori__street = default(string);
                if (_mod__city) _ori__city = default(string);
                if (_mod__state) _ori__state = default(string);
                if (_mod__zip_code) _ori__zip_code = default(string);
            }
            _mod__customer_id = false;
            _mod__first_name = false;
            _mod__last_name = false;
            _mod__phone = false;
            _mod__email = false;
            _mod__street = false;
            _mod__city = false;
            _mod__state = false;
            _mod__zip_code = false;
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
            _cur__customer_id = (int)value;
            OnPropertyChanged("customer_id");
            OnAfterPropertyChanged("customer_id");
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
namespace BlazorDemo.Client.VenturaAutoCreate
{
	public partial class Incr_sales_customers_Record
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
