using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopTenBites.Web.ApplicationCore.Models
{
    public class AppSettingsOptions
    {
        public string YelpApiKey { get; set; }
        public string GoogleMapsApiKey { get; set; }
        public string SendGridApiKey { get; set; }
        public string UploadsPath { get; set; }
        public string UploadsVirtualDirectory { get; set; }
        public string Error404ViewPath { get; set; }
        public string Error500ViewPath { get; set; }
        public string EmailAddress { get; set; }
        public string ApplicationName { get; set; }
    }
}
