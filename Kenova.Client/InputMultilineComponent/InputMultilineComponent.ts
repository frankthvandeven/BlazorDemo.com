
class InputMultilineComponent {
    dotNetReference: dotNetHandler;
    element: HTMLElement;
    offset: number;

    constructor(dotNetReference) {
        this.dotNetReference = dotNetReference;
    }

    dispose() { }

    // Call a C# method as follows. The C# method must have the [JSInvokable] attribute.
    //this.dotNetReference.invokeMethodAsync('CallOnFirstUpdate', param1);

    Start(container_id) {
        this.element = document.getElementById(container_id);
        this.element.style.boxSizing = 'border-box';
        this.offset = this.element.offsetHeight - this.element.clientHeight;
        this.updateAutoSize();

        this.element.addEventListener('input', this.updateAutoSize, false);
        this.element.addEventListener('keydown', this.handleKeydown, false);
    }

    updateAutoSize = () => {
        this.element.style.height = 'auto'; // this is the measurement trick
        this.element.style.height = (this.element.scrollHeight + this.offset) + 'px';
    }

    Stop() {
        this.element.removeEventListener('keydown', this.handleKeydown, false);
        this.element.removeEventListener('input', this.updateAutoSize, false);
    }

    ResetHeight() {
        this.updateAutoSize();
    }

    handleKeydown = (e) => {

        // Make sure no modifier key is pressed
        if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
            return;

        if (e.keyCode === 13) { // enter

            //e.preventDefault();
            e.stopPropagation();

        }
    }
}

