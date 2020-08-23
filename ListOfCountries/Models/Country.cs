using System;
using System.Collections.Generic;
using System.Linq;

namespace ListOfCountries.Models
{
    public class Country
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Region { get; set; }
        public string Subregion { get; set; }
        public string CapitalCity { get; set; }
        public int Population { get; set; }
        public IEnumerable<string> Currencies { get; set; }
        public IEnumerable<string> Languages { get; set; }
        public IEnumerable<string> BorderingCountries { get; set; }

    }
}
