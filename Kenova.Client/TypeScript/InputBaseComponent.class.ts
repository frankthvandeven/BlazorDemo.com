
class InputBaseComponent {
    dotNetReference: dotNetHandler;
    inputbox: HTMLElement;
    has_zoombutton: boolean;
    has_dropdownbutton: boolean;

    constructor(dotNetReference) {
        this.dotNetReference = dotNetReference;
    }

    dispose() { }

    // Call a C# method as follows. The C# method must have the [JSInvokable] attribute.
    //this.dotNetReference.invokeMethodAsync('CallOnFirstUpdate', param1);

    Start(inputbox_id, has_zoombutton, has_dropdownbutton) {
        this.inputbox = document.getElementById(inputbox_id);
        this.has_zoombutton = has_zoombutton;
        this.has_dropdownbutton = has_dropdownbutton;

        this.inputbox.addEventListener('keydown', this.handleKeydown, false);
    }

    Stop() {
        this.inputbox.removeEventListener('keydown', this.handleKeydown, false);
    }

    handleKeydown = (e) => {

        // Make sure no modifier key is pressed
        if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
            return;

        if (e.keyCode === 120) { // F9 (magnifier icon)

            if (this.has_zoombutton) {
                // TODO: fix out-of-process-async
                this.dotNetReference.invokeMethodAsync('KeyZoomPressed');
                //e.preventDefault();
                e.stopPropagation();
            }
            else if (this.has_dropdownbutton) {
                // TODO: fix out-of-process-async
                this.dotNetReference.invokeMethodAsync('KeyDropdownPressed');
                e.stopPropagation();
            }

        }
        else if (e.keyCode === 13) { // Enter

            if (this.has_dropdownbutton) {
                // TODO: fix out-of-process-async
                this.dotNetReference.invokeMethodAsync('KeyDropdownPressed');
                e.stopPropagation();
            }
            else if (this.has_zoombutton) {
                // TODO: fix out-of-process-async
                this.dotNetReference.invokeMethodAsync('KeyZoomPressed');
                e.stopPropagation();
            }
        }

    }

    ComponentHidden = () => {
        return window.getComputedStyle(this.inputbox).display === "none"
    }
}
