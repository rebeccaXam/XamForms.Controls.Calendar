using Android.App;
using Android.Content.PM;
using Android.OS;

namespace CalendarDemo.Droid
{
    [Activity(Label = "CalendarDemo", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
			XamForms.Controls.Droid.Calendar.Init();
			LoadApplication(new App());
        }
    }
}

