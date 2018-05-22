using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMGs.Models;
using RMGs.Stereotypes;

namespace RMGs.Controllers
{
    public class HomeController : Controller
    {
        RMGContext db;
        IHostingEnvironment _appEnvironment;

        public HomeController(RMGContext context, IHostingEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;

        }

        private async Task<User> Initialize()
        {
            return await db.Users.FirstOrDefaultAsync(u => u.EMail == User.Identity.Name);
        }
        //----------------------------------------------------------

        public IActionResult Index(int category = 0)
        {
            if (category < 0 || category > 4) { return Redirect("~/Home/Index?category=0"); }
            ViewData["Category"] = Convert.ToString(category);
            ViewData["UserId"] = Convert.ToString(0);

            User u = Initialize().Result;

            if (u != null)
            {
                ViewData["UserId"] = Convert.ToString(u.Id);
            }

            //List<Order> orders = this.db.Orders.Where(o => (int)o.Type == category).ToList();
            List<Order> orders = this.db.Orders.Where(o => (int)o.Type == category).ToList();
            List<RealEstate> estates = this.db.RealEstates.ToList();

            return View(orders);
        }
        //--------------------------------------------------------------

        [HttpGet]
        [Authorize]
        public IActionResult Profile(int id = 0)
        {
            if (id<0) { return Redirect("~/Home/Sorry"); }
            ViewData["userId"] = Convert.ToString(id);

            User data;
            if (id == 0) { data = Initialize().Result; }
            else { data = db.Users.Find(id); }
            if (data==null) { return Redirect("~/Home/Sorry"); }

            return View(data);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Profile(User User)
        {
            User u = Initialize().Result;
            if (u == null || User==null) { return Redirect("~/Home/Sorry"); }

            u.EMail = User.EMail;
            u.Phone = User.Phone;
            u.FIO = User.FIO;

            db.Users.Update(u);
            db.SaveChanges();

            return Redirect("~/Home/Profile?id=0");
        }
        //---------------------------------------------------------------
        [Authorize]
        public IActionResult UserOrders(int category = 0)
        {
            if (category < 0 || category > 4) { return Redirect("~/Home/UserOrders?category=0"); }
            ViewData["Category"] = Convert.ToString(category);

            User us = Initialize().Result;
            if (us==null) { return Redirect("~/Home/Sorry"); }

            List<Order> lorders=db.Orders.Where(o => o.UserId == us.Id).ToList();
            List<RealEstate> lestate = db.RealEstates.ToList();

            if (us.Orders == null) { return Redirect("~/Home/Sorry"); }
            List <Order> orders = us.Orders.Where(o => (int)o.Type == category).ToList();
 
            return View(orders);
        }

        //--------------------------------------------------------------
        [HttpGet]
        [Authorize]
        public IActionResult CreateOrder()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateOrder(Order Order, RealEstate RealEstate, IFormFile Image)
        {
            User us = Initialize().Result;
            if (us==null || Order==null || RealEstate==null || Image==null) { return Redirect("~/Home/Sorry"); }
            RealEstate.Photo = ImgSaveAsync(Image).Result;

            db.RealEstates.Add(RealEstate);
            db.SaveChanges();

            Order.RealEstateId = RealEstate.Id;
            Order.UserId = us.Id;
            Order.UserName = us.FIO;
            db.Orders.Add(Order);
            db.SaveChanges();

            return Redirect("~/Home/Index?category=0");
        }

        private async Task<String> ImgSaveAsync(IFormFile Image)
        {
            if (Image != null)
            {
                // путь к папке Files
                string path = "/Files/" + Image.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }
                String file = path;//FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
                return file;
            }
            return null;
        }
        //----------------------------------------------------------------------------
        [Authorize]
        [HttpGet]
        public IActionResult ViewOrder(int OrderId)
        {
            if (OrderId <= 0) { return Redirect("~/Home/Sorry"); }

            Order Order = db.Orders.Find(OrderId);
            if (Order == null) { return Redirect("~/Home/Sorry"); }

            RealEstate r = db.RealEstates.Find(Order.RealEstateId);
            if (r == null) { return Redirect("~/Home/Sorry"); }

            User us = Initialize().Result;
            if (us==null) { return Redirect("~/Home/Sorry"); }

            ViewData["userHas"] = Convert.ToString(us.Orders!=null);
            ViewData["orId"] = Convert.ToString(OrderId);
            ViewData["reId"] = Convert.ToString(Order.RealEstateId);
            return View(Order);
        }
        //--------------------------------------------------------------------------
        [Authorize]
        [HttpPost]
        public IActionResult DeleteOrder(int OrderId, int RealEstateId)
        {
            if (OrderId <= 0 || RealEstateId <= 0) { return Redirect("~/Home/Sorry"); }

            Order Order = db.Orders.Find(OrderId);//new Order { Id = OrderId};
            RealEstate RealEstate = db.RealEstates.Find(RealEstateId);//new RealEstate { Id = RealEstateId };

            db.Orders.Remove(Order);
            db.RealEstates.Remove(RealEstate);
            db.SaveChanges();
            return Redirect("~/Home/UserOrders?category=0");
        }

