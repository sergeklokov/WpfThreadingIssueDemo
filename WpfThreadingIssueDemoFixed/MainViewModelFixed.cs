using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfThreadingIssueDemoFixed
{
	//❌ BROKEN ON PURPOSE
	public class MainViewModelFixed : INotifyPropertyChanged
	{
		private string _status;

		public ObservableCollection<string> Items { get; }
			= new ObservableCollection<string>();

		public string Status
		{
			get => _status;
			private set
			{
				if (_status == value)
					return;

				_status = value;
				OnPropertyChanged();
			}
		}

		public MainViewModelFixed()
		{
			// Simulate background work
			//Task.Run(() =>
			//{
			//	Thread.Sleep(1000);

			//	// ❌ THREADING VIOLATION
			//	Items.Add("Added from background thread");
			//});

			Status = "Idle";
		}


		/// <summary>
		/// Explicit async initialization method.
		/// Call this AFTER assigning DataContext.
		/// </summary>
		public async Task InitializeAsync()
		{
			Status = "Loading...";

			// Background work (no UI access here)
			var data = await Task.Run(() =>
			{
				// Simulate expensive work
				Task.Delay(1000).Wait();

				return new[]
				{
					"Item 1",
					"Item 2",
					"Item 3"
				};
			});

			// After await:
			// we are back on the UI thread (WPF SynchronizationContext)
			Items.Clear();

			foreach (var item in data)
				Items.Add(item);

			Status = "Ready";
		}

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
