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
        
        public async Task Add(User entity)
        {
            var user = Mapper.Map<Entities.User>(entity);
            await Add(user);
            entity.Id = user.Id;
        }
        
        public async Task Update(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int id)
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

        private async Task Add(Entities.User user)
        {
            using (var context = new TrueMoneyContext())
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
        }
    }
}
