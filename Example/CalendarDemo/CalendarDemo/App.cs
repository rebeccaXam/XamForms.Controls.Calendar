using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XamForms.Controls;

namespace CalendarDemo
{
    public class App : Application
    {
		Calendar calendar;
        public App()
        {
			calendar = new Calendar
			{
				//MaxDate=DateTime.Now.AddDays(30), 
				MinDate = DateTime.Now.AddDays(-1),
				MultiSelectDates = true,
				DisableAllDates = false,
				ShowNumberOfWeek = true,
				EnableTitleMonthYearDetails = true,
				WeekdaysTextColor = Color.Teal,
				StartDay = DayOfWeek.Monday,
				SelectedTextColor = Color.Fuchsia,
				SpecialDates = new List<SpecialDate>{
					new SpecialDate(DateTime.Now.AddDays(2)) { BackgroundColor = Color.Green, TextColor = Color.Accent, BorderColor = Color.Lime, BorderWidth=8, Selectable = true },
					new SpecialDate(DateTime.Now.AddDays(3)) { BackgroundColor = Color.Green, TextColor = Color.Blue, Selectable = true }
				}
			};
							
			calendar.DateClicked += (sender, e) => {
				System.Diagnostics.Debug.WriteLine( calendar.SelectedDates);
			};
			var vm = new CalendarVM();
			calendar.SetBinding(Calendar.DateCommandProperty, nameof(vm.DateChosen));
			calendar.SetBinding(Calendar.SelectedDateProperty, nameof(vm.Date));
			calendar.BindingContext = vm;
			var calendarXaml = new CalendarXamlView();
			calendarXaml.BindingContext = vm;
            // The root page of your application
            MainPage = new ContentPage
            {
				BackgroundColor= Color.White,
				Content = new ScrollView {
					Content = new StackLayout {
							Padding = new Thickness(5, Device.OS == TargetPlatform.iOS ? 25 : 5, 5, 5),
							Children = {
							calendar,
							calendarXaml
						}
					}
                }
            };
		}

        protected override void OnStart()
        {
			// Handle when your app starts
			calendar.SpecialDates.Add(new SpecialDate(DateTime.Now.AddDays(5)) { BackgroundColor = Color.Fuchsia, TextColor = Color.Accent, BorderColor = Color.Maroon, BorderWidth = 8 });
			calendar.SpecialDates.Add(new SpecialDate(DateTime.Now.AddDays(6)) { BackgroundColor = Color.Fuchsia, TextColor = Color.Accent, BorderColor = Color.Maroon, BorderWidth = 8 });
			calendar.RaiseSpecialDatesChanged();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
