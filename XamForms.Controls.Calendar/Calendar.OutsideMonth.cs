using Xamarin.Forms;

namespace XamForms.Controls
{
	public partial class Calendar : ContentView
	{

		#region DatesTextColorOutsideMonth

		public static readonly BindableProperty DatesTextColorOutsideMonthProperty =
			BindableProperty.Create(nameof(DatesTextColorOutsideMonth), typeof(Color), typeof(Calendar), Color.FromHex("#aaaaaa"),
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDatesTextColorOutsideMonth((Color)newValue, (Color)oldValue));

		protected void ChangeDatesTextColorOutsideMonth(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsEnabled && !b.IsSelected && b.IsOutOfMonth).ForEach(b => b.TextColor = newValue);
		}

		/// <summary>
		/// Gets or sets the text color of the dates not in the focused month.
		/// </summary>
		/// <value>The dates text color outside month.</value>
		public Color DatesTextColorOutsideMonth
		{
			get { return (Color)GetValue(DatesTextColorOutsideMonthProperty); }
			set { SetValue(DatesTextColorOutsideMonthProperty, value); }
		}

		#endregion

		#region DatesBackgroundColorOutsideMonth

		public static readonly BindableProperty DatesBackgroundColorOutsideMonthProperty =
			BindableProperty.Create(nameof(DatesBackgroundColorOutsideMonth), typeof(Color), typeof(Calendar), Color.White,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDatesBackgroundColorOutsideMonth((Color)newValue, (Color)oldValue));

		protected void ChangeDatesBackgroundColorOutsideMonth(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsEnabled && !b.IsSelected && b.IsOutOfMonth).ForEach(b => b.BackgroundColor = newValue);
		}

		/// <summary>
		/// Gets or sets the background color of the dates not in the focused month.
		/// </summary>
		/// <value>The dates background color outside month.</value>
		public Color DatesBackgroundColorOutsideMonth
		{
			get { return (Color)GetValue(DatesBackgroundColorOutsideMonthProperty); }
			set { SetValue(DatesBackgroundColorOutsideMonthProperty, value); }
		}

		#endregion

		#region DatesFontAttributesOutsideMonth

		public static readonly BindableProperty DatesFontAttributesOutsideMonthProperty =
			BindableProperty.Create(nameof(DatesFontAttributesOutsideMonth), typeof(FontAttributes), typeof(Calendar), FontAttributes.None,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDatesFontAttributesOutsideMonth((FontAttributes)newValue, (FontAttributes)oldValue));

		protected void ChangeDatesFontAttributesOutsideMonth(FontAttributes newValue, FontAttributes oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsEnabled && !b.IsSelected && b.IsOutOfMonth).ForEach(b => b.FontAttributes = newValue);
		}

		/// <summary>
		/// Gets or sets the dates font attributes for dates outside of the month.
		/// </summary>
		/// <value>The dates font attributes.</value>
		public FontAttributes DatesFontAttributesOutsideMonth
		{
			get { return (FontAttributes)GetValue(DatesFontAttributesOutsideMonthProperty); }
			set { SetValue(DatesFontAttributesOutsideMonthProperty, value); }
		}

		#endregion

		#region DatesFontOutsideMonth

		public static readonly BindableProperty DatesFontOutsideMonthProperty =
			BindableProperty.Create(nameof(DatesFontOutsideMonth), typeof(Font), typeof(Calendar), Font.Default,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDatesFontOutsideMonth((Font)newValue, (Font)oldValue));

		protected void ChangeDatesFontOutsideMonth(Font newValue, Font oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsEnabled && !b.IsSelected && b.IsOutOfMonth).ForEach(b => b.Font = newValue);
		}

		/// <summary>
		/// Gets or sets the dates font for dates outside of the month.
		/// </summary>
		/// <value>The dates font attributes.</value>
		public Font DatesFontOutsideMonth
		{
			get { return (Font)GetValue(DatesFontOutsideMonthProperty); }
			set { SetValue(DatesFontOutsideMonthProperty, value); }
		}

		#endregion

		#region DatesFontFamilyOutsideMonth

		public static readonly BindableProperty DatesFontFamilyOutsideMonthProperty =
			BindableProperty.Create(nameof(DatesFontFamilyOutsideMonth), typeof(string), typeof(Calendar), default(string),
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDatesFontFamilyOutsideMonth((string)newValue, (string)oldValue));

		protected void ChangeDatesFontFamilyOutsideMonth(string newValue, string oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsEnabled && !b.IsSelected && b.IsOutOfMonth).ForEach(b => b.FontFamily = newValue);
		}

		/// <summary>
		/// Gets or sets the dates font family for dates outside of the month.
		/// </summary>
		public string DatesFontFamilyOutsideMonth
		{
			get { return GetValue(DatesFontOutsideMonthProperty) as string; }
			set { SetValue(DatesFontOutsideMonthProperty, value); }
		}

		#endregion
	}
}
