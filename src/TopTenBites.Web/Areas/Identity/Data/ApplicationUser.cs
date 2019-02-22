using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TopTenBites.Web.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData, MaxLength(128)]
        public string FirstName { get; set; }
        [PersonalData, MaxLength(128)]
        public string LastName { get; set; }
    }
}
