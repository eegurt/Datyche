using System.ComponentModel.DataAnnotations;

namespace Datyche.Models
{
    public class User
    {
        public int Id { get; set; } = 1;

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4)]
        [RegularExpression(@"[a-zA-Z]+[\w]*")]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string Password { get; set; }

        public User() { }
    }
}
