using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PicturesAPI.Models
{
    public class ItemModel
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string CategoryName { get; set; }

        public string BrandName { get; set; }
        public string Size { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string CurrencySymbol { get; set; }
        public string MainImageUrl { get; set; }
    }
}