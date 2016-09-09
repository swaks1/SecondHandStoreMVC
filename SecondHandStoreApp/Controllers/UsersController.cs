using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SecondHandStoreApp.Models;
using SecondHandStoreApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandStoreApp.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class UsersController : Controller
    {
        UserRepository _userRepository = new UserRepository();
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
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShoppingCart(string returnUrl)
        {
            var userId = User.Identity.GetUserId();

            var myUser = _userRepository.GetMyUser(userId);

            ViewBag.popularItems = _storeItemRepository.GetPopular().Take(3);
            ViewBag.returnUrl = returnUrl;

            return View(myUser);
        }

        public ActionResult AddToCart(int id, string returnUrl)
        {
            var userId = User.Identity.GetUserId();

            _userRepository.AddItemToShopingCart(userId, id);

            return RedirectToAction("ShoppingCart",new {returnUrl = returnUrl});
        }

        public ActionResult DeleteFromCart(int id)
        {
            var userId = User.Identity.GetUserId();

            _userRepository.DeleteItemFromShopingCart(userId, id);

            return RedirectToAction("ShoppingCart");
        }

        public ActionResult UserDetails()
        {
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserDetails(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var user = UserManager.FindById(userId);
                user.PhoneNumber = model.PhoneNumber;
                user.MyUser.FullName = model.MyUser.FullName;
                user.MyUser.City = model.MyUser.City;
                user.MyUser.Address = model.MyUser.Address;
                UserManager.Update(user);
            }
            return RedirectToAction("PaymentMetohod");
        }


        public ActionResult PaymentMetohod()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentMetohod(PaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                
            }
            return RedirectToAction("OrderReview");
        }

        public ActionResult OrderReview()
        {
            var userId = User.Identity.GetUserId();

            var myUser = _userRepository.GetMyUser(userId);

            return View(myUser);
        }

        [HttpPost,ActionName("OrderReview")]
        [ValidateAntiForgeryToken]
        public ActionResult OrderReviewPost()
        {
            var userId = User.Identity.GetUserId();
            _userRepository.AddToOrders(userId);

            return RedirectToAction("GetOrders");
        }

        public ActionResult GetSellings()
        {
            var userId = User.Identity.GetUserId();
            var user = _userRepository.GetAppUser(userId);

            if(user.MyUser.SellerID == null)
            {
                return View("NotSeller");
            }

            return View(user.MyUser.seller.SellingItems.Where(s => s.IsAvailable && s.IsFinished).ToList());
        }

        public ActionResult GetOrders()
        {
            var userId = User.Identity.GetUserId();
            var user = _userRepository.GetAppUser(userId);
        

            return View(user.MyUser.boughtItems);
        }


    }
}