namespace api.Repositories
{
    using api.Models;
    using Npgsql;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MessageRepository : IMessageRepository
    {
        private readonly string _connectionString;

        public MessageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddMessageAsync(Message message)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = new NpgsqlCommand("INSERT INTO Messages (MessageText, Timestamp) VALUES (@text, @timestamp)", connection))
                {
                    cmd.Parameters.AddWithValue("text", message.MessageText);
                    cmd.Parameters.AddWithValue("timestamp", DateTime.UtcNow);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Message>> GetMessagesAsync()
        {
            var messages = new List<Message>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT * FROM Messages ORDER BY Timestamp DESC", connection))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            messages.Add(new Message
                            {
                                Id = reader.GetInt32(0),
                                MessageText = reader.GetString(1),
                                Timestamp = reader.GetDateTime(2)
                            });
                        }
                    }
                }
            }
            return messages;
        }
    }


}
