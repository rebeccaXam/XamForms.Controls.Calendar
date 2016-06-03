using XamForms.Controls;
using XamForms.Controls.Windows;
using Windows.UI.Xaml;
using System;
#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#else
using Xamarin.Forms.Platform.WinRT;
#endif

[assembly: ExportRenderer(typeof(CalendarButton), typeof(CalendarButtonRenderer))]
namespace XamForms.Controls.Windows
{
	public class CalendarButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.MinWidth = 48;
            Control.MinHeight = 48;
            Control.Style = Application.Current.Resources["CalendarButtonStyle"] as Style;
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var element = Element as CalendarButton;
            if (e.PropertyName == nameof(element.TextWithoutMeasure) || e.PropertyName == "Renderer")
            {
                Control.Content = element.TextWithoutMeasure;
            }
        }
    }

	public static class Calendar
	{
		public static void Init()
		{
            var resourceDictionary = new ResourceDictionary();
#if WINDOWS_UWP
            var url = "ms-appx:///XamForms.Controls.Calendar.UWP/Resources.xaml";
#elif WINDOWS_PHONE_APP
            var url = "ms-appx:///XamForms.Controls.Calendar.WinPhone/Resources.xaml";
#else
            var url = "ms-appx:///XamForms.Controls.Calendar.Windows/Resources.xaml";
#endif 
            resourceDictionary.Source = new Uri(url, UriKind.Absolute);
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }
	}
}
