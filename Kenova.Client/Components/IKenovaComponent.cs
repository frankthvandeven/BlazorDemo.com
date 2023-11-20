namespace Kenova.Client.Components
{

    /// <summary>
    /// This interface is used on all components that can receive focus.
    /// </summary>
    public interface IKenovaComponent
    {
        ValueTask<bool> PerformFocusAsync(string focusID);

        ValueTask PerformAutoFocusAsync();

        /// <summary>
        /// -1 = no autofocus, 100 = default priority 
        /// </summary>
        ValueTask<int> MeasureAutoFocusPriorityAsync();

        ValueTask<bool> PerformEnterPressedAsync();

        ValueTask<bool> PerformEscapePressedAsync();

        /// <summary>
        /// Is the component hidden in the browser's DOM (display: none)?
        /// </summary>
        ValueTask<bool> ComponentHiddenAsync();

        /// <summary>
        /// Are the component's HTML elements inside the specified element_id?
        /// </summary>
        ValueTask<bool> IsChildOfAsync(string parent_id);

    }

}

