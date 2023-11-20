
using System;

namespace Kenova.Client.Components
{
    public static class PropertyCopier
    {
        public static void Copy(object from, object to)
        {
            var fromProperties = from.GetType().GetProperties();
            var toProperties = to.GetType().GetProperties();

            foreach (var fromProperty in fromProperties)
            {
                foreach (var toProperty in toProperties)
                {
                    if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                    {
                        if (KenovaClientConfig.Diagnostics) Console.WriteLine($"PropertyCopier - copying {fromProperty.Name} value {fromProperty.GetValue(from)}");
                        toProperty.SetValue(to, fromProperty.GetValue(from));
                        break;
                    }
                }
            }
        }

    }
}
