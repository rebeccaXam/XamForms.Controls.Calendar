using System;
using System.Collections.Generic;
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
		public string FontFamily { get; set; }
		public int? BorderWidth { get; set; }
		public double? FontSize { get; set; }
		public bool Selectable { get; set; }
		/// <summary>
		/// Gets or sets the background image (only working on iOS and Android).
		/// </summary>
		/// <value>The background pattern.</value>
		public FileImageSource BackgroundImage { get; set; }
		/// <summary>
		/// Gets or sets the background pattern (only working on iOS and Android).
		/// </summary>
		/// <value>The background pattern.</value>
		public BackgroundPattern BackgroundPattern{ get; set; }
	}

	public class BackgroundPattern
	{
		protected int columns;
		public BackgroundPattern(int columns)
		{
			this.columns = columns;
		}

		public List<Pattern> Pattern;

		public float GetTop(int t)
		{
			float r = 0;
			for (int i = t-columns; i > -1; i-=columns)
			{
				r += Pattern[i].HeightPercent;
			}
			return r;
		}

		public float GetLeft(int l)
		{
			float r = 0;
			for (int i = l-1; i > -1 && (i+1) % columns != 0; i--)
			{
				r += Pattern[i].WidthPercent;
			}
			return r;
		}
	}

	/*Left = 1,
	Center = 2,
	Right = 4,
	Top = 8,
	Middle = 16,
	Bottom = 32*/
	public enum TextAlign
	{
		LeftTop = 9,
		LeftCenter = 17,
		LeftBottom = 33,
		CenterTop = 10,
		Middle = 18,
		CenterBottom = 34,
		RightTop = 12,
		RightCenter = 20,
		RightBottom = 36
	}

	public struct Pattern
	{
		public float WidthPercent;
		public float HeightPercent;
		public Color Color;

		public string Text;
		public Color TextColor;
		public float TextSize;
		public TextAlign TextAlign;
	}
}

