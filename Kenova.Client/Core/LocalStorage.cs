using Microsoft.JSInterop;
using System;
using System.Text.Json;

namespace Kenova.Client
{

    /// <summary>
    /// LocalStorage.Set("Test", 42);
    /// var val = LocalStorage.Get<int>("Test");
    /// LocalStorage.Remove("Test");
    /// LocalStorage.Clear();
    /// </summary>
    public static class LocalStorage
    {
        public static ValueTask SetStringAsync(string key, string value)
        {
            if (value is null)
                throw new ArgumentNullException("value");

            return KenovaClientConfig.JSRuntime.InvokeVoidAsync("localStorage.setItem", new object[] { key, value });

        }

        public static async ValueTask<string> GetStringAsync(string key, string defaultValue = null)
        {
            string result = await KenovaClientConfig.JSRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (result == null)
                return defaultValue;

            return result;
        }

        //public static void SetJson<T>(string key, T value)
        //{
        //    string jsVal = null;

        //    if (value != null)
        //        jsVal = JsonSerializer.Serialize(value);

        //    KenovaClientConfig.JSInProcessRuntime.InvokeVoid("localStorage.setItem", new object[] { key, jsVal });
        //}

        //public static T GetJson<T>(string key, T defaultValue = default)
        //{
        //    string val = KenovaClientConfig.JSInProcessRuntime.Invoke<string>("localStorage.getItem", key);

        //    if (val == null)
        //        return defaultValue; 

        //    T result = JsonSerializer.Deserialize<T>(val);

        //    return result;
        //}

        public static ValueTask RemoveAsync(string key)
        {
            return KenovaClientConfig.JSRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        public static ValueTask ClearAsync()
        {
            return KenovaClientConfig.JSRuntime.InvokeVoidAsync("localStorage.clear");
        }

    }
}
