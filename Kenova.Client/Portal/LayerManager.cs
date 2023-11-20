using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;

namespace Kenova.Client.Components
{
    public static class LayerManager
    {
        private static Stack<LayerDefinition> _layer_stack = new Stack<LayerDefinition>();

        /// <summary>
        /// The stack of layer definitions. 
        /// Never call LayerStack.Clear() but call LayerStack.CloseAllAsync() instead.
        /// </summary>
        public static Stack<LayerDefinition> LayerStack
        {
            get { return _layer_stack; }
        }

        public static async ValueTask CloseTopmostAsync()
        {
            if (_layer_stack.Count == 0)
                return;

            var ld = _layer_stack.Peek();

            await ld.CloseCancelAsync();
        }


        /// <summary>
        /// Close all layers. StateHasChanged will be called 
        /// </summary>
        public static async ValueTask CloseAllAsync()
        {
            // Copies the Stack to an array, in the same order Pop would return the items.
            LayerDefinition[] stackArray = LayerStack.ToArray();

            for (int i = 0; i < stackArray.Length; i++)
            {
                var ld = stackArray[i];
                
                await ld.CloseCancelAsync();
            }

        }

        /// <summary>
        /// Close all layers without calling any StateHasChanged, and set LayerResult.Aborted to true.
        /// </summary>
        public static async ValueTask AbortAllAsync()
        {
            // Copies the Stack to an array, in the same order Pop would return the items.
            LayerDefinition[] stackArray = LayerStack.ToArray();

            for (int i = 0; i < stackArray.Length; i++)
            {
                var ld = stackArray[i];

                await ld.AbortAsync();
            }

        }

        public static async ValueTask CloseNonSolidsAsync()
        {
            while (_layer_stack.Count > 0)
            {
                var ld = _layer_stack.Peek();

                if (ld.Kind != LayerKind.Modal && ld.Kind != LayerKind.ModalWindow)
                {
                    await ld.CloseCancelAsync();
                }
                else
                {
                    break;
                }

            }
        }

        //private static void CloseIfTopmostIsDropdown()
        //{
        //    if (_layer_stack.Count == 0)
        //        return;

        //    var ld = _layer_stack.Peek();

        //    if (ld.Kind == LayerKind.Dropdown || ld.Kind == LayerKind.DropdownBalloon)
        //        _layer_stack.Pop();
        //}


        public static LayerDefinition FindLayerDefinition(KenovaDialogBase component)
        {
            if (component == null)
                throw new ArgumentNullException("component");

            foreach (var ld in _layer_stack)
            {
                if (ld.ComponentReference != null && ld.ComponentReference.Equals(component))
                    return ld;
            }

            return null;
        }

        //public static void CloseOk(LayerDefinition ld, object returnvalue = null)
        //{
        //    PrivateClose(ld, CloseReason.Ok, returnvalue);
        //}

        //public static void CloseCancel(LayerDefinition ld)
        //{
        //    PrivateClose(ld, CloseReason.Cancelled, null);
        //}


        /// <summary>
        /// This method is called from JavaScript. See 'kenovainterop.js'
        /// </summary>
        [JSInvokable]
        public static async ValueTask OnCloseDropdownLayers(int closecount)
        {
            for (int i = 0; i < closecount; i++)
            {
                var ld = _layer_stack.Peek();

                await ld.CloseCancelAsync();
            }

            return;

            //if (_layer_stack.Count == 0)
            //    return;

            //var ld = _layer_stack.Peek();

            //LayerManager.Close(ld);
        }

    }

    public class LayerResult
    {
        internal LayerResult()
        {
        }

        public bool Cancelled { get; internal set; } = false;

        public bool Aborted { get; internal set; } = false;


        public object Data { get; internal set; } = null;

        public T GetData<T>()
        {
            return (T)Data;
        }

    }


}
