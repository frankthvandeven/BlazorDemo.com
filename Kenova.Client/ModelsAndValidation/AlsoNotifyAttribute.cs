using System;

namespace Kenova.Client.Components
{
    /// <summary>
    /// Makes the generated property raise an extra PropertyChanged event with 
    /// the specified property name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class AlsoNotifyAttribute : Attribute
    {
        private string _propertyName;

        public AlsoNotifyAttribute(string propertyName)
        {
            _propertyName = propertyName;
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }

    }

}
