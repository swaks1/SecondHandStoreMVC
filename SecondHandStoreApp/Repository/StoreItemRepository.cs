using SecondHandStoreApp.Interfaces;
using SecondHandStoreApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SecondHandStoreApp.Repository
{
    public class StoreItemRepository : IGenericInterface<StoreItem>
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public bool Create(StoreItem obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.IsAvailable = true;
            obj.IsApproved = false;
            db.StoreItems.Add(obj);
            db.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            StoreItem si = GetById(id);
            si.IsAvailable = false;
            db.SaveChanges();
            return true;
        }

        public List<StoreItem> GetAll()
        {
            return db.StoreItems.ToList();
        }

        public StoreItem GetById(int id)
        {
            return GetAll().FirstOrDefault(si => si.ID == id);
        }

        public bool Update(StoreItem obj)
        {
            //var dbObj = GetById(obj.ID);
            //if (dbObj == null)
            //    return false;

            //dbObj.ItemName = obj.ItemName;
            //dbObj.Price = obj.Price;
            //dbObj.Description = obj.Description;
            //dbObj.Brand = obj.Brand;

            db.Entry(obj).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        public List<StoreItem> GetAllApproved()
        {
            return GetAll().FindAll(s => s.IsApproved == true && s.IsAvailable == true);
        }


        public List<StoreItem> GetAllUnapproved()
        {
            return GetAll().FindAll(s => s.IsApproved == false && s.IsAvailable == true);
        }




    }
}