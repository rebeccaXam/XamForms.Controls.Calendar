## Calendar Control Plugin for Xamarin.Forms

A simple Calendar control for your Xamarin.Forms projects

#### Setup
* Available on NuGet: https://www.nuget.org/packages/XamForms.Controls.Calendar/  [![NuGet](https://img.shields.io/nuget/v/XamForms.Controls.Calendar.svg?label=NuGet)](https://www.nuget.org/packages/XamForms.Controls.Calendar/)
* Install into your PCL project and Client projects.

<p align="center">
  <img src="https://raw.githubusercontent.com/rebeccaXam/XamForms.Controls.Calendar/master/iOS.png" height="200"/>
  <img src="https://raw.githubusercontent.com/rebeccaXam/XamForms.Controls.Calendar/master/Android.png" height="200"/>
  <img src="https://raw.githubusercontent.com/rebeccaXam/XamForms.Controls.Calendar/master/WinPhone.png" height="200"/>
  <img src="https://raw.githubusercontent.com/rebeccaXam/XamForms.Controls.Calendar/master/Win8.png" height="200"/>
  <img src="https://raw.githubusercontent.com/rebeccaXam/XamForms.Controls.Calendar/master/UWP.png" height="200"/>
</p>

In your iOS, Android, and Windows projects call:

```
Xamarin.Forms.Init();//platform specific init
XamForms.Controls.<PLATFORM>.Calendar.Init();
```

You must do this AFTER you call Xamarin.Forms.Init();

**Note:** I you are having problems like: [When Changing Months, the days do not update properly](https://github.com/rebeccaXam/XamForms.Controls.Calendar/issues/2) in Windows UWP, try adding this to your UWP project AssemblyInfo.cs:
```
[assembly:Xamarin.Forms.Platform.UWP.ExportRenderer(typeof(XamForms.Controls.CalendarButton), typeof(XamForms.Controls.Windows.CalendarButtonRenderer))]
```

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
<controls:Calendar Padding="10,0,10,0" StartDay="Monday" SelectedBorderWidth="4" DisabledBorderColor="Black">
```


**Bindable Properties**
* bool ```MultiSelectDates``` Gets or sets if multiple dates can be selected (default is false)
* List<DateTime> ```SelectedDates``` Gets the list of selected dates
* List<SpecialDate> ```SpecialDates``` Gets or sets a list of special dates (dates that differ in color, font and selectability)
* DateTime? ```SelectedDate``` Gets or sets a date the selected date
* DateTime? ```MinDate``` Gets or sets the maximum date.
* DateTime? ```MaxDate``` Gets or sets the minimum date.
* DateTime ```StartDate``` Gets or sets a date, to pick the month, the calendar is focused on
* DayOfWeek ```StartDay``` Gets or sets the day the calendar starts the week with.
* int ```BorderWidth``` Gets or sets the border width of the calendar.
* int ```OuterBorderWidth``` Gets or sets the width of the whole calandar border.
* Color ```BorderColor``` Gets or sets the border color of the calendar.
* Color ```DatesBackgroundColor``` Gets or sets the background color of the normal dates.
* Color ```DatesTextColor``` Gets or sets the text color of the normal dates.
* Color ```DatesTextColorOutsideMonth``` Gets or sets the text color of the dates not in the focused month.
* Color ```DatesBackgroundColorOutsideMonth``` Gets or sets the background color of the dates not in the focused month.
* double ```DatesFontSize``` Gets or sets the font size of the normal dates.
* Color ```WeekdaysTextColor``` Gets or sets the text color of the weekdays labels.
* Color ```WeekdaysBackgroundColor``` Gets or sets the background color of the weekdays labels.
* double ```WeekdaysFontSize``` Gets or sets the font size of the weekday labels.
* string ```WeekdaysFormat``` Gets or sets the date format of the weekday labels.
* bool ```WeekdaysShow``` Gets or sets wether to show the weekday labels.
* bool ```MonthNavigationShow``` Gets or sets wether to show the month navigation.
* string ```TitleLabelFormat``` Gets or sets the format of the title in the month navigation.
* Label ```TitleLabel``` Gets the title label in the month navigation.
* CalendarButton ```TitleLeftArrow``` Gets the left button of the month navigation.
* CalendarButton ```TitleRightArrow``` Gets the right button of the month navigation.
* int ```SelectedBorderWidth``` Gets or sets the border width of the selected date.
* Color ```SelectedBorderColor``` Gets or sets the color of the selected date.
* Color? ```SelectedBackgroundColor``` Gets or sets the background color of the selected date.
* Color? ```SelectedTextColor``` Gets or sets the text color of the selected date.
* double ```SelectedFontSize``` Gets or sets the font size of the selected date.
* int ```DisabledBorderWidth``` Gets or sets the border width of the disabled dates.
* Color ```DisabledBorderColor``` Gets or sets the border color of the disabled dates.
* Color ```DisabledBackgroundColor``` Gets or sets the background color of the disabled dates.
* Color ```DisabledTextColor``` Gets or sets the text color of the disabled dates.
* double ```DisabledFontSize``` Gets or sets the font size of the disabled dates.
* ICommand ```DateCommand``` Gets or sets the selected date command.
* ICommand ```RightArrowCommand``` Gets or sets the when the right arrow was pressed command.
* ICommand ```LeftArrowCommand``` Gets or sets the when the left arrow was pressed command.

#### Contributors
* [rebeccaXam](https://github.com/rebeccaXam)

#### License
https://github.com/rebeccaXam/XamForms.Controls.Calendar/blob/master/LICENSE
