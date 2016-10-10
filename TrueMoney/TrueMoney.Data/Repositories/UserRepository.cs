using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TrueMoney.Infrastructure.Entities;
using TrueMoney.Infrastructure.Repositories;

namespace TrueMoney.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> GetById(int id)
        {
            var user = await LoadById(id);
            return Mapper.Map<User>(user);
        }
            
        public async Task<IList<User>> GetAll()
        {
            var users = await LoadAll();
            return Mapper.Map<IEnumerable<User>>(users).ToList();
        }

        public Task<User> GetUserByName(string name)
        {
            throw new NotImplementedException();
        }

        private async Task<Entities.User> LoadById(int id)
        {
            using (var context = new TrueMoneyContext())
            {
                return await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        private async Task<List<Entities.User>> LoadAll()
        {
            using (var context = new TrueMoneyContext())
            {
                return await context.Users.ToListAsync();
            }
        }
    }
}
