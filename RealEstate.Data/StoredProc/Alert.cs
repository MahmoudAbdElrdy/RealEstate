using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Data.StoredProc
{
    public class Alert
    {
        public string Name { get; set; }
        public string CustomerName { get; set; } 
        public string ProjectName { get; set; }  
        public string CustomerPhone { get; set; }   
        public string DateSting { get; set; } 
        public string Stock { get; set; } 
        public DateTime? Date { get; set; }
        public int? ContractID { get; set; }
        public int? ContractDetailBillId { get; set; } 
        public int? ContractDetailId { get; set; }  
        public int? ProjectUnitID { get; set; }
        public int? ProjectID { get; set; } 
        public double Paid { get; set; }
        public double? Amount { get; set; }
        public double Remainder { get; set; }
        public int FloorNumber { get; set; }
        public int Number { get; set; }
        public string Details { get; set; } 
    }
    public class PrintBill
    {
        public string Name { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public string CustomerPhone { get; set; }
        public string NationalNumber { get; set; } 
        public string DateSting { get; set; }
        public string Stock { get; set; }
        public DateTime? Date { get; set; }
        public int? ContractID { get; set; }
        public int? ContractDetailBillId { get; set; }
        public int? ContractDetailId { get; set; }
        public int? ProjectID { get; set; }
        public decimal Paid { get; set; }
        public double Amount { get; set; }
        public decimal Remainder { get; set; }
     
    }
}
