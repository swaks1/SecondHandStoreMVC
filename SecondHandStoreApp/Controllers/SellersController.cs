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
using Microsoft.AspNet.Identity.EntityFramework;

namespace SecondHandStoreApp.Controllers
{
    public class SellersController : Controller
    {
        private SellerRepository _sellerRepository = new SellerRepository();
        private UserRepository _userRepository = new UserRepository();


        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManger;
        private ApplicationSignInManager _signInManager;


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
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManger ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManger = value;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }


        // GET: Sellers
        public ActionResult Index()
        {
            return View(_sellerRepository.GetAll());
        }

        // GET: Sellers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = _sellerRepository.GetById((int)id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        // GET: Sellers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = _sellerRepository.GetById((int)id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        // POST: Sellers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Seller seller)
        {
            if (ModelState.IsValid)
            {
                _sellerRepository.Update(seller);
                return RedirectToAction("Index");
            }
            return View(seller);
        }

        // GET: Sellers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = _sellerRepository.GetById((int)id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _sellerRepository.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult MakeSeller()
        {
            if (User.IsInRole("Seller"))
                return RedirectToAction("UnAuthorized", "Account");

            var userId = User.Identity.GetUserId();         
            var user = UserManager.FindById(userId);

            return View(user); 
         }

        [HttpPost ,ActionName("MakeSeller")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult MakeSellerPost(Seller seller)
        {
         
            var userId = User.Identity.GetUserId();          
            var user = UserManager.FindById(userId);

            var isSaved =  _userRepository.MakeSeller(user.MyUser.ID, seller);

            if (isSaved)
            {    
                var role = RoleManager.FindByName("Seller");

                if(role == null)
                {
                    role = new IdentityRole("Seller");
                    RoleManager.Create(role);
                }

                UserManager.AddToRole(userId, role.Name);

                //usr must re-log so the Roles will take effect
                HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
            }

            return RedirectToAction("index","StoreItems");
        }


    }
}
