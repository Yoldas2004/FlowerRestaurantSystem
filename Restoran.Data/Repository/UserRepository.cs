using Microsoft.EntityFrameworkCore;
using Restoran.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restoran.Data.Repository
{
    public class UserRepository:GenericRepository<User>, IUserRepository
    {
         

        public UserRepository(RestoranDbContext  context):base(context)
        {
             
        }

       
        public async Task<User?> GetByUserNameAsync(string username) 
        {
          return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }




    }
}
