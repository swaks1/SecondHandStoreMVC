using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecondHandStoreApp.Models
{
    public class MyImage
    {
        public int ID { get; set; }
        public string Image { get; set; }

        public virtual int? StoreItemId { get; set; }
        public virtual StoreItem StoreItem { get; set; }
    }
}