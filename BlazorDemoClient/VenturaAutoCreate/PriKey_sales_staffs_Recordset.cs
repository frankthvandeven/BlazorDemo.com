/*
	Project file: "C:\Active\Kenova\Projects\BikeStores.venproj"
	Target platform: NETStandard
	Generator version: 4.0.138
	Generated on: Tuesday, June 8, 2021 at 7:29:20 AM
	At the bottom of this file you find a template for extending Recordsets with calculated columns for XAML data binding.
*/
using VenturaSQL;
using System;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;

namespace BlazorDemo.Client.VenturaAutoCreate
{
	/// <summary>
	/// The updateable table is [sales].[staffs]. Updateable table column information:
	/// • 8 out of 8 table columns are present in the resultset.
	/// • All primary key columns are present in the resultset: staff_id.
	/// • Non-primary key columns present in the resultset: first_name, last_name, email, phone, active, store_id and manager_id.
	/// Recordset item automatically created by VenturaSQL Studio.
	/// </summary>
	public partial class PriKey_sales_staffs_Recordset : ResultsetObservable<PriKey_sales_staffs_Recordset, PriKey_sales_staffs_Record>, IRecordsetBase
	{
		private IResultsetBase[] _resultsets;
		private object[] _inputparametervalues;
		private InputParamHolder _inputparamholder;
		private VenturaSqlSchema _parameterschema;
		private int _rowlimit = 500;
		private const string CRLF = "\r\n";

		public PriKey_sales_staffs_Recordset()
		{
			_resultsets = new IResultsetBase[] { this };


			_inputparametervalues = new object[1];
			_inputparamholder = new InputParamHolder(_inputparametervalues);

			ColumnArrayBuilder param_array = new ColumnArrayBuilder();

			param_array.AddParameterColumn("@staff_id", typeof(int), true, false, DbType.Int32, null, null, null);

			_parameterschema = new VenturaSqlSchema(param_array);

			ColumnArrayBuilder schema_array = new ColumnArrayBuilder();

			schema_array.Add(new VenturaSqlColumn("staff_id", typeof(int), false)
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
				ColumnSize = 50
			});

			schema_array.Add(new VenturaSqlColumn("last_name", typeof(string), false)
			{
				Updateable = true,
				ColumnSize = 50
			});

			schema_array.Add(new VenturaSqlColumn("email", typeof(string), false)
			{
				Updateable = true,
				ColumnSize = 255
			});

			schema_array.Add(new VenturaSqlColumn("phone", typeof(string), true)
			{
				Updateable = true,
				ColumnSize = 25
			});

			schema_array.Add(new VenturaSqlColumn("active", typeof(byte), false)
			{
				Updateable = true,
				ColumnSize = 1,
				NumericPrecision = 3
			});

			schema_array.Add(new VenturaSqlColumn("store_id", typeof(int), false)
			{
				Updateable = true,
				ColumnSize = 4,
				NumericPrecision = 10
			});

			schema_array.Add(new VenturaSqlColumn("manager_id", typeof(int), true)
			{
				Updateable = true,
				ColumnSize = 4,
				NumericPrecision = 10
			});

			((IResultsetBase)this).Schema = new VenturaSqlSchema(schema_array);
			((IResultsetBase)this).UpdateableTablename = "[sales].[staffs]";
		}

