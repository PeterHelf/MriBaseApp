using System;
using System.Collections.Generic;
using System.Text;

namespace MriBase.Models.Models
{
    public class GroupInfo
    {
        public int GroupId { get; }

        public string GroupName { get; }

        public GroupInfo(int groupId, string groupName)
        {
            this.GroupId = groupId;
            this.GroupName = groupName;
        }
    }
}
