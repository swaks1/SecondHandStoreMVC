﻿using SecondHandStoreApp.Models;
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
            return GetAll().FirstOrDefault(u => u.MyUser.ID == id);
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

        public bool MakeSeller(MyUser obj, Seller s)
        {
            ApplicationUser u = GetById(obj.ID);

            u.MyUser.seller = s;

            db.SaveChanges();
            return true;
        }

        public List<StoreItem> GetSellingItems(MyUser u)
        {
            ApplicationUser dbU = GetById(u.ID);
            return _sellerRepository.getSellingItemsForSeller(dbU.My.seller);
        }

    }
}