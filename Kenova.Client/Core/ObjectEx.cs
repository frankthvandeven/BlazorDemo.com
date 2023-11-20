using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Kenova.Client.Core
{
    internal static class ObjectEx
    {
        public readonly static string PropertyPathRegExPattern;

        public readonly static Regex PropertyPathRegEx;

        static ObjectEx()
        {
            ObjectEx.PropertyPathRegExPattern = "\\w*\\[[-{}.\\w\\s]+\\]|\\[[-{}.\\w\\s]+\\]|\\w+";
            ObjectEx.PropertyPathRegEx = new Regex(ObjectEx.PropertyPathRegExPattern, RegexOptions.Compiled);
        }

        public static PropertyInfo GetIndexerProperty(Type targetType, string propertyName, ref object indexValue)
        {
            int num;
            PropertyInfo propertyInfo;
            Guid guid;
            using (IEnumerator<PropertyInfo> enumerator = TypeEx.GetIndexerProperties(targetType).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    PropertyInfo current = enumerator.Current;
                    if (!String.IsNullOrWhiteSpace(propertyName) && current.Name != propertyName)
                    {
                        continue;
                    }
                    ParameterInfo[] indexParameters = current.GetIndexParameters();
                    if ((int)indexParameters.Length > 1)
                    {
                        continue;
                    }
                    Type parameterType = indexParameters[0].ParameterType;
                    if (parameterType == typeof(Int32) && Int32.TryParse(indexValue as String, out num))
                    {
                        indexValue = num;
                        propertyInfo = current;
                        return propertyInfo;
                    }
                    else if (!(parameterType == typeof(Guid)) || !Guid.TryParse(indexValue as String, out guid))
                    {
                        if (parameterType != typeof(String))
                        {
                            continue;
                        }
                        propertyInfo = current;
                        return propertyInfo;
                    }
                    else
                    {
                        indexValue = guid;
                        propertyInfo = current;
                        return propertyInfo;
                    }
                }
                return null;
            }
            return propertyInfo;
        }

        internal static T GetPropertyValue<T>(this object target, string path, IValueConverter converter = null, object converterParameter = null, CultureInfo culture = null)
        {
            T t;
            if (target == null)
            {
                t = default(T);
                return t;
            }
            object propertyValue = target.GetPropertyValue(path);
            if (converter != null)
            {
                propertyValue = converter.Convert(propertyValue, typeof(T), converterParameter, culture);
            }
            if (propertyValue != null)
            {
                return (T)propertyValue;
            }
            t = default(T);
            return t;
        }

        private static object GetPropertyValue(this object target, string path)
        {
            if (target == null)
            {
                return null;
            }
            Type type = target.GetType();
            if (String.IsNullOrEmpty(path))
            {
                PropertyInfo propertyInfo = TypeEx.GetDefaultProperties(type).FirstOrDefault<PropertyInfo>();
                if (!(propertyInfo != null) || propertyInfo.GetIndexParameters().Length != 0)
                {
                    return target;
                }
                return propertyInfo.GetValue(target);
            }
            MatchCollection matchCollections = ObjectEx.PropertyPathRegEx.Matches(path);
            if (matchCollections.Count > 1)
            {
                Match item = matchCollections[0];
                object propertyValue = target.GetPropertyValue(item.Value);
                string str = path.Substring(item.Index + item.Length);
                if (str.StartsWith("."))
                {
                    str = str.Substring(1);
                }
                return propertyValue.GetPropertyValue(str);
            }
            if (!path.Contains("["))
            {
                PropertyInfo property = type.GetProperty(path);
                if (property == null)
                {
                    return null;
                }
                return property.GetValue(target, null);
            }
            string str1 = path.Substring(0, path.IndexOf('['));
            object obj = path.Substring(path.IndexOf('[') + 1, path.IndexOf(']') - path.IndexOf('[') - 1);
            PropertyInfo indexerProperty = ObjectEx.GetIndexerProperty(type, str1, ref obj);
            if (indexerProperty != null)
            {
                return indexerProperty.GetValue(target, new Object[] { obj });
            }
            return target.GetPropertyValue(str1).GetPropertyValue(path.Substring(path.IndexOf('['), path.IndexOf(']') - path.IndexOf('[') + 1));
        }

        internal static void SetPropertyValue<T>(this object target, string path, T value, IValueConverter converter = null, object converterParameter = null, CultureInfo culture = null)
        {
            if (target == null)
            {
                return;
            }
            Type type = target.GetType();
            if (String.IsNullOrEmpty(path))
            {
                PropertyInfo propertyInfo = TypeEx.GetDefaultProperties(type).FirstOrDefault<PropertyInfo>();
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(target, value, null, converter, converterParameter, culture);
                }
                return;
            }
            MatchCollection matchCollections = ObjectEx.PropertyPathRegEx.Matches(path);
            if (matchCollections.Count > 1)
            {
                Match item = matchCollections[0];
                object propertyValue = target.GetPropertyValue(item.Value);
                string str = path.Substring(item.Index + item.Length + 1);
                propertyValue.SetPropertyValue<T>(str, value, converter, converterParameter, culture);
                return;
            }
            if (!path.Contains("["))
            {
                type.GetProperty(path).SetValue(target, value, null, converter, converterParameter, culture);
                return;
            }
            string str1 = path.Substring(0, path.IndexOf('['));
            object obj = path.Substring(path.IndexOf('[') + 1, path.IndexOf(']') - path.IndexOf('[') - 1);
            PropertyInfo indexerProperty = ObjectEx.GetIndexerProperty(type, str1, ref obj);
            if (indexerProperty != null)
            {
                indexerProperty.SetValue(target, value, new Object[] { obj }, converter, converterParameter, culture);
                return;
            }
            target.GetPropertyValue(str1).SetPropertyValue<T>(path.Substring(path.IndexOf('['), path.IndexOf(']') - path.IndexOf('[') + 1), value, converter, converterParameter, culture);
        }

        internal static void SetValue(this PropertyInfo propertyInfo, object target, object value, object[] index, IValueConverter converter, object converterParameter, CultureInfo culture)
        {
            if (converter != null && (value == null || propertyInfo.PropertyType != value.GetType()))
            {
                value = converter.ConvertBack(value, propertyInfo.PropertyType, converterParameter, culture);
            }
            else if (propertyInfo.PropertyType.GetTypeInfo().IsEnum && value is String)
            {
                try
                {
                    value = Enum.Parse(propertyInfo.PropertyType, (String)value, true);
                }
                catch
                {
                }
            }
            propertyInfo.SetValue(target, value, index);
        }
    }
}