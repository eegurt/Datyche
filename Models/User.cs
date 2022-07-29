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
        [RegularExpression(@"[a-zA-Z]{1}[\w]*")] // starts with letter, includes any word character [a-zA-Z0-9_]
        public string? Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8)]
        [RegularExpression(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[\S]+")] // at least 1 lowercase letter, 1 uppercase letter, 1 digit, 1 or more of any non-space characters
        public string? Password { get; set; }

        public User() {}
    }
}
