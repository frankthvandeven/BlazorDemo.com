
// list al global vars: console.log(Object.keys(window));

declare var DotNet: dotNetHandler;

interface dotNetHandler {
    invokeMethodAsync<T>(methodName: string, ...args : any): Promise<T>;
}

interface HTMLElement {
    tabIndex: number;
}
