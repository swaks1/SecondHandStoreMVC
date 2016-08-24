using SecondHandStoreApp.Interfaces;
using SecondHandStoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecondHandStoreApp.Repository
{
    public class SellerRepository : IGenericInterface<Seller>
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public bool Create(Seller obj)
        {
            db.Sellers.Add(obj);
            db.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            Seller s = GetById(id);
            s.IsActive = false;
            db.SaveChanges();
            return true;
        }

        public List<Seller> GetAll()
        {
            return db.Sellers.ToList();
        }

        public Seller GetById(int id)
        {
            return GetAll().FirstOrDefault(s => s.ID == id);
        }

        public bool Update(Seller obj)
        {
            Seller s = GetById(obj.ID);
            if (s == null)
                return false;

            s.TransactionNum = obj.TransactionNum;
            s.IsActive = obj.IsActive;
            s.Name = obj.Name;
            db.SaveChanges();
            return true;
        }

        public bool AddSellingItem(int sId,StoreItem si)
        {
            Seller s = GetById(sId);
            if (s == null)
                return false;
            s.SellingItems.Add(si);
            db.SaveChanges();
            return true;
        }

        public List<StoreItem> getSellingItemsForSeller(Seller obj)
        {
            return GetById(obj.ID).SellingItems.ToList();
        }





    }
}