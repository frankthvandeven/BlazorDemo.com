using Kenova.Client.SystemComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Text;

namespace Kenova.Client.Components
{

    /*
     * RenderTreeBuilder subroutines for rendering rows.
     */

    public partial class HyperGrid<ItemType> : KenovaComponentBase
    {

        private bool _didrender_header = false;
        private bool _didrender_table = false;
        private StringBuilder _container_style = new StringBuilder(100);

        private void BuildRow(RenderTreeBuilder __builder, ItemType item)
        {
            int row_index = this.Data.DisplayItems.IndexOf(item);

            BuildRow(__builder, item, row_index);
        }

        private void BuildRow(RenderTreeBuilder __builder, ItemType item, int row_index)
        {
            bool is_selected = item.Equals(this.Data.SelectedItem);

            string selectedStyle = null;

            bool row_enabled = true;

            if (this.Data.RowEnabledExpression != null)
            {
                row_enabled = this.Data.RowEnabledExpression(item);
            }

            string tooltip = null;

            if (this.Data.TooltipExpression != null)
            {
                tooltip = this.Data.GetTooltipString(item);
            }

            //int row_number = row_index; // Create a copy of the row_index integer
            //string tabindex = index == _focussedItemIndex ? "0" : "-1";

            if (row_enabled)
            {
                if (is_selected)
                {
                    selectedStyle = "selected";
                }

            }

            string modeclass = null;

            if (this.Data.DropdownMode)
                modeclass = "dropdownmode";

            __builder.OpenElement(94, "div");
            __builder.AddAttribute(95, "class", string.Concat(new string[]
            {
                    "kn-hyper-datarow ",
                    modeclass,
                    " ",
                    selectedStyle,
                    " ",
                    row_enabled ? "enabled" : "disabled"
            }));

            __builder.AddAttribute<MouseEventArgs>(96, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, delegate (MouseEventArgs e)
            {
                this.OnRowClick(e, item);
            }));

            if (this.Data.SelectedItemExpression != null)
            {
                __builder.AddAttribute<MouseEventArgs>(97, "ondblclick", EventCallback.Factory.Create<MouseEventArgs>(this, (MouseEventArgs e) =>
                {
                    this.OnRowDoubleClick(e, item);
                }));
            }

            /*
            __builder.AddAttribute<FocusEventArgs>(98, "onfocus", EventCallback.Factory.Create<FocusEventArgs>(this, delegate (FocusEventArgs e)
            {
                this.Div_DataRow_Focus(index);
            }));

            __builder.AddAttribute<FocusEventArgs>(99, "onblur", EventCallback.Factory.Create<FocusEventArgs>(this, delegate (FocusEventArgs e)
            {
                this.Div_DataRow_Focus(index);
            }));
            */

            //Do not set attribute tabindex, keep it outside shadow dom
            //__builder.AddAttribute(107, "tabindex", "-1");

            __builder.AddAttribute(108, "_idx", row_index); // index in this.Data.DisplayItems
            __builder.AddAttribute(109, "title", tooltip);
            __builder.SetKey(row_index);

            if (this.Data.UseMultiCheck != MultiCheck.Off)
            {
                __builder.OpenElement(115, "div");

                if (this.WidePadding == false)
                {
                    __builder.AddAttribute(119, "class", "kn-hyper-datacolumn");
                }
                else
                {
                    __builder.AddAttribute(123, "class", "kn-hyper-datacolumn widepadding");
                }

                __builder.AddAttribute(126, "style", "width:18px");
                __builder.AddAttribute<MouseEventArgs>(127, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, delegate ()
                {
                    this.OnOffBoxClicked(item);
                }));

                __builder.AddEventStopPropagationAttribute(132, "onclick", true);

                __builder.OpenComponent<OnOffBox>(134);
                __builder.AddAttribute(135, "Enabled", RuntimeHelpers.TypeCheck<bool>(row_enabled));
                __builder.AddAttribute(136, "Checked", RuntimeHelpers.TypeCheck<bool>(this.Data.GetItemIsChecked(item)));
                __builder.CloseComponent();

                __builder.CloseElement();
            }

