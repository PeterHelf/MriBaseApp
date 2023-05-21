using MriBase.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MriBase.Models.Models
{
    public class GroupRoleUserInfo
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GroupRole Role { get; set; }

        public GroupRoleUserInfo(int userId, string email, string username, string firstName, string lastName, GroupRole role)
        {
            this.UserId = userId;
            this.Email = email;
            this.Username = username;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Role = role;
        }
    }
}
