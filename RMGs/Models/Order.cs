using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RMGs.Models
{
    public enum OrderType { Sale, Purchase, Lease, Rent, Swap }
    public class Order
    {
        public int Id { get; set; }//number of order

        public String Priece { get; set; }

        public OrderType Type { get; set; }

        public int UserId { get; set; }//link to user

        public String UserName { get; set; }

        public int RealEstateId { get; set; }//link to real estate
        public RealEstate RealEstate { get; set; }//

        public List<Review> Comments { get; set; }
    }
}
