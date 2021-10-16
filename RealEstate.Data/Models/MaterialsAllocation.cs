using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class MaterialsAllocation
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public double Quantity { get; set; }
        public double Cost { get; set; }
        public DateTime Date { get; set; }
        public int ProjectId { get; set; }
        public string Notes { get; set; }

        public virtual Project Project { get; set; }
        public virtual MaterialsPurchase Purchase { get; set; }
    }
}
