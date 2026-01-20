using System.ComponentModel.DataAnnotations;

namespace BnsBazarApp.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        
        public string Email { get; set; } = string.Empty;

        // 🔐 Hashed password (NOT plain text)
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // 👤 Role for authorization
        [Required]
        
        public string Role { get; set; } = "User"; // Default role

        // 📅 Optional audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}