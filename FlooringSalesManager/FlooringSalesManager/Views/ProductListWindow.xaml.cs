using FlooringSalesManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FlooringSalesManager.Views
{
    public partial class ProductListWindow : Window
    {
        private List<Product> _allProducts;

        public ProductListWindow()
        {
            InitializeComponent();

            // Sample data for now
            _allProducts = new List<Product>
            {   
                new Product { Id = 1, Name = "Дъб Мока", Type = "Ламинат", PricePerSquareMeter = 22.5m, M2PerBox = 2.4m },
                new Product { Id = 2, Name = "Сив SPC", Type = "SPC", PricePerSquareMeter = 35.0m, M2PerBox = 1.9m }
            };

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

    }
}
