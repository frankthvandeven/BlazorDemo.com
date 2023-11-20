using Kenova.Client.Authorization;
using Kenova.Client.Core;
using Kenova.Client.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Kenova.Client
{
    public static class KenovaClientConfig
    {
        private static bool _diagnostics = false;

        private static IServiceProvider _serviceProvider;
        private static IJSRuntime _jsRuntime;
        //private static IJSInProcessRuntime _jsInProcessRuntime;
        private static KenovaAuthenticationStateProvider _authenticationStateProvider;

        private static ulong _counter;

        private static Settings _settings = new Settings();
        private static Labels _labels = new Labels();

        private static KenovaLocalizer _localizer;

        public static bool Diagnostics
        {
            get { return _diagnostics; }
            set { _diagnostics = value; } 
        }

        public static KenovaLocalizer Localizer
        {
            get { return _localizer; }
            internal set { _localizer = value; }
        }

        public static IServiceProvider ServiceProvider
        {
            get { return _serviceProvider; }
            internal set { _serviceProvider = value; }
        }

        /// <summary>
        /// For asynchronous calls. Slower.
        /// </summary>
        public static IJSRuntime JSRuntime
        {
            get { return _jsRuntime; }
            internal set { _jsRuntime = value; }
        }

        /// <summary>
        /// For synchronous calls. Faster. Only works in Blazor WebAssembly projects.
        /// </summary>
        //public static IJSInProcessRuntime JSInProcessRuntime
        //{
        //    get { return _jsInProcessRuntime; }
        //    internal set { _jsInProcessRuntime = value; }
        //}

        public static KenovaAuthenticationStateProvider AuthenticationStateProvider
        {
            get { return _authenticationStateProvider; }
            internal set { _authenticationStateProvider = value; }
        }

        public static Guid AppGuid { get; set; } = Guid.NewGuid();

        public static Settings Settings
        {
            get { return _settings; }
        }

        public static Labels Labels
        {
            get { return _labels; }
        }

        public static string GetUniqueElementID()
        {
            string id = "kn" + Convert.ToString(_counter);

            _counter++;

            if (_counter == ulong.MaxValue)
                _counter = 0;

            return id;
        }



    }
}
