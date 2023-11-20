using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Kenova.Client.SystemComponents
{
    /// <summary>
    /// A simplified CheckBox optimized for use in lists and datagrids.
    /// </summary>
    public partial class OnOffBox : KenovaComponentBase
    {
        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public bool Enabled { get; set; } = true;

        [Parameter]
        public Action OnOffBoxClicked { get; set; }

        private void Item_Div_Clicked(MouseEventArgs e)
        {
            if (!Enabled)
                return;

            OnOffBoxClicked?.Invoke();
        }

    }
}

