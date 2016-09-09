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
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using SecondHandStoreApp.LuceneNetSearch;

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



        public ActionResult ListItems(string sortOrder, string searchString, string searchGender,
            string searchCategory, string searchSubcategory, string currentFilter, int? page, int? pageSize)
        {

            if (searchGender == null)
            {
                return RedirectToAction("Index");
            }


            ViewBag.CurrentSort = sortOrder;
            ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewBag.Gender = searchGender;
            ViewBag.Category = searchCategory;

            if (pageSize != null)
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



            switch (searchGender.ToLower())
            {
                case "female":
                    items = _storeItemRepository.Filter(i => i.itemGender.HasFlag(Gender.Female) && i.isSold == false);
                    break;

                case "male":
                    items = _storeItemRepository.Filter(i => i.itemGender.ToString() == "Male" && i.isSold == false);
                    break;
                default:
                    items = _storeItemRepository.Filter(i => (i.itemGender.HasFlag(Gender.Female) || i.itemGender.HasFlag(Gender.Male)) && i.isSold == false);
                    break;
            }

            if (searchCategory == null)
                searchCategory = "no";

            switch (searchCategory.ToLower())
            {
                case "clothes":
                    if (searchSubcategory != null)
                    {
                        switch (searchSubcategory.ToLower())
                        {
                            case "blazers":
                                items = items.Where(i => i.category.ToString() == "Clothes"
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Blazers));
                                break;
                            case "dresses":
                                items = items.Where(i => i.category.ToString() == "Clothes"
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Dresses));
                                break;
                            case "jacketandcoat":
                                items = items.Where(i => i.category.ToString() == "Clothes"
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.JacketAndCoat));
                                break;
                            case "jeans":
                                items = items.Where(i => i.category.ToString() == "Clothes"
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Jeans));
                                break;
                            case "knitwear":
                                items = items.Where(i => i.category.ToString() == "Clothes"
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Knitwear));
                                break;
                            case "shorts":
                                items = items.Where(i => i.category.ToString() == "Clothes"
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Shorts));
                                break;
                            case "skirts":
                                items = items.Where(i => i.category.ToString() == "Clothes"
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Skirts));
                                break;
                            case "suits":
                                items = items.Where(i => i.category.ToString() == "Clothes"
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Suits));
                                break;
                            case "tops":
                                items = items.Where(i => i.category.ToString() == "Clothes"
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Tops));
                                break;
                            case "trousers":
                                items = items.Where(i => i.category.ToString() == "Clothes"
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Trousers));
                                break;
                            case "jumpsuit":
                                items = items.Where(i => i.category.ToString() == "Clothes"
                                                     && i.subcategoryClothes.HasFlag(SubcategoryClothes.Jumpsuit));
                                break;
                            case "beachwear":
                                items = items.Where(i => i.category.ToString() == "Clothes"
                                                     && i.subcategoryClothes.HasFlag(SubcategoryClothes.Beachwear));
                                break;
                            default:
                                items = items.Where(i => i.category.ToString() == "Clothes");
                                break;
                        }
                    }
                    else
                    {
                        items = items.Where(i => i.category.ToString() == "Clothes");
                    }
                    break;

                case "shoes":
                    if (searchSubcategory != null)
                    {
                        switch (searchSubcategory.ToLower())
                        {
                            case "trainers":
                                items = items.Where(i => i.category.HasFlag(Category.Shoes)
                                                     && i.subcategoryShoes.HasFlag(SubcategoryShoes.Trainers));
                                break;
                            case "sandals":
                                items = items.Where(i => i.category.HasFlag(Category.Shoes)
                                                     && i.subcategoryShoes.HasFlag(SubcategoryShoes.Sandals));
                                break;
                            case "heels":
                                items = items.Where(i => i.category.HasFlag(Category.Shoes)
                                                     && i.subcategoryShoes.HasFlag(SubcategoryShoes.Heels));
                                break;
                            case "boots":
                                items = items.Where(i => i.category.HasFlag(Category.Shoes)
                                                     && i.subcategoryShoes.HasFlag(SubcategoryShoes.Boots));
                                break;
                            case "shoes":
                                items = items.Where(i => i.category.HasFlag(Category.Shoes)
                                                     && i.subcategoryShoes.HasFlag(SubcategoryShoes.Shoes));
                                break;
                            default:
                                items = items.Where(i => i.category.HasFlag(Category.Shoes));
                                break;
                        }

                    }
                    else
                    {
                        items = items.Where(i => i.category.HasFlag(Category.Shoes));

                    }
                    break;
                case "accessories":
                    if (searchSubcategory != null)
                    {
                        switch (searchSubcategory.ToLower())
                        {
                            case "belts":
                                items = items.Where(i => i.category.HasFlag(Category.Accessories)
                                                      && i.subcategoryAccessories.HasFlag(SubcategoryAccessories.Belts));
                                break;
                            case "glasses":
                                items = items.Where(i => i.category.HasFlag(Category.Accessories)
                                                      && i.subcategoryAccessories.HasFlag(SubcategoryAccessories.Glasses));
                                break;
                            case "hats":
                                items = items.Where(i => i.category.HasFlag(Category.Accessories)
                                                      && i.subcategoryAccessories.HasFlag(SubcategoryAccessories.Hats));
                                break;
                            case "scarves":
                                items = items.Where(i => i.category.HasFlag(Category.Accessories)
                                                      && i.subcategoryAccessories.HasFlag(SubcategoryAccessories.Scarves));
                                break;
                            case "sunglasses":
                                items = items.Where(i => i.category.HasFlag(Category.Accessories)
                                                      && i.subcategoryAccessories.HasFlag(SubcategoryAccessories.Sunglasses));
                                break;
                            case "watches":
                                items = items.Where(i => i.category.HasFlag(Category.Accessories)
                                                      && i.subcategoryAccessories.HasFlag(SubcategoryAccessories.Watches));
                                break;
                            case "mobilephonecases":
                                items = items.Where(i => i.category.HasFlag(Category.Accessories)
                                                      && i.subcategoryAccessories.HasFlag(SubcategoryAccessories.MobilePhoneCases));
                                break;
                            case "jewellery":
                                items = items.Where(i => i.category.HasFlag(Category.Accessories)
                                                      && i.subcategoryAccessories.HasFlag(SubcategoryAccessories.Jewellery));
                                break;
                            default:
                                items = items.Where(i => i.category.HasFlag(Category.Accessories));
                                break;
                        }
                    }
                    else
                    {
                        items = items.Where(i => i.category.HasFlag(Category.Accessories));
                    }
                    break;
                case "bags":
                    if (searchSubcategory != null)
                    {
                        switch (searchSubcategory.ToLower())
                        {
                            case "backpacks":
                                items = items.Where(i => i.category.HasFlag(Category.Bags)
                                                      && i.subcategoryBags.HasFlag(SubcategoryBags.Backpacks));
                                break;
                            case "wallets":
                                items = items.Where(i => i.category.HasFlag(Category.Bags)
                                                      && i.subcategoryBags.HasFlag(SubcategoryBags.Wallets));
                                break;
                            case "bag":
                                items = items.Where(i => i.category.HasFlag(Category.Bags)
                                                      && i.subcategoryBags.HasFlag(SubcategoryBags.Bag));
                                break;
                            default:
                                items = items.Where(i => i.category.HasFlag(Category.Bags));
                                break;
                        }
                    }
                    else
                    {
                        items = items.Where(i => i.category.HasFlag(Category.Bags));

                    }
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

        public ActionResult Search(string searchString)
        {
            //SEARCH WITHOUT LUCINE
            //IQueryable<StoreItem> items;

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    items = _storeItemRepository
            //                    .Filter(i => i.ItemName.ToLower().Contains(searchString.ToLower()) && i.isSold == false);

            //    ViewBag.searchString = searchString;
            //    return View(items.ToList());

            //}
            //return View();

            //LUCINE SEARCH
            if (!String.IsNullOrEmpty(searchString))
            {
                var items = LuceneSearch.Search(searchString);
                ViewBag.searchString = searchString;

                List<StoreItem> result = new List<StoreItem>();

                foreach (var item in items)
                {
                    var dbItem = _storeItemRepository.GetById(item.ID);
                    result.Add(dbItem);
                }

                return View(result);
            }

            return View();

        }


        public ActionResult Contact()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                await SendContactMailTask(model);
                ViewBag.message = "Mail sent successfully";
            }
            return View();
        }

        public ActionResult FAQ()
        {

            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }


        public ActionResult LearnMore()
        {

            return View();
        }

        public async Task<ActionResult> SendMail()
        {
            await SendMailTask();

            return Json("ok", JsonRequestBehavior.AllowGet);

        }
        static async Task SendMailTask()
        {
            string apiKey = System.Configuration.ConfigurationManager.AppSettings["mailApiKey"];
            dynamic sg = new SendGridAPIClient(apiKey);

            Email from = new Email("Riste_P@outlook.com");// company address
            string subject = "Hello World from the SendGrid CSharp Library!";
            Email to = new Email("marija283@hotmail.com");
            Content content = new Content("text/plain", "Zdravo Ubava !");
            Mail mail = new Mail(from, subject, to, content);

            dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
        }

        static async Task SendContactMailTask(ContactViewModel model)
        {
            //string apiKey = System.Configuration.ConfigurationManager.AppSettings["mailApiKey"];
            //dynamic sg = new SendGridAPIClient(apiKey);

            //Email to = new Email("marija283@hotmail.com");// company address
            //Email from = new Email(model.Email, model.FirstName + " " + model.LastName );
            //string subject = model.Subject;
            //Content content = new Content("text/plain", model.Message);
            //Mail mail = new Mail(from, subject, to, content);

            //dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
            IdentityMessage message = new IdentityMessage
            {
                Body = model.Message,
                Subject = model.Subject,
                Destination = model.Email

            };
            sendMail(message);
        }


        static void sendMail(IdentityMessage message)
        {
            #region formatter
            string text = string.Format(message.Body);
            string html = "<strong>" + message.Body + "</strong>";

            #endregion

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("SecondHandStore@EMT.com");
            msg.To.Add(new MailAddress(message.Destination));
            msg.Subject = message.Subject;
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            using (SmtpClient smtpClient = new SmtpClient())
            {
                NetworkCredential credentials = new NetworkCredential("secondhandstoreemt@gmail.com", "secondstore123");
                smtpClient.Credentials = credentials;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Send(msg);

            }

        }

        public ActionResult ShowErrorPage()
        {
            return View("Error");
        }

        //make sure you have already added all items to Index.. \Admin\AddAllItemsToIndex
        public ActionResult SearchWithLucene(string query)
        {
            
            var items = LuceneSearch.Search(query);

            return Json(items,JsonRequestBehavior.AllowGet); 
        }
    }
}