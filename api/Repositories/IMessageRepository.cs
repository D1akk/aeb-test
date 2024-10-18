namespace api.Repositories
{
    using api.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMessageRepository
    {
        Task AddMessageAsync(Message message);
        Task<List<Message>> GetMessagesAsync();
    }

}
