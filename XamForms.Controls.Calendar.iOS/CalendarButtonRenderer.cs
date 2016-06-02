using XamForms.Controls;
using XamForms.Controls.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;
#if __UNIFIED__
using Foundation;
#else
using MonoTouch.Foundation;
#endif

[assembly: Xamarin.Forms.ExportRenderer(typeof(CalendarButton), typeof(CalendarButtonRenderer))]
namespace XamForms.Controls.iOS
{
	[Preserve(AllMembers = true)]
    public class CalendarButtonRenderer : ButtonRenderer
    {
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var element = Element as CalendarButton;
            if (e.PropertyName == nameof(element.TextWithoutMeasure) || e.PropertyName == "Renderer")
            {
                Control.SetTitle(element.TextWithoutMeasure, UIControlState.Normal);
				Control.SetTitle(element.TextWithoutMeasure, UIControlState.Disabled);
            }
			if (e.PropertyName == nameof(element.TextColor) || e.PropertyName == "Renderer")
			{
				Control.SetTitleColor(element.TextColor.ToUIColor(), UIControlState.Disabled);
				Control.SetTitleColor(element.TextColor.ToUIColor(), UIControlState.Normal);
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

