using FlooringSalesManager.Data;
using FlooringSalesManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FlooringSalesManager.Views
{
    public partial class ProductListWindow : Window
    {
        private List<ProductBase> _allProducts = new();

        public ProductListWindow()
        {
            InitializeComponent();
            LoadProductsFromDatabase();
        }

        // Loads all products from the SQLite DB and displays them in the DataGrid
        private void LoadProductsFromDatabase()
        {
            try
            {
                _allProducts = ProductDatabase.GetAllProducts();
                ProductDataGrid.ItemsSource = _allProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        // Search/filter logic
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
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

        // Opens the Add Product dialog
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddProductWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadProductsFromDatabase(); // Refresh after adding
            }
        }

        // Optionally, add Edit and Delete handlers here

        // Example of Delete (assuming you add a button in each row):
        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem is ProductBase selectedProduct)
            {
                if (MessageBox.Show($"Сигурни ли сте, че искате да изтриете {selectedProduct.Name}?", "Потвърждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ProductDatabase.DeleteProduct(selectedProduct.Id);
                    LoadProductsFromDatabase();
                }
            }
        }
    }
}
