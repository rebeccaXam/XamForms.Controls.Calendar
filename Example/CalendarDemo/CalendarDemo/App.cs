using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XamForms.Controls;

namespace CalendarDemo
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            MainPage = new ContentPage
            {
				BackgroundColor= Color.White,
                Content = new StackLayout
                {
					Padding = new Thickness(5,Device.OS == TargetPlatform.iOS ? 25 : 5,5,5),
                    Children = {
						new Calendar { 
							//MaxDate=DateTime.Now.AddDays(30), 
							MinDate=DateTime.Now.AddDays(-1), 
							SpecialDates = new List<SpecialDate>{ 
								new SpecialDate(DateTime.Now.AddDays(2)) { BackgroundColor = Color.Red, TextColor = Color.Accent, BorderColor = Color.Maroon, BorderWidth=8 },
								new SpecialDate(DateTime.Now.AddDays(3)) { BackgroundColor = Color.Green, TextColor = Color.Blue, Selectable = true }
							} 
						}
                    }
                }
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
