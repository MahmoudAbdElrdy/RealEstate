using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            MaterialsPurchases = new HashSet<MaterialsPurchase>();
            SupplierPayments = new HashSet<SupplierPayment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string NationalNumber { get; set; }
        public string Phones { get; set; }
        public string Address { get; set; }
        public string Speciality { get; set; }

        public virtual ICollection<MaterialsPurchase> MaterialsPurchases { get; set; }
        public virtual ICollection<SupplierPayment> SupplierPayments { get; set; }
    }
}
