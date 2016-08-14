using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondHandStore.Entities
{
    public class Seller
    {
        public string TransactionNum { get; set; }

        public virtual ICollection<StoreItem> SellingItems { get; set; }
    }
}
