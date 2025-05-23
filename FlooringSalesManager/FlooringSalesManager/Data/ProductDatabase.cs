using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using FlooringSalesManager.Models;

namespace FlooringSalesManager.Data
{
    public static class ProductDatabase
    {
        public static void AddProduct(ProductBase product)
        {
            using var conn = DatabaseHelper.GetConnection();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
INSERT INTO Products 
(Number, Name, Type, PricePerSquareMeter, M2PerBox, PricePerMeter, LengthPerPiece)
VALUES ($num, $name, $type, $sqm, $m2box, $perMeter, $lenPiece)";
            cmd.Parameters.AddWithValue("$num", product.Number);
            cmd.Parameters.AddWithValue("$name", product.Name);
            cmd.Parameters.AddWithValue("$type", product.Type);

            if (product is FlooringProduct flooring)
            {
                cmd.Parameters.AddWithValue("$sqm", flooring.PricePerSquareMeter);
                cmd.Parameters.AddWithValue("$m2box", flooring.M2PerBox);
                cmd.Parameters.AddWithValue("$perMeter", DBNull.Value);
                cmd.Parameters.AddWithValue("$lenPiece", DBNull.Value);
            }
            else if (product is SkirtingProduct skirting)
            {
                cmd.Parameters.AddWithValue("$sqm", DBNull.Value);
                cmd.Parameters.AddWithValue("$m2box", DBNull.Value);
                cmd.Parameters.AddWithValue("$perMeter", skirting.PricePerMeter);
                cmd.Parameters.AddWithValue("$lenPiece", skirting.LengthPerPiece);
            }
            else
            {
                cmd.Parameters.AddWithValue("$sqm", DBNull.Value);
                cmd.Parameters.AddWithValue("$m2box", DBNull.Value);
                cmd.Parameters.AddWithValue("$perMeter", DBNull.Value);
                cmd.Parameters.AddWithValue("$lenPiece", DBNull.Value);
            }

            cmd.ExecuteNonQuery();
        }

        public static List<ProductBase> GetAllProducts()
        {
            var products = new List<ProductBase>();

            using var conn = DatabaseHelper.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Products";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var type = reader.GetString(reader.GetOrdinal("Type"));
                if (type == "Настилка")
                {
                    products.Add(new FlooringProduct
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Number = reader.GetString(reader.GetOrdinal("Number")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Type = type,
                        PricePerSquareMeter = reader.IsDBNull(reader.GetOrdinal("PricePerSquareMeter")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PricePerSquareMeter")),
                        M2PerBox = reader.IsDBNull(reader.GetOrdinal("M2PerBox")) ? 0 : reader.GetDecimal(reader.GetOrdinal("M2PerBox"))
                    });
                }
                else if (type == "Перваз")
                {
                    products.Add(new SkirtingProduct
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Number = reader.GetString(reader.GetOrdinal("Number")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Type = type,
                        PricePerMeter = reader.IsDBNull(reader.GetOrdinal("PricePerMeter")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PricePerMeter")),
                        LengthPerPiece = reader.IsDBNull(reader.GetOrdinal("LengthPerPiece")) ? 0 : reader.GetDecimal(reader.GetOrdinal("LengthPerPiece"))
                    });
                }
            }
            return products;
        }

        public static void UpdateProduct(ProductBase product)
        {
            using var conn = DatabaseHelper.GetConnection();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
UPDATE Products
SET Number = $num,
    Name = $name,
    Type = $type,
    PricePerSquareMeter = $sqm,
    M2PerBox = $m2box,
    PricePerMeter = $perMeter,
    LengthPerPiece = $lenPiece
WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", product.Id);
            cmd.Parameters.AddWithValue("$num", product.Number);
            cmd.Parameters.AddWithValue("$name", product.Name);
            cmd.Parameters.AddWithValue("$type", product.Type);

            if (product is FlooringProduct flooring)
            {
                cmd.Parameters.AddWithValue("$sqm", flooring.PricePerSquareMeter);
                cmd.Parameters.AddWithValue("$m2box", flooring.M2PerBox);
                cmd.Parameters.AddWithValue("$perMeter", DBNull.Value);
                cmd.Parameters.AddWithValue("$lenPiece", DBNull.Value);
            }
            else if (product is SkirtingProduct skirting)
            {
                cmd.Parameters.AddWithValue("$sqm", DBNull.Value);
                cmd.Parameters.AddWithValue("$m2box", DBNull.Value);
                cmd.Parameters.AddWithValue("$perMeter", skirting.PricePerMeter);
                cmd.Parameters.AddWithValue("$lenPiece", skirting.LengthPerPiece);
            }
            else
            {
                cmd.Parameters.AddWithValue("$sqm", DBNull.Value);
                cmd.Parameters.AddWithValue("$m2box", DBNull.Value);
                cmd.Parameters.AddWithValue("$perMeter", DBNull.Value);
                cmd.Parameters.AddWithValue("$lenPiece", DBNull.Value);
            }

            cmd.ExecuteNonQuery();
        }

        public static void DeleteProduct(int productId)
        {
            using var conn = DatabaseHelper.GetConnection();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = "DELETE FROM Products WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", productId);
            cmd.ExecuteNonQuery();
        }
    }
}
