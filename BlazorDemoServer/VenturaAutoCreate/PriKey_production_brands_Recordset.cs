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

namespace BlazorDemo.Server.VenturaAutoCreate
{
    /// <summary>
    /// The updateable table is [production].[brands]. Updateable table column information:
    /// • 2 out of 2 table columns are present in the resultset.
    /// • All primary key columns are present in the resultset: brand_id.
    /// • Non-primary key column present in the resultset: brand_name.
    /// Recordset item automatically created by VenturaSQL Studio.
    /// </summary>
    public partial class PriKey_production_brands_Recordset : ResultsetObservable<PriKey_production_brands_Recordset, PriKey_production_brands_Record>, IRecordsetBase
    {
        private IResultsetBase[] _resultsets;
        private string _sqlscript;
        private object[] _inputparametervalues;
        private InputParamHolder _inputparamholder;
        private VenturaSqlSchema _parameterschema;
        private int _rowlimit = 500;
        private const string CRLF = "\r\n";

        public PriKey_production_brands_Recordset()
        {
            _resultsets = new IResultsetBase[] { this };

            _sqlscript = "SELECT [brand_id],[brand_name]" + CRLF +
                         "FROM [production].[brands]" + CRLF +
                         "WHERE [brand_id] = @brand_id";

            _inputparametervalues = new object[1];
            _inputparamholder = new InputParamHolder(_inputparametervalues);

            ColumnArrayBuilder param_array = new ColumnArrayBuilder();

            param_array.AddParameterColumn("@brand_id", typeof(int), true, false, DbType.Int32, null, null, null);

            _parameterschema = new VenturaSqlSchema(param_array);

            ColumnArrayBuilder schema_array = new ColumnArrayBuilder();

            schema_array.Add(new VenturaSqlColumn("brand_id", typeof(int), false)
            {
                ColumnSize = 4,
                NumericPrecision = 10,
                ProviderType = 8,
                IsKey = true,
                IsIdentity = true,
                IsAutoIncrement = true,
                BaseSchemaName = "production",
                BaseTableName = "brands",
                BaseColumnName = "brand_id"
            });

            schema_array.Add(new VenturaSqlColumn("brand_name", typeof(string), false)
            {
                Updateable = true,
                ColumnSize = 255,
                ProviderType = 22,
                BaseSchemaName = "production",
                BaseTableName = "brands",
                BaseColumnName = "brand_name"
            });

            ((IResultsetBase)this).Schema = new VenturaSqlSchema(schema_array);
            ((IResultsetBase)this).UpdateableTablename = "[production].[brands]";
        }

