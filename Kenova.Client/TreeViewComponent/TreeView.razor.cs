using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Kenova.Client.Components
{


    public partial class TreeView : KenovaComponentBase
    {

        //private StringBuilder _container_style = new StringBuilder(100);

        [Parameter]
        public TreeViewData Data { get; set; } = null;

        private TreeViewData _tvData;

        protected override void OnInitialized()
        {
            if (this.Data == null)
            {
                throw new InvalidOperationException("Parameter Data is not set.");
            }

            // Data can only be set once.
            _tvData = this.Data;

        }

        private void OnRowClick(MouseEventArgs e, TreeViewRenderItem item)
        {
            if (item.ChildCount == 0)
                return;

            var tvi = item.Item;

            tvi.IsExpanded = !tvi.IsExpanded;

            this.Data.CreateRenderList();

            // statehaschanged komt vanzelf.
        }



    }
}
