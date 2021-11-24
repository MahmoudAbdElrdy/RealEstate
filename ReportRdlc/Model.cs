using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ReportRdlc
{
    public partial class Model : DbContext
    {
        public Model()
            : base("name=RealEstateNew")
        {
        }

        public virtual DbSet<ViewPayInstallment> ViewPayInstallments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
