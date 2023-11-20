
class ToolbarComponent {
    container: HTMLElement;
    dotNetReference: dotNetHandler;
    first_visible: number;
    last_visible: number;
    visible_count: number;
    overflow_button_visible: boolean;
    overflow_button_element: HTMLElement;
    resizeObserver: ResizeObserver;

    constructor(dotNetReference) {
        this.dotNetReference = dotNetReference;
    }

    dispose() { }

    // Call a C# method as follows. The C# method must have the [JSInvokable] attribute.
    //this.dotNetReference.invokeMethodAsync('CallOnFirstUpdate', param1);

    Start(container_id) {
        this.container = document.getElementById(container_id);
        this.first_visible = -1;
        this.last_visible = -1;
        this.visible_count = -1;
        this.overflow_button_visible = false;
        this.overflow_button_element = null;

        this.resizeObserver = new ResizeObserver(entries => {
            //entries.forEach(entry => {
            //    console.log('observed width', entry.contentRect.width);
            //    console.log('observed height', entry.contentRect.height);
            //});
            this.KNMeasureOverflow();
        });

        // observe() will call the callback right away, without any resize
        this.resizeObserver.observe(this.container);
        this.container.addEventListener('keydown', this.handleKeydown, false);
        this.container.addEventListener('focusin', this.handleFocusin, false);
        this.container.addEventListener('focusout', this.handleFocusout, false);
        this.container.addEventListener('click', this.handleClick, false);
    }

    Stop() {
        this.container.removeEventListener('click', this.handleClick, false);
        this.container.removeEventListener('focusout', this.handleFocusout, false);
        this.container.removeEventListener('focusin', this.handleFocusin, false);
        this.container.removeEventListener('keydown', this.handleKeydown, false);
        this.resizeObserver.unobserve(this.container);
    }

    KNMeasureOverflow = () => {

        let container = this.container;
        let children = container.children as HTMLCollectionOf<HTMLElement>;
        let i;
        // Make all elements visible
        for (i = 0; i < children.length; i++) {
            children[i].style.removeProperty('display');
            //children[i].setAttribute("_visible", "yes");
        }
        let totalwidth = 0.0;
        for (i = 0; i < children.length - 1; i++) {
            totalwidth += children[i].offsetWidth;
        }

        let overflowButton = false;
        let availableWidth = container.offsetWidth;

        if (totalwidth > container.offsetWidth) {
            overflowButton = true;
            availableWidth -= children[children.length - 1].offsetWidth;
        }
        totalwidth = 0.0;
        let visible_items = 0;
        for (i = 0; i < children.length - 1; i++) {
            totalwidth += children[i].offsetWidth;
            if (totalwidth <= availableWidth) {
                visible_items++;
            }
            else {
                children[i].style.display = "none";
                //children[i].setAttribute("_visible", "no");
                children[i].tabIndex = -1;
            }
        }

        if (overflowButton == false) {
            children[children.length - 1].style.display = "none";
            //children[children.length - 1].setAttribute("_visible", "no");
            children[children.length - 1].tabIndex = -1;
        }

        if (visible_items == 0) {
            this.first_visible = -1;
            this.last_visible = -1;
        }
        else {
            this.first_visible = 0;
            this.last_visible = visible_items - 1;
        }

        this.visible_count = visible_items;
        this.overflow_button_visible = overflowButton;
        this.overflow_button_element = children[children.length - 1];

        this.CalculateTabindexZero();

    }

    handleFocusin = (e) => {
        this.SetRovingTabZero(e.srcElement);
    }

    handleFocusout = (e) => {
    }

    handleClick = (e) => {

        if (this.didClickElement(e, this.overflow_button_element)) {
            this.dotNetReference.invokeMethodAsync('OnOverflowClicked', this.visible_count);
            return;
        }

        for (let index = 0; index < this.visible_count; index++) {
            if (this.didClickElement(e, this.container.children[index])) {
                this.dotNetReference.invokeMethodAsync('OnButtonClicked', index);
                return;
            }
        }

    }

    didClickElement = (e, element) => {
        return element == e.target || element.contains(e.target);
    }

    handleKeydown = (e) => {

        // Make sure no modifier key is pressed
        if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
            return;

        // sets this.is_first and this.is_last
        //this.CalculateTabindexZero();

        let focus_index = this.GetRovingTabZero();
        let buttons = this.container.children as HTMLCollectionOf<HTMLElement>;

        if (e.keyCode === 38 || e.keyCode === 37) { // up, left

            if (focus_index == -2) {
                if (this.visible_count != 0) {
                    buttons[this.last_visible].focus();
                }
            }
            else if (focus_index == 0) {
                if (this.overflow_button_visible) {
                    this.overflow_button_element.focus();
                }
                else {
                    buttons[this.last_visible].focus();
                }
            }
            else if (focus_index > 0) {

                buttons[focus_index - 1].focus();
            }

            e.preventDefault();
            e.stopPropagation();

        }
        else if (e.keyCode === 40 || e.keyCode === 39) { // down, right

            if (focus_index == -2) {
                if (this.visible_count != 0) {
                    buttons[0].focus();
                }
            }
            else if (focus_index == this.last_visible) {
                if (this.overflow_button_visible) {
                    this.overflow_button_element.focus();
                }
                else {
                    buttons[0].focus();
                }
            }
            else if (focus_index >= 0 && focus_index < this.last_visible) {

                buttons[focus_index + 1].focus();
            }

            e.preventDefault();
            e.stopPropagation();

        }
        else if (e.keyCode === 13) { // enter

            if (focus_index == -2) {
                this.dotNetReference.invokeMethodAsync('OnOverflowClicked', this.visible_count);
            }
            else if (focus_index >= 0) {
                this.dotNetReference.invokeMethodAsync('OnButtonClicked', focus_index);
            }

            e.preventDefault();
            e.stopPropagation();

        }
        else if (e.keyCode === 32) { // space

            if (focus_index == -2) {
                this.dotNetReference.invokeMethodAsync('OnOverflowClicked', this.visible_count);
            }
            else if (focus_index >= 0) {
                this.dotNetReference.invokeMethodAsync('OnSpacePressed', focus_index);
            }

            e.stopPropagation();
            e.preventDefault();

        }
        else if (e.keyCode === 36) { // home

            if (this.visible_count > 0) {
                buttons[0].focus();
            }

            e.preventDefault();
            e.stopPropagation();

        }
        else if (e.keyCode === 35) { // end

            if (this.overflow_button_visible) {
                this.overflow_button_element.focus();
            }
            else {
                if (this.last_visible != -1) {
                    buttons[this.last_visible].focus();
                }
            }

            e.preventDefault();
            e.stopPropagation();

        }

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

            if (this.first_visible == 0) {
                tab_index_0 = 0;
            }

            if (tab_index_0 == -1) {

                if (this.overflow_button_visible) {
                    tab_index_0 = buttons.length - 1;
                }
            }

            buttons[tab_index_0].tabIndex = 0;
        }

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

        if (this.overflow_button_element.tabIndex == 0) {
            return -2;
        }

        for (let i = 0; i < this.visible_count; i++) {
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

