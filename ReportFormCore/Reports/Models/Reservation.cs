using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class Reservation
    {
        public int Id { get; set; }
        public int ProjectUnitDescriptionId { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ProjectUnitDescription ProjectUnitDescription { get; set; }
    }
}
