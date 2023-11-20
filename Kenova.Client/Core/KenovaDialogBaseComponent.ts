
class KenovaDialogBaseComponent {
    dotNetReference: dotNetHandler;
    container: HTMLElement;
    opened_as_layer: boolean;
    focus_top_element: HTMLElement;
    focus_bottom_element: HTMLElement;
    button_has_focus: boolean;

    constructor(dotNetReference) {
        this.dotNetReference = dotNetReference;
    }

    dispose() { }

    // Call a C# method as follows. The C# method must have the [JSInvokable] attribute.
    //this.dotNetReference.invokeMethodAsync('CallOnFirstUpdate', param1);

    Start(container_id, opened_as_layer, focus_top_id, focus_bottom_id) {
        this.container = document.getElementById(container_id);
        this.opened_as_layer = opened_as_layer;

        if (opened_as_layer) {
            this.focus_top_element = document.getElementById(focus_top_id);
            this.focus_bottom_element = document.getElementById(focus_bottom_id);
        }

        this.button_has_focus = false;

        this.container.addEventListener('keydown', this.handleKeydown, false);
        this.container.addEventListener('focusin', this.handleFocusin, false);
        this.container.addEventListener('focusout', this.handleFocusout, false);

        if (this.opened_as_layer) {
            this.focus_top_element.addEventListener('focus', this.handleTopFocusin, false);
            this.focus_bottom_element.addEventListener('focus', this.handleBottomFocusin, false);
        }

    }

    Stop() {
        if (this.opened_as_layer) {
            this.focus_bottom_element.removeEventListener('focus', this.handleBottomFocusin, false);
            this.focus_top_element.removeEventListener('focus', this.handleTopFocusin, false);
        }

        this.container.removeEventListener('keydown', this.handleKeydown, false);
        this.container.removeEventListener('focusin', this.handleFocusin, false);
        this.container.removeEventListener('focusout', this.handleFocusout, false);
    }

    handleKeydown = (e) => {

        // Ctrl key pressed, but not other modifier key
        if (e.ctrlKey && !e.shiftKey && !e.altKey && !e.metaKey) {

            if (e.keyCode === 13) { // Ctrl-enter
                this.dotNetReference.invokeMethodAsync('OnEnterPressed');
                e.preventDefault();
            }
        }

    }

    getTabbableElements = () => {

        let query = "a[href]:not([tabindex='-1'])," +
            "area[href]:not([tabindex='-1'])," +
            "button:not([disabled]):not([tabindex='-1'])," +
            "input:not([disabled]):not([tabindex='-1']):not([type='hidden'])," +
            "select:not([disabled]):not([tabindex='-1'])," +
            "textarea:not([disabled]):not([tabindex='-1'])," +
            "iframe:not([tabindex='-1'])," +
            "details:not([tabindex='-1'])," +
            "[tabindex]:not([tabindex='-1'])," +
            "[contentEditable=true]:not([tabindex='-1']";

        return this.container.querySelectorAll(query);
    }

    // https://usefulangle.com/post/145/javascript-add-remove-css-class


    handleTopFocusin = (e) => {
        let stops = this.getTabbableElements();
        let element = stops[stops.length - 2] as HTMLElement; // element stops.length - 1 is the dummy
        element.focus();
    }

    handleBottomFocusin = (e) => {
        let stops = this.getTabbableElements();
        let element = stops[1] as HTMLElement; // element 0 is the dummy
        element.focus();
    }

    handleFocusin = (e) => {
        //this.dotNetReference.invokeMethodAsync('LayerGotFocus');

        let focussed_element = e.srcElement;

        if (focussed_element.hasAttribute('_kn_ctrl_enter') == false) {
            return;
        }

        // The currently focussed element has custom attribute _kn_ctrl_enter, so remove the default button indicator

        this.button_has_focus = true;

        let list = this.container.querySelectorAll("div[_kn_defaultbutton]");

        //btn-default

        for (let i = 0; i < list.length; i++) {
            let element = list[i];

            element.classList.remove('btn-default');

            //console.log('found _kn_ctrl_enter ' + i);
        }

    }

    handleFocusout = (e) => {
        //this.dotNetReference.invokeMethodAsync('LayerLostFocus');

        this.button_has_focus = false;

        let list = this.container.querySelectorAll("div[_kn_defaultbutton]");

        for (let i = 0; i < list.length; i++) {
            let element = list[i];

            if (i == 0) {
                element.classList.add('btn-default');
            }
        }

    }

    RefreshButtons = () => {

        if (this.button_has_focus) {
            return;
        }

        let list = this.container.querySelectorAll("div[_kn_defaultbutton]");

        for (let i = 0; i < list.length; i++) {
            let element = list[i];

            if (i == 0) {
                element.classList.add('btn-default');
            }
        }


    }


}

