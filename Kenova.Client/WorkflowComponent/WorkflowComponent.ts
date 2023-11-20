

//export function InstantiateWorkflow(dotNetReference) {
//    return new Component(dotNetReference);
//}

// Good snap to grid
// http://jsfiddle.net/fabricjs/S9sLu/
// https://codepen.io/atindo23/pen/OJLbdrJ

// https://stackoverflow.com/questions/64908748/call-js-generated-from-typescript-in-blazor-using-ijsobjectreference

// https://code.tutsplus.com/articles/best-free-canvas-libraries-in-javascript--cms-37317

// Coordinates of a mouse event:
// MouseEvent.offsetX and MouseEvent.offsetY
// MouseEvent.clientX and MouseEvent.clientY
// MouseEvent.pageX and MouseEvent.pageY
// MouseEvent.screenX and MouseEvent.screenY

interface IWorkflowConnector {
    x: number;
    y: number;
    connected: boolean;
    connectedTo: string;
    connectedIndex: number;
    hovering: boolean;
}

interface IWorkflowItem {
    id: string;
    x: number;
    y: number;
    inConnectors: Array<IWorkflowConnector>;
    outConnectors: Array<IWorkflowConnector>;

    isDragging: boolean;
    dragOffsetX: number;
    dragOffsetY: number;
}

interface IWorkflowData {
    id: string;
    items: Array<IWorkflowItem>;
}

/*interface IWorkflowData {
    Id: string;
    Items: Array<{
        nomColonne: {
            type: string,
            typeWithSpecificValues: "value" | "key" // Alternative for type
            filter: string,
        }
    }>,
}*/

class WorkflowComponent {
    dotNetReference: dotNetHandler;
    container: HTMLElement;
    canvas: HTMLCanvasElement;
    itemWIDTH: number;
    itemHEIGHT: number;
    resizeObserver: ResizeObserver;
    ctx: CanvasRenderingContext2D;
    items: Array<IWorkflowItem> = [];
    colorITEMBORDER: string = "#BBBBBB";
    grid: number = 5;
    connectorRadius: number = 8; // was 4

    dragging: boolean = false;
    drawing: boolean = false;
    lineStartConnector: IWorkflowConnector = null;
    lineStartX: number = -1;
    lineStartY: number = -1;
    lineEndX: number = -1;
    lineEndY: number = -1;

    //dotNetReference = null;
    //source_element = null;
    //target_element = null;

    constructor(dotNetReference: dotNetHandler) {
        this.dotNetReference = dotNetReference;
    }

    dispose() { }

    // Call a C# method as follows. The C# method must have the [JSInvokable] attribute.
    //this.dotNetReference.invokeMethodAsync('CallOnFirstUpdate', param1);

    Start(container_id, canvas_id) {
        this.container = document.getElementById(container_id);
        this.canvas = document.getElementById(canvas_id) as HTMLCanvasElement;

        this.itemWIDTH = 60;
        this.itemHEIGHT = 100;

        this.resizeObserver = new ResizeObserver(entries => {
            //entries.forEach(entry => {
            //    console.log('observed width', entry.contentRect.width);
            //    console.log('observed height', entry.contentRect.height);
            //});
            this.ResizeCanvas();
        });

        // observe() will call the callback right away, without any resize
        this.resizeObserver.observe(this.container);

        this.ctx = this.canvas.getContext("2d");

        // drag related variables
        this.dragging = false;

        // an array of objects that define different rectangles

        //this.rects = [];
        //this.rects.push({
        //    x: 0,
        //    y: 10,
        //    fill: "#444444",
        //    isDragging: false
        //});

        // listen for mouse events
        //this.canvas.onmousedown = this.myDown;
        //this.canvas.onmouseup = this.myUp;
        //this.canvas.onmousemove = this.myMove;

        // call to draw the scene
        this.draw();

        //this.container.addEventListener('keydown', this.handleKeydown, false);
        //this.container.addEventListener('mousedown', this.handleMouseDown, false);

        document.addEventListener('mousemove', this.mouseMove, false);

        this.canvas.addEventListener('mousedown', this.mouseDown, false);
        document.addEventListener('mouseup', this.mouseUp, false);
        //this.canvas.addEventListener('mousemove', this.mouseMove, false);

    }

