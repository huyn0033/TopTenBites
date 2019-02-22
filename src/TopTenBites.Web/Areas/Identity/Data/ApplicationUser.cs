using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopTenBites.Web.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public string Address { get; set; }
        [PersonalData]
        public string City { get; set; }
        [PersonalData]
        public string State { get; set; }
        [PersonalData]
        public string ZipCode { get; set; }
    }
}
