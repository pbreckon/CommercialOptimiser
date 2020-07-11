using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialOptimiser.App
{
    public static class BlazorAppContext
    {
        /// <summary>
        /// The IP for the current session
        /// </summary>
        public static string UserIpAddress { get; set; }
    }
}
