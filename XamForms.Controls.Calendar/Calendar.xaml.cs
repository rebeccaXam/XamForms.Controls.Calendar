using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamForms.Controls
{
	public partial class Calendar : ContentView
	{
		List<CalendarButton> buttons;
		public Grid MainCalendar;
		StackLayout calendar;

		public Calendar()
		{
			InitializeComponent();
			MonthNavigation.HeightRequest = Device.OS == TargetPlatform.Windows ? 50 : 32;
			TitleLabel = CenterLabel;
			TitleLeftArrow = LeftArrow;
			TitleRightArrow = RightArrow;
			MonthNavigationLayout = MonthNavigation;
			LeftArrow.Clicked += LeftArrowClickedEvent;
			RightArrow.Clicked += RightArrowClickedEvent;
			dayLabels = new List<Label>();
			weekNumberLabels = new List<Label>();
			buttons = new List<CalendarButton>();

			var columDef = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
			var rowDef = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
			DayLabels = new Grid { VerticalOptions = LayoutOptions.Start, RowSpacing = 0, ColumnSpacing = 0, Padding = 0 };
			DayLabels.ColumnDefinitions = new ColumnDefinitionCollection { columDef, columDef, columDef, columDef, columDef, columDef, columDef };
			MainCalendar = new Grid { VerticalOptions = LayoutOptions.Start, RowSpacing = 0, ColumnSpacing = 0, Padding = 1, BackgroundColor = BorderColor };
			MainCalendar.ColumnDefinitions = new ColumnDefinitionCollection { columDef, columDef, columDef, columDef, columDef, columDef, columDef };
			MainCalendar.RowDefinitions = new RowDefinitionCollection { rowDef, rowDef, rowDef, rowDef, rowDef, rowDef };
			WeekNumbers = new Grid { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.Start, RowSpacing = 0, ColumnSpacing = 0, Padding = new Thickness(0, 0, 0, 0) };
			WeekNumbers.ColumnDefinitions = new ColumnDefinitionCollection { columDef };
			WeekNumbers.RowDefinitions = new RowDefinitionCollection { rowDef, rowDef, rowDef, rowDef, rowDef, rowDef };
			CalendarViewType = DateTypeEnum.Normal;
			/*var panGesture = new PanGestureRecognizer();panGesture.PanUpdated += (s, e) =>
			{
				var t = e;
			};
			MainCalendar.GestureRecognizers.Add(panGesture);*/
			SelectedDates = new List<DateTime>();
			DayLabels.PropertyChanged += (sender, e) =>
			{
				if (DayLabels.Height > 0) WeekNumbers.Padding = new Thickness(0, DayLabels.Height, 0, 0);
			};
			YearsRow = 4;
			YearsColumn = 4;
		}

		#region MinDate

		public static readonly BindableProperty MinDateProperty =
			BindableProperty.Create(nameof(MinDate), typeof(DateTime?), typeof(Calendar), null,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeCalendar(CalandarChanges.MaxMin));

		/// <summary>
		/// Gets or sets the minimum date.
		/// </summary>
		/// <value>The minimum date.</value>
		public DateTime? MinDate
		{
			get { return (DateTime?)GetValue(MinDateProperty); }
			set { SetValue(MinDateProperty, value); ChangeCalendar(CalandarChanges.MaxMin); }
		}

		#endregion

		#region MaxDate

		public static readonly BindableProperty MaxDateProperty =
			BindableProperty.Create(nameof(MaxDate), typeof(DateTime?), typeof(Calendar), null,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeCalendar(CalandarChanges.MaxMin));

		/// <summary>
		/// Gets or sets the max date.
		/// </summary>
		/// <value>The max date.</value>
		public DateTime? MaxDate
		{
			get { return (DateTime?)GetValue(MaxDateProperty); }
			set { SetValue(MaxDateProperty, value); }
		}

		#endregion

		#region StartDate

		public static readonly BindableProperty StartDateProperty =
			BindableProperty.Create(nameof(StartDate), typeof(DateTime), typeof(Calendar), DateTime.Now,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeCalendar(CalandarChanges.StartDate));

		/// <summary>
		/// Gets or sets a date, to pick the month, the calendar is focused on
		/// </summary>
		/// <value>The start date.</value>
		public DateTime StartDate
		{
			get { return (DateTime)GetValue(StartDateProperty); }
			set { SetValue(StartDateProperty, value); }
		}

		#endregion

		#region StartDay

		public static readonly BindableProperty StartDayProperty =
			BindableProperty.Create(nameof(StartDate), typeof(DayOfWeek), typeof(Calendar), DayOfWeek.Sunday,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeCalendar(CalandarChanges.StartDay));

		/// <summary>
		/// Gets or sets the day the calendar starts the week with.
		/// </summary>
		/// <value>The start day.</value>
		public DayOfWeek StartDay
		{
			get { return (DayOfWeek)GetValue(StartDayProperty); }
			set { SetValue(StartDayProperty, value); }
		}

		#endregion

		#region BorderWidth

		public static readonly BindableProperty BorderWidthProperty =
			BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(Calendar), Device.OS == TargetPlatform.iOS ? 1 : 3,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeBorderWidth((int)newValue, (int)oldValue));

		protected void ChangeBorderWidth(int newValue, int oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => !b.IsSelected && b.IsEnabled).ForEach(b => b.BorderWidth = newValue);
		}

		/// <summary>
		/// Gets or sets the border width of the calendar.
		/// </summary>
		/// <value>The width of the border.</value>
		public int BorderWidth
		{
			get { return (int)GetValue(BorderWidthProperty); }
			set { SetValue(BorderWidthProperty, value); }
		}

		#endregion

		#region OuterBorder

		public static readonly BindableProperty OuterBorderWidthProperty =
			BindableProperty.Create(nameof(OuterBorderWidth), typeof(int), typeof(Calendar), Device.OS == TargetPlatform.iOS ? 1 : 3,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).MainCalendar.Padding = (int)newValue);

		/// <summary>
		/// Gets or sets the width of the whole calandar border.
		/// </summary>
		/// <value>The width of the outer border.</value>
		public int OuterBorderWidth
		{
			get { return (int)GetValue(OuterBorderWidthProperty); }
			set { SetValue(OuterBorderWidthProperty, value); }
		}

		#endregion

		#region BorderColor

		public static readonly BindableProperty BorderColorProperty =
			BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(Calendar), Color.FromHex("#dddddd"),
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeBorderColor((Color)newValue, (Color)oldValue));

		protected void ChangeBorderColor(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			MainCalendar.BackgroundColor = newValue;
			buttons.FindAll(b => b.IsEnabled && !b.IsSelected).ForEach(b => b.BorderColor = newValue);
		}

		/// <summary>
		/// Gets or sets the border color of the calendar.
		/// </summary>
		/// <value>The color of the border.</value>
		public Color BorderColor
		{
			get { return (Color)GetValue(BorderColorProperty); }
			set { SetValue(BorderColorProperty, value); }
		}

		#endregion

		#region DatesBackgroundColor

		public static readonly BindableProperty DatesBackgroundColorProperty =
			BindableProperty.Create(nameof(DatesBackgroundColor), typeof(Color), typeof(Calendar), Color.White,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDatesBackgroundColor((Color)newValue, (Color)oldValue));

		protected void ChangeDatesBackgroundColor(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsEnabled && (!b.IsSelected || !SelectedBackgroundColor.HasValue)).ForEach(b => b.BackgroundColor = newValue);
		}

		/// <summary>
		/// Gets or sets the background color of the normal dates.
		/// </summary>
		/// <value>The color of the dates background.</value>
		public Color DatesBackgroundColor
		{
			get { return (Color)GetValue(DatesBackgroundColorProperty); }
			set { SetValue(DatesBackgroundColorProperty, value); }
		}

		#endregion

		#region DatesTextColor

		public static readonly BindableProperty DatesTextColorProperty =
			BindableProperty.Create(nameof(DatesTextColor), typeof(Color), typeof(Calendar), Color.Black,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDatesTextColor((Color)newValue, (Color)oldValue));

		protected void ChangeDatesTextColor(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsEnabled && (!b.IsSelected || !SelectedTextColor.HasValue) && !b.IsOutOfMonth).ForEach(b => b.TextColor = newValue);
		}

		/// <summary>
		/// Gets or sets the text color of the normal dates.
		/// </summary>
		/// <value>The color of the dates text.</value>
		public Color DatesTextColor
		{
			get { return (Color)GetValue(DatesTextColorProperty); }
			set { SetValue(DatesTextColorProperty, value); }
		}

		#endregion

		#region DatesFontAttributes

		public static readonly BindableProperty DatesFontAttributesProperty =
			BindableProperty.Create(nameof(DatesFontAttributes), typeof(FontAttributes), typeof(Calendar), FontAttributes.None,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDatesFontAttributes((FontAttributes)newValue, (FontAttributes)oldValue));

		protected void ChangeDatesFontAttributes(FontAttributes newValue, FontAttributes oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsEnabled && (!b.IsSelected || !SelectedTextColor.HasValue) && !b.IsOutOfMonth).ForEach(b => b.FontAttributes = newValue);
		}

		/// <summary>
		/// Gets or sets the dates font attributes.
		/// </summary>
		/// <value>The dates font attributes.</value>
		public FontAttributes DatesFontAttributes
		{
			get { return (FontAttributes)GetValue(DatesFontAttributesProperty); }
			set { SetValue(DatesFontAttributesProperty, value); }
		}

		#endregion

		#region DatesFontSize

		public static readonly BindableProperty DatesFontSizeProperty =
			BindableProperty.Create(nameof(DatesFontSize), typeof(double), typeof(Calendar), 20.0,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDatesFontSize((double)newValue, (double)oldValue));

		protected void ChangeDatesFontSize(double newValue, double oldValue)
		{
			if (Math.Abs(newValue - oldValue) < 0.01) return;
			buttons.FindAll(b => !b.IsSelected && b.IsEnabled).ForEach(b => b.FontSize = newValue);
		}

		/// <summary>
		/// Gets or sets the font size of the normal dates.
		/// </summary>
		/// <value>The size of the dates font.</value>
		public double DatesFontSize
		{
			get { return (double)GetValue(DatesFontSizeProperty); }
			set { SetValue(DatesFontSizeProperty, value); }
		}

		#endregion

		#region DateCommand

        public static readonly BindableProperty DateCommandProperty =
            BindableProperty.Create(nameof(DateCommand), typeof(ICommand), typeof(Calendar), null);

        /// <summary>
        /// Gets or sets the selected date command.
        /// </summary>
        /// <value>The date command.</value>
        public ICommand DateCommand
        {
            get { return (ICommand)GetValue(DateCommandProperty); }
            set { SetValue(DateCommandProperty, value); }
        }

		#endregion

		public DateTime CalendarStartDate
		{
			get
			{
				var start = StartDate;
				var beginOfMonth = start.Day == 1;
				while (!beginOfMonth || start.DayOfWeek != StartDay)
				{
					start = start.AddDays(-1);
					beginOfMonth |= start.Day == 1;
				}
				return start;
			}
		}

		#region Functions

		protected async override void OnParentSet()
		{
			if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
			{
				FillCalendarWindows();
			}
			else {
				// iOS and Android can create controls on another thread when they are not attached to the main ui yet, 
				// windows can not
				await FillCalendar();
			}
			calendar = new StackLayout { Padding = 0, Spacing = 0, Orientation = StackOrientation.Vertical, Children = { DayLabels, MainCalendar } };
			ShowHideWeekNumbers();
			base.OnParentSet();
		}

		protected Task FillCalendar()
		{
			return Task.Factory.StartNew(() =>
			{
				FillCalendarWindows();
			});
		}

		protected void FillCalendarWindows()
		{
			for (int r = 0; r < 6; r++)
			{
				for (int c = 0; c < 7; c++)
				{
					if (r == 0)
					{
						dayLabels.Add(new Label
						{
							HorizontalOptions = LayoutOptions.Center,
							VerticalOptions = LayoutOptions.Center,
							TextColor = WeekdaysTextColor,
							FontSize = WeekdaysFontSize,
							FontAttributes = WeekdaysFontAttributes
						});
						DayLabels.Children.Add(dayLabels.Last(), c, r);
					}
					buttons.Add(new CalendarButton
					{
						BorderRadius = 0,
						BorderWidth = BorderWidth,
						BorderColor = BorderColor,
						FontSize = DatesFontSize,
						BackgroundColor = DatesBackgroundColor,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand
					});
					buttons.Last().Clicked += DateClickedEvent;
					MainCalendar.Children.Add(buttons.Last(), c, r);
				}
				weekNumberLabels.Add(new Label
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					TextColor = NumberOfWeekTextColor,
					BackgroundColor = NumberOfWeekBackgroundColor,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Center,
					FontSize = NumberOfWeekFontSize,
					FontAttributes = WeekdaysFontAttributes
				});
				WeekNumbers.Children.Add(weekNumberLabels.Last(), 0, r);
			}
			WeekNumbers.WidthRequest = NumberOfWeekFontSize + (NumberOfWeekFontSize / 2) + 6;
			ChangeCalendar(CalandarChanges.All);
		}

        protected void ChangeCalendar(CalandarChanges changes)
        {
			Device.BeginInvokeOnMainThread(() =>
			{
				if (changes.HasFlag(CalandarChanges.StartDate))
				{
					CenterLabel.Text = StartDate.ToString(TitleLabelFormat);
					ChangeWeekNumbers();
				}

				var start = CalendarStartDate.Date;
				var beginOfMonth = false;
				var endOfMonth = false;
				for (int i = 0; i < buttons.Count; i++)
				{
					endOfMonth |= beginOfMonth && start.Day == 1;
					beginOfMonth |= start.Day == 1;

					if (i < 7 && WeekdaysShow && changes.HasFlag(CalandarChanges.StartDay))
					{
						dayLabels[i].Text = start.ToString(WeekdaysFormat);
					}

					if (changes.HasFlag(CalandarChanges.All))
					{
						buttons[i].Text = string.Format("{0}", start.Day);
					}
					else
					{
						buttons[i].TextWithoutMeasure = string.Format("{0}", start.Day);
					}
					buttons[i].Date = start;

					buttons[i].IsOutOfMonth = !(beginOfMonth && !endOfMonth);

					SpecialDate sd = null;
					if (SpecialDates != null)
					{
						sd = SpecialDates.FirstOrDefault(s => s.Date.Date == start.Date);
					}

					if ((MinDate.HasValue && start < MinDate) || (MaxDate.HasValue && start > MaxDate) || (DisableAllDates && sd == null))
					{
						SetButtonDisabled(buttons[i]);
					}
					else if (SelectedDates.Contains(start.Date))
					{
						SetButtonSelected(buttons[i], sd);
					}
					else if (sd != null)
					{
						SetButtonSpecial(buttons[i], sd);
					}
					else
					{
						SetButtonNormal(buttons[i]);
					}
					start = start.AddDays(1);
				}
			});
        }

        protected void SetButtonNormal(CalendarButton button)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                button.IsEnabled = true;
                button.IsSelected = false;
                button.FontSize = DatesFontSize;
                button.BorderWidth = BorderWidth;
                button.BorderColor = BorderColor;
                button.BackgroundColor = button.IsOutOfMonth ? DatesBackgroundColorOutsideMonth : DatesBackgroundColor;
                button.TextColor = button.IsOutOfMonth ? DatesTextColorOutsideMonth : DatesTextColor;
				button.FontAttributes = button.IsOutOfMonth ? DatesFontAttributesOutsideMonth : DatesFontAttributes;
            });
        }

		protected void DateClickedEvent(object s, EventArgs a)
		{
			var selectedDate = (s as CalendarButton).Date;
			if (SelectedDate.HasValue && selectedDate.HasValue && SelectedDate.Value == selectedDate.Value)
			{
				ChangeSelectedDate(selectedDate);
				SelectedDate = null;
			}
			else
			{
				SelectedDate = selectedDate;
			}
		}

		#endregion

        public event EventHandler<DateTimeEventArgs> DateClicked;
    }
}

