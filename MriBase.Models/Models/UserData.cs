using System;
using System.Collections.Generic;

namespace MriBase.Models.Models
{
    [Serializable]
    public class UserData
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<TimedTraining> DailyTrainings { get; set; }

        public UserData(string userName, string password)
        {
            this.DailyTrainings = new List<TimedTraining>();
            UserName = userName;
            Password = password;
        }

        public UserData()
        {
            this.DailyTrainings = new List<TimedTraining>();
        }
    }
}