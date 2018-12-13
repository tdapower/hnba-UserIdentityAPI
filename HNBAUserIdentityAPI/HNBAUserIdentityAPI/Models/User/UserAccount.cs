using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HNBAUserIdentityAPI.Models.User
{
    public class UserAccount
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserDisplayName { get; set; }
        public string Company { get; set; }
        public string BranchCode { get; set; }
        public string Epf { get; set; }
        public int UserRoleCode { get; set; }
        public int UserStatus { get; set; }
        public bool IsTravelEntitled { get; set; }
        public string UserOtherBranches { get; set; }
        public string PasswordStatus { get; set; }
        public string PasswordLastUpdatedDate { get; set; }
        
    }
}