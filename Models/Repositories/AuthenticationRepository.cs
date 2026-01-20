using BnsBazarApp.Models.Data;
using BnsBazarApp.Models.DTOs;

namespace BnsBazarApp.Models.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly BnsBazarDbContext _context;

        public AuthenticationRepository(BnsBazarDbContext context)
        {
            _context = context;
        }

        public AppUser Authenticate(LoginDTO loginDTO)
        {
            var user = _context.AppUsers
                .FirstOrDefault(u => u.Email == loginDTO.Email);

            if (user == null)
                return null;

            if (BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
                return user;

            return null;
        }

        public AppUser CreateUser(CreateUserDTO userDTO)
        {
            if (_context.AppUsers.Any(u => u.Email == userDTO.Email))
                return null;

            var user = new AppUser
            {
                Email = userDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password),
                Role = "User"
            };

            _context.AppUsers.Add(user);
            _context.SaveChanges();
            return user;
        }
    }
}