using BnsBazarApp.Models.DTOs;
using BnsBazarApp.Models.Data;
namespace BnsBazarApp.Models.Repositories
{
    public interface IAuthenticationRepository
    {
        AppUser Authenticate(LoginDTO loginDTO);
        AppUser CreateUser(CreateUserDTO userDTO);
    }
}
