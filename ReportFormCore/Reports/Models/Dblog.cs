using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class Dblog
    {
        public int Id { get; set; }
        public string Operation { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
    }
}
