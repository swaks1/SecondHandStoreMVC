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
using PayPal.Api;
using System.Net;
using System.Text;
using System.IO;

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


            //TODO: if(searchGender == null)  what to return??

            switch (searchGender.ToLower())
                {
                    case "female":
                        items = _storeItemRepository.Filter(i => i.itemGender.HasFlag(Gender.Female) && i.isSold==false);
                        break;

                    case "male":
                        items = _storeItemRepository.Filter(i => i.itemGender.HasFlag(Gender.Male) && i.isSold == false);
                        break;
                    default:
                        items = _storeItemRepository.Filter(i => i.itemGender.HasFlag(Gender.Ungendered) && i.isSold == false) ;
                        break;
                }

                switch (searchCategory.ToLower())
                {
                    case "clothes":
                    if (searchSubcategory != null)
                    {
                        switch (searchSubcategory.ToLower())
                        {
                            case "blazers":
                                items = items.Where(i => i.category.HasFlag(Category.Clothes)
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Blazers));
                                break;
                            case "dresses":
                                items = items.Where(i => i.category.HasFlag(Category.Clothes)
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Dresses));
                                break;
                            case "jacketandcoat":
                                items = items.Where(i => i.category.HasFlag(Category.Clothes)
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.JacketAndCoat));
                                break;
                            case "jeans":
                                items = items.Where(i => i.category.HasFlag(Category.Clothes)
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Jeans));
                                break;
                            case "knitwear":
                                items = items.Where(i => i.category.HasFlag(Category.Clothes)
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Knitwear));
                                break;
                            case "shorts":
                                items = items.Where(i => i.category.HasFlag(Category.Clothes)
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Shorts));
                                break;
                            case "skirts":
                                items = items.Where(i => i.category.HasFlag(Category.Clothes)
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Skirts));
                                break;
                            case "suits":
                                items = items.Where(i => i.category.HasFlag(Category.Clothes)
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Suits));
                                break;
                            case "tops":
                                items = items.Where(i => i.category.HasFlag(Category.Clothes)
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Tops));
                                break;
                            case "trousers":
                                items = items.Where(i => i.category.HasFlag(Category.Clothes)
                                                      && i.subcategoryClothes.HasFlag(SubcategoryClothes.Trousers));
                                break;
                            case "jumpsuit":
                                items = items.Where(i => i.category.HasFlag(Category.Clothes)
                                                     && i.subcategoryClothes.HasFlag(SubcategoryClothes.Jumpsuit));
                                break;
                            case "beachwear":
                                items = items.Where(i => i.category.HasFlag(Category.Clothes)
                                                     && i.subcategoryClothes.HasFlag(SubcategoryClothes.Beachwear));
                                break;
                            default:
                                items = items.Where(i => i.category.HasFlag(Category.Clothes));
                                break;
                        }
                    }
                    else
                    {
                        items = items.Where(i => i.category.HasFlag(Category.Clothes));
                    }
                        break;

                    case "shoes":
                    if (searchSubcategory != null)
                    {
                        switch (searchSubcategory.ToLower()) {
                            case "trainers":
                                items = items.Where(i => i.category.HasFlag(Category.Shoes)
                                                     &&  i.subcategoryShoes.HasFlag(SubcategoryShoes.Trainers));
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
                    else {
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
          
            //var items = _storeItemRepository.GetAllApproved();
            IQueryable<StoreItem> items;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                items = _storeItemRepository
                                .Filter(i => i.ItemName.ToLower().Contains(searchString.ToLower()) && i.isSold == false);

                ViewBag.searchString = searchString;
                return View(items.ToList());

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
            string apiKey = System.Configuration.ConfigurationManager.AppSettings["mailApiKey"];
            dynamic sg = new SendGridAPIClient(apiKey);

            Email to = new Email("marija283@hotmail.com");// company address
            Email from = new Email(model.Email, model.FirstName + " " + model.LastName );
            string subject = model.Subject;
            Content content = new Content("text/plain", model.Message);
            Mail mail = new Mail(from, subject, to, content);

            dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
        }
        
        public ActionResult PaymentWithCreditCard()
        {
            //create and item for which you are taking payment
            //if you need to add more items in the list
            //Then you will need to create multiple item objects or use some loop to instantiate object
            Item item = new Item();
            item.name = "Demo Item";
            item.currency = "USD";
            item.price = "5";
            item.quantity = "1";
            item.sku = "sku";

            //Now make a List of Item and add the above item to it
            //you can create as many items as you want and add to this list
            List<Item> itms = new List<Item>();
            itms.Add(item);
            ItemList itemList = new ItemList();
            itemList.items = itms;

            //Address for the payment
            Address billingAddress = new Address();
            billingAddress.city = "NewYork";
            billingAddress.country_code = "US";
            billingAddress.line1 = "23rd street kew gardens";
            billingAddress.postal_code = "43210";
            billingAddress.state = "NY";


            //Now Create an object of credit card and add above details to it
            //Please replace your credit card details over here which you got from paypal
            CreditCard crdtCard = new CreditCard();
            crdtCard.billing_address = billingAddress;
            crdtCard.cvv2 = "874";  //card cvv2 number
            crdtCard.expire_month = 10; //card expire date
            crdtCard.expire_year = 2021; //card expire year
            crdtCard.first_name = "Marija";
            crdtCard.last_name = "Todorova";
            crdtCard.number = "4032036140345909"; //enter your credit card number here
            crdtCard.type = "visa"; //credit card type here paypal allows 4 types

            // Specify details of your payment amount.
            Details details = new Details();
            details.shipping = "1";
            details.subtotal = "5";
            details.tax = "1";

            // Specify your total payment amount and assign the details object
            Amount amnt = new Amount();
            amnt.currency = "USD";
            // Total = shipping tax + subtotal.
            amnt.total = "7";
            amnt.details = details;

            // Now make a transaction object and assign the Amount object
            Transaction tran = new Transaction();
            tran.amount = amnt;
            tran.description = "Description about the payment amount.";
            tran.item_list = itemList;
            tran.invoice_number = "your invoice number which you are generating";

            // Now, we have to make a list of transaction and add the transactions object
            // to this list. You can create one or more object as per your requirements

            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(tran);

            // Now we need to specify the FundingInstrument of the Payer
            // for credit card payments, set the CreditCard which we made above

            FundingInstrument fundInstrument = new FundingInstrument();
            fundInstrument.credit_card = crdtCard;

            // The Payment creation API requires a list of FundingIntrument

            List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
            fundingInstrumentList.Add(fundInstrument);

            // Now create Payer object and assign the fundinginstrument list to the object
            Payer payr = new Payer();
            payr.funding_instruments = fundingInstrumentList;
            payr.payment_method = "credit_card";

            // finally create the payment object and assign the payer object & transaction list to it
            Payment pymnt = new Payment();
            pymnt.intent = "sale";
            pymnt.payer = payr;
            pymnt.transactions = transactions;

            
                //getting context from the paypal
                //basically we are sending the clientID and clientSecret key in this function
                //to the get the context from the paypal API to make the payment
                //for which we have created the object above.

                //Basically, apiContext object has a accesstoken which is sent by the paypal
                //to authenticate the payment to facilitator account.
                //An access token could be an alphanumeric string

                APIContext apiContext = Configuration.GetAPIContext();

                //Create is a Payment class function which actually sends the payment details
                //to the paypal API for the payment. The function is passed with the ApiContext
                //which we received above.

                Payment createdPayment = pymnt.Create(apiContext);

                //if the createdPayment.state is "approved" it means the payment was successful else not

                if (createdPayment.state.ToLower() != "approved")
                {
                    return Json("NOT OK", JsonRequestBehavior.AllowGet);
                }
            
            

            return Json("OK", JsonRequestBehavior.AllowGet);
        }


        public ActionResult PaymentWithPaypal()
        {
            //getting the apiContext as earlier
            APIContext apiContext = Configuration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    // So we have provided URL of this controller only
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/Home/PaymentWithPayPal?";

                    //guid we are generating for storing the paymentID received in session
                    //after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
              
                return Json("NOT OK",JsonRequestBehavior.AllowGet);
            }

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        private Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {

            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            itemList.items.Add(new Item()
            {
                name = "Item Name",
                currency = "USD",
                price = "5",
                quantity = "1",
                sku = "sku"
            });

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "5"
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = "7", // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = "your invoice number",
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }

        internal static class Configuration
        {
            //these variables will store the clientID and clientSecret
            //by reading them from the web.config
            public readonly static string ClientId;
            public readonly static string ClientSecret;

            static Configuration()
            {
                var config = GetConfig();
                ClientId = config["clientId"];
                ClientSecret = config["clientSecret"];
            }

            // getting properties from the web.config
            public static Dictionary<string, string> GetConfig()
            {
                return ConfigManager.Instance.GetProperties();
            }

            private static string GetAccessToken()
            {
                // getting accesstocken from paypal                
                string accessToken = new OAuthTokenCredential
            (ClientId, ClientSecret, GetConfig()).GetAccessToken();

                return accessToken;
            }

            public static APIContext GetAPIContext()
            {
                // return apicontext object by invoking it with the accesstoken
                APIContext apiContext = new APIContext(GetAccessToken());
                apiContext.Config = GetConfig();
                return apiContext;
            }
        }
    }
}