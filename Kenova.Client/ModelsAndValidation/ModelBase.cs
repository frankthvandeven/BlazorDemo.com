using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Kenova.Client.Components
{
    public delegate void ValidationChangedEventHandler(string propertyName);

    public abstract class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event ValidationChangedEventHandler ValidationChanged;

        internal ModelBase()
        {
        }

        public abstract List<ModelFieldInfo> GetFieldList();

        /// <summary>
        /// Returns null if the field or property was not registered for validation.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public abstract ModelFieldInfo GetModelFieldInfo(string fieldName);

        public abstract bool IsRegistered(string fieldName);

        public abstract bool IsModelModified { get; internal set; }

        public abstract void SetModelModified();

        public abstract void ResetModelModified();

        public abstract Task<bool> ValidateAllAsync();

        public abstract Task<bool> ValidateOneAsync(string propertyName);


        /// <summary>
        /// Fire the PropertyChanged event.
        /// You can indicate that all fields on the object have changed by
        /// using null or String.Empty for the fieldName argument.
        /// </summary>
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (IsModelModified == false)
            {
                IsModelModified = true;

                if (propertyName.Length > 0)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsModelModified)));
            }

            _ = ValidateOneAsync(propertyName);

            //if (KenovaClientConfig.Diagnostics) Console.WriteLine($"ModelBase.cs is sending a propertychanged event for {propertyName}");

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// This will cause components listening to the ForField to Rerender.
        /// </summary>
        protected void NotifyValidationChanged(string propertyName)
        {
            ValidationChanged?.Invoke(propertyName);
        }


    }
}
