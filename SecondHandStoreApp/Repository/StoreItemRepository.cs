using SecondHandStoreApp.Interfaces;
using SecondHandStoreApp.LuceneNetSearch;
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

        public bool DeleteItem (int id)
        {
            var dbItem = GetById(id);

            //get list of the Images..else you will have colleciton has modified error
            var listImages = dbItem.Images.ToList();

            foreach(var img in listImages)
            {
                var dbImg = db.MyImages.FirstOrDefault(i => i.ID == img.ID);
                db.MyImages.Remove(dbImg);
            }

            db.SaveChanges();

            //delete the Lucene Index
            LuceneSearch.ClearLuceneIndexRecord(dbItem.ID);

            db.StoreItems.Remove(dbItem);

            db.SaveChanges();

            return true;
        }

        public bool DisableItem(int id)
        {
            StoreItem si = GetById(id);
            si.IsAvailable = false;
            db.SaveChanges();

            //delete the Lucene Index
            LuceneSearch.ClearLuceneIndexRecord(si.ID);

            return true;
        }

        public bool EnableItem(int id)
        {
            StoreItem si = GetById(id);
            si.IsAvailable = true;
            db.SaveChanges();

            //add the item to Lucene Index too
            LuceneSearch.AddUpdateLuceneIndex(new List<StoreItem> { si });

            return true;
        }

        public IQueryable<StoreItem> Filter(Expression<Func<StoreItem,bool>> predicate)
        {
            var result = db.StoreItems
                    .Where(s => s.IsApproved == true && s.IsAvailable == true)
                    .Where(predicate);        
            return result;
        }

        public List<StoreItem> GetAll()
        {
            return db.StoreItems.Where(i => i.IsFinished).ToList();
        }

        public StoreItem GetById(int id)
        {
            return db.StoreItems.FirstOrDefault(i => i.ID == id);
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
            dbObj.category = obj.category;
            dbObj.condition = obj.condition;
            dbObj.HelperImagePaths = obj.HelperImagePaths;
            dbObj.itemGender = obj.itemGender;
            dbObj.ItemName = obj.ItemName;
            dbObj.length = obj.length;
            dbObj.material = obj.material;
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

            if (dbObj.IsApproved)
            {
                //add the item to Lucene Index too
                LuceneSearch.AddUpdateLuceneIndex(new List<StoreItem> { dbObj });
            }
            

            return true;
        }

        public bool UpdateStep1(StoreItem obj)
        {
            var dbObj = GetById(obj.ID);
            if (dbObj == null)
                return false;

            dbObj.ItemName = obj.ItemName;
            dbObj.Price = obj.Price;
            dbObj.category = obj.category;
            dbObj.itemGender = obj.itemGender;
            dbObj.subcategoryAccessories = obj.subcategoryAccessories;
            dbObj.subcategoryBags = obj.subcategoryBags;
            dbObj.subcategoryClothes = obj.subcategoryClothes;
            dbObj.subcategoryShoes = obj.subcategoryShoes;

            db.SaveChanges();

            if (dbObj.IsApproved)
            {
                //add the item to Lucene Index too
                LuceneSearch.AddUpdateLuceneIndex(new List<StoreItem> { dbObj });
            }

            return true;
        }

        public bool UpdateStep2(StoreItem obj)
        {
            var dbObj = GetById(obj.ID);
            if (dbObj == null)
                return false;

            dbObj.Description = obj.Description;
            dbObj.Brand = obj.Brand;
            dbObj.condition = obj.condition;
            dbObj.material = obj.material;
            dbObj.shoeSize = obj.shoeSize;
            dbObj.size = obj.size;
            dbObj.width = obj.width;
            dbObj.length = obj.length;

            foreach (string img in obj.HelperImagePaths)
            {
                dbObj.Images.Add(new MyImage { Image = img, StoreItemId = dbObj.ID });
            }
            db.SaveChanges();

            if (dbObj.IsApproved)
            {
                //add the item to Lucene Index too
                LuceneSearch.AddUpdateLuceneIndex(new List<StoreItem> { dbObj });
            }


            return true;
        }

        public bool UpdateStep3(int storeItemID, int sellerID)
        {
            var dbObj = GetById(storeItemID);
            if (dbObj == null)
                return false;
            dbObj.SellerId = sellerID;      
            db.SaveChanges();

            return true;
        }

        public bool updateStep4(int storeItemID)
        {
            var dbObj = GetById(storeItemID);
            if (dbObj == null)
                return false;
            dbObj.IsFinished = true;

            db.SaveChanges();
            return true;
        }



        public List<StoreItem> GetAllApproved()
        {
            return GetAll().FindAll(s => s.IsApproved == true && s.IsAvailable == true);
        }


        public List<StoreItem> GetAllUnapproved()
        {
            return GetAll().FindAll(s => s.IsApproved == false && s.IsAvailable == true && s.IsFinished);
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

            //add the item to Lucene Index too
            LuceneSearch.AddUpdateLuceneIndex(new List<StoreItem> { dbItem });

            return true;
        }

        public List<StoreItem> GetPopular()
        {
            var items = db.StoreItems.Where(p => p.IsApproved && p.IsAvailable && p.IsFinished).Take(9);

            return items.ToList();
        }

        
    }
}