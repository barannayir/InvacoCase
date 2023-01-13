using InvacoCase.Interfaces;
using InvacoCase.Models;

namespace InvacoCase.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<User> GetAll()
        {
            return _context.Users;
        }
        public async Task<User> GetById(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(string id)
        {
            var user = await GetById(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
