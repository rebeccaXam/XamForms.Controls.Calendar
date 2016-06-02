using Windows.UI;
using Windows.UI.Xaml.Media;
using XamForms.Controls;
using XamForms.Controls.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        private static bool _runOnce = false;
        protected void RunOne()
        {
            if (_runOnce) return;
            var resourceDictionary = new ResourceDictionary();
            var url = string.Format("ms-appx:///{0}/Resources.xaml", typeof(CalendarButtonRenderer).AssemblyQualifiedName.Split(',')[1].Trim());
            resourceDictionary.Source = new Uri(url, UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            _runOnce = true;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            RunOne();
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
			var d = "";
		}
	}
}
