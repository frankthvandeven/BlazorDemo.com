﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Kenova.Client.Components
{
    public partial class OptionButton : KenovaComponentBase, IDisposable
    {
        public readonly string ContainerID = KenovaClientConfig.GetUniqueElementID();


        [CascadingParameter]
        private OptionGroup Parent { get; set; }

        [Parameter]
        public string Identifier { get; set; } = KenovaClientConfig.GetUniqueElementID();

        [Parameter]
        public bool Enabled { get; set; } = true;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            if (Parent == null)
                throw new ArgumentNullException(nameof(Parent), "OptionButton must exist within an OptionGroup");

            Parent.Register(this);

        }

        public void Dispose()
        {
            Parent.Unregister(this);
        }

        private void Item_Div_Clicked(MouseEventArgs e)
        {
            if (Enabled == false)
                return;

            Parent.SelectedFieldLink.Value = Identifier;
            Parent.Rerender();
        }



    }
}

