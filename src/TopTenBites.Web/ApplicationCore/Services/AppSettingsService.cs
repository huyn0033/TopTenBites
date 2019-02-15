using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Interfaces;

namespace TopTenBites.Web.ApplicationCore.Services
{
    public class AppSettingsService : IAppSettingsService
    {
        public string YelpApiKey { get; set; }
        public string GoogleMapsApiKey { get; set; }
        public string UploadsPath { get; set; }
        public string UploadsVirtualDirectory { get; set; }
        public string Error404ViewPath { get; set; }
        public string Error500ViewPath { get; set; }
    }
}
