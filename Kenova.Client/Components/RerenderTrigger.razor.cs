using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Kenova.Client.Components
{

    public partial class RerenderTrigger : KenovaComponentBase, IDisposable
    {

        [Parameter]
        public Expression<Func<object>> PropertyExpression { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private Expression<Func<object>> _propertyExpression;
        private string _propertyFieldName;
        private INotifyPropertyChanged _model;

        protected override void OnInitialized()
        {
            if (this.PropertyExpression == null)
                throw new InvalidOperationException("Parameter PropertyExpression is not set");

            _propertyExpression = this.PropertyExpression;

            BindingHelper.ParseAccessor(this.PropertyExpression, out object model, out _propertyFieldName);

            _model = model as INotifyPropertyChanged;

            if (_model != null)
                _model.PropertyChanged += Model_PropertyChanged;

        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (KenovaClientConfig.Diagnostics)Console.WriteLine($"Rerender component forcing a rerender (property {e.PropertyName})");
            this.StateHasChanged();
        }

        public void Dispose()
        {
            if (_model != null)
                _model.PropertyChanged -= Model_PropertyChanged;
        }

    }
}
