using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

// https://docs.microsoft.com/en-us/dotnet/standard/attributes/retrieving-information-stored-in-attributes

namespace Kenova.Client.Components
{
    /// <summary>
    /// The base class for viewmodels
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class ModelTypedBase<TModel> : ModelBase where TModel : ModelBase
    {
        //protected ValidateEventArgs<TModel> e = new ValidateEventArgs<TModel>();

        private bool _ismodelmodified;

        private List<ModelFieldInfo> _mfi_list = new List<ModelFieldInfo>();


        private const string EMPTY_STRING = "";

        public ModelTypedBase()
        {
#if SKIP_THIS
            MemberInfo[] memberInfos = typeof(TModel).GetMembers();

            for (int i = 0; i < memberInfos.Length; i++)
            {
                var attr = (RegisterAttribute)Attribute.GetCustomAttribute(memberInfos[i], typeof(RegisterAttribute));

                if (attr != null)
                {
                    var name = memberInfos[i].Name;

                    var mm = new ModelFieldInfo();
                    mm.FieldName = name;
                    mm.IsValid = false;
                    mm.RemarkText = EMPTY_STRING;
                    mm.Callback = null;

                    this.FieldList.Add(mm);
                }
            }
#endif
        }

        public override List<ModelFieldInfo> GetFieldList()
        {
            return _mfi_list;
        }

        public override ModelFieldInfo GetModelFieldInfo(string fieldName)
        {
            if (fieldName == null)
                throw new ArgumentNullException("fieldName");

            var mfi = _mfi_list.Find(p => p.PropertyName == fieldName);

            return mfi;
        }

        public override bool IsRegistered(string fieldName)
        {
            if (fieldName == null)
                throw new ArgumentNullException("fieldName");

            return _mfi_list.Exists(p => p.PropertyName == fieldName);
        }

        public override bool IsModelModified
        {
            get { return _ismodelmodified; }
            internal set { _ismodelmodified = value; }
        }

        public override void SetModelModified()
        {
            if (!_ismodelmodified)
            {
                _ismodelmodified = true;
                NotifyPropertyChanged(nameof(IsModelModified));
            }
        }

        public override void ResetModelModified()
        {
            if (_ismodelmodified)
            {
                _ismodelmodified = false;
                NotifyPropertyChanged(nameof(IsModelModified));
            }
        }

        /// <summary>
        /// Register a member (property or field) in the validation system.
        /// It is recommended to register the members in the constructor of the viewmodel.
        /// </summary>
        /// <param name="memberLambda">m => m.Membername</param>
        protected void Register(Expression<Func<TModel, object>> memberLambda)
        {
            this.Register(memberLambda, null);
        }

        /// <summary>
        /// Register a member (property or field) in the validation system.
        /// It is recommended to register the members in the constructor of the viewmodel.
        /// </summary>
        /// <param name="memberLambda">m => m.Membername</param>
        /// <param name="callback"></param>
        protected void Register(Expression<Func<TModel, object>> memberLambda, Func<ValidateEventArgs, Task> callback)
        {
            var propertyName = BindingHelper.GetFieldName(memberLambda);

            var exists = this._mfi_list.Exists(p => p.PropertyName == propertyName);

            if (exists)
                throw new Exception($"Property {propertyName} already registered via the [Validate] atribute");

            var mfi = new ModelFieldInfo();
            mfi.PropertyName = propertyName;
            mfi.IsValid = false;
            mfi.RemarkText = EMPTY_STRING;
            mfi.Callback = callback;

            this._mfi_list.Add(mfi);
        }

        /// <summary>
        /// Force the validation event for each of the registered fields.
        /// The ModelFieldInfo.RemarkText and ModelFieldInfo.IsValid will be updated.
        /// </summary>
        /// <returns>Returns true if all validation items are valid.</returns>
        public override async Task<bool> ValidateAllAsync()
        {
            bool all_valid = true;
            var list = new List<Task<bool>>();

            foreach (var mfi in this._mfi_list)
            {
                Task<bool> task = validate_mfi(mfi);

                if (task != null && task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Canceled)
                    list.Add(task);
            }

            foreach (Task<bool> task in list)
            {
                bool single_valid = await task;

                if (single_valid == false)
                    all_valid = false;
            }

            return all_valid;
        }

        public async Task<bool> ValidateOneAsync(Expression<Func<TModel, object>> memberLambda)
        {
            var propertyName = BindingHelper.GetFieldName(memberLambda);

            bool valid = await ValidateOneAsync(propertyName);

            return valid;
        }

        public override async Task<bool> ValidateOneAsync(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("propertyName");

            var mfi = _mfi_list.Find(p => p.PropertyName == propertyName);

            if (mfi == null)
                throw new Exception($"Property {propertyName} not registered in the validation system");

            bool valid = await validate_mfi(mfi);

            return valid;
        }

        private async Task<bool> validate_mfi(ModelFieldInfo mfi)
        {
            var e = new ValidateEventArgs<TModel>
            {
                FieldName = mfi.PropertyName,
                IsValid = false,
                RemarkText = EMPTY_STRING
            };

            if (mfi.Callback == null)
            {
                Task task = ValidateEventAsync(e);

                if (task != null && task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Canceled)
                {
                    mfi.IsValidating = true; // the Remark component...
                    NotifyValidationChanged(mfi.PropertyName); // ... will display a spinner
                    await task;
                }
            }
            else
            {
                Task task = mfi.Callback(e);

                if (task != null && task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Canceled)
                {
                    mfi.IsValidating = true; // the Remark component...
                    NotifyValidationChanged(mfi.PropertyName); // ... will display a spinner
                    await task;
                }

            }

            bool mfiModified = false;

            if (mfi.IsValidating)
            {
                mfiModified = true;
                mfi.IsValidating = false;
            }

            if (mfi.IsValid != e.IsValid)
            {
                mfiModified = true;
                mfi.IsValid = e.IsValid;
            }

            if (mfi.RemarkText != e.RemarkText)
            {
                mfiModified = true;
                mfi.RemarkText = e.RemarkText;
            }

            e.RemarkText = EMPTY_STRING;

            if (mfiModified)
                NotifyValidationChanged(mfi.PropertyName);

            return e.IsValid;
        }

        // This one will be overriden in the viewmodel.
        protected virtual Task ValidateEventAsync(ValidateEventArgs<TModel> e)
        {
            return null; // Task.CompletedTask;
        }

    }
}
