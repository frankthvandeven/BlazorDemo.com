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
    /// This recordset contains 2 resultsets.
    /// Recordset item automatically created by VenturaSQL Studio.
    /// </summary>
    public partial class Updates2Recordset : IRecordsetBase
    {
        private IResultsetBase[] _resultsets;
        private object[] _inputparametervalues;
        private InputParamHolder _inputparamholder;
        private VenturaSqlSchema _parameterschema;
        private int _rowlimit = 500;
        private const string CRLF = "\r\n";

        private MultiResultsetOrders _resultset1;
        private MultiResultsetOrderItems _resultset2;

        public Updates2Recordset()
        {
            _resultset1 = new MultiResultsetOrders();
            _resultset2 = new MultiResultsetOrderItems();

            _resultsets = new IResultsetBase[] { _resultset1, _resultset2 };


            _inputparametervalues = new object[2];
            _inputparamholder = new InputParamHolder(_inputparametervalues);

            ColumnArrayBuilder param_array = new ColumnArrayBuilder();

            param_array.AddParameterColumn("@SearchText", typeof(string), true, false, DbType.String, null, null, null);
            param_array.AddParameterColumn("@order_id", typeof(int), true, false, DbType.Int32, null, null, null);

            _parameterschema = new VenturaSqlSchema(param_array);

        }

        byte[] IRecordsetBase.Hash
        {
            get { return new byte[] { 175, 238, 94, 26, 212, 210, 174, 49, 114, 26, 205, 187, 38, 93, 155, 253 }; }
        }

        string IRecordsetBase.HashString
        {
            get { return "AFEE5E1AD4D2AE31721ACDBB265D9BFD"; }
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

        public MultiResultsetOrders ResultsetOrders
        {
            get { return _resultset1; }
        }

        public MultiResultsetOrderItems ResultsetOrderItems
        {
            get { return _resultset2; }
        }

        /// <summary>
        /// The updateable table is [sales].[orders]. Updateable table column information:
        /// • 8 out of 8 table columns are present in the resultset.
        /// • All primary key columns are present in the resultset: order_id.
        /// • Non-primary key columns present in the resultset: customer_id, order_status, order_date, required_date, shipped_date,
        ///   store_id and staff_id.
        /// </summary>
        public partial class MultiResultsetOrders : ResultsetObservable<MultiResultsetOrders, MultiResultsetOrdersRecord>
        {
            public MultiResultsetOrders()
            {
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
                this.InsertItem(index, new MultiResultsetOrdersRecord());
                this.CurrentRecordIndex = index;
            }

            public void Append(MultiResultsetOrdersRecord record)
            {
                int index = this.RecordCount;
                this.InsertItem(index, record);
                this.CurrentRecordIndex = index;
            }

            public MultiResultsetOrdersRecord NewRecord()
            {
                return new MultiResultsetOrdersRecord();
            }

            protected override MultiResultsetOrdersRecord InternalCreateExistingRecordObject(object[] columnvalues) => new MultiResultsetOrdersRecord(columnvalues);

        }

        public sealed partial class MultiResultsetOrdersRecord : IRecordBase, INotifyPropertyChanged
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


            public MultiResultsetOrdersRecord()
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

            public MultiResultsetOrdersRecord(object[] columnvalues)
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
        /// <summary>
        /// The updateable table is [sales].[order_items]. Updateable table column information:
        /// • 6 out of 6 table columns are present in the resultset.
        /// • All primary key columns are present in the resultset: order_id and item_id.
        /// • Non-primary key columns present in the resultset: product_id, quantity, list_price and discount.
        /// </summary>
        public partial class MultiResultsetOrderItems : ResultsetObservable<MultiResultsetOrderItems, MultiResultsetOrderItemsRecord>
        {
            public MultiResultsetOrderItems()
            {
                ColumnArrayBuilder schema_array = new ColumnArrayBuilder();

                schema_array.Add(new VenturaSqlColumn("order_id", typeof(int), false)
                {
                    Updateable = true,
                    ColumnSize = 4,
                    NumericPrecision = 10,
                    IsKey = true
                });

                schema_array.Add(new VenturaSqlColumn("item_id", typeof(int), false)
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
                    NumericPrecision = 10
                });

                schema_array.Add(new VenturaSqlColumn("quantity", typeof(int), false)
                {
                    Updateable = true,
                    ColumnSize = 4,
                    NumericPrecision = 10
                });

                schema_array.Add(new VenturaSqlColumn("list_price", typeof(decimal), false)
                {
                    Updateable = true,
                    ColumnSize = 17,
                    NumericPrecision = 10,
                    NumericScale = 2
                });

                schema_array.Add(new VenturaSqlColumn("discount", typeof(decimal), false)
                {
                    Updateable = true,
                    ColumnSize = 17,
                    NumericPrecision = 4,
                    NumericScale = 2
                });

                ((IResultsetBase)this).Schema = new VenturaSqlSchema(schema_array);
                ((IResultsetBase)this).UpdateableTablename = "[sales].[order_items]";
            }

            /// <summary>
            /// Database Column Updateable. Table [sales].[order_items]. PrimaryKey. NotReadonly. NotNull.
            /// </summary>
            public int order_id
            {
                get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.order_id; }
                set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.order_id = value; }
            }

            /// <summary>
            /// Database Column Updateable. Table [sales].[order_items]. PrimaryKey. NotReadonly. NotNull.
            /// </summary>
            public int item_id
            {
                get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.item_id; }
                set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.item_id = value; }
            }

            /// <summary>
            /// Database Column Updateable. Table [sales].[order_items]. NotReadonly. NotNull.
            /// </summary>
            public int product_id
            {
                get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.product_id; }
                set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.product_id = value; }
            }

            /// <summary>
            /// Database Column Updateable. Table [sales].[order_items]. NotReadonly. NotNull.
            /// </summary>
            public int quantity
            {
                get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.quantity; }
                set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.quantity = value; }
            }

            /// <summary>
            /// Database Column Updateable. Table [sales].[order_items]. NotReadonly. NotNull.
            /// </summary>
            public decimal list_price
            {
                get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.list_price; }
                set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.list_price = value; }
            }

            /// <summary>
            /// Database Column Updateable. Table [sales].[order_items]. NotReadonly. NotNull.
            /// </summary>
            public decimal discount
            {
                get { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); return CurrentRecord.discount; }
                set { if (CurrentRecord == null) throw new InvalidOperationException(VenturaSqlStrings.CURRENT_RECORD_NOT_SET); CurrentRecord.discount = value; }
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
                this.InsertItem(index, new MultiResultsetOrderItemsRecord());
                this.CurrentRecordIndex = index;
            }

            public void Append(MultiResultsetOrderItemsRecord record)
            {
                int index = this.RecordCount;
                this.InsertItem(index, record);
                this.CurrentRecordIndex = index;
            }

            public MultiResultsetOrderItemsRecord NewRecord()
            {
                return new MultiResultsetOrderItemsRecord();
            }

            protected override MultiResultsetOrderItemsRecord InternalCreateExistingRecordObject(object[] columnvalues) => new MultiResultsetOrderItemsRecord(columnvalues);

        }

        public sealed partial class MultiResultsetOrderItemsRecord : IRecordBase, INotifyPropertyChanged
        {
            private DataRecordStatus _recordstatus;
            private bool _started_with_dbvalues;

            private int _cur__order_id; private int _ori__order_id; private bool _mod__order_id;
            private int _cur__item_id; private int _ori__item_id; private bool _mod__item_id;
            private int _cur__product_id; private int _ori__product_id; private bool _mod__product_id;
            private int _cur__quantity; private int _ori__quantity; private bool _mod__quantity;
            private decimal _cur__list_price; private decimal _ori__list_price; private bool _mod__list_price;
            private decimal _cur__discount; private decimal _ori__discount; private bool _mod__discount;


            public MultiResultsetOrderItemsRecord()
            {
                _cur__order_id = 0;
                _cur__item_id = 0;
                _cur__product_id = 0;
                _cur__quantity = 0;
                _cur__list_price = 0.0m;
                _cur__discount = 0.0m;
                _started_with_dbvalues = false;
                _recordstatus = DataRecordStatus.New;
            }

            public MultiResultsetOrderItemsRecord(object[] columnvalues)
            {
                _cur__order_id = (int)columnvalues[0];
                _cur__item_id = (int)columnvalues[1];
                _cur__product_id = (int)columnvalues[2];
                _cur__quantity = (int)columnvalues[3];
                _cur__list_price = (decimal)columnvalues[4];
                _cur__discount = (decimal)columnvalues[5];
                _started_with_dbvalues = true;
                _recordstatus = DataRecordStatus.Existing;
            }

            /// <summary>
            /// Database Column Updateable. Table [sales].[order_items]. PrimaryKey. NotReadonly. NotNull.
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
            /// Database Column Updateable. Table [sales].[order_items]. PrimaryKey. NotReadonly. NotNull.
            /// </summary>
            public int item_id
            {
                get { return _cur__item_id; }
                set
                {
                    if (_started_with_dbvalues == false) _mod__item_id = true;
                    if (_cur__item_id == value) return;
                    if (_started_with_dbvalues == true)
                    {
                        if (_mod__item_id == false) { _ori__item_id = _cur__item_id; _mod__item_id = true; } // existing record and column is not modified
                        else { if (value == _ori__item_id) { _ori__item_id = default(int); _mod__item_id = false; } } // existing record and column is modified
                    }
                    _cur__item_id = value; OnPropertyChanged("item_id"); OnAfterPropertyChanged("item_id");
                }
            }

            /// <summary>
            /// Database Column Updateable. Table [sales].[order_items]. NotReadonly. NotNull.
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
            /// Database Column Updateable. Table [sales].[order_items]. NotReadonly. NotNull.
            /// </summary>
            public int quantity
            {
                get { return _cur__quantity; }
                set
                {
                    if (_started_with_dbvalues == false) _mod__quantity = true;
                    if (_cur__quantity == value) return;
                    if (_started_with_dbvalues == true)
                    {
                        if (_mod__quantity == false) { _ori__quantity = _cur__quantity; _mod__quantity = true; } // existing record and column is not modified
                        else { if (value == _ori__quantity) { _ori__quantity = default(int); _mod__quantity = false; } } // existing record and column is modified
                    }
                    _cur__quantity = value; OnPropertyChanged("quantity"); OnAfterPropertyChanged("quantity");
                }
            }

            /// <summary>
            /// Database Column Updateable. Table [sales].[order_items]. NotReadonly. NotNull.
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

            /// <summary>
            /// Database Column Updateable. Table [sales].[order_items]. NotReadonly. NotNull.
            /// </summary>
            public decimal discount
            {
                get { return _cur__discount; }
                set
                {
                    if (_started_with_dbvalues == false) _mod__discount = true;
                    if (_cur__discount == value) return;
                    if (_started_with_dbvalues == true)
                    {
                        if (_mod__discount == false) { _ori__discount = _cur__discount; _mod__discount = true; } // existing record and column is not modified
                        else { if (value == _ori__discount) { _ori__discount = default(decimal); _mod__discount = false; } } // existing record and column is modified
                    }
                    _cur__discount = value; OnPropertyChanged("discount"); OnAfterPropertyChanged("discount");
                }
            }

            public bool IsModified(string column_name)
            {
                if (column_name == "order_id") return _mod__order_id;
                if (column_name == "item_id") return _mod__item_id;
                if (column_name == "product_id") return _mod__product_id;
                if (column_name == "quantity") return _mod__quantity;
                if (column_name == "list_price") return _mod__list_price;
                if (column_name == "discount") return _mod__discount;
                throw new ArgumentOutOfRangeException(String.Format(VenturaSqlStrings.UNKNOWN_COLUMN_NAME, column_name));
            }

            public int ModifiedColumnCount()
            {
                int count = 0;
                if (_mod__order_id == true) count++;
                if (_mod__item_id == true) count++;
                if (_mod__product_id == true) count++;
                if (_mod__quantity == true) count++;
                if (_mod__list_price == true) count++;
                if (_mod__discount == true) count++;
                return count;
            }

            public bool PendingChanges()
            {
                if (_recordstatus == DataRecordStatus.New || _recordstatus == DataRecordStatus.ExistingDelete) return true;
                int count = 0;
                if (_started_with_dbvalues)
                {
                    if (_mod__order_id) count++;
                    if (_mod__item_id) count++;
                }
                if (_mod__product_id == true) count++;
                if (_mod__quantity == true) count++;
                if (_mod__list_price == true) count++;
                if (_mod__discount == true) count++;
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
                if (_mod__order_id == false) throw new Exception(string.Format(VenturaSqlStrings.VALUE_NOT_SET_MSG, record_index_to_display, "order_id"));
                if (_mod__item_id == false) throw new Exception(string.Format(VenturaSqlStrings.VALUE_NOT_SET_MSG, record_index_to_display, "item_id"));
            }

            void IRecordBase.WriteChangesToTrackArray(TrackArray track_array)
            {
                if (_recordstatus == DataRecordStatus.New)
                {
                    track_array.AppendDataValue(0, _cur__order_id);
                    track_array.AppendDataValue(1, _cur__item_id);
                    track_array.AppendDataValue(2, _cur__product_id);
                    track_array.AppendDataValue(3, _cur__quantity);
                    track_array.AppendDataValue(4, _cur__list_price);
                    track_array.AppendDataValue(5, _cur__discount);
                }
                else if (_recordstatus == DataRecordStatus.Existing)
                {
                    if (_started_with_dbvalues)
                    {
                        if (_mod__order_id) track_array.AppendDataValue(0, _cur__order_id);
                        if (_mod__item_id) track_array.AppendDataValue(1, _cur__item_id);
                    }
                    if (_mod__product_id) track_array.AppendDataValue(2, _cur__product_id);
                    if (_mod__quantity) track_array.AppendDataValue(3, _cur__quantity);
                    if (_mod__list_price) track_array.AppendDataValue(4, _cur__list_price);
                    if (_mod__discount) track_array.AppendDataValue(5, _cur__discount);
                    if (track_array.HasData == false) return;
                }

                if (_recordstatus == DataRecordStatus.Existing || _recordstatus == DataRecordStatus.ExistingDelete)
                {
                    track_array.AppendPrikeyValue(0, (_mod__order_id && _started_with_dbvalues) ? _ori__order_id : _cur__order_id);
                    track_array.AppendPrikeyValue(1, (_mod__item_id && _started_with_dbvalues) ? _ori__item_id : _cur__item_id);
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
                    if (_mod__item_id) _ori__item_id = default(int);
                    if (_mod__product_id) _ori__product_id = default(int);
                    if (_mod__quantity) _ori__quantity = default(int);
                    if (_mod__list_price) _ori__list_price = default(decimal);
                    if (_mod__discount) _ori__discount = default(decimal);
                }
                _mod__order_id = false;
                _mod__item_id = false;
                _mod__product_id = false;
                _mod__quantity = false;
                _mod__list_price = false;
                _mod__discount = false;
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
}

// The following commented out code is a template for implementing calculated columns.
//
// How to guide: https://docs.sysdev.nl/CalculatedColumns.html

/*
namespace BlazorDemo.Client.VenturaRecordsets
{
	public partial class Updates2Recordset
	{
		public partial class MultiResultsetOrdersRecord
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

		public partial class MultiResultsetOrderItemsRecord
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
}
*/
