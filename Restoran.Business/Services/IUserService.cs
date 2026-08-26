using Restoran.Data.Entities;
using Restoran.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restoran.Business.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(string userName,string password,RoleType roleType);
        Task<User> LoginAsync(string userName,string password);
    }

}
