using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class Customer
    {
        public Customer()
        {
            ProjectVisits = new HashSet<ProjectVisit>();
            Questions = new HashSet<Question>();
            Reservations = new HashSet<Reservation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Referrer { get; set; }
        public string Phone { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<ProjectVisit> ProjectVisits { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
