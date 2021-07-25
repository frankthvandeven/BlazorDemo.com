using Kenova.WebAssembly.Client.Components;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorDemo.Client.Components
{
    public partial class GridMultiCheck : ComponentBase
    {
        private List<Person> Persons = SampleData.CreatePersonsList();
        private HyperData<Person> GridList = new();
        private HyperData<Person> GridField = new();
        private Person SelectedPerson = null;

        protected override void OnInitialized()
        {
            // The first HyperGrid
            GridList.Items = Persons;
            //GridList.SelectedItemExpression = () => this.SelectedPerson;
            GridList.UseMultiCheck = MultiCheck.List;
            GridList.UseHeader = true;
            GridList.UseFilter = true;
            GridList.CheckedItemsChanged = CheckedItemsChanged;

            GridList.Columns.Add(c => c.Name, "Name", 200);
            GridList.Columns.Add(c => c.City, "City", 150);

            // Check the first two persons
            GridList.CheckedItems.Add(Persons[0]);
            GridList.CheckedItems.Add(Persons[1]);

            // The second HyperGrid
            GridField.Items = Persons;
            //GridField.SelectedItemExpression = () => this.SelectedPerson;

            GridField.IsChecked = c => c.IsSelected;
            GridField.SetChecked = c => c.IsSelected = true;
            GridField.SetUnchecked = c => c.IsSelected = false;
            GridField.UseMultiCheck = MultiCheck.ItemField;

            GridField.UseHeader = true;
            GridField.UseFilter = true;
            GridField.CheckedItemsChanged = CheckedItemsChanged;

            GridField.Columns.Add(c => c.Name, "Name", 200);
            GridField.Columns.Add(c => c.City, "City", 150);

            //GridFeld.RecalculateCheckedItemsCount();
            //GridList.RecalculateCheckedItemsCount();

        }

        private void CheckedItemsChanged()
        {

        }

        //public Cust_Recordset rs;



        //private void IncreaseDate()
        //{
        //    Model.SelectedDate = Model.SelectedDate.AddDays(1);

        //}

    }

}

