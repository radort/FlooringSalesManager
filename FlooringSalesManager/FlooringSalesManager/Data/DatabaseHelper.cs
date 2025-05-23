using Microsoft.Data.Sqlite;
using System.IO;
using System.Diagnostics;

namespace FlooringSalesManager.Data
{
    public static class DatabaseHelper
    {
        private static readonly string DbFile = "Data/store.db";

        public static SqliteConnection GetConnection()
        {
            Directory.CreateDirectory("Data");
            string dbFullPath = Path.GetFullPath(DbFile);
            Debug.WriteLine("SQLite DB path: " + dbFullPath);
            var connection = new SqliteConnection($"Data Source={DbFile}");
            connection.Open();
            return connection;
        }

        public static void EnsureTables()
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Products (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Number TEXT NOT NULL,
    Name TEXT NOT NULL,
    Type TEXT NOT NULL,
    PricePerSquareMeter REAL,
    M2PerBox REAL,
    PricePerMeter REAL,
    LengthPerPiece REAL
);

CREATE TABLE IF NOT EXISTS Orders (
    OrderId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderDate TEXT NOT NULL,
    CustomerName TEXT,
    CustomerPhone TEXT
);

CREATE TABLE IF NOT EXISTS OrderItems (
    OrderItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    ProductType TEXT NOT NULL,
    ProductNumber TEXT NOT NULL,
    Quantity REAL NOT NULL,
    UnitPrice REAL NOT NULL,
    FOREIGN KEY(OrderId) REFERENCES Orders(OrderId)
);



";
            cmd.ExecuteNonQuery();
        }

    }
}
