using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class Contract
    {
        public Contract()
        {
            ContractDetails = new HashSet<ContractDetail>();
            FileContracts = new HashSet<FileContract>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string NationalNumber { get; set; }
        public string Phone { get; set; }
        public DateTime Date { get; set; }
        public string Program { get; set; }
        public string Address { get; set; }
        public bool IsStock { get; set; }
        public double TotalCost { get; set; }
        public double? MeterCost { get; set; }
        public int? ProjectUnitId { get; set; }
        public int ProjectId { get; set; }
        public double? StockCount { get; set; }
        public double? MetersCount { get; set; }
        public string Notes { get; set; }
        public double? MetersNumer { get; set; }

        public virtual ProjectUnit ProjectUnit { get; set; }
        public virtual ICollection<ContractDetail> ContractDetails { get; set; }
        public virtual ICollection<FileContract> FileContracts { get; set; }
    }
}
