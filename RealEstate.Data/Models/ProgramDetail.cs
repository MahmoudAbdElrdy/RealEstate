using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class ProgramDetail
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }

        public virtual Program Program { get; set; }
    }
}
