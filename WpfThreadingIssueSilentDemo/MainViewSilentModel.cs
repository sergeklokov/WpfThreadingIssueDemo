using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfThreadingIssueSilentDemo
{
	//❌ BROKEN ON PURPOSE
	public class MainViewSilentModel : INotifyPropertyChanged
	{
		public ObservableCollection<string> Items { get; }
			= new ObservableCollection<string>();

		public MainViewSilentModel()
		{
			// Simulate background work
			Task.Run(() =>
			{
				Thread.Sleep(1000);

				// ❌ THREADING VIOLATION
				Items.Add("Added from background thread");
			});
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
