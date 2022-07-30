using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Datyche.Models
{
    public class User
    {
        public ObjectId Id { get; set; }

        [Required(ErrorMessage = "Обязательно к заполнению")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Обязательно к заполнению")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Никнейм должен иметь от 4 до 20 символов включительно")]
        // starts with letter, includes any word characters
        [RegularExpression(@"[a-zA-Z]{1}[\w]*", ErrorMessage = "Никнейм должен начинаться с буквы. Допустимые символы: буквы, цифры, нижнее подчеркивание")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Обязательно к заполнению")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Пароль должен иметь от 8 до 20 символов включительно")]
        // at least 1 lowercase letter, 1 uppercase letter, 1 digit, 1 or more of any non-space characters
        [RegularExpression(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[\S]+", ErrorMessage = "Пароль должен содержать как минимум 1 строчную, 1 заглавную буквы и 1 цифру.")]
        public string? Password { get; set; }

        public User() { }
    }
}
