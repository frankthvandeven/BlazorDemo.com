
class PortalRootComponent {
    dotNetReference: dotNetHandler;

    constructor(dotNetReference) {
        this.dotNetReference = dotNetReference;
    }

    dispose() { }

    // Call a C# method as follows. The C# method must have the [JSInvokable] attribute.
    //this.dotNetReference.invokeMethodAsync('CallOnFirstUpdate', param1);

    Start() {
        document.addEventListener('keydown', this.handleKeydown, false);
    }

    Stop() {
        document.removeEventListener('keydown', this.handleKeydown, false);
    }

    handleKeydown = (e) => { // e.stopImmediatePropagation() ??

        // Ctrl key pressed, but not other modifier key
        if (e.ctrlKey && !e.shiftKey && !e.altKey && !e.metaKey) {

            if (e.keyCode === 191) { // Ctrl + forward slash
                // TODO: fix out-of-process-async
                this.dotNetReference.invokeMethodAsync('OnSearchKeyPressed');
                e.preventDefault();
            }
            return;
        }

        // Make sure no modifier key is pressed
        if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
            return;

        if (e.keyCode === 27) { // escape
            // TODO: fix out-of-process-async
            this.dotNetReference.invokeMethodAsync('OnEscapePressed');
            e.preventDefault();

        }

    }


}

