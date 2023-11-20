using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;

namespace Kenova.Client.Components
{
    public partial class Remark : KenovaComponentBase, IRerender, IDisposable
    {

        private ValidatorLink Link;

        /// <summary>
        /// For example "() => this.Model.Name"
        /// </summary>
        [Parameter]
        public Expression<Func<object>> ForField { get; set; }

        /// <summary>
        /// The default value of this parameter is true.
        /// </summary>
        [Parameter]
        public bool WordWrap { get; set; } = true;

        [Parameter]
        public ModelBase Model { get; set; } = null;

        [Parameter]
        public string PropertyName { get; set; } = null;

        protected override void OnInitialized()
        {
            if (ForField == null && Model == null)
                throw new ArgumentNullException("Parameter ForField or Model+PropertyName must be set");

            if (ForField != null && (Model != null || PropertyName != null))
                throw new ArgumentNullException("Don't set both ForField and Model+PropertyName");

            if (Model != null && PropertyName == null)
                throw new ArgumentNullException("When parameter Model is set, parameter PropertyName must also be set");


            if (ForField != null)
                Link = new ValidatorLink(this, ForField);
            else
                Link = new ValidatorLink(this, Model, PropertyName);
        }

        public void Dispose()
        {
            Link.Dispose();
        }

        public void Rerender()
        {
            this.StateHasChanged();
        }


    }
}
