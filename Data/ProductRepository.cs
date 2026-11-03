using Lab_8 ;
using Lab_8.Models;

using Microsoft.Data.SqlClient;

namespace Lab_8.Data
{
    public class BridalProductRepository
    {
        private readonly string _connectionString;

        public BridalProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection is missing.");
        }

        public async Task<List<BridalProduct>> GetAllAsync()
        {
            var products = new List<BridalProduct>();

            const string sql = @"
                SELECT Id, Title, [Description], [Image], Price, Quantity, [Status]
                FROM dbo.BridalProducts
                ORDER BY Id DESC;";

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(sql, connection);

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                products.Add(new BridalProduct
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"]?.ToString() ?? string.Empty,
                    Description = reader["Description"] == DBNull.Value ? null : reader["Description"]?.ToString(),
                    Image = reader["Image"] == DBNull.Value ? null : reader["Image"]?.ToString(),
                    Price = Convert.ToDecimal(reader["Price"]),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    Status = reader["Status"]?.ToString() ?? "Available"
                });
            }

            return products;
        }

        public async Task<BridalProduct?> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT Id, Title, [Description], [Image], Price, Quantity, [Status]
                FROM dbo.BridalProducts
                WHERE Id = @Id;";

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                // Galat: return new BridalProducts
                // Sahi:
                return new BridalProduct
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"]?.ToString() ?? string.Empty,
                    Description = reader["Description"] == DBNull.Value ? null : reader["Description"]?.ToString(),
                    Image = reader["Image"] == DBNull.Value ? null : reader["Image"]?.ToString(),
                    Price = Convert.ToDecimal(reader["Price"]),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    Status = reader["Status"]?.ToString() ?? "Available"
                };
            }

            return null;
        }

        public async Task CreateAsync(BridalProduct product)
        {
            const string sql = @"
                INSERT INTO dbo.BridalProducts (Title, [Description], [Image], Price, Quantity, [Status])
                VALUES (@Title, @Description, @Image, @Price, @Quantity, @Status);";

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Title", product.Title);
            command.Parameters.AddWithValue("@Description", (object?)product.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@Image", (object?)product.Image ?? DBNull.Value);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@Quantity", product.Quantity);
            command.Parameters.AddWithValue("@Status", product.Status);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<bool> UpdateAsync(BridalProduct product)
        {
            const string sql = @"
                UPDATE dbo.BridalProducts
                SET Title = @Title,
                    [Description] = @Description,
                    [Image] = @Image,
                    Price = @Price,
                    Quantity = @Quantity,
                    [Status] = @Status
                WHERE Id = @Id;";

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Id", product.Id);
            command.Parameters.AddWithValue("@Title", product.Title);
            command.Parameters.AddWithValue("@Description", (object?)product.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@Image", (object?)product.Image ?? DBNull.Value);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@Quantity", product.Quantity);
            command.Parameters.AddWithValue("@Status", product.Status);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM dbo.BridalProducts WHERE Id = @Id;";

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }
}