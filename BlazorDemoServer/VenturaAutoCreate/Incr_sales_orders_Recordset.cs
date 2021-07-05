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

namespace BlazorDemo.Server.VenturaAutoCreate
{
	/// <summary>
	/// The updateable table is [sales].[orders]. Updateable table column information:
	/// • 8 out of 8 table columns are present in the resultset.
	/// • All primary key columns are present in the resultset: order_id.
	/// • Non-primary key columns present in the resultset: customer_id, order_status, order_date, required_date, shipped_date,
	///   store_id and staff_id.
	/// Recordset item automatically created by VenturaSQL Studio.
	/// </summary>
	public partial class Incr_sales_orders_Recordset : ResultsetObservable<Incr_sales_orders_Recordset, Incr_sales_orders_Record>, IRecordsetBase, IRecordsetIncremental
	{
		private bool _has_more_rows = false;
		private Connector _incremental_connector = null;
		private int _incremental_offset = 0;
		private int _last_exec_startindex = -1;
		private int _last_exec_count = 0;
		private IResultsetBase[] _resultsets;
		private string _sqlscript;
		private int _rowlimit = 500;
		private const string CRLF = "\r\n";

		public Incr_sales_orders_Recordset()
		{
			_resultsets = new IResultsetBase[] { this };

			_sqlscript = "SELECT [order_id],[customer_id],[order_status],[order_date],[required_date],[shipped_date],[store_id],[staff_id]" + CRLF +
			             "FROM [sales].[orders]" + CRLF +
			             "ORDER BY [order_id]" + CRLF +
			             "OFFSET @RowOffset ROWS FETCH NEXT @RowLimit ROWS ONLY";

			ColumnArrayBuilder schema_array = new ColumnArrayBuilder();

			schema_array.Add(new VenturaSqlColumn("order_id", typeof(int), false)
			{
				ColumnSize = 4,
				NumericPrecision = 10,
				ProviderType = 8,
				IsKey = true,
				IsIdentity = true,
				IsAutoIncrement = true,
				BaseSchemaName = "sales",
				BaseTableName = "orders",
				BaseColumnName = "order_id"
			});

			schema_array.Add(new VenturaSqlColumn("customer_id", typeof(int), true)
			{
				Updateable = true,
				ColumnSize = 4,
				NumericPrecision = 10,
				ProviderType = 8,
				BaseSchemaName = "sales",
				BaseTableName = "orders",
				BaseColumnName = "customer_id"
			});

			schema_array.Add(new VenturaSqlColumn("order_status", typeof(byte), false)
			{
				Updateable = true,
				ColumnSize = 1,
				NumericPrecision = 3,
				ProviderType = 20,
				BaseSchemaName = "sales",
				BaseTableName = "orders",
				BaseColumnName = "order_status"
			});

			schema_array.Add(new VenturaSqlColumn("order_date", typeof(DateTime), false)
			{
				Updateable = true,
				ColumnSize = 3,
				ProviderType = 31,
				BaseSchemaName = "sales",
				BaseTableName = "orders",
				BaseColumnName = "order_date"
			});

			schema_array.Add(new VenturaSqlColumn("required_date", typeof(DateTime), false)
			{
				Updateable = true,
				ColumnSize = 3,
				ProviderType = 31,
				BaseSchemaName = "sales",
				BaseTableName = "orders",
				BaseColumnName = "required_date"
			});

			schema_array.Add(new VenturaSqlColumn("shipped_date", typeof(DateTime), true)
			{
				Updateable = true,
				ColumnSize = 3,
				ProviderType = 31,
				BaseSchemaName = "sales",
				BaseTableName = "orders",
				BaseColumnName = "shipped_date"
			});

			schema_array.Add(new VenturaSqlColumn("store_id", typeof(int), false)
			{
				Updateable = true,
				ColumnSize = 4,
				NumericPrecision = 10,
				ProviderType = 8,
				BaseSchemaName = "sales",
				BaseTableName = "orders",
				BaseColumnName = "store_id"
			});

			schema_array.Add(new VenturaSqlColumn("staff_id", typeof(int), false)
			{
				Updateable = true,
				ColumnSize = 4,
				NumericPrecision = 10,
				ProviderType = 8,
				BaseSchemaName = "sales",
				BaseTableName = "orders",
				BaseColumnName = "staff_id"
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
			this.InsertItem(index, new Incr_sales_orders_Record());
			this.CurrentRecordIndex = index;
		}

		public void Append(Incr_sales_orders_Record record)
		{
			int index = this.RecordCount;
			this.InsertItem(index, record);
			this.CurrentRecordIndex = index;
		}

		public Incr_sales_orders_Record NewRecord()
		{
			return new Incr_sales_orders_Record();
		}

		protected override Incr_sales_orders_Record InternalCreateExistingRecordObject(object[] columnvalues) => new Incr_sales_orders_Record(columnvalues);

		byte[] IRecordsetBase.Hash
		{
			get { return new byte[] { 121, 59, 90, 37, 69, 63, 109, 126, 251, 159, 134, 63, 173, 3, 161, 233 }; }
		}

		string IRecordsetBase.HashString
		{
			get { return "793B5A25453F6D7EFB9F863FAD03A1E9"; }
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
			get { return "System.Data.SqlClient"; }
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
			get { return _sqlscript; }
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

	public sealed partial class Incr_sales_orders_Record : IRecordBase, INotifyPropertyChanged
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


		public Incr_sales_orders_Record()
		{
			_cur__order_id = 0;
			_cur__customer_id = null;
			_cur__order_status = 0;
			_cur__order_date = new DateTime(1900, 1, 1);
			_cur__required_date = new DateTime(1900, 1, 1);
			_cur__shipped_date = null;
			_cur__store_id = 0;
			_cur__staff_id = 0;
			_started_with_dbvalues = false;
			_recordstatus = DataRecordStatus.New;
		}

		public Incr_sales_orders_Record(object[] columnvalues)
		{
			_cur__order_id = (int)columnvalues[0];
			_cur__customer_id = (int?)columnvalues[1];
			_cur__order_status = (byte)columnvalues[2];
			_cur__order_date = (DateTime)columnvalues[3];
			_cur__required_date = (DateTime)columnvalues[4];
			_cur__shipped_date = (DateTime?)columnvalues[5];
			_cur__store_id = (int)columnvalues[6];
			_cur__staff_id = (int)columnvalues[7];
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
			}
			_mod__order_id = false;
			_mod__customer_id = false;
			_mod__order_status = false;
			_mod__order_date = false;
			_mod__required_date = false;
			_mod__shipped_date = false;
			_mod__store_id = false;
			_mod__staff_id = false;
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
namespace BlazorDemo.Server.VenturaAutoCreate
{
	public partial class Incr_sales_orders_Record
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