		/// <summary>
		/// Database Column NotUpdateable. Table [sales].[staffs]. PrimaryKey. NotReadonly. NotNull. IsIdentity. AutoIncrement.
		/// </summary>
		public int staff_id
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.staff_id; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.staff_id = value; }
		}

		/// <summary>
		/// Database Column Updateable. Table [sales].[staffs]. NotReadonly. NotNull.
		/// </summary>
		public string first_name
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.first_name; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.first_name = value; }
		}

		/// <summary>
		/// Database Column Updateable. Table [sales].[staffs]. NotReadonly. NotNull.
		/// </summary>
		public string last_name
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.last_name; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.last_name = value; }
		}

		/// <summary>
		/// Database Column Updateable. Table [sales].[staffs]. NotReadonly. NotNull.
		/// </summary>
		public string email
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.email; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.email = value; }
		}

		/// <summary>
		/// Database Column Updateable. Table [sales].[staffs]. NotReadonly. AllowNull.
		/// </summary>
		public string phone
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.phone; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.phone = value; }
		}

		/// <summary>
		/// Database Column Updateable. Table [sales].[staffs]. NotReadonly. NotNull.
		/// </summary>
		public byte active
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.active; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.active = value; }
		}

		/// <summary>
		/// Database Column Updateable. Table [sales].[staffs]. NotReadonly. NotNull.
		/// </summary>
		public int store_id
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.store_id; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.store_id = value; }
		}

		/// <summary>
		/// Database Column Updateable. Table [sales].[staffs]. NotReadonly. AllowNull.
		/// </summary>
		public int? manager_id
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.manager_id; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.manager_id = value; }
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
			this.InsertItem(index, new PriKey_sales_staffs_Record());
			this.CurrentRecordIndex = index;
		}

		public void Append(PriKey_sales_staffs_Record record)
		{
			int index = this.RecordCount;
			this.InsertItem(index, record);
			this.CurrentRecordIndex = index;
		}

		public PriKey_sales_staffs_Record NewRecord()
		{
			return new PriKey_sales_staffs_Record();
		}

		protected override PriKey_sales_staffs_Record InternalCreateExistingRecordObject(object[] columnvalues) => new PriKey_sales_staffs_Record(columnvalues);

		byte[] IRecordsetBase.Hash
		{
			get { return new byte[] { 81, 0, 147, 65, 200, 182, 136, 92, 44, 5, 21, 121, 10, 226, 238, 190 }; }
		}

		string IRecordsetBase.HashString
		{
			get { return "51009341C8B6885C2C0515790AE2EEBE"; }
		}

		VenturaSqlPlatform IRecordsetBase.GeneratorTarget
		{
			get { return VenturaSqlPlatform.NETStandard; }
		}

		Version IRecordsetBase.GeneratorVersion
		{
			get { return new Version(4,0,138); }
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

		public void SetExecSqlParams(int? staff_id)
		{
			_inputparametervalues[0] = staff_id;
		}

		public void ExecSql(int? staff_id)
		{
			_inputparametervalues[0] = staff_id;
			Transactional.ExecSql(VenturaSqlConfig.DefaultConnector, new IRecordsetBase[] { this });
		}

		public void ExecSql(Connector connector, int? staff_id)
		{
			_inputparametervalues[0] = staff_id;
			Transactional.ExecSql(connector, new IRecordsetBase[] { this });
		}

		public async Task ExecSqlAsync(int? staff_id)
		{
			_inputparametervalues[0] = staff_id;
			await Transactional.ExecSqlAsync(VenturaSqlConfig.DefaultConnector, new IRecordsetBase[] { this });
		}

		public async Task ExecSqlAsync(Connector connector, int? staff_id)
		{
			_inputparametervalues[0] = staff_id;
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

			public int? staff_id
			{
				get { return (int?)_values[0]; }
				set { _values[0] = value; }
			}

		}

	}

	public sealed partial class PriKey_sales_staffs_Record : IRecordBase, INotifyPropertyChanged
	{
		private DataRecordStatus _recordstatus;
		private bool _started_with_dbvalues;

		private int _cur__staff_id; private int _ori__staff_id; private bool _mod__staff_id;
		private string _cur__first_name; private string _ori__first_name; private bool _mod__first_name;
		private string _cur__last_name; private string _ori__last_name; private bool _mod__last_name;
		private string _cur__email; private string _ori__email; private bool _mod__email;
		private string _cur__phone; private string _ori__phone; private bool _mod__phone;
		private byte _cur__active; private byte _ori__active; private bool _mod__active;
		private int _cur__store_id; private int _ori__store_id; private bool _mod__store_id;
		private int? _cur__manager_id; private int? _ori__manager_id; private bool _mod__manager_id;


		public PriKey_sales_staffs_Record()
		{
			_cur__staff_id = 0;
			_cur__first_name = "";
			_cur__last_name = "";
			_cur__email = "";
			_cur__phone = null;
			_cur__active = 0;
			_cur__store_id = 0;
			_cur__manager_id = null;
			_started_with_dbvalues = false;
			_recordstatus = DataRecordStatus.New;
		}

		public PriKey_sales_staffs_Record(object[] columnvalues)
		{
			_cur__staff_id = (int)columnvalues[0];
			_cur__first_name = (string)columnvalues[1];
			_cur__last_name = (string)columnvalues[2];
			_cur__email = (string)columnvalues[3];
			_cur__phone = (string)columnvalues[4];
			_cur__active = (byte)columnvalues[5];
			_cur__store_id = (int)columnvalues[6];
			_cur__manager_id = (int?)columnvalues[7];
			_started_with_dbvalues = true;
			_recordstatus = DataRecordStatus.Existing;
		}

		/// <summary>
		/// Database Column NotUpdateable. Table [sales].[staffs]. PrimaryKey. NotReadonly. NotNull. IsIdentity. AutoIncrement.
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
		/// Database Column Updateable. Table [sales].[staffs]. NotReadonly. NotNull.
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
		/// Database Column Updateable. Table [sales].[staffs]. NotReadonly. NotNull.
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
		/// Database Column Updateable. Table [sales].[staffs]. NotReadonly. NotNull.
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
		/// Database Column Updateable. Table [sales].[staffs]. NotReadonly. AllowNull.
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
		/// Database Column Updateable. Table [sales].[staffs]. NotReadonly. NotNull.
		/// </summary>
		public byte active
		{
			get { return _cur__active; }
			set
			{
				if (_started_with_dbvalues == false) _mod__active = true;
				if (_cur__active == value) return;
				if (_started_with_dbvalues == true)
				{
					if (_mod__active == false) { _ori__active = _cur__active; _mod__active = true; } // existing record and column is not modified
					else { if (value == _ori__active) { _ori__active = default(byte); _mod__active = false; } } // existing record and column is modified
				}
				_cur__active = value; OnPropertyChanged("active"); OnAfterPropertyChanged("active");
			}
		}

		/// <summary>
		/// Database Column Updateable. Table [sales].[staffs]. NotReadonly. NotNull.
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
		/// Database Column Updateable. Table [sales].[staffs]. NotReadonly. AllowNull.
		/// </summary>
		public int? manager_id
		{
			get { return _cur__manager_id; }
			set
			{
				if (_started_with_dbvalues == false) _mod__manager_id = true;
				if (_cur__manager_id == value) return;
				if (_started_with_dbvalues == true)
				{
					if (_mod__manager_id == false) { _ori__manager_id = _cur__manager_id; _mod__manager_id = true; } // existing record and column is not modified
					else { if (value == _ori__manager_id) { _ori__manager_id = default(int?); _mod__manager_id = false; } } // existing record and column is modified
				}
				_cur__manager_id = value; OnPropertyChanged("manager_id"); OnAfterPropertyChanged("manager_id");
			}
		}

		public bool IsModified(string column_name)
		{
			if (column_name == "staff_id") return _mod__staff_id;
			if (column_name == "first_name") return _mod__first_name;
			if (column_name == "last_name") return _mod__last_name;
			if (column_name == "email") return _mod__email;
			if (column_name == "phone") return _mod__phone;
			if (column_name == "active") return _mod__active;
			if (column_name == "store_id") return _mod__store_id;
			if (column_name == "manager_id") return _mod__manager_id;
			throw new ArgumentOutOfRangeException(String.Format(VenturaSqlStrings.UNKNOWN_COLUMN_NAME, column_name));
		}

		public int ModifiedColumnCount()
		{
			int count = 0;
			if (_mod__staff_id == true) count++;
			if (_mod__first_name == true) count++;
			if (_mod__last_name == true) count++;
			if (_mod__email == true) count++;
			if (_mod__phone == true) count++;
			if (_mod__active == true) count++;
			if (_mod__store_id == true) count++;
			if (_mod__manager_id == true) count++;
			return count;
		}

		public bool PendingChanges()
		{
			if (_recordstatus == DataRecordStatus.New || _recordstatus == DataRecordStatus.ExistingDelete) return true;
			int count = 0;
			if (_mod__first_name == true) count++;
			if (_mod__last_name == true) count++;
			if (_mod__email == true) count++;
			if (_mod__phone == true) count++;
			if (_mod__active == true) count++;
			if (_mod__store_id == true) count++;
			if (_mod__manager_id == true) count++;
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
				track_array.AppendDataValue(3, _cur__email);
				if (_cur__phone != null) track_array.AppendDataValue(4, _cur__phone);
				track_array.AppendDataValue(5, _cur__active);
				track_array.AppendDataValue(6, _cur__store_id);
				if (_cur__manager_id != null) track_array.AppendDataValue(7, _cur__manager_id);
			}
			else if (_recordstatus == DataRecordStatus.Existing)
			{
				if (_mod__first_name) track_array.AppendDataValue(1, _cur__first_name);
				if (_mod__last_name) track_array.AppendDataValue(2, _cur__last_name);
				if (_mod__email) track_array.AppendDataValue(3, _cur__email);
				if (_mod__phone) track_array.AppendDataValue(4, _cur__phone);
				if (_mod__active) track_array.AppendDataValue(5, _cur__active);
				if (_mod__store_id) track_array.AppendDataValue(6, _cur__store_id);
				if (_mod__manager_id) track_array.AppendDataValue(7, _cur__manager_id);
				if (track_array.HasData == false) return;
			}

			if (_recordstatus == DataRecordStatus.Existing || _recordstatus == DataRecordStatus.ExistingDelete)
			{
				track_array.AppendPrikeyValue(0, (_mod__staff_id && _started_with_dbvalues) ? _ori__staff_id : _cur__staff_id);
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
				if (_mod__staff_id) _ori__staff_id = default(int);
				if (_mod__first_name) _ori__first_name = default(string);
				if (_mod__last_name) _ori__last_name = default(string);
				if (_mod__email) _ori__email = default(string);
				if (_mod__phone) _ori__phone = default(string);
				if (_mod__active) _ori__active = default(byte);
				if (_mod__store_id) _ori__store_id = default(int);
				if (_mod__manager_id) _ori__manager_id = default(int?);
			}
			_mod__staff_id = false;
			_mod__first_name = false;
			_mod__last_name = false;
			_mod__email = false;
			_mod__phone = false;
			_mod__active = false;
			_mod__store_id = false;
			_mod__manager_id = false;
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
			_cur__staff_id = (int)value;
			OnPropertyChanged("staff_id");
			OnAfterPropertyChanged("staff_id");
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
	public partial class PriKey_sales_staffs_Record
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
