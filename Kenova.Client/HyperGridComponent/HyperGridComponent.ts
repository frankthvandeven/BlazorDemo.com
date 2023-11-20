
// Do not use 'var' anymore. Use 'let' instead. Let is more clear regarding scope.
// https://www.freecodecamp.org/news/var-let-and-const-whats-the-difference/
// https://codesandbox.io/s/blissful-saha-zb8xi?file=/src/index.js

class HyperGridComponent {
    dotNetReference: dotNetHandler;
    virtualize: boolean;
    container: HTMLElement;
    table_element: HTMLElement;
    input_element: HTMLElement;
    first_in_view: number;
    last_in_view: number;
    header_element: HTMLElement;
    timer_handle: number;
    MILLISECONDS: number;


    permanent_focus: boolean;

    constructor(dotNetReference) {
        this.dotNetReference = dotNetReference;
    }

    dispose() { }

    // Call a C# method as follows. The C# method must have the [JSInvokable] attribute.
    //this.dotNetReference.invokeMethodAsync('CallOnFirstUpdate', param1);

    // https://javascript.info/bubbling-and-capturing

    Start(virtualize, container_id, table_id, input_element_id) {
        this.virtualize = virtualize;
        this.container = document.getElementById(container_id);
        this.table_element = document.getElementById(table_id);
        this.input_element = document.getElementById(input_element_id);
        this.first_in_view = -1;
        this.last_in_view = -1;
        this.timer_handle = null;
        this.MILLISECONDS = 0; // delay to give the scroll handler (of Blazor Virtualize) time to finish
        this.permanent_focus = false;

        if (this.virtualize) {
            this.MILLISECONDS = 25; // Tried 15, but that was too fast
        }

        //if (this.input_element != null) {
        //    this.input_element.setAttribute("_kn_ctrl_enter", ""); // reserve the ctrl-enter key
        //}

        //this.resizeObserver = new ResizeObserver(entries => {
        //    entries.forEach(entry => {
        //        console.log('observed width', entry.contentRect.width);
        //        console.log('observed height', entry.contentRect.height);
        //    });
        //});

        //this.resizeObserver.observe(this.table_element.children[0]); // observe() will call the callback right away, without any resize

        this.table_element.addEventListener('keydown', this.handleKeydown, false);
        this.table_element.addEventListener('scroll', this.handleScroll, false);
        this.table_element.addEventListener('mousedown', this.handleMouseDown, false);

        if (this.input_element != null) {
            this.input_element.addEventListener('focus', this.handleInputElementFocus, false);
            this.input_element.addEventListener('blur', this.handleInputElementBlur, false);
            this.input_element.addEventListener('keydown', this.handleInputElementKeyDown, false); // was true
        }

    }

    Stop() {

        if (this.input_element != null) {
            this.input_element.addEventListener('focus', this.handleInputElementFocus, false);
            this.input_element.addEventListener('blur', this.handleInputElementBlur, false);
            this.input_element.addEventListener('keydown', this.handleInputElementKeyDown, false); // was true
        }

        this.table_element.removeEventListener('mousedown', this.handleMouseDown, false);
        this.table_element.removeEventListener('scroll', this.handleScroll, false);
        this.table_element.removeEventListener('keydown', this.handleKeydown, false);

        //this.resizeObserver.unobserve(this.table_element.children[0]);
    }


    handleInputElementFocus = (e) => {
        this.permanent_focus = true;
        this.instantCalculateTabindexZero(); // display the focus rectangle
    }

    handleInputElementBlur = (e) => {
        this.permanent_focus = false;
        this.instantCalculateTabindexZero(); // hide the focus rectangle
    }

