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

namespace BlazorDemo.Client.VenturaAutoCreate
{
    /// <summary>
    /// The updateable table is [sales].[stores]. Updateable table column information:
    /// • 8 out of 8 table columns are present in the resultset.
    /// • All primary key columns are present in the resultset: store_id.
    /// • Non-primary key columns present in the resultset: store_name, phone, email, street, city, state and zip_code.
    /// Recordset item automatically created by VenturaSQL Studio.
    /// </summary>
    public partial class PriKey_sales_stores_Recordset : ResultsetObservable<PriKey_sales_stores_Recordset, PriKey_sales_stores_Record>, IRecordsetBase
    {
        private IResultsetBase[] _resultsets;
        private object[] _inputparametervalues;
        private InputParamHolder _inputparamholder;
        private VenturaSqlSchema _parameterschema;
        private int _rowlimit = 500;
        private const string CRLF = "\r\n";

        public PriKey_sales_stores_Recordset()
        {
            _resultsets = new IResultsetBase[] { this };


            _inputparametervalues = new object[1];
            _inputparamholder = new InputParamHolder(_inputparametervalues);

            ColumnArrayBuilder param_array = new ColumnArrayBuilder();

            param_array.AddParameterColumn("@store_id", typeof(int), true, false, DbType.Int32, null, null, null);

            _parameterschema = new VenturaSqlSchema(param_array);

            ColumnArrayBuilder schema_array = new ColumnArrayBuilder();

            schema_array.Add(new VenturaSqlColumn("store_id", typeof(int), false)
            {
                ColumnSize = 4,
                NumericPrecision = 10,
                IsKey = true,
                IsIdentity = true,
                IsAutoIncrement = true
            });

            schema_array.Add(new VenturaSqlColumn("store_name", typeof(string), false)
            {
                Updateable = true,
                ColumnSize = 255
            });

            schema_array.Add(new VenturaSqlColumn("phone", typeof(string), true)
            {
                Updateable = true,
                ColumnSize = 25
            });

            schema_array.Add(new VenturaSqlColumn("email", typeof(string), true)
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
                ColumnSize = 255
            });

            schema_array.Add(new VenturaSqlColumn("state", typeof(string), true)
            {
                Updateable = true,
                ColumnSize = 10
            });

            schema_array.Add(new VenturaSqlColumn("zip_code", typeof(string), true)
            {
                Updateable = true,
                ColumnSize = 5
            });

            ((IResultsetBase)this).Schema = new VenturaSqlSchema(schema_array);
            ((IResultsetBase)this).UpdateableTablename = "[sales].[stores]";
        }

