using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class ContractDetail
    {
        public ContractDetail()
        {
            ContractDetailBills = new HashSet<ContractDetailBill>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public int ContractId { get; set; }
        public bool IsExtra { get; set; }

        public virtual Contract Contract { get; set; }
        public virtual ICollection<ContractDetailBill> ContractDetailBills { get; set; }
    }
}
