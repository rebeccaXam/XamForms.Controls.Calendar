using System;
using System.Globalization;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamForms.Controls
{
	public partial class Calendar : ContentView
	{
		protected Grid details;

		public int YearsRow { get; set; }
		public int YearsColumn { get; set; }

		public DateTypeEnum CalendarViewType { get; protected set; }

		public void PrevMonthYearView()
		{
			switch (CalendarViewType)
			{
				case DateTypeEnum.Normal: ShowYears(); break;
				case DateTypeEnum.Month: ShowNormal(); break;
				case DateTypeEnum.Year: ShowMonths(); break;
				default: ShowNormal(); break;
			}
		}

		public void NextMonthYearView()
		{
			switch (CalendarViewType)
			{
				case DateTypeEnum.Normal: ShowMonths(); break;
				case DateTypeEnum.Month: ShowYears(); break;
				case DateTypeEnum.Year: ShowNormal(); break;
				default: ShowNormal(); break;
			}
		}

		public void ShowNormal()
		{
			if (details != null) MainView.Children.Remove(details);
			if (!MainView.Children.Contains(calendar)) MainView.Children.Add(calendar);
			CalendarViewType = DateTypeEnum.Normal;
			TitleLeftArrow.IsVisible = true;
			TitleRightArrow.IsVisible = true;
		}
		public void ShowMonths()
		{
			if (MainView.Children.Contains(calendar)) MainView.Children.Remove(calendar);
			if (details != null && MainView.Children.Contains(details)) MainView.Children.Remove(details);
			var columDef = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
			var rowDef = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
			details = new Grid { VerticalOptions = LayoutOptions.CenterAndExpand, RowSpacing = 0, ColumnSpacing = 0, Padding = 1, BackgroundColor = BorderColor };
			details.ColumnDefinitions = new ColumnDefinitionCollection { columDef, columDef, columDef };
			details.RowDefinitions = new RowDefinitionCollection { rowDef, rowDef, rowDef, rowDef };
			for (int r = 0; r < 4; r++)
			{
				for (int c = 0; c < 3; c++)
				{
					var b = new CalendarButton
					{
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						VerticalOptions = LayoutOptions.CenterAndExpand,
						Text = DateTimeFormatInfo.CurrentInfo.MonthNames[(r * 3) + c],
						Date = new DateTime(StartDate.Year, (r * 3) + c + 1, 1).Date,
						BackgroundColor = DatesBackgroundColor,
						TextColor = DatesTextColor,
						FontSize = DatesFontSize,
						FontAttributes = DatesFontAttributes,
						WidthRequest = calendar.Width / 3 - BorderWidth,
						HeightRequest = calendar.Height / 4 - BorderWidth
					};

						b.Clicked += (sender, e) =>
						{
							MonthYearButtonCommand?.Execute((sender as CalendarButton).Date.Value);
							MonthYearButtonClicked?.Invoke(sender, new DateTimeEventArgs { DateTime = (sender as CalendarButton).Date.Value });
							if (EnableTitleMonthYearView)
							{
								StartDate = (sender as CalendarButton).Date.Value;
								PrevMonthYearView();
							}
						};

					details.Children.Add(b, c, r);
				}
			}
			details.WidthRequest = calendar.Width;
			details.HeightRequest = calendar.Height;
			MainView.Children.Add(details);
			CalendarViewType = DateTypeEnum.Month;
			TitleLeftArrow.IsVisible = false;
			TitleRightArrow.IsVisible = false;
		}

		public void ShowYears()
		{
			if (MainView.Children.Contains(calendar)) MainView.Children.Remove(calendar);
			if (details != null && MainView.Children.Contains(details)) MainView.Children.Remove(details);
			var columDef = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
			var rowDef = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
			details = new Grid { VerticalOptions = LayoutOptions.CenterAndExpand, RowSpacing = 0, ColumnSpacing = 0, Padding = 1, BackgroundColor = BorderColor };
			details.ColumnDefinitions = new ColumnDefinitionCollection { columDef, columDef, columDef, columDef };
			details.RowDefinitions = new RowDefinitionCollection { rowDef, rowDef, rowDef, rowDef };
			for (int r = 0; r < YearsRow; r++)
			{
				for (int c = 0; c < YearsColumn; c++)
				{
					var t = (r * YearsColumn) + c + 1;
					var b = new CalendarButton
					{
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						VerticalOptions = LayoutOptions.CenterAndExpand,
						Text = string.Format("{0}", StartDate.Year + (t - (YearsColumn * YearsRow / 2))),
						Date = new DateTime(StartDate.Year + (t - (YearsColumn * YearsRow / 2)), StartDate.Month, 1).Date,
						BackgroundColor = DatesBackgroundColor,
						TextColor = DatesTextColor,
						FontSize = DatesFontSize,
						FontAttributes = DatesFontAttributes,
						WidthRequest = (calendar.Width / YearsRow) - BorderWidth,
						HeightRequest = calendar.Height / YearsColumn - BorderWidth
					};
					b.Clicked += (sender, e) =>
					{
						MonthYearButtonCommand?.Execute((sender as CalendarButton).Date.Value);
						MonthYearButtonClicked?.Invoke(sender, new DateTimeEventArgs { DateTime = (sender as CalendarButton).Date.Value});
						if (EnableTitleMonthYearView)
						{
							StartDate = (sender as CalendarButton).Date.Value;
							PrevMonthYearView();
						}
					};
					details.Children.Add(b, c, r);
				}
			}
			details.WidthRequest = calendar.Width;
			details.HeightRequest = calendar.Height;
			MainView.Children.Add(details);
			CalendarViewType = DateTypeEnum.Year;
			TitleLeftArrow.IsVisible = true;
			TitleRightArrow.IsVisible = true;
		}

		protected void NextPrevYears(bool next)
		{
			var n = (YearsRow * YearsColumn) * (next ? 1 : -1);
			foreach (var c in details.Children)
			{
				var b = c as CalendarButton;
				b.TextWithoutMeasure = string.Format("{0}",int.Parse(b.TextWithoutMeasure) + n);
				b.Date = new DateTime(b.Date.Value.Year + n,b.Date.Value.Month, b.Date.Value.Day).Date;
			}
		}

		public event EventHandler<DateTimeEventArgs> MonthYearButtonClicked;

		#region MonthYearButtonCommand

		public static readonly BindableProperty MonthYearButtonCommandProperty =
			BindableProperty.Create(nameof(MonthYearButtonCommand), typeof(ICommand), typeof(Calendar), null);

		/// <summary>
		/// Gets or sets the selected date command.
		/// </summary>
		/// <value>The date command.</value>
		public ICommand MonthYearButtonCommand
		{
			get { return (ICommand)GetValue(MonthYearButtonCommandProperty); }
			set { SetValue(MonthYearButtonCommandProperty, value); }
		}

		#endregion
	}
}
