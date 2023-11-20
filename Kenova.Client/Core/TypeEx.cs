using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Kenova.Client.Core
{
    internal static class TypeEx
    {
        public static bool DataTypeIsPrimitive(this Type dataType)
        {
            if (dataType == null)
            {
                return false;
            }
            if (dataType.GetTypeInfo().IsPrimitive || !(dataType != typeof(String)) || !(dataType != typeof(DateTime)))
            {
                return true;
            }
            return dataType == typeof(Decimal);
        }

        public static Type FindGenericType(this Type definition, Type type)
        {
            while (type != null && type != typeof(Object))
            {
                if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == definition)
                {
                    return type;
                }
                if (definition.GetTypeInfo().IsInterface)
                {
                    Type[] interfaces = type.GetInterfaces();
                    for (int i = 0; i < (int)interfaces.Length; i++)
                    {
                        Type type1 = definition.FindGenericType(interfaces[i]);
                        if (type1 != null)
                        {
                            return type1;
                        }
                    }
                }
                type = type.GetTypeInfo().BaseType;
            }
            return null;
        }

        public static IEnumerable<PropertyInfo> GetDefaultProperties(Type type)
        {
            Type type1 = typeof(DefaultMemberAttribute);
            TypeInfo typeInfo = type.GetTypeInfo();
            CustomAttributeData customAttributeDatum = typeInfo.CustomAttributes.FirstOrDefault<CustomAttributeData>((CustomAttributeData ca) => ca.AttributeType == type1);
            if (customAttributeDatum != null)
            {
                foreach (CustomAttributeTypedArgument constructorArgument in customAttributeDatum.ConstructorArguments)
                {
                    PropertyInfo declaredProperty = typeInfo.GetDeclaredProperty(constructorArgument.Value as String);
                    if (declaredProperty == null)
                    {
                        continue;
                    }
                    yield return declaredProperty;
                }
            }
        }

        public static ConstructorInfo GetDefaultPublicCtor(this Type type)
        {
            return type.GetConstructor(new Type[0]);
        }

        public static IList<T> GetEnumValues<T>(this Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException(String.Format("{0} should be a an EnumType", enumType.Name));
            }
            List<T> ts = new List<T>();
            FieldInfo[] fields = enumType.GetFields();
            for (int i = 0; i < (int)fields.Length; i++)
            {
                FieldInfo fieldInfo = fields[i];
                if (!fieldInfo.IsSpecialName)
                {
                    T value = (T)fieldInfo.GetValue(enumType);
                    if (!ts.Contains(value))
                    {
                        ts.Add(value);
                    }
                }
            }
            return ts;
        }

        public static IEnumerable<Type> GetImplementedInterfaces(Type type, bool includeSelf = false)
        {
            if (includeSelf && type.IsInterfaceType())
            {
                yield return type;
            }
            foreach (Type implementedInterface in type.GetTypeInfo().ImplementedInterfaces)
            {
                yield return implementedInterface;
            }
        }

        public static IEnumerable<PropertyInfo> GetIndexerProperties(Type type)
        {
            foreach (PropertyInfo runtimeProperty in type.GetRuntimeProperties())
            {
                if ((int)runtimeProperty.GetIndexParameters().Length != 1)
                {
                    continue;
                }
                yield return runtimeProperty;
            }
            foreach (Type implementedInterface in type.GetTypeInfo().ImplementedInterfaces)
            {
                foreach (PropertyInfo indexerProperty in TypeEx.GetIndexerProperties(implementedInterface))
                {
                    yield return indexerProperty;
                }
            }
        }

        public static Type GetItemType(this IEnumerable enumerable)
        {
            if (enumerable == null)
            {
                return typeof(Object);
            }
            Type type = enumerable.GetType();
            Type nonNullableType = null;
            if (type.IsEnumerableType())
            {
                nonNullableType = type.GetItemType().GetNonNullableType();
            }
            if (nonNullableType != null && nonNullableType != typeof(Object))
            {
                return nonNullableType;
            }
            return (
                from object x in enumerable
                select x.GetType()).FirstOrDefault<Type>() ?? (nonNullableType ?? typeof(Object));
        }

        private static Type GetItemType(this Type enumerableType)
        {
            Type type = typeof(IEnumerable<>).FindGenericType(enumerableType);
            if (type == null)
            {
                return enumerableType;
            }
            return type.GetGenericArguments()[0];
        }

        public static Type GetNonNullableType(this Type type)
        {
            if (!type.IsNullableType())
            {
                return type;
            }
            return Nullable.GetUnderlyingType(type);
        }

        public static IEnumerable<PropertyInfo> GetProperties(this Type type)
        {
            if (!(type != null) || type.DataTypeIsPrimitive())
            {
                return new PropertyInfo[0];
            }
            return type.GetTypeInfo().DeclaredProperties.Where<PropertyInfo>((PropertyInfo p) => {
                bool flag;
                try
                {
                    MethodInfo getMethod = p.GetMethod;
                    flag = (!(getMethod != null) || !getMethod.Attributes.HasFlag(MethodAttributes.Static) ? p.GetIndexParameters().Length == 0 : false);
                }
                catch
                {
                    flag = true;
                }
                return flag;
            }).ToArray<PropertyInfo>();
        }

        public static bool Implements(this Type type, Type interfaceType)
        {
            return TypeEx.GetImplementedInterfaces(type, false).Any<Type>((Type ii) => ii == interfaceType);
        }

        internal static bool IsEnumerableType(this Type enumerableType)
        {
            return typeof(IEnumerable<>).FindGenericType(enumerableType) != null;
        }

        public static bool IsInterfaceType(this Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }

        public static bool IsNullableType(this Type type)
        {
            if (!(type != null) || !type.IsGenericType)
            {
                return false;
            }
            return type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsNumeric(this Type type)
        {
            if (type == typeof(Double) || type == typeof(Single) || type == typeof(Int32) || type == typeof(UInt32) || type == typeof(Int64) || type == typeof(UInt64) || type == typeof(Int16) || type == typeof(UInt16) || type == typeof(SByte) || type == typeof(Byte))
            {
                return true;
            }
            return type == typeof(Decimal);
        }

        public static bool IsNumericIntegral(this Type type)
        {
            if (type == typeof(Int32) || type == typeof(UInt32) || type == typeof(Int64) || type == typeof(UInt64) || type == typeof(Int16) || type == typeof(UInt16) || type == typeof(SByte))
            {
                return true;
            }
            return type == typeof(Byte);
        }

        public static bool IsNumericIntegralSigned(this Type type)
        {
            if (type == typeof(Int32) || type == typeof(Int64) || type == typeof(Int16))
            {
                return true;
            }
            return type == typeof(SByte);
        }

        public static bool IsNumericIntegralUnsigned(this Type type)
        {
            if (type == typeof(UInt32) || type == typeof(UInt64) || type == typeof(UInt16))
            {
                return true;
            }
            return type == typeof(Byte);
        }

        public static bool IsNumericNonIntegral(this Type type)
        {
            if (type == typeof(Double) || type == typeof(Single))
            {
                return true;
            }
            return type == typeof(Decimal);
        }

        internal static bool IsReadOnlyCollection(object instance)
        {
            Type type = typeof(ICollection<>).FindGenericType(instance.GetType());
            if (type == null)
            {
                return false;
            }
            return (Boolean)type.GetProperty("IsReadOnly").GetValue(instance, null);
        }

        public static T New<T>(this Type type)
        {
            ConstructorInfo defaultPublicCtor = type.GetDefaultPublicCtor();
            if (defaultPublicCtor == null)
            {
                throw new InvalidOperationException(String.Format("Cannot find a default constructor for type {0}", type.FullName));
            }
            return (T)defaultPublicCtor.Invoke(new Object[0]);
        }

        public static object New(this Type type)
        {
            if (type == null)
            {
                return null;
            }
            ConstructorInfo defaultPublicCtor = type.GetDefaultPublicCtor();
            if (defaultPublicCtor == null)
            {
                return null;
            }
            return defaultPublicCtor.Invoke(new Object[0]);
        }

        public static T New<T>(this Type type, Action<T> initializers)
        {
            T t = type.New<T>();
            if (initializers != null)
            {
                initializers(t);
            }
            return t;
        }
    }
}