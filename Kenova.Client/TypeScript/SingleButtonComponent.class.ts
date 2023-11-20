
class SingleButtonComponent {
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
    }

    Stop() {
        this.container.removeEventListener('keydown', this.handleKeydown, false);
    }

    handleKeydown = (e) => {

        // Make sure no modifier key is pressed
        if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
            return;

        if (e.keyCode === 13) { // enter

            this.dotNetReference.invokeMethodAsync('OnEnterPressed');

            e.preventDefault();
            e.stopPropagation();
            
        }
        else if (e.keyCode === 32) { // space

            this.dotNetReference.invokeMethodAsync('OnSpacePressed');

            e.stopPropagation();
            e.preventDefault();

        }

    }

}
