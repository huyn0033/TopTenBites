using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace TopTenBites.Web.ViewModels
{
    [DataContract(Name = "Business")]
    public class YelpBusiness
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string alias { get; set; }
        [DataMember]
        public double rating { get; set; }
        [DataMember]
        public string price { get; set; }
        [DataMember]
        public string phone { get; set; }
        [DataMember]
        public string display_phone { get; set; }
        [DataMember]
        public bool is_closed { get; set; }
        [DataMember]
        public List<Category> categories { get; set; } = new List<Category>();
        [DataMember]
        public int review_count { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string image_url { get; set; }
        [DataMember]
        public Location location { get; set; }
        [DataMember]
        public Coordinates coordinates { get; set; }

        public List<MenuItemViewModel> menuItems { get; set; } = new List<MenuItemViewModel>();

        public string GetImageUrlListSize() => image_url?.Replace("o.jpg","ls.jpg");
        public string DisplayRating()
        {
            var s = string.Empty;
            if (this.rating > 4.9)
                s = "stars-5";
            else if (rating > 4.4)
                s = "stars-4_5";
            else if (rating > 3.9)
                s = "stars-4";
            else if (rating > 3.4)
                s = "stars-3_5";
            else if (rating > 2.9)
                s = "stars-3";
            else if (rating > 2.4)
                s = "stars-2_5";
            else if (rating > 1.9)
                s = "stars-2";
            else if (rating > 1.4)
                s = "stars-1_5";
            else if (rating > 0.75)
                s = "stars-1";
            else
                s = "stars-0";

            return $"<div class=\"ratings {s}\"></div>&nbsp;<div class=\"reviewcount\">{this.review_count} reviews</div>"; 
        }

        public string DisplayPriceCategories()
        {
            var categories = string.Empty;
            if (this.categories !=null)
            {
                categories = string.Join(", ", this.categories.Select(x => x.title).ToArray());
            }
            
            return $"{this.price} - {categories}";
        }
    }

    public class RootObject
    {
        public int total { get; set; }
        public List<YelpBusiness> businesses { get; set; } = new List<YelpBusiness>();
    }

    public class Category
    {
        public string alias { get; set; }
        public string title { get; set; }
    }

    public class Location
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string country { get; set; }
        public string[] display_address { get; set; }

        public string DisplayAddress() 
            => this.display_address == null ? string.Empty : string.Join("<br/>", this.display_address);
    }

    public class Coordinates
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
