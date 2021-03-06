﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Infrastructure.Helpers;
using DAL.Interfaces;
using DAL.Models;
using Serilog;
using Z.EntityFramework.Plus;

namespace DAL.Repositories
{
    public class UsersRepository : 
        IWritableRepository<User, int>, 
        IQueryableRepository<User, int>
    {
        private readonly ApplicationContext _db;
        
        public UsersRepository(ApplicationContext dbContext)
        {
            _db = dbContext;
        }

        #region IQueryableRepository

        public IQueryable<User> GetQueryable()
        {
            return _db.Users.AsQueryable();
        }

        public async Task<User> FindAsync(int id)
        {
            return await _db.Users.FindAsync(id);
        }


        #endregion IQueryableRepository

        #region IRepository

        public async Task InsertAsync(User entityToInsert)
        {
            await _db.Users.AddAsync(entityToInsert);
        }

        public async Task UpdateAsync(int id, User entityToUpdate)
        {
            await _db.Users
               .Where(i => i.Id == id)
               .UpdateFromQueryAsync(_ => new User
                {
                    Name = entityToUpdate.Name
                });
        }

        public async Task UpdateSpecificAsync(int id, Dictionary<string, object> dictionary)
        {
            await _db.Users
                .Where(i => i.Id == id)
                .UpdateFromQueryAsync(ReposHelper.UpdateSpecificFields<User>(dictionary));
        }

        public async Task DeleteAsync(int id)
        {
            await _db.Users
                .Where(i => i.Id == id)
                .DeleteFromQueryAsync();

        }
        

        #endregion IRepository

        
    }
}
