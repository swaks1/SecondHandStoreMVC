using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SecondHandStoreApp.Models;
using SecondHandStoreApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace SecondHandStoreApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private UserRepository _userRepository = new UserRepository();
        private SellerRepository _sellerRepository = new SellerRepository();
        private StoreItemRepository _storeItemRepository = new StoreItemRepository();
        private MyImageRepository _myImageRepository = new MyImageRepository();
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

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        //tutorial used: http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application
        public ActionResult ListUsers(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.EmailSortParm = sortOrder == "email_asc" ? "email_desc" : "email_asc";
            var users = _userRepository.GetAll();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.MyUser.FullName.ToLower().Contains(searchString.ToLower())
                                       || s.Email.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.MyUser.FullName).ToList();
                    break;
                case "email_asc":
                    users = users.OrderBy(s => s.Email).ToList();
                    break;
                case "email_desc":
                    users = users.OrderByDescending(s => s.Email).ToList();
                    break;
                default:
                    users = users.OrderBy(s => s.MyUser.FullName).ToList();
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(users.ToPagedList(pageNumber, pageSize));
               
        }

        //     USERS

        // GET: Admin/DisableUser/5
        public ActionResult DisableUser(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/DisableUser/5
        [HttpPost, ActionName("DisableUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DisableConfirmedUser(string id)
        {
            var user = UserManager.FindById(id);
            user.LockoutEndDateUtc = DateTime.Now.AddDays(3);
            UserManager.Update(user);
            return RedirectToAction("Index");
        }

        // GET: Admin/DetailsUser/5
        public ActionResult DetailsUser(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //    STORE ITEMS

        public ActionResult ListStoreItems(string query)
        {
            var items = _storeItemRepository.GetAll();

            switch (query)
            {
                case "notApproved":
                    items = items.Where(i => i.IsApproved == false && i.IsAvailable == true).ToList();
                    break;
                case "Avaible":
                    items = items.Where(i => i.IsAvailable == true).ToList();
                    break;

            }
            

            return View(items);
        }

        // GET: Admin/DisableItem/5
        public ActionResult DisableItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = _storeItemRepository.GetById((int)id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Admin/DisableItem/5
        [HttpPost, ActionName("DisableItem")]
        [ValidateAntiForgeryToken]
        public ActionResult DisableConfirmedItem(int id)
        {

            _storeItemRepository.Delete(id);
            return RedirectToAction("Index");
        }

        // GET: Admin/DetailsItem/5
        public ActionResult DetailsItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = _storeItemRepository.GetById((int)id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        public ActionResult DeleteImg(int? itemID, int imgID)
        {
            if (itemID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var imgToDeletge = _myImageRepository.GetById(imgID);
            string fullPath = Server.MapPath("~/" + imgToDeletge.Image);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            _myImageRepository.Delete(imgToDeletge.ID);
           
            return RedirectToAction("EditItem", new { id = itemID });

        }



        // GET: Admin/EditItem/5
        public ActionResult EditItem(int? id)
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

        // POST: Admin/EditItem/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditItem(StoreItem storeItem)
        {
            if (ModelState.IsValid)
            {
                _storeItemRepository.Update(storeItem);
                return RedirectToAction("ListStoreItems");
            }
            return View(storeItem);
        }

        [HttpGet]
        public ActionResult ApproveItem(int id)
        {
            var storeItem = _storeItemRepository.ApproveItem(id);
            return RedirectToAction("ListStoreItems", new { query = "notApproved" });
        }




    }
}