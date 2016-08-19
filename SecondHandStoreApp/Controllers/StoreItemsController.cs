using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SecondHandStoreApp.Models;

namespace SecondHandStoreApp.Controllers
{
    public class StoreItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StoreItems
        public ActionResult Index()
        {
            var storeItems = db.StoreItems.Include(s => s.seller);
            return View(storeItems.ToList());
        }

        // GET: StoreItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItem storeItem = db.StoreItems.Find(id);
            if (storeItem == null)
            {
                return HttpNotFound();
            }
            return View(storeItem);
        }

        // GET: StoreItems/Create
        public ActionResult Create()
        {
            ViewBag.SellerId = new SelectList(db.Sellers, "ID", "TransactionNum");
            return View();
        }

        // POST: StoreItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IsAvailable,DateCreated,ItemName,Price,itemGender,category,subcategoryClothes,subcategoryBags,subcategoryShoes,subcategoryAccessories,Description,condition,material,size,shoeSize,Brand,length,width,SellerId")] StoreItem storeItem)
        {
            if (ModelState.IsValid)
            {
                db.StoreItems.Add(storeItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SellerId = new SelectList(db.Sellers, "ID", "TransactionNum", storeItem.SellerId);
            return View(storeItem);
        }

        // GET: StoreItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItem storeItem = db.StoreItems.Find(id);
            if (storeItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.SellerId = new SelectList(db.Sellers, "ID", "TransactionNum", storeItem.SellerId);
            return View(storeItem);
        }

        // POST: StoreItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IsAvailable,DateCreated,ItemName,Price,itemGender,category,subcategoryClothes,subcategoryBags,subcategoryShoes,subcategoryAccessories,Description,condition,material,size,shoeSize,Brand,length,width,SellerId")] StoreItem storeItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storeItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SellerId = new SelectList(db.Sellers, "ID", "TransactionNum", storeItem.SellerId);
            return View(storeItem);
        }

        // GET: StoreItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItem storeItem = db.StoreItems.Find(id);
            if (storeItem == null)
            {
                return HttpNotFound();
            }
            return View(storeItem);
        }

        // POST: StoreItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StoreItem storeItem = db.StoreItems.Find(id);
            db.StoreItems.Remove(storeItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
