using System;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace RealEstate.Api
{
    public static class Settings
    {
        private const string SECRET_KEY = "$m@rt@Ma7m0ud#Key!Gm2512&Aug";
        public static readonly SymmetricSecurityKey SIGNING_KEY = new
        SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

        public static int Id { get; set; }
        public static string Department { get; set; } 
        public static string Name { get; set; }  
      
        public static string GetMonthName(byte Month)
        {
            var monthName = "";
            switch (Month)
            {
                case 1:
                    monthName = "يناير";
                    break;
                case 2:
                    monthName = "فبراير";
                    break;
                case 3:
                    monthName = "مارس";
                    break;
                case 4:
                    monthName = "أبريل";
                    break;
                case 5:
                    monthName = "مايو";
                    break;
                case 6:
                    monthName = "يونيو";
                    break;
                case 7:
                    monthName = "يوليو";
                    break;
                case 8:
                    monthName = "أغسطس";
                    break;
                case 9:
                    monthName = "سبتمبر";
                    break;
                case 10:
                    monthName = "أكتوبر";
                    break;
                case 11:
                    monthName = "نوفمبر";
                    break;
                case 12:
                    monthName = "ديسمبر";
                    break;
            }
            return monthName;
        }
    }
}
