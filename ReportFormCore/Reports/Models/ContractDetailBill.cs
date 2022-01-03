using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class ContractDetailBill
    {
        public int Id { get; set; }
        public int ContractDetailId { get; set; }
        public int Number { get; set; }
        public double Paid { get; set; }
        public DateTime Date { get; set; }

        public virtual ContractDetail ContractDetail { get; set; }
    }
}
