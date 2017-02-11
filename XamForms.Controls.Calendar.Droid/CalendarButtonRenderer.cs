using Android.Graphics.Drawables;
using XamForms.Controls.Droid;
using Xamarin.Forms.Platform.Android;
using XamForms.Controls;
using Android.Runtime;
using System;
using System.Collections.Generic;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CalendarButton), typeof(CalendarButtonRenderer))]
namespace XamForms.Controls.Droid
{
	[Preserve(AllMembers = true)]
    public class CalendarButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.TextChanged += (sender, a) =>
            {
                var element = Element as CalendarButton;
				if (Control.Text == element.TextWithoutMeasure || (string.IsNullOrEmpty(Control.Text) && string.IsNullOrEmpty(element.TextWithoutMeasure))) return;
                Control.Text = element.TextWithoutMeasure;
            };
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var element = Element as CalendarButton;
            if (e.PropertyName == nameof(element.TextWithoutMeasure) || e.PropertyName == "Renderer")
            {
                Control.Text = element.TextWithoutMeasure;
            }

			if (e.PropertyName == nameof(Element.TextColor) || e.PropertyName == "Renderer")
			{
				Control.SetTextColor(Element.TextColor.ToAndroid());
			}
			 
			if (e.PropertyName == nameof(Element.BorderWidth) || e.PropertyName == nameof(Element.BorderColor) || e.PropertyName == nameof(Element.BackgroundColor) || e.PropertyName == "Renderer")
            {
				var drawable = new GradientDrawable();
				drawable.SetShape(ShapeType.Rectangle);
				drawable.SetStroke((int)Element.BorderWidth, Element.BorderColor.ToAndroid());
				drawable.SetColor(Element.BackgroundColor.ToAndroid());
				if (element.BackgroundPattern != null)
				{
					var d = new List<Drawable>();
					for (var i = 0; i < element.BackgroundPattern.Pattern.Count; i++)
					{
						d.Add(new ColorDrawable(element.BackgroundPattern.Pattern[i].Color.ToAndroid()));
					}
					drawable.SetColor(Android.Graphics.Color.Transparent);
					d.Add(drawable);
					var layer = new LayerDrawable(d.ToArray());
					for (var i = 0; i < element.BackgroundPattern.Pattern.Count; i++)
					{
						var l = (int)(Control.MinWidth/2 * element.BackgroundPattern.GetLeft(i));
						var t = (int)(Control.MinHeight * element.BackgroundPattern.GetTop(i));
						var r = (int)(Control.MinWidth / 2 * (1-element.BackgroundPattern.Pattern[i].WidthPercent)) - l;
						var b = (int)(Control.MinHeight * (1 - element.BackgroundPattern.Pattern[i].HightPercent)) - t;
						layer.SetLayerInset(i, l, t, r, b);
					}
					layer.SetLayerInset(d.Count - 1, 0, 0, 0, 0);
					Control.SetBackground(layer);
				}
				else 
				{
					Control.SetBackground(drawable);
				}
				Control.SetPadding(0, 0, 0, 0);
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

