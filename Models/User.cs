namespace Datyche.Models
{
    public class User
    {
        private int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public User(string email, string username, string password) {
            this.Email = email;
            this.Name = username;
            this.Password = password;
        }
    }
}
