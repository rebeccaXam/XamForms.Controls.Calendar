## PLEASE NOTE: This repository is deprecated and will no longer be maintained.
## New Repository: https://github.com/lubiepomaranczki/XamForms.Controls.Calendar
## Available on NuGet: https://www.nuget.org/packages/XamForms.Enhanced.Calendar/  [![NuGet](https://img.shields.io/nuget/v/XamForms.Enhanced.Calendar.svg?label=NuGet)](hhttps://www.nuget.org/packages/XamForms.Enhanced.Calendar/)

------

## Calendar Control Plugin for Xamarin.Forms

A simple Calendar control for your Xamarin.Forms projects

#### Setup
* Available on NuGet: https://www.nuget.org/packages/XamForms.Controls.Calendar/  [![NuGet](https://img.shields.io/nuget/v/XamForms.Controls.Calendar.svg?label=NuGet)](https://www.nuget.org/packages/XamForms.Controls.Calendar/)
* Install into your PCL project and Client projects.

<p align="center">
  <img src="https://raw.githubusercontent.com/rebeccaXam/XamForms.Controls.Calendar/master/images/iOS.png" height="200"/>
  <img src="https://raw.githubusercontent.com/rebeccaXam/XamForms.Controls.Calendar/master/images/Android.png" height="200"/>
  <img src="https://raw.githubusercontent.com/rebeccaXam/XamForms.Controls.Calendar/master/images/WinPhone.png" height="200"/>
  <img src="https://raw.githubusercontent.com/rebeccaXam/XamForms.Controls.Calendar/master/images/Win8.png" height="200"/>
  <img src="https://raw.githubusercontent.com/rebeccaXam/XamForms.Controls.Calendar/master/images/UWP.png" height="200"/>
  <img src="https://raw.githubusercontent.com/rebeccaXam/XamForms.Controls.Calendar/master/images/BackgroundPatternDroid.png" height="200"/>
  <img src="https://raw.githubusercontent.com/rebeccaXam/XamForms.Controls.Calendar/master/images/BackgroundpatterniOS.png" height="200"/>
</p>

In your iOS, Android, and Windows projects call:

```
Xamarin.Forms.Init();//platform specific init
XamForms.Controls.<PLATFORM>.Calendar.Init();
```

You must do this AFTER you call Xamarin.Forms.Init();

**IMPORTANT:** I you are having problems like: [When Changing Months, the days do not update properly](https://github.com/rebeccaXam/XamForms.Controls.Calendar/issues/2) in, try adding this to your projects AssemblyInfo.cs:
```
[assembly:Xamarin.Forms.Platform.<Platform>.ExportRenderer(typeof(XamForms.Controls.CalendarButton), typeof(XamForms.Controls.<Platform>.CalendarButtonRenderer))]
```
[Troubleshoot](https://github.com/rebeccaXam/XamForms.Controls.Calendar/wiki#troubleshoot)

#### Usage
Here is a sample:
```
new Calendar
{
  BorderColor = Color.Gay,
  BorderWidth = 3,
  BackgroundColor = Color.Gay,
  StartDay = DayOfWeek.Sunday,
  StartDate = DateTime.Now
}
```

**XAML:**

First add the xmlns namespace:
```xml
xmlns:controls="clr-namespace:XamForms.Controls;assembly=XamForms.Controls.Calendar"
```

Then add the xaml:

```xml
<controls:Calendar Padding="10,0,10,0" StartDay="Monday" SelectedBorderWidth="4" DisabledBorderColor="Black" />
```
#### Documentation: [Wiki](https://github.com/rebeccaXam/XamForms.Controls.Calendar/wiki)

#### Contributors
* [rebeccaXam](https://github.com/rebeccaXam)

#### License
https://github.com/rebeccaXam/XamForms.Controls.Calendar/blob/master/LICENSE
