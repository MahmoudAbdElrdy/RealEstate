using System;
namespace CRM.Services
{
    public class BaseDto
    {
        public string CreatedById { get; set; }
       
        public DateTime CreatedOn { get; set; }
        public bool Stopped { get; set; }
    }

    public class BaseSimple
    {
        public string CreatedById { get; set; }
       
        public DateTime CreatedOn { get; set; }
    }

    public class BaseFullDto : BaseDto
    {
        public string ModifiedById { get; set; }
    
        public DateTime ModifiedOn { get; set; }
    }
}
