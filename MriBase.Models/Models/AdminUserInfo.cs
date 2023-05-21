using MriBase.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MriBase.Models.Models
{
    public class AdminUserInfo : UserInfo
    {
        public AdminUserInfo(int userId, string email, string username, string firstName, string lastName, List<(GroupInfo Group, GroupRole Role)> groups, List<UserRole> userRoles) : base(userId, email, username, firstName, lastName)
        {
            this.GroupMemberships = groups;
            this.UserRoles = userRoles;
        }

        public List<(GroupInfo Group, GroupRole Role)> GroupMemberships { get; set; }

        public List<UserRole> UserRoles { get; set; }
    }
}