    Stop() {
        //this.canvas.removeEventListener('mousemove', this.mouseMove, false);
        document.removeEventListener('mouseup', this.mouseUp, false);
        this.canvas.removeEventListener('mousedown', this.mouseDown, false);

        document.removeEventListener('mousemove', this.mouseMove, false);

        this.resizeObserver.unobserve(this.container);

        //this.container.removeEventListener('mousedown', this.handleMouseDown, false);
        //this.container.removeEventListener('keydown', this.handleKeydown, false);
    }

    SetWorkflowData = (data: IWorkflowData) => {

        //let bdata = JSON.stringify(data);
        this.items = data.items;
        //debugger;
        this.draw();

    }

    ResizeCanvas = () => {

        // make the canvas the same size as the div that contains it
        this.canvas.width = this.container.clientWidth;
        this.canvas.height = this.container.clientHeight;

        this.draw(); // needed?
    }

    OnAfterRender = () => {
        //this.CreateTabZeroElement();
    }

    SetFocus = () => {

        //this.CreateTabZeroElement(); // make sure there is a focus index

        //let focus_index = this.findFocusIndex();
        //let rows = this.container.children;

        //if (focus_index != -1) {
        //    rows[focus_index].focus();
        //}

    }

    handleMouseDown = (e) => {
        //    let rows = this.container.children;

        //    for (let i = 0; i < rows.length; i++) {
        //        let row_element = rows[i];

        //        if (this.didClickElement(e, row_element)) {
        //            row_element.tabIndex = "0";
        //            row_element.focus();
        //        }
        //        else {
        //            row_element.removeAttribute('tabindex');
        //        }
        //    }
    }

    didClickElement = (e, element) => {
        //return element == e.target || element.contains(e.target);
    }

    findFocusIndex = () => {
        //    let rows = this.container.children;

        //    for (let i = 0; i < rows.length; i++) {
        //        if (rows[i].tabIndex == "0") {
        //            return i;
        //        }
        //    }

        //    return -1;
    }

    //handleKeydown = (e) => {

    //    // Make sure no modifier key is pressed
    //    if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey)
    //        return;

    //    if (e.keyCode === 38) { // up
    //        this.KeyUpArrowPressed();
    //        //e.stopPropagation();
    //        e.preventDefault();
    //    }
    //    else if (e.keyCode === 40) { // down
    //        this.KeyDownArrowPressed();
    //        //e.stopPropagation();
    //        e.preventDefault();
    //    }
    //    else if (e.keyCode === 36) { // home
    //        this.KeyHomePressed();
    //        e.preventDefault();
    //    }
    //    else if (e.keyCode === 35) { // end
    //        this.KeyEndPressed();
    //        e.preventDefault();
    //        //e.stopPropagation();
    //    }
    //    else if (e.keyCode === 13) { // enter
    //        this.KeyEnterPressed();
    //        e.preventDefault();
    //        e.stopPropagation();
    //    }
    //    else if (e.keyCode === 32) { // space
    //        this.KeySpacePressed();
    //        e.preventDefault();
    //        e.stopPropagation();
    //    }

    //}

    //KeyEnterPressed = () => {
    //    let focus_index = this.findFocusIndex();

    //    if (focus_index != -1) {
    //        this.dotNetReference.invokeMethodAsync('OnEnterPressed', focus_index);
    //    }
    //}

    //KeySpacePressed = () => {
    //    let focus_index = this.findFocusIndex();

    //    if (focus_index != -1) {
    //        this.dotNetReference.invokeMethodAsync('OnSpacePressed', focus_index);
    //    }
    //}

    //KeyDownArrowPressed = () => {
    //    let focus_index = this.findFocusIndex();

    //    if (focus_index < this.container.children.length - 1) {
    //        focus_index++;
    //        let element = this.container.children[focus_index];
    //        this.ChangeTabZeroElement(element);
    //        //element.scrollIntoView();
    //        element.focus();
    //    }