    handleInputElementKeyDown = (e) => {

        // Make sure no modifier key is pressed
        if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
            return;

        if (e.keyCode === 38) { // up
            this.KeyUpArrowPressed();
            e.preventDefault();
        }
        else if (e.keyCode === 40) { // down
            this.KeyDownArrowPressed();
            e.preventDefault();
        }
        else if (e.keyCode === 36) { // home
            this.KeyHomePressed();
            e.preventDefault();
        }
        else if (e.keyCode === 35) { // end
            this.KeyEndPressed();
            e.preventDefault();
        }
        else if (e.keyCode === 33) { // PgUp
            this.KeyPageUpPressed();
            e.preventDefault();
        }
        else if (e.keyCode === 34) { // PgDn
            this.KeyPageDownPressed();
            e.preventDefault();
        }
        else if (e.keyCode === 13) { // enter
            this.KeyEnterPressed();
            e.preventDefault();
            e.stopPropagation();
        }

    }


    SetFocus = () => {

        this.instantCalculateTabindexZero(); // make sure there is a focus index

        let focus_index = this.findFocusIndex();
        let rows = this.table_element.children as HTMLCollectionOf<HTMLElement>;

        if (focus_index != -1) {
            rows[focus_index].focus();
        }

    }

    KNStartScrollSync(header_id) {
        this.header_element = document.getElementById(header_id);

        this.table_element.addEventListener('scroll', this.handleScrollsync, false);
    }

    KNStopScrollSync() {
        this.table_element.removeEventListener('scroll', this.handleScrollsync, false);
    }

    handleScrollsync = () => {
        this.header_element.scrollLeft = this.table_element.scrollLeft;
    }

    ScrollToTop() {
        this.table_element.scrollTop = 0;
    }

    ScrollToLeft() {
        this.table_element.scrollLeft = 0;
    }

    ScrollToBottom() {
        this.table_element.scrollTop = this.table_element.scrollHeight;
    }

    MakeFirstItemInView(index) {
        let height = this.findElementHeight(this.table_element);

        if (height == null)
            return;

        this.table_element.scrollTop = height * index;

    }

    MakeCenterItemInView(index) {
        let height = this.findElementHeight(this.table_element);

        if (height == null)
            return;

        let top = height * index;
        let space_above = (this.table_element.clientHeight - height) / 2;

        top -= space_above;

        if (top < 0) {
            top = 0;
        }

        this.table_element.scrollTop = top;
    }

    MakeLastItemInView(index) {
        let height = this.findElementHeight(this.table_element);

        if (height == null)
            return;

        let top = height * index;
        let space_above = this.table_element.clientHeight - height;

        top -= space_above;

        if (top < 0) {
            top = 0;
        }

        this.table_element.scrollTop = top;

        /*
        setTimeout(() => {
            this.table_element.scrollTop = top;

        }, 1);
        */

    }

    findElementHeight = (table_element) => {

        let height = null;

        if (table_element.children.length >= 2) {
            let r = table_element.children[1]; // was 2
            if (r.className.includes("kn-hyper-datarow")) {
                height = r.offsetHeight;
            }
        }
        else if (table_element.children.length == 1) {
            let r = table_element.children[0]; // was 1
            if (r.className.includes("kn-hyper-datarow")) {
                height = r.offsetHeight;
            }
        }

        return height;
    }

    handleMouseDown = (e) => {

        let rows = this.table_element.children as HTMLCollectionOf<HTMLElement>;

        for (let i = 0; i < rows.length; i++) {
            let row_element = rows[i];

            if (this.didClickElement(e, row_element)) {
                row_element.tabIndex = 0;

                if (this.permanent_focus) {
                    row_element.style["outline-style"] = "solid";
                }
                else {
                    row_element.style.removeProperty('outline-style');
                }

                row_element.focus();
            }
            else {
                row_element.removeAttribute('tabindex');
                row_element.style.removeProperty('outline-style');
            }

        }
    }

    didClickElement = (e, element) => {
        return element == e.target || element.contains(e.target);
    }

    handleScroll = (e) => {
        this.delayedCalculateTabindexZero();
    }

    findFocusIndex = () => {
        let rows = this.table_element.children as HTMLCollectionOf<HTMLElement>;

        for (let i = 0; i < rows.length; i++) {
            if (rows[i].tabIndex == 0) {
                return i;
            }
        }

        return -1;
    }

