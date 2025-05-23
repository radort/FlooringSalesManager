using FlooringSalesManager.Models;
using FlooringSalesManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FlooringSalesManager.Views
{
    public partial class NewOrderWindow : Window
    {
        private List<OrderItem> _orderItems = new();

        public NewOrderWindow()
        {
            InitializeComponent();
            UpdateQtyLabel(); // Set label on start
        }

        private void ProductTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateQtyLabel();
        }

        private void UpdateQtyLabel()
        {
            var type = ((ComboBoxItem)ProductTypeBox.SelectedItem)?.Content?.ToString();
            // Handles Bulgarian and English for flexibility
            if (type == "Перваз" || type == "Skirting")
                QuantityLabel.Content = "метри:";
            else
                QuantityLabel.Content = "м²:";
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            string? type = ((ComboBoxItem)ProductTypeBox.SelectedItem)?.Content?.ToString();
            string number = ProductNumberBox.Text.Trim();
            string qtyStr = QuantityBox.Text.Trim().Replace(',', '.');
            string priceStr = UnitPriceBox.Text.Trim().Replace(',', '.');

            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(number)
                || string.IsNullOrWhiteSpace(qtyStr) || string.IsNullOrWhiteSpace(priceStr))
            {
                MessageBox.Show("Попълнете всички полета за артикула!");
                return;
            }

            if (!double.TryParse(qtyStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double qty) ||
                qty <= 0)
            {
                MessageBox.Show("Невалидно количество.");
                return;
            }

            if (!decimal.TryParse(priceStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal price) ||
                price <= 0)
            {
                MessageBox.Show("Невалидна цена.");
                return;
            }

            _orderItems.Add(new OrderItem
            {
                ProductType = type,
                ProductNumber = number,
                Quantity = qty,
                UnitPrice = price
            });

            OrderItemsGrid.ItemsSource = null;
            OrderItemsGrid.ItemsSource = _orderItems;

            // Clear input fields
            ProductNumberBox.Text = "";
            QuantityBox.Text = "";
            UnitPriceBox.Text = "";
            ProductNumberBox.Focus();
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button?.DataContext as OrderItem;
            if (item != null)
            {
                _orderItems.Remove(item);
                OrderItemsGrid.ItemsSource = null;
                OrderItemsGrid.ItemsSource = _orderItems;
            }
        }

        private void SaveOrder_Click(object sender, RoutedEventArgs e)
        {
            string name = CustomerNameBox.Text.Trim();
            string phone = CustomerPhoneBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Въведете име и телефон на клиента!");
                return;
            }
            if (!_orderItems.Any())
            {
                MessageBox.Show("Добавете поне един артикул към поръчката!");
                return;
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                CustomerName = name,
                CustomerPhone = phone,
                Items = _orderItems.ToList()
            };

            try
            {
                OrderDatabase.AddOrder(order);
                MessageBox.Show("Поръчката е записана успешно!");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при запис: " + ex.Message);
            }
        }
    }
}
