using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TrueMoney.Data.Entities;

namespace TrueMoney.Data.Repositories
{
    public class UserRepository
    {
        #region Public
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
            using (var context = new TrueMoneyContext())
            {
                var dbUser = context.Users.First(x => x.Id == entity.Id);
                Mapper.Map(entity, dbUser);

                await context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByAspId(string aspId)
        {
            if (string.IsNullOrEmpty(aspId))
            {
                throw new ArgumentNullException(nameof(aspId));
            }

            var user = await LoadByAspId(aspId);
            return Mapper.Map<User>(user);
        }
        #endregion

        #region Private
        private async Task<Entities.User> LoadById(int id)
        {
            using (var context = new TrueMoneyContext())
            {
                return await context.Users.Include(x => x.Passport).FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        private async Task<List<Entities.User>> LoadAll()
        {
            using (var context = new TrueMoneyContext())
            {
                return await context.Users.ToListAsync();
            }
        }

        //private Task Update(Entities.User user)
        //{
        //    using (var context = new TrueMoneyContext())
        //    {
        //        context.Users.Attach;
        //        await context.SaveChangesAsync();
        //    }
        //}

        private async Task<Entities.User> LoadByAspId(string aspId)
        {
            using (var context = new TrueMoneyContext())
            {
                return await context.Users.FirstOrDefaultAsync(x => x.AspUserId == aspId);
            }
        } 
        #endregion
    }
}
