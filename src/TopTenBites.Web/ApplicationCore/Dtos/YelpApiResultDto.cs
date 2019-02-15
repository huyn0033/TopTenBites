using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopTenBites.Web.ApplicationCore.Dtos
{
    public class YelpApiResultDto
    {
        public bool IsSuccessStatusCode { get; set; }
        public int StatusCode { get; set; }
        public string Result { get; set; }
        public string Query { get; set; }
        public string Url { get; set; }
    }
}
