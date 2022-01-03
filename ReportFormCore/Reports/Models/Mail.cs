using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class Mail
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public bool ArchiveTo { get; set; }
        public bool ArchiveFrom { get; set; }
    }
}
