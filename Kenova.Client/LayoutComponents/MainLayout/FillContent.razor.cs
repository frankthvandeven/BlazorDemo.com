using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;

namespace Kenova.Client.Components
{

    public partial class FillContent : KenovaComponentBase, IDisposable
    {
        [Parameter]
        public INotifyPropertyChanged RerenderTrigger { get; set; } = null;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private INotifyPropertyChanged _rerender;

        protected override void OnInitialized()
        {
            _rerender = RerenderTrigger;

            if (_rerender != null)
                _rerender.PropertyChanged += Model_PropertyChanged;

        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (KenovaClientConfig.Diagnostics) Console.WriteLine($"TopContent forcing a rerender (property {e.PropertyName})");
            this.StateHasChanged();
        }

        public void Dispose()
        {
            if (_rerender != null)
                _rerender.PropertyChanged -= Model_PropertyChanged;
        }

    }
}
