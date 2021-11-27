using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.DataAccess
{
    public class ExtraContrcatDto
    {
        public int? ProjectID { get; set; }
        public string  ContractExtraName { get; set; }
    } 
    public class CustomerCardDto 
    {
        public int? ContractID { get; set; } 
        public bool? IsExtra { get; set; }
    }
}
