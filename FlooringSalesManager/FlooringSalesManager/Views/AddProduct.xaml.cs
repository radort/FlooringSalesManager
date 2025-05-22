using FlooringSalesManager.Models;
using FlooringSalesManager.Data;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FlooringSalesManager.Views
{
    public partial class AddProduct : Window
    {
        public AddProduct()
        {
            InitializeComponent();
            TypeComboBox.SelectedIndex = 0; // Default to first type
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedType = (TypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (selectedType == "Ламинат")
            {
                FlooringFields.Visibility = Visibility.Visible;
                SkirtingFields.Visibility = Visibility.Collapsed;
            }
            else if (selectedType == "Перваз")
            {
                FlooringFields.Visibility = Visibility.Collapsed;
                SkirtingFields.Visibility = Visibility.Visible;
            }
            // Add more types if needed
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedType = (TypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            // Validate common fields
            if (string.IsNullOrWhiteSpace(NumberBox.Text) ||
                string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Моля, попълнете всички полета.");
                return;
            }

            ProductBase newProduct;

            if (selectedType == "Ламинат")
            {
                if (string.IsNullOrWhiteSpace(PriceBox.Text) ||
                    string.IsNullOrWhiteSpace(M2PerBoxBox.Text))
                {
                    MessageBox.Show("Моля, попълнете всички полета за ламинат.");
                    return;
                }
                if (!decimal.TryParse(PriceBox.Text, out decimal pricePerSqM) ||
                    !decimal.TryParse(M2PerBoxBox.Text, out decimal m2PerBox))
                {
                    MessageBox.Show("Цена и м² на кутия трябва да са числа.");
                    return;
                }

                newProduct = new FlooringProduct
                {
                    Number = NumberBox.Text,
                    Name = NameBox.Text,
                    Type = selectedType,
                    PricePerSquareMeter = pricePerSqM,
                    M2PerBox = m2PerBox
                };
            }
            else if (selectedType == "Перваз")
            {
                if (string.IsNullOrWhiteSpace(PricePerMeterBox.Text) ||
                    string.IsNullOrWhiteSpace(LengthPerPieceBox.Text))
                {
                    MessageBox.Show("Моля, попълнете всички полета за перваз.");
                    return;
                }
                if (!decimal.TryParse(PricePerMeterBox.Text, out decimal pricePerMeter) ||
                    !decimal.TryParse(LengthPerPieceBox.Text, out decimal lengthPerPiece))
                {
                    MessageBox.Show("Цена на метър и дължина трябва да са числа.");
                    return;
                }

                newProduct = new SkirtingProduct
                {
                    Number = NumberBox.Text,
                    Name = NameBox.Text,
                    Type = selectedType,
                    PricePerMeter = pricePerMeter,
                    LengthPerPiece = lengthPerPiece
                };
            }
            else
            {
                MessageBox.Show("Моля, изберете валиден тип продукт.");
                return;
            }

            // Add to SQLite database
            ProductDatabase.AddProduct(newProduct);

            // Optionally: show a confirmation
            // MessageBox.Show("Продуктът е добавен успешно!");

            this.DialogResult = true;
            this.Close();
        }
    }
}