    makeFocussedElement = (element_to_focus) => {

        let rows = this.table_element.children as HTMLCollectionOf<HTMLElement>;

        for (let i = 0; i < rows.length; i++) {
            let row_element = rows[i];

            if (row_element === element_to_focus) {

                if (this.permanent_focus) {
                    row_element.tabIndex = 0;
                    row_element.style["outline-style"] = "solid";
                }
                else {
                    row_element.tabIndex = 0;
                    row_element.style.removeProperty('outline-style');
                }

                //console.log('roving tabindex 0 goes to element ' + i);
            }
            else {
                row_element.removeAttribute('tabindex');
                row_element.style.removeProperty('outline-style');
            }
        }

    }

    // Called from HyperGrid.OnAfterRender
    OnAfterRender = () => {
        this.delayedCalculateTabindexZero();
    }

    delayedCalculateTabindexZero = () => {

        // Reset the delayed processing timer
        if (this.timer_handle != null) {
            clearTimeout(this.timer_handle);
            this.timer_handle = null;
        }

        // Delay the processing
        this.timer_handle = setTimeout(this.instantCalculateTabindexZero, 50);
    }

    // Based on: https://jsfiddle.net/rplantiko/u2gLr64c/
    //
    // This method sets this.first_in_view and this.last_in_view to the index of first and last visible child element (row).
    // It also will give one of the visible rows a tabindex 0, to make sure the HyperGrid is a tabstop.
    // Logic explained:
    // Does one of the visible elements have tabindex 0?
    // If yes, keep that tabindex.
    // If not, give the "selected" element tabindex 0.
    // If there is no selected element, give the first element tabindex 0.
    instantCalculateTabindexZero = () => {

        let rows = this.table_element.children as HTMLCollectionOf<HTMLElement>;

        let first = -1;
        let last = -1;
        let tab_index_0_found = false;
        let selected_element = null;

        for (let i = 0; i < rows.length; i++) {
            let row_element = rows[i];

            if (row_element.hasAttribute('_idx')) {

                if (row_element.classList.contains('selected')) {
                    selected_element = row_element;
                }

                let in_view = this.isChildElementInView(row_element);

                if (in_view) {
                    if (first == -1) { first = i; }
                    if (i > last) { last = i; }
                    if (row_element.tabIndex == 0) { // an existing tabindex 0 can only remain when the element is in view
                        tab_index_0_found = true;

                        if (this.permanent_focus) {
                            row_element.style["outline-style"] = "solid";
                        }
                        else {
                            row_element.style.removeProperty('outline-style');
                        }
                    }
                }
                else {
                    row_element.removeAttribute('tabindex');
                    row_element.style.removeProperty('outline-style');
                }
            }
        }

        if (tab_index_0_found == false && selected_element != null) {
            if (this.permanent_focus) {
                selected_element.tabIndex = "0";
                selected_element.style["outline-style"] = "solid";
            }
            else {
                selected_element.tabIndex = "0";
                selected_element.style.removeProperty('outline-style');
            }

            tab_index_0_found = true;
        }

        if (tab_index_0_found == false && first != -1) {
            if (this.permanent_focus) {
                rows[first].tabIndex = 0;
                rows[first].style["outline-style"] = "solid";
            }
            else {
                rows[first].tabIndex = 0;
                rows[first].style.removeProperty('outline-style');
            }
        }

        //console.log("determined first and last " + first + ", " + last);

        this.first_in_view = first;
        this.last_in_view = last;
    }

    isChildElementInView(element) {
        let container = this.table_element;
        let containerTop = container.scrollTop;
        let containerBottom = containerTop + container.clientHeight;

        let elemTop = element.offsetTop;
        let elemHeight = element.clientHeight;
        let elemBottom = elemTop + elemHeight;

        if (elemHeight == 0.0)
            return false;

        //console.log("element.offsetTop " + element.offsetTop);

        let half_visible = elemHeight / 2;

        return (elemTop >= (containerTop - half_visible) && elemBottom <= (containerBottom + half_visible));
        //return (elemTop >= containerTop && elemBottom <= (containerBottom + 1.0));

    }

