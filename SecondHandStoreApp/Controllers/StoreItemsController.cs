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

        [Authorize(Roles ="User")]
        // GET: StoreItems/Create1
        public ActionResult Create1(int? id)
        {
            if (id != null)
            {
                StoreItem storeItem = _storeItemRepository.GetById((int)id);
                string UserID = User.Identity.GetUserId();
                var appUser = UserManager.FindById(UserID);
                if (appUser.MyUser.SellerID != null && appUser.MyUser.SellerID != storeItem.SellerId)
                    return View();
                return View(storeItem);
            }
            return View();
        }

        // POST: StoreItems/Create1
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create1(StoreItem storeItem, int? id)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    _storeItemRepository.UpdateStep1(storeItem);
                    return RedirectToAction("Create2", new { id = storeItem.ID });
                }

                string UserID = User.Identity.GetUserId();
                var appUser = UserManager.FindById(UserID);
              
                storeItem.SellerId = appUser.MyUser.SellerID;
                _storeItemRepository.Create(storeItem);
                return RedirectToAction("Create2", new { id = storeItem.ID});
            }

            return View(storeItem);
        }

        [Authorize(Roles = "User")]
        // GET: StoreItems/Create2/4
        public ActionResult Create2(int? id)
        {
            if (id == null || id == 0)
                return RedirectToAction("Create1");
            var item = _storeItemRepository.GetById((int)id);
            if(item == null)
                return RedirectToAction("Create1");
            string UserID = User.Identity.GetUserId();
            var appUser = UserManager.FindById(UserID);
            if (appUser.MyUser.SellerID != item.SellerId)
                return RedirectToAction("Create1");
            return View(item);
        }

        // POST: StoreItems/Create2
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2(HttpPostedFileBase[] file, StoreItem storeItem)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                string UserID = User.Identity.GetUserId();
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
                        string imageName = storeItem.ID + "_" + count + extension;

                        string path = System.IO.Path.Combine(
                                               Server.MapPath(pathToImages), imageName);

                        listImgPaths.Add("Images/" + storeItem.ID + "/" + imageName);
                        // file is uploaded
                        image.SaveAs(path);
                    }
                }

                storeItem.HelperImagePaths = listImgPaths;

                _storeItemRepository.UpdateStep2(storeItem);

                return RedirectToAction("MakeSeller", "Sellers", new { itemId = storeItem.ID });
            }

            return View(storeItem);
        }

        [Authorize(Roles = "User")]
        // GET: StoreItems/CreateCheck/4
        public ActionResult CreateCheck(int? storeItemId)
        {
            if (storeItemId == null || storeItemId == 0)
                return RedirectToAction("Create1");
            var item = _storeItemRepository.GetById((int)storeItemId);
            if (item == null)
                return RedirectToAction("Create1");
            ViewBag.stoteItemID = storeItemId;
            return View(item);
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
            var user = UserManager.FindById(User.Identity.GetUserId());

            if(storeItem.SellerId != user.MyUser.SellerID)
            {
                return RedirectToAction("UnAuthorized","Account");
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

            var user = UserManager.FindById(User.Identity.GetUserId());


            if (storeItem.SellerId != user.MyUser.SellerID)
            {
                return RedirectToAction("UnAuthorized", "Account");
            }

            return View(storeItem);
        }

        // POST: StoreItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _storeItemRepository.DisableItem(id);
            return RedirectToAction("Index");
        }


    }
}
