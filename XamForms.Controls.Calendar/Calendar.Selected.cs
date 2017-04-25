using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace XamForms.Controls
{
	public partial class Calendar : ContentView
	{
		#region SelectedDate

		public static readonly BindableProperty SelectedDateProperty =
			BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(Calendar), null, BindingMode.TwoWay,
				propertyChanged: (bindable, oldValue, newValue) =>
				{
					if ((bindable as Calendar).ChangeSelectedDate(newValue as DateTime?))
					{
						(bindable as Calendar).SelectedDate = null;
					}
				});

		/// <summary>
		/// Gets or sets a date the selected date
		/// </summary>
		/// <value>The selected date.</value>
		public DateTime? SelectedDate
		{
			get { return (DateTime?)GetValue(SelectedDateProperty); }
			set { SetValue(SelectedDateProperty, value.HasValue ? value.Value.Date : value); }
		}

		#endregion

		#region MultiSelectDates

		public static readonly BindableProperty MultiSelectDatesProperty = BindableProperty.Create(nameof(MultiSelectDates), typeof(bool), typeof(Calendar), false);

		/// <summary>
		/// Gets or sets multiple Dates can be selected.
		/// </summary>
		public bool MultiSelectDates
		{
			get { return (bool)GetValue(MultiSelectDatesProperty); }
			set { SetValue(MultiSelectDatesProperty, value); }
		}

		public static readonly BindableProperty SelectedDatesProperty = BindableProperty.Create(nameof(SelectedDates), typeof(List<DateTime>), typeof(Calendar), new List<DateTime>(1));
		/// <summary>
		/// Gets the selected dates when MultiSelectDates is true
		/// </summary>
		/// <value>The selected date.</value>
		public List<DateTime> SelectedDates
		{
			get { return (List<DateTime>)GetValue(SelectedDatesProperty); }
			protected set { SetValue(SelectedDatesProperty, value); }
		}

		#endregion

		#region SelectedBorderWidth

		public static readonly BindableProperty SelectedBorderWidthProperty =
			BindableProperty.Create(nameof(SelectedBorderWidth), typeof(int), typeof(Calendar), Device.OS == TargetPlatform.iOS ? 3 : 5,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedBorderWidth((int)newValue, (int)oldValue));

		protected void ChangeSelectedBorderWidth(int newValue, int oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsSelected).ForEach(b => b.BorderWidth = newValue);
		}

		/// <summary>
		/// Gets or sets the border width of the selected date.
		/// </summary>
		/// <value>The width of the selected border.</value>
		public int SelectedBorderWidth
		{
			get { return (int)GetValue(SelectedBorderWidthProperty); }
			set { SetValue(SelectedBorderWidthProperty, value); }
		}

		#endregion

		#region SelectedBorderColor

		public static readonly BindableProperty SelectedBorderColorProperty =
			BindableProperty.Create(nameof(SelectedBorderColor), typeof(Color), typeof(Calendar), Color.FromHex("#c82727"),
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedBorderColor((Color)newValue, (Color)oldValue));

		protected void ChangeSelectedBorderColor(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsSelected).ForEach(b => b.BorderColor = newValue);
		}

		/// <summary>
		/// Gets or sets the color of the selected date.
		/// </summary>
		/// <value>The color of the selected border.</value>
		public Color SelectedBorderColor
		{
			get { return (Color)GetValue(SelectedBorderColorProperty); }
			set { SetValue(SelectedBorderColorProperty, value); }
		}

		#endregion

		#region SelectedBackgroundColor

		public static readonly BindableProperty SelectedBackgroundColorProperty =
			BindableProperty.Create(nameof(SelectedBackgroundColor), typeof(Color), typeof(Calendar), Color.Default,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedBackgroundColor((Color)newValue, (Color)oldValue));

		protected void ChangeSelectedBackgroundColor(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsSelected).ForEach(b => b.BackgroundColor = (newValue != Color.Default ?  newValue: Color.Transparent));
		}

		/// <summary>
		/// Gets or sets the background color of the selected date.
		/// </summary>
		/// <value>The color of the selected background.</value>
		public Color SelectedBackgroundColor
		{
			get { return (Color)GetValue(SelectedBackgroundColorProperty); }
			set { SetValue(SelectedBackgroundColorProperty, value); }
		}

		#endregion

		#region SelectedTextColor

		public static readonly BindableProperty SelectedTextColorProperty =
			BindableProperty.Create(nameof(SelectedTextColor), typeof(Color), typeof(Calendar), Color.Default,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedTextColor((Color)newValue, (Color)oldValue));

		protected void ChangeSelectedTextColor(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsSelected).ForEach(b => b.TextColor = (newValue != Color.Default ?  newValue: Color.Black));
		}

		/// <summary>
		/// Gets or sets the text color of the selected date.
		/// </summary>
		/// <value>The color of the selected text.</value>
		public Color SelectedTextColor
		{
			get { return (Color)GetValue(SelectedTextColorProperty); }
			set { SetValue(SelectedTextColorProperty, value); }
		}

		#endregion

		#region SelectedFontSize

		public static readonly BindableProperty SelectedFontSizeProperty =
			BindableProperty.Create(nameof(SelectedFontSize), typeof(double), typeof(Calendar), 20.0,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedFontSize((double)newValue, (double)oldValue));

		protected void ChangeSelectedFontSize(double newValue, double oldValue)
		{
			if (Math.Abs(newValue - oldValue) < 0.01) return;
			buttons.FindAll(b => b.IsSelected).ForEach(b => b.FontSize = newValue);
		}

		/// <summary>
		/// Gets or sets the font size of the selected date.
		/// </summary>
		/// <value>The size of the selected font.</value>
		public double SelectedFontSize
		{
			get { return (double)GetValue(SelectedFontSizeProperty); }
			set { SetValue(SelectedFontSizeProperty, value); }
		}

		#endregion

		#region SelectedFontAttributes

		public static readonly BindableProperty SelectedFontAttributesProperty =
			BindableProperty.Create(nameof(SelectedFontAttributes), typeof(FontAttributes), typeof(Calendar), FontAttributes.None,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedFontAttributes((FontAttributes)newValue, (FontAttributes)oldValue));

		protected void ChangeSelectedFontAttributes(FontAttributes newValue, FontAttributes oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsSelected).ForEach(b => b.FontAttributes = newValue);
		}

		/// <summary>
		/// Gets or sets the dates font attributes for selected dates.
		/// </summary>
		/// <value>The dates font attributes.</value>
		public FontAttributes SelectedFontAttributes
		{
			get { return (FontAttributes)GetValue(SelectedFontAttributesProperty); }
			set { SetValue(SelectedFontAttributesProperty, value); }
		}

		#endregion

		#region SelectedFontFamily

		public static readonly BindableProperty SelectedFontFamilyProperty =
					BindableProperty.Create(nameof(SelectedFontFamily), typeof(string), typeof(Calendar), default(string),
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedFontFamily((string)newValue, (string)oldValue));

		protected void ChangeSelectedFontFamily(string newValue, string oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsSelected).ForEach(b => b.FontFamily = newValue);
		}

		/// <summary>
		/// Gets or sets the font family of selected dates.
		/// </summary>
		public string SelectedFontFamily
		{
			get { return GetValue(SelectedFontFamilyProperty) as string; }
			set { SetValue(SelectedFontFamilyProperty, value); }
		}

		#endregion

		protected void SetButtonSelected(CalendarButton button, SpecialDate special)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				var defaultBackgroundColor = button.IsOutOfMonth ? DatesBackgroundColorOutsideMonth : DatesBackgroundColor;
				var defaultTextColor = button.IsOutOfMonth ? DatesTextColorOutsideMonth : DatesTextColor;
				var defaultFontAttributes = button.IsOutOfMonth ? DatesFontAttributesOutsideMonth : DatesFontAttributes;
				button.BackgroundPattern = special != null ? special.BackgroundPattern : null;
				button.IsEnabled = true;
				button.IsSelected = true;
				button.FontSize = SelectedFontSize;
				button.BorderWidth = SelectedBorderWidth;
				button.BorderColor = SelectedBorderColor;
				button.BackgroundColor = SelectedBackgroundColor != Color.Default ? SelectedBackgroundColor : (special != null && special.BackgroundColor.HasValue ? special.BackgroundColor.Value : defaultBackgroundColor);
				button.TextColor = SelectedTextColor != Color.Default ? SelectedTextColor : (special != null && special.TextColor.HasValue ? special.TextColor.Value : defaultTextColor);
				button.FontAttributes = SelectedFontAttributes != FontAttributes.None ? SelectedFontAttributes : (special != null && special.FontAttributes.HasValue ? special.FontAttributes.Value : defaultFontAttributes);
				button.FontFamily = !string.IsNullOrEmpty(SelectedFontFamily) ? SelectedFontFamily : (special != null && !string.IsNullOrEmpty(special.FontFamily) ? special.FontFamily : default(string));
			});
		}

		protected bool ChangeSelectedDate(DateTime? date, bool clicked = true)
		{
			if (!date.HasValue) return false;

			if (!MultiSelectDates)
			{
				buttons.FindAll(b => b.IsSelected).ForEach(b => ResetButton(b));
				SelectedDates.Clear();
			}

			if (buttons.Count == 0)
			{
				SelectedDates.Add(SelectedDate.Value.Date);
			}

			var button = buttons.Find(b => b.Date.HasValue && b.Date.Value.Date == date.Value.Date && b.IsEnabled);
			if (button == null) return false;
			var deselect = button.IsSelected;
			if (button.IsSelected)
			{
				ResetButton(button);
			}
			else
			{
				SelectedDates.Add(SelectedDate.Value.Date);
				var spD = SpecialDates?.FirstOrDefault(s => s.Date.Date == button.Date.Value.Date);
				SetButtonSelected(button, spD);
			}
			if (clicked)
			{
				DateClicked?.Invoke(this, new DateTimeEventArgs { DateTime = SelectedDate.Value });
				DateCommand?.Execute(SelectedDate.Value);
			}
			return deselect;
		}

		protected void ResetButton(CalendarButton b)
		{
			if (b.Date.HasValue) SelectedDates.Remove(b.Date.Value.Date);
			var spD = SpecialDates?.FirstOrDefault(s => s.Date.Date == b.Date.Value.Date);
			SetButtonNormal(b);
			if (spD != null)
			{
				SetButtonSpecial(b, spD);
			}
		}
	}
}
