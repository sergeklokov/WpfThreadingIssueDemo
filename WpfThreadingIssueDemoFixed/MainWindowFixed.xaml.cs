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

namespace WpfThreadingIssueDemoFixed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowFixed : Window
    {
        public MainWindowFixed()
        {
            InitializeComponent();

			var vm = new MainViewModelFixed(); // UI thread
			DataContext = vm;

			Loaded += async (_, __) =>
			{
				await vm.InitializeAsync();
			};
		}
    }
}
