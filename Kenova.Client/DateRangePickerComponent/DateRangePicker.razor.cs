using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kenova.Client.Components
{
    public partial class DateRangePicker : KenovaComponentBase
    {
        private DateTimeOffset? HoverDate { get; set; }

        private string EditText { get; set; }

        public string FormattedRange
        {
            get
            {
                if (!string.IsNullOrEmpty(EditText))
                {
                    return EditText;
                }

                if (SingleDatePicker == true && StartDate != null)
                {
                    return $"{StartDate.Value.ToString(DateFormat)}";
                }

                if (StartDate != null && EndDate != null)
                {
                    return $"{StartDate.Value.ToString(DateFormat)} - {EndDate.Value.ToString(DateFormat)}";
                }
                else
                {
                    return string.Empty;
                }
            }
            set { }
        }

        //private string RootStyles
        //{
        //    get
        //    {
        //        var result = new List<string>();
        //        if (AutoApply == true) { result.Add("auto-apply"); }
        //        result.Add("show-calendar");

        //        result.Add("inline");

        //        return string.Join(" ", result);
        //    }
        //}

        public async Task OnTextInput(ChangeEventArgs e)
        {
            EditText = e.Value.ToString();
            if (SingleDatePicker != true && !e.Value.ToString().Contains("-")) { return; }
            var dateStrings = e.Value.ToString().Split('-').Select(s => s.Trim()).ToList();
            if (SingleDatePicker == true)
            {
                dateStrings = new List<string> { e.Value.ToString(), string.Empty };
            }
            if (dateStrings.Count != 2) { return; }

            var startDateParsed = DateTimeOffset.TryParseExact(dateStrings[0], DateFormat, Culture, System.Globalization.DateTimeStyles.None, out var startDate);
            var endDateParsed = DateTimeOffset.TryParseExact(dateStrings[1], DateFormat, Culture, System.Globalization.DateTimeStyles.None, out var endDate);

            if (endDateParsed && endDate > MaxDate)
            {
                endDate = MaxDate.Value.Date;
            }

            if (startDateParsed && startDate < MinDate)
            {
                startDate = MinDate.Value.Date.AddDays(1).AddTicks(-1);
            }

            if (startDateParsed && SingleDatePicker == true)
            {
                StartDate = startDate.Date;
                EndDate = startDate.Date;
                await ClickApply();
                EditText = null;
            }

            if (startDateParsed && endDateParsed && startDate < endDate
                && (!MinDate.HasValue || startDate > MinDate)
                && (!MaxDate.HasValue || endDate < MaxDate))
            {
                StartDate = startDate.Date;
                EndDate = endDate.Date.AddDays(1).AddTicks(-1);
                await ClickApply();
                EditText = null;
            }
        }

        public void LostFocus(FocusEventArgs e)
        {
            EditText = null;
        }

        private void MonthChanged(DateTimeOffset date, SideType side)
        {
            if (side == SideType.Left)
            {
                LeftCalendar.Month = date;
                if (LinkedCalendars == true)
                {
                    RightCalendar.Month = LeftCalendar.Month.AddMonths(1);
                }
            }
            else
            {
                RightCalendar.Month = date;
                if (LinkedCalendars == true)
                {
                    LeftCalendar.Month = RightCalendar.Month.AddMonths(-1);
                }
            }
            _ = OnMonthChanged.InvokeAsync(null);
        }

        private async Task ClickDate(DateTimeOffset date)
        {
            HoverDate = null;

            if (SingleDatePicker == true)
            {
                StartDate = date.Date;
                EndDate = StartDate;
                await ClickApply();
                return;
            }

            if (EndDate.HasValue || StartDate == null || date < StartDate)
            { //picking start
                EndDate = null;
                StartDate = date.Date;
            }
            else if (!EndDate.HasValue && date < StartDate)
            {
                //special case: clicking the same date for start/end,
                //but the time of the end date is before the start date
                EndDate = StartDate;
            }
            else
            { // picking end
                EndDate = date.Date.AddDays(1).AddTicks(-1);
                if (AutoApply == true)
                {
                    await ClickApply();
                }
            }

        }

        private void OnHoverDate(DateTimeOffset date)
        {
            if (!EndDate.HasValue)
            {
                HoverDate = date;
            }
        }

        public async Task ClickApply()
        {
            if (StartDate.HasValue && EndDate.HasValue)
            {
                StateHasChanged();
                await StartDateChanged.InvokeAsync(StartDate.Value);
                await EndDateChanged.InvokeAsync(EndDate.Value);
                await OnRangeSelect.InvokeAsync(new DateRange { Start = StartDate.Value, End = EndDate.Value });
            }
            //Close();
        }

        public void ClickCancel()
        {
            StartDate = OldStartValue;
            EndDate = OldEndValue;
            //Close();
            _ = OnCancel.InvokeAsync(true);
        }

    }
}
