using System.ComponentModel.DataAnnotations;

namespace BnsBazarApp.Models.DTOs
{
    public class CreateUserDTO
    {
        public string Email { get; set; } = string.Empty;
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[*.!@$%^&,?_-])[A-Za-z0-9*.!@$%^&,?_-]{10,15}$", 
            ErrorMessage ="Password must be between 10-15 characters long and have at least 1" +
                          "upper case chracter, 1 lower case character, 1 number and 1 of" +
                          "the following *.!@$%^&,?_-")]
        public string Password { get; set; } = string.Empty;
        public string PasswordConfirmation { get; set; } = string.Empty;
    }
}
