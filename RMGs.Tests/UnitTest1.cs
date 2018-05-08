using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RMGs.Controllers;
using RMGs.Models;
using System;
using RMGs;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc;
using System.Linq;

namespace RMGs.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private DbContextOptions<RMGContext> TestInit()
        {
            var optionsBuilder = new DbContextOptionsBuilder<RMGContext>();
            var option = optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=rmgsdb;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            return option;
        }

        private void UserEquals(User expected, User result)
        {
            Assert.AreEqual(expected.FIO, result.FIO);
            Assert.AreEqual(expected.EMail, result.EMail);
            Assert.AreEqual(expected.Password, result.Password);
            Assert.AreEqual(expected.Phone, result.Phone);
        }

        private void OrderEquals(Order expected, Order result)
        {
            Assert.AreEqual(expected.Priece, result.Priece);
            Assert.AreEqual(expected.RealEstateId, result.RealEstateId);
            Assert.AreEqual(expected.Type, result.Type);
            Assert.AreEqual(expected.UserName, result.UserName);
            Assert.AreEqual(expected.UserId, result.UserId);
        }

        [TestMethod]
        public void ConnectionDatabase()
        {
            var options = TestInit();

            RMGContext db = new RMGContext(options);

            Assert.IsNotNull(db);
        }

        [TestMethod]
        public void CreatingController()
        {
            var options = TestInit();

            using (RMGContext db = new RMGContext(options))
            {
                HomeController controller = new HomeController(db, null);
                Assert.IsNotNull(controller);
            }
        }

        [TestMethod]
        public void ControllerIndexOpen()
        {
            var options = TestInit();

            using (RMGContext db = new RMGContext(options))
            {
                HomeController controller = new HomeController(db, null);
                ViewResult result = controller.Index() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void GetUserById()
        {
            var options = TestInit();

            using (RMGContext db = new RMGContext(options))
            {
                var result = db.Users.Find(2);
                var expected = new User();
                expected.Id = 2;
                expected.EMail = "admin@mail.ru";
                expected.FIO = "Admin Adminovich";
                expected.Orders = null;
                expected.Password = "12345Ad";
                expected.Phone = "8(902) 345-66-89";

                UserEquals(expected, result);

            }
        }

        [TestMethod]
        public void SetNewUser()
        {
            var options = TestInit();

            using (RMGContext db = new RMGContext(options))
            {
                var newUser = new User();
                newUser.EMail = "admin@gmail.ru";
                newUser.FIO = "Admin AAAA";
                newUser.Orders = null;
                newUser.Password = "12345Pl";
                newUser.Phone = "8(902) 345-66-89";

                db.Users.Add(newUser);
                db.SaveChanges();

                var result = db.Users.Last<User>();

                var expected = new User();
                expected.EMail = "admin@gmail.ru";
                expected.FIO = "Admin AAAA";
                expected.Orders = null;
                expected.Password = "12345Pl";
                expected.Phone = "8(902) 345-66-89";

                UserEquals(expected, result);
            }
        }

        [TestMethod]
        public void EditUser()
        {
            var options = TestInit();

            using (RMGContext db = new RMGContext(options))
            {
                var user = db.Users.Last<User>();
                user.EMail = "aaaaaaa@gmail.com";
                user.FIO = "Admin AAAA";
                user.Orders = null;
                user.Password = "12345Pl";
                user.Phone = "8(902) 345-66-89";

                db.Users.Update(user);
                db.SaveChanges();

                var result = db.Users.Last<User>();
                var expected = new User();
                expected.EMail = "aaaaaaa@gmail.com";
                expected.FIO = "Admin AAAA";
                expected.Orders = null;
                expected.Password = "12345Pl";
                expected.Phone = "8(902) 345-66-89";

                UserEquals(expected, result);
            }
        }

        [TestMethod]
        public void GetOrderById()
        {
            var options = TestInit();

            using (RMGContext db = new RMGContext(options))
            {
                var result = db.Orders.Find(2);
                var expected = new Order();
                expected.Priece = "500";
                expected.RealEstateId = 2;
                expected.Type = OrderType.Sale;
                expected.UserId = 1;
                expected.UserName = "Wesley Snipes";

                OrderEquals(expected, result);

            }
        }

        [TestMethod]
        public void SetNewOrder()
        {
            var options = TestInit();

            using (RMGContext db = new RMGContext(options))
            {
                var newOrder = new Order();
                newOrder.Priece = "1500";
                newOrder.RealEstateId = 2;
                newOrder.Type = OrderType.Purchase;
                newOrder.UserId = 1;
                newOrder.UserName = "ABC ABC";

                db.Orders.Add(newOrder);
                db.SaveChanges();

                var result = db.Orders.Last<Order>();

                var expected = new Order();
                expected.Priece = "1500";
                expected.RealEstateId = 2;
                expected.Type = OrderType.Purchase;
                expected.UserId = 1;
                expected.UserName = "ABC ABC";

                OrderEquals(expected, result);
            }
        }

        [TestMethod]
        public void EditOrder()
        {
            var options = TestInit();

            using (RMGContext db = new RMGContext(options))
            {
                var order = db.Orders.Last<Order>();
                order.Priece = "444444444444";
                order.RealEstateId = 2;
                order.Type = OrderType.Purchase;
                order.UserId = 1;
                order.UserName = "ABC ABC";

                db.Orders.Update(order);
                db.SaveChanges();

                var result = db.Orders.Last<Order>();
                var expected = new Order();
                expected.Priece = "444444444444";
                expected.RealEstateId = 2;
                expected.Type = OrderType.Purchase;
                expected.UserId = 1;
                expected.UserName = "ABC ABC";

                OrderEquals(expected, result);
            }
        }

        [TestMethod]
        public void ControllerProfileRedirect()
        {
            var options = TestInit();

            using (RMGContext db = new RMGContext(options))
            {
                HomeController controller = new HomeController(db, null);
                ViewResult result = controller.Profile(5) as ViewResult;

                var user = new User();
                user.EMail = "admin@gmail.ru";
                user.FIO = "Admin AAAA";
                user.Orders = null;
                user.Password = "12345Pl";
                user.Phone = "8(902) 345-66-89";
              

                UserEquals(user, result.Model as User);
            }
        }

        

        [TestMethod]
        public void ControllerCreateOrderRedirect()
        {
            var options = TestInit();

            using (RMGContext db = new RMGContext(options))
            {
                HomeController controller = new HomeController(db, null);
                ViewResult result = controller.CreateOrder() as ViewResult;
                Assert.IsNotNull(result);
              
            }
        }


        [TestMethod]
        public void ControllerAuthorizationRedirect()
        {
            var options = TestInit();

            using (RMGContext db = new RMGContext(options))
            {
                HomeController controller = new HomeController(db, null);
                ViewResult result = controller.Authorization() as ViewResult;
                Assert.IsNotNull(result);
                
            }
        }
    }
}
