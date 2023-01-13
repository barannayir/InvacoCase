using InvacoCase.Data;
using InvacoCase.Models;

namespace InvacoCase.Interfaces
{
    public interface IUserRepository
    {
        public IQueryable<User> GetAll();
        public Task<User> GetById(string id);
        public Task Add(User user);
        public Task Update(User user);
        public Task Delete(string id);
        



    }
}
