﻿using SecondHandStoreApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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

        public ApplicationUser GetAppUser(string id)
        {
            var dbUser = db.Users.FirstOrDefault(u => u.Id == id);

            return dbUser;
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



        public bool AddToOrders(string userId)
        {
            ApplicationUser dbUser = db.Users.FirstOrDefault(u => u.Id == userId);
            var cartItems = dbUser.MyUser.shopingCart.ToList();
            try
            {
                foreach (var cartItem in cartItems)
                {
                    StoreItem dbItem = db.StoreItems.FirstOrDefault(s => s.ID == cartItem.ID);
                    dbUser.MyUser.shopingCart.Remove(dbItem);
                    var delivery = new Delivery()
                    {
                        Date = DateTime.Now,
                        itemId = cartItem.ID,
                        BuyerId = dbUser.MyUser.ID,
                        item = cartItem
                    };

                    db.Deliverys.Add(delivery);
                    dbItem.isSold = true;
                    db.SaveChanges();

                    dbItem.orderId = delivery.ID;
                }
            
         
            db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return true;
        }

      



    }
}