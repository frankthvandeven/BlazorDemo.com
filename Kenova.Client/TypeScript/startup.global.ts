// https://dmitripavlutin.com/6-ways-to-declare-javascript-functions/
//
// list al global vars: console.log(Object.keys(window));
//
// At the top level:
// let and const are block-scoped and do not create a property on the global object.
// var is hoisted and creates a property on the global object.

{ // start block scope to isolate the variables declared with 'let'

    const GetDeviceType = () => {
        const ua = navigator.userAgent;
        if (/(tablet|ipad|playbook|silk)|(android(?!.*mobi))/i.test(ua)) return "tablet";
        if (/Mobile|iP(hone|od|ad)|Android|BlackBerry|IEMobile|Kindle|Silk-Accelerated|(hpw|web)OS|Opera M(obi|ini)/.test(ua)) return "mobile";
        return "desktop";
    };

    // globalThis and window are the same in the browser.
    // Variables declared with let at top level are visible in classes.

    let ignore_sr_events: boolean = false;
    let device_type: string = GetDeviceType();
    let layerStack = new Array();
    let saved_focussed_element = null;
    let image_canvas = null;

    var KNCreateClassByName = (diagnostics: boolean, classname: string, dotNetReference: dotNetHandler): any => {
        if (diagnostics) console.log("✈️ JavaScript - Instantiating class with name " + classname + " for ComponentWingman");
        var c = eval(classname);
        return new c(dotNetReference);
    }

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
    }

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
                if (item.dropdown_element == null) { throw new Error("KNAddLayer dropdown_element '" + dropdown_id + "' not rendered yet. Timing problem."); }

                let viewportOffset = item.dropdown_element.getBoundingClientRect();
                item.dropdown_element_top = viewportOffset.top; // these are relative to the viewport, i.e. the window
                item.dropdown_element_left = viewportOffset.left;
            }

            if (is_dropdown && overlay_id != null) {
                item.overlay_element = document.getElementById(overlay_id);
                if (item.overlay_element == null) { throw new Error("KNAddLayer overlay_element '" + overlay_id + "' not rendered yet. Timing problem."); }
            }

            layerStack.push(item);
        };

        requestAnimationFrame(renderComplete);
    }

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
                        // Focus on the input element instead of on the 'button'
                        //inputElement.focus();
                        //inputElement.select();
                        setTimeout(function () { inputElement.focus(); inputElement.select(); }, 0);
                        //setTimeout(function () { inputElement.select(); }, 0);
                        return;
                    }
                }
            }

            item.focus_element.focus();
        };

        //setTimeout(function () { item.focus_element.focus(); }, 100);
        requestAnimationFrame(renderComplete);

    }

    const HandleMouseDown = (e: MouseEvent) => {
        let closecount = 0;

        for (let i = layerStack.length - 1; i >= 0; i--) {
            let item = layerStack[i];
            if (!item.is_dropdown) break;
            if (didClickInside(item)) break;
            closecount++;
        }

        if (closecount > 0) DotNet.invokeMethodAsync('Kenova.Client', 'OnCloseDropdownLayers', closecount);
    }

    const didClickInside = (item) => {
        if (item.dropdown_element != null && (item.dropdown_element === event.target || item.dropdown_element.contains(event.target))) { return true; }
        if (item.overlay_element != null && (item.overlay_element === event.target || item.overlay_element.contains(event.target))) { return true; }
        return false;
    }

    const HandleScroll = (e) => {
        if (ignore_sr_events) return;

        let closecount = 0;

        for (let i = layerStack.length - 1; i >= 0; i--) {
            let item = layerStack[i];
            if (!item.is_dropdown) break;
            if (!dropdownMoved(item)) break;
            closecount++;
        }

        if (closecount > 0) {
            DotNet.invokeMethodAsync('Kenova.Client', 'OnCloseDropdownLayers', closecount);
        }

    }

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
    }

    // IMPLEMENT CHECK IN LAYERMANAGER.OPEN. YOU CANNOT OPEN A NON-DROPDOWN LAYER FROM A DROPDOWN!!!
    // console.log()

    const HandleResize = (e) => {

        if (ignore_sr_events) return;

        let closecount = 0;

        for (let i = layerStack.length - 1; i >= 0; i--) {
            let item = layerStack[i];
            if (!item.is_dropdown) break;
            closecount++;
        }

        if (closecount > 0) {
            DotNet.invokeMethodAsync('Kenova.Client', 'OnCloseDropdownLayers', closecount);
        }

    };

    var KNTriggerIgnoreScrollResizeEvents = () => { // Ignore scroll and resize events
        if (device_type == "desktop")
            ignore_sr_events = false;
        else {
            ignore_sr_events = true;
            setTimeout(function () { ignore_sr_events = false; }, 800);
        }
    }

    var KNGetBoundingClientRectByPos = (x: number, y: number): DOMRect => {
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
        if (element == null || document.activeElement == null) return;
        if (!element.contains(document.activeElement)) return;

        if (document.activeElement instanceof HTMLElement) {
            document.activeElement.blur();
        }
    };

    var KNElementHidden = (element_id) => {
        let element = document.getElementById(element_id);
        return window.getComputedStyle(element).display === "none"
    }

    var KNIsChildOf = (parent_id, child_id) => {
        let parent_element = document.getElementById(parent_id);
        let child_element = document.getElementById(child_id);
        return parent_element.contains(child_element);
    }

    //function KNInputAfterBlur(element_id) {
    //    let element = document.getElementById(element_id);
    //    //element.scrollLeft = 0;
    //};

    var KNGetCssVariable = (var_name) => {
        let comp = window.getComputedStyle(document.documentElement);
        return comp.getPropertyValue(var_name);
    }

    var KNSetCssVariable = (var_name, value) => {
        document.documentElement.style.setProperty(var_name, value);
    }

    var KNShowTaskrunner = (caption) => {
        let element = document.getElementById("kn-id-taskrunner-layer");
        let captionElement = document.getElementById("kn-id-taskrunner-caption");
        captionElement.textContent = caption;
        element.style.display = "flex";
    }

    var KNTaskrunnerDisplayFailure = (caption, message) => {
        // hide caption, hide animation
        // show "Operation failed" caption plus the message plus the close button
    }

    var KNHideTaskrunner = () => {
        let element = document.getElementById("kn-id-taskrunner-layer");
        element.style.display = "none";
    }

    /*
    var HookupImageCanvas = (canvas_id) => {
        let container = document.getElementById(canvas_id);
        image_canvas = container;
        window.addEventListener("paste", PasteImage, false);
    }
    var UnhookImageCanvas = (canvas_id) => {
        image_canvas = null;
        let container = document.getElementById(canvas_id);
        window.removeEventListener("paste", PasteImage, false);
    }
    var PasteImage = (pasteEvent) => {
        console.log("pasting");
        if (pasteEvent.clipboardData == false) {
            //if (typeof (callback) == "function") {
            //    callback(undefined);
            //}
        };
        let items = pasteEvent.clipboardData.items;
        if (items == undefined) {
            //if (typeof (callback) == "function") {
            //    callback(undefined);
            //}
        };
        for (let i = 0; i < items.length; i++) {
            // Skip content if not image
            if (items[i].type.indexOf("image") == -1) continue;
            // Retrieve image on clipboard as blob
            let blob = items[i].getAsFile();
            xcallback(blob);
        }
    }
    const xcallback = (imageBlob) => {
        // If there's an image, display it in the canvas
        if (imageBlob) {
            let canvas = image_canvas;
            //let canvas = document.getElementById("mycanvas");
            let ctx = canvas.getContext('2d');
     
            // Create an image to render the blob on the canvas
            let img = new Image();
     
            // Once the image loads, render the img on the canvas
            img.onload = function () {
                // Update dimensions of the canvas with the dimensions of the image
                canvas.width = width;
                canvas.height = height;
     
                // Draw the image
                ctx.drawImage(img, 0, 0);
            };
     
            // Crossbrowser support for URL
            let URLObj = window.URL || window.webkitURL;
     
            // Creates a DOMString containing a URL representing the object given in the parameter
            // namely the original Blob
            img.src = URLObj.createObjectURL(imageBlob);
        }
    }
    */


    // https://www.aspsnippets.com/Articles/JavaScript-Save-BLOB-as-PDF-File.aspx

    /*
    var KNSaveAsFile = (filename, type, bytesBase64) => {
        let link = document.createElement('a');
        link.download = filename;
        link.href = "data:" + type + ";base64," + bytesBase64;
        document.body.appendChild(link); // Needed for Firefox
        link.click();
        document.body.removeChild(link);
    }
    */

    var KNSaveAsFile2 = (filename: string, data: Uint8Array): void => {
        // Blazor converts a byte[] array to Uint8Array (no longer requires processing the Base64 encoding).

        //Convert the Byte Data to BLOB object.
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
    }

    var KNCopyTextToClipboard = (text) => {

        navigator.clipboard.writeText(text).then(function () {
            /*alert("Copied to clipboard!");*/
        })
            .catch(function (error) {
                alert(error);
            });

    }

    var KNCopyElementTextToClipboard = (codeElement) => {

        navigator.clipboard.writeText(codeElement.textContent).then(function () {
            /*alert("Copied to clipboard!");*/
        })
            .catch(function (error) {
                alert(error);
            });
    }

    document.addEventListener('mousedown', HandleMouseDown, true);
    document.addEventListener('scroll', HandleScroll, true);
    window.addEventListener('resize', HandleResize, true);
}