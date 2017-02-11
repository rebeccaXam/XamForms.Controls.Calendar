using System;
using Xamarin.Forms;

namespace XamForms.Controls
{
    public class CalendarButton : Button
    {
		public static readonly BindableProperty DateProperty =
            BindableProperty.Create(nameof(Date), typeof(DateTime?), typeof(CalendarButton), null);

        public DateTime? Date
        {
            get { return (DateTime?)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(CalendarButton), false);

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly BindableProperty IsOutOfMonthProperty =
            BindableProperty.Create(nameof(IsOutOfMonth), typeof(bool), typeof(CalendarButton), false);

        public bool IsOutOfMonth
        {
            get { return (bool)GetValue(IsOutOfMonthProperty); }
            set { SetValue(IsOutOfMonthProperty, value); }
        }

        public static readonly BindableProperty TextWithoutMeasureProperty =
            BindableProperty.Create(nameof(TextWithoutMeasure), typeof(string), typeof(Button), null);

        public string TextWithoutMeasure
        {
            get
            {
                var text = (string)GetValue(TextWithoutMeasureProperty);
                return string.IsNullOrEmpty(text) ? Text : text;
            }
            set { SetValue(TextWithoutMeasureProperty, value); }
        }

		public BackgroundPattern BackgroundPattern { get; set; }
    }
}

