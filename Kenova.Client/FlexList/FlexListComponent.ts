
class FlexListComponent {
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
        this.container.addEventListener('mousedown', this.handleMouseDown, false);
    }

    Stop() {
        this.container.removeEventListener('mousedown', this.handleMouseDown, false);
        this.container.removeEventListener('keydown', this.handleKeydown, false);
    }

    OnAfterRender = () => {
        this.CreateTabZeroElement();
    }

    SetFocus = () => {

        this.CreateTabZeroElement(); // make sure there is a focus index

        let focus_index = this.findFocusIndex();
        let rows = this.container.children as HTMLCollectionOf<HTMLElement>;

        if (focus_index != -1) {
            rows[focus_index].focus();
        }

    }

    handleMouseDown = (e) => {
        let rows = this.container.children as HTMLCollectionOf<HTMLElement>;

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
    }

    didClickElement = (e, element) => {
        return element == e.target || element.contains(e.target);
    }

    findFocusIndex = () => {
        let rows = this.container.children as HTMLCollectionOf<HTMLElement>;

        for (let i = 0; i < rows.length; i++) {
            if (rows[i].tabIndex == 0) {
                return i;
            }
        }

        return -1;
    }

    ChangeTabZeroElement = (element_to_focus) => {

        let rows = this.container.children as HTMLCollectionOf<HTMLElement>;

        for (let i = 0; i < rows.length; i++) {
            let row_element = rows[i];

            if (row_element === element_to_focus) {
                row_element.tabIndex = 0;
                //console.log('roving tabindex 0 goes to element ' + i);
            }
            else {
                row_element.removeAttribute('tabindex');
            }
        }

    }

    CreateTabZeroElement = () => {

        let rows = this.container.children as HTMLCollectionOf<HTMLElement>;
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

    }

    handleKeydown = (e) => {

        // Make sure no modifier key is pressed
        if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
            return;

        if (e.keyCode === 38) { // up
            this.KeyUpArrowPressed();
            //e.stopPropagation();
            e.preventDefault();
        }
        else if (e.keyCode === 40) { // down
            this.KeyDownArrowPressed();
            //e.stopPropagation();
            e.preventDefault();
        }
        else if (e.keyCode === 36) { // home
            this.KeyHomePressed();
            e.preventDefault();
        }
        else if (e.keyCode === 35) { // end
            this.KeyEndPressed();
            e.preventDefault();
            //e.stopPropagation();
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

    }

    KeyEnterPressed = () => {
        let focus_index = this.findFocusIndex();

        if (focus_index != -1) {
            this.dotNetReference.invokeMethodAsync('OnEnterPressed', focus_index);
        }
    }

    KeySpacePressed = () => {
        let focus_index = this.findFocusIndex();

        if (focus_index != -1) {
            this.dotNetReference.invokeMethodAsync('OnSpacePressed', focus_index);
        }
    }

    KeyDownArrowPressed = () => {
        let focus_index = this.findFocusIndex();

        if (focus_index < this.container.children.length - 1) {
            focus_index++;
            let element = this.container.children[focus_index] as HTMLElement;
            this.ChangeTabZeroElement(element);
            //element.scrollIntoView();
            element.focus();
        }

    }

    KeyUpArrowPressed = () => {
        let focus_index = this.findFocusIndex();

        if (focus_index > 0) {
            focus_index--;
            let element = this.container.children[focus_index] as HTMLElement;
            this.ChangeTabZeroElement(element);
            //element.scrollIntoView();
            element.focus();
        }
    }

    KeyHomePressed = () => {
        let focus_index = 0;
        let element = this.container.children[focus_index] as HTMLElement;

        this.ChangeTabZeroElement(element);
        //element.scrollIntoView();
        element.focus();
    }

    KeyEndPressed = () => {
        let focus_index = this.container.children.length - 1;
        let element = this.container.children[focus_index] as HTMLElement;

        this.ChangeTabZeroElement(element);
        //element.scrollIntoView();
        element.focus();
    }

}


