using Microsoft.AspNetCore.Components;
using System;

//
// MULTISELECT / CHECKBOX functionality.
//

namespace Kenova.Client.Components
{

    public partial class HyperGrid<ItemType> : KenovaComponentBase
    {
        private bool HeaderCheckboxChecked = false;
        private bool HeaderCheckboxIndeterminate = false;

        private void HeaderCheckBoxChanged()
        {
            HeaderCheckboxIndeterminate = false;

            if (HeaderCheckboxChecked)
                this.Data.CheckAll();
            else
                this.Data.UncheckAll();

        }

        private void HeaderCheckBoxIndeterminateClicked()
        {
            HeaderCheckboxChecked = true;
            HeaderCheckboxIndeterminate = false;

            this.Data.CheckAll();

        }

        /// <summary>
        /// A div element was clicked.
        /// </summary>
        private void OnOffBoxClicked(ItemType item)
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("OnOffClicked");
            this.Data.ToggleCheck(item);
        }

        /// <summary>
        /// This method is only called while Rendering.
        /// </summary>
        private void CalculateHeaderCheckbox()
        {

            if (this.Data.CheckedItemsCount == 0)
            {
                HeaderCheckboxChecked = false;
                HeaderCheckboxIndeterminate = false;
            }
            else if (this.Data.CheckedItemsCount == this.Data.DisplayItems.Count)
            {
                HeaderCheckboxChecked = true;
                HeaderCheckboxIndeterminate = false;
            }
            else
            {
                HeaderCheckboxChecked = false;
                HeaderCheckboxIndeterminate = true;
            }

        }



    }

}


