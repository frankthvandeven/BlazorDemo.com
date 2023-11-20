using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Linq.Expressions;
using System.Text;
using System.Timers;

// https://github.com/dotnet/aspnetcore/blob/master/src/Components/Web/src/Forms/InputBase.cs

namespace Kenova.Client.Components
{
    /// <summary>
    /// The box around an input field. Contains logic for displaying icons and icon-buttons left 
    /// and right of main content.
    /// </summary>
    public abstract partial class InputBase<TValue> : KenovaComponentBase, IAsyncDisposable, IRerender
    {
        public readonly string InputboxContainerId = KenovaClientConfig.GetUniqueElementID();

        private ComponentWingman<InputBase<TValue>> _wingman = new();

        private StringBuilder containerStyle = new StringBuilder(100);

        protected bool _act_as_displaycomponent = false;

        private System.Timers.Timer aTimer;

        protected FieldLink<TValue> FieldLink { get; private set; }

        [Parameter]
        public string InputElementId { get; set; } = KenovaClientConfig.GetUniqueElementID();

        [Parameter]
        public EventCallback<TValue> FieldChanged { get; set; }

        /// <summary>
        /// For example "() => this.Model.Name"
        /// </summary>
        [Parameter]
        public Expression<Func<TValue>> FieldExpression { get; set; }

        // In Blazor's binding system:
        // Value, ValueChanged and ValueExpression are a trinity.
        // https://stackoverflow.com/questions/60658450/when-to-use-valuechanged-and-valueexpression-in-blazor

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
        /// When set to true, a html-span containing text will be rendered instead of a html-input.
        /// This is for special-use cases only.
        /// The default value for this parameter is false.
        /// </summary>
        [Parameter]
        public bool NoInputElement { get; set; } = false;

        [Parameter]
        public bool ShowClearButton { get; set; } = false;

        [Parameter]
        public bool ShowDropdownButton { get; set; } = false;

        [Parameter]
        public bool ShowZoomButton { get; set; } = false;

        [Parameter]
        public bool ShowSearchIcon { get; set; } = false;

        [Parameter]
        public bool ShowClipboardButton { get; set; } = false;

        /// <summary>
        /// The text to display in an input field before data is entered by the user.
        /// </summary>
        [Parameter]
        public string Placeholder { get; set; } = null;

        [Parameter]
        /// <summary>
        /// Setting DropdownOpen to true, will display the dropdown button upside down.
        /// </summary>
        public bool DropdownOpen { get; set; } = false;

        [Parameter]
        public bool IsPassword { get; set; } = false;

        [Parameter]
        public EventCallback DropdownButtonClicked { get; set; }

        [Parameter]
        public EventCallback ZoomButtonClicked { get; set; }

        [Parameter]
        public UpdateKind FieldUpdate { get; set; } = UpdateKind.Unfocus;

        [Parameter]
        public int Milliseconds { get; set; } = 220;

        /// <summary>
        /// The default value for the AutoComplete parameter is "off"
        /// </summary>
        [Parameter]
        public string AutoComplete { get; set; } = "off";

        [Parameter]
        public string Suffix { get; set; } = null;

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




        protected override void OnInitialized()
        {
            if (LayerComponent == null)
                throw new InvalidOperationException("This component must be placed inside a LayerBaseComponent");

            if (FieldExpression == null)
                throw new InvalidOperationException("The 'FieldExpression' parameter was not set for this Input component");

            LayerComponent.RegisterComponent(this);

            FieldLink = new FieldLink<TValue>(this, FieldExpression, true, null, FieldChanged);

        }

        protected override void OnParametersSet()
        {
            ConvertValue2Text();
        }

        //public virtual void Dispose()
        //{
        //    _wingman.InvokeVoid("Stop");
        //    _wingman.Dispose();

        //    LayerComponent.UnregisterComponent(this);

        //    if (aTimer != null)
        //        aTimer.Dispose();

        //    FieldLink.Dispose();

        //    // Fix for a Blazor WASM bug.
        //    // This is a temporary solution for the "Unknown edit type: 0" in Blazor 3.2
        //    // The exception is thrown when a component that is removed from the DOM has (input?)focus.
        //    // Removing the focus before removing the document from the DOM solves the problem.
        //    JavaScriptCaller.KNChildUnfocus(InputboxContainerId);
        //}

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _wingman.InvokeVoidAsync("Stop");
            await _wingman.DisposeAsync();

            LayerComponent.UnregisterComponent(this);

            if (aTimer != null)
                aTimer.Dispose();

            FieldLink.Dispose();

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
                    await _wingman.InstantiateAsync(this, "InputBaseComponent");

                    await _wingman.InvokeVoidAsync("Start", InputboxContainerId, this.ShowZoomButton, this.ShowDropdownButton);

