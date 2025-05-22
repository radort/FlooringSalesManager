using FlooringSalesManager.Data;
using FlooringSalesManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FlooringSalesManager.Views
{
    public partial class ProductListWindow : Window
    {
        private List<ProductBase> _allProducts = new();

        public ProductListWindow()
        {
            InitializeComponent();

            // Load products from SQLite
            LoadProductsFromDatabase();
        }

        private void LoadProductsFromDatabase()
        {
            _allProducts = ProductDatabase.GetAllProducts();
            ProductDataGrid.ItemsSource = _allProducts;
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var searchText = SearchBox.Text.ToLower();
            var filtered = _allProducts
                .Where(p =>
                    (p.Number ?? "").ToLower().Contains(searchText) ||
                    (p.Name ?? "").ToLower().Contains(searchText) ||
                    (p.Type ?? "").ToLower().Contains(searchText))
                .ToList();
            ProductDataGrid.ItemsSource = filtered;
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddProduct();
            if (addWindow.ShowDialog() == true)
            {
                _allProducts.Add(addWindow.NewProduct);
                ProductDataGrid.ItemsSource = null; // Refresh the grid
                ProductDataGrid.ItemsSource = _allProducts;
            }
        }


    }
}
