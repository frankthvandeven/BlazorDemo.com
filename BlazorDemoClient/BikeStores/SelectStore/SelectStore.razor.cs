using BlazorDemo.Client.VenturaAutoCreate;
using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    public partial class SelectStore : KenovaDialogBase
    {
        private string Title;

        [Parameter]
        public SelectStoreModel Model { get; set; } = new();

        ToolbarItemCollection toolbar = new ToolbarItemCollection();

        private HyperData<GetAll_sales_stores_Record> data = new();

        protected override async Task OnDialogInitializedAsync()
        {
            this.Breadcrumb = "Store";
            this.Title = "Store";

            toolbar.Add("Select", SelectClicked, () => Model.Recordset.CurrentRecord != null, IconKind.FontAwesome, "fas fa-pencil-alt");
            toolbar.SourceCodeButton("BikeStores/SelectStore");

            data.Items = this.Model.Recordset;
            data.SelectedItemExpression = () => Model.Recordset.CurrentRecord;
            //data.Mode = DisplayMode.Virtualization;

            data.Columns.Add(c => c.store_id, "Store#", 60, false);
            data.Columns.Add(c => c.store_name, "Name", 200, false);

            await Model.SearchExec();

            Model.Recordset.FindAndSelectAsCurrent(p => p.store_id == Model.store_id);

        }

        private void SelectClicked()
        {
            Model.store_id = Model.Recordset.CurrentRecord.store_id;

            _ = this.CloseOkAsync();
        }

    }
}
