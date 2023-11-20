using System;
using System.Collections;
using System.Collections.Generic;

namespace VenturaSQL
{

    public partial class VenturaSqlSchema : IEnumerable<VenturaSqlColumn>
    {
        private VenturaSqlColumn[] _list = null;
        private SchemaCode[] _schemacodes = null;

        private VenturaSqlColumn _identity_column = null;

        /// <summary>
        /// This is the only constructor for the VenturaSqlSchema class.
        ///
        /// Schema will be initialized with a list of VenturaSqlColumns.
        /// The ColumnOrdinal will be correctly set for each column.
        /// </summary>
        public VenturaSqlSchema(ColumnArrayBuilder builder)
        {
            _list = builder.ToArray();

            // Set the column ordinal.
            for (short i = 0; i < _list.Length; i++)
                _list[i].ColumnOrdinal = i;


            // Set the indentity column info.
            for (short i = 0; i < _list.Length; i++)
            {
                if (_list[i].IsIdentity == true)
                {
                    _identity_column = _list[i];
                    break;
                }
            }

            // Initialize internal buffer
            _schemacodes = new SchemaCode[_list.Length];

            FillSchemaCodes();
        }

        public VenturaSqlColumn this[int index]
        {
            get
            {
                return _list[index];
            }
        }

        public VenturaSqlColumn this[string column_name]
        {
            get
            {
                for (int x = 0; x < _list.Length; x++)
                {
                    if (_list[x].ColumnName == column_name)
                        return _list[x];
                }
                return null;
            }
        }

        public List<VenturaSqlColumn> FindAll(Predicate<VenturaSqlColumn> match)
        {
            if (match == null)
                throw new ArgumentNullException("match");

            List<VenturaSqlColumn> list = new List<VenturaSqlColumn>();

            for (int i = 0; i < _list.Length; i++)
                if (match(_list[i]))
                    list.Add(_list[i]);

            return list;
        }

        /// <summary>
        /// Returns the (first) column that has IsIdentity set to true.
        /// Returns null if there is no such column.
        /// </summary>
        public VenturaSqlColumn IdentityColumn
        {
            get { return _identity_column; }
        }

        public int Count
        {
            get { return _list.Length; }
        }

        public IEnumerator<VenturaSqlColumn> GetEnumerator()
        {
            foreach (VenturaSqlColumn column in _list)
                yield return column;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void FillSchemaCodes()
        {
            for (int i = 0; i < _list.Length; i++)
            {
                Type type = _list[i].ColumnType;

                if (type == typeof(Boolean))
                    _schemacodes[i] = SchemaCode.Boolean;
                else if (type == typeof(Byte))
                    _schemacodes[i] = SchemaCode.Byte;
                else if (type == typeof(DateTime))
                    _schemacodes[i] = SchemaCode.DateTime;
                else if (type == typeof(Decimal))
                    _schemacodes[i] = SchemaCode.Decimal;
                else if (type == typeof(Single))
                    _schemacodes[i] = SchemaCode.Single;
                else if (type == typeof(Double))
                    _schemacodes[i] = SchemaCode.Double;
                else if (type == typeof(Int16))
                    _schemacodes[i] = SchemaCode.Int16;
                else if (type == typeof(Int32))
                    _schemacodes[i] = SchemaCode.Int32;
                else if (type == typeof(Int64))
                    _schemacodes[i] = SchemaCode.Int64;
                else if (type == typeof(String))
                    _schemacodes[i] = SchemaCode.String;
                else if (type == typeof(Guid))
                    _schemacodes[i] = SchemaCode.Guid;
                else if (type == typeof(byte[]))
                    _schemacodes[i] = SchemaCode.Bytes;
                else if (type == typeof(Object))
                    _schemacodes[i] = SchemaCode.Object;
                else if (type == typeof(TimeSpan))
                    _schemacodes[i] = SchemaCode.TimeSpan;
                else if (type == typeof(DateTimeOffset))
                    _schemacodes[i] = SchemaCode.DateTimeOffset;
                else
                    throw new InvalidOperationException($"VenturaSqlSchema doesn't know how to binarize {type.FullName} yet. Please contact support.");
            }


        }

        private enum SchemaCode : byte
        {
            Boolean = 1,
            Byte = 2,
            DateTime = 3,
            Decimal = 4,
            Single = 5,
            Double = 6,
            Int16 = 7,
            Int32 = 8,
            Int64 = 9,
            String = 10,
            Guid = 11,
            Bytes = 12,
            Object = 13,
            TimeSpan = 14,
            DateTimeOffset = 15
        }

    } // end of class

} // end of namespace

