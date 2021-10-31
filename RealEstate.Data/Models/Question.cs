using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class Question
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Question1 { get; set; }
        public DateTime Date { get; set; }
        public string Region { get; set; }
        public int? CustomerType { get; set; }
        public int? EmployeeId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
