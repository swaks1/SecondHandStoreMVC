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

namespace SecondHandStoreApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private UserRepository _userRepository = new UserRepository();
        private SellerRepository _sellerRepository = new SellerRepository();
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

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListUsers()
        {
            var users = _userRepository.GetAll();

            return View(users);
        }

        // GET: Admin/DeleteUser/5
        public ActionResult DeleteUser(int? id)
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

        // POST: Admin/DeleteUser/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedUser(int id)
        {
            var user = UserManager.FindById("e137c540-7ad7-48e6-bbfe-38a2c45e22ba");

            user.LockoutEndDateUtc = new DateTime(2016, 8, 22);

            UserManager.Update(user);
            return RedirectToAction("Index");
        }

        // GET: Admin/DetailsUser/5
        //public ActionResult DetailsUser(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    if (storeItem == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(storeItem);
        //}




    }
}