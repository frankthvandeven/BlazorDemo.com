using System;
using System.Data;

namespace VenturaSQL
{
    public static class TypeTools
    {

        /// <summary>
        /// Checks for the Nullable<> kind with the '?' question mark at the end.
        /// </summary>
        public static bool IsGenericTypeNullable(Type type)
        {
            if (type.IsGenericType == true)
                if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    return true;

            return false;
        }

        public static bool IsReferenceTypeNullable(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (type.IsValueType == true)
                return false;

            // If this is not a value type, it is a reference type, so it is automatically nullable

            return true;
        }

        /// <summary>
        /// This one supports generic Nullable<> with the '?' question mark at the end of the type name.
        /// Pass the type as for example 'DateTime?' or 'System.DateTime?'.
        /// Always returns a Type or else an Exception is thrown.
        /// </summary>
        public static Type GetType(string fully_qualified_typename)
        {
            if (fully_qualified_typename == null)
                throw new ArgumentNullException("fully_qualified_typename");

            if (fully_qualified_typename.Length == 0)
                throw new ArgumentException("Empty string not allowed for parameter 'fully_qualified_typename'.");

            if (fully_qualified_typename.EndsWith("?"))
            {
                string temp_name = fully_qualified_typename.Remove(fully_qualified_typename.Length - 1);

                Type temp_type = Type.GetType(temp_name);

                if (temp_type == null)
                    throw new InvalidOperationException($"Type.GetType('{temp_name}') returned null.");

                return typeof(Nullable<>).MakeGenericType(temp_type);
            }
            else
            {
                Type temp_type = Type.GetType(fully_qualified_typename);

                if (temp_type == null)
                    throw new InvalidOperationException($"Type.GetType('{fully_qualified_typename}') returned null.");

                return temp_type;
            }

        }

        public static Type ConvertValueTypeToGenericNullable(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            // NOTE: All forms of Nullable<T> are value types.

            if (type.IsValueType == false)
                throw new ArgumentException("'type' must be a value type and not reference type.");

            if (type == typeof(void))
                throw new ArgumentException("Type void not allowed.");

            if (IsGenericTypeNullable(type) == true)
                throw new ArgumentException("'type' is already a Generic Nullable<>");


            // If the type is a ValueType and is not System.Void, convert it to a Nullable<Type>
            return typeof(Nullable<>).MakeGenericType(type);
        }

        public static string FullTypename(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (IsGenericTypeNullable(type) == true)
            {
                Type under_lying = Nullable.GetUnderlyingType(type);
                return under_lying.FullName + "?";
            }

            return type.FullName;
        }

        /// <summary>
        /// Sets result to null if there is no C# typename.
        /// Example: 'System.Int16' or 'Int16' both become 'short'
        /// Returns false if there is no C# typename.
        /// </summary>
        public static bool TryConvertToCSharpTypeName(string typename, out string result)
        {


            if (typename == "System.Object" || typename == "Object")
                result = "object";
            else if (typename == "System.String" || typename == "String")
                result = "string";
            else if (typename == "System.Boolean" || typename == "Boolean")
                result = "bool";
            else if (typename == "System.Byte" || typename == "Byte")
                result = "byte";
            else if (typename == "System.Byte[]" || typename == "Byte[]")
                result = "byte[]";
            else if (typename == "System.SByte" || typename == "SByte")
                result = "sbyte";
            else if (typename == "System.Int16" || typename == "Int16")
                result = "short";
            else if (typename == "System.UInt16" || typename == "UInt16")
                result = "ushort";
            else if (typename == "System.Int32" || typename == "Int32")
                result = "int";
            else if (typename == "System.UInt32" || typename == "UInt32")
                result = "uint";
            else if (typename == "System.Int64" || typename == "Int64")
                result = "long";
            else if (typename == "System.UInt64" || typename == "UInt64")
                result = "ulong";
            else if (typename == "System.Single" || typename == "Single")
                result = "float";
            else if (typename == "System.Double" || typename == "Double")
                result = "double";
            else if (typename == "System.Decimal" || typename == "Decimal")
                result = "decimal";
            else if (typename == "System.Char" || typename == "Char")
                result = "char";
            else
            {
                result = null;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Converts a string to an Enum.
        /// You can use 'DbType.Int32' or 'Int32' as input.
        /// </summary>
        public static T StringToEnum<T>(string input) where T : Enum
        {
            int index = input.LastIndexOf('.');

            if (index != -1)
                input = input.Substring(index + 1);

            T retvar = (T)Enum.Parse(typeof(T), input);

            return retvar;
        }

        public static DbType FrameworkTypeToDbType(Type type)
        {
            if (type == typeof(string))
                return DbType.String; // AnsiString, AnsiStringFixedLength, StringFixedLength, Xml
            else if (type == typeof(byte[]))
                return DbType.Binary;
            else if (type == typeof(byte))
                return DbType.Byte;
            else if (type == typeof(bool))
                return DbType.Boolean;
            else if (type == typeof(decimal))
                return DbType.Decimal; // Currency, VarNumeric
            else if (type == typeof(DateTime))
                return DbType.DateTime; // Date, DateTime2
            else if (type == typeof(Double))
                return DbType.Double;
            else if (type == typeof(Guid))
                return DbType.Guid;
            else if (type == typeof(Int16))
                return DbType.Int16;
            else if (type == typeof(Int32))
                return DbType.Int32;
            else if (type == typeof(Int64))
                return DbType.Int64;
            else if (type == typeof(object))
                return DbType.Object;
            else if (type == typeof(SByte))
                return DbType.SByte;
            else if (type == typeof(Single))
                return DbType.Single;
            else if (type == typeof(TimeSpan))
                return DbType.Time;
            else if (type == typeof(UInt16))
                return DbType.UInt16;
            else if (type == typeof(UInt32))
                return DbType.UInt32;
            else if (type == typeof(UInt64))
                return DbType.UInt64;
            else if (type == typeof(DateTimeOffset))
                return DbType.DateTimeOffset;
            else
                return DbType.Object;

        } // end of class
    }
}
