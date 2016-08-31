using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecondHandStoreApp.Models
{
    public class MyUser
    {
        public MyUser()
        {
            shopingCart = new List<StoreItem>();
            boughtItems = new List<Delivery>();
        }
        public int ID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string FullName { get; set; }

        public virtual int? SellerID { get; set; }
        public virtual Seller seller { get; set; }

        public virtual List<StoreItem> shopingCart { get; set; }

        public virtual ICollection<Delivery> boughtItems { get; set; }
    }
}