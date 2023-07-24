namespace Datyche.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string Author { get; set; }
        public DateTime DateCreated { get; set; }
        public IList<File>? Files { get; set; }

        public PostViewModel(int id, string title, string author, DateTime dateCreated)
        {
            this.Id = id;
            this.Title = title;
            this.Author = author;
            this.DateCreated = dateCreated;
        }

        public PostViewModel(int id, string title, string author, DateTime dateCreated, string description, IList<File> files)
        : this(id, title, author, dateCreated)
        {
            this.Description = description;
            this.Files = files;
        }
    }
}