using System;
using System.IO;

namespace VenturaSQL
{
    public partial class VenturaSqlSchema
    {
        /// <summary>
        /// Used for calculating the Recordset hash 
        /// </summary>
        public void WriteSchemaToStream(Stream stream)
        {
            BinaryWriter bw = new BinaryWriter(stream);

            bw.Write(_list.Length);

            foreach (VenturaSqlColumn column in _list)
            {
                // ColumnName
                bw.Write(column.ColumnName);

                // ColumnType as FullName
                bw.Write(column.FullTypename);

                // BaseTableName
                bw.Write(column.BaseServerName);
                bw.Write(column.BaseCatalogName);
                bw.Write(column.BaseSchemaName);
                bw.Write(column.BaseTableName);
                bw.Write(column.BaseColumnName);

                bw.Write(column.XmlSchemaCollectionDatabase);
                bw.Write(column.XmlSchemaCollectionOwningSchema);
                bw.Write(column.XmlSchemaCollectionName);
                bw.Write(column.UdtAssemblyQualifiedName);

                // ProviderType
                bw.Write(column.ProviderType);

                // ColumnSize
                if (column.ColumnSize != null)
                    bw.Write(column.ColumnSize.Value);

                // NumericPrecision
                if (column.NumericPrecision != null)
                    bw.Write(column.NumericPrecision.Value);

                // NumericScale
                if (column.NumericScale != null)
                    bw.Write(column.NumericScale.Value);

                // begin: bool wrapping
                ushort flags = 0;
                if (column.IsUnique == true) flags |= 1;
                if (column.IsKey == true) flags |= 2;
                if (column.IsNullable == true) flags |= 4;
                if (column.IsAliased == true) flags |= 8;
                if (column.IsExpression == true) flags |= 16;
                if (column.IsIdentity == true) flags |= 32;
                if (column.IsAutoIncrement == true) flags |= 64;
                //if(  column.xxx == true ) flags |=  128;
                //if(  column.xxx == true ) flags |=  256;
                if (column.IsLong == true) flags |= 512;
                if (column.IsReadOnly == true) flags |= 1024;
                if (column.IsRowGuid == true) flags |= 2048;
                if (column.Updateable == true) flags |= 4096;

                bw.Write(flags);

                //valuearray = BitConverter.GetBytes((ushort)flags);
                //stream.Write(valuearray, 0, 2);
                // end: bool wrapping

            } // column loop

        } // end of function

        //public void CheckRowDataTypes(object[] columnvalues)
        //{

        //    if (columnvalues.Length < _list.Length)
        //        throw new VenturaSqlException($"CheckRowDataTypes found an error. Columnvalues array shorter than {_list.Length}");

        //    for (int x = 0; x < _list.Length; x++)
        //    {
        //        object columnvalue = columnvalues[x];
        //        Type columntype = columnvalues[x].GetType();
        //        //TypeCode typecode = Type.GetTypeCode(type);

        //        switch (column.ColumnCode)
        //        {
        //            case VenturaSqlCode.Boolean:
        //                if (!(columnvalue is Boolean))
        //                    ThrowDataTypeException(column.ColumnName, x, "Boolean", columntype.ToString());
        //                break;
        //            case VenturaSqlCode.Byte:
        //                if (!(columnvalue is Byte))
        //                    ThrowDataTypeException(column.ColumnName, x, "Byte", columntype.ToString());
        //                break;
        //            case VenturaSqlCode.DateTime:
        //                if (!(columnvalue is DateTime))
        //                    ThrowDataTypeException(column.ColumnName, x, "DateTime", columntype.ToString());
        //                break;
        //            case VenturaSqlCode.Decimal:
        //                if (!(columnvalue is Decimal))
        //                    ThrowDataTypeException(column.ColumnName, x, "Decimal", columntype.ToString());
        //                break;
        //            case VenturaSqlCode.Single:
        //                if (!(columnvalue is Single))
        //                    ThrowDataTypeException(column.ColumnName, x, "Single(float)", columntype.ToString());
        //                break;
        //            case VenturaSqlCode.Double:
        //                if (!(columnvalue is Double))
        //                    ThrowDataTypeException(column.ColumnName, x, "Double", columntype.ToString());
        //                break;
        //            case VenturaSqlCode.Int16:
        //                if (!(columnvalue is Int16))
        //                    ThrowDataTypeException(column.ColumnName, x, "Int16(short)", columntype.ToString());
        //                break;
        //            case VenturaSqlCode.Int32:
        //                if (!(columnvalue is Int32))
        //                    ThrowDataTypeException(column.ColumnName, x, "Int32(int)", columntype.ToString());
        //                break;
        //            case VenturaSqlCode.Int64:
        //                if (!(columnvalue is Int64))
        //                    ThrowDataTypeException(column.ColumnName, x, "Int64(long)", columntype.ToString());
        //                break;
        //            case VenturaSqlCode.String:
        //                if (!(columnvalue is String))
        //                    ThrowDataTypeException(column.ColumnName, x, "String", columntype.ToString());
        //                break;
        //            case VenturaSqlCode.Guid:
        //                if (!(columnvalue is Guid))
        //                    ThrowDataTypeException(column.ColumnName, x, "Guid", columntype.ToString());
        //                break;
        //            case VenturaSqlCode.Bytes:
        //                if (columntype.IsArray == false)
        //                    ThrowDataTypeException(column.ColumnName, x, "Byte[]", columntype.ToString());

        //                if (columntype.GetArrayRank() != 1)
        //                    ThrowDataTypeException(column.ColumnName, x, "1 dimensional byte array", "multi-dimensional array");

        //                if (columntype.GetElementType().FullName != "System.Byte")
        //                    ThrowDataTypeException(column.ColumnName, x, "Byte[]", "array of " + columntype.GetElementType().ToString());

        //                break;
        //            default:
        //                break;
        //        } // end of switch
        //    } // end of loop

        //} // end of method

        //private void ThrowDataTypeException(string columnname, int columnordinal, string expectedtype, string detectedtype)
        //{
        //    throw new VenturaSqlException($"Schemaholder detected invalid data type in column '{columnname}' (ordinal {columnordinal}). The expected type is {expectedtype} but the detected type is {detectedtype}.");
        //}

        public int GetColumnOrdinal(string columnName)
        {
            for (int x = 0; x < _list.Length; x++)
            {
                if (_list[x].ColumnName == columnName) 
                    return x;
            }

            return -1; // not found
        }

    } // end of class
} // end of namespace

