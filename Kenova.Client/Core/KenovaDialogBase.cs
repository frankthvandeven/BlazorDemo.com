// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Kenova.Client;
using Kenova.Client.Components;
using Kenova.Client.SystemComponents;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Components
{
    // IMPORTANT
    //
    // Many of these names are used in code generation. Keep these in sync with the code generation code
    // See: src/Components/Analyzers/src/ComponentsApi.cs

    // Most of the developer-facing component lifecycle concepts are encapsulated in this
    // base class. The core components rendering system doesn't know about them (it only knows
    // about IComponent). This gives us flexibility to change the lifecycle concepts easily,
    // or for developers to design their own lifecycles as different base classes.

    /// <summary>
    /// Optional base class for components. Alternatively, components may
    /// implement <see cref="IComponent"/> directly.
    /// </summary>
    public abstract partial class KenovaDialogBase : IComponent, IHandleEvent, IHandleAfterRender
    {
        private readonly RenderFragment _renderFragment;
        private RenderHandle _renderHandle;
        private bool _initialized;
        private bool _hasNeverRendered = true;
        private bool _hasPendingQueuedRender;
        private bool _hasCalledOnAfterRender;

        /// <summary>
        /// Constructs an instance of <see cref="ComponentBase"/>.
        /// </summary>
        public KenovaDialogBase()
        {
             this.DebugName = this.GetType().Name;

            //if (KenovaClientConfig.Diagnostics) Console.WriteLine($"Run constructor for {this.GetType().Name}");

            _renderFragment = builder =>
            {
                //if (KenovaClientConfig.Diagnostics) Console.WriteLine($"🧱 KenovaDialogBase ({this.DebugName}) - BuildRenderTree");
                if (KenovaClientConfig.Diagnostics) Console.WriteLine($"📺 Rendering <KenovaDialogBase> for {this.DebugName}");


                _hasPendingQueuedRender = false;
                _hasNeverRendered = false;

                if (_kn_completed_initialization == false)
                {
                    builder.OpenComponent<ProgressDots>(9068);
                    builder.AddAttribute(9069, "Caption", _initlayer_progress_caption);
                    builder.CloseComponent();
                }
                else
                {
                    RenderFragment b = delegate (RenderTreeBuilder builder1)
                    {
                        // OpenRegion needed?
                        BuildRenderTree(builder1);
                        //__builder2.AddContent(6, this.ChildContent);
                    };

                    builder.OpenElement(9070, "div");
                    builder.AddAttribute(9071, "id", _container_id);
                    builder.AddAttribute(9072, "class", "kn-kenovadialogbase");
                    builder.AddAttribute(9073, "style", "width:100%;height:100%");

                    if (this.IsOpenedAsLayer())
                    {
                        // Focus trap
                        builder.OpenElement(2, "div");
                        builder.AddAttribute(3, "id", _focus_top_id);
                        builder.AddAttribute(4, "tabindex", "0");
                        //builder.AddAttribute(5, "class", "kn-focus-top");
                        builder.AddAttribute(6, "style", "pointer-events:none; position:fixed;");
                        builder.CloseElement();
                    }

                    //builder.AddAttribute<KeyboardEventArgs>(9077, "onkeydown", EventCallback.Factory.Create<KeyboardEventArgs>(this, new Action<KeyboardEventArgs>(this.Div_KeyDown)));

                    builder.OpenComponent<CascadingValue<KenovaDialogBase>>(9080);
                    builder.AddAttribute(9081, "Value", this);
                    builder.AddAttribute(9082, "IsFixed", true);
                    builder.AddAttribute(9083, "ChildContent", b);
                    builder.CloseComponent();

                    if (this.IsOpenedAsLayer())
                    {
                        // Focus trap
                        builder.OpenElement(2, "div");
                        builder.AddAttribute(3, "id", _focus_bottom_id);
                        builder.AddAttribute(4, "tabindex", "0");
                        //builder.AddAttribute(5, "class", "kn-focus-top");
                        builder.AddAttribute(6, "style", "pointer-events:none;position:fixed;");
                        builder.CloseElement();
                    }

                    builder.CloseElement();
                }
            };

            // The '@page' directive
            RouteAttribute attr = (RouteAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(RouteAttribute));

            if (attr != null)
            {
                _route_path = attr.Template;
            }

        }

        /// <summary>
        /// Renders the component to the supplied <see cref="RenderTreeBuilder"/>.
        /// </summary>
        /// <param name="builder">A <see cref="RenderTreeBuilder"/> that will receive the render output.</param>
        protected virtual void BuildRenderTree(RenderTreeBuilder builder)
        {
            // Developers can either override this method in derived classes, or can use Razor
            // syntax to define a derived class and have the compiler generate the method.

            // Other code within this class should *not* invoke BuildRenderTree directly,
            // but instead should invoke the _renderFragment field.
        }

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// </summary>
        protected virtual void OnDialogInitialized()
        {
        }

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        ///
        /// Override this method if you will perform an asynchronous operation and
        /// want the component to refresh when that operation is completed.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        protected virtual Task OnDialogInitializedAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in
        /// the render tree, and the incoming values have been assigned to properties.
        /// </summary>
        protected virtual void OnParametersSet()
        {
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in
        /// the render tree, and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        protected virtual Task OnParametersSetAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Notifies the component that its state has changed. When applicable, this will
        /// cause the component to be re-rendered.
        /// </summary>
        protected void StateHasChanged()
        {
            if (_hasPendingQueuedRender)
            {
                return;
            }

            if (_hasNeverRendered || ShouldRender() || _renderHandle.IsRenderingOnMetadataUpdate)
            {
                _hasPendingQueuedRender = true;

                try
                {
                    _renderHandle.Render(_renderFragment);
                }
                catch
                {
                    _hasPendingQueuedRender = false;
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a flag to indicate whether the component should render.
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldRender()
        {
            return true;
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">
        /// Set to <c>true</c> if this is the first time <see cref="OnAfterRender(bool)"/> has been invoked
        /// on this component instance; otherwise <c>false</c>.
        /// </param>
        /// <remarks>
        /// The <see cref="OnAfterRender(bool)"/> and <see cref="OnAfterRenderAsync(bool)"/> lifecycle methods
        /// are useful for performing interop, or interacting with values received from <c>@ref</c>.
        /// Use the <paramref name="firstRender"/> parameter to ensure that initialization work is only performed
        /// once.
        /// </remarks>
        protected virtual void OnAfterRender(bool firstRender)
        {
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered. Note that the component does
        /// not automatically re-render after the completion of any returned <see cref="Task"/>, because
        /// that would cause an infinite render loop.
        /// </summary>
        /// <param name="firstRender">
        /// Set to <c>true</c> if this is the first time <see cref="OnAfterRender(bool)"/> has been invoked
        /// on this component instance; otherwise <c>false</c>.
        /// </param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        /// <remarks>
        /// The <see cref="OnAfterRender(bool)"/> and <see cref="OnAfterRenderAsync(bool)"/> lifecycle methods
        /// are useful for performing interop, or interacting with values received from <c>@ref</c>.
        /// Use the <paramref name="firstRender"/> parameter to ensure that initialization work is only performed
        /// once.
        /// </remarks>
        protected virtual Task OnAfterRenderAsync(bool firstRender)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes the supplied work item on the associated renderer's
        /// synchronization context.
        /// </summary>
        /// <param name="workItem">The work item to execute.</param>
        protected Task InvokeAsync(Action workItem)
        {
            return _renderHandle.Dispatcher.InvokeAsync(workItem);
        }

        /// <summary>
        /// Executes the supplied work item on the associated renderer's
        /// synchronization context.
        /// </summary>
        /// <param name="workItem">The work item to execute.</param>
        protected Task InvokeAsync(Func<Task> workItem)
        {
            return _renderHandle.Dispatcher.InvokeAsync(workItem);
        }

        void IComponent.Attach(RenderHandle renderHandle)
        {
            // This implicitly means a ComponentBase can only be associated with a single
            // renderer. That's the only use case we have right now. If there was ever a need,
            // a component could hold a collection of render handles.
            if (_renderHandle.IsInitialized)
            {
                throw new InvalidOperationException($"The render handle is already set. Cannot initialize a {nameof(ComponentBase)} more than once.");
            }

            _renderHandle = renderHandle;
        }


        /// <summary>
        /// Sets parameters supplied by the component's parent in the render tree.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="Task"/> that completes when the component has finished updating and rendering itself.</returns>
        /// <remarks>
        /// <para>
        /// Parameters are passed when <see cref="SetParametersAsync(ParameterView)"/> is called. It is not required that 
        /// the caller supply a parameter value for all of the parameters that are logically understood by the component.
        /// </para>
        /// <para>
        /// The default implementation of <see cref="SetParametersAsync(ParameterView)"/> will set the value of each property
        /// decorated with <see cref="ParameterAttribute" /> or <see cref="CascadingParameterAttribute" /> that has
        /// a corresponding value in the <see cref="ParameterView" />. Parameters that do not have a corresponding value
        /// will be unchanged.
        /// </para>
        /// </remarks>
        public virtual Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            if (!_initialized)
            {
                _initialized = true;

                return RunInitAndSetParametersAsync();
            }
            else
            {
                return CallOnParametersSetAsync();
            }
        }

        private async Task RunInitAndSetParametersAsync()
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine($"🧱 KenovaDialogBase ({this.DebugName}) - beforeCallingOnDialogInitialized - ENTER");

            // added by fvv
            beforeCallingOnDialogInitialized();

            if (KenovaClientConfig.Diagnostics) Console.WriteLine($"🧱 KenovaDialogBase ({this.DebugName}) - beforeCallingOnDialogInitialized - EXIT");

            OnDialogInitialized();

            if (KenovaClientConfig.Diagnostics) Console.WriteLine($"🧱 KenovaDialogBase ({this.DebugName}) - OnDialogInitializedAsync - ENTER");

            var task = OnDialogInitializedAsync();

            if (task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Canceled)
            {
                // Call state has changed here so that we render after the sync part of OnInitAsync has run
                // and wait for it to finish before we continue. If no async work has been done yet, we want
                // to defer calling StateHasChanged up until the first bit of async code happens or until
                // the end. Additionally, we want to avoid calling StateHasChanged if no
                // async work is to be performed.

                // this one is needed
                StateHasChanged();

                try
                {
                    await task;
                }
                catch // avoiding exception filters for AOT runtime support
                {
                    // Ignore exceptions from task cancellations.
                    // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                    // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                    // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
                    if (!task.IsCanceled)
                    {
                        throw;
                    }
                }

                // Don't call StateHasChanged here. CallOnParametersSetAsync should handle that for us.
            }

            if (KenovaClientConfig.Diagnostics) Console.WriteLine($"🧱 KenovaDialogBase ({this.DebugName}) - OnDialogInitializedAsync - EXIT");


            // added by fvv
            _kn_completed_initialization = true;
            _initlayer_progress_caption = null; // forget the eventual progress caption

            await CallOnParametersSetAsync();
        }

        private async Task CallOnParametersSetAsync()
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine($"🧱 KenovaDialogBase ({this.DebugName}) - OnParametersSetAsync - ENTER");

            OnParametersSet();

            // fvv modified
            await OnParametersSetAsync();

            if (KenovaClientConfig.Diagnostics) Console.WriteLine($"🧱 KenovaDialogBase ({this.DebugName}) - OnParametersSetAsync - EXIT");

            // If no async work is to be performed, i.e. the task has already ran to completion
            // or was canceled by the time we got to inspect it, avoid going async and re-invoking
            // StateHasChanged at the culmination of the async work.

            // fvv commented out
            //var shouldAwaitTask = task.Status != TaskStatus.RanToCompletion &&
            //    task.Status != TaskStatus.Canceled;

            // We always call StateHasChanged here as we want to trigger a rerender after OnParametersSet and
            // the synchronous part of OnParametersSetAsync has run.
            StateHasChanged();

            //return shouldAwaitTask ? CallStateHasChangedOnAsyncCompletion(task) : Task.CompletedTask;
        }

        private async Task CallStateHasChangedOnAsyncCompletion(Task task)
        {
            try
            {
                await task;
            }
            catch // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations, but don't bother issuing a state change.
                if (task.IsCanceled)
                {
                    return;
                }

                throw;
            }

            StateHasChanged();
        }

        Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem callback, object arg)
        {
            var task = callback.InvokeAsync(arg);
            var shouldAwaitTask = task.Status != TaskStatus.RanToCompletion &&
                task.Status != TaskStatus.Canceled;

            // After each event, we synchronously re-render (unless !ShouldRender())
            // This just saves the developer the trouble of putting "StateHasChanged();"
            // at the end of every event callback.

            if (_suppressRender)
            {
                _suppressRender = false;
            }
            else
            {
                StateHasChanged();
            }

            return shouldAwaitTask ?
                CallStateHasChangedOnAsyncCompletion(task) :
                Task.CompletedTask;
        }

        /// <summary>
        /// Set to true when we are 100% sure the LayerDefinition has a reference
        /// to this class instance.
        /// </summary>
        //private bool _layerdefinition_has_component_reference = false;

        // Renderer source code:
        // https://github.com/dotnet/aspnetcore/blob/52eff90fbcfca39b7eb58baad597df6a99a542b0/src/Components/Components/src/RenderTree/Renderer.cs#L695


        Task IHandleAfterRender.OnAfterRenderAsync()
        {
            // added by fvv
            //if (_layerdefinition_has_component_reference == false)
            //{
            //    _layerdefinition_has_component_reference = true;

            //    Portal.UpdateBreadCrumb(this);
            //}

            // added by fvv
            // If an OnAfterRender is triggered during initialization, we simply ignore it.
            if (_kn_completed_initialization == false)
            {
                return Task.CompletedTask;
            }

            var firstRender = !_hasCalledOnAfterRender;

            _hasCalledOnAfterRender = true;

            return this.internalAfterRenderAsync(firstRender);

            // Note that we don't call StateHasChanged to trigger a render after
            // handling this, because that would be an infinite loop. The only
            // reason we have OnAfterRenderAsync is so that the developer doesn't
            // have to use "async void" and do their own exception handling in
            // the case where they want to start an async task.
        }

    }
}