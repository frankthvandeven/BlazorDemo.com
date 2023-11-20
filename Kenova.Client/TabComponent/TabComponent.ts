
class TabComponent {
    dotNetReference: dotNetHandler;
    selected_index: number;
    container: HTMLElement;
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

    Start(container_id: string, selected_index: number) {
        this.selected_index = selected_index;
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
            this.KNCalculateTabBar();
        });

        this.resizeObserver.observe(this.container); // observe() will call the callback right away, without any resize

        this.container.addEventListener('keydown', this.handleKeydown, false);
        this.container.addEventListener('focusin', this.handleFocusin, false);
        this.container.addEventListener('focusout', this.handleFocusout, false);
        this.container.addEventListener('click', this.handleClick, false);

    }

    SetSelectedIndex(selected_index) {
        this.selected_index = selected_index;
        this.KNCalculateTabBar();
    }

    Stop() {
        this.container.removeEventListener('click', this.handleClick, false);
        this.container.removeEventListener('focusout', this.handleFocusout, false);
        this.container.removeEventListener('focusin', this.handleFocusin, false);
        this.container.removeEventListener('keydown', this.handleKeydown, false);
        this.resizeObserver.unobserve(this.container);
    }

    KNCalculateTabBar() {
        let container = this.container;
        let children = container.children as HTMLCollectionOf<HTMLElement>
        let i;
        if (children.length == 0) {
            return;
        }
        // Make all elements visible
        for (i = 0; i < children.length; i++) {
            children[i].style.removeProperty('display');
        }
        let totalwidth = (children.length - 1) * 8.0; // gap
        // Measure the total width of all tabs
        for (i = 0; i < children.length - 1; i++) {
            totalwidth += children[i].offsetWidth;
        }
        let overflowButton = false;
        let availableWidth = container.offsetWidth;
        if (totalwidth > container.offsetWidth) {
            overflowButton = true;
            availableWidth -= children[children.length - 1].offsetWidth; // width of overflow button
            availableWidth -= 8.0; // gap left of overflow button
        }
        totalwidth = 0.0;
        let selected_visible = false;
        if (this.selected_index != -1) {
            for (i = 0; i < children.length - 1; i++) {
                if (i != 0) totalwidth += 8.0; // gap
                totalwidth += children[i].offsetWidth;
                if (totalwidth > availableWidth) break;
                if (i == this.selected_index) selected_visible = true;
            }
        }

        totalwidth = 0.0;
        let firstVisible = 0;
        let lastVisible = -1;
        let visibleCount = 0;
        if (selected_visible || this.selected_index == -1) {
            // count from zero
            for (i = 0; i < children.length - 1; i++) {
                if (i != 0) totalwidth += 8.0; // gap
                totalwidth += children[i].offsetWidth;
                if (totalwidth <= availableWidth) {
                    visibleCount++;
                    lastVisible = i;
                }
                else {
                    children[i].style.display = "none";
                }
            }
        }
        else {
            // count backwards (length - 2 means skipping the overflow button)
            for (let i = children.length - 2; i >= 0; i--) {
                if (i > this.selected_index) {
                    children[i].style.display = "none"; // hide the overflowing tab
                }
                else {
                    if (i != this.selected_index) totalwidth += 8; // gap
                    totalwidth += children[i].offsetWidth;
                    if (totalwidth <= availableWidth) {
                        if (i > lastVisible) {
                            lastVisible = i;
                        }
                        firstVisible = i;
                        visibleCount++;
                    }
                    else {
                        children[i].style.display = "none"; // hide the element
                    }
                }
            }
        }
        if (!overflowButton) {
            children[children.length - 1].style.display = "none";
        }
        if (visibleCount == 0) {
            let firstVisible = -1;
            let lastVisible = -1;
        }

        this.first_visible = firstVisible;
        this.last_visible = lastVisible;
        this.visible_count = visibleCount;
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
            this.dotNetReference.invokeMethodAsync('OnOverflowClicked');
            return;
        }

        for (let index = this.first_visible; index <= this.last_visible; index++) {
            //console.log('Checking inside click for index ' + index + 'first vis ' + this.first_visible + ' last vis ' + this.last_visible);
            if (this.didClickElement(e, this.container.children[index])) {
                this.dotNetReference.invokeMethodAsync('OnTabClicked', index);
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

        let focus_index = this.GetRovingTabZero();
        let buttons = this.container.children as HTMLCollectionOf<HTMLElement>;

        if (e.keyCode === 38 || e.keyCode === 37) { // up, left

            if (focus_index == -2) {
                if (this.visible_count != 0) {
                    buttons[this.last_visible].focus();
                }
            }
            else if (focus_index == this.first_visible) {
                if (this.overflow_button_visible) {
                    this.overflow_button_element.focus();
                }
                else {
                    buttons[this.last_visible].focus();
                }
            }
            else if (focus_index > this.first_visible) {

                buttons[focus_index - 1].focus();
            }

            e.preventDefault();
            e.stopPropagation();

        }
        else if (e.keyCode === 40 || e.keyCode === 39) { // down, right

            if (focus_index == -2) {
                if (this.visible_count != 0) {
                    buttons[this.first_visible].focus();
                }
            }
            else if (focus_index == this.last_visible) {
                if (this.overflow_button_visible) {
                    this.overflow_button_element.focus();
                }
                else {
                    buttons[this.first_visible].focus();
                }
            }
            else if (focus_index >= this.first_visible && focus_index < this.last_visible) {

                buttons[focus_index + 1].focus();
            }

            e.preventDefault();
            e.stopPropagation();

        }
        else if (e.keyCode === 13) { // enter

            if (focus_index == -2) {
                this.dotNetReference.invokeMethodAsync('OnOverflowClicked');
            }
            else if (focus_index >= this.first_visible) {
                this.dotNetReference.invokeMethodAsync('OnEnterPressed', focus_index);
            }

            e.preventDefault();
            e.stopPropagation();

        }
        else if (e.keyCode === 32) { // space

            if (focus_index == -2) {
                this.dotNetReference.invokeMethodAsync('OnOverflowClicked');
            }
            else if (focus_index >= this.first_visible) {
                this.dotNetReference.invokeMethodAsync('OnSpacePressed', focus_index);
            }

            e.stopPropagation();
            e.preventDefault();

        }
        else if (e.keyCode === 36) { // home

            if (this.visible_count > 0) {
                buttons[this.first_visible].focus();
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
    // a) Container DIV resize
    // b) Setting the selected tab
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

            if (tab_index_0 == -1 && this.first_visible != -1) {
                tab_index_0 = this.first_visible;
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

        for (let i = 0; i < buttons.length - 1; i++) {
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

