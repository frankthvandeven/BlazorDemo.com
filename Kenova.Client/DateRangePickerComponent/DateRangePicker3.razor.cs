using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using System;

namespace Kenova.Client.Components
{
    public partial class DateRangePicker : KenovaComponentBase, IKenovaComponent, IAsyncDisposable
    {
        private ComponentWingman<DateRangePicker> _wingman = new();
        private string container_id = KenovaClientConfig.GetUniqueElementID();

        [CascadingParameter]
        public KenovaDialogBase LayerComponent { get; set; }

        private Calendar CalendarLeft;
        private Calendar CalendarRight;

        protected override void OnInitialized()
        {
            if (LayerComponent == null)
                throw new InvalidOperationException($"The {this.GetType().Name} component must be placed inside a LayerBaseComponent");

            LayerComponent.RegisterComponent(this);

            if (string.IsNullOrEmpty(DateFormat))
            {
                DateFormat = Culture.DateTimeFormat.ShortDatePattern;
            }

            if (SingleDatePicker == true) AutoApply = true;

            if (!FirstDayOfWeek.HasValue)
            {
                FirstDayOfWeek = Culture.DateTimeFormat.FirstDayOfWeek;
            }

            LeftCalendar = new CalendarType(FirstDayOfWeek.Value);
            RightCalendar = new CalendarType(FirstDayOfWeek.Value);

            StartDate = StartDate?.Date;
            EndDate = EndDate?.Date.AddDays(1).AddTicks(-1);

            AdjustCalendars();

        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                await this.Mutex.WaitAsync();

                LayerComponent.UnregisterComponent(this);

                await _wingman.InvokeVoidAsync("Stop");
                await _wingman.DisposeAsync();
            }
            finally
            {
                this.Mutex.Release();
            }
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await this.Mutex.WaitAsync();

                if (firstRender)
                {
                    await _wingman.InstantiateAsync(this,"DateRangePickerComponent");
                    await _wingman.InvokeVoidAsync("Start", container_id);
                }

                await _wingman.InvokeVoidAsync("OnAfterRender");

                if (firstRender)
                {
                    LayerComponent.RegisterFirstRenderComplete(this); // must be at the end of OnAfterRender
                }

            }
            finally
            {
                this.Mutex.Release();
            }

        }


        public ValueTask SetFocusAsync()
        {
            return CalendarLeft.SetFocusAsync();
        }



        [Parameter]
        public string FocusID { get; set; } = null;

        [Parameter]
        public bool AutoFocus { get; set; } = false;

        [Parameter]
        public int AutoFocusPriority { get; set; } = 100;

        async ValueTask<bool> IKenovaComponent.PerformFocusAsync(string focusID)
        {
            if (this.FocusID != null && focusID == this.FocusID)
            {
                await this.SetFocusAsync();
                return true;
            }

            return false;
        }

        ValueTask IKenovaComponent.PerformAutoFocusAsync()
        {
            return this.SetFocusAsync();
        }

        ValueTask<int> IKenovaComponent.MeasureAutoFocusPriorityAsync()
        {
            if (this.AutoFocus)
            {
                return ValueTask.FromResult(this.AutoFocusPriority);
            }

            return ValueTask.FromResult(-1);
        }

        ValueTask<bool> IKenovaComponent.PerformEnterPressedAsync()
        {
            return ValueTask.FromResult(false);
        }

        ValueTask<bool> IKenovaComponent.PerformEscapePressedAsync()
        {
            return ValueTask.FromResult(false);
        }

        ValueTask<bool> IKenovaComponent.ComponentHiddenAsync()
        {
            return JavaScriptCaller.KNElementHiddenAsync(container_id);
        }

        ValueTask<bool> IKenovaComponent.IsChildOfAsync(string parent_id)
        {
            return JavaScriptCaller.KNIsChildOfAsync(parent_id, container_id);
        }

    }
}
