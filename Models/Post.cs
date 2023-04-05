using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Datyche.Models
{
    public class Post
    {
        public ObjectId Id { get; set; }

        [Required(ErrorMessage = "Please, enter the title")]
        public string Title { get; set; }

        public string? Description { get; set; }
        public ObjectId Author { get; set; }
        public DateTime Date { get; set; }
        public string[]? Tags { get; set; }
        public byte[][]? Files { get; set; }

        public Post() {}
        public Post(ObjectId id, string title, string description, ObjectId author, string[] tags, byte[][] files)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.Author = author;
            this.Date = DateTime.Now;
            this.Tags = tags;
            this.Files = files;
        }
    }
}
