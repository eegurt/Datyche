using System.ComponentModel.DataAnnotations;

namespace Datyche.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, enter the title")]
        public string Title { get; set; } = default!;

        public string? Description { get; set; }
        public int Author { get; set; }
        public DateTime DateCreated { get; set; } // TODO: ToLocalTime() when rendering page
        public string[]? Tags { get; set; }
        public byte[][]? Files { get; set; } // FIXME: Exclude this field (many-to-many relation) make bridge table idk
    }
}
