using Microsoft.Data.Sqlite;
using FlooringSalesManager.Models;
using System;
using System.Collections.Generic;

namespace FlooringSalesManager.Data
{
    public static class OrderDatabase
    {
        // Add a new order with all items
        public static void AddOrder(Order order)
        {
            using var conn = DatabaseHelper.GetConnection();
            using var transaction = conn.BeginTransaction();

            try
            {
                // Insert order
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
INSERT INTO Orders (OrderDate, CustomerName, CustomerPhone)
VALUES ($date, $name, $phone);
SELECT last_insert_rowid();";
                    cmd.Parameters.AddWithValue("$date", order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("$name", order.CustomerName ?? "");
                    cmd.Parameters.AddWithValue("$phone", order.CustomerPhone ?? "");
                    order.OrderId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Insert order items
                foreach (var item in order.Items)
                {
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = @"
INSERT INTO OrderItems (OrderId, ProductType, ProductNumber, Quantity, UnitPrice)
VALUES ($oid, $type, $num, $qty, $price);";
                    cmd.Parameters.AddWithValue("$oid", order.OrderId);
                    cmd.Parameters.AddWithValue("$type", item.ProductType);
                    cmd.Parameters.AddWithValue("$num", item.ProductNumber);
                    cmd.Parameters.AddWithValue("$qty", item.Quantity);
                    cmd.Parameters.AddWithValue("$price", item.UnitPrice);
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        // Read all orders, including items
        public static List<Order> GetAllOrders()
        {
            var orders = new List<Order>();

            using var conn = DatabaseHelper.GetConnection();

            // 1. Read orders
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Orders ORDER BY OrderDate DESC";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var order = new Order
                    {
                        OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                        OrderDate = DateTime.Parse(reader.GetString(reader.GetOrdinal("OrderDate"))),
                        CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                        CustomerPhone = reader.GetString(reader.GetOrdinal("CustomerPhone"))
                    };
                    orders.Add(order);
                }
            }

            // 2. Read items for each order
            foreach (var order in orders)
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM OrderItems WHERE OrderId = $oid";
                cmd.Parameters.AddWithValue("$oid", order.OrderId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    order.Items.Add(new OrderItem
                    {
                        OrderItemId = reader.GetInt32(reader.GetOrdinal("OrderItemId")),
                        OrderId = order.OrderId,
                        ProductType = reader.GetString(reader.GetOrdinal("ProductType")),
                        ProductNumber = reader.GetString(reader.GetOrdinal("ProductNumber")),
                        Quantity = reader.GetDouble(reader.GetOrdinal("Quantity")),
                        UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice"))
                    });
                }
            }

            return orders;
        }
    }
}
