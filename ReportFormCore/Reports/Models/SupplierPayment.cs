using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class SupplierPayment
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int SupplierId { get; set; }
        public DateTime Date { get; set; }
        public double Paid { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
