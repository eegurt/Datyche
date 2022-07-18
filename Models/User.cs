namespace Datyche.Models
{
    public class User
    {
        private int Id { get; set; }
        private string Email { get; set; }
        private string Name { get; set; }
        private string Password { get; set; }

        public User(string email, string username, string password) {
            this.Email = email;
            this.Name = username;
            this.Password = password;
        }
    }
}
