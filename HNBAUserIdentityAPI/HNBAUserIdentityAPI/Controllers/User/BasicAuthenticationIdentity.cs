using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace HNBAUserIdentityAPI.Controllers.User
{
    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public BasicAuthenticationIdentity(string name, string password,string company) : base(name, "Basic")
        {
            this.Password = password;
            this.Company = company;
        }

        /// <summary>
        /// Basic Auth Password for custom authentication
        /// </summary>
        public string Password { get; set; }
        public string Company { get; set; }
    }
}