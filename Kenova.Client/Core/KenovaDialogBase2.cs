// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

/* The ComponentBase has been modified as follows:
 * 1. Added: bool _kn_completed_initialization = false.
 * 2. The bool is set to true after OnInitializedAsync has been awaited.
 * 3. In the constructor, the renderfragment is modified to display the ProgressDots component while the bool is false.
 * 4. In the constructor, the renderfragment is modified to set a cascading value.
 * 5. Added the _wingman field.
 * 6. Initialize the _wingman in RunInitAndSetParametersAsync.
 * 6. Added: IDisposable, and dispose the _wingman there.
 * 7. Renamed: OnInitialized to OnLayerInitialized.
 * 8. Renamed: OnInitializedAsync to OnLayerInitializedAsync.
 * 9. Added: protected virtual void OnDispose()
 * 9. Added: CloseOk(object returnvalue = null)
 * 10. Added: CloseCancel(object returnvalue = null)
 * 11. Added: SendValue(LayerDefinition ld, object returnvalue)
 * 12. Added: PerformAutofocus call to IHandleAfterRender.OnAfterRenderAsync
 * 13. Added: Portal.RegisterLayerComponent(this) and Portal.UnregisterLayerComponent(this)
 * 14. Added: private KenovaDialogBase ParentLayerComponent. 
 */

using Kenova.Client;
using Kenova.Client.Components;
using Kenova.Client.Util;
using System;
using System.Threading;

namespace Microsoft.AspNetCore.Components
{
    public abstract partial class KenovaDialogBase : IAsyncDisposable
    {
        private readonly string _container_id = KenovaClientConfig.GetUniqueElementID();
        private readonly string _focus_top_id = KenovaClientConfig.GetUniqueElementID();
        private readonly string _focus_bottom_id = KenovaClientConfig.GetUniqueElementID();

        private LayerDefinition _layer_definition;
        bool _kn_completed_initialization = false;
        private ComponentWingman<KenovaDialogBase> _wingman = new();
        private string _route_path;
        private string _breadcrumb;

        // used in the IHandleEvent.HandleEventAsync() method.
        private bool _suppressRender = false;

        private string _initlayer_progress_caption = null;

        protected SemaphoreSlim Mutex { get; } = new SemaphoreSlim(1);


        [CascadingParameter]
        private KenovaDialogBase ParentLayerComponent { get; set; }

        /// <summary>
        /// If the component is opened as a layer, this parameter will contain the LayerDefinition data.
        /// Do not modify. For internal use.
        /// </summary>
        [Parameter]
        public LayerDefinition LayerDefinition { get; set; }

        [Parameter]
        public bool IsOpenedAsRoutedPage { get; set; } = false;

        /// <summary>
        /// The caption to display while OnLayerInitialized is being executed.
        /// Set to null to hide an already visible caption. 
        /// The caption text is rendered instantly.
        /// </summary>
        protected string ProgressCaption
        {
            get { return _initlayer_progress_caption; }
            set
            {
                if (_kn_completed_initialization)
                    return;

                if (_initlayer_progress_caption != value)
                    _initlayer_progress_caption = value;

                StateHasChanged();
            }
        }

        private void beforeCallingOnDialogInitialized()
        {
            // Make an internal copy, as the [Parameter] must be public and can be modified.
            _layer_definition = this.LayerDefinition;

            if (_layer_definition != null)
            {
                _layer_definition.ComponentReference = this;
            }

            if (this.ParentLayerComponent != null)
            {
                throw new InvalidOperationException($"Dialog component {this.GetType().Name} detected it is placed on another dialog. A KenovaDialogBase can not be nested with a KenovaDialogBase");
            }

            //Portal.RegisterLayerComponent(this);

            //var ld = LayerManager.FindLayerDefinition(this);
            //if (KenovaClientConfig.Diagnostics) Console.WriteLine($"Tried to look up the LayerDefinition for {this.GetType().Name} and the result is {(ld == null ? "not found" : "found")}");
        }

        protected bool IsOpenedAsLayer()
        {
            return _layer_definition != null;
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine($"🧱 KenovaDialogBase ({this.DebugName}) - DisposeAsync - ENTER");

            //await this.Mutex.WaitAsync();

            this.OnDispose();

            var task = this.OnDisposeAsync();

            var shouldAwaitTask = task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Canceled;

            if (shouldAwaitTask)
            {
                await task;
            }

            if (_js_started)
            {
                await _wingman.InvokeVoidAsync("Stop"); // stop any event listeneners
            }

            if (_wingman != null)
            {
                await _wingman.DisposeAsync();
            }

            //Portal.UnregisterLayerComponent(this);

            if (KenovaClientConfig.Diagnostics) Console.WriteLine($"🧱 KenovaDialogBase ({this.DebugName}) - DisposeAsync - EXIT");

        }

        protected virtual void OnDispose()
        {
        }

        protected virtual Task OnDisposeAsync()
        {
            return Task.CompletedTask;
        }

        //private const string LAYER_NOT_FOUND = "Component {0} is not registered in the LayerManager";
        private const string NOT_OPENED_AS_LAYER = "Component {0} was not opened as a layer. Method {1} cannot be called.";

        protected Task CloseOkAsync(object returnvalue = null)
        {
            if (_layer_definition == null)
                throw new InvalidOperationException(string.Format(NOT_OPENED_AS_LAYER, this.GetType().Name, nameof(CloseOkAsync)));

            return _layer_definition.CloseOkAsync(returnvalue);
        }

        protected Task CloseCancelAsync()
        {
            if (_layer_definition == null)
                throw new InvalidOperationException(string.Format(NOT_OPENED_AS_LAYER, this.GetType().Name, nameof(CloseCancelAsync)));

            return _layer_definition.CloseCancelAsync();

        }

        protected void SendValue(object returnvalue)
        {
            if (_layer_definition == null)
                throw new InvalidOperationException(string.Format(NOT_OPENED_AS_LAYER, this.GetType().Name, nameof(SendValue)));

            _layer_definition.SendValue(returnvalue);
        }

        /// <summary>
        /// The route path parameter of the '@page' directive (aka the [Route("/")] attribute).
        /// Returns null if the attribute was not set.
        /// For the application's home page, the route parameter is "/"
        /// </summary>
        /// <returns></returns>
        protected internal string GetRoutePath()
        {
            return _route_path;
        }

        public string Breadcrumb
        {
            get { return _breadcrumb; }
            set
            {
                _breadcrumb = value;

                //if (_layerdefinition_has_component_reference == true)
                Portal.UpdateBreadCrumbFor(this);
            }
        }

        /// <summary>
        /// The id of the root div element.
        /// </summary>
        //internal string ContainerID
        //{
        //    get { return _container_id; }
        //}

    }
}