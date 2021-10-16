﻿using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class Question
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Question1 { get; set; }
        public DateTime Date { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
