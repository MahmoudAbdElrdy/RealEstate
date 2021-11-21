using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class ViewSupervisor
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Job { get; set; }
        public int Id { get; set; }
        public double? Debt { get; set; }
        public double? Credit { get; set; }
        public double? Net { get; set; }
    }
}
