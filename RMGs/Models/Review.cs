using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMGs.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public String Comment { get; set; }
        public Boolean Mark { get; set; }
        public String NameUser { get; set; }
        public int UserId { get; set; }
    }
}
