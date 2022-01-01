using BlazorDemo.Client.VenturaAutoCreate;
using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    public partial class SelectStaff : KenovaDialogBase
    {
        private string Title;

        [Parameter]
        public SelectStaffModel Model { get; set; } = new();

        ToolbarItemCollection toolbar = new ToolbarItemCollection();

        private HyperData<GetAll_sales_staffs_Record> data = new();

        protected override async Task OnDialogInitializedAsync()
        {
            this.Breadcrumb = "Staff";
            this.Title = "Staff";

            toolbar.Add("Select", SelectClicked, () => Model.Recordset.CurrentRecord != null, IconKind.FontAwesome, "fas fa-pencil-alt");
            toolbar.SourceCodeButton("BikeStores/SelectStaff");

            data.Items = this.Model.Recordset;
            data.SelectedItemExpression = () => Model.Recordset.CurrentRecord;
            //data.Mode = DisplayMode.Virtualization;

            data.Columns.Add(c => c.staff_id, "Staff#", 60, false);
            data.Columns.Add(c => c.first_name, "First name", 120, false);
            data.Columns.Add(c => c.last_name, "Last name", 120, false);

            await Model.SearchExec();

            Model.Recordset.FindAndSelectAsCurrent(p => p.staff_id == Model.staff_id);

        }

        private void SelectClicked()
        {
            Model.staff_id = Model.Recordset.CurrentRecord.staff_id;

            _ = this.CloseOkAsync();
        }

    }
}
