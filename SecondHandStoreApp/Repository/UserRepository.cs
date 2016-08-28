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

        public List<ApplicationUser> GetAll()
        {
            return db.Users.ToList();
        }

        public ApplicationUser GetById(int id)
        {
            return db.Users.FirstOrDefault(u => u.MyUser.ID == id);
        }

        public bool Update(MyUser obj)
        {
            ApplicationUser u = GetById(obj.ID);
            if (u == null)
                return false;

            u.MyUser.Address = obj.Address;
            u.MyUser.City = obj.City;
            u.MyUser.FullName = obj.FullName;

            db.SaveChanges();
            return true;
        }

        public bool MakeSeller(int id, string transactionNum, string fullName)
        {
            ApplicationUser user = GetById(id);

            user.MyUser.seller = new Seller() { TransactionNum = transactionNum, Name = fullName };
            user.MyUser.seller.IsActive = true;

            db.SaveChanges();
            return true;
        }

        public bool UpdateSeller(int userId, string transactionNum)
        {
            ApplicationUser user = GetById(userId);

            user.MyUser.seller.TransactionNum = transactionNum;
            user.MyUser.seller.IsActive = true;

            db.SaveChanges();
            return true;
        }
        public MyUser GetMyUser(string id)
        {
            var dbUser = db.Users.FirstOrDefault(u => u.Id == id);

            return dbUser.MyUser;
        }



        public List<StoreItem> GetSellingItems(int id)
        {
            ApplicationUser dbU = GetById(id);
            return _sellerRepository.getSellingItemsForSeller(dbU.MyUser.seller.ID);
        }

        public bool AddItemToShopingCart(string userId, int itemId)
        {
            ApplicationUser dbUser = db.Users.FirstOrDefault(u => u.Id == userId);
            StoreItem dbUtem = db.StoreItems.FirstOrDefault(s => s.ID == itemId);
            dbUser.MyUser.shopingCart.Add(dbUtem);

            db.SaveChanges();

            return true;
        }

        public bool DeleteItemFromShopingCart(string userId, int itemId)
        {
            ApplicationUser dbUser = db.Users.FirstOrDefault(u => u.Id == userId);
            StoreItem dbUtem = db.StoreItems.FirstOrDefault(s => s.ID == itemId);
            dbUser.MyUser.shopingCart.Remove(dbUtem);

            db.SaveChanges();

            return true;
        }

      



    }
}