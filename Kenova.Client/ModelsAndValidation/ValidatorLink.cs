using System;
using System.Linq.Expressions;

namespace Kenova.Client.Components
{
    /// <summary>
    /// The ValidatorLink is stored in a Component.
    /// </summary>
    public class ValidatorLink : IDisposable
    {
        private ModelBase _model;

        private string _propertyName;

        private ModelFieldInfo _fieldInfo;

        private IRerender _component;


        public ValidatorLink(IRerender component, ModelBase model, string propertyName)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            if (propertyName == null)
                throw new ArgumentNullException("propertyName");

            _component = component;
            _model = model;
            _propertyName = propertyName;
            _fieldInfo = model.GetModelFieldInfo(propertyName);

            if (_fieldInfo == null)
                throw new ArgumentException($"Property '{_propertyName}' is not registered for Validation in the Model.");

            _model.ValidationChanged += Model_ValidationChanged;
        }


        public ValidatorLink(IRerender component, Expression<Func<object>> fieldExpression)
        {
            if (fieldExpression == null)
                throw new ArgumentNullException("fieldExpression");

            BindingHelper.ParseAccessor(fieldExpression, out object model, out string propertyName);

            if (model is not ModelBase)
                throw new ArgumentException("Parameter 'fieldExpression', the Expression must contain a ModelBase reference.");

            _component = component;
            _model = (ModelBase)model;
            _propertyName = propertyName;
            _fieldInfo = _model.GetModelFieldInfo(propertyName);

            if (_fieldInfo == null)
                throw new ArgumentException($"Property '{_propertyName}' is not registered for Validation in the Model.");

            _model.ValidationChanged += Model_ValidationChanged;
        }

        public void Dispose()
        {
            if (_model != null)
                _model.ValidationChanged -= Model_ValidationChanged;

        }

        private void Model_ValidationChanged(string fieldName)
        {
            if (fieldName != _propertyName)
                return;

            _component.Rerender();
        }

        public bool IsValidating
        {
            get { return _fieldInfo.IsValidating; }
        }

        public bool IsValid
        {
            get { return _fieldInfo.IsValid; }
        }

        public string RemarkText
        {
            get { return _fieldInfo.RemarkText; }
        }

    }
}
