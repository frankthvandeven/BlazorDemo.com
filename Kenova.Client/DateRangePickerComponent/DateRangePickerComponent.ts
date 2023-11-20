
class DateRangePickerComponent {
    dotNetReference: dotNetHandler;
    container: HTMLElement;

    constructor(dotNetReference) {
        this.dotNetReference = dotNetReference;
    }

    dispose() { }

    // Call a C# method as follows. The C# method must have the [JSInvokable] attribute.
    //this.dotNetReference.invokeMethodAsync('CallOnFirstUpdate', param1);

    Start(container_id) {
        this.container = document.getElementById(container_id);

        this.container.addEventListener('keydown', this.handleKeydown, false);
        this.container.addEventListener('focusin', this.handleFocusin, false);
        this.container.addEventListener('focusout', this.handleFocusout, false);
    }

    Stop() {
        this.container.removeEventListener('focusout', this.handleFocusout, false);
        this.container.removeEventListener('focusin', this.handleFocusin, false);
        this.container.removeEventListener('keydown', this.handleKeydown, false);
    }

    handleKeydown = (e) => {

        // Make sure no modifier key is pressed
        if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
            return;

    }

    handleFocusin = (e) => {

    }

    handleFocusout = (e) => {

    }

    OnAfterRender = () => {

    }

}
