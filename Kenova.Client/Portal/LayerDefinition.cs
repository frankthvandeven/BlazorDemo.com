using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kenova.Client.Components
{

    public sealed class LayerDefinition<TComponent> : LayerDefinition where TComponent : KenovaDialogBase
    {
        public LayerDefinition()
        {
            this.ComponentType = typeof(TComponent);
        }

        public object this[Expression<Func<TComponent, object>> expr]
        {
            set
            {
                string fieldname = LambdaHelper.GetMemberName(expr.Body);
                _attributes[fieldname] = value; // Will replace value if key already exists
            }
            get
            {
                return null;
            }
        }

        public void Parameter(Expression<Func<TComponent, object>> expr, object value)
        {
            string fieldname = LambdaHelper.GetMemberName(expr.Body);
            _attributes[fieldname] = value; // Will replace value if key already exists
        }

    }

    public class LayerDefinition
    {
        private TaskCompletionSource<LayerResult> _TCS = null;

        // Layer fixed coordinates

        public double LayerTop { get; internal set; } = -1;
        public double LayerBottom { get; internal set; } = -1;
        public double LayerLeft { get; internal set; } = -1;
        public double LayerRight { get; internal set; } = -1;

        // Absolute maximums

        public double LayerMaxHeight { get; internal set; } = -1;
        public double LayerMaxWidth { get; internal set; } = -1;

        // Content inside layer

        /// <summary>
        /// The ContentSuggestedWidth is set to the owning element width. It is a suggestion to use this value.
        /// Value -1 means the property is not set.
        /// </summary>
        public double ContentSuggestedWidth { get; internal set; } = -1;

        /// <summary>
        /// The ContentSuggestedHeight is a suggestion. Value -1 means the property is not set.
        /// </summary>
        public double ContentSuggestedHeight { get; internal set; } = -1;

        /// <summary>
        /// The MaxHeight is an absolute and must be respected.
        /// </summary>
        public double ContentMaxHeight { get; internal set; } = -1;

        /// <summary>
        /// The MaxWidth is an absolute and must be respected.
        /// </summary>
        public double ContentMaxWidth { get; internal set; } = -1;


        /// <summary>
        /// Optionally force a width setting for the layer container.
        /// The default value is -1, meaning the property will not be set.
        /// </summary>
        public double Width { get; set; } = -1;

        // The rest

        private Type _component_type;
        private string _key = Guid.NewGuid().ToString("N"); // N = 32 characters, digits only
        protected Dictionary<string, object> _attributes = new Dictionary<string, object>();
        private string _owner_id = null;

        /// <summary>
        /// Called after a layer was closed.
        /// </summary>
        public Action<LayerResult> AfterClosed = null;
        public Action<object> ValueReceived = null;

        /// <summary>
        /// The id of the element that owns the dropdown. The dropdown will automatically
        /// be positioned directly above or below the owning element.
        /// </summary>
        public string OwnerID
        {
            get { return _owner_id; }
            set { _owner_id = value; }
        }

        public readonly string ContainerID = KenovaClientConfig.GetUniqueElementID();

        //public WidthSetterHelper SetWidth()
        //{
        //    return new WidthSetterHelper(this);
        //}

        //public HeightSetterHelper SetHeight()
        //{
        //    return new HeightSetterHelper(this);
        //}

        /// <summary>
        /// The default value of this property is LayerKind.Modal
        /// </summary>
        public LayerKind Kind { get; set; } = LayerKind.Modal;

        /// <summary>
        /// The default value of this property is DropdownPosition.Below
        /// </summary>
        public DropdownPosition DropdownPosition { get; internal set; } = DropdownPosition.Below;

        /// <summary>
        /// Only for LayerKind.Dropdown and LayerKind.DropdownBalloon 
        /// </summary>
        public double OwnerWidth { get; internal set; } = -1;

        public string LayerStyle { get; internal set; } = "";

        /// <summary>
        /// The type of the component to open as a layer.
        /// </summary>
        public Type ComponentType
        {
            get { return _component_type; }
            set { _component_type = value; }
        }

        public string Key
        {
            get { return _key; }
        }

        public object this[string parameter_name]
        {
            set
            {
                _attributes[parameter_name] = value; // Will replace value if key already exists
            }

        }

        public void Parameter(string parameter_name, object value)
        {
            _attributes[parameter_name] = value; // Will replace value if key already exists
        }

        internal Dictionary<string, object> Attributes
        {
            get { return _attributes; }
        }

        private KenovaDialogBase _ref = null;

        public KenovaDialogBase ComponentReference
        {
            get { return _ref; }
            internal set
            {
                _ref = value;
            }
        }

        /// <summary>
        /// Display the compoment as a layer. Calculate layer coordinates, add the layerdefinition to
        /// the layer stack. For a dropdown layer, events will be hooked up to monitor for clicks outside the layer area.
        /// Remove all current tabbable elements from the tab order.
        /// </summary>
        public async ValueTask OpenNonBlockingAsync()
        {
            if (this.ComponentType == null)
                throw new InvalidOperationException("Property ComponentType not set");

            // Save the currently focussed element, and then unfocus that element.
            await JavaScriptCaller.KNSaveCurrentFocusAsync();

            if (Kind == LayerKind.TopmostModelessRight)
            {
                await LayerManager.CloseNonSolidsAsync();
            }
            else if (Kind == LayerKind.ModelessRight)
            {
                await LayerManager.CloseNonSolidsAsync();
            }

            var css_props = new List<string>();

            if (Kind == LayerKind.Dropdown || Kind == LayerKind.DropdownBalloon)
            {
                await calculateDropdownCoordinatesAsync(css_props);
            }
            else if (Kind == LayerKind.ModelessRight || Kind == LayerKind.TopmostModelessRight)
            {
                if (this.Width != -1)
                    css_props.Add($"width:{this.Width.ToPixels()}");
            }

            this.LayerStyle = string.Join(";", css_props);

            LayerManager.LayerStack.Push(this);

            await JavaScriptCaller.KNTriggerIgnoreScrollResizeEventsAsync();

            Portal.Refresh();

            if (this.Kind == LayerKind.Dropdown || this.Kind == LayerKind.DropdownBalloon)
            {
                if (string.IsNullOrEmpty(this.OwnerID))
                    throw new InvalidOperationException($"The OwnerID for the LayerDefinition is not set.");
            }

            // A dropdown means:
            // 1. A mouse click outside the panel will close the dropdown (ContainerID).
            // 2. If the OwnerID is set, a click on that owning element will not close the dropdown.

            bool is_dropdown = (this.Kind == LayerKind.Dropdown ||
                                this.Kind == LayerKind.DropdownBalloon ||
                                this.Kind == LayerKind.FlyoutLeft);

            await JavaScriptCaller.KNAddLayerAsync(is_dropdown, this.OwnerID, this.ContainerID);

        }

        /// <summary>
        /// The same as OpenNonBlockingAsync() but will await for the dialog to be closed.
        /// The task will complete when the layer is closed. 
        /// For example as the result of a user clicking on a button.
        /// </summary>
        public async Task<LayerResult> OpenThenWaitForCloseAsync()
        {
            this._TCS = new TaskCompletionSource<LayerResult>();

            await OpenNonBlockingAsync();

            LayerResult result = await this._TCS.Task;

            return result;
        }

        /// <summary>
        /// Returns true when the layered component is rendered.
        /// </summary>
        public bool IsOpen()
        {
            foreach (var ld in LayerManager.LayerStack)
            {
                if (ld.Equals(this))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true when the layered component is rendered as the topmost layer.
        /// </summary>
        public bool IsActive
        {
            get { return LayerManager.LayerStack.Peek().Equals(this); }
        }

        private async ValueTask calculateDropdownCoordinatesAsync(List<string> css_props)
        {
            if (this.OwnerID == null)
                throw new ArgumentNullException("OwnerID");

            BoundingClientRect rect = await JavaScriptCaller.KNGetBoundingClientRectByIdAsync(this.OwnerID);

            this.OwnerWidth = rect.Width;

            // All double vars stand for pixels (px).
            double window_innerwidth = await JavaScriptCaller.KNGetWindowInnerWidthAsync();
            double window_innerheight = await JavaScriptCaller.KNGetWindowInnerHeightAsync();

            // Calculate width related properties

            double space_left_of_owner = rect.Right;
            double space_right_of_owner = window_innerwidth - rect.Left;

            if (space_right_of_owner >= space_left_of_owner)
            {
                this.LayerLeft = rect.Left;
                this.LayerMaxWidth = space_right_of_owner;
            }
            else
            {
                this.LayerRight = window_innerwidth - rect.Right;
                this.LayerMaxWidth = space_left_of_owner;
            }

            this.LayerMaxWidth -= 4; // 4px margin

            // Calculate height related properties

            double space_above_owner = rect.Top;
            double space_below_owner = window_innerheight - rect.Bottom;

            if (space_below_owner >= space_above_owner)
            {
                this.LayerMaxHeight = space_below_owner;
                this.DropdownPosition = DropdownPosition.Below;
                this.LayerTop = rect.Bottom;

                if (this.Kind == LayerKind.DropdownBalloon)
                {
                    this.LayerTop += 12.0d; // Offset for the balloon pointer.
                    this.LayerMaxHeight -= 12.0d;
                }

                this.LayerMaxHeight -= 4; // 4px margin

            }
            else
            {
                this.LayerMaxHeight = space_above_owner;
                this.DropdownPosition = DropdownPosition.Above;
                this.LayerBottom = window_innerheight - rect.Top;

                if (this.Kind == LayerKind.DropdownBalloon)
                {
                    this.LayerBottom += 12.0d; // Offset for the balloon pointer.
                    this.LayerMaxHeight -= 12.0d;
                }

                this.LayerMaxHeight -= 42; // Deduct the space for the TopBar (blue) of 40px plus 2px margin

            }

            if (this.LayerLeft != -1)
                css_props.Add($"left:{this.LayerLeft.ToPixels()}");

            if (this.LayerRight != -1)
                css_props.Add($"right:{this.LayerRight.ToPixels()}");

            if (this.LayerTop != -1)
                css_props.Add($"top:{this.LayerTop.ToPixels()}");

            if (this.LayerBottom != -1)
                css_props.Add($"bottom:{this.LayerBottom.ToPixels()}");

            css_props.Add("width:auto");
            css_props.Add("height:auto");

            this.ContentSuggestedHeight = -1;
            this.ContentSuggestedWidth = this.OwnerWidth - 2; // 2 is the 2 times 1 pixel border width.
            this.ContentMaxHeight = this.LayerMaxHeight - 2; // 2 is the 2 times 1 pixel border width.
            this.ContentMaxWidth = this.LayerMaxWidth - 2; // 2 is the 2 times 1 pixel border width.

        }

        /* CLOSE FUNCTIONALITY */

        public async Task CloseOkAsync(object returnvalue = null)
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("LayerDefinition CloseOK");

            if (LayerManager.LayerStack.Count == 0)
                throw new InvalidOperationException("Layer.Close: There is no layer to close.");

            if (this.IsActive == false)
                throw new ArgumentOutOfRangeException("ld", "LayerManager.Close Error. Only the topmost layer can be closed.");

            // This is a temporary solution for the "Unknown edit type: 0" in Blazor 3.2
            // The exception is thrown when a component that is removed from the DOM has (input?)focus.
            // Removing the focus before removing the document from the DOM solves the problem.
            // (NOTE: call to KNChildUnfocus was moved to InputBase.cs-Dispose)           
            //JavaScriptCaller.KNChildUnfocus(ld.ContainerID);

            await JavaScriptCaller.KNRemoveLayerAsync();

            LayerManager.LayerStack.Pop();

            Portal.Refresh();

            var layerResult = new LayerResult
            {
                Cancelled = false,
                Aborted = false,
                Data = returnvalue
            };

            if (this.AfterClosed != null)
                this.AfterClosed.Invoke(layerResult);

            if (this._TCS != null)
                this._TCS.SetResult(layerResult);

        }

        public async Task CloseCancelAsync()
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("LayerDefinition CloseCancelAsync");

            //return privateCloseAsync(CloseReason.Cancelled, null);

            if (LayerManager.LayerStack.Count == 0)
                throw new InvalidOperationException("Layer.Close: There is no layer to close.");

            if (this.IsActive == false)
                throw new ArgumentOutOfRangeException("ld", "LayerManager.Close Error. Only the topmost layer can be closed.");

            await JavaScriptCaller.KNRemoveLayerAsync();

            LayerManager.LayerStack.Pop();

            Portal.Refresh();

            var layerResult = new LayerResult
            {
                Cancelled = true,
                Aborted = false,
                Data = null
            };

            if (this.AfterClosed != null)
                this.AfterClosed.Invoke(layerResult);

            if (this._TCS != null)
                this._TCS.SetResult(layerResult);


        }

        /// <summary>
        /// Similar to a CloseCancelAsync, but does not call StateHasChanged, and sets LayerResult.Aborted to true.
        /// </summary>
        internal async Task AbortAsync()
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("LayerDefinition AbortAsync");

            if (LayerManager.LayerStack.Count == 0)
                throw new InvalidOperationException("Layer.AbortAsync: There is no layer to abort.");

            if (this.IsActive == false)
                throw new ArgumentOutOfRangeException("ld", "LayerManager.AbortAsync Error. Only the topmost layer can be aborted.");

            await JavaScriptCaller.KNRemoveLayerAsync();

            LayerManager.LayerStack.Pop();

            var layerResult = new LayerResult
            {
                Cancelled = true,
                Aborted = true,
                Data = null
            };

            if (this.AfterClosed != null)
                this.AfterClosed.Invoke(layerResult);

            if (this._TCS != null)
                this._TCS.SetResult(layerResult);

        }


        /// <summary>
        /// Toggle between layer open and layer closed.
        /// </summary>
        public async ValueTask ToggleOpenCloseAsync()
        {
            if (this.IsOpen())
            {
                await this.CloseCancelAsync();
            }
            else
            {
                await this.OpenNonBlockingAsync();
            }
        }


        public void SendValue(object returnvalue)
        {
            this.ValueReceived?.Invoke(returnvalue);
        }

        /// <summary>
        /// The breadcrumb text as stored inside the LayerBaseComponent
        /// </summary>
        internal string Breadcrumb
        {
            get
            {
                if (ComponentReference == null)
                    return null;

                return ComponentReference.Breadcrumb;
            }
        }

    }

    public enum LayerKind
    {
        Modal = 0,
        ModalWindow = 10,

        /// <summary>
        /// The OwnerID must be set for a Dropdown.
        /// </summary>
        Dropdown = 20,

        /// <summary>
        /// The OwnerID must be set for a DropdownBalloon.
        /// </summary>
        DropdownBalloon = 30,

        ModelessRight = 40,

        /// <summary>
        /// Exactly the same a ModelessRight, but this one does not allow layers to opened on top.
        /// TopmostModelesssRight is used by Kenova for the settings and notifications panels of the portal's topbar.
        /// </summary>
        TopmostModelessRight = 42,

        /// <summary>
        /// FlyoutLeft is a dropdown but without OwnerID.
        /// </summary>
        FlyoutLeft = 50
    }

    /// <summary>
    /// Used for layer kind Dropdown and DropdownBalloon.
    /// </summary>
    public enum DropdownPosition
    {
        Below,
        Above
    }

    public enum CloseReason
    {
        Ok,
        Cancelled
    }
}
