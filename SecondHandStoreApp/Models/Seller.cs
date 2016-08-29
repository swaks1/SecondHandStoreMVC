using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondHandStoreApp.Models
{
    public class Seller
    {
        public int ID { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string TransactionNum { get; set; }

        public string Name { get; set; }

        public virtual ICollection<StoreItem> SellingItems { get; set; }
    }
}
