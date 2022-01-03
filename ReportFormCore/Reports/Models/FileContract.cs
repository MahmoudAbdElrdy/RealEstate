using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class FileContract
    {
        public int? ContractId { get; set; }
        public int Id { get; set; }
        public string FilePath { get; set; }

        public virtual Contract Contract { get; set; }
    }
}
