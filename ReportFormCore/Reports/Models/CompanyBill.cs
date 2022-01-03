using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class CompanyBill
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
    }
}
