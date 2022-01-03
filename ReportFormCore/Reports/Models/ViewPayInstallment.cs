using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class ViewPayInstallment
    {
        public int ContractDetailId { get; set; }
        public string Name { get; set; }
        public DateTime ContractDetailDate { get; set; }
        public double Amount { get; set; }
        public int ContractId { get; set; }
        public bool IsExtra { get; set; }
        public int? ContractDetailBillId { get; set; }
        public int? Number { get; set; }
        public double? Paid { get; set; }
        public DateTime? ContractDetailBillDate { get; set; }
        public decimal Remainder { get; set; }
    }
}
