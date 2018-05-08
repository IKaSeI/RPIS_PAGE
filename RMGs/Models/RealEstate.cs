using Microsoft.EntityFrameworkCore;
using RMGs.Stereotypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RMGs.Models
{
    public enum TypeEstate { LandPlot, Accommodation, Recreational, Institutional }
    public class RealEstate
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }
        public String Photo { get; set; }

        public Property Property { get; set; }
        public Location Location { get; set; }
        public TypeEstate Type { get; set; }

    }
}
