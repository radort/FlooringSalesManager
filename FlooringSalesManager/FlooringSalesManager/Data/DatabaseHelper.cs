using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace FlooringSalesManager.Data
{
    public static class DatabaseHelper
    {
        private static readonly string DbFile = "Data/store.db";

        public static SqliteConnection GetConnection()
        {
            Directory.CreateDirectory("Data");
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

-- You can add Orders table creation here later
";
            cmd.ExecuteNonQuery();
        }
    }
}