    /* KEYPRESS PROCESSING */

    handleKeydown = (e) => {

        // Ctrl key pressed, but not other modifier key
        if (e.ctrlKey && !e.shiftKey && !e.altKey && !e.metaKey) {
            if (e.keyCode === 65) { // Ctrl-A
                this.dotNetReference.invokeMethodAsync('SelectAllPressed');
                e.preventDefault();
                e.stopPropagation();
            }
            return;
        }

        // Make sure no modifier key is pressed
        if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
            return;

        if (e.keyCode === 38) { // up
            this.KeyUpArrowPressed();
            e.preventDefault();
        }
        else if (e.keyCode === 40) { // down
            this.KeyDownArrowPressed();
            e.preventDefault();
        }
        else if (e.keyCode === 36) { // home
            this.KeyHomePressed();
            e.preventDefault();
        }
        else if (e.keyCode === 35) { // end
            this.KeyEndPressed();
            e.preventDefault();
        }
        else if (e.keyCode === 33) { // PgUp
            this.KeyPageUpPressed();
            e.preventDefault();
        }
        else if (e.keyCode === 34) { // PgDn
            this.KeyPageDownPressed();
            e.preventDefault();
        }
        else if (e.keyCode === 13) { // enter
            this.KeyEnterPressed();
            e.preventDefault();
            e.stopPropagation();
        }
        else if (e.keyCode === 32) { // space
            this.KeySpacePressed();
            e.preventDefault();
            e.stopPropagation();
        }
        else if (e.keyCode >= 48 && e.keyCode <= 57) { // 0 .. 9
            this.KeySelectPressed();
            e.preventDefault();
            e.stopPropagation();
        }
        else if (e.keyCode >= 65 && e.keyCode <= 90) { // a .. z
            this.KeySelectPressed();
            e.preventDefault();
            e.stopPropagation();
        }
        else if (e.keyCode >= 96 && e.keyCode <= 111) { // NumPad keys
            this.KeySelectPressed();
            e.preventDefault();
            e.stopPropagation();
        }
        else if (e.keyCode >= 186 && e.keyCode <= 223) { // semicolon, equal sign, plus, forward slash etc. etc.
            this.KeySelectPressed();
            e.preventDefault();
            e.stopPropagation();
        }

    }

    KeyEnterPressed = () => {
        let focus_index = this.findFocusIndex();
        let rows = this.table_element.children;

        if (focus_index != -1) {
            let row_element = rows[focus_index];
            let idx = parseInt(row_element.getAttribute('_idx'));

            this.dotNetReference.invokeMethodAsync('OnEnterPressed', idx);
        }
    }

    KeySpacePressed = () => {
        let focus_index = this.findFocusIndex();
        let rows = this.table_element.children;

        if (focus_index != -1) {
            let row_element = rows[focus_index];
            let idx = parseInt(row_element.getAttribute('_idx'));

            this.dotNetReference.invokeMethodAsync('OnSpacePressed', idx);
        }
    }

    KeySelectPressed = () => {
        let focus_index = this.findFocusIndex();
        let rows = this.table_element.children;

        if (focus_index != -1) {
            let row_element = rows[focus_index];
            let idx = parseInt(row_element.getAttribute('_idx'));

            this.dotNetReference.invokeMethodAsync('OnSelectPressed', idx);
        }
    }

    KeyDownArrowPressed = () => {
        let focus_index = this.findFocusIndex();
        let re_focus = this.table_element.contains(document.activeElement);
        let rows = this.table_element.children;
        let container = this.table_element;

        if (focus_index == this.last_in_view) {
            container.scrollTop += this.findElementHeight(container)

            setTimeout(() => {
                this.instantCalculateTabindexZero();
                if (this.last_in_view != -1) {
                    let element = container.children[this.last_in_view] as HTMLElement;
                    this.makeFocussedElement(element);
                    if (re_focus) {
                        element.focus();
                    }
                }

            }, this.MILLISECONDS);
        }
        else if (focus_index < this.last_in_view) {
            //console.log('down - not last in view (focusindex = ' + focus_index + " last_in_view = " + this.last_in_view + ')');
            let element = rows[focus_index + 1] as HTMLElement;
            this.makeFocussedElement(element);
            if (re_focus) {
                element.focus();
            }
        }

    }

