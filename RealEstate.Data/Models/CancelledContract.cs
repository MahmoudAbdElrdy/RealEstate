using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class CancelledContract
    {
        public CancelledContract()
        {
            CancelledContractBills = new HashSet<CancelledContractBill>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Project { get; set; }
        public string Customer { get; set; }
        public double Paid { get; set; }
        public double Back { get; set; }

        public virtual ICollection<CancelledContractBill> CancelledContractBills { get; set; }
    }
}
