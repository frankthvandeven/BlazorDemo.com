using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Linq.Expressions;
using System.Text;
using System.Timers;

// https://stephanwagner.me/auto-resizing-textarea-with-vanilla-javascript

namespace Kenova.Client.Components
{
    public partial class InputMultiline : KenovaComponentBase, IAsyncDisposable, IRerender
    {
        private ComponentWingman<InputMultiline> _wingman = new();

        public readonly string InputboxContainerId = KenovaClientConfig.GetUniqueElementID();
        public readonly string InputElementId = KenovaClientConfig.GetUniqueElementID();

        private StringBuilder containerStyle = new StringBuilder(100);

        protected bool _act_as_displaycomponent = false;

        private System.Timers.Timer aTimer;

        protected FieldLink<string> FieldLink { get; private set; }

        [Parameter]
        public EventCallback<string> FieldChanged { get; set; }

        /// <summary>
        /// For example "() => this.Model.Name"
        /// </summary>
        [Parameter]
        public Expression<Func<string>> FieldExpression { get; set; }

        /// <summary>
        /// The Text as edited in the input field. Updated on each keypress.
        /// </summary>
        protected string Text { get; set; } = "";

        [Parameter]
        public string AdditionalStyle { get; set; } = null;

        [Parameter]
        public string Caption { get; set; } = null;

        [Parameter]
        public double Width { get; set; } = -1;

        [Parameter]
        public double MaxWidth { get; set; } = -1;

        [Parameter]
        public double TextMaxHeight { get; set; } = -1;

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

        [Parameter]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// When set to true, the readonly attribute on the html-input element will be set.
        /// Text can be selected, but not modified.
        /// </summary>
        [Parameter]
        public bool ReadOnly { get; set; } = false;

        /// <summary>
        /// The text to display in an input field before data is entered by the user.
        /// </summary>
        [Parameter]
        public string Placeholder { get; set; } = null;

        [Parameter]
        public UpdateKind FieldUpdate { get; set; } = UpdateKind.Unfocus;

        [Parameter]
        public int Milliseconds { get; set; } = 220;

        [Parameter]
        public string AutoComplete { get; set; } = null;

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
        /// <summary>
        /// The number of rows to reserve. Determinines the (minimum) height of the component.
        /// The default value is 2.
        /// </summary>
        [Parameter]
        public int Rows { get; set; } = 2;

        protected override void OnInitialized()
        {
            if (LayerComponent == null)
                throw new InvalidOperationException("This component must be placed inside a LayerBaseComponent");

            LayerComponent.RegisterComponent(this);

            if (FieldExpression == null)
                throw new Exception("The 'FieldExpression' parameter was not set for this component.");

            FieldLink = new FieldLink<string>(this, FieldExpression, true, null, FieldChanged);

        }
        protected override void OnParametersSet()
        {
            this.Text = FieldLink.Value;
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            LayerComponent.UnregisterComponent(this);

            await _wingman.InvokeVoidAsync("Stop");

            if (aTimer != null)
            {
                aTimer.Elapsed -= OnUserInactive;
                aTimer.Dispose();
            }

            FieldLink.Dispose();

            await _wingman.DisposeAsync();

            // Fix for a Blazor WASM bug.
            // This is a temporary solution for the "Unknown edit type: 0" in Blazor 3.2
            // The exception is thrown when a component that is removed from the DOM has (input?)focus.
            // Removing the focus before removing the document from the DOM solves the problem.
            await JavaScriptCaller.KNChildUnfocusAsync(InputboxContainerId);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await this.Mutex.WaitAsync();

                if (firstRender)
                {
                    await _wingman.InstantiateAsync(this,"InputMultilineComponent");

                    await _wingman.InvokeVoidAsync("Start", InputElementId);
                }

                if (_value_externally_updated)
                {
                    _value_externally_updated = false;
                    await _wingman.InvokeVoidAsync("ResetHeight");
                }

                if (firstRender)
                {
                    LayerComponent.RegisterFirstRenderComplete(this); // must be at the end of OnAfterRender
                }

            }
            finally
            {
                this.Mutex.Release();
            }

        }

        private bool _value_externally_updated;

        public void Rerender()
        {
            // The value probably was modified externally, update the input control's text with the external value.
            this.Text = FieldLink.Value;

            _value_externally_updated = true;

            this.StateHasChanged();
        }

        private void TextBox_GotFocus(FocusEventArgs e)
        {
            //meritest
            _suppressRender = true;

            if (FieldUpdate == UpdateKind.Timer)
            {
                aTimer = new System.Timers.Timer(Milliseconds);
                aTimer.Elapsed += OnUserInactive;
                aTimer.AutoReset = false;
            }

            //JavaScriptCaller.KNSelectAll(input_element_id);
        }

        private bool _suppressRender = false;

        protected override bool ShouldRender()
        {
            if (_suppressRender)
            {
                //if (KenovaClientConfig.Diagnostics) Console.WriteLine("SUPPRESSED ONCE");
                _suppressRender = false;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Called for every keypress. 
        /// </summary>
        private void TextBox_OnInput(ChangeEventArgs e)
        {
            this.Text = (string)e.Value;

            if (this.FieldUpdate == UpdateKind.Timer)
            {
                aTimer.Stop(); // remove previous one (if there is one)

                if (this.Text.Length == 0)
                {
                    FieldLink.Value = this.Text;
                }
                else
                {
                    aTimer.Start(); // reset the timer
                }

            }
            else if (this.FieldUpdate == UpdateKind.Keypress)
            {
                FieldLink.Value = this.Text;
            }

        }

        /// <summary>
        /// This event handler method is called by the timer.
        /// </summary>
        private void OnUserInactive(Object source, ElapsedEventArgs e)
        {
            //if (KenovaClientConfig.Diagnostics) Console.WriteLine($"💋 TIMER INACTIVITY {this.Text}");
            FieldLink.Value = this.Text;
        }

        private void TextBox_LostFocus(FocusEventArgs e)
        {
            //meritest
            //_suppressRender = true;


            if (aTimer != null)
            {
                aTimer.Dispose();
                aTimer = null;
            }

            FieldLink.Value = this.Text;

            //JavaScriptCaller.KNInputAfterBlur(input_element_id);
        }

        /*
        public void SetFocus()
        {
            //if (KenovaClientConfig.Diagnostics) Console.WriteLine("SETTING FOCUS TO " + InputElementId);
            JavaScriptCaller.KNFocusAsync(InputElementId);
        }
        */

    }
}
