using System.ComponentModel.DataAnnotations;

namespace Datyche.Models
{
    public class BufferedFilesUploadDb
    {
        public Post Post { get; set; }
        
        [Required]
        [Display(Name = "Files")]
        public List<IFormFile> FormFiles { get; set; }
    }
}