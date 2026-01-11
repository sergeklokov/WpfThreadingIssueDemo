using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfThreadingIssueSilentDemo
{
	//❌ BROKEN ON PURPOSE
	public class MainViewSilentModel : INotifyPropertyChanged
	{
		public ObservableCollection<string> Items { get; }
			// = new ObservableCollection<string>();

		private string _lastChange;
		public string LastChange
		{
			get => _lastChange;
			private set
			{
				_lastChange = value;
				OnPropertyChanged();
			}
		}

		private string _heartbeat;
		public string Heartbeat
		{
			get => _heartbeat;
			private set
			{
				_heartbeat = value;
				OnPropertyChanged();
			}
		}

		public MainViewSilentModel()
		{
			Items = new ObservableCollection<string>(); // now created on UI thread

			// Force WPF to attach CollectionChanged handlers NOW
			var _view = System.Windows.Data.CollectionViewSource.GetDefaultView(Items);


			// 1️⃣ Heartbeat proves UI thread is alive
			var uiTimer = new System.Windows.Threading.DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(500)
			};

			uiTimer.Tick += (_, __) =>
			{
				Heartbeat = DateTime.Now.ToLongTimeString();
			};

			uiTimer.Start();

			// 2️⃣ Background thread mutates collection (❌ violation)
			Task.Run(() =>
			{
				int i = 0;
				while (true)
				{
					Thread.Sleep(700);

					Items.Add("Item " + i++); // ❌ off UI thread

					Debug.WriteLine($"Added item {i}");
					LastChange = $"Added item {i} at {DateTime.Now:T}";
				}
			});
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
