using Microsoft.AspNetCore.Components;
using System;

namespace Kenova.Client.Components
{
    public partial class DateRangePicker : KenovaComponentBase
    {
        /// <summary>
        /// Attach a named properties config object to this instance of datepicker
        /// </summary>
        [Parameter]
        public string Config { get; set; }

        /// <summary>
        /// All unmatched parameters will be passed to parent element
        /// </summary>
        //[Parameter(CaptureUnmatchedValues = true)]
        //public Dictionary<string, object> Attributes { get; set; }

        /// <summary>
        /// Hide the apply and cancel buttons, and automatically apply a new date range as soon as two dates are clicked.
        /// </summary>
        [Parameter]
        public bool? AutoApply { get; set; }

        /// <summary>
        /// Pick a single date instead of a range. The start and end dates provided to your callback will be the same single date chosen. 
        /// </summary>
        [Parameter]
        public bool? SingleDatePicker { get; set; }

        /// <summary>
        /// Show only one calendar in the picker instead of two calendars.
        /// </summary>
        [Parameter]
        public bool? ShowOnlyOneCalendar { get; set; }

        /// <summary>
        /// The beginning date of the initially selected date range.
        /// </summary>
        [Parameter]
        public DateTimeOffset? StartDate { get; set; }

        [Parameter]
        public EventCallback<DateTimeOffset?> StartDateChanged { set; get; }

        /// <summary>
        /// The end date of the initially selected date range
        /// </summary>
        [Parameter]
        public DateTimeOffset? EndDate { get; set; }

        [Parameter]
        public EventCallback<DateTimeOffset?> EndDateChanged { set; get; }

        /// <summary>
        /// Specify the format string to display dates, default is Culture.DateTimeFormat.ShortDatePattern
        /// </summary>
        [Parameter]
        public string DateFormat { get; set; }

        /// <summary>
        /// Show localized week numbers at the start of each week on the calendars.
        /// </summary>
        [Parameter]
        public bool? ShowWeekNumbers { get; set; }

        /// <summary>
        /// Show ISO week numbers at the start of each week on the calendars.
        /// </summary>
        [Parameter]
        public bool? ShowISOWeekNumbers { get; set; }

        /// <summary>
        /// When enabled, the two calendars displayed will always be for two sequential months (i.e. January and February), and both will be advanced when clicking the left or right arrows above the calendars. When disabled, the two calendars can be individually advanced and display any month/year.
        /// </summary>
        [Parameter]
        public bool LinkedCalendars { get; set; }

        /// <summary>
        /// Show year and month select boxes above calendars to jump to a specific month and year.
        /// </summary>
        [Parameter]
        public bool? ShowDropdowns { get; set; } = true;

        /// <summary> Specify the culture to display dates and text in. Default is CultureInfo.CurrentCulture.</summary>
        [Parameter]
        public System.Globalization.CultureInfo Culture { get; set; }

        /// <summary>The text to display on the Week number heading</summary>
        [Parameter]
        public string WeekAbbreviation { get; set; }

        /// <summary>The day of the week to start from</summary>
        [Parameter]
        public DayOfWeek? FirstDayOfWeek { get; set; }

        /// <summary>The earliest date that can be selected, inclusive. A value of null indicates that there is no minimum date.</summary>
        [Parameter]
        public DateTimeOffset? MinDate { get; set; }

        /// <summary>The latest date that can be selected, inclusive. A value of null indicates that there is no maximum date.</summary>
        [Parameter]
        public DateTimeOffset? MaxDate { get; set; }

        /// <summary>
        /// The maximum TimeSpan between the selected start and end dates. A value of null indicates that there is no limit.
        /// </summary>
        [Parameter]
        public TimeSpan? MaxSpan { get; set; }

        /// <summary>
        /// Whether the picker should pick months based on selected range
        /// </summary>
        [Parameter]
        public bool? AutoAdjustCalendars { get; set; }

        /// <summary>
        /// A function that is passed each date in the two calendars before they are displayed, and may return true or false to indicate whether that date should be available for selection or not. 
        /// </summary>
        [Parameter]
        public Func<DateTimeOffset, bool> DaysEnabledFunction { get; set; }

        /// <summary>
        /// A function that is passed each date in the two calendars before they are displayed, and may return a string or array of CSS class names to apply to that date's calendar cell.
        /// </summary>
        [Parameter]
        public Func<DateTimeOffset, bool> CustomDateFunction { get; set; }

        /// <summary>
        /// Triggered when the apply button is clicked, or when a predefined range is clicked
        /// </summary>
        [Parameter]
        public EventCallback<DateRange> OnRangeSelect { get; set; }

        /// <summary>An event that is invoked on backdrop click (false) or cancel button click (true).</summary>
        [Parameter]
        public EventCallback<bool> OnCancel { get; set; }

        /// <summary>An event that is invoked when left or right calendar's month changed.</summary>
        [Parameter]
        public EventCallback OnMonthChanged { get; set; }

        public CalendarType LeftCalendar { get; set; }
        public CalendarType RightCalendar { get; set; }

        internal DateTimeOffset? OldStartValue { get; set; }
        internal DateTimeOffset? OldEndValue { get; set; }


        public DateRangePicker()
        {
            ShowDropdowns = true;
            AutoAdjustCalendars = true;
            Culture = System.Globalization.CultureInfo.CurrentCulture;
            WeekAbbreviation = string.Empty;
            DaysEnabledFunction = _ => true;
            CustomDateFunction = _ => false;
        }

        /// <summary>
        /// Show picker popup
        /// </summary>
        public void Open()
        {
            //if (Visible) return;

            OldStartValue = StartDate;
            OldEndValue = EndDate;

            if (AutoAdjustCalendars == true) AdjustCalendars();

            //ChosenLabel = CustomRangeLabel;
            //ShowCalendars();
            //Visible = true;
            //JSRuntime.InvokeAsync<object>("clickAndPositionHandler.addClickOutsideEvent", Id, ParentId, DotNetObjectReference.Create(this));
            //JSRuntime.InvokeAsync<object>("clickAndPositionHandler.getPickerPosition", Id, ParentId, Enum.GetName(typeof(DropsType), Drops).ToLower(), Enum.GetName(typeof(SideType), Opens).ToLower());
            //OnOpened.InvokeAsync(null);

            StateHasChanged();
        }

        public void AdjustCalendars()
        {
            LeftCalendar.Month = StartDate ?? DateTime.Now;
            RightCalendar.Month = EndDate ?? DateTime.Now.AddMonths(1);
            if (LeftCalendar.Month.Year == RightCalendar.Month.Year
                && LeftCalendar.Month.Month == RightCalendar.Month.Month)
            {
                RightCalendar.Month = RightCalendar.Month.AddMonths(1);
            }
        }

    }
}
