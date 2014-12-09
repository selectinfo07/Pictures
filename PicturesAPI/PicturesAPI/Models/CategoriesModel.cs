using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PicturesAPI.Models
{
    public class CategoriesModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}