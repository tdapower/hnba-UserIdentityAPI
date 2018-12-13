using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HNBAUserIdentityAPI.Models.User
{
    public class UserMini
    {
        public string UserName { get; set; }
        public string Company { get; set; }
        public string Password { get; set; }

        public string NewPassword { get; set; }
    }
}