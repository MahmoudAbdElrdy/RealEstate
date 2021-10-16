using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class MaterialsPurchase
    {
        public MaterialsPurchase()
        {
            MaterialsAllocations = new HashSet<MaterialsAllocation>();
        }

        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public int ItemId { get; set; }
        public double Quantity { get; set; }
        public double Available { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
        public int SupplierId { get; set; }

        public virtual GoodsType Item { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<MaterialsAllocation> MaterialsAllocations { get; set; }
    }
}
