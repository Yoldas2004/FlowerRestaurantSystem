using Restoran.Data.Entities;
using Restoran.Data.Enums;
using Restoran.Data.Repository;
 

namespace Restoran.Business.Services
{
    public class UserService:   IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
             _userRepository = userRepository;
        }

        public async Task<User> RegisterAsync(string userName,string password,RoleType role )
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("Username bos olamaz");
            }
            if (string.IsNullOrWhiteSpace(password)) 
            {
                throw new ArgumentException("password bos olamaz");
            }
            
            int count = password.Length;
            if (count < 6)
            {
                throw new ArgumentException("Password 6 'dan kucuk olamaz");
            }
            var allUser = await _userRepository.GetByUserNameAsync(userName);
            if (allUser != null )
            {
                throw new ArgumentException("Bu kullanıcı adı zaten kayıtlı");
            }
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var newUser = new User {UserName = userName,PasswordHash = hashPassword , Role =role};
           
           
           await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();
            return newUser;
        }
        public async Task<User> LoginAsync(string userName, string password)
        { 
       
          var user = await _userRepository.GetByUserNameAsync(userName);

            if (user == null)
            {
                throw new ArgumentException("Kullanici bulunamadi ");
            }
            var isTruePassword = BCrypt.Net.BCrypt.Verify(password,user.PasswordHash );
            if (!isTruePassword)
            {
                throw new ArgumentException("Sifre yanlis ");
            }
            return user;
        }
    }
}
