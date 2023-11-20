using Microsoft.AspNetCore.Components;
using System;

namespace Kenova.Client.Components
{
    public partial class DropdownListBasic<ValueType> : KenovaComponentBase, IRerender, IDisposable
    {

        /* All parameters in this file are relayed to InputString */

        [Parameter]
        public string AdditionalStyle { get; set; } = null;

        [Parameter]
        public string Caption { get; set; } = null;

        [Parameter]
        public double Width { get; set; } = -1;

        [Parameter]
        public double MaxWidth { get; set; } = -1;

        /// <summary>
        /// Explicitly sets the width of the InputBox.
        /// The default value for this parameter is -1.
        /// </summary>
        [Parameter]
        public double InputBoxWidth { get; set; } = -1;

        /// <summary>
        /// Explicitly sets the width of the Caption.
        /// The default value for this parameter is -1.
        /// </summary>
        [Parameter]
        public double CaptionWidth { get; set; } = -1;

        /// <summary>
        /// Explicitly sets the width of the Remark.
        /// The default value for this parameter is -1.
        /// </summary>
        [Parameter]
        public double RemarkWidth { get; set; } = -1;

        /// <summary>
        /// The default value for this parameter is false.
        /// </summary>
        [Parameter]
        public bool CaptionLeft { get; set; } = false;

        /// <summary>
        /// The default value for this parameter is false.
        /// </summary>
        [Parameter]
        public bool RemarkRight { get; set; } = false;

        /// <summary>
        /// The default value for this parameter is false.
        /// </summary>
        [Parameter]
        public bool HideRemark { get; set; } = false;

        [Parameter]
        public bool Enabled { get; set; } = true;

        [Parameter]
        public string FocusID { get; set; } = null;

        [Parameter]
        public bool AutoFocus { get; set; } = false;

        [Parameter]
        public int AutoFocusPriority { get; set; } = 100;



    }

}


