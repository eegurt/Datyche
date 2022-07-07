namespace Datyche.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public string[] Tags { get; set; }
        public string Media { get; set; }
        public Post(string name, string author)
        {
            Id++;
            Name = name;
            Author = author;
            Date = DateTime.Now;
        }
    }
}