    //}

    //KeyUpArrowPressed = () => {
    //    let focus_index = this.findFocusIndex();

    //    if (focus_index > 0) {
    //        focus_index--;
    //        let element = this.container.children[focus_index];
    //        this.ChangeTabZeroElement(element);
    //        //element.scrollIntoView();
    //        element.focus();
    //    }
    //}

    //KeyHomePressed = () => {
    //    let focus_index = 0;
    //    let element = this.container.children[focus_index];

    //    this.ChangeTabZeroElement(element);
    //    //element.scrollIntoView();
    //    element.focus();
    //}

    //KeyEndPressed = () => {
    //    let focus_index = this.container.children.length - 1;
    //    let element = this.container.children[focus_index];

    //    this.ChangeTabZeroElement(element);
    //    //element.scrollIntoView();
    //    element.focus();
    // }

    // canvas example


    // redraw the scene
    draw = () => {

        // clear the whole canvas
        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);

        // draw the background
        //this.ctx.fillStyle = "#FAF7F8";
        //this.rect(0, 0, this.WIDTH, this.HEIGHT);

        // redraw each rect in the rects[] array
        for (var i = 0; i < this.items.length; i++) {
            this.drawWorkflowItem(this.items[i]);
        }

        // Line drawing

        for (var i = 0; i < this.items.length; i++) {
            let item = this.items[i];
            for (let i = 0; i < item.outConnectors.length; i++) {
                let connector = item.outConnectors[i];

                if (connector.connected === true) {
                    let target: IWorkflowConnector = this.findTargetConnector(connector.connectedTo, connector.connectedIndex);

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

        let start_pos_x = this.items[0].x + this.itemWIDTH;;
        let start_pos_y = this.items[0].y + 12;
        let end_pos_x = this.items[1].x + this.itemWIDTH;
        let end_pos_y = this.items[1].y + 12;

        //let half_x = (start_pos_x - end_pos_x) / 2;
        //let half_y = (start_pos_y - end_pos_y) / 2;

        let distance_x = end_pos_x - start_pos_x;

        let half_x = start_pos_x + (distance_x / 2);  // (end_pos_x - start_pos_x) / 2;
        //let half_y = (end_pos_y - start_pos_y) / 2;

        // start by drawing a simple leader line
        this.ctx.beginPath();
        //this.ctx.moveTo(start_rect.x + this.itemWIDTH, start_rect.y + 12);
        //this.ctx.lineTo(end_rect.x + this.itemWIDTH, end_rect.y + 12);

        this.ctx.moveTo(start_pos_x, start_pos_y);
        this.ctx.lineTo(half_x, start_pos_y);
        this.ctx.lineTo(half_x, end_pos_y);
        this.ctx.lineTo(end_pos_x, end_pos_y);

        //this.ctx.lineTo(start_pos_y.x + this.itemWIDTH, start_pos_y.y + 12);
        //this.ctx.moveTo(start_pos_x.x + this.itemWIDTH, start_pos_x.y + 12);
        //this.ctx.lineTo(start_pos_y.x + this.itemWIDTH, start_pos_y.y + 12);

        this.ctx.lineWidth = 1.5;
        this.ctx.strokeStyle = "red";
        this.ctx.stroke();

        // detect mouse click on line
        //if (this.mouseX > start_pos_x && this.mouseX < end_pos_x && this.mouseY > start_pos_y && this.mouseY < end_pos_y) {
        //
        // draw a blue box
        //this.ctx.beginPath();
        //this.ctx.rect(start_pos_x, start_pos_y, distance_x, this.itemHEIGHT);
        //this.ctx.fillStyle = "blue";
        //this.ctx.fill();
        //}

        //    //this.ctx.beginPath();
    }

    findTargetConnector = (id: string, index: number): IWorkflowConnector => {

        let targetItem: IWorkflowItem = this.items.find(e => e.id === id);

        if (targetItem === undefined)
            return null;

        return targetItem.inConnectors[index];
    }

    // https://stackoverflow.com/questions/10957689/collision-detection-between-a-line-and-a-circle-in-javascript

    /*
    aaa() {

    


    }

    lineCollision = (point, line, minDist = 0) => {
        let lineSize = {};

        if (line.x1 > line.x2) { lineSize.xs = line.x2; lineSize.xb = line.x1; } else { lineSize.xs = line.x1; lineSize.xb = line.x2; }
        if (line.y1 > line.y2) { lineSize.ys = line.y2; lineSize.yb = line.y1; } else { lineSize.ys = line.y1; lineSize.yb = line.y2; }
        if (point.x < lineSize.xs - minDist || point.x > lineSize.xb + minDist || point.y < lineSize.ys - minDist || point.y > lineSize.yb + minDist) {
            //if not within collision box
            return false;
        }
        //slope = y2-y1/x2-x1

        let y2My1 = (line.y2 - line.y1); let x2Mx1 = (line.x2 - line.x1);

        let slope = (line.y2 - line.y1) / (line.x2 - line.x1);

        //reduce slope precision
        slope = Math.floor(Math.abs(slope) * 1000) / 1000;

        if (slope === 0) {
            //if horizontal line
            if (Math.abs(point.y - line.y1) <= minDist) {
                return true;
            }
        } else if (slope === Infinity) {
            //if vertical line
            if (Math.abs(point.x - line.x1) <= minDist) {
                return true;
            }
        } else {
            //y=mx+b b=y+mx
            let b = (line.y1 + (slope * (point.x)));
            let yDif = point.y + line.x1;
            if (Math.abs(b - yDif) <= minDist) {
                return true;
            }
        }
        return false;
    }
*/

    // Draw a signle workflow item
    drawWorkflowItem = (item: IWorkflowItem) => {

        this.ctx.lineWidth = 1.5;
        this.ctx.strokeStyle = this.colorITEMBORDER;

        // stroke the rounded rectangle
        this.roundRect(item.x, item.y, this.itemWIDTH, this.itemHEIGHT, 10);

        // fill the rounded rectangle
        this.ctx.fillStyle = "white";
        this.ctx.fill();

        // Stroke the item
        //this.ctx.strokeRect(r.x, r.y, this.itemWIDTH, this.itemHEIGHT);

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

    }

    drawConnector = (connector: IWorkflowConnector) => {
        // Stroke
        this.ctx.beginPath();
        this.ctx.arc(connector.x, connector.y, this.connectorRadius, 0, 2 * Math.PI); // arc() is a path method.
        //this.ctx.lineWidth = 1;
        this.ctx.stroke();

        // Fill the 'stroked' shape
        if (connector.hovering === true) {
            this.ctx.fillStyle = "red";
        }
        else {
            this.ctx.fillStyle = "white";
        }


        this.ctx.fill(); // .fill() and .stroke() automatically do a ctx.closePath()
    }


    // handle mousedown events
    mouseDown = (e: MouseEvent) => {

        // tell the browser we're handling this mouse event
        e.preventDefault();
        e.stopPropagation();

        // getBoundingClientRect() returns the size of an element and its position relative to the viewport
        let BCR = this.canvas.getBoundingClientRect();

        // get the current mouse position relative to the canvas
        let mx = e.clientX - BCR.left;
        let my = e.clientY - BCR.top;

        let hitfound: boolean = false;

        // Find out if we clicked on an out-connector
        for (var i = 0; i < this.items.length; i++) {
            let item = this.items[i];

            for (let i = 0; i < item.outConnectors.length; i++) {
                let connector = item.outConnectors[i];
                let hit = this.didHitConnector(mx, my, connector)

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

        // test each rect to see if mouse is inside
        this.dragging = false;

        //if (hitfound ==)


        for (var i = 0; i < this.items.length; i++) {
            let item = this.items[i];
            if (mx >= item.x &&
                mx < item.x + this.itemWIDTH &&
                my >= item.y &&
                my < item.y + this.itemHEIGHT) {

                // if yes, set that rects isDragging=true
                //debugger;
                this.dragging = true;
                item.isDragging = true;

                item.dragOffsetX = item.x - mx;
                item.dragOffsetY = item.y - my;

            }
        }

    }

    // handle mouseup events
    mouseUp = (e: MouseEvent) => {

        // getBoundingClientRect() returns the size of an element and its position relative to the viewport
        let BCR = this.canvas.getBoundingClientRect();

        // get the current mouse position relative to the canvas
        let mx = e.clientX - BCR.left;
        let my = e.clientY - BCR.top;

        // tell the browser we're handling this mouse event
        e.preventDefault();
        e.stopPropagation();

        if (this.drawing === true) {

            // Find out if we released the button on an in-connector
            for (var i = 0; i < this.items.length; i++) {
                let item = this.items[i];

                for (let i = 0; i < item.inConnectors.length; i++) {
                    let connector = item.inConnectors[i];
                    let hit = this.didHitConnector(mx, my, connector)

                    if (hit === true) {
                        this.lineStartConnector.connected = true;
                        this.lineStartConnector.connectedTo = item.id;
                        this.lineStartConnector.connectedIndex = i;
                    }
                }

            }
        }

        // clear all the drawing flags
        this.drawing = false;
        this.lineStartConnector = null;
        this.lineStartX = -1;
        this.lineStartY = -1;
        this.lineEndX = -1;
        this.lineEndY = -1;

        // clear all the dragging flags
        this.dragging = false;

        for (let i = 0; i < this.items.length; i++) {
            this.items[i].isDragging = false;
        }
    }

    mouseMove = (e: MouseEvent) => {

        //console.log('mouse x (document) ', e.clientX);

        // getBoundingClientRect() returns the size of an element and its position relative to the viewport
        let BCR = this.canvas.getBoundingClientRect();

        // get the current mouse position relative to the canvas
        let mx = e.clientX - BCR.left;
        let my = e.clientY - BCR.top;

        if (this.dragging === true) {

            // tell the browser we're handling this mouse event
            e.preventDefault();
            e.stopPropagation();


            //console.log('mouse x relative to canvas: x ' + mx + ' y ' + my);

            // move each rect that isDragging
            // by the distance the mouse has moved
            // since the last mousemove
            for (var i = 0; i < this.items.length; i++) {
                let item = this.items[i];
                if (item.isDragging) {
                    item.x = mx + item.dragOffsetX;
                    item.y = my + item.dragOffsetY;

                    //r.x = Math.round(r.x / this.grid) * this.grid;
                    //r.y = Math.round(r.y / this.grid) * this.grid;

                }
            }
        }

        if (this.dragging === false && this.drawing == false) {

            for (var i = 0; i < this.items.length; i++) {
                let item = this.items[i];

                for (let i = 0; i < item.inConnectors.length; i++) {
                    let connector = item.inConnectors[i];
                    let hit = this.didHitConnector(mx, my, connector)
                    connector.hovering = hit;

                }

                for (let i = 0; i < item.outConnectors.length; i++) {
                    let connector = item.outConnectors[i];
                    let hit = this.didHitConnector(mx, my, connector)
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
                    let hit = this.didHitConnector(mx, my, connector)
                    connector.hovering = hit;

                }
            }

        }

        // redraw the scene with the new rect positions
        this.draw();
    }

    // Pythagoras hit testing for circles: http://jsfiddle.net/m1erickson/Bgh9d/
    didHitConnector(mx: number, my: number, connector: IWorkflowConnector): boolean {

        let dx = mx - connector.x;
        let dy = my - connector.y;
        let isInside = (dx * dx + dy * dy) < (this.connectorRadius * this.connectorRadius);

        return isInside;
    }

    // Draw a rectangle with rounded corners.
    // https://stackoverflow.com/questions/1255512/how-to-draw-a-rounded-rectangle-using-html-canvas
    roundRect = (x: number, y: number, w: number, h: number, radius: number) => {
        let r = x + w;
        let b = y + h;

        this.ctx.beginPath();
        //this.ctx.strokeStyle = "green";
        //this.ctx.lineWidth = "4";
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
    }

}

