using Microsoft.AspNetCore.Components;
using System;

namespace Kenova.Client.Components
{
    public partial class FastIcon : KenovaComponentBase
    {

        [Parameter]
        public bool Enabled { get; set; } = true;

        [Parameter]
        public IconSize Size { get; set; } = IconSize.Regular;

        [Parameter]
        public IconDefinition IconDefinition { get; set; } = null;

        protected override void OnInitialized()
        {
            if (this.IconDefinition == null)
                throw new ArgumentException("Parameter IconDefinition must be set");

        }

    }

    public enum IconSize
    {
        /// <summary>
        /// 16px * 16px
        /// </summary>
        Regular,

        /// <summary>
        /// 32px * 32px
        /// </summary>
        Double
    }

}
