using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfThreadingIssueSilentDemoFixed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowSilentFixed : Window
    {
		private readonly MainViewSilentModelFixed _vm;
		public MainWindowSilentFixed()
        {
            InitializeComponent();

			_vm = new MainViewSilentModelFixed();   // ✅ UI thread
			DataContext = _vm;
		}

		private async void Reload_Click(object sender, RoutedEventArgs e)
		{
			await _vm.LoadItemsAsync();
		}
	}
}
