using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SecondHandStoreApp.Models
{
    public class Delivery
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }

        public virtual int? BuyerId { get; set; }
        public virtual MyUser Buyer { get; set; }

        public virtual int? itemId { get; set; }
        [Required]
        public virtual StoreItem item { get; set; }
    }
}