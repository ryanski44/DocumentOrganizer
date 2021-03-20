using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentOrganizerUI.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpaceOrGibberish(this string value)
        {
            if (value == null) return true;

            for (int i = 0; i < value.Length; i++)
            {
                if (!Char.IsWhiteSpace(value[i]) && value[i] != 0xFFFD) return false;
            }

            return true;
        }
    }
}
