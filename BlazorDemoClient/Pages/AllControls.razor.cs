using BlazorDemo.Client.Components;
using Kenova.WebAssembly.Client.Components;
using Kenova.WebAssembly.Client.Util;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Pages
{
    public partial class AllControls : LayerComponentBase
    {
        private readonly DemoModel Model = new();
        private readonly AllControlsModel Model2 = new();
        private MenuItemCollection MenuItems = new();
        private string CssVersion;

        private TabItemCollection tabItems = new();
        private ToolbarItemCollection toolbarButtons = new();
        private List<Person> Persons = SampleData.CreatePersonsList();
        private List<Person> CheckedPersons = new();

        private Person SelectedPerson = null;

        private string SelectedState = "AL";

        private string InputString = "The Kenova System";
        private DateTime InputDate = DateTime.Now;
        private DateTime? InputDateNullable = DateTime.Now;

        private bool IsEnabled = false;

        //private HyperGrid<Cust_Record> _datagrid;

        private InputString input_string;

        private const string SVG_TAB = "<svg viewBox='0 -20 512 512'><path d='m465 81.433594v-21.433594c0-33.085938-26.914062-60-60-60h-345c-33.085938 0-60 26.914062-60 60v352c0 33.085938 26.914062 60 60 60h392c33.085938 0 60-26.914062 60-60v-272c0-28.617188-20.148438-52.609375-47-58.566406zm-40-21.433594v20h-90v-20c0-7.011719-1.21875-13.738281-3.441406-20h73.441406c11.027344 0 20 8.972656 20 20zm-150-20c11.027344 0 20 8.972656 20 20v20h-91v-20c0-7.011719-1.21875-13.738281-3.441406-20zm197 372c0 11.027344-8.972656 20-20 20h-392c-11.027344 0-20-8.972656-20-20v-352c0-11.027344 8.972656-20 20-20h84c11.027344 0 20 8.972656 20 20v60h288c11.027344 0 20 8.972656 20 20zm0 0'/></svg>";

        protected override void OnLayerInitialized()
        {
            Breadcrumb = "All Kenova Controls";

            MenuItems.Add("Introduction", "intro", null, IconKind.FontAwesome, "fal fa-medal");
            MenuItems.Add("Headers", "headers", null, IconKind.FontAwesome, "fas fa-heading");
            MenuItems.Add("Icons", "icons", null, IconKind.FontAwesome, "fal fa-icons");
            MenuItems.Add("Models and Validation", "validation", null, IconKind.FontAwesome, "fas fa-box-check");
            MenuItems.Add("Basic controls", "basic", null, IconKind.FontAwesome, "fas fa-gamepad-alt");
            MenuItems.Add("Input controls", "input", null, IconKind.FontAwesome, "fas fa-keyboard");
            MenuItems.Add("Display controls", "display", null, IconKind.FontAwesome, "far fa-eye");
            MenuItems.Add("InputMultiline", "inputmultiline", null, IconKind.FontAwesome, "far fa-line-height");
            MenuItems.Add("Overlays", "overlays", null, IconKind.FontAwesome, "fal fa-layer-group");
            MenuItems.Add("Tab pages", "tabs", null, IconKind.Vector);
            MenuItems.Icon.IconData = SVG_TAB;
            MenuItems.Add("Toolbar", "toolbar", null, IconKind.FontAwesome, "fas fa-tools");

            MenuItems.Add("DropdownList", "dropdownlist", null, IconKind.FontAwesome, "fas fa-tools");
            MenuItems.Add("DropdownListBasic", "dropdownlistbasic", null, IconKind.FontAwesome, "fas fa-tools");
            MenuItems.Add("DropdownMultiCheckList", "dropdownmultichecklist", null, IconKind.FontAwesome, "fas fa-tools");

            tabItems.Add("1", "First tab");
            tabItems.Add("2", "Second tab");
            tabItems.Add("3", "Third tab");
            tabItems.Add("4", "Fourth tab");
            tabItems.Add("5", "Fifth tab");
            tabItems.Add("6", "Sixth tab");
            tabItems.Add("7", "Seventh tab");
            tabItems.Add("8", "Eighth tab");
            tabItems.Add("9", "Ninth tab");
            tabItems.Add("10", "Tenth tab");
            tabItems.Add("11", "Eleventh tab");
            tabItems.Add("12", "Twelfth tab");

            toolbarButtons.Add("Add", null, null, IconKind.Vector);
            toolbarButtons.Icon.IconData = "<svg viewBox=\"0 0 448 512\"><path d=\"M400 64c8.8 0 16 7.2 16 16v352c0 8.8-7.2 16-16 16H48c-8.8 0-16-7.2-16-16V80c0-8.8 7.2-16 16-16h352m0-32H48C21.5 32 0 53.5 0 80v352c0 26.5 21.5 48 48 48h352c26.5 0 48-21.5 48-48V80c0-26.5-21.5-48-48-48zm-60 206h-98v-98c0-6.6-5.4-12-12-12h-12c-6.6 0-12 5.4-12 12v98h-98c-6.6 0-12 5.4-12 12v12c0 6.6 5.4 12 12 12h98v98c0 6.6 5.4 12 12 12h12c6.6 0 12-5.4 12-12v-98h98c6.6 0 12-5.4 12-12v-12c0-6.6-5.4-12-12-12z\"/></svg>";
            toolbarButtons.Add("Volume", null, null, IconKind.FontAwesome, "fal fa-phone-volume");
            toolbarButtons.Add("Manage view", null, null, IconKind.FontAwesome, "fal fa-cog");
            toolbarButtons.Add("Refresh", null, null, IconKind.FontAwesome, "fal fa-redo");
            toolbarButtons.Add("Export to CSV", null, null, IconKind.FontAwesome, "fal fa-arrow-to-bottom");
            toolbarButtons.Add("Power off", null, () => this.IsEnabled, IconKind.FontAwesome, "fal fa-power-off");
            toolbarButtons.Add("Battery", null, null, IconKind.FontAwesome, "fal fa-battery-full");
            toolbarButtons.Add("Patient", null, null, IconKind.FontAwesome, "far fa-head-side-medical");
            toolbarButtons.Add("Select", null, null, IconKind.FontAwesome, "far fa-bullseye-pointer");
            toolbarButtons.Add("Edit", null, null, IconKind.FontAwesome, "fas fa-pencil-alt");
            toolbarButtons.Add("New", null, null, IconKind.FontAwesome, "fas fa-plus");
            toolbarButtons.Add("Delete", null, null, IconKind.FontAwesome, "fas fa-trash");
            toolbarButtons.Add("Close", null, null, IconKind.FontAwesome, "far fa-times");
            toolbarButtons.Add("Finish", null, null, IconKind.FontAwesome, "fas fa-flag-checkered");

            // Set the second person in the list as the selected person.
            this.SelectedPerson = Persons[1];

            // Adding the first and second Person to the list of checked persons.
            CheckedPersons.Add(Persons[0]);
            CheckedPersons.Add(Persons[1]);

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                CssVersion = await JavaScriptCaller.KNGetCssVariableAsync("--kenova-css-version");
                this.StateHasChanged();
            }
        }


        private void CheckedItemsChanged()
        {

        }

        private void OpenFirstOverlay()
        {
            var ld = new LayerDefinition<OverlayTester>
            {
                Kind = LayerKind.Modal,
                [p => p.Number] = 0
            };

            _ = ld.OpenNonBlockingAsync();

        }

        private void DropdownListBasic_FieldChanged()
        {
            // Doing nothing is enough. Just defining an EventCallback is enough for a re-render to start.
        }

        private void SetStateToNY_Clicked()
        {
            this.SelectedState = "NY";
        }

        private void BtnSelectTab2()
        {
            this.Model2.SelectedTab = "2";
        }

        private void SelectBasicControls()
        {
            this.Model2.SelectedMenuItem = "basic";
        }

    }
}

