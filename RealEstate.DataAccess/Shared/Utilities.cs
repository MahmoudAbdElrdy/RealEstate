using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RealEstate.DataAccess
{
    public static class Utilities
    {
        /// <summary>
        /// Change date value based on timezone
        /// </summary>
        public static DateTime ChangeDateTimezone(DateTime date, string timezone)
        {
            /* Ex: 
             * timezone =  (hh:mm) 
             *             +03:30
             *             +01:00
             *              00:00
             *             -01:00 
             *             -03:30
            */

            // if timezone is plus or minus
            if (timezone.Contains("+") || timezone.Contains("-"))
            {
                int hrs = int.Parse(timezone.Substring(1, 2));  // take hours
                int mins = int.Parse(timezone.Substring(4, 2));  // take minutes

                if (timezone.Substring(0, 1) == "+")
                {
                    date = date.AddHours(hrs).AddMinutes(mins);
                }
                else if (timezone.Substring(0, 1) == "-")
                {
                    date = date.AddHours(-hrs).AddMinutes(-mins);
                }
            }

            return date;
        }

        /// <summary>
        /// Change date value based on timezone, used
        /// </summary>
        public static DateTime ChangeDateTimezoneToUTC(DateTime date, string timezone)
        {
            /* Ex: 
             * timezone =  (hh:mm) 
             *             +03:30
             *             +01:00
             *              00:00
             *             -01:00 
             *             -03:30
            */

            // if timezone is plus or minus
            if (timezone.Contains("+") || timezone.Contains("-"))
            {
                int hrs = int.Parse(timezone.Substring(1, 2));  // take hours
                int mins = int.Parse(timezone.Substring(4, 2));  // take minutes

                if (timezone.Substring(0, 1) == "+")
                {
                    date = date.AddHours(-hrs).AddMinutes(-mins);
                }
                else if (timezone.Substring(0, 1) == "-")
                {
                    date = date.AddHours(+hrs).AddMinutes(+mins);
                }
            }

            return date;
        }

        public static string FixToken(string token)
        {
            // http url replaces '+' with space, so here we bring back the '+'
            token = token.Replace(" ", "+");
            token = token.Replace(" ", "%20");
            return token;
        }
        public static string GenerateId(string parent, int max, int digit = 2)
        {
            string maxStr = "";
            int newLastDigit = 0;
            string lastDigitsStr = "";
            string newId = "";
            string prefix = "";

            //for (int i = 1; i < digit - 1; i++)
            //{
            //    prefix += "0";
            //}

            maxStr = max.ToString();

            if (maxStr.Length % digit != 0)
                maxStr = "0" + maxStr;

            lastDigitsStr = maxStr.Substring(maxStr.Length - digit, digit);
            newLastDigit = Convert.ToInt32(lastDigitsStr) + 1;

            if (newLastDigit.ToString().Length < digit)
            {
                newId = parent + "0" + (newLastDigit).ToString();
            }
            else
            {
                newId = parent + newLastDigit;
            }

            return newId;
        }
        public static string GenerateIdInt64(string parent, Int64 max, int digit = 2) 
        {
            string maxStr = "";
            Int64 newLastDigit = 0;
            string lastDigitsStr = "";
            string newId = "";
            string prefix = "";

            //for (int i = 1; i < digit - 1; i++)
            //{
            //    prefix += "0";
            //}

            maxStr = max.ToString();

            if (maxStr.Length % digit != 0)
                maxStr = "0" + maxStr;

            lastDigitsStr = maxStr.Substring(maxStr.Length - digit, digit);
            newLastDigit = Convert.ToInt64(lastDigitsStr) + 1;

            if (newLastDigit.ToString().Length < digit)
            {
                newId = parent + "0" + (newLastDigit).ToString();
            }
            else
            {
                newId = parent + newLastDigit;
            }

            return newId;
        }
    }
}
