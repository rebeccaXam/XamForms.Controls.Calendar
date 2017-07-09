using XamForms.Controls;
using XamForms.Controls.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
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
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var element = Element as CalendarButton;
            
            if (e.PropertyName == nameof(element.TextWithoutMeasure) || e.PropertyName == "Renderer")
            {
                Control.Content = element.TextWithoutMeasure;
            }
            
            if (Element.BorderWidth > 0 && (e.PropertyName == nameof(element.BorderWidth) || e.PropertyName == "Renderer"))
            {
                Control.BorderThickness = new Thickness(Element.BorderWidth);
            }
        }
    }

	public static class Calendar
	{
        public static void Init()
		{
#if WINDOWS_APP
           XamForms.Controls.Calendar.GridSpace = -5;
#else
           XamForms.Controls.Calendar.GridSpace = 0.1;
#endif
        }
    }
}
