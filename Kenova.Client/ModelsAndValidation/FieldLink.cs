using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kenova.Client.Components
{

    /// <summary>
    /// The FieldLink holds value of TValue type.
    /// 
    /// The FieldLink is for use inside a Component.
    /// </summary>
    public class FieldLink<TValue> : IDisposable
    {
        private Expression<Func<TValue>> _fieldExpression;
        private Func<TValue> _fieldGetter;
        private Action<TValue> _fieldSetter;
        private EventCallback<TValue> _callback { get; set; }

        private bool _autoRerender;
        private Action _valueChanged { get; set; }

        private ModelBase _model;
        private INotifyPropertyChanged _notify_host;
        private string _field_name;

        private bool _registered = false;

        private TValue _internal_value = default;

        private readonly LinkMode _linkMode = LinkMode.External;

        private IRerender _component;

        /// <summary>
        /// FieldLink is a helper class to keep a value and organize the signaling related.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="fieldExpression"></param>
        /// <param name="valueChanged"></param>
        /// <param name="callback"></param>
        public FieldLink(IRerender component, Expression<Func<TValue>> fieldExpression, bool autoRerender, Action valueChanged, EventCallback<TValue> callback)
            : this(component, fieldExpression, autoRerender, valueChanged)
        {
            // The chained constructor will be called first.

            // The callback is a eventcallback for the consumer of the component.
            _callback = callback;
        }

        /// <summary>
        /// FieldLink is a helper class to keep a value and organize the signaling related.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="fieldExpression"></param>
        /// <param name="valuechanged"></param>
        public FieldLink(IRerender component, Expression<Func<TValue>> fieldExpression, bool autoRerender, Action valuechanged)
        {
            _component = component;
            _autoRerender = autoRerender;
            _valueChanged = valuechanged;

            if (fieldExpression == null)
            {
                fieldExpression = () => this._internal_value;
                _linkMode = LinkMode.Internal;
            }

            _fieldExpression = fieldExpression;
            _fieldGetter = fieldExpression.Compile();
            _fieldSetter = BindingHelper.CreateValueSetter<TValue>(fieldExpression);

            BindingHelper.ParseAccessor(fieldExpression, out object model, out _field_name);

            _model = model as ModelBase;
            _notify_host = model as INotifyPropertyChanged;

            if (_model != null)
            {
                _registered = _model.IsRegistered(_field_name);
            }

            if (_notify_host != null)
                _notify_host.PropertyChanged += Model_PropertyChanged;

        }

        public void Dispose()
        {
            if (_notify_host != null)
                _notify_host.PropertyChanged -= Model_PropertyChanged;

            _internal_value = default;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _field_name && _inside_setter == 0)
            {
                if (this._valueChanged != null)
                {
                    _valueChanged();
                }

                if (this._autoRerender)
                {
                    _component.Rerender();
                }
            }

        }

        public Expression<Func<TValue>> Expression
        {
            get { return _fieldExpression; }
        }

        public async Task SetValueAsync(TValue value)
        {
            bool isEqual = EqualityComparer<TValue>.Default.Equals(_fieldGetter(), value);

            if (isEqual)
                return;

            try
            {
                _inside_setter++;
                _fieldSetter(value);
            }
            finally
            {
                _inside_setter--;
            }
            
            if (this._valueChanged != null)
            {
                _valueChanged();
            }
            
            if (_callback.HasDelegate)
            {
                await _callback.InvokeAsync(value);
            }

        }

        private int _inside_setter = 0;

        public TValue Value
        {
            get
            {
                return _fieldGetter();
            }
            set
            {
                bool isEqual = EqualityComparer<TValue>.Default.Equals(_fieldGetter(), value);

                if (isEqual)
                    return;

                try
                {
                    _inside_setter++;
                    _fieldSetter(value);
                }
                finally
                {
                    _inside_setter--;
                }

                if (this._valueChanged != null)
                {
                    _valueChanged();
                }

                if (_callback.HasDelegate)
                {
                    _ = _callback.InvokeAsync(value);
                }

            }
        }

        public ModelBase Model
        {
            get { return _model; }
        }

        public string PropertyName
        {
            get { return _field_name; }
        }

        /// <summary>
        /// Returns true when the property registered in the model's validation system.
        /// </summary>
        public bool Registered
        {
            get { return _registered; }
        }

        public LinkMode Mode
        {
            get { return _linkMode; }
        }

        public enum LinkMode
        {
            External,
            Internal
        }

    }
}
