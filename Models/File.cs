using System.Text.Json.Serialization;

namespace Datyche.Models
{
    public class File
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public Post Post { get; set; } = null!;
    }
}