        [Authorize]
        public IActionResult DeleteOrderView(int OrderId, int RealEstateId)
        {
            if (OrderId<=0 && RealEstateId<=0) { return Redirect("~/Home/Sorry"); }
            return DeleteOrder(OrderId,RealEstateId);
        }

        [Authorize]
        public IActionResult EditOrder(int OrderId, int RealEstateId)
        {
            Order Order = db.Orders.Find(OrderId);
            RealEstate RealEstate = db.RealEstates.Find(RealEstateId);
            if (Order == null || RealEstate == null) { return Redirect("~/Home/Sorry"); }

            ViewData["orId"] = Convert.ToString(OrderId);
            ViewData["reId"] = Convert.ToString(RealEstateId);
            ViewData["usId"] = Convert.ToString(Order.UserId);
            return View(Order);
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditOrder(Order Order, IFormFile Image)
        {
            if (Image != null)
            {
                Order.RealEstate.Photo = ImgSaveAsync(Image).Result;
            }

            db.Orders.Update(Order);

            db.SaveChanges();
            return Redirect("~/Home/UserOrders?category=0");
        }
        //----------------------------------------------------------------------------

        public IActionResult Authorization()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(String EMail, String Password)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.EMail == EMail && u.Password == Password);
                if (user != null)
                {
                    await Authenticate(EMail); // аутентификация

                    return Redirect("~/Home/Index?category=0");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return Redirect("~/Home/Sorry");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/Home/Index?category=0");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(User User)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.EMail == User.EMail);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    //User.BookMarks = new List<int>();
                    db.Users.Add(User);
                    await db.SaveChangesAsync();

                    await Authenticate(User.EMail); // аутентификация

                    return Redirect("~/Home/Index?category=0");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return Redirect("~/Home/Sorry");
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        //--------------------------------------------------------------------------------------------

        [Authorize]
        public IActionResult Search(OrderType Ot, TypeEstate Et, String Country, String City)
        {
            if (City ==null || Country == null)
            { return Redirect("~/Home/Index?category=0"); }

            OrderType otSearch = OrderType.Swap;
            switch (Ot)
            {
                case OrderType.Sale:
                    {
                        otSearch = OrderType.Purchase;
                        break;
                    }
                case OrderType.Purchase:
                    {
                        otSearch = OrderType.Sale;
                        break;
                    }
                case OrderType.Lease:
                    {
                        otSearch = OrderType.Rent;
                        break;
                    }
                case OrderType.Rent:
                    {
                        otSearch = OrderType.Lease;
                        break;
                    }           
            }
            User u = Initialize().Result;
            List<Order> orders = this.db.Orders.Where(o => o.Type == otSearch && o.UserId!=u.Id).ToList();
            List < RealEstate > real = this.db.RealEstates.ToList();
            return View(orders);
        }
        //------------------------------------------------------------------
        [Authorize]
        [HttpGet]
        public void AddTOBookMarks(int OrderId)
        {
            User us = Initialize().Result;
            if (us == null) { return; }

            List<BookMarks> lb = db.BookMarks.Where(b => b.UserId == us.Id && b.OrderId == OrderId).ToList();
            if (lb.Count()==0)
            {
                //db.Users.Include(i => i.BookMarks).First(s => s.Id == id);
                BookMarks b = new BookMarks { OrderId = OrderId, UserId = us.Id };
                db.BookMarks.Add(b);
            }
            else
            {
                db.BookMarks.Remove(lb[0]);
            }
            db.SaveChanges();
        }

        [Authorize]
        public IActionResult BookMarks(int category = 0)
        {
            if (category < 0 || category > 4) { return Redirect("~/Home/Index?category=0"); }
            ViewData["Category"] = Convert.ToString(category);

            User us = Initialize().Result;

            if (us != null)
            {
                ViewData["UserId"] = Convert.ToString(us.Id);
            }

            if (us == null) { return Redirect("~/Home/Sorry"); }

            List<BookMarks> bm = db.BookMarks.Where(b => b.UserId == us.Id).ToList();

            List<Order> o = new List<Order>();

            for (int i = 0; i < bm.Count(); ++i)
            {
                o.Add(db.Orders.Find(bm.ElementAt(i).OrderId));
            }
            List<RealEstate> r = db.RealEstates.ToList();

            o =  o.Where(x => (int)x.Type == category).ToList();

            return View(o);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Sorry()
        {
            return View();
        }
    }
}
