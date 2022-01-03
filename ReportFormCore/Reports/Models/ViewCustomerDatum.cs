using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class ViewCustomerDatum
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string NationalNumber { get; set; }
        public string Phone { get; set; }
        public string Date { get; set; }
        public string Program { get; set; }
        public string Address { get; set; }
        public bool IsStock { get; set; }
        public string Stock { get; set; }
        public double TotalCost { get; set; }
        public double? MeterCost { get; set; }
        public int? ProjectUnitId { get; set; }
        public int ProjectId { get; set; }
        public double? StockCount { get; set; }
        public double? MetersCount { get; set; }
        public string Notes { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAddress { get; set; }
        public int? Floors { get; set; }
        public int? ApartmentNumber { get; set; }
    }
}
