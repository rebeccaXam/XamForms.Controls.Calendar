using Windows.UI;
using Windows.UI.Xaml.Media;
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
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var element = Element as CalendarButton;
            if (e.PropertyName == nameof(element.TextWithoutMeasure) || e.PropertyName == "Renderer")
            {
                Control.Content = element.TextWithoutMeasure;
                Control.Padding = new Thickness(0);
                Control.Width = Math.Min(Control.ActualWidth, Control.ActualHeight);
                Control.Height = Math.Min(Control.ActualWidth, Control.ActualHeight);
            }

            if (e.PropertyName == nameof(element.TextColor) || e.PropertyName == "Renderer")
            {
                Control.Foreground = new SolidColorBrush(
                    Color.FromArgb((byte)(element.TextColor.A * 255),
                    (byte)(element.TextColor.R * 255),
                    (byte)(element.TextColor.G * 255),
                    (byte)(element.TextColor.B * 255)));
            }
            
            if (e.PropertyName == nameof(element.BackgroundColor) || e.PropertyName == "Renderer")
            {
                Control.Background = new SolidColorBrush(
                    Color.FromArgb((byte)(element.BackgroundColor.A * 255),
                    (byte)(element.BackgroundColor.R * 255),
                    (byte)(element.BackgroundColor.G * 255),
                    (byte)(element.BackgroundColor.B * 255)));
            }
        }
    }

	public static class Calendar
	{
		public static void Init()
		{
			var d = "";
		}
	}
}
