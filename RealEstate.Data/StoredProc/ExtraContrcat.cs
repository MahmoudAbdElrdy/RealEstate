using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Data.StoredProc
{
   public class ExtraContrcat
    {
        public int? ContractId { get; set; }
        public string ContractName { get; set; }
        public string NationalNumber { get; set; }
        public int? ProjectId { get; set; }
        public int? ContractDetailBillId { get; set; }
        public int? ContractDetailId { get; set; }
        public int? Number { get; set; }
        public double? Paid { get; set; }
        public DateTime? ContractDetailBillDate { get; set; }
        public int? Id { get; set; }
        public string ContractDetailName { get; set; }
        public DateTime? ContractDetailDate { get; set; }
        public double? ContractDetailAmount { get; set; }
        public bool? IsExtra { get; set; }
        public string ProjectName { get; set; }
    }
}
