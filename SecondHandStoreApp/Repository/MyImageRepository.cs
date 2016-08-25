using SecondHandStoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecondHandStoreApp.Repository
{
    public class MyImageRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        StoreItemRepository _storeItemRepo = new StoreItemRepository();

        public List<MyImage> GetAll()
        {
            return db.MyImages.ToList();
        }

        public MyImage GetById(int id)
        {
            return GetAll().FirstOrDefault(si => si.ID == id);
        }

        public List<MyImage> GetImagesForItem(int id)
        {
            return GetAll().Where(i => i.StoreItemId == id).ToList();
        }

        public bool Delete(int id)
        {
            var img = GetById(id);
            db.MyImages.Remove(img);
            db.SaveChanges();
            return true;
            
        }



    }
}