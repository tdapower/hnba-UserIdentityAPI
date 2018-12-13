using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HNBAUserIdentityAPI.Models.User
{
    public class HNBASystem
    {
        public int SystemCode { get; set; }
        public string SystemName { get; set; }
        public int IsActive { get; set; }

    }
}