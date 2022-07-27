using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Datyche.Models
{
    public class User
    {
        public ObjectId Id { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4)]
        [RegularExpression(@"[a-zA-Z]+[\w]*")]
        public string? Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string? Password { get; set; }

        public User() {}
    }
}
