using Android.Graphics.Drawables;
using XamForms.Controls.Droid;
using Xamarin.Forms.Platform.Android;
using XamForms.Controls;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CalendarButton), typeof(CalendarButtonRenderer))]
namespace XamForms.Controls.Droid
{
    public class CalendarButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.TextChanged += (sender, a) =>
            {
                var element = Element as CalendarButton;
                if (Control.Text == element.TextWithoutMeasure) return;
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

            if (e.PropertyName == nameof(Element.BorderWidth) || e.PropertyName == nameof(Element.BorderColor) || e.PropertyName == nameof(Element.BackgroundColor) || e.PropertyName == "Renderer")
            {
                var drawable = new GradientDrawable();
                drawable.SetShape(ShapeType.Rectangle);
                drawable.SetStroke((int)Element.BorderWidth, Element.BorderColor.ToAndroid());
                drawable.SetColor(Element.BackgroundColor.ToAndroid());
                Control.SetBackground(drawable);
                Control.SetPadding(0, 0, 0, 0);
            }
        }
    }
}

