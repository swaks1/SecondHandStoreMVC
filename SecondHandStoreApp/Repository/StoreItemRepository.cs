using SecondHandStoreApp.Interfaces;
using SecondHandStoreApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace SecondHandStoreApp.Repository
{
    public class StoreItemRepository : IGenericInterface<StoreItem>
    {
        ApplicationDbContext db = new ApplicationDbContext();
        UserRepository _userRepository = new UserRepository();

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

        public IQueryable<StoreItem> Filter(Expression<Func<StoreItem,bool>> predicate)
        {
            var result = db.StoreItems
                    .Where(predicate)
                    .Where(s => s.IsApproved == true && s.IsAvailable == true);
            return result;
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
            var dbObj = GetById(obj.ID);
            if (dbObj == null)
                return false;
     
            dbObj.ItemName = obj.ItemName;
            dbObj.Price = obj.Price;
            dbObj.Description = obj.Description;
            dbObj.Brand = obj.Brand;
            dbObj.SellerId = obj.SellerId;
            dbObj.category = obj.category;
            dbObj.condition = obj.condition;
            dbObj.DateCreated = obj.DateCreated;
            dbObj.HelperImagePaths = obj.HelperImagePaths;
            dbObj.IsApproved = obj.IsApproved;
            dbObj.IsAvailable = obj.IsAvailable;
            dbObj.itemGender = obj.itemGender;
            dbObj.ItemName = obj.ItemName;
            dbObj.length = obj.length;
            dbObj.material = obj.material;
            dbObj.SellerId = obj.SellerId;
            dbObj.shoeSize = obj.shoeSize;
            dbObj.size = obj.size;
            dbObj.subcategoryAccessories = obj.subcategoryAccessories;
            dbObj.subcategoryBags = obj.subcategoryBags;
            dbObj.subcategoryClothes = obj.subcategoryClothes;
            dbObj.subcategoryShoes = obj.subcategoryShoes;
            dbObj.width = obj.width;
           

            foreach (string img in dbObj.HelperImagePaths)
            {
                dbObj.Images.Add(new MyImage { Image = img, StoreItemId = dbObj.ID });
            }

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

        public List<StoreItem> GetItemsForUser(int id)
        {
            var user = _userRepository.GetById(id);
            return GetAllApproved().FindAll(s => s.SellerId == user.MyUser.SellerID);
        }

        public bool ApproveItem(int id)
        {
            var dbItem = GetById(id);
            dbItem.IsApproved = true;
            db.SaveChanges();

            return true;
        }
    }
}