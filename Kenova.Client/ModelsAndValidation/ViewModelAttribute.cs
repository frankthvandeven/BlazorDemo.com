using System;

namespace Kenova.Client.Components
{

    /// <summary>
    /// Instructs the Kenova.SourceGenerator (install NuGet package) to scan the class for fields.
    /// A property will be generated for each field name that starts with 2 underscore characters.<br/><br/>
    /// For example: private string __Name;  (double underscore!)<br/><br/>
    /// The generated property name is the field name without the 2 underscores.<br/><br/>
    /// Place the [AlsoNotify("propertyname")] attribute above the field declaration, 
    /// to trigger multiple PropertyChanged events with different property names.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ViewModelAttribute : Attribute
    {
    }
}
