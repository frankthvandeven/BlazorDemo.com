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

namespace BlazorDemo.Client.VenturaRecordsets
{
	/// <summary>
	/// The resultset is read-only.
	/// </summary>
	public partial class SearchCustomersRecordset : ResultsetObservable<SearchCustomersRecordset, SearchCustomersRecord>, IRecordsetBase
	{
		private IResultsetBase[] _resultsets;
		private object[] _inputparametervalues;
		private InputParamHolder _inputparamholder;
		private VenturaSqlSchema _parameterschema;
		private int _rowlimit = 500;
		private const string CRLF = "\r\n";

		public SearchCustomersRecordset()
		{
			_resultsets = new IResultsetBase[] { this };


			_inputparametervalues = new object[2];
			_inputparamholder = new InputParamHolder(_inputparametervalues);

			ColumnArrayBuilder param_array = new ColumnArrayBuilder();

			param_array.AddParameterColumn("@SearchText", typeof(string), true, false, DbType.String, null, null, null);
			param_array.AddParameterColumn("@CustomerId", typeof(int), true, false, DbType.Int32, null, null, null);

			_parameterschema = new VenturaSqlSchema(param_array);

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
				ColumnSize = 255
			});

			schema_array.Add(new VenturaSqlColumn("last_name", typeof(string), false)
			{
				ColumnSize = 255
			});

			schema_array.Add(new VenturaSqlColumn("phone", typeof(string), true)
			{
				ColumnSize = 25
			});

			schema_array.Add(new VenturaSqlColumn("email", typeof(string), false)
			{
				ColumnSize = 255
			});

			schema_array.Add(new VenturaSqlColumn("street", typeof(string), true)
			{
				ColumnSize = 255
			});

			schema_array.Add(new VenturaSqlColumn("city", typeof(string), true)
			{
				ColumnSize = 50
			});

			schema_array.Add(new VenturaSqlColumn("state", typeof(string), true)
			{
				ColumnSize = 25
			});

			schema_array.Add(new VenturaSqlColumn("zip_code", typeof(string), true)
			{
				ColumnSize = 5
			});

			((IResultsetBase)this).Schema = new VenturaSqlSchema(schema_array);
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
		/// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. NotNull.
		/// </summary>
		public string first_name
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.first_name; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.first_name = value; }
		}

		/// <summary>
		/// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. NotNull.
		/// </summary>
		public string last_name
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.last_name; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.last_name = value; }
		}

		/// <summary>
		/// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. AllowNull.
		/// </summary>
		public string phone
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.phone; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.phone = value; }
		}

		/// <summary>
		/// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. NotNull.
		/// </summary>
		public string email
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.email; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.email = value; }
		}

		/// <summary>
		/// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. AllowNull.
		/// </summary>
		public string street
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.street; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.street = value; }
		}

		/// <summary>
		/// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. AllowNull.
		/// </summary>
		public string city
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.city; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.city = value; }
		}

		/// <summary>
		/// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. AllowNull.
		/// </summary>
		public string state
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.state; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.state = value; }
		}

		/// <summary>
		/// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. AllowNull.
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
			this.InsertItem(index, new SearchCustomersRecord());
			this.CurrentRecordIndex = index;
		}

		public void Append(SearchCustomersRecord record)
		{
			int index = this.RecordCount;
			this.InsertItem(index, record);
			this.CurrentRecordIndex = index;
		}

		public SearchCustomersRecord NewRecord()
		{
			return new SearchCustomersRecord();
		}

		protected override SearchCustomersRecord InternalCreateExistingRecordObject(object[] columnvalues) => new SearchCustomersRecord(columnvalues);

		byte[] IRecordsetBase.Hash
		{
			get { return new byte[] { 206, 58, 16, 70, 52, 97, 157, 52, 22, 167, 80, 184, 52, 163, 125, 177 }; }
		}

		string IRecordsetBase.HashString
		{
			get { return "CE3A104634619D3416A750B834A37DB1"; }
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

		public void SetExecSqlParams(string SearchText, int? CustomerId)
		{
			_inputparametervalues[0] = SearchText;
			_inputparametervalues[1] = CustomerId;
		}

		public void ExecSql(string SearchText, int? CustomerId)
		{
			_inputparametervalues[0] = SearchText;
			_inputparametervalues[1] = CustomerId;
			Transactional.ExecSql(VenturaSqlConfig.DefaultConnector, new IRecordsetBase[] { this });
		}

		public void ExecSql(Connector connector, string SearchText, int? CustomerId)
		{
			_inputparametervalues[0] = SearchText;
			_inputparametervalues[1] = CustomerId;
			Transactional.ExecSql(connector, new IRecordsetBase[] { this });
		}

		public async Task ExecSqlAsync(string SearchText, int? CustomerId)
		{
			_inputparametervalues[0] = SearchText;
			_inputparametervalues[1] = CustomerId;
			await Transactional.ExecSqlAsync(VenturaSqlConfig.DefaultConnector, new IRecordsetBase[] { this });
		}

		public async Task ExecSqlAsync(Connector connector, string SearchText, int? CustomerId)
		{
			_inputparametervalues[0] = SearchText;
			_inputparametervalues[1] = CustomerId;
			await Transactional.ExecSqlAsync(connector, new IRecordsetBase[] { this });
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

			public int? CustomerId
			{
				get { return (int?)_values[1]; }
				set { _values[1] = value; }
			}

		}

	}

	public sealed partial class SearchCustomersRecord : IRecordBase, INotifyPropertyChanged
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


		public SearchCustomersRecord()
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

		public SearchCustomersRecord(object[] columnvalues)
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
		/// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. NotNull.
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
		/// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. NotNull.
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
		/// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. AllowNull.
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
		/// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. NotNull.
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
		/// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. AllowNull.
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

		/// <summary>
		/// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. AllowNull.
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
		/// Database Column NotUpdateable. Table [sales].[customers]. NotReadonly. AllowNull.
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
			return false;
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
namespace BlazorDemo.Client.VenturaRecordsets
{
	public partial class SearchCustomersRecord
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
