using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationFramework.Models
{
    public class UserModel
    {
        public string userName { get; set; }
        public string password { get; set; }
    }

    public class UserResponse
    {
        public string userID { get; set; }
        public string username { get; set; }
        public List<object> books { get; set; }
    }

    public class TokenResponse
    {
        public string token { get; set; }
        public string expires { get; set; }
        public string status { get; set; }
        public string result { get; set; }
    }
}
