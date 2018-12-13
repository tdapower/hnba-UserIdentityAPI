using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HNBAUserIdentityAPI.Models.User
{
    public class UserSystemRole
    {
        public string UserName { get; set; }
        public string CompanyCode { get; set; }
        public int SystemCode { get; set; }
        public int UserRoleCode { get; set; }
        public int IsAllowed { get; set; }
        


    }
}