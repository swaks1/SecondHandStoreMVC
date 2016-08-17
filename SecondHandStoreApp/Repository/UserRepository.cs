using SecondHandStoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecondHandStoreApp.Repository
{
    public class UserRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();
        SellerRepository _sellerRepository = new SellerRepository();

        {
        }

        {
        }

        public bool Update(MyUser obj)
        {
                return false;


            db.SaveChanges();
            return true;
        }

        public bool MakeSeller(MyUser obj, Seller s)
        {


            db.SaveChanges();
            return true;
        }

        public List<StoreItem> GetSellingItems(MyUser u)
        {
        }

    }
}