        /// <summary>
        /// Database Column NotUpdateable. Table [production].[brands]. PrimaryKey. NotReadonly. NotNull. IsIdentity. AutoIncrement.
        /// </summary>
        public int brand_id
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.brand_id; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.brand_id = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [production].[brands]. NotReadonly. NotNull.
        /// </summary>
        public string brand_name
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.brand_name; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.brand_name = value; }
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
            this.InsertItem(index, new PriKey_production_brands_Record());
            this.CurrentRecordIndex = index;
        }

        public void Append(PriKey_production_brands_Record record)
        {
            int index = this.RecordCount;
            this.InsertItem(index, record);
            this.CurrentRecordIndex = index;
        }

        public PriKey_production_brands_Record NewRecord()
        {
            return new PriKey_production_brands_Record();
        }

        protected override PriKey_production_brands_Record InternalCreateExistingRecordObject(object[] columnvalues) => new PriKey_production_brands_Record(columnvalues);

        byte[] IRecordsetBase.Hash
        {
            get { return new byte[] { 211, 72, 10, 89, 72, 128, 63, 0, 243, 50, 62, 160, 147, 90, 242, 204 }; }
        }

        string IRecordsetBase.HashString
        {
            get { return "D3480A5948803F00F3323EA0935AF2CC"; }
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
            get { return "System.Data.SqlClient"; }
        }

        IResultsetBase[] IRecordsetBase.Resultsets
        {
            get { return _resultsets; }
        }

        string IRecordsetBase.SqlScript
        {
            get { return _sqlscript; }
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

        public void SetExecSqlParams(int? brand_id)
        {
            _inputparametervalues[0] = brand_id;
        }

        public void ExecSql(int? brand_id)
        {
            _inputparametervalues[0] = brand_id;
            Transactional.ExecSql(VenturaSqlConfig.DefaultConnector, new IRecordsetBase[] { this });
        }

        public void ExecSql(Connector connector, int? brand_id)
        {
            _inputparametervalues[0] = brand_id;
            Transactional.ExecSql(connector, new IRecordsetBase[] { this });
        }

        public async Task ExecSqlAsync(int? brand_id)
        {
            _inputparametervalues[0] = brand_id;
            await Transactional.ExecSqlAsync(VenturaSqlConfig.DefaultConnector, new IRecordsetBase[] { this });
        }

        public async Task ExecSqlAsync(Connector connector, int? brand_id)
        {
            _inputparametervalues[0] = brand_id;
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

            public int? brand_id
            {
                get { return (int?)_values[0]; }
                set { _values[0] = value; }
            }

        }

    }

    public sealed partial class PriKey_production_brands_Record : IRecordBase, INotifyPropertyChanged
    {
        private DataRecordStatus _recordstatus;
        private bool _started_with_dbvalues;

        private int _cur__brand_id; private int _ori__brand_id; private bool _mod__brand_id;
        private string _cur__brand_name; private string _ori__brand_name; private bool _mod__brand_name;


        public PriKey_production_brands_Record()
        {
            _cur__brand_id = 0;
            _cur__brand_name = "";
            _started_with_dbvalues = false;
            _recordstatus = DataRecordStatus.New;
        }

        public PriKey_production_brands_Record(object[] columnvalues)
        {
            _cur__brand_id = (int)columnvalues[0];
            _cur__brand_name = (string)columnvalues[1];
            _started_with_dbvalues = true;
            _recordstatus = DataRecordStatus.Existing;
        }

        /// <summary>
        /// Database Column NotUpdateable. Table [production].[brands]. PrimaryKey. NotReadonly. NotNull. IsIdentity. AutoIncrement.
        /// </summary>
        public int brand_id
        {
            get { return _cur__brand_id; }
            set
            {
                if (_started_with_dbvalues == false) _mod__brand_id = true;
                if (_cur__brand_id == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__brand_id == false) { _ori__brand_id = _cur__brand_id; _mod__brand_id = true; } // existing record and column is not modified
                    else { if (value == _ori__brand_id) { _ori__brand_id = default(int); _mod__brand_id = false; } } // existing record and column is modified
                }
                _cur__brand_id = value; OnPropertyChanged("brand_id"); OnAfterPropertyChanged("brand_id");
            }
        }

        /// <summary>
        /// Database Column Updateable. Table [production].[brands]. NotReadonly. NotNull.
        /// </summary>
        public string brand_name
        {
            get { return _cur__brand_name; }
            set
            {
                if (value == null) throw new ArgumentNullException("brand_name", VenturaSqlStrings.SET_NULL_MSG);
                if (_started_with_dbvalues == false) _mod__brand_name = true;
                if (_cur__brand_name == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__brand_name == false) { _ori__brand_name = _cur__brand_name; _mod__brand_name = true; } // existing record and column is not modified
                    else { if (value == _ori__brand_name) { _ori__brand_name = default(string); _mod__brand_name = false; } } // existing record and column is modified
                }
                _cur__brand_name = value; OnPropertyChanged("brand_name"); OnAfterPropertyChanged("brand_name");
            }
        }

        public bool IsModified(string column_name)
        {
            if (column_name == "brand_id") return _mod__brand_id;
            if (column_name == "brand_name") return _mod__brand_name;
            throw new ArgumentOutOfRangeException(String.Format(VenturaSqlStrings.UNKNOWN_COLUMN_NAME, column_name));
        }

        public int ModifiedColumnCount()
        {
            int count = 0;
            if (_mod__brand_id == true) count++;
            if (_mod__brand_name == true) count++;
            return count;
        }

        public bool PendingChanges()
        {
            if (_recordstatus == DataRecordStatus.New || _recordstatus == DataRecordStatus.ExistingDelete) return true;
            int count = 0;
            if (_mod__brand_name == true) count++;
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
                track_array.AppendDataValue(1, _cur__brand_name);
            }
            else if (_recordstatus == DataRecordStatus.Existing)
            {
                if (_mod__brand_name) track_array.AppendDataValue(1, _cur__brand_name);
                if (track_array.HasData == false) return;
            }

            if (_recordstatus == DataRecordStatus.Existing || _recordstatus == DataRecordStatus.ExistingDelete)
            {
                track_array.AppendPrikeyValue(0, (_mod__brand_id && _started_with_dbvalues) ? _ori__brand_id : _cur__brand_id);
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
                if (_mod__brand_id) _ori__brand_id = default(int);
                if (_mod__brand_name) _ori__brand_name = default(string);
            }
            _mod__brand_id = false;
            _mod__brand_name = false;
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
            _cur__brand_id = (int)value;
            OnPropertyChanged("brand_id");
            OnAfterPropertyChanged("brand_id");
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
namespace BlazorDemo.Server.VenturaAutoCreate
{
	public partial class PriKey_production_brands_Record
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
