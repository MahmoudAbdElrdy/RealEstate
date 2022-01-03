using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class ViewCancelledContract
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Project { get; set; }
        public string Customer { get; set; }
        public double Paid { get; set; }
        public double Back { get; set; }
    }
}
