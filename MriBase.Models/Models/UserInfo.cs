namespace MriBase.Models.Models
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserInfo(int userId, string email, string username, string firstName, string lastName)
        {
            this.UserId = userId;
            this.Email = email;
            this.Username = username;
            this.FirstName = firstName;
            this.LastName = lastName;
        }
    }
}
