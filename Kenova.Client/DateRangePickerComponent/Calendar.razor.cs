using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kenova.Client.Components
{
    public partial class Calendar : KenovaComponentBase
    {
        [CascadingParameter] public DateRangePicker Picker { get; set; }

        [Parameter] public CalendarType CalendarData { get; set; }
        [Parameter] public bool? ShowWeekNumbers { get; set; } = false;
        [Parameter] public bool? ShowISOWeekNumbers { get; set; } = false;
        //[Parameter] public DateTimeOffset? MinDate { get; set; }
        //[Parameter] public DateTimeOffset? MaxDate { get; set; }
        [Parameter] public bool LinkedCalendars { get; set; } = true;
        [Parameter] public SideType? Side { get; set; }
        [Parameter] public bool? ShowDropdowns { get; set; } = true;
        [Parameter] public bool? SingleDatePicker { get; set; } = false;
        [Parameter] public string WeekAbbreviation { get; set; } = string.Empty;
        [Parameter] public DateTimeOffset? HoverDate { get; set; }
        [Parameter] public CultureInfo Culture { get; set; }
        [Parameter] public Func<DateTimeOffset, bool> DaysEnabledFunction { get; set; }
        [Parameter] public Func<DateTimeOffset, bool> CustomDateFunction { get; set; }

        [Parameter] public EventCallback<DateTimeOffset> OnMonthChanged { get; set; }
        [Parameter] public EventCallback<DateTimeOffset> OnClickDate { get; set; }
        [Parameter] public EventCallback<DateTimeOffset> OnHoverDate { get; set; }

        [Parameter] public TimeSpan? MaxSpan { get; set; }


        private readonly string _select_year_id = KenovaClientConfig.GetUniqueElementID();


        private int MinYear
        {
            get { return Picker.MinDate?.Year ?? 1900; }
        }

        private int MaxYear
        {
            get { return Picker.MaxDate?.Year ?? DateTime.Now.AddYears(100).Year; }
        }

        private StringBuilder sb = new StringBuilder(255);

        private List<string> DayNames { get; set; } = new List<string>();

        private List<string> GetDayNames()
        {
            var dayNames = Culture.DateTimeFormat.ShortestDayNames.ToList();
            var firstDayNumber = (int)CalendarData.FirstDayOfWeek;
            if (firstDayNumber > 0)
            {
                for (int i = 0; i < firstDayNumber; i++)
                {
                    var item = dayNames[0];
                    dayNames.Insert(dayNames.Count, item);
                    dayNames.RemoveAt(0);
                }
            }
            return dayNames;
        }

        protected override void OnInitialized()
        {
            DayNames = GetDayNames();
        }

        public ValueTask SetFocusAsync()
        {
            return JavaScriptCaller.KNFocusAsync(this._select_year_id);
        }

        public bool IsDayDisabled(DateTimeOffset date)
        {
            if (DaysEnabledFunction != null)
            {
                return !DaysEnabledFunction(date);
            }
            else
            {
                return false;
            }
        }

        public bool IsCustomDate(DateTimeOffset date)
        {
            if (CustomDateFunction != null)
            {
                return CustomDateFunction(date);
            }
            else
            {
                return false;
            }
        }

        private Task PreviousMonth(MouseEventArgs e)
        {
            return OnMonthChanged.InvokeAsync(CalendarData.Month.AddMonths(-1));
        }

        private Task NextMonth(MouseEventArgs e)
        {
            return OnMonthChanged.InvokeAsync(CalendarData.Month.AddMonths(1));
        }

        private Task MonthSelected(ChangeEventArgs e)
        {
            var month = int.Parse(e.Value.ToString());
            var d = CalendarData.Month;
            return OnMonthChanged.InvokeAsync(new DateTime(d.Year, month, d.Day, d.Hour, d.Minute, d.Second));
        }

        private Task YearSelected(ChangeEventArgs e)
        {
            var year = int.Parse(e.Value.ToString());
            var d = CalendarData.Month;
            return OnMonthChanged.InvokeAsync(new DateTime(year, d.Month, d.Day, d.Hour, d.Minute, d.Second));
        }

        private Task ClickDate(bool disabled, DateTimeOffset date)
        {
            if (disabled)
                return Task.CompletedTask;

            return OnClickDate.InvokeAsync(date);
        }

        private Task OnMouseOverDate(DateTimeOffset date)
        {
            if (HoverDate != date)
            {
                return OnHoverDate.InvokeAsync(date);
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        private int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}
