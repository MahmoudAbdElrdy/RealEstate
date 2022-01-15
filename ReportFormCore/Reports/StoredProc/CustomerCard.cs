

using System;

namespace ReportFormCore.Data.StoredProc 
{
    public  class CustomerCard 
    {
        public int ContractDetailId { get; set; }
        public string Name { get; set; }
        public string ContractDetailDate { get; set; }
        public double Amount { get; set; }
        public int ContractId { get; set; }
        public bool IsExtra { get; set; }
        public int? ContractDetailBillId { get; set; }
        public int? Number { get; set; }
        public double? Paid { get; set; }
        public double? AmountPaid { get; set; }  
        public string ContractDetailBillDate { get; set; }
        public double? Remainder { get; set; }
        public DateTime? Date { get; set; }
    }
}
