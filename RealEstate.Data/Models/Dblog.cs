using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class Dblog
    {
        public int Id { get; set; }
        public string Operation { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
    }
}
