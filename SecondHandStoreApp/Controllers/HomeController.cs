using SecondHandStoreApp.Repository;
using SecondHandStoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace SecondHandStoreApp.Controllers
{
    public class HomeController : Controller
    {

        private UserRepository _userRepository = new UserRepository();
        private StoreItemRepository _storeItemRepository = new StoreItemRepository();

      
        public ActionResult Index()
        {
            var popularProducts = _storeItemRepository.GetPopular();
            return View(popularProducts);
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
            string searchCategory, string searchSubcategory, string currentFilter, int? page, int? pageSize)
        {
            ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewBag.Gender = searchGender;
            ViewBag.Category = searchCategory;
            
            if(pageSize!= null)
            {
                ViewBag.PageSize = pageSize;
            }

            //var items = _storeItemRepository.GetAllApproved();
            IQueryable<StoreItem> items;

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
                    items = items.OrderBy(i => i.Price);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(i => i.Price);
                    break;
                default:
                    items = items.OrderBy(i => i.ItemName);
                    break;
            }

            
            int pageNumber = (page ?? 1);
            ViewBag.PageSize = ViewBag.PageSize ?? 3;

            //return items ToList... or ToPagedList
            return View(items.ToPagedList(pageNumber, (int)ViewBag.PageSize));

        }

        public async Task<ActionResult> SendMail()
        {
            await SendMailTask();

            return Json("ok", JsonRequestBehavior.AllowGet);

        }
        static async Task SendMailTask()
        {
            string apiKey = "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";
            dynamic sg = new SendGridAPIClient(apiKey);

            Email from = new Email("Riste_P@outlook.com");
            string subject = "Hello World from the SendGrid CSharp Library!";
            Email to = new Email("marija283@hotmail.com");
            Content content = new Content("text/plain", "Zdravo Ubava !");
            Mail mail = new Mail(from, subject, to, content);

            dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
        }
    }
}