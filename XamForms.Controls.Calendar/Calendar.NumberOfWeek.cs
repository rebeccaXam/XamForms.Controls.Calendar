using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace XamForms.Controls
{
	public partial class Calendar : ContentView
	{
		List<Grid> WeekNumbers;
		List<Label> weekNumberLabels;

		#region NumberOfWeekTextColor

		public static readonly BindableProperty NumberOfWeekTextColorProperty =
		  BindableProperty.Create(nameof(NumberOfWeekTextColor), typeof(Color), typeof(Calendar), Color.FromHex("#aaaaaa"),
								  propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeNumberOfWeekTextColor((Color)newValue, (Color)oldValue));

		protected void ChangeNumberOfWeekTextColor(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			weekNumberLabels.ForEach(l => l.TextColor = newValue);
		}

		/// <summary>
		/// Gets or sets the text color of the number of the week labels.
		/// </summary>
		/// <value>The color of the weekdays text.</value>
		public Color NumberOfWeekTextColor
		{
			get { return (Color)GetValue(NumberOfWeekTextColorProperty); }
			set { SetValue(NumberOfWeekTextColorProperty, value); }
		}

		public static readonly BindableProperty NumberOfWeekBackgroundColorProperty =
			BindableProperty.Create(nameof(NumberOfWeekBackgroundColor), typeof(Color), typeof(Calendar), Color.Transparent,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeNumberOfWeekBackgroundColor((Color)newValue, (Color)oldValue));

		protected void ChangeNumberOfWeekBackgroundColor(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			weekNumberLabels.ForEach(l => l.BackgroundColor = newValue);
		}

		/// <summary>
		/// Gets or sets the background color of the number of the week labels.
		/// </summary>
		/// <value>The color of the number of the weeks background.</value>
		public Color NumberOfWeekBackgroundColor
		{
			get { return (Color)GetValue(NumberOfWeekBackgroundColorProperty); }
			set { SetValue(NumberOfWeekBackgroundColorProperty, value); }
		}

		#endregion

		#region NumberOfWeekFontSize

		public static readonly BindableProperty NumberOfWeekFontSizeProperty =
			BindableProperty.Create(nameof(NumberOfWeekFontSize), typeof(double), typeof(Calendar), 14.0,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeNumberOfWeekFontSize((double)newValue, (double)oldValue));

		protected void ChangeNumberOfWeekFontSize(double newValue, double oldValue)
		{
			if (Math.Abs(newValue - oldValue) < 0.01) return;
			WeekNumbers?.ForEach((obj) => obj.WidthRequest = newValue*( Device.OS == TargetPlatform.iOS ? 1.5 : 2.2));
			weekNumberLabels.ForEach(l => l.FontSize = newValue);
		}

		/// <summary>
		/// Gets or sets the font size of the number of the week labels.
		/// </summary>
		/// <value>The size of the weekdays font.</value>
		public double NumberOfWeekFontSize
		{
			get { return (double)GetValue(NumberOfWeekFontSizeProperty); }
			set { SetValue(NumberOfWeekFontSizeProperty, value); }
		}

		#endregion

		#region NumberOfWeekFontAttributes

		public static readonly BindableProperty NumberOfWeekFontAttributesProperty =
			BindableProperty.Create(nameof(NumberOfWeekFontAttributes), typeof(FontAttributes), typeof(Calendar), FontAttributes.None,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeNumberOfWeekFontAttributes((FontAttributes)newValue, (FontAttributes)oldValue));

		protected void ChangeNumberOfWeekFontAttributes(FontAttributes newValue, FontAttributes oldValue)
		{
			if (newValue == oldValue) return;
			weekNumberLabels.ForEach(l => l.FontAttributes = newValue);
		}

		/// <summary>
		/// Gets or sets the font attributes of the number of the week labels.
		/// </summary>
		public FontAttributes NumberOfWeekFontAttributes
		{
			get { return (FontAttributes)GetValue(NumberOfWeekFontAttributesProperty); }
			set { SetValue(NumberOfWeekFontAttributesProperty, value); }
		}

		#endregion

		#region ShowNumberOfWeek

		public static readonly BindableProperty ShowNumberOfWeekProperty =
			BindableProperty.Create(nameof(ShowNumberOfWeek), typeof(bool), typeof(Calendar), false,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ShowHideElements());

		/// <summary>
		/// Gets or sets wether to show the number of the week labels.
		/// </summary>
		/// <value>The weekdays show.</value>
		public bool ShowNumberOfWeek
		{
			get { return (bool)GetValue(ShowNumberOfWeekProperty); }
			set { SetValue(ShowNumberOfWeekProperty, value); }
		}

		#endregion

		protected void ChangeWeekNumbers(DateTime start, int i)
		{
			if (!ShowNumberOfWeek) return;
			CultureInfo ciCurr = CultureInfo.CurrentCulture;
			var weekNum = ciCurr.Calendar.GetWeekOfYear(start, CalendarWeekRule.FirstFourDayWeek, StartDay);
			weekNumberLabels[(i / 7)].Text = string.Format("{0}", weekNum);
		}

		protected void ShowHideElements()
		{
			if (MainCalendars.Count < 1) return;
			Content = null;
			ContentView.Children.Clear();
			for (var i = 0; i < ShowNumOfMonths; i++)
			{
				var main = MainCalendars[i] as Layout;

				if (ShowInBetweenMonthLabels && i > 0)
				{
					var label = new Label
					{
						FontSize = TitleLabel.FontSize,
						VerticalTextAlignment = TitleLabel.VerticalTextAlignment,
						HorizontalTextAlignment = TitleLabel.HorizontalTextAlignment,
						FontAttributes = TitleLabel.FontAttributes,
						TextColor = TitleLabel.TextColor,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Text = ""
					};
					if (TitleLabels == null)
					{
						TitleLabels = new List<Label>(ShowNumOfMonths);
					}
					TitleLabels.Add(label);
					/*if (i == ShowNumOfMonths - 1)
					{
						MonthNavigationLayout.Children.Remove(TitleRightArrow);
						ContentView.Children.Add(new StackLayout { 
							Orientation = StackOrientation.Horizontal, 
							Padding = new Thickness(TitleLeftArrow.FontSize*1.5,0,0,0), Children = { label, TitleRightArrow }  
						});
					}
					else 
					{*/
						ContentView.Children.Add(label);
					//}
				}

				if (ShowNumberOfWeek)
				{
					main = new StackLayout
					{
						Padding = 0,
						Spacing = 0,
						VerticalOptions = LayoutOptions.FillAndExpand,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Orientation = StackOrientation.Horizontal,
						Children = { WeekNumbers[i], MainCalendars[i] }
					};
					if(WeekdaysShow) DayLabels.Padding = new Thickness(NumberOfWeekFontSize + (NumberOfWeekFontSize / 2) + 6, 0, 0, 0);
				}

				if (WeekdaysShow)
				{
					var stack = new StackLayout
					{
						Padding = 0,
						Spacing = 0,
						VerticalOptions = LayoutOptions.FillAndExpand,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Orientation = StackOrientation.Vertical,
						Children = { DayLabels, main }
					};
					ContentView.Children.Add(stack);
				}
				else
				{
					ContentView.Children.Add(main);
				}
			}
			Content = MainView;
		}
	}
}
