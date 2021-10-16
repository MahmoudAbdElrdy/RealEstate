using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class ProjectExpense
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ItemId { get; set; }
        public double Cost { get; set; }
        public int ProjectId { get; set; }
        public int Number { get; set; }
        public string Notes { get; set; }
        public int? SiteRepId { get; set; }

        public virtual GoodsType Item { get; set; }
        public virtual Project Project { get; set; }
        public virtual SiteRep SiteRep { get; set; }
    }
}
