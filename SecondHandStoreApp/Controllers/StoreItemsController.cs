using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SecondHandStoreApp.Models;
using SecondHandStoreApp.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.IO;

namespace SecondHandStoreApp.Controllers
{
    public class StoreItemsController : Controller
    {
        private StoreItemRepository _storeItemRepository = new StoreItemRepository();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: StoreItems
        public ActionResult Index()
        {
            //var storeItems = db.StoreItems.Include(s => s.seller);
            return View(_storeItemRepository.GetAllApproved());
        }

        // GET: StoreItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItem storeItem = _storeItemRepository.GetById((int)id);
            if (storeItem == null)
            {
                return HttpNotFound();
            }
            return View(storeItem);
        }

        [Authorize(Roles ="Seller")]
        // GET: StoreItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StoreItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase[] file, StoreItem storeItem)
        {
            if (ModelState.IsValid)
            {
                _storeItemRepository.Create(storeItem);

                string UserID = User.Identity.GetUserId();
                var appUser = UserManager.FindById(UserID);
                List<string> listImgPaths = new List<string>();

                //check if folder exist if not create it...
                var pathToImages = "~/Images/" + storeItem.ID + "/";
                bool exists = Directory.Exists(Server.MapPath(pathToImages));           
                if (!exists)
                   Directory.CreateDirectory(Server.MapPath(pathToImages));

                var count = 0;
                foreach (var image in file)
                {
                    count++;
                    if (image != null && image.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(image.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath(pathToImages), UserID + "_" + count + extension);
                        listImgPaths.Add(path);
                        // file is uploaded
                        image.SaveAs(path);
                    }
                }

                storeItem.HelperImagePaths = listImgPaths;
                storeItem.SellerId = appUser.MyUser.SellerID;

                _storeItemRepository.Update(storeItem);

                return RedirectToAction("Index");
            }

            return View(storeItem);
        }

        // GET: StoreItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItem storeItem = _storeItemRepository.GetById((int)id);
            if (storeItem == null)
            {
                return HttpNotFound();
            }
            return View(storeItem);
        }

        // POST: StoreItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StoreItem storeItem)
        {
            if (ModelState.IsValid)
            {
                _storeItemRepository.Update(storeItem);
                return RedirectToAction("Index");
            }
            return View(storeItem);
        }

        // GET: StoreItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItem storeItem = _storeItemRepository.GetById((int)id);
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
            _storeItemRepository.Delete(id);
            return RedirectToAction("Index");
        }


    }
}
