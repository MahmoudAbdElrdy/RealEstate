namespace ReportRdlc
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ViewPayInstallment
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ContractDetailId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "date")]
        public DateTime ContractDetailDate { get; set; }

        [Key]
        [Column(Order = 3)]
        public double Amount { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ContractID { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool IsExtra { get; set; }

        public int? ContractDetailBillId { get; set; }

        public int? Number { get; set; }

        public double? Paid { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ContractDetailBillDate { get; set; }

        public double? Remainder { get; set; }
    }
}
