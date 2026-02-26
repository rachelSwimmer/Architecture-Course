using server.Models;

namespace server.Interfaces
{
    public interface IBuyerRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        
    }
}
