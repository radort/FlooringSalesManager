using FlooringSalesManager.Models; // Adjust if your namespace is different
using System;
using System.Windows;
using System.Windows.Controls;

namespace FlooringSalesManager.Views
{
    public partial class AddProduct : Window
    {
        public ProductBase NewProduct { get; private set; }

        public AddProduct()
        {
            InitializeComponent();
            // Optionally select a default type
            TypeComboBox.SelectedIndex = 0;
        }

        // Show/hide fields depending on the selected type
        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedType = (TypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (selectedType == "Настилка")
            {
                FlooringFields.Visibility = Visibility.Visible;
                SkirtingFields.Visibility = Visibility.Collapsed;
            }
            else if (selectedType == "Перваз")
            {
                FlooringFields.Visibility = Visibility.Collapsed;
                SkirtingFields.Visibility = Visibility.Visible;
            }
            // Add logic for more types if needed
        }

        // Create the appropriate Product and close the window
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

                NewProduct = new FlooringProduct
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

                NewProduct = new SkirtingProduct
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

            this.DialogResult = true;
            this.Close();
        }
    }
}
