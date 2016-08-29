using Microsoft.AspNet.Identity;
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

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShoppingCart()
        {
            var userId = User.Identity.GetUserId();

            var myUser = _userRepository.GetMyUser(userId);

            return View(myUser);
        }

        public ActionResult AddToCart(int id)
        {
            var userId = User.Identity.GetUserId();

            _userRepository.AddItemToShopingCart(userId, id);

            return RedirectToAction("ShoppingCart");
        }

        public ActionResult DeleteFromCart(int id)
        {
            var userId = User.Identity.GetUserId();

            _userRepository.DeleteItemFromShopingCart(userId, id);

            return RedirectToAction("ShoppingCart");
        }


        public ActionResult GetSellings()
        {
            var userId = User.Identity.GetUserId();
            var user = _userRepository.GetAppUser(userId);

            if(user.MyUser.SellerID == null)
            {
                return View("NotSeller");
            }

            return View(user.MyUser.seller.SellingItems.ToList());
        }

        
    }
}