        /// <summary>
        /// Database Column NotUpdateable. Table [sales].[stores]. PrimaryKey. NotReadonly. NotNull. IsIdentity. AutoIncrement.
        /// </summary>
        public int store_id
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.store_id; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.store_id = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[stores]. NotReadonly. NotNull.
        /// </summary>
        public string store_name
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.store_name; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.store_name = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[stores]. NotReadonly. AllowNull.
        /// </summary>
        public string phone
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.phone; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.phone = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[stores]. NotReadonly. AllowNull.
        /// </summary>
        public string email
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.email; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.email = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[stores]. NotReadonly. AllowNull.
        /// </summary>
        public string street
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.street; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.street = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[stores]. NotReadonly. AllowNull.
        /// </summary>
        public string city
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.city; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.city = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[stores]. NotReadonly. AllowNull.
        /// </summary>
        public string state
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.state; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.state = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[stores]. NotReadonly. AllowNull.
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
            this.InsertItem(index, new PriKey_sales_stores_Record());
            this.CurrentRecordIndex = index;
        }

        public void Append(PriKey_sales_stores_Record record)
        {
            int index = this.RecordCount;
            this.InsertItem(index, record);
            this.CurrentRecordIndex = index;
        }

        public PriKey_sales_stores_Record NewRecord()
        {
            return new PriKey_sales_stores_Record();
        }

        protected override PriKey_sales_stores_Record InternalCreateExistingRecordObject(object[] columnvalues) => new PriKey_sales_stores_Record(columnvalues);

        byte[] IRecordsetBase.Hash
        {
            get { return new byte[] { 235, 142, 190, 33, 90, 142, 193, 119, 19, 95, 121, 148, 205, 254, 162, 233 }; }
        }

        string IRecordsetBase.HashString
        {
            get { return "EB8EBE215A8EC177135F7994CDFEA2E9"; }
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

        public void SetExecSqlParams(int? store_id)
        {
            _inputparametervalues[0] = store_id;
        }

        public void ExecSql(int? store_id)
        {
            _inputparametervalues[0] = store_id;
            Transactional.ExecSql(VenturaSqlConfig.DefaultConnector, new IRecordsetBase[] { this });
        }

        public void ExecSql(Connector connector, int? store_id)
        {
            _inputparametervalues[0] = store_id;
            Transactional.ExecSql(connector, new IRecordsetBase[] { this });
        }

        public async Task ExecSqlAsync(int? store_id)
        {
            _inputparametervalues[0] = store_id;
            await Transactional.ExecSqlAsync(VenturaSqlConfig.DefaultConnector, new IRecordsetBase[] { this });
        }

        public async Task ExecSqlAsync(Connector connector, int? store_id)
        {
            _inputparametervalues[0] = store_id;
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

            public int? store_id
            {
                get { return (int?)_values[0]; }
                set { _values[0] = value; }
            }

        }

    }

    public sealed partial class PriKey_sales_stores_Record : IRecordBase, INotifyPropertyChanged
    {
        private DataRecordStatus _recordstatus;
        private bool _started_with_dbvalues;

        private int _cur__store_id; private int _ori__store_id; private bool _mod__store_id;
        private string _cur__store_name; private string _ori__store_name; private bool _mod__store_name;
        private string _cur__phone; private string _ori__phone; private bool _mod__phone;
        private string _cur__email; private string _ori__email; private bool _mod__email;
        private string _cur__street; private string _ori__street; private bool _mod__street;
        private string _cur__city; private string _ori__city; private bool _mod__city;
        private string _cur__state; private string _ori__state; private bool _mod__state;
        private string _cur__zip_code; private string _ori__zip_code; private bool _mod__zip_code;


        public PriKey_sales_stores_Record()
        {
            _cur__store_id = 0;
            _cur__store_name = "";
            _cur__phone = null;
            _cur__email = null;
            _cur__street = null;
            _cur__city = null;
            _cur__state = null;
            _cur__zip_code = null;
            _started_with_dbvalues = false;
            _recordstatus = DataRecordStatus.New;
        }

        public PriKey_sales_stores_Record(object[] columnvalues)
        {
            _cur__store_id = (int)columnvalues[0];
            _cur__store_name = (string)columnvalues[1];
            _cur__phone = (string)columnvalues[2];
            _cur__email = (string)columnvalues[3];
            _cur__street = (string)columnvalues[4];
            _cur__city = (string)columnvalues[5];
            _cur__state = (string)columnvalues[6];
            _cur__zip_code = (string)columnvalues[7];
            _started_with_dbvalues = true;
            _recordstatus = DataRecordStatus.Existing;
        }

        /// <summary>
        /// Database Column NotUpdateable. Table [sales].[stores]. PrimaryKey. NotReadonly. NotNull. IsIdentity. AutoIncrement.
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
        /// Database Column Updateable. Table [sales].[stores]. NotReadonly. NotNull.
        /// </summary>
        public string store_name
        {
            get { return _cur__store_name; }
            set
            {
                if (value == null) throw new ArgumentNullException("store_name", VenturaSqlStrings.SET_NULL_MSG);
                if (_started_with_dbvalues == false) _mod__store_name = true;
                if (_cur__store_name == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__store_name == false) { _ori__store_name = _cur__store_name; _mod__store_name = true; } // existing record and column is not modified
                    else { if (value == _ori__store_name) { _ori__store_name = default(string); _mod__store_name = false; } } // existing record and column is modified
                }
                _cur__store_name = value; OnPropertyChanged("store_name"); OnAfterPropertyChanged("store_name");
            }
        }

        /// <summary>
        /// Database Column Updateable. Table [sales].[stores]. NotReadonly. AllowNull.
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
        /// Database Column Updateable. Table [sales].[stores]. NotReadonly. AllowNull.
        /// </summary>
        public string email
        {
            get { return _cur__email; }
            set
            {
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
        /// Database Column Updateable. Table [sales].[stores]. NotReadonly. AllowNull.
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
        /// Database Column Updateable. Table [sales].[stores]. NotReadonly. AllowNull.
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
        /// Database Column Updateable. Table [sales].[stores]. NotReadonly. AllowNull.
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
        /// Database Column Updateable. Table [sales].[stores]. NotReadonly. AllowNull.
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
            if (column_name == "store_id") return _mod__store_id;
            if (column_name == "store_name") return _mod__store_name;
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
            if (_mod__store_id == true) count++;
            if (_mod__store_name == true) count++;
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
            if (_mod__store_name == true) count++;
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
                track_array.AppendDataValue(1, _cur__store_name);
                if (_cur__phone != null) track_array.AppendDataValue(2, _cur__phone);
                if (_cur__email != null) track_array.AppendDataValue(3, _cur__email);
                if (_cur__street != null) track_array.AppendDataValue(4, _cur__street);
                if (_cur__city != null) track_array.AppendDataValue(5, _cur__city);
                if (_cur__state != null) track_array.AppendDataValue(6, _cur__state);
                if (_cur__zip_code != null) track_array.AppendDataValue(7, _cur__zip_code);
            }
            else if (_recordstatus == DataRecordStatus.Existing)
            {
                if (_mod__store_name) track_array.AppendDataValue(1, _cur__store_name);
                if (_mod__phone) track_array.AppendDataValue(2, _cur__phone);
                if (_mod__email) track_array.AppendDataValue(3, _cur__email);
                if (_mod__street) track_array.AppendDataValue(4, _cur__street);
                if (_mod__city) track_array.AppendDataValue(5, _cur__city);
                if (_mod__state) track_array.AppendDataValue(6, _cur__state);
                if (_mod__zip_code) track_array.AppendDataValue(7, _cur__zip_code);
                if (track_array.HasData == false) return;
            }

            if (_recordstatus == DataRecordStatus.Existing || _recordstatus == DataRecordStatus.ExistingDelete)
            {
                track_array.AppendPrikeyValue(0, (_mod__store_id && _started_with_dbvalues) ? _ori__store_id : _cur__store_id);
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
                if (_mod__store_id) _ori__store_id = default(int);
                if (_mod__store_name) _ori__store_name = default(string);
                if (_mod__phone) _ori__phone = default(string);
                if (_mod__email) _ori__email = default(string);
                if (_mod__street) _ori__street = default(string);
                if (_mod__city) _ori__city = default(string);
                if (_mod__state) _ori__state = default(string);
                if (_mod__zip_code) _ori__zip_code = default(string);
            }
            _mod__store_id = false;
            _mod__store_name = false;
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
            _cur__store_id = (int)value;
            OnPropertyChanged("store_id");
            OnAfterPropertyChanged("store_id");
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
	public partial class PriKey_sales_stores_Record
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
