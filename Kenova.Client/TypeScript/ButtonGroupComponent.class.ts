
class ButtonGroupComponent {
    dotNetReference: dotNetHandler;
    container: HTMLElement;
    is_first: boolean;
    is_last: boolean;
    //dotNetReference = null;
    //source_element = null;
    //target_element = null;

    constructor(dotNetReference) {
        this.dotNetReference = dotNetReference;
    }

    dispose() { }

    // Call a C# method as follows. The C# method must have the [JSInvokable] attribute.
    //this.dotNetReference.invokeMethodAsync('CallOnFirstUpdate', param1);

    // https://javascript.info/bubbling-and-capturing

    Start(container_id)
    {
        const aa = new ToolbarComponent(null);

        this.container = document.getElementById(container_id);
        this.is_first = false;
        this.is_last = false;

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

        // sets this.is_first and this.is_last
        this.CalculateTabindexZero();

        let focus_index = this.GetRovingTabZero();
        let buttons = this.container.children as HTMLCollectionOf<HTMLElement>;

        if (e.keyCode === 38 || e.keyCode === 37) { // up, left

            if (!this.is_first) {

                // Try to skip a TextOnly button. It only works when there is a single TextOnly button in the middle of a ButtonBar
                if (buttons[focus_index - 1].classList.contains('kn-textonlybutton')) {
                    if (focus_index > 0) {
                        focus_index--;
                    }
                }

                buttons[focus_index - 1].focus();
            }
            else {
                buttons[buttons.length - 1].focus();
            }

            e.preventDefault();
            e.stopPropagation();

        }
        else if (e.keyCode === 40 || e.keyCode === 39) { // down, right

            if (!this.is_last) {

                // Try to skip a TextOnly button. It only works when there is a single TextOnly button in the middle of a ButtonBar
                if (buttons[focus_index + 1].classList.contains('kn-textonlybutton')) {
                    if (focus_index < buttons.length - 1) {
                        focus_index++;
                    }
                }

                buttons[focus_index + 1].focus();
            }
            else {
                buttons[0].focus();
            }

            e.preventDefault();
            e.stopPropagation();

        }
        else if (e.keyCode === 13) { // enter

            if (focus_index != -1) {
                this.dotNetReference.invokeMethodAsync('OnEnterPressed', focus_index);
            }

            e.preventDefault();
            e.stopPropagation();

        }
        else if (e.keyCode === 32) { // space

            if (focus_index != -1) {
                this.dotNetReference.invokeMethodAsync('OnSpacePressed', focus_index);
            }

            e.stopPropagation();
            e.preventDefault();

        }
        else if (e.keyCode === 36) { // home

            buttons[0].focus();

            e.preventDefault();
            e.stopPropagation();

        }
        else if (e.keyCode === 35) { // end

            buttons[buttons.length - 1].focus();

            e.preventDefault();
            e.stopPropagation();

        }
    }

    handleFocusin = (e) => {

        this.SetRovingTabZero(e.srcElement);

    }

    handleFocusout = (e) => {

    }

    // This function is called from:
    // a) AfterRender in de .cs file
    // b) handleKeydown
    CalculateTabindexZero = () => {

        let buttons = this.container.children as HTMLCollectionOf<HTMLElement>;
        let tab_index_0 = -1;

        for (let i = 0; i < buttons.length; i++) {
            if (buttons[i].tabIndex == 0) {
                tab_index_0 = i;
                break;
            }
        }


        if (tab_index_0 == -1) { // no tabindex 0 found. Create a new one.

            for (let i = 0; i < buttons.length; i++) {
                if (buttons[i].classList.contains('selected')) {
                    tab_index_0 = i;
                    break;
                }
            }

            if (tab_index_0 == -1) {
                tab_index_0 = 0;
            }

            buttons[tab_index_0].tabIndex = 0;
        }

        this.is_first = tab_index_0 == 0 ? true : false;
        this.is_last = tab_index_0 == (buttons.length - 1) ? true : false;

    }

    SetRovingTabZero = (focussed_element) => {

        let buttons = this.container.children as HTMLCollectionOf<HTMLElement>;

        for (let i = 0; i < buttons.length; i++) {
            let element = buttons[i];

            if (element === focussed_element) {
                element.tabIndex = 0;
            }
            else {
                if (element.tabIndex == 0) {
                    element.tabIndex = -1;
                }
            }
        }
    }

    GetRovingTabZero = () => {
        let buttons = this.container.children as HTMLCollectionOf<HTMLElement>;

        for (let i = 0; i < buttons.length; i++) {
            if (buttons[i].tabIndex == 0) {
                return i;
            }
        }

        return -1;
    }

    FocusByIndex = (index) => {
        let buttons = this.container.children as HTMLCollectionOf<HTMLElement>;

        buttons[index].focus();
    }

}
