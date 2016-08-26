using SecondHandStoreApp.Repository;
using SecondHandStoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace SecondHandStoreApp.Controllers
{
    public class HomeController : Controller
    {

        private UserRepository _userRepository = new UserRepository();
        private StoreItemRepository _storeItemRepository = new StoreItemRepository();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    

        public ActionResult ListItems(string sortOrder, string searchString, string searchGender,
            string searchCategory, string searchSubcategory, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewBag.Gender = searchGender;
            ViewBag.Category = searchCategory;
            ViewBag.Subcategory = searchSubcategory;

            //var items = _storeItemRepository.GetAllApproved();
            IEnumerable<StoreItem> items;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            
                switch (searchGender)
                {
                    case "female":
                        items = _storeItemRepository.Filter(i => i.itemGender.HasFlag(Gender.FEMALE));
                        break;

                    case "male":
                        items = _storeItemRepository.Filter(i => i.itemGender.HasFlag(Gender.MALE));
                        break;

                    default:
                        items = _storeItemRepository.Filter(i => i.itemGender.HasFlag(Gender.UNGENDERED));
                        break;
                }
                switch (searchCategory)
                {
                    case "clothes":
                        items = items.Where(i => i.category.HasFlag(Category.Clothes));
                        break;
                    case "shoes":
                        items = items.Where(i =>i.category.HasFlag(Category.Shoes));
                        break;
                    case "accessories":
                        items = items.Where(i => i.category.HasFlag(Category.Accessories));
                        break;
                    case "bags":
                        items = items.Where(i => i.category.HasFlag(Category.Bags));
                        break;
                    default:
                        break;
                }

            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(i => i.ItemName);
                    break;
                case "price_asc":
                    items = items.OrderBy(i => i.Price).ToList();
                    break;
                case "price_desc":
                    items = items.OrderByDescending(i => i.Price);
                    break;
                default:
                    items = items.OrderBy(i => i.ItemName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.PageSize = pageSize;
            return View(items.ToPagedList(pageNumber, pageSize));

        }
    }
}