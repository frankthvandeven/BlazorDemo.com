using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Kenova.Client.Util;

public class ComponentWingman<TComponent> : IAsyncDisposable where TComponent : class, IComponent, IHandleAfterRender
{
    // Async: IJSObjectReference, Sync:  IJSInProcessObjectReference

    private const string FAILED_TO_GET_INSTANCE = "Failed get JavaScript class instance";
    private const string INSTANTIATE_ONCE = "InstantiateAsync can only be called once";

    private IJSObjectReference _loaded_module;
    private DotNetObjectReference<TComponent> _dotNetReference;
    private IJSObjectReference _classinstance;

    private bool _is_instantiated = false;

    /// <summary>
    /// Instantiate by JavaScript classname.
    /// </summary>
    public async Task InstantiateAsync(TComponent component, string classname)
    {

        if (_is_instantiated)
            throw new Exception(INSTANTIATE_ONCE);

        if (component == null)
            throw new ArgumentNullException("component");

        if (classname is null)
            throw new ArgumentNullException("classname");

        _dotNetReference = DotNetObjectReference.Create<TComponent>(component);

        _classinstance = await KenovaClientConfig.JSRuntime.InvokeAsync<IJSObjectReference>("KNCreateClassByName", KenovaClientConfig.Diagnostics, classname, _dotNetReference);

        if (_classinstance == null)
            throw new Exception();

        _is_instantiated = true;
    }

    /// <summary>
    /// Instantiate by loaded JavaScript module.
    /// How to load a JavaScript module:
    /// public IJSObjectReference Root = await JSRuntime.InvokeAsync&lt;IJSObjectReference&gt;("import", "./_content/Client/root.js");
    /// Do not forget to DisposeAsync() the loaded module after disposing ComponentWingman.
    /// </summary>
    public async Task InstantiateByLoadedModuleAsync(TComponent component, IJSObjectReference loaded_module)
    {
        if (_is_instantiated)
            throw new Exception(INSTANTIATE_ONCE);

        if (component == null)
            throw new ArgumentNullException("component");

        if (loaded_module == null)
            throw new ArgumentNullException("module");

        _dotNetReference = DotNetObjectReference.Create<TComponent>(component);

        _classinstance = await loaded_module.InvokeAsync<IJSObjectReference>("Instantiate", _dotNetReference);

        if (_classinstance == null)
            throw new Exception(FAILED_TO_GET_INSTANCE);

        _is_instantiated = true;

    }

    /// <summary>
    /// Instantiate by module name.
    /// For example "./_content/Kenova.Client/portalroot.interop.js"
    /// </summary>
    public async Task InstantiateByModuleFilenameAsync(TComponent component, string module_filename)
    {
        if (_is_instantiated)
            throw new Exception(INSTANTIATE_ONCE);

        if (component == null)
            throw new ArgumentNullException("component");

        if (module_filename == null)
            throw new ArgumentNullException("module");

        _loaded_module = await KenovaClientConfig.JSRuntime.InvokeAsync<IJSObjectReference>("import", module_filename);

        _dotNetReference = DotNetObjectReference.Create<TComponent>(component);

        // original code
        _classinstance = await _loaded_module.InvokeAsync<IJSObjectReference>("Instantiate", _dotNetReference);

        if (_classinstance == null)
            throw new Exception(FAILED_TO_GET_INSTANCE);

        _is_instantiated = true;

    }

    public bool IsInstantiated
    {
        get { return _is_instantiated; }
    }

    public ValueTask InvokeVoidAsync(string identifier, params object[] args)
    {
        return _classinstance.InvokeVoidAsync(identifier, args);
    }

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, params object[] args)
    {
        return _classinstance.InvokeAsync<TValue>(identifier, args);
    }

    //public void InvokeVoid(string identifier, params object[] args)
    //{
    //    _classinstance.InvokeVoid(identifier, args);
    //}

    //public TValue Invoke<TValue>(string identifier, params object[] args)
    //{
    //    return _classinstance.Invoke<TValue>(identifier, args);
    //}

    public async ValueTask DisposeAsync()
    {
        _is_instantiated = false;

        if (_classinstance is not null)
        {
            await _classinstance.InvokeVoidAsync("dispose");
            await _classinstance.DisposeAsync();
        }

        if (_dotNetReference is not null)
        {
            _dotNetReference.Dispose();
        }

        if (_loaded_module != null)
        {
            await _loaded_module.DisposeAsync();
        }

    }

}
