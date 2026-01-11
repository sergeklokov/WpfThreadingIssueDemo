using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfThreadingIssueSilentDemoFixed
{
	public class MainViewSilentModelFixed : INotifyPropertyChanged
	{
		public ObservableCollection<string> Items { get; }
			= new ObservableCollection<string>();

		public MainViewSilentModelFixed()
		{
			// Constructor is UI-thread only
			// No background work here

			// X THIS CODE WILL BREAK IT! 
			//Task.Run(() =>
			//{
			//	Thread.Sleep(1000);
			//	// ❌ THREADING VIOLATION
			//	Items.Add("Added from background thread");
			//});
		}

		public async Task LoadItemsAsync()
		{
			// ❌ NO UI ACCESS HERE
			var data = await Task.Run(() =>
			{
				Task.Delay(1000).Wait();

				return new[]
				{
					"Alpha",
					"Beta",
					"Gamma"
				};
			});

			// ✅ Back on UI thread after await
			Items.Clear();

			foreach (var item in data)
				Items.Add(item);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
