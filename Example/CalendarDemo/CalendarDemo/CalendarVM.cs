using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace CalendarDemo
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class CalendarVM : BaseViewModel
	{
		private DateTime? _date;
		public DateTime? Date
		{
			get
			{
				return _date;
			}
			set
			{
				_date = value;
				NotifyPropertyChanged(nameof(Date));
			}
		}

		private ObservableCollection<XamForms.Controls.SpecialDate> attendances;
		public ObservableCollection<XamForms.Controls.SpecialDate> Attendances
		{
			get { return attendances; }
			set { attendances = value;  NotifyPropertyChanged(nameof(Attendances));}
		}

		public ICommand DateChosen
		{
			get { 
				return new Command((obj) => { 
					System.Diagnostics.Debug.WriteLine(obj as DateTime?);
				});
			}
		}
		
	}
}
