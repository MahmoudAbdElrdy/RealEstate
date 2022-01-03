using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class CancelledContractBill
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public DateTime Date { get; set; }
        public double Paid { get; set; }

        public virtual CancelledContract Contract { get; set; }
    }
}
