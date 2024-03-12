using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Essentials;

namespace Church.Mobile.Helpers
{
    public static class XPlatform
    {
        public static bool IsThereInternet => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public static bool IsPasswordValid(this string Password)
        {

            if (String.IsNullOrEmpty(Password))
            {
                return false;
            }
            Match match = Regex.Match(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$");
            if (match.Success)
            {
                return true;
            }
            return false;
        }

        public static bool IsEmailValid(string Email)
        {
            return Regex.IsMatch(Email, @"^.+@[^\.].*\.[a-z]{2,}$");
        }

        public static string FormatEmail(string Email)
        {
            return Email.Replace(" ", "");
        }
        public static bool IsQtyValid(this string Qty)
        {
            return Qty.ToCharArray().All(x => char.IsDigit(x)); //Make sure all characters are numbers;
        }
    }
}
