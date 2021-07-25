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
    /// The updateable table is [production].[stocks]. Updateable table column information:
    /// • 3 out of 3 table columns are present in the resultset.
    /// • All primary key columns are present in the resultset: store_id and product_id.
    /// • Non-primary key column present in the resultset: quantity.
    /// Recordset item automatically created by VenturaSQL Studio.
    /// </summary>
    public partial class GetAll_production_stocks_Recordset : ResultsetObservable<GetAll_production_stocks_Recordset, GetAll_production_stocks_Record>, IRecordsetBase
    {
        private IResultsetBase[] _resultsets;
        private int _rowlimit = 500;
        private const string CRLF = "\r\n";

        public GetAll_production_stocks_Recordset()
        {
            _resultsets = new IResultsetBase[] { this };


            ColumnArrayBuilder schema_array = new ColumnArrayBuilder();

            schema_array.Add(new VenturaSqlColumn("store_id", typeof(int), false)
            {
                Updateable = true,
                ColumnSize = 4,
                NumericPrecision = 10,
                IsKey = true
            });

            schema_array.Add(new VenturaSqlColumn("product_id", typeof(int), false)
            {
                Updateable = true,
                ColumnSize = 4,
                NumericPrecision = 10,
                IsKey = true
            });

            schema_array.Add(new VenturaSqlColumn("quantity", typeof(int), true)
            {
                Updateable = true,
                ColumnSize = 4,
                NumericPrecision = 10
            });

            ((IResultsetBase)this).Schema = new VenturaSqlSchema(schema_array);
            ((IResultsetBase)this).UpdateableTablename = "[production].[stocks]";
        }

        /// <summary>
        /// Database Column Updateable. Table [production].[stocks]. PrimaryKey. NotReadonly. NotNull.
        /// </summary>
        public int store_id
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.store_id; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.store_id = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [production].[stocks]. PrimaryKey. NotReadonly. NotNull.
        /// </summary>
        public int product_id
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.product_id; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.product_id = value; }
        }

        /// <summary>
        /// Database Column Updateable. Table [production].[stocks]. NotReadonly. AllowNull.
        /// </summary>
        public int? quantity
        {
            get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.quantity; }
            set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.quantity = value; }
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
            this.InsertItem(index, new GetAll_production_stocks_Record());
            this.CurrentRecordIndex = index;
        }

        public void Append(GetAll_production_stocks_Record record)
        {
            int index = this.RecordCount;
            this.InsertItem(index, record);
            this.CurrentRecordIndex = index;
        }

        public GetAll_production_stocks_Record NewRecord()
        {
            return new GetAll_production_stocks_Record();
        }

        protected override GetAll_production_stocks_Record InternalCreateExistingRecordObject(object[] columnvalues) => new GetAll_production_stocks_Record(columnvalues);

        byte[] IRecordsetBase.Hash
        {
            get { return new byte[] { 127, 203, 148, 136, 96, 12, 248, 176, 117, 177, 213, 8, 240, 129, 77, 205 }; }
        }

        string IRecordsetBase.HashString
        {
            get { return "7FCB9488600CF8B075B1D508F0814DCD"; }
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

        public async Task ExecSqlAsync()
        {
            await Transactional.ExecSqlAsync(VenturaSqlConfig.DefaultConnector, new IRecordsetBase[] { this });
        }

        public async Task ExecSqlAsync(Connector connector)
        {
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

    }

    public sealed partial class GetAll_production_stocks_Record : IRecordBase, INotifyPropertyChanged
    {
        private DataRecordStatus _recordstatus;
        private bool _started_with_dbvalues;

        private int _cur__store_id; private int _ori__store_id; private bool _mod__store_id;
        private int _cur__product_id; private int _ori__product_id; private bool _mod__product_id;
        private int? _cur__quantity; private int? _ori__quantity; private bool _mod__quantity;


        public GetAll_production_stocks_Record()
        {
            _cur__store_id = 0;
            _cur__product_id = 0;
            _cur__quantity = null;
            _started_with_dbvalues = false;
            _recordstatus = DataRecordStatus.New;
        }

        public GetAll_production_stocks_Record(object[] columnvalues)
        {
            _cur__store_id = (int)columnvalues[0];
            _cur__product_id = (int)columnvalues[1];
            _cur__quantity = (int?)columnvalues[2];
            _started_with_dbvalues = true;
            _recordstatus = DataRecordStatus.Existing;
        }

        /// <summary>
        /// Database Column Updateable. Table [production].[stocks]. PrimaryKey. NotReadonly. NotNull.
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
        /// Database Column Updateable. Table [production].[stocks]. PrimaryKey. NotReadonly. NotNull.
        /// </summary>
        public int product_id
        {
            get { return _cur__product_id; }
            set
            {
                if (_started_with_dbvalues == false) _mod__product_id = true;
                if (_cur__product_id == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__product_id == false) { _ori__product_id = _cur__product_id; _mod__product_id = true; } // existing record and column is not modified
                    else { if (value == _ori__product_id) { _ori__product_id = default(int); _mod__product_id = false; } } // existing record and column is modified
                }
                _cur__product_id = value; OnPropertyChanged("product_id"); OnAfterPropertyChanged("product_id");
            }
        }

        /// <summary>
        /// Database Column Updateable. Table [production].[stocks]. NotReadonly. AllowNull.
        /// </summary>
        public int? quantity
        {
            get { return _cur__quantity; }
            set
            {
                if (_started_with_dbvalues == false) _mod__quantity = true;
                if (_cur__quantity == value) return;
                if (_started_with_dbvalues == true)
                {
                    if (_mod__quantity == false) { _ori__quantity = _cur__quantity; _mod__quantity = true; } // existing record and column is not modified
                    else { if (value == _ori__quantity) { _ori__quantity = default(int?); _mod__quantity = false; } } // existing record and column is modified
                }
                _cur__quantity = value; OnPropertyChanged("quantity"); OnAfterPropertyChanged("quantity");
            }
        }

        public bool IsModified(string column_name)
        {
            if (column_name == "store_id") return _mod__store_id;
            if (column_name == "product_id") return _mod__product_id;
            if (column_name == "quantity") return _mod__quantity;
            throw new ArgumentOutOfRangeException(String.Format(VenturaSqlStrings.UNKNOWN_COLUMN_NAME, column_name));
        }

        public int ModifiedColumnCount()
        {
            int count = 0;
            if (_mod__store_id == true) count++;
            if (_mod__product_id == true) count++;
            if (_mod__quantity == true) count++;
            return count;
        }

        public bool PendingChanges()
        {
            if (_recordstatus == DataRecordStatus.New || _recordstatus == DataRecordStatus.ExistingDelete) return true;
            int count = 0;
            if (_started_with_dbvalues)
            {
                if (_mod__store_id) count++;
                if (_mod__product_id) count++;
            }
            if (_mod__quantity == true) count++;
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
            if (_started_with_dbvalues) return;
            if (_recordstatus == DataRecordStatus.Existing) return;
            if (_mod__store_id == false) throw new Exception(string.Format(VenturaSqlStrings.VALUE_NOT_SET_MSG, record_index_to_display, "store_id"));
            if (_mod__product_id == false) throw new Exception(string.Format(VenturaSqlStrings.VALUE_NOT_SET_MSG, record_index_to_display, "product_id"));
        }

        void IRecordBase.WriteChangesToTrackArray(TrackArray track_array)
        {
            if (_recordstatus == DataRecordStatus.New)
            {
                track_array.AppendDataValue(0, _cur__store_id);
                track_array.AppendDataValue(1, _cur__product_id);
                if (_cur__quantity != null) track_array.AppendDataValue(2, _cur__quantity);
            }
            else if (_recordstatus == DataRecordStatus.Existing)
            {
                if (_started_with_dbvalues)
                {
                    if (_mod__store_id) track_array.AppendDataValue(0, _cur__store_id);
                    if (_mod__product_id) track_array.AppendDataValue(1, _cur__product_id);
                }
                if (_mod__quantity) track_array.AppendDataValue(2, _cur__quantity);
                if (track_array.HasData == false) return;
            }

            if (_recordstatus == DataRecordStatus.Existing || _recordstatus == DataRecordStatus.ExistingDelete)
            {
                track_array.AppendPrikeyValue(0, (_mod__store_id && _started_with_dbvalues) ? _ori__store_id : _cur__store_id);
                track_array.AppendPrikeyValue(1, (_mod__product_id && _started_with_dbvalues) ? _ori__product_id : _cur__product_id);
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
                if (_mod__product_id) _ori__product_id = default(int);
                if (_mod__quantity) _ori__quantity = default(int?);
            }
            _mod__store_id = false;
            _mod__product_id = false;
            _mod__quantity = false;
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
	public partial class GetAll_production_stocks_Record
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
