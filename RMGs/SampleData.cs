using RMGs.Models;
using RMGs.Stereotypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMGs
{
    public class SampleData
    {
        public static void Initialize(RMGContext context)
        {
            if (!context.Users.Any())
            {

                User u = new User
                {
                    EMail = "helloYesThisIsDog@gmail.com",
                    FIO = "Wesley Snipes",
                    Password = "edrenBaton777",
                    Phone = "88005553535",
                    Orders = new List<Order>
                    {
                    }
                };

                context.Users.Add(u);
                context.SaveChanges();

            //    Location l1 = new Location
            //    {
            //        City = "City",
            //        Country = "Country",
            //        ApartmentsNumber = "12",
            //        HouseBlockNumber = "32s",
            //        Street = "street",
            //        HouseNumber = "34"
            //    };

            //    Property p1 = new Property
            //    {
            //        AmountOfFloors = "2",
            //        AmountOfRooms = "7",
            //        Area = "45",
            //        Height = "25"
            //    };

            //    RealEstate e1 = new RealEstate
            //    {
            //        Name = "House",
            //        Location = l1,
            //        Property = p1,
            //        Type = TypeEstate.Accommodation,
            //        Description = "Easy description for real estate",
            //        Photo = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }
            //};

            //    context.RealEstates.Add(e1);
            //    context.SaveChanges();

            //    Order o1 = new Order
            //    {
            //        UserId = u.Id,
            //        RealEstateId = e1.Id,
            //        Priece = "9999",
            //        UserName = u.FIO,
            //        //RealEstate = e1,
            //        Type = OrderType.Sale
            //    };

            //    context.Orders.Add(o1);
            //    context.SaveChanges();
       
            //    //------------------------------------------------------------------------------------------

            //    Location l2 = new Location
            //    {
            //        City = "City1",
            //        Country = "Country1",
            //        Street = "street1",
            //        HouseNumber = "85",
            //        ApartmentsNumber="",
            //        HouseBlockNumber = ""
            //    };

            //    Property p2 = new Property
            //    {
            //        AmountOfFloors = "12",
            //        AmountOfRooms = "17",
            //        Area = "145",
            //        Height = ""
            //    };


            //    RealEstate e2 = new RealEstate
            //    {
            //        Name = "The Mordor Earth",
            //        Location = l2,
            //        Property = p2,
            //        Type = TypeEstate.LandPlot,
            //        Description = "Fucking description for this real estates",
            //        Photo = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }
            //};

            //    context.RealEstates.Add(e2);
            //    context.SaveChanges();

            //    Order o2 = new Order
            //                {
            //                    UserId = u.Id,
            //                    RealEstateId = e2.Id,
            //        //RealEstate = e2,
            //        Priece = "20000000",
            //        UserName = u.FIO,
            //        Type = OrderType.Sale
            //    };

            //    context.Orders.Add(o2);
            //    context.SaveChanges();
            }
        }
    }
}
