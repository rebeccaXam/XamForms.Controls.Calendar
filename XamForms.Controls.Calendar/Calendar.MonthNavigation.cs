using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamForms.Controls
{
	public partial class Calendar : ContentView
	{

		/// <summary>
		/// Gets the title label in the month navigation.
		/// </summary>
		public Label TitleLabel { get; protected set; }
		
		#region TitleLeftArrow

		/// <summary>
		/// Gets the left button of the month navigation.
		/// </summary>
		public CalendarButton TitleLeftArrow { get; protected set; }
		
		public static readonly BindableProperty TitleLeftArrowColorProperty =
			BindableProperty.Create(nameof(TitleLeftArrowColor), typeof(Color), typeof(Calendar), true,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).TitleLeftArrow.TextColor = (Color)newValue);

		/// <summary>
		/// Gets or sets the color of the left arrow.
		/// </summary>
		/// <value>The month navigation show.</value>
		public bool TitleLeftArrowColor
		{
			get { return (Color)GetValue(TitleLeftArrowColorProperty); }
			set { SetValue(TitleLeftArrowColorProperty, value); }
		}
		
		#endregion
		
		#region TitleRightArrow

		/// <summary>
		/// Gets the right button of the month navigation.
		/// </summary>
		public CalendarButton TitleRightArrow { get; protected set; }
		
		public static readonly BindableProperty TitleRightArrowColorProperty =
			BindableProperty.Create(nameof(TitleRightArrowColor), typeof(Color), typeof(Calendar), true,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).TitleRightArrow.TextColor = (Color)newValue);

		/// <summary>
		/// Gets or sets the color of the right arrow.
		/// </summary>
		/// <value>The month navigation show.</value>
		public bool TitleRightArrowColor
		{
			get { return (Color)GetValue(TitleRightArrowColorProperty); }
			set { SetValue(TitleRightArrowColorProperty, value); }
		}
		
		#endregion

		/// <summary>
		/// Gets the right button of the month navigation.
		/// </summary>
		public StackLayout MonthNavigationLayout { get; protected set; }

		#region MonthNavigationShow

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

		#endregion

		#region TitleLabelFormat

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

		#endregion

		#region EnableTitleMonthYearDetails

		public static readonly BindableProperty EnableTitleMonthYearDetailsProperty =
		BindableProperty.Create(nameof(EnableTitleMonthYearView), typeof(bool), typeof(Calendar), false,
			propertyChanged: (bindable, oldValue, newValue) =>
			{
				(bindable as Calendar).TitleLabel.GestureRecognizers.Clear();
				if (!(bool)newValue) return;
				var gr = new TapGestureRecognizer();
				gr.Tapped += (sender, e) => (bindable as Calendar).NextMonthYearView();
				(bindable as Calendar).TitleLabel.GestureRecognizers.Add(gr);
			});

		/// <summary>
		/// Gets or sets wether on Title pressed the month, year or normal view is showen
		/// </summary>
		/// <value>The weekdays show.</value>
		public bool EnableTitleMonthYearView
		{
			get { return (bool)GetValue(EnableTitleMonthYearDetailsProperty); }
			set { SetValue(EnableTitleMonthYearDetailsProperty, value); }
		}

		#endregion

		public event EventHandler<DateTimeEventArgs> RightArrowClicked, LeftArrowClicked;

		#region RightArrowCommand

		public static readonly BindableProperty RightArrowCommandProperty =
			BindableProperty.Create(nameof(RightArrowCommand), typeof(ICommand), typeof(Calendar), null);

		public ICommand RightArrowCommand
		{
			get { return (ICommand)GetValue(RightArrowCommandProperty); }
			set { SetValue(RightArrowCommandProperty, value); }
		}

		protected void RightArrowClickedEvent(object s, EventArgs a)
		{
			if (CalendarViewType == DateTypeEnum.Year)
			{
				NextPrevYears(true);
			}
			else 
			{
				NextMonth();
			}
			RightArrowClicked?.Invoke(s, new DateTimeEventArgs { DateTime = StartDate });
			RightArrowCommand?.Execute(StartDate);
		}
		
		public void NextMonth() 
		{
			StartDate = new DateTime(StartDate.Year, StartDate.Month, 1).AddMonths(1);
		}

		#endregion

		#region LeftArrowCommand

		public static readonly BindableProperty LeftArrowCommandProperty =
			BindableProperty.Create(nameof(LeftArrowCommand), typeof(ICommand), typeof(Calendar), null);

		public ICommand LeftArrowCommand
		{
			get { return (ICommand)GetValue(LeftArrowCommandProperty); }
			set { SetValue(LeftArrowCommandProperty, value); }
		}

		protected void LeftArrowClickedEvent(object s, EventArgs a)
		{
			if (CalendarViewType == DateTypeEnum.Year)
			{
				NextPrevYears(false);
			}
			else
			{
				PreviousMonth();
			}
			LeftArrowClicked?.Invoke(s, new DateTimeEventArgs { DateTime = StartDate });
			LeftArrowCommand?.Execute(StartDate);
		}
		
		public void PreviousMonth()
		{
		    StartDate = new DateTime(StartDate.Year, StartDate.Month, 1).AddMonths(-1);
		}
		#endregion
	}
}