    KeyUpArrowPressed = () => {
        let focus_index = this.findFocusIndex();
        let re_focus = this.table_element.contains(document.activeElement);
        let rows = this.table_element.children;
        let container = this.table_element;

        if (focus_index == this.first_in_view) {
            container.scrollTop -= this.findElementHeight(container)

            setTimeout(() => {
                this.instantCalculateTabindexZero();
                if (this.first_in_view != -1) {
                    let element = container.children[this.first_in_view] as HTMLElement;
                    this.makeFocussedElement(element);

                    if (re_focus) {
                        element.focus();
                    }
                }

            }, this.MILLISECONDS);
        }
        else if (focus_index > this.first_in_view) {

            let element = rows[focus_index - 1] as HTMLElement;
            this.makeFocussedElement(element);

            if (re_focus) {
                element.focus();
            }
        }

    }

    KeyPageUpPressed = () => {
        let focus_index = this.findFocusIndex();
        let re_focus = this.table_element.contains(document.activeElement);
        let rows = this.table_element.children;
        let container = this.table_element;

        if (focus_index > this.first_in_view) {
            if (this.first_in_view != -1) {
                let element = rows[this.first_in_view] as HTMLElement;
                this.makeFocussedElement(element);
                if (re_focus) {
                    element.focus();
                }
            }
        }
        else {
            container.scrollTop -= container.clientHeight

            setTimeout(() => {
                this.instantCalculateTabindexZero();
                if (this.first_in_view != -1) {
                    let element = container.children[this.first_in_view] as HTMLElement;
                    this.makeFocussedElement(element);
                    if (re_focus) {
                        element.focus();
                    }
                }

            }, this.MILLISECONDS);
        }

    }

    KeyPageDownPressed = () => {
        let focus_index = this.findFocusIndex();
        let re_focus = this.table_element.contains(document.activeElement);
        let rows = this.table_element.children;
        let container = this.table_element;

        if (focus_index < this.last_in_view) {
            if (this.last_in_view != -1) {
                let element = rows[this.last_in_view] as HTMLElement;
                this.makeFocussedElement(element);
                if (re_focus) {
                    element.focus();
                }
            }
        }
        else {
            container.scrollTop += container.clientHeight

            setTimeout(() => {
                this.instantCalculateTabindexZero(); // we need to update as we modified scrollTop
                if (this.last_in_view != -1) {
                    let element = container.children[this.last_in_view] as HTMLElement;
                    this.makeFocussedElement(element);
                    if (re_focus) {
                        element.focus();
                    }
                }

            }, this.MILLISECONDS);
        }

    }

    KeyHomePressed = () => {
        let re_focus = this.table_element.contains(document.activeElement);
        let container = this.table_element;

        container.scrollTop = 0;

        setTimeout(() => {
            this.instantCalculateTabindexZero();
            if (this.first_in_view != -1) {
                let element = container.children[this.first_in_view] as HTMLElement;
                this.makeFocussedElement(element);
                if (re_focus) {
                    element.focus();
                }
            }

        }, this.MILLISECONDS);

    }

    KeyEndPressed = () => {
        let re_focus = this.table_element.contains(document.activeElement);
        let container = this.table_element;

        container.scrollTop = container.scrollHeight;

        setTimeout(() => {
            this.instantCalculateTabindexZero();

            if (this.last_in_view != -1) {
                let element = container.children[this.last_in_view] as HTMLElement;
                this.makeFocussedElement(element);
                if (re_focus) {
                    element.focus();
                }
            }

        }, this.MILLISECONDS);

    }

}
