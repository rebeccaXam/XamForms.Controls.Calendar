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
        List<Label> labels;
        List<CalendarButton> buttons;
        public Grid DayLabels, MainCalendar;

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
            labels = new List<Label>();
            buttons = new List<CalendarButton>();

            var columDef = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            var rowDef = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            DayLabels = new Grid { VerticalOptions = LayoutOptions.Start, RowSpacing = 0, ColumnSpacing = 0, Padding = 0 };
            DayLabels.ColumnDefinitions = new ColumnDefinitionCollection { columDef, columDef, columDef, columDef, columDef, columDef, columDef };
            MainCalendar = new Grid { VerticalOptions = LayoutOptions.Start, RowSpacing = 0, ColumnSpacing = 0, Padding = 1, BackgroundColor = BorderColor };
            MainCalendar.ColumnDefinitions = new ColumnDefinitionCollection { columDef, columDef, columDef, columDef, columDef, columDef, columDef };
            MainCalendar.RowDefinitions = new RowDefinitionCollection { rowDef, rowDef, rowDef, rowDef, rowDef, rowDef };
        }

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
            MainView.Children.Add(DayLabels);
            MainView.Children.Add(MainCalendar);
            base.OnParentSet();
        }

        protected void FillCalendarWindows()
        {
            for (int r = 0; r < 6; r++)
            {
                for (int c = 0; c < 7; c++)
                {
                    if (r == 0)
                    {
                        labels.Add(new Label {
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            TextColor = Color.Black,
                            FontSize = 18,
                            FontAttributes = FontAttributes.Bold });
                        DayLabels.Children.Add(labels.Last(), c, r);
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
            }

            ChangeCalendar(CalandarChanges.All);
        }

        protected Task FillCalendar()
        {
            return Task.Factory.StartNew(() =>
            {
                FillCalendarWindows();
            });
        }

        public static readonly BindableProperty SelectedDateProperty =
            BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(Calendar), null,
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedDate((DateTime?)newValue));
        /// <summary>
        /// Gets or sets a date the selected date
        /// </summary>
        /// <value>The selected date.</value>
        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

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

        public static readonly BindableProperty DatesTextColorOutsideMonthProperty =
            BindableProperty.Create(nameof(DatesTextColorOutsideMonth), typeof(Color), typeof(Calendar), Color.FromHex("#aaaaaa"),
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDatesTextColorOutsideMonth((Color)newValue, (Color)oldValue));

        protected void ChangeDatesTextColorOutsideMonth(Color newValue, Color oldValue)
        {
            if (newValue == oldValue) return;
            buttons.FindAll(b => b.IsEnabled && !b.IsSelected && b.IsOutOfMonth).ForEach(b => b.TextColor = newValue);
        }

        /// <summary>
        /// Gets or sets the text color of the dates not in the focused month.
        /// </summary>
        /// <value>The dates text color outside month.</value>
        public Color DatesTextColorOutsideMonth
        {
            get { return (Color)GetValue(DatesTextColorOutsideMonthProperty); }
            set { SetValue(DatesTextColorOutsideMonthProperty, value); }
        }

        public static readonly BindableProperty DatesBackgroundColorOutsideMonthProperty =
            BindableProperty.Create(nameof(DatesBackgroundColorOutsideMonth), typeof(Color), typeof(Calendar), Color.White,
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDatesBackgroundColorOutsideMonth((Color)newValue, (Color)oldValue));

        protected void ChangeDatesBackgroundColorOutsideMonth(Color newValue, Color oldValue)
        {
            if (newValue == oldValue) return;
            buttons.FindAll(b => b.IsEnabled && !b.IsSelected && b.IsOutOfMonth).ForEach(b => b.BackgroundColor = newValue);
        }

        /// <summary>
        /// Gets or sets the background color of the dates not in the focused month.
        /// </summary>
        /// <value>The dates background color outside month.</value>
        public Color DatesBackgroundColorOutsideMonth
        {
            get { return (Color)GetValue(DatesBackgroundColorOutsideMonthProperty); }
            set { SetValue(DatesBackgroundColorOutsideMonthProperty, value); }
        }

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

        public static readonly BindableProperty WeekdaysTextColorProperty =
            BindableProperty.Create(nameof(WeekdaysTextColor), typeof(Color), typeof(Calendar), Color.FromHex("#aaaaaa"),
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeWeekdaysTextColor((Color)newValue, (Color)oldValue));

        protected void ChangeWeekdaysTextColor(Color newValue, Color oldValue)
        {
            if (newValue == oldValue) return;
            labels.ForEach(l => l.TextColor = newValue);
        }

        /// <summary>
        /// Gets or sets the text color of the weekdays labels.
        /// </summary>
        /// <value>The color of the weekdays text.</value>
        public Color WeekdaysTextColor
        {
            get { return (Color)GetValue(WeekdaysTextColorProperty); }
            set { SetValue(WeekdaysTextColorProperty, value); }
        }

        public static readonly BindableProperty WeekdaysBackgroundColorProperty =
            BindableProperty.Create(nameof(WeekdaysBackgroundColor), typeof(Color), typeof(Calendar), Color.Transparent,
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeWeekdaysBackgroundColor((Color)newValue, (Color)oldValue));

        protected void ChangeWeekdaysBackgroundColor(Color newValue, Color oldValue)
        {
            if (newValue == oldValue) return;
            labels.ForEach(l => l.BackgroundColor = newValue);
        }

        /// <summary>
        /// Gets or sets the background color of the weekdays labels.
        /// </summary>
        /// <value>The color of the weekdays background.</value>
        public Color WeekdaysBackgroundColor
        {
            get { return (Color)GetValue(WeekdaysBackgroundColorProperty); }
            set { SetValue(WeekdaysBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty WeekdaysFontSizeProperty =
            BindableProperty.Create(nameof(WeekdaysFontSize), typeof(double), typeof(Calendar), 16.0,
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeWeekdaysFontSize((double)newValue, (double)oldValue));

        protected void ChangeWeekdaysFontSize(double newValue, double oldValue)
        {
            if (Math.Abs(newValue - oldValue) < 0.01) return;
            labels.ForEach(l => l.FontSize = newValue);
        }

        /// <summary>
        /// Gets or sets the font size of the weekday labels.
        /// </summary>
        /// <value>The size of the weekdays font.</value>
        public double WeekdaysFontSize
        {
            get { return (double)GetValue(WeekdaysFontSizeProperty); }
            set { SetValue(WeekdaysFontSizeProperty, value); }
        }

        public static readonly BindableProperty WeekdaysFormatProperty =
            BindableProperty.Create(nameof(WeekdaysFormat), typeof(string), typeof(Calendar), "ddd",
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeWeekdays());

        /// <summary>
        /// Gets or sets the date format of the weekday labels.
        /// </summary>
        /// <value>The weekdays format.</value>
        public string WeekdaysFormat
        {
            get { return (string)GetValue(WeekdaysFormatProperty); }
            set { SetValue(WeekdaysFormatProperty, value); }
        }

        public static readonly BindableProperty WeekdaysShowProperty =
            BindableProperty.Create(nameof(WeekdaysShow), typeof(bool), typeof(Calendar), true,
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).DayLabels.IsVisible = (bool)newValue);

        /// <summary>
        /// Gets or sets wether to show the weekday labels.
        /// </summary>
        /// <value>The weekdays show.</value>
        public bool WeekdaysShow
        {
            get { return (bool)GetValue(WeekdaysShowProperty); }
            set { SetValue(WeekdaysShowProperty, value); }
        }

        public static readonly BindableProperty MonthNavigationShowProperty =
            BindableProperty.Create(nameof(MonthNavigationShow), typeof(bool), typeof(Calendar), true,
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).MonthNavigation.IsVisible = (bool)newValue);

        /// <summary>
        /// Gets or sets wether to show the month navigation.
        /// </summary>
        /// <value>The month navigation show.</value>
        public bool MonthNavigationShow
        {
            get { return (bool)GetValue(MonthNavigationShowProperty); }
            set { SetValue(MonthNavigationShowProperty, value); }
        }

        public static readonly BindableProperty TitleLabelFormatProperty =
            BindableProperty.Create(nameof(TitleLabelFormat), typeof(string), typeof(Calendar), "MMM yyyy",
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).CenterLabel.Text = ((bindable as Calendar).StartDate).ToString((string)newValue));

        /// <summary>
        /// Gets or sets the format of the title in the month navigation.
        /// </summary>
        /// <value>The title label format.</value>
        public string TitleLabelFormat
        {
            get { return (string)GetValue(TitleLabelFormatProperty); }
            set { SetValue(TitleLabelFormatProperty, value); }
        }

        /// <summary>
        /// Gets the title label in the month navigation.
        /// </summary>
        public Label TitleLabel { get; protected set; }

        /// <summary>
        /// Gets the left button of the month navigation.
        /// </summary>
        public CalendarButton TitleLeftArrow { get; protected set; }

        /// <summary>
        /// Gets the right button of the month navigation.
        /// </summary>
        public CalendarButton TitleRightArrow { get; protected set; }
		
        /// <summary>
        /// Gets the right button of the month navigation.
        /// </summary>
        public StackLayout MonthNavigationLayout { get; protected set; }
		
        public static readonly BindableProperty SelectedBorderWidthProperty =
            BindableProperty.Create(nameof(SelectedBorderWidth), typeof(int), typeof(Calendar), Device.OS == TargetPlatform.iOS ? 3 : 5,
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedBorderWidth((int)newValue, (int)oldValue));

        protected void ChangeSelectedBorderWidth(int newValue, int oldValue)
        {
            if (newValue == oldValue) return;
            buttons.FindAll(b => b.IsSelected).ForEach(b => b.BorderWidth = newValue);
        }

        /// <summary>
        /// Gets or sets the border width of the selected date.
        /// </summary>
        /// <value>The width of the selected border.</value>
        public int SelectedBorderWidth
        {
            get { return (int)GetValue(SelectedBorderWidthProperty); }
            set { SetValue(SelectedBorderWidthProperty, value); }
        }

        public static readonly BindableProperty SelectedBorderColorProperty =
            BindableProperty.Create(nameof(SelectedBorderColor), typeof(Color), typeof(Calendar), Color.FromHex("#c82727"),
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedBorderColor((Color)newValue, (Color)oldValue));

        protected void ChangeSelectedBorderColor(Color newValue, Color oldValue)
        {
            if (newValue == oldValue) return;
            buttons.FindAll(b => b.IsSelected).ForEach(b => b.BorderColor = newValue);
        }

        /// <summary>
        /// Gets or sets the color of the selected date.
        /// </summary>
        /// <value>The color of the selected border.</value>
        public Color SelectedBorderColor
        {
            get { return (Color)GetValue(SelectedBorderColorProperty); }
            set { SetValue(SelectedBorderColorProperty, value); }
        }

        public static readonly BindableProperty SelectedBackgroundColorProperty =
            BindableProperty.Create(nameof(SelectedBackgroundColor), typeof(Color?), typeof(Calendar), null,
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedBackgroundColor((Color?)newValue, (Color?)oldValue));

        protected void ChangeSelectedBackgroundColor(Color? newValue, Color? oldValue)
        {
            if (newValue == oldValue) return;
            if (newValue.HasValue) buttons.FindAll(b => b.IsSelected).ForEach(b => b.BackgroundColor = newValue.Value);
        }

        /// <summary>
        /// Gets or sets the background color of the selected date.
        /// </summary>
        /// <value>The color of the selected background.</value>
        public Color? SelectedBackgroundColor
        {
            get { return (Color?)GetValue(SelectedBackgroundColorProperty); }
            set { SetValue(SelectedBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty SelectedTextColorProperty =
            BindableProperty.Create(nameof(SelectedTextColor), typeof(Color?), typeof(Calendar), null,
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedTextColor((Color?)newValue, (Color?)oldValue));

        protected void ChangeSelectedTextColor(Color? newValue, Color? oldValue)
        {
            if (newValue == oldValue) return;
            if (newValue.HasValue) buttons.FindAll(b => b.IsSelected).ForEach(b => b.TextColor = newValue.Value);
        }

        /// <summary>
        /// Gets or sets the text color of the selected date.
        /// </summary>
        /// <value>The color of the selected text.</value>
        public Color? SelectedTextColor
        {
            get { return (Color?)GetValue(SelectedTextColorProperty); }
            set { SetValue(SelectedTextColorProperty, value); }
        }

        public static readonly BindableProperty SelectedFontSizeProperty =
            BindableProperty.Create(nameof(SelectedFontSize), typeof(double), typeof(Calendar), 20.0,
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedFontSize((double)newValue, (double)oldValue));

        protected void ChangeSelectedFontSize(double newValue, double oldValue)
        {
            if (Math.Abs(newValue - oldValue) < 0.01) return;
            buttons.FindAll(b => b.IsSelected).ForEach(b => b.FontSize = newValue);
        }

        /// <summary>
        /// Gets or sets the font size of the selected date.
        /// </summary>
        /// <value>The size of the selected font.</value>
        public double SelectedFontSize
        {
            get { return (double)GetValue(SelectedFontSizeProperty); }
            set { SetValue(SelectedFontSizeProperty, value); }
        }

        public static readonly BindableProperty DisabledBorderWidthProperty =
            BindableProperty.Create(nameof(DisabledBorderWidth), typeof(int), typeof(Calendar), Device.OS == TargetPlatform.iOS ? 1 : 3,
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDisabledBorderWidth((int)newValue, (int)oldValue));

        protected void ChangeDisabledBorderWidth(int newValue, int oldValue)
        {
            if (newValue == oldValue) return;
            buttons.FindAll(b => !b.IsEnabled).ForEach(b => b.BorderWidth = newValue);
        }

        /// <summary>
        /// Gets or sets the border width of the disabled dates.
        /// </summary>
        /// <value>The width of the disabled border.</value>
        public int DisabledBorderWidth
        {
            get { return (int)GetValue(DisabledBorderWidthProperty); }
            set { SetValue(DisabledBorderWidthProperty, value); }
        }

        public static readonly BindableProperty DisabledBorderColorProperty =
            BindableProperty.Create(nameof(DisabledBorderColor), typeof(Color), typeof(Calendar), Color.FromHex("#cccccc"),
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDisabledBorderColor((Color)newValue, (Color)oldValue));

        protected void ChangeDisabledBorderColor(Color newValue, Color oldValue)
        {
            if (newValue == oldValue) return;
            buttons.FindAll(b => !b.IsEnabled).ForEach(b => b.BorderColor = newValue);
        }

        /// <summary>
        /// Gets or sets the border color of the disabled dates.
        /// </summary>
        /// <value>The color of the disabled border.</value>
        public Color DisabledBorderColor
        {
            get { return (Color)GetValue(DisabledBorderColorProperty); }
            set { SetValue(DisabledBorderColorProperty, value); }
        }

        public static readonly BindableProperty DisabledBackgroundColorProperty =
            BindableProperty.Create(nameof(DisabledBackgroundColor), typeof(Color), typeof(Calendar), Color.Gray,
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDisabledBackgroundColor((Color)newValue, (Color)oldValue));

        protected void ChangeDisabledBackgroundColor(Color newValue, Color oldValue)
        {
            if (newValue == oldValue) return;
            buttons.FindAll(b => !b.IsEnabled).ForEach(b => b.BackgroundColor = newValue);
        }

        /// <summary>
        /// Gets or sets the background color of the disabled dates.
        /// </summary>
        /// <value>The color of the disabled background.</value>
        public Color DisabledBackgroundColor
        {
            get { return (Color)GetValue(DisabledBackgroundColorProperty); }
            set { SetValue(DisabledBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty DisabledTextColorProperty =
            BindableProperty.Create(nameof(DisabledTextColor), typeof(Color), typeof(Calendar), Color.FromHex("#dddddd"),
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDisabledTextColor((Color)newValue, (Color)oldValue));

        protected void ChangeDisabledTextColor(Color newValue, Color oldValue)
        {
            if (newValue == oldValue) return;
            buttons.FindAll(b => !b.IsEnabled).ForEach(b => b.TextColor = newValue);
        }

        /// <summary>
        /// Gets or sets the text color of the disabled dates.
        /// </summary>
        /// <value>The color of the disabled text.</value>
        public Color DisabledTextColor
        {
            get { return (Color)GetValue(DisabledTextColorProperty); }
            set { SetValue(DisabledTextColorProperty, value); }
        }

        public static readonly BindableProperty DisabledFontSizeProperty =
            BindableProperty.Create(nameof(DisabledFontSize), typeof(double), typeof(Calendar), 20.0,
                                    propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDisabledFontSize((double)newValue, (double)oldValue));

        protected void ChangeDisabledFontSize(double newValue, double oldValue)
        {
            if (Math.Abs(newValue - oldValue) < 0.01) return;
            buttons.FindAll(b => !b.IsEnabled).ForEach(b => b.FontSize = newValue);
        }

        /// <summary>
        /// Gets or sets the font size of the disabled dates.
        /// </summary>
        /// <value>The size of the disabled font.</value>
        public double DisabledFontSize
        {
            get { return (double)GetValue(DisabledFontSizeProperty); }
            set { SetValue(DisabledFontSizeProperty, value); }
        }

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

        public static readonly BindableProperty RightArrowCommandProperty =
            BindableProperty.Create(nameof(RightArrowCommand), typeof(ICommand), typeof(Calendar), null);

        public ICommand RightArrowCommand
        {
            get { return (ICommand)GetValue(RightArrowCommandProperty); }
            set { SetValue(RightArrowCommandProperty, value); }
        }

        public static readonly BindableProperty LeftArrowCommandProperty =
            BindableProperty.Create(nameof(LeftArrowCommand), typeof(ICommand), typeof(Calendar), null);

        public ICommand LeftArrowCommand
        {
            get { return (ICommand)GetValue(LeftArrowCommandProperty); }
            set { SetValue(LeftArrowCommandProperty, value); }
        }

		public static readonly BindableProperty SpecialDatesProperty = 
			BindableProperty.Create(nameof(SpecialDates), typeof(List<SpecialDate>), typeof(Calendar), null,
			                        propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeCalendar(CalandarChanges.MaxMin));

		public List<SpecialDate> SpecialDates
		{
			get { return (List<SpecialDate>)GetValue(SpecialDatesProperty); }
			set { SetValue(SpecialDatesProperty, value); }
		}

		public void RaiseSpecialDatesChanged()
		{
			OnPropertyChanged(nameof(SpecialDates));
		}

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

        protected void ChangeWeekdays()
        {
            if (!WeekdaysShow) return;
            var start = CalendarStartDate;
            for (int i = 0; i < labels.Count; i++)
            {
                labels[i].Text = start.ToString(WeekdaysFormat);
                start = start.AddDays(1);
            }
        }

        protected void ChangeCalendar(CalandarChanges changes)
        {
            if (changes.HasFlag(CalandarChanges.StartDate))
            {
                Device.BeginInvokeOnMainThread(() => CenterLabel.Text = StartDate.ToString(TitleLabelFormat));
            }

            var start = CalendarStartDate;
            var beginOfMonth = false;
            var endOfMonth = false;
            for (int i = 0; i < buttons.Count; i++)
            {
                endOfMonth |= beginOfMonth && start.Day == 1;
                beginOfMonth |= start.Day == 1;

				if (i < 7 && WeekdaysShow && changes.HasFlag(CalandarChanges.StartDay))
                {
                    labels[i].Text = start.ToString(WeekdaysFormat);
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

                var isInsideMonth = beginOfMonth && !endOfMonth;
				SpecialDate sd = null;
				if (SpecialDates != null)
				{
					sd = SpecialDates.FirstOrDefault(s => s.Date.Date == start.Date);
				}

				if ((MinDate.HasValue && start < MinDate) || (MaxDate.HasValue && start > MaxDate))
                {
                    SetButtonDisabled(buttons[i]);
                }
                else if (SelectedDate.HasValue && start.Date == SelectedDate.Value.Date)
                {
                    SetButtonSelected(buttons[i], isInsideMonth);
                }
				else if (sd != null)
				{
					SetButtonSpecial(buttons[i], sd);
				}
				else
                {
                    SetButtonNormal(buttons[i], isInsideMonth);
                }
                start = start.AddDays(1);
            }
        }

        protected void SetButtonNormal(CalendarButton button, bool isInsideMonth)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                button.IsEnabled = true;
                button.IsSelected = false;
                button.IsOutOfMonth = !isInsideMonth;
                button.FontSize = DatesFontSize;
                button.BorderWidth = BorderWidth;
                button.BorderColor = BorderColor;
                button.BackgroundColor = isInsideMonth ? DatesBackgroundColor : DatesBackgroundColorOutsideMonth;
                button.TextColor = isInsideMonth ? DatesTextColor : DatesTextColorOutsideMonth;
            });
        }

        protected void SetButtonSelected(CalendarButton button, bool isInsideMonth)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                button.IsEnabled = true;
                button.IsSelected = true;
                button.FontSize = SelectedFontSize;
                button.BorderWidth = SelectedBorderWidth;
                button.BorderColor = SelectedBorderColor;
                button.BackgroundColor = SelectedBackgroundColor.HasValue ? SelectedBackgroundColor.Value : DatesBackgroundColor;
                button.TextColor = SelectedTextColor.HasValue ? SelectedTextColor.Value : (isInsideMonth ? DatesTextColor : DatesTextColorOutsideMonth);
            });
        }

        protected void SetButtonDisabled(CalendarButton button)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                button.FontSize = DisabledFontSize;
                button.BorderWidth = DisabledBorderWidth;
                button.BorderColor = DisabledBorderColor;
                button.BackgroundColor = DisabledBackgroundColor;
                button.TextColor = DisabledTextColor;
                button.IsEnabled = false;
                button.IsSelected = false;
                button.IsOutOfMonth = false;
            });
        }

		protected void SetButtonSpecial(CalendarButton button, SpecialDate special)
		{
            Device.BeginInvokeOnMainThread(() =>
            {
                if (special.FontSize.HasValue) button.FontSize = special.FontSize.Value;
			    if (special.BorderWidth.HasValue) button.BorderWidth = special.BorderWidth.Value;
			    if (special.BorderColor.HasValue) button.BorderColor = special.BorderColor.Value;
			    if (special.BackgroundColor.HasValue) button.BackgroundColor = special.BackgroundColor.Value;
			    if (special.TextColor.HasValue) button.TextColor = special.TextColor.Value;
			    button.IsEnabled = special.Selectable;
            });
        }

        protected void DateClickedEvent(object s, EventArgs a)
        {
            SelectedDate = (s as CalendarButton).Date;
        }

        protected void ChangeSelectedDate(DateTime? date)
        {
            if (!date.HasValue) return;
            var button = buttons.Find(b => b.Date.HasValue && b.Date.Value.Date == date.Value.Date);
            if (button == null) return;
			buttons.FindAll(b => b.IsSelected).ForEach(b => {
				var spD = SpecialDates?.FirstOrDefault(s => s.Date.Date == b.Date.Value.Date);
				SetButtonNormal(b, !b.IsOutOfMonth);
				if (spD != null)
				{
					SetButtonSpecial(b, spD);
				}
            });
            SetButtonSelected(button, !button.IsOutOfMonth);
            DateClicked?.Invoke(this, new DateTimeEventArgs { DateTime = SelectedDate.Value });
            DateCommand?.Execute(SelectedDate.Value);
        }

        protected void LeftArrowClickedEvent(object s, EventArgs a)
        {
            StartDate = new DateTime(StartDate.Year, StartDate.Month, 1).AddMonths(-1);
            LeftArrowClicked?.Invoke(s, new DateTimeEventArgs { DateTime = StartDate });
            LeftArrowCommand?.Execute(StartDate);
        }

        protected void RightArrowClickedEvent(object s, EventArgs a)
        {
            StartDate = new DateTime(StartDate.Year, StartDate.Month, 1).AddMonths(1);
            RightArrowClicked?.Invoke(s, new DateTimeEventArgs { DateTime = StartDate });
            RightArrowCommand?.Execute(StartDate);
        }

        public event EventHandler<DateTimeEventArgs> RightArrowClicked, LeftArrowClicked, DateClicked;
    }

    public static class EnumerableExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T item in enumeration)
			{
				action(item);
			}
		}
	}
}

