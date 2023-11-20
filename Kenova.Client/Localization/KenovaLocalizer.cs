using Microsoft.AspNetCore.Components;
using System.Resources;

namespace Kenova.Client.Localization
{

    public class KenovaLocalizer
    {
        protected ResourceManager _resourceManager;

        public KenovaLocalizer()
        {
            //_resourceManager = KenovaResources.ResourceManager;
        }

        internal void SetResourceManager(ResourceManager rm)
        {
            _resourceManager = rm;
        }

        // To get the locale key from mapped resources file
        public string this[string key]
        {
            get
            {
                return _resourceManager.GetString(key);
            }
        }

        public string this[string key, params object[] arguments]
        {
            get
            {
                return string.Format(_resourceManager.GetString(key), arguments);
            }
        }

        /// <summary>
        /// Returns a MarkupString, resulting in raw Html.
        /// </summary>
        /// <returns></returns>
        public MarkupString Html(string key)
        {
            return new MarkupString(this[key]);
        }

        /// <summary>
        /// Returns a MarkupString, resulting in raw Html.
        /// </summary>
        /// <returns></returns>
        public MarkupString Html(string key, params object[] arguments)
        {
            return new MarkupString(string.Format(this[key], arguments));
        }

    }
}
