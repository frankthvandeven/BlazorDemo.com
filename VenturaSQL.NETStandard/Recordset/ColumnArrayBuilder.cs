using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace VenturaSQL
{
    /// <summary>
    /// First you build a list of VenturaSqlColumn's with ColumnArrayBuilder, and then
    /// you use this list to initialize the VenturaSqlSchema object.
    /// </summary>
    public partial class ColumnArrayBuilder
    {
        private List<VenturaSqlColumn> _list = new List<VenturaSqlColumn>();

        public VenturaSqlColumn[] ToArray()
        {
            return _list.ToArray();
        }

        /// <summary>
        /// Adds a raw VenturaSqlColumn object to the collection.
        /// </summary>
        /// <param name="column"></param>
        public void Add(VenturaSqlColumn column)
        {
            _list.Add(column);
        }

        // <summary>
        // Adds a column to the collection used to store Sql parameter information.
        // </summary>
        public void AddParameterColumn(string column_name, Type column_type, bool input, bool output, DbType? dbtype, int? columnsize, byte? precision, byte? scale)
        {
            VenturaSqlColumn tempcolumn = new VenturaSqlColumn(column_name, column_type, true);
            tempcolumn.Input = input;
            tempcolumn.Output = output;

            tempcolumn.DbType = dbtype;
            tempcolumn.ColumnSize = columnsize;
            tempcolumn.NumericPrecision = precision;
            tempcolumn.NumericScale = scale;

            _list.Add(tempcolumn);
        }
    }
}


/// <summary>
/// Schema will be read from a byte array (received via Http/TCP).
/// </summary>
//public void AddFromBuffer(byte[] buffer, int offset)
//{
//    int bytelen;

//    // Fieldcount
//    int FieldCount = BitConverter.ToInt16(buffer, offset);
//    offset += 2;

//    for (int x = 0; x < FieldCount; x++)
//    {
//        VenturaSqlColumn tempcolumn = new VenturaSqlColumn();

//        // ColumnName
//        bytelen = BitConverter.ToInt16(buffer, offset);
//        offset += 2;

//        tempcolumn.ColumnName = Encoding.UTF8.GetString(buffer, offset, bytelen);
//        offset += bytelen;

//        // ColumnCode
//        tempcolumn.ColumnCode = (VenturaSqlCode)buffer[offset];
//        offset += 1;

//        // ColumnSource
//        tempcolumn.ColumnSource = (VenturaSqlColumnSource)buffer[offset];
//        offset += 1;

//        if (tempcolumn.ColumnSource == VenturaSqlColumnSource.SqlServer)
//        {
//            // BaseTableName
//            bytelen = buffer[offset];
//            offset += 1;

//            tempcolumn.BaseTableName = Encoding.UTF8.GetString(buffer, offset, bytelen);
//            offset += bytelen;

//            // ProviderType
//            tempcolumn.ProviderType = (VenturaSqlDbType)BitConverter.ToInt32(buffer, offset);
//            offset += 4;

//            tempcolumn.ColumnSize = BitConverter.ToInt32(buffer, offset);
//            offset += 4;

//            tempcolumn.NumericPrecision = buffer[offset];
//            offset += 1;

//            tempcolumn.NumericScale = buffer[offset];
//            offset += 1;

//            // work the bool flags
//            ushort flags = BitConverter.ToUInt16(buffer, offset);
//            offset += 2;

//            tempcolumn.IsUnique = (flags & 1) == 1;
//            tempcolumn.IsKey = (flags & 2) == 2;
//            tempcolumn.AllowDBNull = (flags & 4) == 4;
//            tempcolumn.IsAliased = (flags & 8) == 8;
//            tempcolumn.IsExpression = (flags & 16) == 16;
//            tempcolumn.IsIdentity = (flags & 32) == 32;
//            tempcolumn.IsAutoIncrement = (flags & 64) == 64;
//            //tempcolumn.xxx = (flags & 128) == 128;
//            //tempcolumn.xxx = (flags & 256) == 256;
//            tempcolumn.IsLong = (flags & 512) == 512;
//            tempcolumn.IsReadOnly = (flags & 1024) == 1024;
//            tempcolumn.IsRowGuid = (flags & 2048) == 2048;
//            tempcolumn.Updateable 2048
//        } // endif if database is the source

//        _list.Add(tempcolumn);
//    } // end of field loop

//} // end of constructor
