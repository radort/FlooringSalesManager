using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlooringSalesManager.Views;

namespace FlooringSalesManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void NewOrder_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewOrderWindow();
            window.ShowDialog();
        }

        private void OrderList_Click(object sender, RoutedEventArgs e)
        {
            var window = new OrderListWindow();
            window.ShowDialog();
        }

        private void ProductList_Click(object sender, RoutedEventArgs e)
        {
            var window = new ProductListWindow();
            window.ShowDialog();
        }

    }
}