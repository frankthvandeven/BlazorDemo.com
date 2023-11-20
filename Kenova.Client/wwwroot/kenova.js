class KenovaDialogBaseComponent {
    constructor(dotNetReference) {
        this.handleKeydown = (e) => {
            if (e.ctrlKey && !e.shiftKey && !e.altKey && !e.metaKey) {
                if (e.keyCode === 13) {
                    this.dotNetReference.invokeMethodAsync('OnEnterPressed');
                    e.preventDefault();
                }
            }
        };
        this.getTabbableElements = () => {
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
        };
        this.handleTopFocusin = (e) => {
            let stops = this.getTabbableElements();
            let element = stops[stops.length - 2];
            element.focus();
        };
        this.handleBottomFocusin = (e) => {
            let stops = this.getTabbableElements();
            let element = stops[1];
            element.focus();
        };
        this.handleFocusin = (e) => {
            let focussed_element = e.srcElement;
            if (focussed_element.hasAttribute('_kn_ctrl_enter') == false) {
                return;
            }
            this.button_has_focus = true;
            let list = this.container.querySelectorAll("div[_kn_defaultbutton]");
            for (let i = 0; i < list.length; i++) {
                let element = list[i];
                element.classList.remove('btn-default');
            }
        };
        this.handleFocusout = (e) => {
            this.button_has_focus = false;
            let list = this.container.querySelectorAll("div[_kn_defaultbutton]");
            for (let i = 0; i < list.length; i++) {
                let element = list[i];
                if (i == 0) {
                    element.classList.add('btn-default');
                }
            }
        };
        this.RefreshButtons = () => {
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
        };
        this.dotNetReference = dotNetReference;
    }
    dispose() { }
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
}
class DateRangePickerComponent {
    constructor(dotNetReference) {
        this.handleKeydown = (e) => {
            if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
                return;
        };
        this.handleFocusin = (e) => {
        };
        this.handleFocusout = (e) => {
        };
        this.OnAfterRender = () => {
        };
        this.dotNetReference = dotNetReference;
    }
    dispose() { }
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
}
class FlexListComponent {
    constructor(dotNetReference) {
        this.OnAfterRender = () => {
            this.CreateTabZeroElement();
        };
        this.SetFocus = () => {
            this.CreateTabZeroElement();
            let focus_index = this.findFocusIndex();
            let rows = this.container.children;
            if (focus_index != -1) {
                rows[focus_index].focus();
            }
        };
        this.handleMouseDown = (e) => {
            let rows = this.container.children;
            for (let i = 0; i < rows.length; i++) {
                let row_element = rows[i];
                if (this.didClickElement(e, row_element)) {
                    row_element.tabIndex = 0;
                    row_element.focus();
                }
                else {
                    row_element.removeAttribute('tabindex');
                }
            }
        };
        this.didClickElement = (e, element) => {
            return element == e.target || element.contains(e.target);
        };
        this.findFocusIndex = () => {
            let rows = this.container.children;
            for (let i = 0; i < rows.length; i++) {
                if (rows[i].tabIndex == 0) {
                    return i;
                }
            }
            return -1;
        };
        this.ChangeTabZeroElement = (element_to_focus) => {
            let rows = this.container.children;
            for (let i = 0; i < rows.length; i++) {
                let row_element = rows[i];
                if (row_element === element_to_focus) {
                    row_element.tabIndex = 0;
                }
                else {
                    row_element.removeAttribute('tabindex');
                }
            }
        };
        this.CreateTabZeroElement = () => {
            let rows = this.container.children;
            let tab_index_0_found = false;
            for (let i = 0; i < rows.length; i++) {
                if (rows[i].tabIndex == 0) {
                    tab_index_0_found = true;
                    break;
                }
            }
            if (tab_index_0_found == false) {
                rows[0].tabIndex = 0;
            }
        };
        this.handleKeydown = (e) => {
            if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
                return;
            if (e.keyCode === 38) {
                this.KeyUpArrowPressed();
                e.preventDefault();
            }
            else if (e.keyCode === 40) {
                this.KeyDownArrowPressed();
                e.preventDefault();
            }
            else if (e.keyCode === 36) {
                this.KeyHomePressed();
                e.preventDefault();
            }
            else if (e.keyCode === 35) {
                this.KeyEndPressed();
                e.preventDefault();
            }
            else if (e.keyCode === 13) {
                this.KeyEnterPressed();
                e.preventDefault();
                e.stopPropagation();
            }
            else if (e.keyCode === 32) {
                this.KeySpacePressed();
                e.preventDefault();
                e.stopPropagation();
            }
        };
        this.KeyEnterPressed = () => {
            let focus_index = this.findFocusIndex();
            if (focus_index != -1) {
                this.dotNetReference.invokeMethodAsync('OnEnterPressed', focus_index);
            }
        };
        this.KeySpacePressed = () => {
            let focus_index = this.findFocusIndex();
            if (focus_index != -1) {
                this.dotNetReference.invokeMethodAsync('OnSpacePressed', focus_index);
            }
        };
        this.KeyDownArrowPressed = () => {
            let focus_index = this.findFocusIndex();
            if (focus_index < this.container.children.length - 1) {
                focus_index++;
                let element = this.container.children[focus_index];
                this.ChangeTabZeroElement(element);
                element.focus();
            }
        };
        this.KeyUpArrowPressed = () => {
            let focus_index = this.findFocusIndex();
            if (focus_index > 0) {
                focus_index--;
                let element = this.container.children[focus_index];
                this.ChangeTabZeroElement(element);
                element.focus();
            }
        };
        this.KeyHomePressed = () => {
            let focus_index = 0;
            let element = this.container.children[focus_index];
            this.ChangeTabZeroElement(element);
            element.focus();
        };
        this.KeyEndPressed = () => {
            let focus_index = this.container.children.length - 1;
            let element = this.container.children[focus_index];
            this.ChangeTabZeroElement(element);
            element.focus();
        };
        this.dotNetReference = dotNetReference;
    }
    dispose() { }
    Start(container_id) {
        this.container = document.getElementById(container_id);
        this.container.addEventListener('keydown', this.handleKeydown, false);
        this.container.addEventListener('mousedown', this.handleMouseDown, false);
    }
    Stop() {
        this.container.removeEventListener('mousedown', this.handleMouseDown, false);
        this.container.removeEventListener('keydown', this.handleKeydown, false);
    }
}
class HyperGridComponent {
    constructor(dotNetReference) {
        this.handleInputElementFocus = (e) => {
            this.permanent_focus = true;
            this.instantCalculateTabindexZero();
        };
        this.handleInputElementBlur = (e) => {
            this.permanent_focus = false;
            this.instantCalculateTabindexZero();
        };
        this.handleInputElementKeyDown = (e) => {
            if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
                return;
            if (e.keyCode === 38) {
                this.KeyUpArrowPressed();
                e.preventDefault();
            }
            else if (e.keyCode === 40) {
                this.KeyDownArrowPressed();
                e.preventDefault();
            }
            else if (e.keyCode === 36) {
                this.KeyHomePressed();
                e.preventDefault();
            }
            else if (e.keyCode === 35) {
                this.KeyEndPressed();
                e.preventDefault();
            }
            else if (e.keyCode === 33) {
                this.KeyPageUpPressed();
                e.preventDefault();
            }
            else if (e.keyCode === 34) {
                this.KeyPageDownPressed();
                e.preventDefault();
            }
            else if (e.keyCode === 13) {
                this.KeyEnterPressed();
                e.preventDefault();
                e.stopPropagation();
            }
        };
        this.SetFocus = () => {
            this.instantCalculateTabindexZero();
            let focus_index = this.findFocusIndex();
            let rows = this.table_element.children;
            if (focus_index != -1) {
                rows[focus_index].focus();
            }
        };
        this.handleScrollsync = () => {
            this.header_element.scrollLeft = this.table_element.scrollLeft;
        };
        this.findElementHeight = (table_element) => {
            let height = null;
            if (table_element.children.length >= 2) {
                let r = table_element.children[1];
                if (r.className.includes("kn-hyper-datarow")) {
                    height = r.offsetHeight;
                }
            }
            else if (table_element.children.length == 1) {
                let r = table_element.children[0];
                if (r.className.includes("kn-hyper-datarow")) {
                    height = r.offsetHeight;
                }
            }
            return height;
        };
        this.handleMouseDown = (e) => {
            let rows = this.table_element.children;
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
        };
        this.didClickElement = (e, element) => {
            return element == e.target || element.contains(e.target);
        };
        this.handleScroll = (e) => {
            this.delayedCalculateTabindexZero();
        };
        this.findFocusIndex = () => {
            let rows = this.table_element.children;
            for (let i = 0; i < rows.length; i++) {
                if (rows[i].tabIndex == 0) {
                    return i;
                }
            }
            return -1;
        };
        this.makeFocussedElement = (element_to_focus) => {
            let rows = this.table_element.children;
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
                }
                else {
                    row_element.removeAttribute('tabindex');
                    row_element.style.removeProperty('outline-style');
                }
            }
        };
        this.OnAfterRender = () => {
            this.delayedCalculateTabindexZero();
        };
        this.delayedCalculateTabindexZero = () => {
            if (this.timer_handle != null) {
                clearTimeout(this.timer_handle);
                this.timer_handle = null;
            }
            this.timer_handle = setTimeout(this.instantCalculateTabindexZero, 50);
        };
        this.instantCalculateTabindexZero = () => {
            let rows = this.table_element.children;
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
                        if (first == -1) {
                            first = i;
                        }
                        if (i > last) {
                            last = i;
                        }
                        if (row_element.tabIndex == 0) {
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
            this.first_in_view = first;
            this.last_in_view = last;
        };
        this.handleKeydown = (e) => {
            if (e.ctrlKey && !e.shiftKey && !e.altKey && !e.metaKey) {
                if (e.keyCode === 65) {
                    this.dotNetReference.invokeMethodAsync('SelectAllPressed');
                    e.preventDefault();
                    e.stopPropagation();
                }
                return;
            }
            if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
                return;
            if (e.keyCode === 38) {
                this.KeyUpArrowPressed();
                e.preventDefault();
            }
            else if (e.keyCode === 40) {
                this.KeyDownArrowPressed();
                e.preventDefault();
            }
            else if (e.keyCode === 36) {
                this.KeyHomePressed();
                e.preventDefault();
            }
            else if (e.keyCode === 35) {
                this.KeyEndPressed();
                e.preventDefault();
            }
            else if (e.keyCode === 33) {
                this.KeyPageUpPressed();
                e.preventDefault();
            }
            else if (e.keyCode === 34) {
                this.KeyPageDownPressed();
                e.preventDefault();
            }
            else if (e.keyCode === 13) {
                this.KeyEnterPressed();
                e.preventDefault();
                e.stopPropagation();
            }
            else if (e.keyCode === 32) {
                this.KeySpacePressed();
                e.preventDefault();
                e.stopPropagation();
            }
            else if (e.keyCode >= 48 && e.keyCode <= 57) {
                this.KeySelectPressed();
                e.preventDefault();
                e.stopPropagation();
            }
            else if (e.keyCode >= 65 && e.keyCode <= 90) {
                this.KeySelectPressed();
                e.preventDefault();
                e.stopPropagation();
            }
            else if (e.keyCode >= 96 && e.keyCode <= 111) {
                this.KeySelectPressed();
                e.preventDefault();
                e.stopPropagation();
            }
            else if (e.keyCode >= 186 && e.keyCode <= 223) {
                this.KeySelectPressed();
                e.preventDefault();
                e.stopPropagation();
            }
        };
        this.KeyEnterPressed = () => {
            let focus_index = this.findFocusIndex();
            let rows = this.table_element.children;
            if (focus_index != -1) {
                let row_element = rows[focus_index];
                let idx = parseInt(row_element.getAttribute('_idx'));
                this.dotNetReference.invokeMethodAsync('OnEnterPressed', idx);
            }
        };
        this.KeySpacePressed = () => {
            let focus_index = this.findFocusIndex();
            let rows = this.table_element.children;
            if (focus_index != -1) {
                let row_element = rows[focus_index];
                let idx = parseInt(row_element.getAttribute('_idx'));
                this.dotNetReference.invokeMethodAsync('OnSpacePressed', idx);
            }
        };
        this.KeySelectPressed = () => {
            let focus_index = this.findFocusIndex();
            let rows = this.table_element.children;
            if (focus_index != -1) {
                let row_element = rows[focus_index];
                let idx = parseInt(row_element.getAttribute('_idx'));
                this.dotNetReference.invokeMethodAsync('OnSelectPressed', idx);
            }
        };
        this.KeyDownArrowPressed = () => {
            let focus_index = this.findFocusIndex();
            let re_focus = this.table_element.contains(document.activeElement);
            let rows = this.table_element.children;
            let container = this.table_element;
            if (focus_index == this.last_in_view) {
                container.scrollTop += this.findElementHeight(container);
                setTimeout(() => {
                    this.instantCalculateTabindexZero();
                    if (this.last_in_view != -1) {
                        let element = container.children[this.last_in_view];
                        this.makeFocussedElement(element);
                        if (re_focus) {
                            element.focus();
                        }
                    }
                }, this.MILLISECONDS);
            }
            else if (focus_index < this.last_in_view) {
                let element = rows[focus_index + 1];
                this.makeFocussedElement(element);
                if (re_focus) {
                    element.focus();
                }
            }
        };
        this.KeyUpArrowPressed = () => {
            let focus_index = this.findFocusIndex();
            let re_focus = this.table_element.contains(document.activeElement);
            let rows = this.table_element.children;
            let container = this.table_element;
            if (focus_index == this.first_in_view) {
                container.scrollTop -= this.findElementHeight(container);
                setTimeout(() => {
                    this.instantCalculateTabindexZero();
                    if (this.first_in_view != -1) {
                        let element = container.children[this.first_in_view];
                        this.makeFocussedElement(element);
                        if (re_focus) {
                            element.focus();
                        }
                    }
                }, this.MILLISECONDS);
            }
            else if (focus_index > this.first_in_view) {
                let element = rows[focus_index - 1];
                this.makeFocussedElement(element);
                if (re_focus) {
                    element.focus();
                }
            }
        };
        this.KeyPageUpPressed = () => {
            let focus_index = this.findFocusIndex();
            let re_focus = this.table_element.contains(document.activeElement);
            let rows = this.table_element.children;
            let container = this.table_element;
            if (focus_index > this.first_in_view) {
                if (this.first_in_view != -1) {
                    let element = rows[this.first_in_view];
                    this.makeFocussedElement(element);
                    if (re_focus) {
                        element.focus();
                    }
                }
            }
            else {
                container.scrollTop -= container.clientHeight;
                setTimeout(() => {
                    this.instantCalculateTabindexZero();
                    if (this.first_in_view != -1) {
                        let element = container.children[this.first_in_view];
                        this.makeFocussedElement(element);
                        if (re_focus) {
                            element.focus();
                        }
                    }
                }, this.MILLISECONDS);
            }
        };
        this.KeyPageDownPressed = () => {
            let focus_index = this.findFocusIndex();
            let re_focus = this.table_element.contains(document.activeElement);
            let rows = this.table_element.children;
            let container = this.table_element;
            if (focus_index < this.last_in_view) {
                if (this.last_in_view != -1) {
                    let element = rows[this.last_in_view];
                    this.makeFocussedElement(element);
                    if (re_focus) {
                        element.focus();
                    }
                }
            }
            else {
                container.scrollTop += container.clientHeight;
                setTimeout(() => {
                    this.instantCalculateTabindexZero();
                    if (this.last_in_view != -1) {
                        let element = container.children[this.last_in_view];
                        this.makeFocussedElement(element);
                        if (re_focus) {
                            element.focus();
                        }
                    }
                }, this.MILLISECONDS);
            }
        };
        this.KeyHomePressed = () => {
            let re_focus = this.table_element.contains(document.activeElement);
            let container = this.table_element;
            container.scrollTop = 0;
            setTimeout(() => {
                this.instantCalculateTabindexZero();
                if (this.first_in_view != -1) {
                    let element = container.children[this.first_in_view];
                    this.makeFocussedElement(element);
                    if (re_focus) {
                        element.focus();
                    }
                }
            }, this.MILLISECONDS);
        };
        this.KeyEndPressed = () => {
            let re_focus = this.table_element.contains(document.activeElement);
            let container = this.table_element;
            container.scrollTop = container.scrollHeight;
            setTimeout(() => {
                this.instantCalculateTabindexZero();
                if (this.last_in_view != -1) {
                    let element = container.children[this.last_in_view];
                    this.makeFocussedElement(element);
                    if (re_focus) {
                        element.focus();
                    }
                }
            }, this.MILLISECONDS);
        };
        this.dotNetReference = dotNetReference;
    }
    dispose() { }
    Start(virtualize, container_id, table_id, input_element_id) {
        this.virtualize = virtualize;
        this.container = document.getElementById(container_id);
        this.table_element = document.getElementById(table_id);
        this.input_element = document.getElementById(input_element_id);
        this.first_in_view = -1;
        this.last_in_view = -1;
        this.timer_handle = null;
        this.MILLISECONDS = 0;
        this.permanent_focus = false;
        if (this.virtualize) {
            this.MILLISECONDS = 25;
        }
        this.table_element.addEventListener('keydown', this.handleKeydown, false);
        this.table_element.addEventListener('scroll', this.handleScroll, false);
        this.table_element.addEventListener('mousedown', this.handleMouseDown, false);
        if (this.input_element != null) {
            this.input_element.addEventListener('focus', this.handleInputElementFocus, false);
            this.input_element.addEventListener('blur', this.handleInputElementBlur, false);
            this.input_element.addEventListener('keydown', this.handleInputElementKeyDown, false);
        }
    }
    Stop() {
        if (this.input_element != null) {
            this.input_element.addEventListener('focus', this.handleInputElementFocus, false);
            this.input_element.addEventListener('blur', this.handleInputElementBlur, false);
            this.input_element.addEventListener('keydown', this.handleInputElementKeyDown, false);
        }
        this.table_element.removeEventListener('mousedown', this.handleMouseDown, false);
        this.table_element.removeEventListener('scroll', this.handleScroll, false);
        this.table_element.removeEventListener('keydown', this.handleKeydown, false);
    }
    KNStartScrollSync(header_id) {
        this.header_element = document.getElementById(header_id);
        this.table_element.addEventListener('scroll', this.handleScrollsync, false);
    }
    KNStopScrollSync() {
        this.table_element.removeEventListener('scroll', this.handleScrollsync, false);
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
        let half_visible = elemHeight / 2;
        return (elemTop >= (containerTop - half_visible) && elemBottom <= (containerBottom + half_visible));
    }
}
class InputMultilineComponent {
    constructor(dotNetReference) {
        this.updateAutoSize = () => {
            this.element.style.height = 'auto';
            this.element.style.height = (this.element.scrollHeight + this.offset) + 'px';
        };
        this.handleKeydown = (e) => {
            if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
                return;
            if (e.keyCode === 13) {
                e.stopPropagation();
            }
        };
        this.dotNetReference = dotNetReference;
    }
    dispose() { }
    Start(container_id) {
        this.element = document.getElementById(container_id);
        this.element.style.boxSizing = 'border-box';
        this.offset = this.element.offsetHeight - this.element.clientHeight;
        this.updateAutoSize();
        this.element.addEventListener('input', this.updateAutoSize, false);
        this.element.addEventListener('keydown', this.handleKeydown, false);
    }
    Stop() {
        this.element.removeEventListener('keydown', this.handleKeydown, false);
        this.element.removeEventListener('input', this.updateAutoSize, false);
    }
    ResetHeight() {
        this.updateAutoSize();
    }
}
class PortalRootComponent {
    constructor(dotNetReference) {
        this.handleKeydown = (e) => {
            if (e.ctrlKey && !e.shiftKey && !e.altKey && !e.metaKey) {
                if (e.keyCode === 191) {
                    this.dotNetReference.invokeMethodAsync('OnSearchKeyPressed');
                    e.preventDefault();
                }
                return;
            }
            if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
                return;
            if (e.keyCode === 27) {
                this.dotNetReference.invokeMethodAsync('OnEscapePressed');
                e.preventDefault();
            }
        };
        this.dotNetReference = dotNetReference;
    }
    dispose() { }
    Start() {
        document.addEventListener('keydown', this.handleKeydown, false);
    }
    Stop() {
        document.removeEventListener('keydown', this.handleKeydown, false);
    }
}
class TabComponent {
    constructor(dotNetReference) {
        this.handleFocusin = (e) => {
            this.SetRovingTabZero(e.srcElement);
        };
        this.handleFocusout = (e) => {
        };
        this.handleClick = (e) => {
            if (this.didClickElement(e, this.overflow_button_element)) {
                this.dotNetReference.invokeMethodAsync('OnOverflowClicked');
                return;
            }
            for (let index = this.first_visible; index <= this.last_visible; index++) {
                if (this.didClickElement(e, this.container.children[index])) {
                    this.dotNetReference.invokeMethodAsync('OnTabClicked', index);
                    return;
                }
            }
        };
        this.didClickElement = (e, element) => {
            return element == e.target || element.contains(e.target);
        };
        this.handleKeydown = (e) => {
            if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
                return;
            let focus_index = this.GetRovingTabZero();
            let buttons = this.container.children;
            if (e.keyCode === 38 || e.keyCode === 37) {
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
            else if (e.keyCode === 40 || e.keyCode === 39) {
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
            else if (e.keyCode === 13) {
                if (focus_index == -2) {
                    this.dotNetReference.invokeMethodAsync('OnOverflowClicked');
                }
                else if (focus_index >= this.first_visible) {
                    this.dotNetReference.invokeMethodAsync('OnEnterPressed', focus_index);
                }
                e.preventDefault();
                e.stopPropagation();
            }
            else if (e.keyCode === 32) {
                if (focus_index == -2) {
                    this.dotNetReference.invokeMethodAsync('OnOverflowClicked');
                }
                else if (focus_index >= this.first_visible) {
                    this.dotNetReference.invokeMethodAsync('OnSpacePressed', focus_index);
                }
                e.stopPropagation();
                e.preventDefault();
            }
            else if (e.keyCode === 36) {
                if (this.visible_count > 0) {
                    buttons[this.first_visible].focus();
                }
                e.preventDefault();
                e.stopPropagation();
            }
            else if (e.keyCode === 35) {
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
        };
        this.CalculateTabindexZero = () => {
            let buttons = this.container.children;
            let tab_index_0 = -1;
            for (let i = 0; i < buttons.length; i++) {
                if (buttons[i].tabIndex == 0) {
                    tab_index_0 = i;
                    break;
                }
            }
            if (tab_index_0 == -1) {
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
        };
        this.SetRovingTabZero = (focussed_element) => {
            let buttons = this.container.children;
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
        };
        this.GetRovingTabZero = () => {
            let buttons = this.container.children;
            if (this.overflow_button_element.tabIndex == 0) {
                return -2;
            }
            for (let i = 0; i < buttons.length - 1; i++) {
                if (buttons[i].tabIndex == 0) {
                    return i;
                }
            }
            return -1;
        };
        this.FocusByIndex = (index) => {
            let buttons = this.container.children;
            buttons[index].focus();
        };
        this.dotNetReference = dotNetReference;
    }
    dispose() { }
    Start(container_id, selected_index) {
        this.selected_index = selected_index;
        this.container = document.getElementById(container_id);
        this.first_visible = -1;
        this.last_visible = -1;
        this.visible_count = -1;
        this.overflow_button_visible = false;
        this.overflow_button_element = null;
        this.resizeObserver = new ResizeObserver(entries => {
            this.KNCalculateTabBar();
        });
        this.resizeObserver.observe(this.container);
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
        let children = container.children;
        let i;
        if (children.length == 0) {
            return;
        }
        for (i = 0; i < children.length; i++) {
            children[i].style.removeProperty('display');
        }
        let totalwidth = (children.length - 1) * 8.0;
        for (i = 0; i < children.length - 1; i++) {
            totalwidth += children[i].offsetWidth;
        }
        let overflowButton = false;
        let availableWidth = container.offsetWidth;
        if (totalwidth > container.offsetWidth) {
            overflowButton = true;
            availableWidth -= children[children.length - 1].offsetWidth;
            availableWidth -= 8.0;
        }
        totalwidth = 0.0;
        let selected_visible = false;
        if (this.selected_index != -1) {
            for (i = 0; i < children.length - 1; i++) {
                if (i != 0)
                    totalwidth += 8.0;
                totalwidth += children[i].offsetWidth;
                if (totalwidth > availableWidth)
                    break;
                if (i == this.selected_index)
                    selected_visible = true;
            }
        }
        totalwidth = 0.0;
        let firstVisible = 0;
        let lastVisible = -1;
        let visibleCount = 0;
        if (selected_visible || this.selected_index == -1) {
            for (i = 0; i < children.length - 1; i++) {
                if (i != 0)
                    totalwidth += 8.0;
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
            for (let i = children.length - 2; i >= 0; i--) {
                if (i > this.selected_index) {
                    children[i].style.display = "none";
                }
                else {
                    if (i != this.selected_index)
                        totalwidth += 8;
                    totalwidth += children[i].offsetWidth;
                    if (totalwidth <= availableWidth) {
                        if (i > lastVisible) {
                            lastVisible = i;
                        }
                        firstVisible = i;
                        visibleCount++;
                    }
                    else {
                        children[i].style.display = "none";
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
}
class ToolbarComponent {
    constructor(dotNetReference) {
        this.KNMeasureOverflow = () => {
            let container = this.container;
            let children = container.children;
            let i;
            for (i = 0; i < children.length; i++) {
                children[i].style.removeProperty('display');
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
                    children[i].tabIndex = -1;
                }
            }
            if (overflowButton == false) {
                children[children.length - 1].style.display = "none";
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
        };
        this.handleFocusin = (e) => {
            this.SetRovingTabZero(e.srcElement);
        };
        this.handleFocusout = (e) => {
        };
        this.handleClick = (e) => {
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
        };
        this.didClickElement = (e, element) => {
            return element == e.target || element.contains(e.target);
        };
        this.handleKeydown = (e) => {
            if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
                return;
            let focus_index = this.GetRovingTabZero();
            let buttons = this.container.children;
            if (e.keyCode === 38 || e.keyCode === 37) {
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
            else if (e.keyCode === 40 || e.keyCode === 39) {
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
            else if (e.keyCode === 13) {
                if (focus_index == -2) {
                    this.dotNetReference.invokeMethodAsync('OnOverflowClicked', this.visible_count);
                }
                else if (focus_index >= 0) {
                    this.dotNetReference.invokeMethodAsync('OnButtonClicked', focus_index);
                }
                e.preventDefault();
                e.stopPropagation();
            }
            else if (e.keyCode === 32) {
                if (focus_index == -2) {
                    this.dotNetReference.invokeMethodAsync('OnOverflowClicked', this.visible_count);
                }
                else if (focus_index >= 0) {
                    this.dotNetReference.invokeMethodAsync('OnSpacePressed', focus_index);
                }
                e.stopPropagation();
                e.preventDefault();
            }
            else if (e.keyCode === 36) {
                if (this.visible_count > 0) {
                    buttons[0].focus();
                }
                e.preventDefault();
                e.stopPropagation();
            }
            else if (e.keyCode === 35) {
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
        };
        this.CalculateTabindexZero = () => {
            let buttons = this.container.children;
            let tab_index_0 = -1;
            for (let i = 0; i < buttons.length; i++) {
                if (buttons[i].tabIndex == 0) {
                    tab_index_0 = i;
                    break;
                }
            }
            if (tab_index_0 == -1) {
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
        };
        this.SetRovingTabZero = (focussed_element) => {
            let buttons = this.container.children;
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
        };
        this.GetRovingTabZero = () => {
            let buttons = this.container.children;
            if (this.overflow_button_element.tabIndex == 0) {
                return -2;
            }
            for (let i = 0; i < this.visible_count; i++) {
                if (buttons[i].tabIndex == 0) {
                    return i;
                }
            }
            return -1;
        };
        this.FocusByIndex = (index) => {
            let buttons = this.container.children;
            buttons[index].focus();
        };
        this.dotNetReference = dotNetReference;
    }
    dispose() { }
    Start(container_id) {
        this.container = document.getElementById(container_id);
        this.first_visible = -1;
        this.last_visible = -1;
        this.visible_count = -1;
        this.overflow_button_visible = false;
        this.overflow_button_element = null;
        this.resizeObserver = new ResizeObserver(entries => {
            this.KNMeasureOverflow();
        });
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
}
class ButtonGroupComponent {
    constructor(dotNetReference) {
        this.handleKeydown = (e) => {
            if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
                return;
            this.CalculateTabindexZero();
            let focus_index = this.GetRovingTabZero();
            let buttons = this.container.children;
            if (e.keyCode === 38 || e.keyCode === 37) {
                if (!this.is_first) {
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
            else if (e.keyCode === 40 || e.keyCode === 39) {
                if (!this.is_last) {
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
            else if (e.keyCode === 13) {
                if (focus_index != -1) {
                    this.dotNetReference.invokeMethodAsync('OnEnterPressed', focus_index);
                }
                e.preventDefault();
                e.stopPropagation();
            }
            else if (e.keyCode === 32) {
                if (focus_index != -1) {
                    this.dotNetReference.invokeMethodAsync('OnSpacePressed', focus_index);
                }
                e.stopPropagation();
                e.preventDefault();
            }
            else if (e.keyCode === 36) {
                buttons[0].focus();
                e.preventDefault();
                e.stopPropagation();
            }
            else if (e.keyCode === 35) {
                buttons[buttons.length - 1].focus();
                e.preventDefault();
                e.stopPropagation();
            }
        };
        this.handleFocusin = (e) => {
            this.SetRovingTabZero(e.srcElement);
        };
        this.handleFocusout = (e) => {
        };
        this.CalculateTabindexZero = () => {
            let buttons = this.container.children;
            let tab_index_0 = -1;
            for (let i = 0; i < buttons.length; i++) {
                if (buttons[i].tabIndex == 0) {
                    tab_index_0 = i;
                    break;
                }
            }
            if (tab_index_0 == -1) {
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
        };
        this.SetRovingTabZero = (focussed_element) => {
            let buttons = this.container.children;
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
        };
        this.GetRovingTabZero = () => {
            let buttons = this.container.children;
            for (let i = 0; i < buttons.length; i++) {
                if (buttons[i].tabIndex == 0) {
                    return i;
                }
            }
            return -1;
        };
        this.FocusByIndex = (index) => {
            let buttons = this.container.children;
            buttons[index].focus();
        };
        this.dotNetReference = dotNetReference;
    }
    dispose() { }
    Start(container_id) {
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
}
class InputBaseComponent {
    constructor(dotNetReference) {
        this.handleKeydown = (e) => {
            if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
                return;
            if (e.keyCode === 120) {
                if (this.has_zoombutton) {
                    this.dotNetReference.invokeMethodAsync('KeyZoomPressed');
                    e.stopPropagation();
                }
                else if (this.has_dropdownbutton) {
                    this.dotNetReference.invokeMethodAsync('KeyDropdownPressed');
                    e.stopPropagation();
                }
            }
            else if (e.keyCode === 13) {
                if (this.has_dropdownbutton) {
                    this.dotNetReference.invokeMethodAsync('KeyDropdownPressed');
                    e.stopPropagation();
                }
                else if (this.has_zoombutton) {
                    this.dotNetReference.invokeMethodAsync('KeyZoomPressed');
                    e.stopPropagation();
                }
            }
        };
        this.ComponentHidden = () => {
            return window.getComputedStyle(this.inputbox).display === "none";
        };
        this.dotNetReference = dotNetReference;
    }
    dispose() { }
    Start(inputbox_id, has_zoombutton, has_dropdownbutton) {
        this.inputbox = document.getElementById(inputbox_id);
        this.has_zoombutton = has_zoombutton;
        this.has_dropdownbutton = has_dropdownbutton;
        this.inputbox.addEventListener('keydown', this.handleKeydown, false);
    }
    Stop() {
        this.inputbox.removeEventListener('keydown', this.handleKeydown, false);
    }
}
class SingleButtonComponent {
    constructor(dotNetReference) {
        this.handleKeydown = (e) => {
            if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
                return;
            if (e.keyCode === 13) {
                this.dotNetReference.invokeMethodAsync('OnEnterPressed');
                e.preventDefault();
                e.stopPropagation();
            }
            else if (e.keyCode === 32) {
                this.dotNetReference.invokeMethodAsync('OnSpacePressed');
                e.stopPropagation();
                e.preventDefault();
            }
        };
        this.dotNetReference = dotNetReference;
    }
    dispose() { }
    Start(container_id) {
        this.container = document.getElementById(container_id);
        this.container.addEventListener('keydown', this.handleKeydown, false);
    }
    Stop() {
        this.container.removeEventListener('keydown', this.handleKeydown, false);
    }
}
{
    const GetDeviceType = () => {
        const ua = navigator.userAgent;
        if (/(tablet|ipad|playbook|silk)|(android(?!.*mobi))/i.test(ua))
            return "tablet";
        if (/Mobile|iP(hone|od|ad)|Android|BlackBerry|IEMobile|Kindle|Silk-Accelerated|(hpw|web)OS|Opera M(obi|ini)/.test(ua))
            return "mobile";
        return "desktop";
    };
    let ignore_sr_events = false;
    let device_type = GetDeviceType();
    let layerStack = new Array();
    let saved_focussed_element = null;
    let image_canvas = null;
    var KNCreateClassByName = (diagnostics, classname, dotNetReference) => {
        if (diagnostics)
            console.log("✈️ JavaScript - Instantiating class with name " + classname + " for ComponentWingman");
        var c = eval(classname);
        return new c(dotNetReference);
    };
    var KNGetStartingCaption = () => {
        let caption = "Starting";
        if (localStorage.portal_language == "nl") {
            caption = "Starten";
        }
        return caption;
    };
    var KNSaveCurrentFocus = () => {
        saved_focussed_element = document.activeElement;
        if (document.activeElement != null && document.activeElement instanceof HTMLElement) {
            document.activeElement.blur();
        }
    };
    var KNAddLayer = (is_dropdown, dropdown_id, overlay_id) => {
        const renderComplete = () => {
            const item = {
                is_dropdown: is_dropdown,
                dropdown_element: null,
                dropdown_element_top: null,
                dropdown_element_left: null,
                overlay_element: null,
                focus_element: saved_focussed_element,
            };
            saved_focussed_element = null;
            if (is_dropdown && dropdown_id != null) {
                item.dropdown_element = document.getElementById(dropdown_id);
                if (item.dropdown_element == null) {
                    throw new Error("KNAddLayer dropdown_element '" + dropdown_id + "' not rendered yet. Timing problem.");
                }
                let viewportOffset = item.dropdown_element.getBoundingClientRect();
                item.dropdown_element_top = viewportOffset.top;
                item.dropdown_element_left = viewportOffset.left;
            }
            if (is_dropdown && overlay_id != null) {
                item.overlay_element = document.getElementById(overlay_id);
                if (item.overlay_element == null) {
                    throw new Error("KNAddLayer overlay_element '" + overlay_id + "' not rendered yet. Timing problem.");
                }
            }
            layerStack.push(item);
        };
        requestAnimationFrame(renderComplete);
    };
    var KNRemoveLayer = () => {
        const renderComplete = () => {
            let item = layerStack.pop();
            if (item.focus_element == null)
                return;
            let parent = item.focus_element.parentElement;
            if (parent != null) {
                let inputNodes = parent.getElementsByTagName('INPUT');
                if (inputNodes.length == 1) {
                    let inputElement = inputNodes[0];
                    if (inputElement.className.includes("kn-inputbox-input-element")) {
                        setTimeout(function () { inputElement.focus(); inputElement.select(); }, 0);
                        return;
                    }
                }
            }
            item.focus_element.focus();
        };
        requestAnimationFrame(renderComplete);
    };
    const HandleMouseDown = (e) => {
        let closecount = 0;
        for (let i = layerStack.length - 1; i >= 0; i--) {
            let item = layerStack[i];
            if (!item.is_dropdown)
                break;
            if (didClickInside(item))
                break;
            closecount++;
        }
        if (closecount > 0)
            DotNet.invokeMethodAsync('Kenova.Client', 'OnCloseDropdownLayers', closecount);
    };
    const didClickInside = (item) => {
        if (item.dropdown_element != null && (item.dropdown_element === event.target || item.dropdown_element.contains(event.target))) {
            return true;
        }
        if (item.overlay_element != null && (item.overlay_element === event.target || item.overlay_element.contains(event.target))) {
            return true;
        }
        return false;
    };
    const HandleScroll = (e) => {
        if (ignore_sr_events)
            return;
        let closecount = 0;
        for (let i = layerStack.length - 1; i >= 0; i--) {
            let item = layerStack[i];
            if (!item.is_dropdown)
                break;
            if (!dropdownMoved(item))
                break;
            closecount++;
        }
        if (closecount > 0) {
            DotNet.invokeMethodAsync('Kenova.Client', 'OnCloseDropdownLayers', closecount);
        }
    };
    const dropdownMoved = (item) => {
        if (item.dropdown_element == null) {
            return false;
        }
        let viewportOffset = item.dropdown_element.getBoundingClientRect();
        if (item.dropdown_element_top != null && item.dropdown_element_top != viewportOffset.top) {
            return true;
        }
        if (item.dropdown_element_left != null && item.dropdown_element_left != viewportOffset.left) {
            return true;
        }
        return false;
    };
    const HandleResize = (e) => {
        if (ignore_sr_events)
            return;
        let closecount = 0;
        for (let i = layerStack.length - 1; i >= 0; i--) {
            let item = layerStack[i];
            if (!item.is_dropdown)
                break;
            closecount++;
        }
        if (closecount > 0) {
            DotNet.invokeMethodAsync('Kenova.Client', 'OnCloseDropdownLayers', closecount);
        }
    };
    var KNTriggerIgnoreScrollResizeEvents = () => {
        if (device_type == "desktop")
            ignore_sr_events = false;
        else {
            ignore_sr_events = true;
            setTimeout(function () { ignore_sr_events = false; }, 800);
        }
    };
    var KNGetBoundingClientRectByPos = (x, y) => {
        let element = document.elementFromPoint(x, y);
        return element.getBoundingClientRect();
    };
    var KNGetBoundingClientRectById = (element_id) => {
        let element = document.getElementById(element_id);
        return element.getBoundingClientRect();
    };
    var KNGetWindowInnerHeight = () => {
        return window.innerHeight;
    };
    var KNGetWindowInnerWidth = () => {
        return window.innerWidth;
    };
    var KNSelectAll = (element_id) => {
        let element = document.getElementById(element_id);
        if (element instanceof HTMLInputElement || element instanceof HTMLTextAreaElement) {
            element.select();
        }
    };
    var KNFocus = (element_id, select_all) => {
        let element = document.getElementById(element_id);
        element.focus();
        if (select_all && element instanceof HTMLInputElement || element instanceof HTMLTextAreaElement) {
            element.select();
        }
    };
    var KNUnfocus = () => {
        if (document.activeElement != null && document.activeElement instanceof HTMLElement)
            document.activeElement.blur();
    };
    var KNChildUnfocus = (element_id) => {
        let element = document.getElementById(element_id);
        if (element == null || document.activeElement == null)
            return;
        if (!element.contains(document.activeElement))
            return;
        if (document.activeElement instanceof HTMLElement) {
            document.activeElement.blur();
        }
    };
    var KNElementHidden = (element_id) => {
        let element = document.getElementById(element_id);
        return window.getComputedStyle(element).display === "none";
    };
    var KNIsChildOf = (parent_id, child_id) => {
        let parent_element = document.getElementById(parent_id);
        let child_element = document.getElementById(child_id);
        return parent_element.contains(child_element);
    };
    var KNGetCssVariable = (var_name) => {
        let comp = window.getComputedStyle(document.documentElement);
        return comp.getPropertyValue(var_name);
    };
    var KNSetCssVariable = (var_name, value) => {
        document.documentElement.style.setProperty(var_name, value);
    };
    var KNShowTaskrunner = (caption) => {
        let element = document.getElementById("kn-id-taskrunner-layer");
        let captionElement = document.getElementById("kn-id-taskrunner-caption");
        captionElement.textContent = caption;
        element.style.display = "flex";
    };
    var KNTaskrunnerDisplayFailure = (caption, message) => {
    };
    var KNHideTaskrunner = () => {
        let element = document.getElementById("kn-id-taskrunner-layer");
        element.style.display = "none";
    };
    var KNSaveAsFile2 = (filename, data) => {
        let blob = new Blob([data], { type: "application/octetstream" });
        let object_url = URL.createObjectURL(blob);
        let a = document.createElement("a");
        a.setAttribute("download", filename);
        a.setAttribute("href", object_url);
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        URL.revokeObjectURL(object_url);
        console.log("KNSaveAsFile2 complete");
    };
    var KNCopyTextToClipboard = (text) => {
        navigator.clipboard.writeText(text).then(function () {
        })
            .catch(function (error) {
            alert(error);
        });
    };
    var KNCopyElementTextToClipboard = (codeElement) => {
        navigator.clipboard.writeText(codeElement.textContent).then(function () {
        })
            .catch(function (error) {
            alert(error);
        });
    };
    document.addEventListener('mousedown', HandleMouseDown, true);
    document.addEventListener('scroll', HandleScroll, true);
    window.addEventListener('resize', HandleResize, true);
}
class WorkflowComponent {
    constructor(dotNetReference) {
        this.items = [];
        this.colorITEMBORDER = "#BBBBBB";
        this.grid = 5;
        this.connectorRadius = 8;
        this.dragging = false;
        this.drawing = false;
        this.lineStartConnector = null;
        this.lineStartX = -1;
        this.lineStartY = -1;
        this.lineEndX = -1;
        this.lineEndY = -1;
        this.SetWorkflowData = (data) => {
            this.items = data.items;
            this.draw();
        };
        this.ResizeCanvas = () => {
            this.canvas.width = this.container.clientWidth;
            this.canvas.height = this.container.clientHeight;
            this.draw();
        };
        this.OnAfterRender = () => {
        };
        this.SetFocus = () => {
        };
        this.handleMouseDown = (e) => {
        };
        this.didClickElement = (e, element) => {
        };
        this.findFocusIndex = () => {
        };
        this.draw = () => {
            this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
            for (var i = 0; i < this.items.length; i++) {
                this.drawWorkflowItem(this.items[i]);
            }
            for (var i = 0; i < this.items.length; i++) {
                let item = this.items[i];
                for (let i = 0; i < item.outConnectors.length; i++) {
                    let connector = item.outConnectors[i];
                    if (connector.connected === true) {
                        let target = this.findTargetConnector(connector.connectedTo, connector.connectedIndex);
                        if (target != null) {
                            this.ctx.beginPath();
                            this.ctx.moveTo(connector.x, connector.y);
                            this.ctx.lineTo(target.x, target.y);
                            this.ctx.lineWidth = 1.5;
                            this.ctx.strokeStyle = "green";
                            this.ctx.stroke();
                        }
                    }
                }
            }
            if (this.drawing === true) {
                this.ctx.beginPath();
                this.ctx.moveTo(this.lineStartX, this.lineStartY);
                this.ctx.lineTo(this.lineEndX, this.lineEndY);
                this.ctx.lineWidth = 1.5;
                this.ctx.strokeStyle = "red";
                this.ctx.stroke();
            }
            return;
            let start_pos_x = this.items[0].x + this.itemWIDTH;
            ;
            let start_pos_y = this.items[0].y + 12;
            let end_pos_x = this.items[1].x + this.itemWIDTH;
            let end_pos_y = this.items[1].y + 12;
            let distance_x = end_pos_x - start_pos_x;
            let half_x = start_pos_x + (distance_x / 2);
            this.ctx.beginPath();
            this.ctx.moveTo(start_pos_x, start_pos_y);
            this.ctx.lineTo(half_x, start_pos_y);
            this.ctx.lineTo(half_x, end_pos_y);
            this.ctx.lineTo(end_pos_x, end_pos_y);
            this.ctx.lineWidth = 1.5;
            this.ctx.strokeStyle = "red";
            this.ctx.stroke();
        };
        this.findTargetConnector = (id, index) => {
            let targetItem = this.items.find(e => e.id === id);
            if (targetItem === undefined)
                return null;
            return targetItem.inConnectors[index];
        };
        this.drawWorkflowItem = (item) => {
            this.ctx.lineWidth = 1.5;
            this.ctx.strokeStyle = this.colorITEMBORDER;
            this.roundRect(item.x, item.y, this.itemWIDTH, this.itemHEIGHT, 10);
            this.ctx.fillStyle = "white";
            this.ctx.fill();
            let x = item.x;
            let y = item.y + 10;
            for (let i = 0; i < item.inConnectors.length; i++) {
                let connector = item.inConnectors[i];
                connector.x = x;
                connector.y = y;
                this.drawConnector(connector);
                y += 20;
            }
            x = item.x + this.itemWIDTH;
            y = item.y + 10;
            for (let i = 0; i < item.outConnectors.length; i++) {
                let connector = item.outConnectors[i];
                connector.x = x;
                connector.y = y;
                this.drawConnector(connector);
                y += 20;
            }
        };
        this.drawConnector = (connector) => {
            this.ctx.beginPath();
            this.ctx.arc(connector.x, connector.y, this.connectorRadius, 0, 2 * Math.PI);
            this.ctx.stroke();
            if (connector.hovering === true) {
                this.ctx.fillStyle = "red";
            }
            else {
                this.ctx.fillStyle = "white";
            }
            this.ctx.fill();
        };
        this.mouseDown = (e) => {
            e.preventDefault();
            e.stopPropagation();
            let BCR = this.canvas.getBoundingClientRect();
            let mx = e.clientX - BCR.left;
            let my = e.clientY - BCR.top;
            let hitfound = false;
            for (var i = 0; i < this.items.length; i++) {
                let item = this.items[i];
                for (let i = 0; i < item.outConnectors.length; i++) {
                    let connector = item.outConnectors[i];
                    let hit = this.didHitConnector(mx, my, connector);
                    if (hit === true) {
                        hitfound = true;
                        this.drawing = true;
                        this.lineStartConnector = connector;
                        this.lineStartX = connector.x;
                        this.lineStartY = connector.y;
                        this.lineEndX = mx;
                        this.lineEndY = my;
                        return;
                    }
                }
            }
            this.dragging = false;
            for (var i = 0; i < this.items.length; i++) {
                let item = this.items[i];
                if (mx >= item.x &&
                    mx < item.x + this.itemWIDTH &&
                    my >= item.y &&
                    my < item.y + this.itemHEIGHT) {
                    this.dragging = true;
                    item.isDragging = true;
                    item.dragOffsetX = item.x - mx;
                    item.dragOffsetY = item.y - my;
                }
            }
        };
        this.mouseUp = (e) => {
            let BCR = this.canvas.getBoundingClientRect();
            let mx = e.clientX - BCR.left;
            let my = e.clientY - BCR.top;
            e.preventDefault();
            e.stopPropagation();
            if (this.drawing === true) {
                for (var i = 0; i < this.items.length; i++) {
                    let item = this.items[i];
                    for (let i = 0; i < item.inConnectors.length; i++) {
                        let connector = item.inConnectors[i];
                        let hit = this.didHitConnector(mx, my, connector);
                        if (hit === true) {
                            this.lineStartConnector.connected = true;
                            this.lineStartConnector.connectedTo = item.id;
                            this.lineStartConnector.connectedIndex = i;
                        }
                    }
                }
            }
            this.drawing = false;
            this.lineStartConnector = null;
            this.lineStartX = -1;
            this.lineStartY = -1;
            this.lineEndX = -1;
            this.lineEndY = -1;
            this.dragging = false;
            for (let i = 0; i < this.items.length; i++) {
                this.items[i].isDragging = false;
            }
        };
        this.mouseMove = (e) => {
            let BCR = this.canvas.getBoundingClientRect();
            let mx = e.clientX - BCR.left;
            let my = e.clientY - BCR.top;
            if (this.dragging === true) {
                e.preventDefault();
                e.stopPropagation();
                for (var i = 0; i < this.items.length; i++) {
                    let item = this.items[i];
                    if (item.isDragging) {
                        item.x = mx + item.dragOffsetX;
                        item.y = my + item.dragOffsetY;
                    }
                }
            }
            if (this.dragging === false && this.drawing == false) {
                for (var i = 0; i < this.items.length; i++) {
                    let item = this.items[i];
                    for (let i = 0; i < item.inConnectors.length; i++) {
                        let connector = item.inConnectors[i];
                        let hit = this.didHitConnector(mx, my, connector);
                        connector.hovering = hit;
                    }
                    for (let i = 0; i < item.outConnectors.length; i++) {
                        let connector = item.outConnectors[i];
                        let hit = this.didHitConnector(mx, my, connector);
                        connector.hovering = hit;
                    }
                }
            }
            if (this.dragging === false && this.drawing == true) {
                this.lineEndX = mx;
                this.lineEndY = my;
                for (var i = 0; i < this.items.length; i++) {
                    let item = this.items[i];
                    for (let i = 0; i < item.inConnectors.length; i++) {
                        let connector = item.inConnectors[i];
                        let hit = this.didHitConnector(mx, my, connector);
                        connector.hovering = hit;
                    }
                }
            }
            this.draw();
        };
        this.roundRect = (x, y, w, h, radius) => {
            let r = x + w;
            let b = y + h;
            this.ctx.beginPath();
            this.ctx.moveTo(x + radius, y);
            this.ctx.lineTo(r - radius, y);
            this.ctx.quadraticCurveTo(r, y, r, y + radius);
            this.ctx.lineTo(r, y + h - radius);
            this.ctx.quadraticCurveTo(r, b, r - radius, b);
            this.ctx.lineTo(x + radius, b);
            this.ctx.quadraticCurveTo(x, b, x, b - radius);
            this.ctx.lineTo(x, y + radius);
            this.ctx.quadraticCurveTo(x, y, x + radius, y);
            this.ctx.stroke();
        };
        this.dotNetReference = dotNetReference;
    }
    dispose() { }
    Start(container_id, canvas_id) {
        this.container = document.getElementById(container_id);
        this.canvas = document.getElementById(canvas_id);
        this.itemWIDTH = 60;
        this.itemHEIGHT = 100;
        this.resizeObserver = new ResizeObserver(entries => {
            this.ResizeCanvas();
        });
        this.resizeObserver.observe(this.container);
        this.ctx = this.canvas.getContext("2d");
        this.dragging = false;
        this.draw();
        document.addEventListener('mousemove', this.mouseMove, false);
        this.canvas.addEventListener('mousedown', this.mouseDown, false);
        document.addEventListener('mouseup', this.mouseUp, false);
    }
    Stop() {
        document.removeEventListener('mouseup', this.mouseUp, false);
        this.canvas.removeEventListener('mousedown', this.mouseDown, false);
        document.removeEventListener('mousemove', this.mouseMove, false);
        this.resizeObserver.unobserve(this.container);
    }
    didHitConnector(mx, my, connector) {
        let dx = mx - connector.x;
        let dy = my - connector.y;
        let isInside = (dx * dx + dy * dy) < (this.connectorRadius * this.connectorRadius);
        return isInside;
    }
}
