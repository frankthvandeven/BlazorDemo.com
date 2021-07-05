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
	/// The updateable table is [production].[products]. Updateable table column information:
	/// • 6 out of 6 table columns are present in the resultset.
	/// • All primary key columns are present in the resultset: product_id.
	/// • Non-primary key columns present in the resultset: product_name, brand_id, category_id, model_year and list_price.
	/// Recordset item automatically created by VenturaSQL Studio.
	/// </summary>
	public partial class Incr_production_products_Recordset : ResultsetObservable<Incr_production_products_Recordset, Incr_production_products_Record>, IRecordsetBase, IRecordsetIncremental
	{
		private bool _has_more_rows = false;
		private Connector _incremental_connector = null;
		private int _incremental_offset = 0;
		private int _last_exec_startindex = -1;
		private int _last_exec_count = 0;
		private IResultsetBase[] _resultsets;
		private int _rowlimit = 500;
		private const string CRLF = "\r\n";

		public Incr_production_products_Recordset()
		{
			_resultsets = new IResultsetBase[] { this };


			ColumnArrayBuilder schema_array = new ColumnArrayBuilder();

			schema_array.Add(new VenturaSqlColumn("product_id", typeof(int), false)
			{
				ColumnSize = 4,
				NumericPrecision = 10,
				IsKey = true,
				IsIdentity = true,
				IsAutoIncrement = true
			});

			schema_array.Add(new VenturaSqlColumn("product_name", typeof(string), false)
			{
				Updateable = true,
				ColumnSize = 255
			});

			schema_array.Add(new VenturaSqlColumn("brand_id", typeof(int), false)
			{
				Updateable = true,
				ColumnSize = 4,
				NumericPrecision = 10
			});

			schema_array.Add(new VenturaSqlColumn("category_id", typeof(int), false)
			{
				Updateable = true,
				ColumnSize = 4,
				NumericPrecision = 10
			});

			schema_array.Add(new VenturaSqlColumn("model_year", typeof(short), false)
			{
				Updateable = true,
				ColumnSize = 2,
				NumericPrecision = 5
			});

			schema_array.Add(new VenturaSqlColumn("list_price", typeof(decimal), false)
			{
				Updateable = true,
				ColumnSize = 17,
				NumericPrecision = 10,
				NumericScale = 2
			});

			((IResultsetBase)this).Schema = new VenturaSqlSchema(schema_array);
			((IResultsetBase)this).UpdateableTablename = "[production].[products]";
		}

		/// <summary>
		/// Database Column NotUpdateable. Table [production].[products]. PrimaryKey. NotReadonly. NotNull. IsIdentity. AutoIncrement.
		/// </summary>
		public int product_id
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.product_id; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.product_id = value; }
		}

		/// <summary>
		/// Database Column Updateable. Table [production].[products]. NotReadonly. NotNull.
		/// </summary>
		public string product_name
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.product_name; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.product_name = value; }
		}

		/// <summary>
		/// Database Column Updateable. Table [production].[products]. NotReadonly. NotNull.
		/// </summary>
		public int brand_id
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.brand_id; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.brand_id = value; }
		}

		/// <summary>
		/// Database Column Updateable. Table [production].[products]. NotReadonly. NotNull.
		/// </summary>
		public int category_id
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.category_id; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.category_id = value; }
		}

		/// <summary>
		/// Database Column Updateable. Table [production].[products]. NotReadonly. NotNull.
		/// </summary>
		public short model_year
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.model_year; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.model_year = value; }
		}

		/// <summary>
		/// Database Column Updateable. Table [production].[products]. NotReadonly. NotNull.
		/// </summary>
		public decimal list_price
		{
			get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.list_price; }
			set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.list_price = value; }
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
			this.InsertItem(index, new Incr_production_products_Record());
			this.CurrentRecordIndex = index;
		}

		public void Append(Incr_production_products_Record record)
		{
			int index = this.RecordCount;
			this.InsertItem(index, record);
			this.CurrentRecordIndex = index;
		}

		public Incr_production_products_Record NewRecord()
		{
			return new Incr_production_products_Record();
		}

		protected override Incr_production_products_Record InternalCreateExistingRecordObject(object[] columnvalues) => new Incr_production_products_Record(columnvalues);

		byte[] IRecordsetBase.Hash
		{
			get { return new byte[] { 87, 169, 166, 101, 155, 72, 241, 102, 190, 198, 146, 184, 15, 254, 60, 200 }; }
		}

		string IRecordsetBase.HashString
		{
			get { return "57A9A6659B48F166BEC692B80FFE3CC8"; }
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

	public sealed partial class Incr_production_products_Record : IRecordBase, INotifyPropertyChanged
	{
		private DataRecordStatus _recordstatus;
		private bool _started_with_dbvalues;

		private int _cur__product_id; private int _ori__product_id; private bool _mod__product_id;
		private string _cur__product_name; private string _ori__product_name; private bool _mod__product_name;
		private int _cur__brand_id; private int _ori__brand_id; private bool _mod__brand_id;
		private int _cur__category_id; private int _ori__category_id; private bool _mod__category_id;
		private short _cur__model_year; private short _ori__model_year; private bool _mod__model_year;
		private decimal _cur__list_price; private decimal _ori__list_price; private bool _mod__list_price;


		public Incr_production_products_Record()
		{
			_cur__product_id = 0;
			_cur__product_name = "";
			_cur__brand_id = 0;
			_cur__category_id = 0;
			_cur__model_year = 0;
			_cur__list_price = 0.0m;
			_started_with_dbvalues = false;
			_recordstatus = DataRecordStatus.New;
		}

		public Incr_production_products_Record(object[] columnvalues)
		{
			_cur__product_id = (int)columnvalues[0];
			_cur__product_name = (string)columnvalues[1];
			_cur__brand_id = (int)columnvalues[2];
			_cur__category_id = (int)columnvalues[3];
			_cur__model_year = (short)columnvalues[4];
			_cur__list_price = (decimal)columnvalues[5];
			_started_with_dbvalues = true;
			_recordstatus = DataRecordStatus.Existing;
		}

		/// <summary>
		/// Database Column NotUpdateable. Table [production].[products]. PrimaryKey. NotReadonly. NotNull. IsIdentity. AutoIncrement.
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
		/// Database Column Updateable. Table [production].[products]. NotReadonly. NotNull.
		/// </summary>
		public string product_name
		{
			get { return _cur__product_name; }
			set
			{
				if (value == null) throw new ArgumentNullException("product_name", VenturaSqlStrings.SET_NULL_MSG);
				if (_started_with_dbvalues == false) _mod__product_name = true;
				if (_cur__product_name == value) return;
				if (_started_with_dbvalues == true)
				{
					if (_mod__product_name == false) { _ori__product_name = _cur__product_name; _mod__product_name = true; } // existing record and column is not modified
					else { if (value == _ori__product_name) { _ori__product_name = default(string); _mod__product_name = false; } } // existing record and column is modified
				}
				_cur__product_name = value; OnPropertyChanged("product_name"); OnAfterPropertyChanged("product_name");
			}
		}

		/// <summary>
		/// Database Column Updateable. Table [production].[products]. NotReadonly. NotNull.
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
		/// Database Column Updateable. Table [production].[products]. NotReadonly. NotNull.
		/// </summary>
		public int category_id
		{
			get { return _cur__category_id; }
			set
			{
				if (_started_with_dbvalues == false) _mod__category_id = true;
				if (_cur__category_id == value) return;
				if (_started_with_dbvalues == true)
				{
					if (_mod__category_id == false) { _ori__category_id = _cur__category_id; _mod__category_id = true; } // existing record and column is not modified
					else { if (value == _ori__category_id) { _ori__category_id = default(int); _mod__category_id = false; } } // existing record and column is modified
				}
				_cur__category_id = value; OnPropertyChanged("category_id"); OnAfterPropertyChanged("category_id");
			}
		}

		/// <summary>
		/// Database Column Updateable. Table [production].[products]. NotReadonly. NotNull.
		/// </summary>
		public short model_year
		{
			get { return _cur__model_year; }
			set
			{
				if (_started_with_dbvalues == false) _mod__model_year = true;
				if (_cur__model_year == value) return;
				if (_started_with_dbvalues == true)
				{
					if (_mod__model_year == false) { _ori__model_year = _cur__model_year; _mod__model_year = true; } // existing record and column is not modified
					else { if (value == _ori__model_year) { _ori__model_year = default(short); _mod__model_year = false; } } // existing record and column is modified
				}
				_cur__model_year = value; OnPropertyChanged("model_year"); OnAfterPropertyChanged("model_year");
			}
		}

		/// <summary>
		/// Database Column Updateable. Table [production].[products]. NotReadonly. NotNull.
		/// </summary>
		public decimal list_price
		{
			get { return _cur__list_price; }
			set
			{
				if (_started_with_dbvalues == false) _mod__list_price = true;
				if (_cur__list_price == value) return;
				if (_started_with_dbvalues == true)
				{
					if (_mod__list_price == false) { _ori__list_price = _cur__list_price; _mod__list_price = true; } // existing record and column is not modified
					else { if (value == _ori__list_price) { _ori__list_price = default(decimal); _mod__list_price = false; } } // existing record and column is modified
				}
				_cur__list_price = value; OnPropertyChanged("list_price"); OnAfterPropertyChanged("list_price");
			}
		}

		public bool IsModified(string column_name)
		{
			if (column_name == "product_id") return _mod__product_id;
			if (column_name == "product_name") return _mod__product_name;
			if (column_name == "brand_id") return _mod__brand_id;
			if (column_name == "category_id") return _mod__category_id;
			if (column_name == "model_year") return _mod__model_year;
			if (column_name == "list_price") return _mod__list_price;
			throw new ArgumentOutOfRangeException(String.Format(VenturaSqlStrings.UNKNOWN_COLUMN_NAME, column_name));
		}

		public int ModifiedColumnCount()
		{
			int count = 0;
			if (_mod__product_id == true) count++;
			if (_mod__product_name == true) count++;
			if (_mod__brand_id == true) count++;
			if (_mod__category_id == true) count++;
			if (_mod__model_year == true) count++;
			if (_mod__list_price == true) count++;
			return count;
		}

		public bool PendingChanges()
		{
			if (_recordstatus == DataRecordStatus.New || _recordstatus == DataRecordStatus.ExistingDelete) return true;
			int count = 0;
			if (_mod__product_name == true) count++;
			if (_mod__brand_id == true) count++;
			if (_mod__category_id == true) count++;
			if (_mod__model_year == true) count++;
			if (_mod__list_price == true) count++;
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
				track_array.AppendDataValue(1, _cur__product_name);
				track_array.AppendDataValue(2, _cur__brand_id);
				track_array.AppendDataValue(3, _cur__category_id);
				track_array.AppendDataValue(4, _cur__model_year);
				track_array.AppendDataValue(5, _cur__list_price);
			}
			else if (_recordstatus == DataRecordStatus.Existing)
			{
				if (_mod__product_name) track_array.AppendDataValue(1, _cur__product_name);
				if (_mod__brand_id) track_array.AppendDataValue(2, _cur__brand_id);
				if (_mod__category_id) track_array.AppendDataValue(3, _cur__category_id);
				if (_mod__model_year) track_array.AppendDataValue(4, _cur__model_year);
				if (_mod__list_price) track_array.AppendDataValue(5, _cur__list_price);
				if (track_array.HasData == false) return;
			}

			if (_recordstatus == DataRecordStatus.Existing || _recordstatus == DataRecordStatus.ExistingDelete)
			{
				track_array.AppendPrikeyValue(0, (_mod__product_id && _started_with_dbvalues) ? _ori__product_id : _cur__product_id);
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
				if (_mod__product_id) _ori__product_id = default(int);
				if (_mod__product_name) _ori__product_name = default(string);
				if (_mod__brand_id) _ori__brand_id = default(int);
				if (_mod__category_id) _ori__category_id = default(int);
				if (_mod__model_year) _ori__model_year = default(short);
				if (_mod__list_price) _ori__list_price = default(decimal);
			}
			_mod__product_id = false;
			_mod__product_name = false;
			_mod__brand_id = false;
			_mod__category_id = false;
			_mod__model_year = false;
			_mod__list_price = false;
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
			_cur__product_id = (int)value;
			OnPropertyChanged("product_id");
			OnAfterPropertyChanged("product_id");
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
	public partial class Incr_production_products_Record
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