                    LayerComponent.RegisterFirstRenderComplete(this); // must be at the end of OnAfterRender
                }

            }
            finally
            {
                this.Mutex.Release();
            }

        }

        public void Rerender()
        {
            // The value probably was modified externally, update the input control's text with the external value.
            ConvertValue2Text();

            this.StateHasChanged();
        }

        private void TextBox_GotFocus(FocusEventArgs e)
        {
            //meritest
            //_suppressRender = true;

            if (FieldUpdate == UpdateKind.Timer)
            {
                aTimer = new System.Timers.Timer(Milliseconds);
                aTimer.Elapsed += OnUserInactive;
                aTimer.AutoReset = false;
            }

            this.OnGotFocus();
            //JavaScriptCaller.KNSelectAll(input_element_id);
        }

        private bool _suppressRender = false;

        protected override bool ShouldRender()
        {
            if (_suppressRender)
            {
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
            //_suppressRender = true;
            this.Text = (string)e.Value;

            if (this.FieldUpdate == UpdateKind.Timer)
            {
                aTimer.Stop(); // remove previous one (if there is one)

                if (this.Text.Length == 0)
                {
                    ConvertText2Value();
                }
                else
                {
                    aTimer.Start(); // reset the timer
                }

            }
            else if (this.FieldUpdate == UpdateKind.Keypress)
            {
                ConvertText2Value();
            }

            /*
            if (this.Model != null)
                if (this.Model.IsModelModified == false)
                    this.Model.SetModelModified();
            */

            OnInput(e);

            //if (_TextBox.FocusState != FocusState.Unfocused)
            //{
            //    _enterbutton.Visibility = Visibility.Visible;
            //    _cancelbutton.Visibility = Visibility.Visible;
            //}
        }

        /// <summary>
        /// This event handler method is called by the timer.
        /// </summary>
        private void OnUserInactive(Object source, ElapsedEventArgs e)
        {
            //Console.WriteLine($"💋 TIMER INACTIVITY {this.Text}");
            ConvertText2Value();
        }

        private void TextBox_LostFocus(FocusEventArgs e)
        {

            if (aTimer != null)
            {
                aTimer.Elapsed -= OnUserInactive;
                aTimer.Dispose();
                aTimer = null;
            }

            //_enterbutton.Visibility = Visibility.Collapsed;
            //_cancelbutton.Visibility = Visibility.Collapsed;

            // Try to convert the text to a (strongly typed) value.
            // However, if the text entered is invalid, this.Value
            // will not be updated.
            ConvertText2Value();

            // Convert the value back to text.
            // If the text was not updated in the previous step, this
            // will effectively "undo" the text entered to the previous value.
            ConvertValue2Text();

            this.OnLostFocus();

            //JavaScriptCaller.KNInputAfterBlur(input_element_id);
        }

        private void ClearText_Div_Clicked(MouseEventArgs e)
        {
            if (this.Enabled == false) return;

            this.Text = "";

            ConvertText2Value();

            _ = this.SetFocusAsync();
        }

        /// <summary>
        /// Divert the focus to the input element.
        /// </summary>
        private void Suffix_Div_Clicked(MouseEventArgs e)
        {
            if (this.Enabled == false) return;

            _ = this.SetFocusAsync();
        }

        /// <summary>
        /// This is the input component container
        /// </summary>
        private async void Container_Div_ClickedAsync()
        {
            if (this._act_as_displaycomponent == true) return;
            if (this.Enabled == false) return;
            if (this.NoInputElement == false) return;
            if (this.IsSingleActionButtonActive() == false) return;

            if (ShowZoomButton)
            {
                await OnZoomButtonClickedAsync();
                await ZoomButtonClicked.InvokeAsync();
            }
            else if (ShowDropdownButton)
            {
                await OnDropdownButtonClickedAsync();
                await DropdownButtonClicked.InvokeAsync();
            }

        }

        private void Copy_Div_Clicked(MouseEventArgs e)
        {
            if (!Enabled)
                return;

            _ = Clipboard.CopyTextToClipboardAsync(this.Text);
        }

        [JSInvokable]
        public async ValueTask KeyZoomPressed()
        {
            if (this.Enabled == false)
                return;

            await OnZoomButtonClickedAsync();
            await ZoomButtonClicked.InvokeAsync();
        }

        [JSInvokable]
        public async ValueTask KeyDropdownPressed()
        {
            if (this.Enabled == false)
                return;

            await OnDropdownButtonClickedAsync();
            await DropdownButtonClicked.InvokeAsync();
        }

        private async void Zoom_Div_ClickedAsync()
        {
            if (this.Enabled == false) return;

            if (NoInputElement == false) // the Dropdown arrow click is handled in Container_Div_Clicked
            {
                await OnZoomButtonClickedAsync();
                await ZoomButtonClicked.InvokeAsync();
            }
        }

        /// <summary>
        /// This is the dropdown arrow.
        /// </summary>
        private async ValueTask Dropdown_Div_ClickedAsync()
        {
            if (this.Enabled == false) return;

            if (NoInputElement == false) // the Dropdown arrow click is handled in Container_Div_Clicked
            {
                await OnDropdownButtonClickedAsync();
                await DropdownButtonClicked.InvokeAsync();
            }
        }

        private bool IsSingleActionButtonActive()
        {
            int count = 0;

            if (ShowZoomButton) count++;
            if (ShowDropdownButton) count++;

            return count == 1;
        }

        protected abstract void ConvertText2Value();

        protected abstract void ConvertValue2Text();

        protected virtual void OnGotFocus()
        {
        }

        protected virtual void OnLostFocus()
        {
        }

        protected virtual void OnInput(ChangeEventArgs e)
        {
        }

        /// <summary>
        /// Called when the Clear text button was clicked.
        /// </summary>
        //protected virtual void OnClearTextButtonClicked()
        //{
        //}

        /// <summary>
        /// Called when the Dropdown button was clicked.
        /// </summary>
        public virtual ValueTask OnDropdownButtonClickedAsync()
        {
            return ValueTask.CompletedTask; // new ValueTask();
        }

        /// <summary>
        /// Called when the Zoom button was clicked.
        /// </summary>
        public virtual ValueTask OnZoomButtonClickedAsync()
        {
            return ValueTask.CompletedTask; // new ValueTask();
        }

    }

    public enum UpdateKind
    {
        Unfocus,
        Keypress,
        Timer
    }

}





