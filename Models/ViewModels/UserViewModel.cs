namespace Datyche.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

        public UserViewModel(string id, string email, string username)
        {
            this.Id = id;
            this.Email = email;
            this.Username = username;
        }
    }
}