using System.ComponentModel.DataAnnotations;

namespace Datyche.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required field")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required field")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Username must be 4 to 20 symbols length")]
        // starts with letter, includes any word characters
        [RegularExpression(@"[a-zA-Z]{1}[\w]*", ErrorMessage = "Username must start with a letter. Valid characters: letters, digits, underscore(_)")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required field")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be 8 to 20 symbols length")]
        // at least 1 lowercase letter, 1 uppercase letter, 1 digit, 1 or more of any non-space characters
        [RegularExpression(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[\S]+", ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter and 1 digit")]
        public string Password { get; set; }

        public User(int id, string email, string username, string password) {
            this.Id = id;
            this.Email = email;
            this.Username = username;
            this.Password = password;
        }
    }
}