            for (int col_index = 0; col_index < this.Data.Columns.Count; col_index++)
            {
                Column<ItemType> column = this.Data.Columns[col_index];
                BuildColumn(__builder, item, column, row_enabled, col_index);
            }

            __builder.CloseElement();
        }

        private void BuildColumn(RenderTreeBuilder __builder, ItemType item, Column<ItemType> column, bool row_enabled, int col_index)
        {
            string element_id = KenovaClientConfig.GetUniqueElementID(); // $"{_id_base}-{row_index}-{col_index}";

            string alignStyle = null;

            if (column.Kind == ColumnKind.IconDefinition)
            {
                alignStyle = "center";
            }

            __builder.OpenElement(162, "div");

            if (!this.WidePadding)
            {
                __builder.AddAttribute(166, "class", "kn-hyper-datacolumn " + alignStyle);
            }
            else
            {
                __builder.AddAttribute(170, "class", "kn-hyper-datacolumn widepadding " + alignStyle);
            }

            __builder.AddAttribute(173, "style", "width:" + column.Width.ToPixels());
            //__builder.AddAttribute(174, "id", element_id);

            if (column.Kind == ColumnKind.Field)
            {
                string html_string = column.ValueToHtml(item);

                if (column.DisplayAsHyperlink && row_enabled)
                {

                    __builder.OpenElement(183, "a");
                    __builder.AddAttribute(184, "href", "");

                    __builder.AddAttribute<MouseEventArgs>(186, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, (MouseEventArgs e) => this.OnAnchorClick(e, item, column.FieldName, element_id)));
                    __builder.AddEventPreventDefaultAttribute(187, "onclick", true);
                    __builder.AddEventStopPropagationAttribute(188, "onclick", true);

                    __builder.AddAttribute(190, "class", "kn-hyper-anchor");
                    __builder.AddContent(191, (MarkupString)html_string);
                    __builder.CloseElement();
                }
                else
                {
                    __builder.AddContent(196, (MarkupString)html_string);
                }
            }
            else
            {
                if (column.Kind == ColumnKind.IconDefinition)
                {
                    __builder.OpenComponent<FastIcon>(203);
                    __builder.AddAttribute(204, "Enabled", RuntimeHelpers.TypeCheck<bool>(row_enabled));
                    __builder.AddAttribute(205, "IconDefinition", RuntimeHelpers.TypeCheck<IconDefinition>(column.GetIconDefinition(item)));
                    __builder.CloseComponent();
                }
            }
            __builder.CloseElement();

        }

    }
}







/*

/// Virtualize source code:
/// https://github.com/dotnet/aspnetcore/blob/5fae4d619c9b7cf94dfe7a324014c1704b28c15f/src/Components/Web/src/Virtualization/Virtualize.cs#L324

/// <summary>
/// _arr holds the virtualization buffer.
/// To avoid creating an array for every request.
/// </summary>
private ItemType[] _arr = null;

private ValueTask<ItemsProviderResult<ItemType>> Virt_LoadData(ItemsProviderRequest request)
{
    if (this._hyperData == null)
    {
        return ValueTask.FromResult(new ItemsProviderResult<ItemType>(null, 0));
    }

    int totalRows = this.Data.DisplayItems.Count;

    if (totalRows == 0)
    {
        return ValueTask.FromResult(new ItemsProviderResult<ItemType>(null, 0));
    }

    // The remaining number of items counting from the StartIndex;
    int remaining = totalRows - request.StartIndex;

    // If remaining is less or equal to zero, then the list was changed outside our control.
    if (remaining <= 0)
    {
        return ValueTask.FromResult(new ItemsProviderResult<ItemType>(null, 0));
    }

    var numberofItems = Math.Min(request.Count, remaining);

    // Initialize a new array buffer if needed.
    if (_arr == null || _arr.Length != numberofItems)
    {
        _arr = new ItemType[numberofItems];
    }

    int index = request.StartIndex;

    for (int x = 0; x < numberofItems; x++)
    {
        _arr[x] = this.Data.DisplayItems[index];
        index++;
    }

    return ValueTask.FromResult(new ItemsProviderResult<ItemType>(_arr, totalRows));
}
*/
