using Restoran.Data.Entities;
using Restoran.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restoran.Data.Repository

{
    public interface IUserRepository:IGenericRepository<User> 
    {
        Task<User?> GetByUserNameAsync(string username);
    }
}
