using System;
using Xamarin.Forms;

namespace XamForms.Controls
{
	public class SpecialDate
	{
		public SpecialDate(DateTime date)
		{
			Date = date;
		}

		public DateTime Date { get; set; }
		public Color? TextColor { get; set; }
		public Color? BackgroundColor { get; set; }
		public Color? BorderColor { get; set; }
		public FontAttributes? FontAttributes { get; set; }
		public int? BorderWidth { get; set; }
		public double? FontSize { get; set; }
		public bool Selectable { get; set; }
	}
}

