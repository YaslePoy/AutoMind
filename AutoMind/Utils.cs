using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    public static class Utils
    {
        public static bool IsNumeric(this string str) => 
            double.TryParse(str, CultureInfo.InvariantCulture, out double v);
        
    }
}
