using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMGs.Models
{
    public class User
    {
        public int Id { get; set; }//user's identity
        public String FIO { get; set; }//user's FIO
        public String Phone { get; set; }//user's phone 
        public String EMail { get; set; }//user's e-mail
        public String Password { get; set; }//user's password

        public List<Order> Orders { get; set; }//user's orders
    }
}
