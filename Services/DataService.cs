using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;
using Models;

namespace Services
{
    public class DataService
    {
        private readonly string _connectionString = Environment.GetEnvironmentVariable("bggdb") ?? "";

        public async Task<(bool Success, string Message)> UpsertBoardgamesAsync(List<DataRequest> requests)
        {
            string jsonData = JsonSerializer.Serialize(requests);
            await using NpgsqlConnection conn = new NpgsqlConnection(Base64.Decode(_connectionString));
            await conn.OpenAsync();

            const string sql = "SELECT * FROM app.upsert_boardgames(@data)";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("data", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonData);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                bool success = reader.GetBoolean(0);
                string message = reader.GetString(1);
                return (success, message);
            }

            return (false, "No response from database");
        }
    }
    
    public static class Base64
    {
        public static string Decode(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}