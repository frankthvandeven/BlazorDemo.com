using Kenova.Client;
using Kenova.Client.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Components
{
    public abstract partial class KenovaDialogBase
    {
        private string DebugName;

        private List<RegisteredComponentInfo> _components = new();

        internal void RegisterComponent(IKenovaComponent component)
        {
            if (component == null)
                throw new ArgumentNullException("component");

            var info = new RegisteredComponentInfo(component);

            _components.Add(info);

        }

        /// <summary>
        /// Kenova needs this call to make sure the LayerComponentBase.OnAfterRender method is executed last.
        /// </summary>
        internal void RegisterFirstRenderComplete(IKenovaComponent component)
        {
            if (component == null)
                throw new ArgumentNullException("component");

            var info = _components.Find(p => p.Component.Equals(component));

            info.RegisterFirstRenderCompleted = true;
            info.TCS.SetResult();
        }

        internal void UnregisterComponent(IKenovaComponent component)
        {
            if (component == null)
                throw new ArgumentNullException("component");

            var info = _components.Find(p => p.Component.Equals(component));

            if (info.RegisterFirstRenderCompleted == false)
            {
                throw new InvalidOperationException($"Forgot to call LayerComponentBase.RegisterFirstRenderComplete() in the OnAfterRender(firstRender) method of {component.GetType().FullName}. App is unreliable.");
            }

            _components.Remove(info);
        }


        public async ValueTask SetFocusAsync(string focusID)
        {
            if (focusID == null)
                throw new ArgumentNullException("focusID");

            foreach (var item in _components)
            {
                var component = item.Component;

                bool didfocus = await component.PerformFocusAsync(focusID);

                if (didfocus)
                    break;

            }
        }

        private TaskCompletionSource _refresh_tcs = null;

        internal Task RefreshAsync()
        {
            _refresh_tcs = new TaskCompletionSource();

            this.StateHasChanged(); // This method signals the Blazor renderer, and does not block

            return _refresh_tcs.Task;
        }

        private bool _js_started = false;

        private bool FirstRenderAwait_Complete = false;

        /* Understanding the order of events:
         * 
         * Index                - OnInitialized
         *       SurveyPrompt   - OnInitialized
         *       SurveyPrompt2  - OnInitialized
         *           Nested     - OnInitialized
         * Index                - OnAfterrender
         *       SurveyPrompt   - OnAfterrender True
         *       SurveyPrompt2  - OnAfterrender True
         * Nested               - OnAfterrender True
         *
         * Guaranteed: ALL OnInitialized for all components will have run before the first OnAfterrender runs.
         * Guaranteed: The order of OnAfterRender is not fixed but flexible.
         * 
         * If you want to make sure that all OnAfterrender methods for all components have completed
         * you need to set a TaskCompletionSource for each nested component when it is initialized.
         */

        private int nestlevel = 0;

        private async Task internalAfterRenderAsync(bool firstRender)
        {
            nestlevel++;

            if (nestlevel == 1)
            {
                if (KenovaClientConfig.Diagnostics) Console.WriteLine($"🧱 KenovaDialogBase ({this.DebugName}) - internalAfterRenderAsync - ENTER - firstRender {firstRender}");
            }
            else
            {
                if (KenovaClientConfig.Diagnostics) Console.WriteLine($"🧱 KenovaDialogBase ({this.DebugName}) - internalAfterRenderAsync - ENTER - firstRender {firstRender} - NESTLEVEL NESTLEVEL NESTLEVEL {nestlevel} NESTLEVEL NESTLEVEL NESTLEVEL {nestlevel}");
            }

            //await this.Mutex.WaitAsync();

            bool synchro_await = false;

            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i].TCS.Task.Status == TaskStatus.WaitingForActivation)
                {
                    synchro_await = true;
                    break;
                }
                //else
                //{
                //    if (KenovaClientConfig.Diagnostics) Console.WriteLine($"UNUSUAL COMPONENT STATUS {_components[i].TCS.Task.Status}");
                //}

            }

            if (synchro_await == true)
            {
                // Create a task array
                Task[] tasks = new Task[_components.Count];

                for (int i = 0; i < _components.Count; i++)
                {
                    //if (KenovaClientConfig.Diagnostics) Console.WriteLine($"Preparing to await component {_components[i].TCS.Task.Status}");
                    tasks[i] = _components[i].TCS.Task;

                    if (_components[i].TCS.Task.Status != TaskStatus.WaitingForActivation)
                    {
                        if (KenovaClientConfig.Diagnostics) Console.WriteLine($"XXXX TASK DIFFERENT STATUS {_components[i].TCS.Task.Status}");
                    }
                }

                // Wait until the OnAfterRender() for ALL IKenovaComponents has finished.
                // This is important as OnAfterRender in the components is used to run JavaScript 'Start' functions that
                // set the initial state. 
                await Task.WhenAll(tasks);

            }

            // At this point we are sure all the nested components are rendered and running

            if (firstRender)
            {
                await _wingman.InstantiateAsync(this, "KenovaDialogBaseComponent");

                _js_started = true;
                await _wingman.InvokeVoidAsync("Start", _container_id, this.IsOpenedAsLayer(), _focus_top_id, _focus_bottom_id);

                // PerformAutoFocus can rest assured that all IKenovaComponents have been AfterRendered and are up and running.
                await this.PerformAutoFocusAsync();

                // Give the default (toolbar)button a special border color
                await _wingman.InvokeVoidAsync("RefreshButtons", _container_id);
                OnAfterRender(true);
                // There is some Exception handling missing here?
                await OnAfterRenderAsync(true);

                FirstRenderAwait_Complete = true;
            }
            else // not firstRender
            {
                if (FirstRenderAwait_Complete == false)
                    return;

                // Give the default (toolbar)button a special border color
                await _wingman.InvokeVoidAsync("RefreshButtons", _container_id);

                OnAfterRender(false);

                // There is some Exception handling missing here?
                await OnAfterRenderAsync(false);
            }

            if (_refresh_tcs != null && _refresh_tcs.Task.Status == TaskStatus.WaitingForActivation)
            {
                _refresh_tcs.SetResult();
            }

            if (KenovaClientConfig.Diagnostics) Console.WriteLine($"🧱 KenovaDialogBase ({this.DebugName}) - internalAfterRenderAsync - EXIT");

            nestlevel--;
        }

        /// <summary>
        /// Try to focus the first component that is a) marked as 'AutoFocus', b) Enabled
        /// and d) Visible in the browser's DOM (not display: none).
        /// </summary>
        public async ValueTask PerformAutoFocusAsync()
        {
            int highest_priority = -1;
            IKenovaComponent highest_priority_component = null;

            for (int i = 0; i < _components.Count; i++)
            {
                IKenovaComponent component = _components[i].Component;

                bool ishidden = await component.ComponentHiddenAsync();

                if (ishidden == false)
                {
                    int priority = await component.MeasureAutoFocusPriorityAsync();

                    if (priority > highest_priority)
                    {
                        highest_priority = priority;
                        highest_priority_component = component;
                    }
                }
            }

            if (highest_priority_component != null)
            {
                await highest_priority_component.PerformAutoFocusAsync();
            }
        }

        /// <summary>
        /// Try to focus the first component that is a) marked as 'AutoFocus', b) Enabled,
        /// d) Visible in the browser's DOM (not display: none) and e) the component's DOM elements are a child
        /// of the specified parent_id element.
        /// </summary>
        public async ValueTask PerformAutoFocusAsync(string parent_id)
        {
            if (parent_id == null)
                throw new ArgumentNullException(nameof(parent_id));

            int highest_priority = -1;
            IKenovaComponent highest_priority_component = null;

            for (int i = 0; i < _components.Count; i++)
            {
                IKenovaComponent component = _components[i].Component;

                bool ishidden = await component.ComponentHiddenAsync();
                bool ischild = await component.IsChildOfAsync(parent_id);

                if (ishidden == false && ischild)
                {
                    int priority = await component.MeasureAutoFocusPriorityAsync();

                    if (priority > highest_priority)
                    {
                        highest_priority = priority;
                        highest_priority_component = component;
                    }
                }
            }

            if (highest_priority_component != null)
            {
                await highest_priority_component.PerformAutoFocusAsync();
            }

        }

        internal async ValueTask PerformCancelAsync()
        {
            bool complete = false;

            foreach (var item in _components)
            {
                var component = item.Component;
                var isperformed = await component.PerformEscapePressedAsync();

                if (isperformed)
                {
                    complete = true;
                    break;
                }
            }

            if (!complete)
            {
                await this.CloseCancelAsync();
            }


        }



        [JSInvokable]
        public async ValueTask OnEnterPressed()
        {
            foreach (var item in _components)
            {
                var component = item.Component;
                var isperformed = await component.PerformEnterPressedAsync();

                if (isperformed)
                {
                    break;
                }
            }
        }

        /*
        [JSInvokable]
        public void LayerGotFocus()
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine($"LAYER GOT FOCUS {DateTime.Now.Ticks}");
        }

        [JSInvokable]
        public void LayerLostFocus()
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine($"LAYER LOST FOCUS {DateTime.Now.Ticks}");
        }
        */
    }

    internal class RegisteredComponentInfo
    {
        public readonly IKenovaComponent Component;
        public readonly TaskCompletionSource TCS;

        internal bool RegisterFirstRenderCompleted;

        public RegisteredComponentInfo(IKenovaComponent component)
        {
            Component = component;
            TCS = new TaskCompletionSource();
        }

    }


}