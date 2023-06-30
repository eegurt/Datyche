using System.Text.Json.Serialization;

namespace Datyche.Models
{
    public class File
    {
        public int Id { get; set; }

        public string Path { get; set; } = null!;

        [JsonIgnore]
        public IList<Post>? Posts { get; set; }
    }
}