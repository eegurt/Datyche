using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Datyche.Models
{
    public class Post
    {
        public ObjectId Id { get; set; }

        [Required(ErrorMessage = "Please, enter the title")]
        public string? Title { get; set; }

        public string? Description { get; set; }
        public ObjectId Author { get; set; }
        public DateTime Date { get; set; }
        public string[]? Tags { get; set; }
        public byte[][]? Files { get; set; }
    }
}
