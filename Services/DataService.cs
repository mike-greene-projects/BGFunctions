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
        private readonly string _connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONN")
                                                    ?? "Host=localhost;Port=5432;Username=postgres;Password=password;Database=mydb";

        public async Task<(bool Success, string Message)> UpsertBoardgamesAsync(List<DataRequest> requests)
        {
            // Serialize the list to JSON to send to the stored procedure
            var jsonData = JsonSerializer.Serialize(requests);

            await using NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = "SELECT * FROM upsert_boardgames(@data)";

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
}